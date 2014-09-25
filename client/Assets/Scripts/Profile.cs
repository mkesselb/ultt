using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Profile : MonoBehaviour {
	
	private Main main;
	private DBInterface dbinterface;
	private JSONParser jsonparser;
	
	public GameObject overviewKlassen, overviewKurse, overviewTasks;
	
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
		Debug.Log ("in dbinputhandler of profile");
		GameObject generatedBtn;
		string target = response[0];
		string data = response[1];
		List<string[]> parsedData;
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
		}
	}
	
	public void setUserId(int id){
		userid = id;
	}
	
	
	
}
