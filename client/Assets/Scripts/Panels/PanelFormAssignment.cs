using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using SimpleJSON;

public class PanelFormAssignment : MonoBehaviour {
	/// <summary>
	/// The database interface.
	/// </summary>
	private DBInterface dbinterface;

	/// <summary>
	/// The main class.
	/// </summary>
	private Main main;

	/// <summary>
	/// assignment prefab.
	/// </summary>
	public GameObject assignment;

	/// <summary>
	/// The task_id.
	/// </summary>
	public int task_id;

	/// <summary>
	/// The list of assignment gameobjects in the form.
	/// </summary>
	public List<GameObject> assignments;

	/// <summary>
	/// The current assignment_id.
	/// </summary>
	public int assignment_id;
	
	/// <summary>
	/// The button for adding an assignment to the form.
	/// </summary>
	public GameObject btnAddAssignment;

	/// <summary>
	/// The button for saving the form.
	/// </summary>
	public GameObject btnSave;

	void Start () {
		btnAddAssignment.GetComponent<Button> ().onClick.AddListener (() => {addAssignmentForm ();});
		btnSave.GetComponent<Button> ().onClick.AddListener (() => {saveAssignments();});
		//init ();
	}

	public void init(){
		main = GameObject.Find ("Scripts").GetComponent<Main>();
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();

		foreach (GameObject go in assignments) {
			Destroy (go);
		}

		btnAddAssignment.transform.FindChild("Text").GetComponent<Text>().text
			= LocaleHandler.getText ("button-assig-add", main.getLang());
		btnSave.transform.FindChild("Text").GetComponent<Text>().text
			= LocaleHandler.getText ("button-assig-save", main.getLang());

		assignments = new List<GameObject> ();
		assignment_id = 0;
		dbinterface.getTask ("taskData", task_id, gameObject);
	}

	/// <summary>
	/// Handles incoming data from the database
	/// </summary>
	/// 
	/// <param name="response">response data from the database.</param>
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

	/// <summary>
	/// Loads saved assignments to form.
	/// </summary>
	/// 
	/// <param name="csv">already saved assignment data.</param>
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

	/// <summary>
	/// Adds assignment to form.
	/// </summary>
	/// 
	/// <param name="aname">text in first column of assignment.</param>
	/// <param name="bname">text in second column of assignment.</param>
	public void addAssignmentForm(string aname = "Neue Zuordnung1", string bname = "Neue Zuordnung2"){
		GameObject generatedAssignment = Instantiate (assignment, Vector3.zero, Quaternion.identity) as GameObject;

		int id = assignment_id;
		generatedAssignment.name = "assignment" + id;
		generatedAssignment.transform.parent = GameObject.Find ("panelAssignment/assignments").transform;
		generatedAssignment.transform.FindChild ("formAssign/InputField1").GetComponent<InputField> ().text = aname;
		generatedAssignment.transform.FindChild ("formAssign/InputField2").GetComponent<InputField> ().text = bname;
		generatedAssignment.transform.FindChild ("formAssign/ButtonDelete").GetComponent<Button> ().onClick.AddListener (() => {deleteAssignments(generatedAssignment);});
		assignments.Add (generatedAssignment);
	
		assignment_id++;
	}

	/// <summary>
	/// Saves assignment data from form.
	/// </summary>
	public void saveAssignments(){
		AssignmentData assignmentData = new AssignmentData ("");
		for (int i = 0; i < assignments.Count; i++){
			//save question text
			string assignment1 = assignments[i].transform.FindChild("formAssign/InputField1").GetComponent<InputField>().text;
			string assignment2 = assignments[i].transform.FindChild("formAssign/InputField2").GetComponent<InputField>().text;
			AssignmentQuestion assignmentQuestion = new AssignmentQuestion(assignment1, assignment2);
			assignmentData.addQuestion(assignmentQuestion);
			
		}
		//TODO fill in description
		dbinterface.editTask ("editTask", task_id, "", assignmentData.getCSV(), gameObject);
	}

	/// <summary>
	/// Delete assignment from form.
	/// </summary>
	/// 
	/// <param name="assignment">assignment gameobject to be deleted.</param>
	public void deleteAssignments(GameObject generatedAssignment){
		assignments.RemoveAt(assignments.IndexOf(generatedAssignment));
		Destroy (generatedAssignment);
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
