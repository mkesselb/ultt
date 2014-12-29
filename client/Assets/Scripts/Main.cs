using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	private ErrorHandler errorhandler;
	private IdHandler idhandler;
	private DBInterface dbinterface;
	private string lang = "de";
	
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

	//panel for Header
	public GameObject panelHeader;

	//objects on panelHeader
	public Text headerText;
	public Text btnBackText;
	public GameObject messagebox;
	public GameObject dialogbox;

	//panels for tasks
	public GameObject panelQuiz;
	public GameObject panelTaskAssignment;
	public GameObject panelTaskCategory;

	//panels for task forms
	public GameObject panelFormQuiz;
	public GameObject panelFormCategory;
	public GameObject panelFormAssign;
	
	//userid
	public int userid;
	
	void Start(){
		dbinterface = gameObject.GetComponent<DBInterface>();
		idhandler = gameObject.GetComponent<IdHandler>();
		idhandler.setupMapping ();
		CSVHelper.addSwap (",", "#csw");
		errorhandler = new ErrorHandler (lang);

		//activate logInScreen, deactivate others
		panelLogInScreen.SetActive(true);
		panelHeader.SetActive (false);
		panelRegister.SetActive(false);
		panelProfile.SetActive(false);
		panelTeacherClass.SetActive(false);
		panelUserClass.SetActive(false);
		panelQuiz.SetActive (false);
		messagebox.SetActive (false);
		dialogbox.SetActive (false);

		panelFormQuiz.SetActive (false);
		panelFormCategory.SetActive (false);
		panelFormAssign.SetActive (false);

		panelTaskAssignment.SetActive (false);
		panelTaskCategory.SetActive (false);
	}

	public void eventHandler(string eventname, int id){
		switch(eventname){
		case "logInSuccess": 	panelLogInScreen.SetActive(false);
								panelHeader.SetActive(true);
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
		case "openPanelFormQuiz": panelFormQuiz.SetActive(true);
								panelFormQuiz.GetComponent<PanelFormQuiz>().setTaskId(id);
								panelFormQuiz.GetComponent<PanelFormQuiz>().init();
								break;
		case "openPanelFormCategory": panelFormCategory.SetActive(true);
								panelFormCategory.GetComponent<PanelFormCategory>().setTaskId(id);
								panelFormCategory.GetComponent<PanelFormCategory>().init();
								break;
		case "openPanelFormAssignment": panelFormAssign.SetActive(true);
								panelFormAssign.GetComponent<PanelFormAssignment>().setTaskId(id);
								panelFormAssign.GetComponent<PanelFormAssignment>().init();
								break;
		case "startTaskQuiz":	panelQuiz.SetActive(true);
								panelQuiz.GetComponent<PanelQuiz>().setTaskId(id);
								panelQuiz.GetComponent<PanelQuiz>().init();
								break;
		case "startTaskAssign":	panelTaskAssignment.SetActive(true);
								panelTaskAssignment.GetComponent<PanelTaskAssignment>().setTaskId(id);
								panelTaskAssignment.GetComponent<PanelTaskAssignment>().init();
								break;
		case "startTaskCategory":
								panelTaskCategory.SetActive(true);
								panelTaskCategory.GetComponent<PanelTaskCategory>().setTaskId(id);
								panelTaskCategory.GetComponent<PanelTaskCategory>().init();
								break;
		}
	}
	
	public void back(){	
				if (panelProfile.activeSelf && panelCreateClass.activeSelf) {
			panelCreateClass.SetActive (false);
			btnBackText.GetComponent<Text> ().text = "zurück";
		} else if (panelProfile.activeSelf && panelCreateTask.activeSelf) {
			panelCreateTask.SetActive (false);
			btnBackText.GetComponent<Text> ().text = "zurück";
		} else if (panelProfile.activeSelf && panelRegistration.activeSelf) {
			panelRegistration.SetActive (false);
			panelLogInScreen.SetActive (true);
			btnBackText.GetComponent<Text> ().text = "zurück";
		} else if (panelRegister.activeSelf) {
			panelRegister.SetActive (false);
			panelLogInScreen.SetActive (true);
			btnBackText.GetComponent<Text> ().text = "zurück";
		} else if (panelTeacherClass.activeSelf && panelStudentList.activeSelf) {
			panelStudentList.SetActive (false);
			btnBackText.GetComponent<Text> ().text = "zurück";
		} else if (panelTeacherClass.activeSelf && !panelStudentList.activeSelf) {
			panelTeacherClass.SetActive (false);
			panelProfile.SetActive (true);
			btnBackText.GetComponent<Text> ().text = "zurück";
		} else if (panelUserClass.activeSelf) {
			panelUserClass.SetActive (false);
			panelProfile.SetActive (true);
			btnBackText.GetComponent<Text> ().text = "zurück";
		} else if (panelQuiz.activeSelf) {
			panelQuiz.SetActive (false);
			btnBackText.GetComponent<Text> ().text = "zurück";
		} else if (panelTaskAssignment.activeSelf) {
			panelTaskAssignment.SetActive (false);
			btnBackText.GetComponent<Text> ().text = "zurück";
		} else if (panelTaskCategory.activeSelf) {
			panelTaskCategory.SetActive (false);
			btnBackText.GetComponent<Text> ().text = "zurück";
		} else if (panelFormQuiz.activeSelf) {
			panelFormQuiz.SetActive (false);
			btnBackText.GetComponent<Text> ().text = "zurück";
		} else if (panelFormAssign.activeSelf) {
			panelFormAssign.SetActive (false);
			btnBackText.GetComponent<Text> ().text = "zurück";
		} else if (panelFormCategory.activeSelf) {
			panelFormCategory.SetActive (false);
			btnBackText.GetComponent<Text> ().text = "zurück";
		} else if (panelProfile.activeSelf && !panelCreateClass.activeSelf && !panelRegistration.activeSelf) {
			panelProfile.SetActive (false);
			panelLogInScreen.SetActive (true);
			panelHeader.SetActive (false);
			panelProfile.GetComponent<Profile> ().clear ();
			btnBackText.GetComponent<Text> ().text = "zurück";
		}
	}
		
	//called by dbinterface when received www form contains an error
	public void dbErrorHandler(string target, string errortext){
		Debug.Log ("Error message from db on target "+target+": "+errortext);

		writeToMessagebox ("Es konnte keine Verbindung zum Server hergstellt werden.");
	}

	//called by dbinterface when an errorcode is returned from server
	public void dbResponseHandler(int errorcode){
		writeToMessagebox(errorhandler.getErrorMessage(errorcode, lang));
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

	public void activateDialogbox(string question, int idOfObjectToDelete, GameObject receiver, string receiverMethod){
		dialogbox.SetActive (true);
		dialogbox.transform.FindChild ("Text").GetComponent<Text> ().text = question;
		dialogbox.transform.FindChild ("ButtonNo/Text").GetComponent<Text> ().text = "no";
		dialogbox.transform.FindChild ("ButtonYes/Text").GetComponent<Text> ().text = "yes";
		dialogbox.transform.FindChild("ButtonNo").GetComponent<Button>().onClick.AddListener(()=> {returnDialogboxResult(0, idOfObjectToDelete,receiver, receiverMethod);});
		dialogbox.transform.FindChild("ButtonYes").GetComponent<Button>().onClick.AddListener(()=> {returnDialogboxResult(1, idOfObjectToDelete, receiver, receiverMethod);});
	}

	public void returnDialogboxResult(int answer, int idOfObject, GameObject receiver, string receiverMethod){
		int[] temp = new int[]{answer, idOfObject};
		receiver.SendMessage(receiverMethod,temp);
		dialogbox.SetActive (false);
	}

	public void setHeaderText(string username){
		panelHeader.transform.FindChild ("Top/Text").GetComponent<Text> ().text = username;
	}

	public void setUserId(int id){
		userid = id;	
	}
	public int getUserId(){
		return userid;	
	}
}