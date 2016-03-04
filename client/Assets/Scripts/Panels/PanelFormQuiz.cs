using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using SimpleJSON;

public class PanelFormQuiz : MonoBehaviour {

	/// <summary>
	/// The database interface.
	/// </summary>
	private DBInterface dbinterface;

	/// <summary>
	/// The main class.
	/// </summary>
	private Main main;

	/// <summary>
	/// The question prefab.
	/// </summary>
	public GameObject question;

	/// <summary>
	/// The answer prefab.
	/// </summary>
	public GameObject answer;

	/// <summary>
	/// The task_id.
	/// </summary>
	public int task_id;

	/// <summary>
	/// The list of questions.
	/// </summary>
	public List<GameObject> questions;

	/// <summary>
	/// The question_id.
	/// </summary>
	public int question_id;

	/// <summary>
	/// The list of answers.
	/// </summary>
	public List<List<GameObject>> answers;

	/// <summary>
	/// The button to add a new question
	/// </summary>
	public GameObject btnAddQuestion;

	/// <summary>
	/// The button for saving the form.
	/// </summary>
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

		/* TODO: this code fragment gives nullpointer...disabling it seems fine
		question.transform.FindChild("ButtonAdd/Text").GetComponent<Text>().text
			= LocaleHandler.getText ("button-quiz-add-answer", main.getLang());
		*/

		questions = new List<GameObject> ();
		answers = new List<List<GameObject>> ();
		question_id = 0;
		dbinterface.getTask ("taskData", task_id, gameObject);
	}

	// <summary>
	/// Handles incoming data from the database
	/// </summary>
	/// 
	/// <param name="response">response data from the database.</param>
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

	// <summary>
	/// Loads saved questions to form.
	/// </summary>
	/// 
	/// <param name="csv">already saved question data.</param>
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

	/// <summary>
	/// Adds answer to form.
	/// </summary>
	/// 
	/// <param name="questionName">name of the question.</param>
	/// <param name="id">id of the question.</param>
	/// <param name="answerText">text of answer.</param>
	/// <param name="correct">flag for correct answer.</param>
	public void addAnswerForm(string questionName, int id, string answerText = "Neue Antwort", bool correct = false){
		GameObject generatedAnswer = Instantiate (answer, Vector3.zero, Quaternion.identity) as GameObject;
		generatedAnswer.transform.parent = GameObject.Find(questionName+"/answers").transform;
		generatedAnswer.transform.FindChild ("InputField").GetComponent<InputField> ().text = answerText;
		generatedAnswer.transform.FindChild ("Toggle").GetComponent<Toggle> ().isOn = correct;
		generatedAnswer.transform.FindChild("ButtonDelete").GetComponent<Button>().onClick.AddListener (() => {deleteAnswerElement(generatedAnswer, id);});
		answers [id].Add (generatedAnswer);
	}

	/// <summary>
	/// Adds question to form.
	/// </summary>
	/// 
	/// <param name="qname">question name.</param>
	/// <param name="load">flag for adding answers to question.</param>
	public void addQuestionForm(string qname = "Neue Frage", bool load = false){
		GameObject generatedQuestion = Instantiate (question, Vector3.zero, Quaternion.identity) as GameObject;

		int id = question_id;
		question_id++;
		generatedQuestion.name = "question" + id;
		generatedQuestion.transform.parent = GameObject.Find ("panelQuestions/questions").transform;
		generatedQuestion.transform.FindChild("panelButtons/ButtonAddAnswer").GetComponent<Button>().onClick.AddListener (() => {addAnswerForm(generatedQuestion.name, id);});
		generatedQuestion.transform.FindChild("panelButtons/ButtonDeleteQuestion").GetComponent<Button>().onClick.AddListener (() => {deleteQuestionElement(generatedQuestion);});
		generatedQuestion.transform.FindChild ("InputField").GetComponent<InputField> ().text = qname;
		questions.Add (generatedQuestion);
	
		List<GameObject> answersForQuestion = new List<GameObject> ();
		answers.Add (answersForQuestion);
		if (!load) {
			addAnswerForm (generatedQuestion.name, id);
			addAnswerForm (generatedQuestion.name, id);
		}
	}

	/// <summary>
	/// Delete generated question field from form.
	/// </summary>
	public void deleteQuestionElement(GameObject generatedQuestion){
		questions.RemoveAt(questions.IndexOf (generatedQuestion));
		Destroy (generatedQuestion);
	}

	/// <summary>
	/// Delete generated answer field from form.
	/// </summary>
	public void deleteAnswerElement(GameObject generatedAnswer, int id){
		answers [id].RemoveAt (answers [id].IndexOf (generatedAnswer));
		Destroy (generatedAnswer);
	}



	/// <summary>
	/// Saves question data from form.
	/// </summary>
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

	/// <summary>
	/// Set task id of currently edited task.
	/// </summary>
	/// 
	/// <param name="id">task id of currently edited task.</param>
	public void setTaskId(int id){
		task_id = id;
	}
}
