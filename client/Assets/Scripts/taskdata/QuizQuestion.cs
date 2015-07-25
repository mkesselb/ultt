using System;
using System.Collections;
using System.Collections.Generic;

public class QuizQuestion : TaskQuestion{

	/// <summary>
	/// String list of question answers.
	/// </summary>
	private List<string> questionAnswers;

	/* the lists should be tied by indices.
	 * the list of questionweights allows to give weights to correct answers.
	 * giving the same weight for each answer (e.g. 1) can be used for normal questions, or for answers where selected ones get >0, non-selected get 0
	 */

	/// <summary>
	/// Integer list of question weights.
	/// </summary>
	private List<int> questionWeights;

	/// <summary>
	/// Number of total weights of the answers. Used for score computation.
	/// </summary>
	private int totalWeights = 0;

	/// <summary>
	/// Initializes a new instance of the <see cref="QuizQuestion"/> class.
	/// Sets up the internal lists of answers and weights.
	/// </summary>
	/// 
	/// <param name="questionText">string question text.</param>
	/// <param name="questionAns">list of string question answers.</param>
	/// <param name="questionWeigh">list of integer question weights.</param>
	public QuizQuestion(string questionText, List<string> questionAns, List<int> questionWeigh) : base(questionText){
		questionAnswers = questionAns;
		questionWeights = questionWeigh;
		foreach(int i in questionWeights){
			totalWeights += i;
		}
	}

	/// <summary>
	/// For parameter user-supplied answer, checks the selections (weights) of the quiz answers and rates them to the true weights of the question.
	/// Returns a list of integers, corresponding to the list of answers.
	/// The list contains the following possible outcomes:
	/// 1: false answer, not selected.
	/// 2: right answer, selected.
	/// 3: false answer, selected.
	/// 4: right answer, not selected.
	/// </summary>
	/// 
	/// <returns>a int list which contains ratings for the supplied answers.</returns>
	/// 
	/// <param name="answer">the user-supplied answer to be checked.</param>
	public List<int> checkAnswerIndices(QuizQuestion answer){
		object[] aw_ = (object[])answer.getAnswer();
		List<string> answerQ = (List<string>)aw_[0];
		List<int> answerW = (List<int>)aw_[1];

		List<int> annotatedIndices = new List<int>();
		for(int i = 0; i < answerQ.Count; i++){
			String s = answerQ[i];
			int w = answerW[i];
			int j = questionAnswers.IndexOf(s);
			if(j >= 0){
				int wQ = questionWeights[j];
				if(wQ == 0 && w == 0){
					annotatedIndices.Add(0);
				}
				if(wQ > 0 && w > 0){
					annotatedIndices.Add(1);
				}
				if(wQ == 0 && w > 0){
					annotatedIndices.Add(2);
				}
				if(wQ > 0 && w == 0){
					annotatedIndices.Add(3);
				}
			} else{
				annotatedIndices.Add(-1);
			}
		}

		return annotatedIndices;
	}

	/// <summary>
	/// Checks the parameter answer against the internal answer.
	/// Is intended to be used with a user-supplied answer (parameter) against the internal correct answer (which is built
	/// from the saved task configuration).
	/// 
	/// Checks the parameter TaskQuestion answer weights against the internal answer weights.
	/// The score is based on the fraction of achieved / total weights.
	/// </summary>
	/// 
	/// <returns>The score of achieved / total weights [0,1].</returns>
	/// 
	/// <param name="answer">user-supplied answer to be checked.</param>
	override public double checkAnswer(TaskQuestion answer){
		//parameter answer QuizQuestion shall have the same questions as this object, with the weights as 0 for non-selected and >0 for selected answers
		QuizQuestion ans = (QuizQuestion)answer;
		object[] aw_ = (object[])ans.getAnswer();
		List<string> answerQ = (List<string>)aw_[0];
		List<int> answerW = (List<int>)aw_[1];

		double score = 0;
		for(int i = 0; i < answerQ.Count; i++){
			String s = answerQ[i];
			int w = answerW[i];
			int j = questionAnswers.IndexOf(s);
			if(j >= 0){
				int wQ = questionWeights[j];
				if(wQ > 0 && w > 0){
					//only rightly selected answers count for score
					score += wQ;
				}
			}
		}

		return score / totalWeights;
	}

	/// <returns>The number of answers.</returns>
	public int getNumAnswers(){
		return questionAnswers.Count;
	}

	/// <returns>An array of [questionAnswers, questionWeights].</returns>
	override public object getAnswer(){
		return new object[] {questionAnswers, questionWeights};
	}

	/// <summary>
	/// Gets the CSV representation of this TaskQuestion. Implementation is left for extention classes.
	/// This method is important for .csv string message exchange with the server.
	/// 
	/// The .csv representation of a qiz question is the question text, followed by the question answers and corresponding weights:
	/// qText,qAnswer1,qWeight1,...,qAnswerN,qWeightN
	/// </summary>
	override public string getCSVRepresentation(){
		string csv = CSVHelper.swapEncode(this.getQuestionText());
		for(int i = 0; i < questionAnswers.Count; i++){
			csv += "," + CSVHelper.swapEncode(questionAnswers[i]) 
				+ "," + CSVHelper.swapEncode(questionWeights[i].ToString());
		}
		return csv;
	}

	/// <summary>
	/// Provides a randomization of the quiz questions and answers.
	/// </summary>
	override public void shuffleAnswers(){
		List<int> indices = new List<int> ();
		for (int i = 0; i < questionAnswers.Count; i++) {
			indices.Add (i);
		}
		Shuffle.shuffle (indices);
		List<string> answers = new List<string> (questionAnswers);
		List<int> weigths = new List<int>(questionWeights);
		questionAnswers.Clear ();
		questionWeights.Clear ();
		foreach (int i in indices) {
			questionAnswers.Add (answers[i]);
			questionWeights.Add (weigths[i]);
		}
	}
}