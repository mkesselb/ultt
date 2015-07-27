using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class which represents assignment tasks. It provides methods to gather and manage individual task questions of assignemnt tasks.
/// </summary>
public class AssignmentData : TaskData{

	/// <summary>
	/// Initializes a new instance of the <see cref="AssignmentData"/> class.
	/// The parameter .csv string is parsed to construct the task questions.
	/// </summary>
	/// 
	/// <param name="csv">csv string of an assignment task.</param>
	public AssignmentData (string csv) : base(csv){
		//nothing else to do here?!
	}

	/// <summary>
	/// Constructs the task question from parameter .csv line.
	/// See the AssignmentQuestion.getCSVRepresentation() for the csv structure.
	/// </summary>
	/// 
	/// <returns>The task question object of an assignment.</returns>
	/// 
	/// <param name="csvLine">the .csv string line.</param>
	override public TaskQuestion constructTaskQuestion(string csvLine){
		string[] p = csvLine.Split (new char[]{','});
		return new AssignmentQuestion(CSVHelper.swapDecode(p[1]),
		                              CSVHelper.swapDecode(p[2]));
	}

	/// <summary>
	/// Aggregates a list of AssignmentQuestions that match the parameter field string on the parameter index.
	/// </summary>
	/// 
	/// <returns>The aggregated list of AssignmentQuestions.</returns>
	/// 
	/// <param name="field">Field string to be matched.</param>
	/// <param name="index">Number of field to be checked (0 / 1).</param>
	public List<AssignmentQuestion> getQuestionWithField(string field, int index){
		//index parameter is the index of the field, that is either 0 (first) or 1 (second)
		//list contains all questions which have the field in the index
		List<AssignmentQuestion> aq = new List<AssignmentQuestion> ();

		foreach(AssignmentQuestion a in taskQuestions){
			List<string> ans = (List<string>)a.getAnswer();
			if(ans[index].Equals(field)){
				aq.Add (a);
			}
		}

		//list should not be empty!
		return aq;
	}

	/// <returns>The number of assignment questions.</returns>
	override public int getFullPoints(){
		return this.taskQuestions.Count;
	}
}