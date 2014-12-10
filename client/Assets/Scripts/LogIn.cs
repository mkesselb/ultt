using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using SimpleJSON;

public class LogIn : MonoBehaviour {
	
	private Main main;
	private DBInterface dbinterface;
	
	public Text inputUsername, inputPassword;
	
	void Start(){
		Debug.Log ("LogIn: Start()");
		main = GameObject.Find ("Scripts").GetComponent<Main>();
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();
	}

	//call dbinterface to send request
	public void clickedBtnLogIn(){
		Debug.Log("LogIn Button clicked");
		dbinterface.sendLogInData("logInData", inputUsername.text, inputPassword.text, gameObject);	
		Debug.Log ("LogIn data send");
	}
	
	public void clickedBtnRegister(){
		main.eventHandler("register",0);	
	}
	
	
	public void dbInputHandler(string[] response){
		string target = response[0];
		string data = response[1];
		JSONNode parsedData = JSONParser.JSONparse(data);
		switch(target){	
		case "logInData": 	
							JSONNode user = parsedData[0];
							main.eventHandler("logInSuccess", int.Parse (user["user_id"]));
							break;
		}
	}
}