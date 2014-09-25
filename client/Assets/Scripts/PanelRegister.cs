using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelRegister : MonoBehaviour {
	
	private Main main;
	private DBInterface dbinterface;
	
	//Input fields
	public Text inputVorname, inputNachname, inputUsername, inputPassword, inputPassword2, inputEmail, inputSchool;


	void Start () {
	
		main = GameObject.Find ("Scripts").GetComponent<Main>();
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();
	}
	
	public void clickedBtnRegister(){
		if(inputVorname.text == "Vorname" || inputNachname.text == "Nachname" || inputUsername.text == "Username" || inputPassword.text == "Passwort" || inputPassword2.text == "Passwort erneut eingeben" || inputEmail.text == "Email" || inputSchool.text == "Schule"){
			Debug.Log ("error");
			main.errorHandler("registerFormNotCorrectlyFilled", "Ein Feld wurde nicht richtig ausgefüllt.");
		} else if (inputPassword.text != inputPassword2.text) {
			Debug.Log("error2");
			main.errorHandler("registerFormNotCorrectlyFilled", "Der Inhalt der Passwort-Felder stimmt nicht überein.");
		} else {
			WWWForm form = new WWWForm();
			form.AddField("method", "register"); //TODO change to db method
			form.AddField("",inputVorname.text);
			form.AddField("",inputNachname.text);
			form.AddField("",inputUsername.text);
			form.AddField("",inputPassword.text);
			form.AddField("",inputEmail.text);
			form.AddField("",inputSchool.text);	
			dbinterface.sendRegisterData("register", form, gameObject);	
		}
	}
	
	public void dbInputHandler(string[] response){
		string target = response[0];
		string data = response[1];
		switch(target){	
		case "register":	Debug.Log ("Register erfolgreich"); 
							main.eventHandler("registered", 0);
							break;
		}
	}
}
