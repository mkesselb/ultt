using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Profile : MonoBehaviour {
	
	private Main main;
	private DBInterface dbinterface;
	private JSONParser jsonparser;
	
	//Objects on "normal" view
	public GameObject overviewKlassen, overviewKurse, overviewTasks;
	//creation form (to create new class)
	public GameObject panelCreateClass;
	//objects on creation form
	public Text classname, classsubject, classschoolyear;
	//registration form (to register to existing class via classcode)
	public GameObject panelRegistration;
	//object on registration form
	public Text classcode;
	
	public Text fieldUserData;
	
	//Button prefab to dynamically generate buttons
	public GameObject button, buttonUserClass, buttonTeacherClass;
	
	public int userid;
	public User user;
	public List<UserClass> userClasses;
	public List<TeacherClass> teacherClasses;
	public List<GameObject> userClassesBtns, teacherClassesBtns;
	
	
	
	void Start(){
		main = GameObject.Find ("Scripts").GetComponent<Main>();
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();
		jsonparser = GameObject.Find ("Scripts").GetComponent<JSONParser>();
		fieldUserData = GameObject.Find ("fieldUserData").GetComponent<Text>();
		
		panelCreateClass.SetActive(false);
		panelRegistration.SetActive(false);
		overviewKlassen.SetActive(true);
		
		
		//userid = main.getUserId();
		userClasses = new List<UserClass>();
		teacherClasses = new List<TeacherClass>();
		userClassesBtns = new List<GameObject>();
		teacherClassesBtns = new List<GameObject>();
		
		Debug.Log ("Send request for user data");
		dbinterface.getUserData("userData", userid, gameObject);
	
	}
	
	
	public void clickedBtn(string target){
		
		//cases: button of each class, course, task
		switch(target){
		case "btnKlassen":	//show overview of Klassen
							overviewKlassen.SetActive(true);
							overviewKurse.SetActive(false);
							overviewTasks.SetActive(false);
							dbinterface.getMeineKlassen("Klassen", userid, gameObject);
							break;
		case "btnKurse": 	//show overview of Kurse
							overviewKlassen.SetActive(false);
							overviewKurse.SetActive(true);
							overviewTasks.SetActive(false);
							dbinterface.getMeineKurse("Kurse", userid, gameObject);
							break;
		case "btnTasks":	//show overview of Tasks
							overviewKlassen.SetActive(false);
							overviewKurse.SetActive(false);
							overviewTasks.SetActive(true);
							dbinterface.getMeineTasks("Tasks", userid, gameObject);
							break;
		}
	}

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
		}
			
	}
	
	

	
	
	
	public void dbInputHandler(string[] response){
		GameObject generatedBtn;
		string target = response[0];
		string data = response[1];
		List<string[]> parsedData;
		Debug.Log ("in dbinputhandler of profile, target "+target);
		switch(target){	
		case "userData": 	//parse received user data and save in user
							parsedData = jsonparser.JSONparse(data);
							user = new User(userid, parsedData[0]);
							//write user data to profile screen
							fieldUserData.text = user.getFirstName()+"\n"+user.getLastName();
			
							//activate first overview: Klassen
							overviewKurse.SetActive(false);
							overviewTasks.SetActive(false);
							//get Klassen of user
							dbinterface.getMeineKlassen("Klassen", userid, gameObject);
							break;
						
		case "Klassen":		//generate a button for each Klasse = teacherClass
			
							//delete old buttons and clear all references
							teacherClasses.Clear();
							foreach(GameObject b in teacherClassesBtns){
								Destroy(b);
							}	
							teacherClassesBtns.Clear();
							//parse data to string array
							parsedData = jsonparser.JSONparse(data);
			
							//split parsed data into data packages for one object (=TeacherClass)
							//for (int i = 0; i<parsedData.Length/12; i++){
							foreach (string[] s in parsedData){
								/*for (int j = 0; j<s.Length; j++){
									Debug.Log(s[j]);
								}
								Debug.Log ("---------------");*/
								//add object to object list
								TeacherClass temp = new TeacherClass(userid,s);
								teacherClasses.Add(temp);
								//generate button and add to button list
								generatedBtn = Instantiate(buttonTeacherClass, Vector3.zero, Quaternion.identity) as GameObject;
								generatedBtn.transform.parent = GameObject.Find("ContentKlassen").transform;
								generatedBtn.transform.FindChild("Text").GetComponent<Text>().text = temp.getClassname();
								teacherClassesBtns.Add(generatedBtn);
				
								//set method to be called at onclick event
								generatedBtn.GetComponent<Button>().onClick.AddListener(() => {clickedBtn("btnTeacherClasses", temp.getClassId());});
								generatedBtn.transform.FindChild("ButtonDelete").GetComponent<Button>().onClick.AddListener(() => {deleteClass(temp.getClassId());});			
			}
							break;
						
						
			
							break;
		case "Kurse":		//generate buttons
			
							//delete old buttons and clear all references
							userClasses.Clear ();
							foreach(GameObject b in userClassesBtns){
								Destroy(b);
							}	
							userClassesBtns.Clear();
							parsedData = jsonparser.JSONparse(data);
			
							foreach (string[] s in parsedData){
								/*for (int j = 0; j<s.Length; j++){
									Debug.Log(s[j]);
								}
								Debug.Log ("---------------");*/
								UserClass temp = new UserClass(userid,s);
								userClasses.Add(temp);
								generatedBtn = Instantiate(buttonUserClass, Vector3.zero, Quaternion.identity) as GameObject;
								generatedBtn.transform.parent = GameObject.Find("ContentKurse").transform;
								generatedBtn.transform.FindChild("Text").GetComponent<Text>().text = temp.getClassname();
								userClassesBtns.Add(generatedBtn);
								//set method to be called at onclick event
								generatedBtn.GetComponent<Button>().onClick.AddListener(() => {clickedBtn("btnUserClasses", temp.getClassId());});
							}
							break;
						
		case "Tasks":		//generate buttons
							break;
			
		case "deletedClass":parsedData = jsonparser.JSONparse(data);
							string[] result = parsedData[0];
							if(result[1] == "1"){ //success
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
		case "registered": 	panelRegistration.SetActive(false);
							//TODO check if successfull, else: call main.dberrorhandler
							dbinterface.getMeineKurse("Kurse", userid, gameObject);
							break;
		}
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
		dbinterface.createClass("addedClass", classname.GetComponent<Text>().text, userid, int.Parse (classsubject.GetComponent<Text>().text), classschoolyear.GetComponent<Text>().text, gameObject); 
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
