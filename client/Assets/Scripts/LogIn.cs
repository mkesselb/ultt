using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using SimpleJSON;

public class LogIn : MonoBehaviour {
	
	private Main main;
	private DBInterface dbinterface;
	private Form loginForm;
	
	public Text inputUsername, inputPassword;
	
	void Start(){
		main = GameObject.Find ("Scripts").GetComponent<Main>();
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();

		List<string> keys = new List<string> ();
		keys.Add ("username");
		keys.Add ("password");
		List<GameObject> formFields = new List<GameObject> ();
		formFields.Add (GameObject.Find ("inputUsername"));
		//TODO: hide password of user on ui
		formFields.Add (GameObject.Find ("inputPassword"));
		List<IValidator> formValidaotrs = new List<IValidator> ();
		formValidaotrs.Add (new TextValidator ());
		formValidaotrs.Add (new TextValidator (6, 12));
		loginForm = new Form (keys, formFields, formValidaotrs, new Color(0.75f,0.75f,0.75f,1), Color.red);
	}

	//call dbinterface to send request
	public void clickedBtnLogIn(){
		dbinterface.sendLogInData("logInData", loginForm, gameObject);	
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