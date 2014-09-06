using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Profile : MonoBehaviour {
	
	private DBInterface dbinterface;
	
	public GameObject overviewKlassen, overviewKurse, overviewTasks;
	
	public Text fieldUserData;
	
	//Button prefab to dynamically generate buttons
	public Button button;
	
	public int user_id;
	
	public User user;
	
	
	void Start(){
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();
		fieldUserData = GameObject.Find ("fieldUserData").GetComponent<Text>();
		
		overviewKlassen.SetActive(true);
		//TODO get user id: user_id = 
		dbinterface.getUserData("userData", user_id, gameObject);
		
	}
	
	
	public void clickedBtn(string target){
		
		//cases: button of each class, course, task
		switch(target){
		case "btnKlassen":	//show overview of Klassen
							overviewKlassen.SetActive(true);
							overviewKurse.SetActive(false);
							overviewTasks.SetActive(false);
							dbinterface.GetMeineKlassen("Klassen", user_id, gameObject);
							break;
		case "btnKurse": 	//show overview of Kurse
							overviewKlassen.SetActive(false);
							overviewKurse.SetActive(true);
							overviewTasks.SetActive(false);
							dbinterface.GetMeineKurse("Kurse", user_id, gameObject);
							break;
		case "btnTasks":	//show overview of Tasks
							overviewKlassen.SetActive(false);
							overviewKurse.SetActive(false);
							overviewTasks.SetActive(true);
							dbinterface.GetMeineTasks("Tasks", user_id, gameObject);
							break;
		}
	}
	
	
	public void dbInputHandler(string[] response){
		Button generatedBtn;
		string target = response[0];
		string data = response[1];
		switch(target){	
		case "userData": 	//parse received user data and save in user
							user = new User(parseJSON(data));
							//write user data to profile screen
							fieldUserData.text = user.getFirstName()+"\n"+user.getLastName();
			
							//activate first overview: Klassen
							overviewKurse.SetActive(false);
							overviewTasks.SetActive(false);
							//TODO get Klassen of user and write to profile screen
							break;
		case "Klassen":		Debug.Log ("Antwort auf Klassen: "+data);
							//generate button for each Klasse
							//one button
							/*generatedBtn = (Button) Instantiate(button, Vector3.zero, Quaternion.identity);
							generatedBtn.transform.parent = overviewKlassen.transform.FindChild("ContentKlassen");
							generatedBtn.transform.FindChild("Text").GetComponent<Text>().text = "neuer Button";*/
							
						
						
			
							break;
		case "Kurse":		Debug.Log ("Antwort auf Kurse: "+data);
							//generate buttons
							break;
		case "Tasks":		Debug.Log ("Antwort auf Tasks: "+data);
							//generate buttons
							break;
		}
	}
	
	
	
	
	
	//TODO change delimiters
	private string[] parseJSON(string json){
		Debug.Log ("call parse");
		string[] delimiters = { "[{\"", "\":\"", ",\"", "\":", "\",\"", "\"}]", "}]" };
        string[] temp = new string[30];
		Debug.Log ("try start parse");
		temp = json.Split(delimiters,System.StringSplitOptions.RemoveEmptyEntries);
		Debug.Log ("parse finished");
	
		for (int i = 0; i< 14; i++){
			Debug.Log ("data "+ i + ": "+ temp[i]);

		}
		
		return temp;
	}
}
