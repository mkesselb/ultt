using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

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

	private List<GameObject> panelStack;
	
	void Start(){
		dbinterface = gameObject.GetComponent<DBInterface>();
		idhandler = gameObject.GetComponent<IdHandler>();
		idhandler.setupMapping ();
		CSVHelper.addSwap (",", "#csw");
		errorhandler = new ErrorHandler (lang);
		LocaleHandler.setupMapping (lang);

		//activate logInScreen, deactivate others
		panelLogInScreen.SetActive(true);
		panelHeader.SetActive (true);
		panelHeader.transform.FindChild ("Top").gameObject.SetActive (false);
		panelHeader.transform.FindChild ("btnBack").gameObject.SetActive (false);
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

		panelStack = new List<GameObject> ();
		panelStack.Add (panelLogInScreen);



		//dbinterface.getResultOfStudent("studentlistDetail", 15, student_id, 0, gameObject);
		//dbinterface.getResultOfStudents ("examResults", teacherClass.getClassId (), 0, gameObject);

	}

	public void eventHandler(string eventname, int id, int id2 = 0, bool isTeacher = false){
		switch(eventname){
		case "logInSuccess": 	//panelLogInScreen.SetActive(false);
								panelHeader.transform.FindChild ("Top").gameObject.SetActive (true);
								panelHeader.transform.FindChild ("btnBack").gameObject.SetActive (true);
								panelProfile.SetActive(true);
								panelStack.Add(panelProfile);
								panelProfile.GetComponent<Profile>().setUserId(id);
								this.setUserId(id);
								break;	
		case "register":		//panelLogInScreen.SetActive(false);
								panelRegister.SetActive(true);
								panelStack.Add(panelRegister);
								panelHeader.transform.FindChild ("Top").gameObject.SetActive (true);
								panelHeader.transform.FindChild ("btnBack").gameObject.SetActive (true);
								break;
		case "registered": 		
								back();
								/*panelRegister.SetActive(false);
								panelLogInScreen.SetActive(true);*/
								break;
		case "openTeacherClass":
								panelStack.Add(panelTeacherClass);
								panelProfile.SetActive(false);
								panelTeacherClass.SetActive(true);
								panelTeacherClass.GetComponent<PanelTeacherClass>().setClassId(id);
								panelTeacherClass.GetComponent<PanelTeacherClass>().init();
								break;
		case "openUserClass": 	
								panelStack.Add(panelUserClass);
								panelProfile.SetActive(false);
								panelUserClass.SetActive(true);
								panelUserClass.GetComponent<PanelUserClass>().setClassId(id);
								panelUserClass.GetComponent<PanelUserClass>().init ();
								break;
		case "openPanelFormQuiz":
								panelStack.Add(panelFormQuiz);
								panelFormQuiz.SetActive(true);
								panelFormQuiz.GetComponent<PanelFormQuiz>().setTaskId(id);
								panelFormQuiz.GetComponent<PanelFormQuiz>().init();
								break;
		case "openPanelFormCategory": 
								panelStack.Add(panelFormCategory);
								panelFormCategory.SetActive(true);
								panelFormCategory.GetComponent<PanelFormCategory>().setTaskId(id);
								panelFormCategory.GetComponent<PanelFormCategory>().init();
								break;
		case "openPanelFormAssignment": 
								panelStack.Add(panelFormAssign);
								panelFormAssign.SetActive(true);
								panelFormAssign.GetComponent<PanelFormAssignment>().setTaskId(id);
								panelFormAssign.GetComponent<PanelFormAssignment>().init();
								break;
		case "startTaskQuiz":	
								panelStack.Add(panelQuiz);
								panelQuiz.SetActive(true);
								Debug.Log (id + ";" + userid + ";" + id2);
								panelQuiz.GetComponent<PanelQuiz>().setTaskId(id);
								panelQuiz.GetComponent<PanelQuiz>().setUserId(userid);
								panelQuiz.GetComponent<PanelQuiz>().setTaskForClassId(id2);
								panelQuiz.GetComponent<PanelQuiz>().setIsTeacher(isTeacher);

								panelQuiz.GetComponent<PanelQuiz>().init();
								break;
		case "startTaskAssign":	
								panelStack.Add(panelTaskAssignment);
								panelTaskAssignment.SetActive(true);
								Debug.Log (id + ";" + userid + ";" + id2);
								panelTaskAssignment.GetComponent<PanelTaskAssignment>().setTaskId(id);
								panelTaskAssignment.GetComponent<PanelTaskAssignment>().setUserId(userid);
								panelTaskAssignment.GetComponent<PanelTaskAssignment>().setTaskForClassId(id2);
								panelTaskAssignment.GetComponent<PanelTaskAssignment>().setIsTeacher(isTeacher);

								panelTaskAssignment.GetComponent<PanelTaskAssignment>().init();
								break;
		case "startTaskCategory":
								panelStack.Add(panelTaskCategory);
								panelTaskCategory.SetActive(true);
								Debug.Log (id + ";" + userid + ";" + id2);
								panelTaskCategory.GetComponent<PanelTaskCategory>().setTaskId(id);
								panelTaskCategory.GetComponent<PanelTaskCategory>().setUserId(userid);
								panelTaskCategory.GetComponent<PanelTaskCategory>().setTaskForClassId(id2);
								panelTaskCategory.GetComponent<PanelTaskCategory>().setIsTeacher(isTeacher);

								panelTaskCategory.GetComponent<PanelTaskCategory>().init();
								break;
		case "finishTask":		back();
								//panelQuiz.SetActive (false);
								break;

		}
	}
	
	public void back(){	
		int c = panelStack.Count-1;
		panelStack [c].SetActive (false);
		panelStack.RemoveAt (c);
		panelStack [c - 1].SetActive (true);
		if (panelStack [c - 1] == panelLogInScreen) {
			panelHeader.transform.FindChild ("Top").gameObject.SetActive (false);
			panelHeader.transform.FindChild ("btnBack").gameObject.SetActive (false);
		}
	}
		
	//called by dbinterface when received www form contains an error
	public void dbErrorHandler(string target, string errortext){
		Debug.Log ("Error message from db on target "+target+": "+errortext);

		writeToMessagebox (LocaleHandler.getText("noserver-connection", lang));
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
		dialogbox.transform.FindChild ("ButtonNo/Text").GetComponent<Text> ().text = LocaleHandler.getText("dialog-no", lang);
		dialogbox.transform.FindChild ("ButtonYes/Text").GetComponent<Text> ().text = LocaleHandler.getText("dialog-yes", lang);
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

	public string getLang(){
		return lang;
	}

	public void addToPanelStack(GameObject g){
		this.panelStack.Add (g);
	}
}