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
	public GameObject panelCreateTask;
	//objects on creation form
	public Text classname, classsubject, classschoolyear;
	//objects on task creation form
	public Text taskname, tasksubject, tasktype, taskpublic;
	//registration form (to register to existing class via classcode)
	public GameObject panelRegistration;
	//object on registration form
	public Text classcode;
	
	public Text fieldUserData;
	
	//Button prefab to dynamically generate buttons
	public GameObject button, buttonUserClass, buttonTeacherClass, buttonTasks;
	
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
							dbinterface.getMeineKlassen("Klassen", userid, gameObject);
							break;
		case "btnKurse": 	//show overview of courses
							overviewKlassen.SetActive(false);
							overviewKurse.SetActive(true);
							overviewTasks.SetActive(false);
							dbinterface.getMeineKurse("Kurse", userid, gameObject);
							break;
		case "btnTasks":	//show overview of tasks
							overviewKlassen.SetActive(false);
							overviewKurse.SetActive(false);
							overviewTasks.SetActive(true);
							dbinterface.getMeineTasks("Tasks", userid, gameObject);
							break;
		}
	}

	//action performed after clicking on a certain teacherClass, course or task
	public void clickedBtn(string target, int id){
		switch(target){
		case "btnTeacherClasses":
							Debug.Log ("clicked in teacherClassesButton with classid: "+id);
							//getClassId to load corresponding class
							main.eventHandler("openTeacherClass", id);
							break;
		case "btnUserClasses":
							Debug.Log ("clicked in userClassesButton with classid: "+id);
							//getClassId to load corresponding class
							main.eventHandler("openUserClass", id);
							break;
		case "btnTasks":
							Debug.Log ("clicked in taskButton with taskid: "+id);
							//open taskedit form
							main.eventHandler("openTaskEdit", id);
							break;
		}
			
	}

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
							//write user data to profile screen
							//fieldUserData.text = user.getFirstName()+"\n"+user.getLastName();
			
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
									generatedBtn.transform.FindChild("Button").GetComponent<Button>().onClick.AddListener(() => {clickedBtn("btnTeacherClasses", temp.getClassId());});
									generatedBtn.transform.FindChild("ButtonDelete").GetComponent<Button>().onClick.AddListener(() => {deleteClass(temp.getClassId());});
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
									generatedBtn.transform.FindChild("Button").GetComponent<Button>().onClick.AddListener(() => {clickedBtn("btnUserClasses", temp.getClassId());});
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
									generatedBtn.transform.FindChild("Button").GetComponent<Button>().onClick.AddListener(() => {clickedBtn("btnTasks", temp.getTaskId());});
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
		dbinterface.createTask("addedTask", 
		                       taskname.GetComponent<Text>().text, 
		                       int.Parse(taskpublic.GetComponent<Text>().text), 
		                       userid,
		                       int.Parse(tasksubject.GetComponent<Text>().text), 
		                       int.Parse (tasktype.GetComponent<Text>().text),
		                       gameObject); 
	}
				
	public void deleteClass(int class_id){
		Debug.Log ("button clicked, try to delete class");	
		//delete class
		dbinterface.deleteClass("deletedClass", class_id, gameObject);
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
