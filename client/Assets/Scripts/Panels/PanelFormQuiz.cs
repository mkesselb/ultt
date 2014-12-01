using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PanelFormQuiz : MonoBehaviour {

	public GameObject question;
	public GameObject answer;

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
		init ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void init(){
		questions = new List<GameObject> ();
		answers = new List<List<GameObject>> ();
		question_id = 0;
		addQuestionForm();

	}

	public void addAnswerForm(string questionName, int id){
		Debug.Log ("Add answer");
		GameObject generatedAnswer = Instantiate (answer, Vector3.zero, Quaternion.identity) as GameObject;
		generatedAnswer.transform.parent = GameObject.Find(questionName+"/answers").transform;
		answers [id].Add (generatedAnswer);
	}

	public void addQuestionForm(){
		Debug.Log ("Add question");
		GameObject generatedQuestion = Instantiate (question, Vector3.zero, Quaternion.identity) as GameObject;

		int id = question_id;
		generatedQuestion.name = "question" + id;
		generatedQuestion.transform.parent = GameObject.Find ("panelQuestions/questions").transform;
		generatedQuestion.transform.FindChild("ButtonAdd").GetComponent<Button>().onClick.AddListener (() => {addAnswerForm(generatedQuestion.name, id);});	
		questions.Add (generatedQuestion);
		List<GameObject> answersForQuestion = new List<GameObject> ();
		answers.Add (answersForQuestion);
		addAnswerForm (generatedQuestion.name, id);
		addAnswerForm (generatedQuestion.name, id);
		question_id++;
	}

	public void saveQuestions(){
		for (int i = 0; i < questions.Count; i++){
			//save question text
			Debug.Log ("Question: "+ questions[i].transform.FindChild("InputField/Text").GetComponent<Text>().text);

			//save answers
			List<GameObject> answerList = answers[i];
			foreach(GameObject obj in answerList){
				Debug.Log ("answer: "+obj.transform.FindChild("InputField/Text").GetComponent<Text>().text);
			}
		}


//		foreach (GameObject q in questions) {
//			Debug.Log ("questionText: "+q.transform.FindChild("InputField/Text").GetComponent<Text>().text);
//
//			Transform[] answers = q.transform.Find("answers").GetComponentsInChildren<Transform>();
//
//			foreach (Transform answer in answers){
//				Debug.Log ("answer: "+answer.FindChild("InputField/Text").GetInstanceID());
//			}


//			Transform[] allChildren = q.transform.GetComponentsInChildren<Transform>();
//			foreach(Transform tr in allChildren){
//				if(tr.tag =="FormQuizQuestion"){
//					Debug.Log ("Question: "+tr.GetComponent<Text>().text);
//				}
//			}
//		}


	}
}
