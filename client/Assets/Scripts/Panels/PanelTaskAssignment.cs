using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using SimpleJSON;

public class PanelTaskAssignment : MonoBehaviour {
	private DBInterface dbinterface;
	private Main main;
	
	public GameObject assignmentToggleLeft;
	public GameObject assignmentToggleRight;
	//public GameObject statusText;
	public GameObject btnNextAssignment;
	//public GameObject btnRelease;
	
	private AssignmentData assData;
	private AssignmentData userAnswer;
	
	public List<int> correctFlag;
	
	private double answers;
	private double points;
	
	public int task_id;
	public int task_for_class_id;
	public int user_id;
	public bool isTeacher;
	
	void Start () {
		btnNextAssignment.GetComponent<Button> ().onClick.AddListener (() => {nextAssignment ();});
		btnNextAssignment.transform.FindChild ("Text").GetComponent<Text> ().text = "überprüfen";
	}
	
	public void init(){
		
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();
		main = GameObject.Find ("Scripts").GetComponent<Main>();
		
		//first, destroy all toggles
		for (int i = 0; i < gameObject.transform.FindChild("panelAssignmentLeft/panelLeft").childCount; i++){
			Destroy(gameObject.transform.FindChild("panelAssigmentLeft/panelLeft").GetChild (i).gameObject);
		}
		for (int i = 0; i < gameObject.transform.FindChild("panelAssignmentRight/panelRight").childCount; i++){
			Destroy(gameObject.transform.FindChild("panelAssignmentRight/panelRight").GetChild (i).gameObject);
		}
		
		answers = -1;
		points = 0;
		
		/*gameObject.transform.FindChild("Text").GetComponent<Text> ().text 
			= LocaleHandler.getText ("info-nextword", main.getLang());*/

		dbinterface.getTask ("taskData", task_id, gameObject);
	}
	
	public void nextAssignment(bool first = false){
		//check current assignment
		string selectedAssLeft = "";
		string selectedAssRight = "";
		
		//find toggled answer on left side
		for (int i = 0; i < gameObject.transform.FindChild("panelAssignmentLeft/panelLeft").childCount; i++){
			Transform trLeft = gameObject.transform.FindChild("panelAssignmentLeft/panelLeft").GetChild (i);
			if(trLeft.GetComponent<Toggle>().isOn){
				selectedAssLeft = trLeft.Find ("Label").GetComponent<Text>().text;
				//remove entry from list
				Destroy (trLeft.gameObject);
			}
		}
		//find toggled answer on right side
		for (int i = 0; i < gameObject.transform.FindChild("panelAssignmentRight/panelRight").childCount; i++){
			Transform trRight = gameObject.transform.FindChild("panelAssignmentRight/panelRight").GetChild (i);
			if(trRight.GetComponent<Toggle>().isOn){
				selectedAssRight = trRight.Find ("Label").GetComponent<Text>().text;
				//remove entry from list
				Destroy (trRight.gameObject);
			}
		}
		
		//check if assigment is correct
		Debug.Log ("selected:" + selectedAssLeft + ";" + selectedAssRight);
		AssignmentQuestion check = new AssignmentQuestion (selectedAssLeft, selectedAssRight);
		int isCorrect = 0;
		foreach (AssignmentQuestion aq in assData.getQuestionWithField (selectedAssLeft, 0)) {;
			if (aq.checkAnswer(check) == 1) {
				points++;
				isCorrect = 1;
				break;
			}
		}
		
		//add true [1] or false [0] to correctFlag 
		correctFlag.Add (isCorrect);
		userAnswer.addQuestion(check);
		answers--;
		
		//leave task after the last couple
		if (answers == 0) {
			//call to saveTask
			int p = (int)(100 * (double)points / assData.getFullPoints());
			string results = p + "\n";
			foreach(int i in correctFlag){
				results += i + ",";
			}
			results += "\n" + userAnswer.getCSV();

			if(isTeacher){
				main.writeToMessagebox("Ergebnis: " + points + "/" + assData.getFullPoints());
				finishTask();
			} else{
				dbinterface.saveTask("savedTask", user_id, task_for_class_id, results, gameObject);
			}
		}
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
		case "savedTask":
			main.writeToMessagebox("Ergebnis gespeichert: " + points + "/" + assData.getFullPoints());
			finishTask();
			break;
		}
	}

	public void finishTask(){
		main.eventHandler ("finishTask", task_id);
	}
	
	public void loadAssignmentsFromTask(string csv){
		this.assData = new AssignmentData (csv);
		this.userAnswer = new AssignmentData ("");
		this.correctFlag = new List<int> ();
		List<string> left = new List<string> ();
		List<string> right = new List<string> ();
		
		//populate toggle group
		bool active = false;
		foreach (AssignmentQuestion ts in assData.getQuestions()) {
			List<string> a = (List<string>) ts.getAnswer();
			left.Add(a[0]);
			right.Add(a[1]);
		}
		
		Shuffle.shuffle (left);
		Shuffle.shuffle (right);
		answers = left.Count;
		
		for (int i = 0; i < left.Count; i++) {
			GameObject assLeft = Instantiate (assignmentToggleLeft, Vector3.zero, Quaternion.identity) as GameObject;
			GameObject assRight = Instantiate (assignmentToggleRight, Vector3.zero, Quaternion.identity) as GameObject;
			assLeft.transform.parent = gameObject.transform.FindChild ("panelAssignmentLeft/panelLeft");
			assRight.transform.parent = gameObject.transform.FindChild ("panelAssignmentRight/panelRight");
			assLeft.transform.FindChild ("Label").GetComponent<Text> ().text = left[i];
			assRight.transform.FindChild ("Label").GetComponent<Text> ().text = right[i];
			assLeft.GetComponent<Toggle>().isOn = active;
			assRight.GetComponent<Toggle>().isOn = active;
			assLeft.GetComponent<Toggle>().group = assLeft.transform.parent.GetComponent<ToggleGroup>();
			assRight.GetComponent<Toggle>().group = assRight.transform.parent.GetComponent<ToggleGroup>();
		}
	}
	
	public void setTaskId(int id){
		task_id = id;
	}
	
	public void setTaskForClassId(int id){
		task_for_class_id = id;
	}
	
	public void setUserId(int id){
		user_id = id;
	}

	public void setIsTeacher(bool isteacher){
		this.isTeacher = isteacher;
	}
}