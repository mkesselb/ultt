using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract base class for managing data (questions) of tasks. Should be extended by all concrete task forms.
/// </summary>
public abstract class TaskData{

	/// <summary>
	/// List of TaskQuestion objects, gathering all the individual questions of this task.
	/// </summary>
	protected List<TaskQuestion> taskQuestions;

	/// <summary>
	/// Initializes a new instance of the <see cref="TaskData"/> class.
	/// Builds the list of task questions from parameter .csv string.
	/// For initialization of an empty TaskData, use empty string as parameter.
	/// </summary>
	/// 
	/// <param name="csv">.csv string to be used to build the TaskQuestions.</param>
	public TaskData(string csv){
		taskQuestions = constructFromCSV(csv);
	}

	/// <returns>The questions.</returns>
	public List<TaskQuestion> getQuestions(){
		return taskQuestions;
	}

	/// <summary>
	/// Adds the parameter question.
	/// </summary>
	public void addQuestion(TaskQuestion t){
		taskQuestions.Add(t);
	}

	/// <returns>The question of paramter index.</returns>
	public TaskQuestion getQuestion(int index){
		return taskQuestions[index];
	}

	/// <summary>
	/// Builds the .csv representation of this task. Used for .csv message exchange to/from the server.
	/// </summary>
	/// 
	/// <returns>The .csv string.</returns>
	public string getCSV(){
		string csv = "";
		
		foreach (TaskQuestion q in taskQuestions) {
			string c = q.getCSVRepresentation();
			csv += taskQuestions.IndexOf(q) + "," + c + "\n";
		}
		
		return csv;
	}

	/// <summary>
	/// Constructs this task from the parameter .csv string. Used to build the object after .csv message exchange from the server.
	/// </summary>
	/// 
	/// <returns>the list of task questions extracted from the .csv string.</returns>
	/// 
	/// <param name="csv">the .csv string.</param>
	public List<TaskQuestion> constructFromCSV(string csv){
		List<TaskQuestion> taskQ = new List<TaskQuestion>();
		if (csv.Length == 0) {
			return taskQ;
		}
		string[] lines = csv.Split (new char[]{'\n'});
		
		foreach (string s in lines) {
			if(s.Length > 0){
				taskQ.Add(constructTaskQuestion(s));
			}
		}
		
		return taskQ;
	}

	/// <summary>
	/// Shuffles the questions of this task. Implementation is defined in TaskQuestions.
	/// </summary>
	public void shuffleQuestions(){
		Shuffle.shuffle (taskQuestions);
		foreach (TaskQuestion t in taskQuestions) {
			t.shuffleAnswers();
		}
	}

	/* abstract methods */

	/// <summary>
	/// Constructs the task question from parameter .csv line. Implementation is defined in concrete classes.
	/// </summary>
	/// 
	/// <returns>The task question.</returns>
	/// 
	/// <param name="csvLine">the .csv string line.</param>
	public abstract TaskQuestion constructTaskQuestion (string csvLine);

	/// <summary>
	/// Gets the full possible points. Implementation is defined in concrete classes.
	/// </summary>
	public abstract int getFullPoints();
}