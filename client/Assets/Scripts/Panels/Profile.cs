using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using SimpleJSON;

public class Profile : MonoBehaviour {
	
	private Main main;
	private DBInterface dbinterface;
	
	//Objects on standard view
	public GameObject overviewKlassen, overviewKurse, overviewTasks;
	//creation form (to create new class)
	public GameObject panelCreateClass;
	//objects on creation form
	public Text classname, classsubject, classschoolyear;
	//creation form for task
	public GameObject panelCreateTask;
	//objects on task creation form
	public Text taskname, tasksubject, taskpublic;
	public Toggle toggleAssignment, toggleQuiz, toggleCategory;
	//registration form (to register to existing class via classcode)
	public GameObject panelRegistration;
	//object on registration form
	public Text classcode;
	
	public Text fieldUserData;
	
	//Button prefab to dynamically generate buttons
	public GameObject button, buttonUserClass, buttonTeacherClass, buttonTasks;

	//Menu buttons
	public GameObject menuUserClass, menuTeacherClass, menuTasks;
	
	public int userid;
	public User user;
	public List<UserClass> userClasses;
	public List<TeacherClass> teacherClasses;
	public List<TaskOverview> tasks;
	public List<GameObject> userClassesBtns, teacherClassesBtns, tasksBtns;

	void Start(){
		main = GameObject.Find ("Scripts").GetComponent<Main>();
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();
		//fieldUserData = GameObject.Find ("fieldUserData").GetComponent<Text>();
		
		//only teacherClasses view is actve first
		panelCreateClass.SetActive(false);
		panelCreateTask.SetActive (false);
		panelRegistration.SetActive(false);
		overviewKlassen.SetActive(true);

		menuTeacherClass.GetComponent<Button> ().interactable = false;
		menuUserClass.GetComponent<Button> ().interactable = true;
		menuTasks.GetComponent<Button> ().interactable = true;


		//userid = main.getUserId();
		userClasses = new List<UserClass>();
		teacherClasses = new List<TeacherClass>();
		tasks = new List<TaskOverview> ();
		userClassesBtns = new List<GameObject>();
		teacherClassesBtns = new List<GameObject>();
		tasksBtns = new List<GameObject>();
		
		Debug.Log ("Send request for user data");
		dbinterface.getUserData("userData", userid, gameObject);
	
	}

	//action performed after clicking on menu items teacherClasses, courses or tasks
	public void clickedBtn(string target){
		
		//cases: button of each class, course, task
		switch(target){
		case "btnKlassen":	//show overview of teacherClasses
							overviewKlassen.SetActive(true);
							overviewKurse.SetActive(false);
							overviewTasks.SetActive(false);
							menuTeacherClass.GetComponent<Button> ().interactable = false;
							menuUserClass.GetComponent<Button> ().interactable = true;
							menuTasks.GetComponent<Button> ().interactable = true;
							dbinterface.getMeineKlassen("Klassen", userid, gameObject);
							break;
		case "btnKurse": 	//show overview of courses
							overviewKlassen.SetActive(false);
							overviewKurse.SetActive(true);
							overviewTasks.SetActive(false);
							menuTeacherClass.GetComponent<Button> ().interactable = true;
							menuUserClass.GetComponent<Button> ().interactable = false;
							menuTasks.GetComponent<Button> ().interactable = true;
							dbinterface.getMeineKurse("Kurse", userid, gameObject);
							break;
		case "btnTasks":	//show overview of tasks
							overviewKlassen.SetActive(false);
							overviewKurse.SetActive(false);
							overviewTasks.SetActive(true);
							menuTeacherClass.GetComponent<Button> ().interactable = true;
							menuUserClass.GetComponent<Button> ().interactable = true;
							menuTasks.GetComponent<Button> ().interactable = false;
							dbinterface.getMeineTasks("Tasks", userid, gameObject);
							break;
		}
	}

	//action performed after clicking on a certain teacherClass, course or task
	public void clickedBtn(string target, int id, string type = ""){
		switch(target){
		case "openTeacherClass":
							//getClassId to load corresponding class
							main.eventHandler("openTeacherClass", id);
							break;
		case "openUserClass":
							//getClassId to load corresponding class
							main.eventHandler("openUserClass", id);
							break;
		case "openTaskForm": switch(type){
			case "Zuordnung": main.eventHandler("openPanelFormAssignment", id);
							break;
			case "Quiz": 	main.eventHandler("openPanelFormQuiz", id);
							break;
			case "Kategorie": main.eventHandler ("openPanelFormCategory", id);
							break;
							}
							break;
		
		}
			
	}

	/*public void openTaskForm(int task_id){
		switch (type) {
		case "Quiz":	main.eventHandler("openPanelFormQuiz", task_id);
						break;
		case "Assignment": main.eventHandler("openPanelFormAssignment", task_id);
						break;
		case "Category": main.eventHandler ("openPanelFormCategory", task_id);
						break;

		}

	}*/

	//called by dbinterface to handle input data: set user data, generate new buttons, delete buttons
	public void dbInputHandler(string[] response){
		GameObject generatedBtn;
		string target = response[0];
		string data = response[1];
		JSONNode parsedData = JSONParser.JSONparse(data);
		Debug.Log ("in dbinputhandler of profile, target "+target);
		switch(target){	
		case "userData": 	//save in user object
							user = new User(parsedData[0]);
							main.setHeaderText(user.getFirstName());
			
							//activate first overview: teacherClasses
							overviewKurse.SetActive(false);
							overviewTasks.SetActive(false);
							//get classes of user
							dbinterface.getMeineKlassen("Klassen", userid, gameObject);
							break;
						
		case "Klassen":		//delete old buttons and clear all references
							teacherClasses.Clear();
							foreach(GameObject b in teacherClassesBtns){
								Destroy(b);
							}	
							teacherClassesBtns.Clear();
			
							//split parsed data into data packages for one teacherClass)
							for(int i = 0; i < parsedData.Count; i++){
								JSONNode n = parsedData[i];
								if(n.Count > 0){
									TeacherClass temp = new TeacherClass(userid,n);
									//add teacherClass to list
									teacherClasses.Add(temp);
									//generate button for teacherClass and add to button list
									generatedBtn = Instantiate(buttonTeacherClass, Vector3.zero, Quaternion.identity) as GameObject;
									generatedBtn.transform.parent = GameObject.Find("ContentKlassen").transform;
									generatedBtn.transform.FindChild("Button/Text").GetComponent<Text>().text = temp.getClassname();
									teacherClassesBtns.Add(generatedBtn);
					
									//set method to be called at onclick event for main button ("Button") and delete button ("ButtonDelete") on button object
									generatedBtn.transform.FindChild("Button").GetComponent<Button>().onClick.AddListener(() => {clickedBtn("openTeacherClass", temp.getClassId());});
									generatedBtn.transform.FindChild("ButtonDelete").GetComponent<Button>().onClick.AddListener(() => {confirmDeleteClass(temp.getClassId());});
								}
							}
							break;

		case "Kurse":		//delete old buttons and clear all references
							userClasses.Clear ();
							foreach(GameObject b in userClassesBtns){
								Destroy(b);
							}	
							userClassesBtns.Clear();

							//generate buttons
							for(int i = 0; i < parsedData.Count; i++){
								JSONNode n = parsedData[i];
								if(n.Count > 0){	
									UserClass temp = new UserClass(userid,n);
									userClasses.Add(temp);
									generatedBtn = Instantiate(buttonUserClass, Vector3.zero, Quaternion.identity) as GameObject;
									generatedBtn.transform.parent = GameObject.Find("ContentKurse").transform;
									generatedBtn.transform.FindChild("Button/Text").GetComponent<Text>().text = temp.getClassname();
									userClassesBtns.Add(generatedBtn);
									//set method to be called at onclick event
									generatedBtn.transform.FindChild("Button").GetComponent<Button>().onClick.AddListener(() => {clickedBtn("openUserClass", temp.getClassId());});
								}
							}
							break;
						
		case "Tasks":		//delete old buttons and clear all references
							tasks.Clear ();
							foreach(GameObject b in tasksBtns){
								Destroy(b);
							}	
							tasksBtns.Clear();

							//generate buttons
							for(int i = 0; i < parsedData.Count; i++){
								JSONNode n = parsedData[i];
								if(n.Count > 0){	
									TaskOverview temp = new TaskOverview(n);
									tasks.Add(temp);
									generatedBtn = Instantiate(buttonTasks, Vector3.zero, Quaternion.identity) as GameObject;
									generatedBtn.transform.parent = GameObject.Find("ContentTasks").transform;
									generatedBtn.transform.FindChild("Button/Text").GetComponent<Text>().text = temp.getTaskName();
									tasksBtns.Add(generatedBtn);
									//set method to be called at onclick event
									generatedBtn.transform.FindChild("Button").GetComponent<Button>().onClick.AddListener(() => {clickedBtn("openTaskForm",temp.getTaskId(), temp.getTypeName());});
					generatedBtn.transform.FindChild("ButtonDelete").GetComponent<Button>().onClick.AddListener(() => {confirmDeleteTask(temp.getTaskId());});
									
								}
							}			
							break;
			
		case "deletedClass":
							if(int.Parse(parsedData[0]["success"]) == 1){ //success
								//refresh overview
								dbinterface.getMeineKlassen("Klassen", userid, gameObject);
							} else {
								main.dbErrorHandler("deleteClass", "Löschen fehlgeschlagen");
							}
							break;
		case "addedClass":	//TODO check if successfull, else: call main.dberrorhandler
							panelCreateClass.SetActive(false);
							dbinterface.getMeineKlassen("Klassen", userid, gameObject);
							break;
		case "addedTask":	
							panelCreateTask.SetActive(false);
							dbinterface.getMeineTasks("Tasks", userid, gameObject);
							break;
		case "registered": 	panelRegistration.SetActive(false);
							//TODO check if successfull, else: call main.dberrorhandler
							dbinterface.getMeineKurse("Kurse", userid, gameObject);
							break;
		}
	}

	//delete profile data at log out
	public void clear(){
		userClasses.Clear ();
		teacherClasses.Clear ();
		userClassesBtns.Clear ();
		teacherClassesBtns.Clear ();
		tasksBtns.Clear ();
		setUserId (0);
	}

	public void showTaskCreation(){
		Debug.Log ("button clicked for show task creation");
		panelCreateTask.SetActive (true);
	}
	
	public void showCreationFormForClass(){
		Debug.Log ("button clicked, show creation form");	
		//activate form to insert class data
		panelCreateClass.SetActive(true);
		//wait until button "create" is clicked (button calls addClass)
	}
	
	public void addClass(){
		//insert class into db if for filled correctly
		//TODO check if filled correctly, else: call main.errorhandler
		//needs validators
		dbinterface.createClass("addedClass", classname.GetComponent<Text>().text, userid, int.Parse (classsubject.GetComponent<Text>().text), classschoolyear.GetComponent<Text>().text, gameObject); 
	}

	public void addTask(){
		//add task into db
		//TODO use ids/names from helper class
		int type = 0;
		if (toggleCategory.isOn) {
			type = 2;
		}
		if (toggleAssignment.isOn) {
			type = 1;
		}
		if (toggleQuiz.isOn) {
			type = 3;
		}

		dbinterface.createTask("addedTask", 
		                       taskname.GetComponent<Text>().text, 
		                       int.Parse(taskpublic.GetComponent<Text>().text), 
		                       userid,
		                       int.Parse(tasksubject.GetComponent<Text>().text), 
		                       type,
		                       gameObject); 
	}
				
	public void confirmDeleteClass(int class_id){
		Debug.Log ("button clicked, try to delete class");	
		main.activateDialogbox ("Do you want to delete this class?", class_id, gameObject, "deleteClass");

	}

	public void deleteClass(int[] temp){
		int answer = temp [0];
		int id = temp [1];
		if (answer == 1) {
			dbinterface.deleteClass ("deletedClass", id, gameObject);
		}
	}

	public void confirmDeleteTask(int topic_id){
		Debug.Log ("button clicked, try to delete task");	
		main.activateDialogbox ("Do you want to delete this task?", topic_id, gameObject, "deleteTask");
	}

	public void deleteTask(int[] temp){
		int answer = temp [0];
		int id = temp [1];
		if (answer == 1) {
			//TODO dbmethod delete topic
			Debug.Log("TODO: call dbmethod for deleting task");
		}
	}
	
	public void showRegisterToClassForm(){
		Debug.Log ("button clicked, show registration form");	
		panelRegistration.SetActive(true);
		//wait until button "register" is clicked (button calls register) 
	}
	
	public void register(){
		Debug.Log ("button clicked, try to register");	
		dbinterface.registerUserToClass("registered", userid, classcode.GetComponent<Text>().text, gameObject);	
	}
	
	public void setUserId(int id){
		userid = id;
	}
}
