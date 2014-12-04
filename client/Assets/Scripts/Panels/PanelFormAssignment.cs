using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PanelFormAssignment : MonoBehaviour {
	private DBInterface dbinterface;
	private JSONParser jsonparser;
	
	public GameObject assignment;
	
	public int task_id;
	
	public List<GameObject> assignments;
	public int assignment_id;
	
	//text fields
	public GameObject btnAddAssignment;
	public GameObject btnSave;
	
	// Use this for initialization
	void Start () {
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();
		jsonparser = GameObject.Find ("Scripts").GetComponent<JSONParser>();
		btnAddAssignment.GetComponent<Button> ().onClick.AddListener (() => {addAssignmentForm ();});
		btnSave.GetComponent<Button> ().onClick.AddListener (() => {saveAssignments();});
		init ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void init(){
		
		assignments = new List<GameObject> ();
		assignment_id = 0;
		//test id
		task_id = 2;
		//dbinterface.getTask ("taskData", task_id, gameObject);
	}
	
	public void dbInputHandler(string[] response){
		Debug.Log ("in dbinputhandler of PanelFormQuiz");
		string target = response [0];
		string data = response [1];
		List<string[]> parsedData;
		switch (target) {
		case "taskData": parsedData = jsonparser.JSONparse(data);
			foreach (string s in parsedData[0]){
				Debug.Log ("data: "+ s);
			}
			//Task task = new Task(task_id, parsedData[0]); 
			//TODO: get csv data from task
			loadAssignmentsFromTask("");
			break;
		}
	}
	
	public void loadAssignmentsFromTask(string csv){
		AssignmentData qu = new AssignmentData (csv);
		if (qu.getQuestions ().Count == 0) {
			addAssignmentForm();
		} else {
			foreach (QuizQuestion q in qu.getQuestions()) {
				addAssignmentForm(q.getQuestionText(), true);
				List<string> ans = (List<string>)(((object[])q.getAnswer())[0]);
				List<int> weig = (List<int>)(((object[])q.getAnswer())[1]);
			}
		}
		//TODO: assignment load
	}
	
	public void addAssignmentForm(string aname = "Neue Frage", bool load = false){
		GameObject generatedQuestion = Instantiate (assignment, Vector3.zero, Quaternion.identity) as GameObject;

		//TODO: add onload add assignment text
		int id = assignment_id;
		generatedQuestion.name = "assignment" + id;
		generatedQuestion.transform.parent = GameObject.Find ("panelAssignment/assignments").transform;
		//generatedQuestion.transform.FindChild ("InputField/Text").GetComponent<Text> ().text = aname;
		assignments.Add (generatedQuestion);
	
		assignment_id++;
	}
	
	public void saveAssignments(){
		AssignmentData assignmentData = new AssignmentData ("");
		for (int i = 0; i < assignments.Count; i++){
			//save question text
			string assignment1 = assignments[i].transform.FindChild("formAssign/InputField1/Text").GetComponent<Text>().text;
			string assignment2 = assignments[i].transform.FindChild("formAssign/InputField2/Text").GetComponent<Text>().text;

			AssignmentQuestion assignmentQuestion = new AssignmentQuestion(assignment1, assignment2);
			assignmentData.addQuestion(assignmentQuestion);
			
		}
		//TODO fill in description
		dbinterface.editTask ("editTask", task_id, "", assignmentData.getCSV(), gameObject);
		
		
		
		
		
	}
	
	public void setTaskId(int id){
		task_id = id;
	}
}
