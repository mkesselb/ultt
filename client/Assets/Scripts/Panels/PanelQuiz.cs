using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJSON;

public class PanelQuiz : MonoBehaviour {

	/// <summary>
	/// The main class.
	/// </summary>
	private Main main;

	/// <summary>
	/// The database interface.
	/// </summary>
	private DBInterface dbinterface;

	/// <summary>
	/// The task_id.
	/// </summary>
	public int task_id;

	/// <summary>
	/// The task_for_class_id.
	/// </summary>
	public int task_for_class_id;

	/// <summary>
	/// The user_id.
	/// </summary>
	public int user_id;

	/// <summary>
	/// The teacher flag.
	/// </summary>
	public bool isTeacher;

	/// <summary>
	/// The quizdata object.
	/// </summary>
	public QuizData quizData;

	/// <summary>
	/// The toggle prefab.
	/// </summary>
	public GameObject toggleAnswer;

	/// <summary>
	/// The question textfield.
	/// </summary>
	public Text textQuestion;

	/// <summary>
	/// The question number textfield.
	/// </summary>
	public Text textQuestionNr;

	/// <summary>
	/// The check button.
	/// </summary>
	public GameObject btnCheck;

	/// <summary>
	/// The next button.
	/// </summary>
	public GameObject btnNext;

	/// <summary>
	/// The question number.
	/// </summary>
	public int questionNr;

	/// <summary>
	/// The number of correct answers.
	/// </summary>
	public int correctAnswers;

	/// <summary>
	/// The number of answers.
	/// </summary>
	public int totalAnswers;

	/// <summary>
	/// The quizdata object with the user answers.
	/// </summary>
	public QuizData userAnswers;

	public Dictionary<string, List<int>> cAns;

	/// <summary>
	/// The list of answer gameobjects.
	/// </summary>
	public List<GameObject> answers;

	/// <summary>
	/// The list of questions.
	/// </summary>
	public List<Question> questions;

	/// <summary>
	/// The color for correct answers.
	/// </summary>
	public Color colorCorrect;

	/// <summary>
	/// The color for wrong answers.
	/// </summary>
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

	/// <summary>
	/// Handles incoming data from the database.
	/// </summary>
	/// 
	/// <param name="response">response data from the database.</param>
	public void dbInputHandler(string[] response){
		Debug.Log ("in dbinputhandler of PanelQuiz");
		string target = response [0];
		string data = response [1];
		JSONNode parsedData = JSONParser.JSONparse(data);
		switch (target) {	
		case "taskData":
			Task task = new Task(task_id, parsedData[0]); 
			quizData = new QuizData(task.getDatafile());
			quizData.shuffleQuestions();
			userAnswers = new QuizData("");
			cAns = new Dictionary<string, List<int>>();
			showNextQuestion ();
			break;
		case "savedTask":
			main.writeToMessagebox("Ergebnis gespeichert: " + correctAnswers + "/" + quizData.getFullPoints());
			finishTask();
			break;
		}

	}

	/// <summary>
	/// Shows next quiz question.
	/// </summary>
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
						//textQuestion.GetComponent<Text> ().text = correctAnswers+"/"+totalAnswers;

						//save results
						int p = (quizData.getFullPoints() == 0 ? 0 : (int)(100 * (double)correctAnswers / quizData.getFullPoints()));
						string results = p + "\n";
						foreach(string key in cAns.Keys){
							results += key + ",";
							foreach(int i in cAns[key]){
								results += i + ",";
							}
						}
						results += "\n" + userAnswers.getCSV();
						
						if(isTeacher){
							main.writeToMessagebox("Ergebnis: " + correctAnswers + "/" + quizData.getFullPoints());
							finishTask();
						} else{
							dbinterface.saveTask("savedTask", user_id, task_for_class_id, results, gameObject);
						}
						
						//finish button
						/*GameObject btn;
			
						btn = Instantiate (btnNext, Vector3.zero, Quaternion.identity) as GameObject;
						btn.transform.parent = GameObject.Find ("contentQuestion").transform;
						btn.transform.FindChild("Text").GetComponent<Text>().text = "beenden";
						btn.GetComponent<Button>().onClick.AddListener (() => {finishTask ();});*/
				}
	}

	/// <summary>
	/// Go to next question.
	/// </summary>
	private void next() {
		Debug.Log ("clicked");
		showNextQuestion ();
	}

	/// <summary>
	/// Check the user answer.
	/// </summary>
	/// 
	/// <param name="questionnr">number of answered question.</param>
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

		userAnswers.addQuestion (answer);
		cAns.Add (answer.getQuestionText (), results);

		//next button
		GameObject btn;
		
		btn = Instantiate (btnNext, Vector3.zero, Quaternion.identity) as GameObject;
		btn.transform.parent = GameObject.Find ("contentQuestion").transform;
		btn.GetComponent<Button>().onClick.AddListener (() => {next ();});
	}

	/// <summary>
	/// Finished task.
	/// </summary>
	public void finishTask(){
		main.eventHandler ("finishTask", task_id);
	}

	/// <summary>
	/// Set task id of currently edited task.
	/// </summary>
	/// 
	/// <param name="id">task id of currently edited task.</param>
	public void setTaskId(int id){
			task_id = id;
	}

	/// <summary>
	/// Set task_for_class_id of currently edited task.
	/// </summary>
	/// 
	/// <param name="id">task id of currently edited task.</param>
	public void setTaskForClassId(int id){
		task_for_class_id = id;
	}

	/// <summary>
	/// Set user_id.
	/// </summary>
	/// 
	/// <param name="id">user_id.</param>
	public void setUserId(int id){
		user_id = id;
	}

	public void setIsTeacher(bool isteacher){
		this.isTeacher = isteacher;
	}
}