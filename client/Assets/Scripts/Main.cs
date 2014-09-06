using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class Main : MonoBehaviour {
	
	private DBInterface dbinterface;
	
	//LogIn Screen
	public GameObject panelLogInScreen;
	
	
	//Profile Screen
	public GameObject panelProfile;
	
	

	
	//Objects
	User user;
	
	
	void Start(){
		Debug.Log ("Main: Start()");
		dbinterface = gameObject.GetComponent<DBInterface>();
		
		//activate logInScreen, deactivate rest
		panelLogInScreen.SetActive(true);
		panelProfile.SetActive(false);
		
	}
	
		

	public void eventHandler(string eventname){
		switch(eventname){
		case "logInSuccess": 	panelLogInScreen.SetActive(false);
								panelProfile.SetActive(true);
								break;	
		}
	}
		

	
	//called by dbinterface when received www form contains an error
	public void dbErrorHandler(string target, string errortext){
		Debug.Log ("Error message from db on target "+target+": "+errortext);
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


