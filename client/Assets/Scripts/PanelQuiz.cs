using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PanelQuiz : MonoBehaviour {
	//multiple choice, one right answer

	public GameObject answerBtn;
	public Text textQuestion;
	public Text textQuestionNr;

	public int questionNr;
	public int correctAnswers;

	public List<GameObject> answerBtns;
	public List<Question> questions;




	void Start () {
		init ();
	}
	

	void Update () {
	
	}




	public void init(){
		//set question number to 0
		questionNr = 0;
		//set correct answers to 0
		correctAnswers = 0;
		//delete buttons by iterating button array
		foreach(GameObject b in answerBtns){
			Destroy(b);
		}
		answerBtns.Clear ();
		answerBtns = new List<GameObject> ();
		//TODO fetch questions and stuff into data object and shuffle order
		questions.Add (new Question("Frage 1", new List<string>{"Antwort 1", "Antwort 2", "Antwort 3"}, new List<string>{"correct", "wrong", "correct"}));

		showNextQuestion ();
	}

	public void showNextQuestion(){
		GameObject generatedBtn;
	
		//show question nr and question text
		textQuestionNr.GetComponent<Text> ().text = "Frage " + (questionNr+1) + ":";
		textQuestion.GetComponent<Text> ().text = questions [questionNr].getQuestionText ();
		//generate buttons for answer options
		for (int i = 0; i < questions [questionNr].getAnswersAmount(); i++) {
		
			generatedBtn = Instantiate (answerBtn, Vector3.zero, Quaternion.identity) as GameObject;
			generatedBtn.transform.parent = GameObject.Find ("contentQuestion").transform;
			generatedBtn.transform.FindChild("Text").GetComponent<Text>().text = questions [questionNr].getAnswer(i);

			//set method to be called at onclick event
			//add onclick action for buttons: method: clickedBtn, param: "wrong"||"correct"
			string temp = questions[questionNr].getAnswerValues(i);
			Debug.Log (temp);
			generatedBtn.GetComponent<Button>().onClick.AddListener(() => {clickedBtn(temp);});
		}
		questionNr ++;
	}

	public void clickedBtn(string answer){
		switch (answer) {
		case "wrong":	//change color to red
			break;
		case "correct": //change color to green
						correctAnswers ++;
			break;
		}
		showNextQuestion ();
		//TODO StartCoroutine (next);
	}

	private IEnumerator next() {
		yield return new WaitForSeconds (2);
		showNextQuestion ();
	}

}
