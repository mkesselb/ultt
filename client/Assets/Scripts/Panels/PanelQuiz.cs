using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PanelQuiz : MonoBehaviour {
	//multiple choice, one right answer

	private Main main;
	private DBInterface dbinterface;

	public int task_id;
	public QuizData quizData;

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
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();

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

		

		//dbinterface.getTask ("taskData", task_id, gameObject);
		dbInputHandler(new string[]{"",""});
	}

	public void dbInputHandler(string[] response){
		Debug.Log ("in dbinputhandler of PanelQuiz");
		string target = response [0];
		string data = response [1];
		switch (target) {	
		case "taskData":
			quizData = new QuizData(data);
			break;
		}
		showNextQuestion ();

	}

	public void showNextQuestion(){
		if (questionNr < quizData.getQuestions().Count) {
						GameObject generatedBtn;
						//delete old answer buttons
						foreach (GameObject b in answerBtns) {
								Destroy (b);
						}
						answerBtns.Clear ();
						Debug.Log ("Start quiz");
						QuizQuestion question =  (QuizQuestion)quizData.getQuestion(questionNr);

						//show question nr and question text
						textQuestionNr.GetComponent<Text> ().text = "Frage " + (questionNr + 1) + ":";
						textQuestion.GetComponent<Text> ().text = question.getQuestionText();

						object[] data = (object[])question.getAnswer();

						List<string> questionAnswers = (List<string>)data[0];
						/*List<int> questionWeights = (List<string>)data[1];


						//######################################################
						
						//generate buttons for answer options
						for (int i = 0; i < question.getAnswer(); i++) {
								generatedBtn = Instantiate (answerBtn, Vector3.zero, Quaternion.identity) as GameObject;
								generatedBtn.transform.parent = GameObject.Find ("contentQuestion").transform;
								generatedBtn.transform.FindChild ("Text").GetComponent<Text> ().text = "";

								//set method to be called at onclick event
								//add onclick action for buttons: method: clickedBtn, param: "wrong"||"correct"
								/*string temp;
								int answerNr = i;
								generatedBtn.GetComponent<Button> ().onClick.AddListener (() => {clickedBtn (answerNr,temp);});
								answerBtns.Add (generatedBtn);
						}*/
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
			break;
		case "correct": //change color to green
			Debug.Log (answerNr);
			answerBtns[answerNr].GetComponent<Image>().color = colorCorrect;
			correctAnswers ++;
			break;
		}
		StartCoroutine (next());
	}

	private IEnumerator next() {
		yield return new WaitForSeconds (2);
		showNextQuestion ();
	}

	public void setTaskId(int id){
			task_id = id;
	}

}
