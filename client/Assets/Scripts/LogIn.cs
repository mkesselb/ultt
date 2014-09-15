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
		Debug.Log ("LogIn data send");
	}
	
	
	public void dbInputHandler(string[] response){
		string target = response[0];
		string data = response[1];
		switch(target){	
		case "logInData": 	string[] temp = parseJSON(data);
							main.setUserId(int.Parse(temp[1]));
							main.eventHandler("logInSuccess");
							
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
