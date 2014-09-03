using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class Main : MonoBehaviour {
	
	private DBInterface dbinterface;
	
	
	//LogIn Screen
	private GameObject panelLogInScreen;
	public Text inputUsername, inputPassword;
	
	//Profile Screen
	private GameObject panelProfile;
	public Text fieldUserData;
	
	//Courses Screen
	private GameObject panelMyCourses;
	
	//Objects
	User user;
	
	
	void Start(){
		dbinterface = gameObject.GetComponent<DBInterface>();
		
		inputUsername = GameObject.Find("inputUsername/Text").GetComponent<Text>();	
		inputPassword = GameObject.Find("inputPassword/Text").GetComponent<Text>();	
		fieldUserData = GameObject.Find ("fieldUserData").GetComponent<Text>();
		
		panelLogInScreen = GameObject.Find("panelLogInScreen");
		panelProfile = GameObject.Find ("panelProfile");
		panelMyCourses = GameObject.Find("panelMyCourses");
		
		panelProfile.SetActive(false);
		panelLogInScreen.SetActive(true);
		panelMyCourses.SetActive(false);
		
		
		
	}
	
	
	
		
	//called by dbinterface when received www form contains valid data (= no error)
	public void dbInputHandler(string target, string data){
		switch(target){	
		case "logInData": 	Debug.Log ("data: "+data); 
							if(data == "success"){
								panelLogInScreen.SetActive(false);
								prepareProfileScreen(data);
								panelProfile.SetActive(true);
							} else {
								inputUsername.text = data;
							}
							break;
		case "myCourses":	Debug.Log ("data: "+data);
							panelProfile.SetActive(false);		
							panelMyCourses.SetActive(true);
							break;
			
		}
	}
	
	//called by dbinterface when received www form contains an error
	public void dbErrorHandler(string target, string errortext){
		switch(target){	
		case "logInData": 	Debug.Log ("error: "+errortext); break;
		case "myCourses":	Debug.Log ("error: "+errortext); break;	
			
		}
	}
	
	
	public void prepareProfileScreen(string data){
		Debug.Log("prepare Profile");
		user = new User(parseJSON(data));
		fieldUserData.text = user.getFirstName()+"\n"+user.getLastName();
		
	}
	
	
	
	//funtions called by BUTTONS
	public void clickedBtnLogIn(){
		Debug.Log("Button clicked");
		dbinterface.sendLogInData("logInData", inputUsername.text, inputPassword.text);	
	}
	
	public void clickedBtnMyCourses(){
		Debug.Log ("Button clicked");
		dbinterface.GetMyCourses("myCourses", user.getUserId());	
	}
	

	//TODO change delimiters
	//TODO delete empty fields in array
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


