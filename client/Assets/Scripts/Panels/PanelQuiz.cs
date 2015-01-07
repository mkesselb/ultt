using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJSON;

public class PanelQuiz : MonoBehaviour {
	//multiple choice, one right answer

	private Main main;
	private DBInterface dbinterface;

	public int task_id;
	public QuizData quizData;

	public GameObject toggleAnswer;
	public Text textQuestion;
	public Text textQuestionNr;
	public GameObject btnCheck;
	public GameObject btnNext;

	public int questionNr;
	public int correctAnswers;
	public int totalAnswers;

	public List<GameObject> answers;
	public List<Question> questions;

	public Color colorCorrect;
	public Color colorWrong;


	void Start () {
	}
	

	void Update () {

	}




	public void init(){
		main = GameObject.Find ("Scripts").GetComponent<Main>();
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();

		//set question number to 0
		questionNr = 0;
		//set correct answers to 0
		correctAnswers = 0;
		totalAnswers = 0;
		//delete buttons by iterating button array
		foreach(GameObject b in answers){
			Destroy(b);
		}
		answers.Clear ();
		answers = new List<GameObject> ();
		dbinterface.getTask ("taskData", task_id, gameObject);

	}

	public void dbInputHandler(string[] response){
		Debug.Log ("in dbinputhandler of PanelQuiz");
		string target = response [0];
		string data = response [1];
		JSONNode parsedData = JSONParser.JSONparse(data);
		switch (target) {	
		case "taskData":
			Task task = new Task(task_id, parsedData[0]); 
			quizData = new QuizData(task.getDatafile());
			break;
		}
		showNextQuestion ();

	}

	public void showNextQuestion(){
		if (questionNr < quizData.getQuestions().Count) {
			
						GameObject generatedAns;
						//delete old answer buttons
						foreach (GameObject b in answers) {
								Destroy (b);
						}
						answers.Clear ();
						Destroy(GameObject.Find ("contentQuestion/btnNext(Clone)"));
						
						QuizQuestion question =  (QuizQuestion)quizData.getQuestion(questionNr);

						Debug.Log ("Questionnr: "+ questionNr+", questiontext: "+question.getQuestionText());

						//show question nr and question text
						textQuestionNr.GetComponent<Text> ().text = "Frage " + (questionNr + 1) + ":";
						textQuestion.GetComponent<Text> ().text = question.getQuestionText();
						
						List<string> ans = (List<string>)(((object[])question.getAnswer())[0]);
						List<int> weig = (List<int>)(((object[])question.getAnswer())[1]);
						for(int i = 0; i < ans.Count; i++){
							generatedAns = Instantiate (toggleAnswer, Vector3.zero, Quaternion.identity) as GameObject;
							generatedAns.transform.parent = GameObject.Find ("contentQuestion").transform;
							generatedAns.transform.FindChild ("Label").GetComponent<Text> ().text = ans[i];
							answers.Add (generatedAns);
						}
						//check button
						GameObject btn;
					
						btn = Instantiate (btnCheck, Vector3.zero, Quaternion.identity) as GameObject;
						btn.transform.parent = GameObject.Find ("contentQuestion").transform;
						int nr = questionNr;
						btn.GetComponent<Button>().onClick.AddListener (() => {checkAnswers(nr);});	
			
						
						questionNr ++;
				} else {
						textQuestionNr.GetComponent<Text> ().text = "";
						textQuestion.GetComponent<Text> ().text = "";
						//delete buttons
						foreach (GameObject b in answers) {
							Destroy (b);
						}
						answers.Clear ();
						Destroy(GameObject.Find ("contentQuestion/btnCheck(Clone)"));
						Destroy(GameObject.Find ("contentQuestion/btnNext(Clone)"));
						//show result

						textQuestion.GetComponent<Text> ().text = correctAnswers+"/"+totalAnswers;
						//finish button
						GameObject btn;
			
						btn = Instantiate (btnNext, Vector3.zero, Quaternion.identity) as GameObject;
						btn.transform.parent = GameObject.Find ("contentQuestion").transform;
						btn.transform.FindChild("Text").GetComponent<Text>().text = "beenden";
						btn.GetComponent<Button>().onClick.AddListener (() => {finishTask ();});	
						
				}
	}

	private void next() {
		Debug.Log ("clicked");
		showNextQuestion ();
	}

	public void checkAnswers(int questionnr){
		QuizQuestion question =  (QuizQuestion)quizData.getQuestion(questionnr);
		List<int> a = new List<int>();
		foreach (GameObject g in answers) {
			a.Add(g.transform.GetComponent<Toggle>().isOn?1:0);
		}
		QuizQuestion answer = new QuizQuestion (question.getQuestionText(), (List<string>)(((object[])question.getAnswer())[0]), a);
		List<int> results = question.checkAnswerIndices (answer);

		for (int i = 0; i< results.Count; i++) {
			if(results[i] == 0) { answers[i].transform.FindChild ("Background").GetComponent<Image>().color = colorCorrect;	correctAnswers++;	}  //false answer, not selected
			if(results[i] == 1) { answers[i].transform.FindChild ("Background").GetComponent<Image>().color = colorCorrect; correctAnswers++; 	} //right answer, selected
			if(results[i] == 2) { answers[i].transform.FindChild ("Background").GetComponent<Image>().color = colorWrong; 	} //false answer, selected
			if(results[i] == 3) { answers[i].transform.FindChild ("Background").GetComponent<Image>().color = colorWrong; 		} //right answer, not selected
			answers[i].GetComponent<Toggle>().interactable = false;
			totalAnswers++;
		}

		Destroy(GameObject.Find ("contentQuestion/btnCheck(Clone)"));

		//TODO save result

		//next button
		GameObject btn;
		
		btn = Instantiate (btnNext, Vector3.zero, Quaternion.identity) as GameObject;
		btn.transform.parent = GameObject.Find ("contentQuestion").transform;
		btn.GetComponent<Button>().onClick.AddListener (() => {next ();});	
			
		

	}

	public void finishTask(){
		main.eventHandler ("finishTask", task_id);
	}

	public void setTaskId(int id){
			task_id = id;
	}

}
