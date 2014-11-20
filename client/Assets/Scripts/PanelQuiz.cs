using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PanelQuiz : MonoBehaviour {
	//multiple choice, one right answer

	private Main main;

	public GameObject answerBtn;
	public Text textQuestion;
	public Text textQuestionNr;

	public int questionNr;
	public int correctAnswers;

	public List<GameObject> answerBtns;
	public List<Question> questions;

	public Color colorCorrect;
	public Color colorWrong;


	void Start () {
		main = GameObject.Find ("Scripts").GetComponent<Main>();
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
		questions.Add (new Question("Frage 1", new List<string>{"Antwort 1a", "Antwort 1b", "Antwort 1c"}, new List<string>{"correct", "wrong", "correct"}));
		questions.Add (new Question("Frage 2", new List<string>{"Antwort 2a", "Antwort 2b", "Antwort 2c"}, new List<string>{"correct", "wrong", "correct"}));
		questions.Add (new Question("Frage 3", new List<string>{"Antwort 3a", "Antwort 3b", "Antwort 3c"}, new List<string>{"correct", "wrong", "correct"}));

		showNextQuestion ();
	}

	public void showNextQuestion(){
		if (questionNr < questions.Count) {
						GameObject generatedBtn;
						//delete old answer buttons
						foreach (GameObject b in answerBtns) {
								Destroy (b);
						}
						answerBtns.Clear ();
	
						//show question nr and question text
						textQuestionNr.GetComponent<Text> ().text = "Frage " + (questionNr + 1) + ":";
						textQuestion.GetComponent<Text> ().text = questions [questionNr].getQuestionText ();
						//generate buttons for answer options
						for (int i = 0; i < questions [questionNr].getAnswersAmount(); i++) {
							
								generatedBtn = Instantiate (answerBtn, Vector3.zero, Quaternion.identity) as GameObject;
								generatedBtn.transform.parent = GameObject.Find ("contentQuestion").transform;
								generatedBtn.transform.FindChild ("Text").GetComponent<Text> ().text = questions [questionNr].getAnswer (i);

								//set method to be called at onclick event
								//add onclick action for buttons: method: clickedBtn, param: "wrong"||"correct"
								string temp = questions [questionNr].getAnswerValues (i);
								int answerNr = i;
								generatedBtn.GetComponent<Button> ().onClick.AddListener (() => {clickedBtn (answerNr,temp);});
								answerBtns.Add (generatedBtn);
						}
						questionNr ++;
				} else {
						textQuestionNr.GetComponent<Text> ().text = "";
						textQuestion.GetComponent<Text> ().text = "";
						//delete buttons
						foreach (GameObject b in answerBtns) {
							Destroy (b);
						}
						answerBtns.Clear ();
						//show result
						main.writeToMessagebox((correctAnswers+"/"+questions.Count));
				}
	}

	public void clickedBtn(int answerNr, string answer){
		switch (answer) {
		case "wrong":	//change color to red
			Debug.Log (answerNr);
			answerBtns[answerNr].GetComponent<Image>().color = colorWrong;
			//Debug.Log (button.transform.FindChild("ButtonQuiz(Clone)").GetType().ToString());
			break;
		case "correct": //change color to green
			Debug.Log (answerNr);
			answerBtns[answerNr].GetComponent<Image>().color = colorCorrect;
			correctAnswers ++;
			break;
		}
		//showNextQuestion ();
		StartCoroutine (next());
	}

	private IEnumerator next() {
		yield return new WaitForSeconds (2);
		showNextQuestion ();
	}

}
