using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Assignment questions represent the task of assigning pairs of matching strings together.
/// This class provides functionality to manage data for such tasks and to perform answer checking.
/// </summary>
public class AssignmentQuestion : TaskQuestion{

	/// <summary>
	/// A list of strings, representing the 2 fields of the assignments.
	/// </summary>
	private List<string> fields;

	/// <summary>
	/// Initializes a new instance of the <see cref="AssignmentQuestion"/> class.
	/// </summary>
	/// 
	/// <param name="alt1">first assignment string.</param>
	/// <param name="alt2">second assignment string.</param>
	public AssignmentQuestion(string alt1, string alt2) : base(""){
		fields = new List<string> ();
		fields.Add (alt1);
		fields.Add (alt2);
	}

	/// <summary>
	/// Checks the parameter answer against the internal answer.
	/// Checks whether all the supplied answers can be found in the field list of this question object.
	/// </summary>
	/// 
	/// <returns>1 for successful check (correct answer), 0 otherwise.</returns>
	/// 
	/// <param name="answer">the answer question object to be checked.</param>
	override public double checkAnswer(TaskQuestion answer){
		AssignmentQuestion ans = (AssignmentQuestion)answer;
		List<string> aw = (List<string>)ans.getAnswer();
		foreach (string s in fields) {
			if(!aw.Contains(s)){
				return 0;
			}
		}

		return 1;
	}

	/// <returns>The string list of fields.</returns>
	override public object getAnswer(){
		return fields;
	}

	/// <summary>
	/// Gets the CSV representation of this TaskQuestion. Implementation is left for extention classes.
	/// This method is important for .csv string message exchange with the server.
	/// .csv representation of an assignment question are the two assignment strings: "fields[0],fields[1]".
	/// </summary>
	/// 
	/// <returns>The CSV representation.</returns>
	override public string getCSVRepresentation(){
		return CSVHelper.swapEncode(fields[0]) + "," + CSVHelper.swapEncode(fields[1]);
	}

	/// <summary>
	/// Shuffling is not implemented for assignment questions. Does nothing.
	/// </summary>
	override public void shuffleAnswers(){
		return;
	}
}