using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Question : MonoBehaviour {

	private string questiontext;
	private List<string> answers;
	private List<string> answerValues; //wrong || correct

	public Question(string questiontext, List<string> answers, List<string> answerValues){
		this.questiontext = questiontext;
		this.answers = answers;
		this.answerValues = answerValues;
	}

	public string getQuestionText(){
		return questiontext;
	}

	public int getAnswersAmount(){
		return answers.Count;
	}

	public string getAnswer(int index){
		return answers[index];
	}

	public string getAnswerValues(int index){
		return answerValues[index];
	}
}
