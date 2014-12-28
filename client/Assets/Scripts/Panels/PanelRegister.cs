using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PanelRegister : MonoBehaviour {
	
	private Main main;
	private DBInterface dbinterface;
	private Form registerForm;
	
	//Input fields
	public Text inputVorname, inputNachname, inputUsername, inputPassword, inputPassword2, inputEmail, inputSchool;

	void Start () {
		main = GameObject.Find ("Scripts").GetComponent<Main>();
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();

		List<string> keys = new List<string> ();
		keys.Add ("firstname");
		keys.Add ("lastname");
		keys.Add ("username");
		keys.Add ("password");
		keys.Add ("password2");
		keys.Add ("email");
		keys.Add ("school");
		List<GameObject> formFields = new List<GameObject> ();
		formFields.Add (GameObject.Find ("inputVorname"));
		formFields.Add (GameObject.Find ("inputNachname"));
		formFields.Add (GameObject.Find ("inputUsername"));
		formFields.Add (GameObject.Find ("inputPassword"));
		formFields.Add (GameObject.Find ("inputPassword2"));
		formFields.Add (GameObject.Find ("inputEmail"));
		formFields.Add (GameObject.Find ("inputSchool"));
		List<IValidator> formValidaotrs = new List<IValidator> ();
		formValidaotrs.Add (new TextValidator ());
		formValidaotrs.Add (new TextValidator ());
		formValidaotrs.Add (new TextValidator ());
		formValidaotrs.Add (new TextValidator (6, 12));
		formValidaotrs.Add (new TextValidator (6, 12));
		formValidaotrs.Add (new TextValidator ());
		formValidaotrs.Add (new TextValidator ());
		registerForm = new Form (keys, formFields, formValidaotrs, new Color(0.75f,0.75f,0.75f,1), Color.red);
	}
	
	public void clickedBtnRegister(){
		dbinterface.sendRegisterData("register", registerForm, gameObject);
	}
	
	public void dbInputHandler(string[] response){
		string target = response[0];
		string data = response[1];
		switch(target){	
		case "register":	main.eventHandler("registered", 0);
							break;
		}
	}
}