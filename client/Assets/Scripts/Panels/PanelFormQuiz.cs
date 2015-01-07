using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using SimpleJSON;

public class PanelFormQuiz : MonoBehaviour {
	private DBInterface dbinterface;
	private Main main;

	public GameObject question;
	public GameObject answer;

	public int task_id;

	public List<GameObject> questions;
	public int question_id;

	//text fields
	public List<List<GameObject>> answers;
	public GameObject btnAddQuestion;
	public GameObject btnSave;

	// Use this for initialization
	void Start () {
		btnAddQuestion.GetComponent<Button> ().onClick.AddListener (() => {addQuestionForm ();});
		btnSave.GetComponent<Button> ().onClick.AddListener (() => {saveQuestions();});
		//init ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void init(){
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();
		main = GameObject.Find ("Scripts").GetComponent<Main>();

		foreach (GameObject q in questions){
			Destroy(q);	
		}
		questions.Clear();

		GameObject.Find ("panelQuestions/Text").GetComponent<Text> ().text = LocaleHandler.getText ("quiz-info", main.getLang());
		btnAddQuestion.transform.FindChild("Text").GetComponent<Text>().text
			= LocaleHandler.getText ("button-quiz-add-question", main.getLang());
		btnSave.transform.FindChild("Text").GetComponent<Text>().text
			= LocaleHandler.getText ("button-quiz-save", main.getLang());
		question.transform.FindChild("ButtonAdd/Text").GetComponent<Text>().text
			= LocaleHandler.getText ("button-quiz-add-answer", main.getLang());

		questions = new List<GameObject> ();
		answers = new List<List<GameObject>> ();
		question_id = 0;
		dbinterface.getTask ("taskData", task_id, gameObject);
	}

	public void dbInputHandler(string[] response){
		Debug.Log ("in dbinputhandler of PanelFormQuiz");
		string target = response [0];
		string data = response [1];
		JSONNode parsedData = JSONParser.JSONparse(data);
		switch (target) {
		case "taskData":
			Task task = new Task(task_id, parsedData[0]); 
			Debug.Log("received datafile for quiz: "+task.getDatafile());
			loadQuestionsFromTask(task.getDatafile());
			break;
		case "editTask":
			if(int.Parse(parsedData[0]["success"]) == 1){ //success
				main.writeToMessagebox("Die Änderungen wurden erfolgreich gespeichert.");
			} else {
				main.writeToMessagebox("Die Änderungen konnten nicht gespeichert werden.");
			}
			break;
		}
	}

	public void loadQuestionsFromTask(string csv){
		QuizData qu = new QuizData (csv);
		if (qu.getQuestions ().Count == 0) {
			addQuestionForm();
		} else {
			foreach (QuizQuestion q in qu.getQuestions()) {
				addQuestionForm(q.getQuestionText(), true);
				List<string> ans = (List<string>)(((object[])q.getAnswer())[0]);
				List<int> weig = (List<int>)(((object[])q.getAnswer())[1]);
				for(int i = 0; i < ans.Count; i++){
					addAnswerForm("question"+(question_id-1), question_id-1, ans[i], weig[i]>0);
				}
			}
		}
	}

	public void addAnswerForm(string questionName, int id, string answerText = "Neue Antwort", bool correct = false){
		GameObject generatedAnswer = Instantiate (answer, Vector3.zero, Quaternion.identity) as GameObject;
		generatedAnswer.transform.parent = GameObject.Find(questionName+"/answers").transform;
		generatedAnswer.transform.FindChild ("InputField").GetComponent<InputField> ().text = answerText;
		generatedAnswer.transform.FindChild ("Toggle").GetComponent<Toggle> ().isOn = correct;
		answers [id].Add (generatedAnswer);
	}

	public void addQuestionForm(string qname = "Neue Frage", bool load = false){
		GameObject generatedQuestion = Instantiate (question, Vector3.zero, Quaternion.identity) as GameObject;

		int id = question_id;
		generatedQuestion.name = "question" + id;
		generatedQuestion.transform.parent = GameObject.Find ("panelQuestions/questions").transform;
		generatedQuestion.transform.FindChild("ButtonAdd").GetComponent<Button>().onClick.AddListener (() => {addAnswerForm(generatedQuestion.name, id);});	
		generatedQuestion.transform.FindChild ("InputField").GetComponent<InputField> ().text = qname;
		questions.Add (generatedQuestion);
		List<GameObject> answersForQuestion = new List<GameObject> ();
		answers.Add (answersForQuestion);
		if (!load) {
			addAnswerForm (generatedQuestion.name, id);
			addAnswerForm (generatedQuestion.name, id);
		}
		question_id++;
	}

	public void saveQuestions(){
		QuizData quizData = new QuizData ("");
		for (int i = 0; i < questions.Count; i++){
			//save question text
			string questionText = questions[i].transform.FindChild("InputField").GetComponent<InputField>().text;
			List<string> questionAns = new List<string>();
			List<int> questionWeight = new List<int>();
			//save answers
			List<GameObject> answerList = answers[i];
			foreach(GameObject obj in answerList){
				questionAns.Add (obj.transform.FindChild("InputField").GetComponent<InputField>().text);
				questionWeight.Add (obj.transform.FindChild("Toggle").GetComponent<Toggle>().isOn? 1 : 0);
			}
			Debug.Log (questionAns.ToString());
			Debug.Log (questionWeight.ToString());
			QuizQuestion quizQuestion = new QuizQuestion(questionText, questionAns, questionWeight);
			quizData.addQuestion(quizQuestion);
		
		}
		//TODO fill in description
		dbinterface.editTask ("editTask", task_id, "", quizData.getCSV(), gameObject);
	}

	public void setTaskId(int id){
		task_id = id;
	}
}
