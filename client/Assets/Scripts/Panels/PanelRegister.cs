using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PanelRegister : MonoBehaviour {

	/// <summary>
	/// The main class.
	/// </summary>
	private Main main;

	/// <summary>
	/// The database interface.
	/// </summary>
	private DBInterface dbinterface;

	/// <summary>
	/// The register form.
	/// </summary>
	private Form registerForm;
	
	/// <summary>
	/// The input fields.
	/// </summary>
	public Text inputVorname, inputNachname, inputUsername, inputPassword, inputPassword2, inputEmail, inputSchool;

	void Start () {
		main = GameObject.Find ("Scripts").GetComponent<Main>();
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();

		GameObject.Find ("fieldHeadline").GetComponent<Text> ().text = LocaleHandler.getText ("register-info", main.getLang());
		GameObject.Find ("btnRegister/Text").GetComponent<Text> ().text = LocaleHandler.getText ("register-button", main.getLang());

		List<string> keys = new List<string> ();
		keys.Add ("firstname");
		keys.Add ("lastname");
		keys.Add ("username");
		keys.Add ("password");
		keys.Add ("password2");
		keys.Add ("email");
		keys.Add ("school");
		List<GameObject> formFields = new List<GameObject> ();
		formFields.Add (gameObject.transform.FindChild("inputVorname").gameObject);
		formFields.Add (gameObject.transform.FindChild("inputNachname").gameObject);
		formFields.Add (gameObject.transform.FindChild("inputUsername").gameObject);
		formFields.Add (gameObject.transform.FindChild("inputPassword").gameObject);
		formFields.Add (gameObject.transform.FindChild("inputPassword2").gameObject);
		formFields.Add (gameObject.transform.FindChild("inputEmail").gameObject);
		formFields.Add (gameObject.transform.FindChild("inputSchool").gameObject);
		List<IValidator> formValidaotrs = new List<IValidator> ();
		//TODO: false red paints on errors
		formValidaotrs.Add (new TextValidator ());
		formValidaotrs.Add (new TextValidator ());
		formValidaotrs.Add (new TextValidator ());
		formValidaotrs.Add (new TextValidator (6, 12));
		formValidaotrs.Add (new TextValidator (6, 12));
		formValidaotrs.Add (new TextValidator ());
		formValidaotrs.Add (new TextValidator ());
		registerForm = new Form (keys, formFields, formValidaotrs, new Color(0.75f,0.75f,0.75f,1), Color.red);
	}

	/// <summary>
	/// Saves registration data.
	/// </summary>
	public void clickedBtnRegister(){
		dbinterface.sendRegisterData("register", registerForm, gameObject);
	}

	/// <summary>
	/// Handles incoming data from the database
	/// </summary>
	/// 
	/// <param name="response">response data from the database.</param>
	public void dbInputHandler(string[] response){
		string target = response[0];
		string data = response[1];
		switch(target){	
		case "register":	main.eventHandler("registered", 0);
							break;
		}
	}
}