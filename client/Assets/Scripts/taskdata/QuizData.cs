using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class which represents quiz tasks. It provides methods to gather and manage individual task questions of quiz tasks.
/// </summary>
public class QuizData : TaskData{

	/// <summary>
	/// Initializes a new instance of the <see cref="QuizData"/> class.
	/// The parameter .csv string is parsed to construct the task questions.
	/// </summary>
	/// 
	/// <param name="csv">csv string of an quiz task.</param>
	public QuizData(string csv) : base(csv){
		//nothing else
	}

	/// <summary>
	/// Constructs the task question from parameter .csv line.
	/// See the QuizQuestion.getCSVRepresentation() for the csv structure.
	/// </summary>
	/// 
	/// <returns>The task question object of a quiz.</returns>
	/// 
	/// <param name="csvLine">the .csv string line.</param>
	override public TaskQuestion constructTaskQuestion(string csvLine){
		string[] p = csvLine.Split (new char[]{','});
		List<string> answers = new List<string>();
		List<int> weights = new List<int>();
		for(int i = 2; i < p.Length; i = i+2){
			answers.Add(CSVHelper.swapDecode(p[i]));
			weights.Add(int.Parse(CSVHelper.swapDecode(p[i+1])));
		}
		return new QuizQuestion(CSVHelper.swapDecode(p[1]), answers, weights);
	}

	/// <summary>
	/// Adds the parameter quiz question to the task.
	/// </summary>
	public void addQuizQuestion(QuizQuestion q){
		this.taskQuestions.Add (q);
	}
	
	/// <returns>The full achievable points, which is the number of answers of all quiz questions.</returns>
	override public int getFullPoints(){
		//TODO: decide on full points == num questions or num answers?
		int points = 0;

		foreach (TaskQuestion q in this.taskQuestions) {
			points += ((QuizQuestion)q).getNumAnswers();
		}

		return points;
	}
}