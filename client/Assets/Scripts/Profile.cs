using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Profile : MonoBehaviour {
	
	private Main main;
	private DBInterface dbinterface;
	
	public GameObject overviewKlassen, overviewKurse, overviewTasks;
	
	public Text fieldUserData;
	
	//Button prefab to dynamically generate buttons
	public Button button;
	
	public int userid;
	public User user;
	public List<UserClass> userClasses;
	public List<TeacherClass> teacherClasses;
	
	
	
	void Start(){
		main = GameObject.Find ("Scripts").GetComponent<Main>();
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();
		fieldUserData = GameObject.Find ("fieldUserData").GetComponent<Text>();
		
		overviewKlassen.SetActive(true);
		
		userid = main.getUserId();
		userClasses = new List<UserClass>();
		teacherClasses = new List<TeacherClass>();
		
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
	
	
	public void dbInputHandler(string[] response){
		Button generatedBtn;
		string target = response[0];
		string data = response[1];
		switch(target){	
		case "userData": 	//parse received user data and save in user
							user = new User(userid, parseJSON(data));
							//write user data to profile screen
							fieldUserData.text = user.getFirstName()+"\n"+user.getLastName();
			
							//activate first overview: Klassen
							overviewKurse.SetActive(false);
							overviewTasks.SetActive(false);
							//TODO get Klassen of user and write to profile screen
							dbinterface.getMeineKlassen("Klassen", userid, gameObject);
							break;
						
		case "Klassen":		//TODO generate button for each Klasse, yet only for first
							string[] parsedData = parseJSON(data);
							
							TeacherClass temp = new TeacherClass(userid,parsedData);
							teacherClasses.Add(temp);
							generatedBtn = (Button) Instantiate(button, Vector3.zero, Quaternion.identity);
							generatedBtn.transform.parent = overviewKlassen.transform.FindChild("ContentKlassen");
							generatedBtn.transform.FindChild("Text").GetComponent<Text>().text = temp.getClassname();
						
							break;
						
						
			
							break;
		case "Kurse":		//generate buttons
							break;
		case "Tasks":		//generate buttons
							break;
		}
	}
	
	
	
	
	
	//TODO change delimiters
	private string[] parseJSON(string json){
		Debug.Log ("call parse");
		string[] delimiters = { "[{\"", "\":\"", ",\"", "\":", "\",\"", "\"}]", "}]", "}" };
        string[] temp = new string[30];
		Debug.Log ("try start parse");
		temp = json.Split(delimiters,System.StringSplitOptions.RemoveEmptyEntries);
		Debug.Log ("parse finished");
		return temp;
	}
}
