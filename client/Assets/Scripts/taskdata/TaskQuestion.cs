using System;
using System.Collections;
using System.Collections.Generic;

public abstract class TaskQuestion{
	private string questionText;

	public TaskQuestion(string questionT){
		questionText = questionT;
	}

	public string getQuestionText(){
		return questionText;
	}

	/* abstract methods */
	public abstract double checkAnswer(TaskQuestion answer);
	public abstract object getAnswer();
	public abstract string getCSVRepresentation();
}