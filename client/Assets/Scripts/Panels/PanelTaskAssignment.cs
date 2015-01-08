using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using SimpleJSON;

public class PanelTaskAssignment : MonoBehaviour {
	//test parameter -> to be deleted if form is attached to rest of app
	private bool first = true;
	private DBInterface dbinterface;
	
	public GameObject assignment;
	
	public int task_id;
	
	public List<GameObject> assignments;
	public int assignment_id;
	
	//text fields
	public GameObject btnRelease;

	// Use this for initialization
	void Start () {
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();
		btnRelease.GetComponent<Button> ().onClick.AddListener (() => {releaseAssignments();});
		//init ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void init(){
		
		assignments = new List<GameObject> ();
		assignment_id = 0;
		//dbinterface.getTask ("taskData", task_id, gameObject);
	}
	
	public void dbInputHandler(string[] response){
		Debug.Log ("in dbinputhandler of PanelFormAssignment");
		string target = response [0];
		string data = response [1];
		JSONNode parsedData = JSONParser.JSONparse(data);
		switch (target) {
		case "taskData": 
			Task task = new Task(task_id, parsedData[0]); 
			loadAssignmentsFromTask(task.getDatafile());
			break;
		}
	}
	
	public void loadAssignmentsFromTask(string csv){
		AssignmentData qu = new AssignmentData (csv);
		if (qu.getQuestions ().Count == 0) {
			addAssignmentForm();
		} else {
			foreach (AssignmentQuestion q in qu.getQuestions()) {
				List<string> ans = (List<string>)q.getAnswer();
				addAssignmentForm(ans[0], ans[1]);
			}
		}
	}
	
	public void addAssignmentForm(string aname = "Neue Zuordnung1", string bname = "Neue Zuordnung2"){
		if (first) {
			dbinterface.getTask ("taskData", task_id, gameObject);
			first = false;
			return;
		}
		GameObject generatedAssignment = Instantiate (assignment, Vector3.zero, Quaternion.identity) as GameObject;

		int id = assignment_id;
		generatedAssignment.name = "assignment" + id;
		generatedAssignment.transform.parent = GameObject.Find ("panelAssignmentLeft/panelLeft").transform;
		generatedAssignment.transform.FindChild ("panelAssignmentLeft/InputField1/Text").GetComponent<Text> ().text = aname;
		generatedAssignment.transform.FindChild ("panelAssignmentRight/InputField2/Text").GetComponent<Text> ().text = bname;
		assignments.Add (generatedAssignment);
	
		assignment_id++;
	}
	
	public void releaseAssignments(){
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
