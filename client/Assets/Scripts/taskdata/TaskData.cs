using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskData{
	protected List<TaskQuestion> taskQuestions;

	public TaskData(string csv){
		taskQuestions = constructFromCSV(csv);
	}

	public List<TaskQuestion> getQuestions(){
		return taskQuestions;
	}

	public void addQuestion(TaskQuestion t){
		taskQuestions.Add(t);
	}

	public TaskQuestion getQuestion(int index){
		return taskQuestions[index];
	}

	public string getCSV(){
		string csv = "";
		
		foreach (TaskQuestion q in taskQuestions) {
			string c = q.getCSVRepresentation();
			csv += taskQuestions.IndexOf(q) + "," + c + "\n";
		}
		
		return csv;
	}

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

	/* abstract methods */
	public abstract TaskQuestion constructTaskQuestion (string csvLine);
}