using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.Collections.Generic;

public class Main : MonoBehaviour {
	
	private DBInterface dbinterface;
	
	//LogIn Screen
	public GameObject panelLogInScreen;
	
	//Register Screen
	public GameObject panelRegister;
	
	//Profile Screen
	public GameObject panelProfile;
	
	//TeacherClassScreen
	public GameObject panelTeacherClass;
	
	//UserClassScreen
	public GameObject panelUserClass;
	
	//Panels on profile panel
	public GameObject panelCreateClass, panelRegistration, panelCreateTask;
	
	//panel on panelteacherClass
	public GameObject panelStudentList;

	//objects on panelHeader
	public Text headerText;
	public Text btnBackText;
	public GameObject messagebox;
	
	//userid
	public int userid;

	
	void Start(){
		
		dbinterface = gameObject.GetComponent<DBInterface>();
		
		//activate logInScreen, deactivate others
		panelLogInScreen.SetActive(true);
		panelRegister.SetActive(false);
		panelProfile.SetActive(false);
		panelTeacherClass.SetActive(false);
		panelUserClass.SetActive(false);
		messagebox.SetActive (false);
		
	}
	
		
	public void eventHandler(string eventname, int id){
		switch(eventname){
		case "logInSuccess": 	panelLogInScreen.SetActive(false);
								panelProfile.SetActive(true);
								panelProfile.GetComponent<Profile>().setUserId(id);
								break;	
		case "register":		panelLogInScreen.SetActive(false);
								panelRegister.SetActive(true);
								break;
		case "registered": 		panelRegister.SetActive(false);
								panelLogInScreen.SetActive(true);
								break;
		case "openTeacherClass": panelProfile.SetActive(false);
								panelTeacherClass.SetActive(true);
								panelTeacherClass.GetComponent<PanelTeacherClass>().setClassId(id);
								panelTeacherClass.GetComponent<PanelTeacherClass>().init();
								break;
		case "openUserClass": 	panelProfile.SetActive(false);
								panelUserClass.SetActive(true);
								panelUserClass.GetComponent<PanelUserClass>().setClassId(id);
								panelUserClass.GetComponent<PanelUserClass>().init ();
								break;
		
		}
	}
	
	public void back(){	
		if (panelProfile.activeSelf && panelCreateClass.activeSelf) {
			panelCreateClass.SetActive (false);
		}else if(panelProfile.activeSelf && panelCreateTask.activeSelf){
			panelCreateTask.SetActive(false);
		}else if(panelProfile.activeSelf && panelRegistration.activeSelf){
			panelRegistration.SetActive(false);
			panelLogInScreen.SetActive(true);
		}else if(panelProfile.activeSelf && !panelCreateClass.activeSelf && !panelRegistration.activeSelf){
			panelProfile.SetActive(false);
			panelLogInScreen.SetActive(true);
		}else if(panelRegister.activeSelf){
			panelRegister.SetActive(false);
			panelLogInScreen.SetActive(true);
		}else if(panelTeacherClass.activeSelf && panelStudentList.activeSelf){
			panelStudentList.SetActive(false);
		} else if(panelTeacherClass.activeSelf && !panelStudentList.activeSelf){
			panelTeacherClass.SetActive(false);
			panelProfile.SetActive(true);
		} else if(panelUserClass.activeSelf){
			panelUserClass.SetActive(false);
			panelProfile.SetActive(true);
		} 
								
	}
		
	//called by dbinterface when received www form contains an error
	public void dbErrorHandler(string target, string errortext){
		Debug.Log ("Error message from db on target "+target+": "+errortext);

		writeToMessagebox ("Es konnte keine Verbindung zum Server hergstellt werden.");

	}
	
	public void errorHandler(string target, string errortext){
		string displayedErrorText = "";
		switch(target){
		case "registerFormNotCorrectlyFilled": 	Debug.Log ("Register form not correctly filled: "+errortext);
			displayedErrorText = errortext;
												break;
		
		}
		writeToMessagebox (displayedErrorText);
	}

	public void writeToMessagebox(string text){
		messagebox.transform.FindChild ("Text").GetComponent<Text> ().text = text;
		messagebox.SetActive (true);
	}

	public void deactivateMessagebox(){
		messagebox.SetActive (false);
	}
	
	
	public void setUserId(int id){
		userid = id;	
	}
	public int getUserId(){
		return userid;	
	}
	
	

	
}


