using System;
using System.Collections;
using System.Collections.Generic;

public class QuizQuestion : TaskQuestion{
	private List<string> questionAnswers;
	private List<int> questionWeights;
	//the lists should be tied by indices.
	//the list of questionweights allows to give weights to correct answers.
	//giving the same weight for each answer (e.g. 1) can be used for normal questions, or for answers where selected ones get >0, non-selected get 0
	private int totalWeights = 0;

	public QuizQuestion(string questionText, List<string> questionAns, List<int> questionWeigh) : base(questionText){
		questionAnswers = questionAns;
		questionWeights = questionWeigh;
		foreach(int i in questionWeights){
			totalWeights += i;
		}
	}

	//the following outcomes are possible for each index:
	//0:	false answer, not selected
	//1:	right answer, selected
	//2:	false answer, selected
	//3:	right answer, not selected
	//parameter answer QuizQuestion shall have the same questions as this object, with the weights as 0 for non-selected and >0 for selected answers
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

	//parameter answer QuizQuestion shall have the same questions as this object, with the weights as 0 for non-selected and >0 for selected answers
	override public double checkAnswer(TaskQuestion answer){
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
	
	override public object getAnswer(){
		return new object[] {questionAnswers, questionWeights};
	}
	
	override public string getCSVRepresentation(){
		string csv = CSVHelper.swapEncode(this.getQuestionText());
		for(int i = 0; i < questionAnswers.Count; i++){
			csv += "," + CSVHelper.swapEncode(questionAnswers[i]) 
				+ "," + CSVHelper.swapEncode(questionWeights[i].ToString());
		}
		return csv;
	}

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