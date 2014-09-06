using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
		Debug.Log ("data send");
	}
	
	
	public void dbInputHandler(string[] response){
		string target = response[0];
		string data = response[1];
		switch(target){	
		case "logInData": 	Debug.Log ("data: "+data); 
							if(data == "success"){
								main.eventHandler("logInSuccess");
							} else {
								//TODO ui error message
							}
							break;
		}
	}
	

}
