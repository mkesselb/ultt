using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using SimpleJSON;

public class Profile : MonoBehaviour {
	/// <summary>
	/// The main class.
	/// </summary>
	private Main main;

	/// <summary>
	/// The database interface.
	/// </summary>
	private DBInterface dbinterface;

	/// <summary>
	/// The id handler
	/// </summary>
	private IdHandler idhandler;
	
	/// <summary>
	/// The sub panels.
	/// </summary>
	public GameObject overviewKlassen, overviewKurse, overviewTasks;

	/// <summary>
	/// The panel with form to create a class.
	/// </summary>
	public GameObject panelCreateClass;

	/// <summary>
	/// The subject toggle in the form.
	/// </summary>
	public GameObject subjectToggle;

	/// <summary>
	/// The form to create a class.
	/// </summary>
	private Form createClassForm;

	/// <summary>
	/// The class text fields.
	/// </summary>
	public Text classname, classschoolyear;

	/// <summary>
	/// The panel with form t create task.
	/// </summary>
	public GameObject panelCreateTask;

	/// <summary>
	/// The form to create a task.
	/// </summary>
	private Form createTaskForm;

	/// <summary>
	/// The textfields on the task creation form.
	/// </summary>
	public Text taskname, taskpublic;

	/// <summary>
	/// The toggles on the task creation form.
	/// </summary>
	public Toggle toggleAssignment, toggleQuiz, toggleCategory;

	/// <summary>
	/// The panel with registration form.
	/// </summary>
	public GameObject panelRegistration;

	/// <summary>
	/// The textfield for the class code on creation form.
	/// </summary>
	public Text classcode;

	/// <summary>
	/// The textfield for the user data.
	/// </summary>
	public Text fieldUserData;
	
	/// <summary>
	/// The button prefabs.
	/// </summary>
	public GameObject button, buttonUserClass, buttonTeacherClass, buttonTasks;

	/// <summary>
	/// The menu buttons.
	/// </summary>
	public GameObject menuUserClass, menuTeacherClass, menuTasks;

	/// <summary>
	/// The user_id.
	/// </summary>
	public int userid;

	/// <summary>
	/// The user object.
	/// </summary>
	public User user;

	/// <summary>
	/// The list of user classes.
	/// </summary>
	public List<UserClass> userClasses;

	/// <summary>
	/// The list of teacher classes.
	/// </summary>
	public List<TeacherClass> teacherClasses;

	/// <summary>
	/// The list of tasks.
	/// </summary>
	public List<TaskOverview> tasks;

	/// <summary>
	/// The lists of buttons.
	/// </summary>
	public List<GameObject> userClassesBtns, teacherClassesBtns, tasksBtns;

	void Start(){
		main = GameObject.Find ("Scripts").GetComponent<Main>();
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();
		idhandler = GameObject.Find ("Scripts").GetComponent<IdHandler>();
		//fieldUserData = GameObject.Find ("fieldUserData").GetComponent<Text>();
		
		//only teacherClasses view is actve first
		panelCreateClass.SetActive(false);
		panelCreateTask.SetActive (false);
		panelRegistration.SetActive(false);
		overviewKlassen.SetActive(true);
		menuTeacherClass.GetComponent<Button> ().interactable = false;
		menuUserClass.GetComponent<Button> ().interactable = true;
		menuTasks.GetComponent<Button> ().interactable = true;

		//set text to correct locale
		GameObject.Find ("btnKlassen/Text").GetComponent<Text> ().text = LocaleHandler.getText ("class-tab", main.getLang());
		GameObject.Find ("btnKurse/Text").GetComponent<Text> ().text = LocaleHandler.getText ("course-tab", main.getLang());
		GameObject.Find ("btnTasks/Text").GetComponent<Text> ().text = LocaleHandler.getText ("task-tab", main.getLang());
		gameObject.transform.FindChild("OverviewKlassen/ContentKlassen/ButtonAdd/Text").GetComponent<Text> ().text 
			= LocaleHandler.getText ("add-class", main.getLang());
		gameObject.transform.FindChild("OverviewKurse/ContentKurse/ButtonAdd/Text").GetComponent<Text> ().text 
			= LocaleHandler.getText ("add-course", main.getLang());
		gameObject.transform.FindChild("OverviewTasks/ContentTasks/ButtonAdd/Text").GetComponent<Text> ().text 
			= LocaleHandler.getText ("add-task", main.getLang());

		//create class form localization
		gameObject.transform.FindChild("panelCreateClass/fieldHeadline").GetComponent<Text> ().text 
			= LocaleHandler.getText ("class-add-info", main.getLang());
		gameObject.transform.FindChild("panelCreateClass/textClassName").GetComponent<Text> ().text 
			= LocaleHandler.getText ("class-name", main.getLang());
		gameObject.transform.FindChild("panelCreateClass/textYear").GetComponent<Text> ().text 
			= LocaleHandler.getText ("class-year", main.getLang());
		gameObject.transform.FindChild("panelCreateClass/textSubject").GetComponent<Text> ().text 
			= LocaleHandler.getText ("class-subject", main.getLang());
		gameObject.transform.FindChild("panelCreateClass/btnCreate/Text").GetComponent<Text> ().text 
			= LocaleHandler.getText ("button-add-class", main.getLang());

		//course registration form localization
		gameObject.transform.FindChild("panelRegisterToClass/Text").GetComponent<Text> ().text 
			= LocaleHandler.getText ("course-info1", main.getLang()) + "\n" 
				+ LocaleHandler.getText ("course-info2", main.getLang()) + "\n" 
				+ LocaleHandler.getText ("course-info3", main.getLang());
		gameObject.transform.FindChild("panelRegisterToClass/fieldHeadline").GetComponent<Text> ().text 
			= LocaleHandler.getText ("course-code-info", main.getLang());
		gameObject.transform.FindChild("panelRegisterToClass/btnRegister/Text").GetComponent<Text> ().text 
			= LocaleHandler.getText ("button-add-course", main.getLang());

		//task creation form localization
		gameObject.transform.FindChild("panelCreateTask/fieldHeadline").GetComponent<Text> ().text 
			= LocaleHandler.getText ("task-add-info", main.getLang());
		gameObject.transform.FindChild("panelCreateTask/textName").GetComponent<Text> ().text 
			= LocaleHandler.getText ("task-name", main.getLang());
		//privacy not supported now
		//gameObject.transform.FindChild("panelCreateTask/textPrivacy").GetComponent<Text> ().text = LocaleHandler.getText ("task-privacy", main.getLang());
		gameObject.transform.FindChild("panelCreateTask/textSubject").GetComponent<Text> ().text 
			= LocaleHandler.getText ("task-subject", main.getLang());
		gameObject.transform.FindChild("panelCreateTask/textType").GetComponent<Text> ().text 
			= LocaleHandler.getText ("tasktype", main.getLang());
		gameObject.transform.FindChild("panelCreateTask/btnCreate/Text").GetComponent<Text> ().text 
			= LocaleHandler.getText ("button-add-task", main.getLang());

		//userid = main.getUserId();
		userClasses = new List<UserClass>();
		teacherClasses = new List<TeacherClass>();
		tasks = new List<TaskOverview> ();
		userClassesBtns = new List<GameObject>();
		teacherClassesBtns = new List<GameObject>();
		tasksBtns = new List<GameObject>();
		
		Debug.Log ("Send request for user data");
		dbinterface.getUserData("userData", userid, gameObject);
		initCreateClassForm ();
		initCreateTaskForm ();
	}

	public void initPanel(){
		//shows a default class view
		overviewKlassen.SetActive(true);
		overviewKurse.SetActive(false);
		overviewTasks.SetActive(false);
		menuTeacherClass.GetComponent<Button> ().interactable = false;
		menuUserClass.GetComponent<Button> ().interactable = true;
		menuTasks.GetComponent<Button> ().interactable = true;
		dbinterface.getMeineKlassen("Klassen", userid, gameObject);
	}

	// <summary>
	/// Initialize the form for creating tasks.
	/// </summary>
	private void initCreateTaskForm(){
		List<string> keys = new List<string> ();
		keys.Add ("taskname");
		List<GameObject> formFields = new List<GameObject> ();
		formFields.Add (panelCreateTask.transform.Find("inputName").gameObject);
		List<IValidator> formValidators = new List<IValidator> ();
		formValidators.Add (new TextValidator ());
		this.createTaskForm = new Form (keys, formFields, formValidators, new Color(0.75f,0.75f,0.75f,1), Color.red);

		//also init panel for subjects
		bool first = true;
		foreach(string s in idhandler.getAllNames("subjects")){
			GameObject subjectT = Instantiate (subjectToggle, Vector3.zero, Quaternion.identity) as GameObject;
			
			subjectT.transform.parent = panelCreateTask.transform.FindChild("panelSubject/subjects");
			subjectT.transform.FindChild ("Label").GetComponent<Text> ().text = s;
			subjectT.GetComponent<Toggle>().isOn = first;
			subjectT.GetComponent<Toggle>().group = subjectT.transform.parent.GetComponent<ToggleGroup>();
			if(first){
				first = false;
			}
		}
	}

	// <summary>
	/// Initialize the form for creating a class.
	/// </summary>
	private void initCreateClassForm(){
		List<string> keys = new List<string> ();
		keys.Add ("classname");
		keys.Add ("school_year");
		List<GameObject> formFields = new List<GameObject> ();
		formFields.Add (panelCreateClass.transform.Find("inputName").gameObject);
		formFields.Add (panelCreateClass.transform.Find("inputSchoolyear").gameObject);
		List<IValidator> formValidators = new List<IValidator> ();
		formValidators.Add (new TextValidator ());
		formValidators.Add (new TextValidator ());
		this.createClassForm = new Form (keys, formFields, formValidators, new Color(0.75f,0.75f,0.75f,1), Color.red);

		//also init panel for subjects
		bool first = true;
		foreach(string s in idhandler.getAllNames("subjects")){
			GameObject subjectT = Instantiate (subjectToggle, Vector3.zero, Quaternion.identity) as GameObject;

			subjectT.transform.parent = panelCreateClass.transform.FindChild("panelSubject/subjects");
			subjectT.transform.FindChild ("Label").GetComponent<Text> ().text = s;
			subjectT.GetComponent<Toggle>().isOn = first;
			subjectT.GetComponent<Toggle>().group = subjectT.transform.parent.GetComponent<ToggleGroup>();
			if(first){
				first = false;
			}
		}
	}

	// <summary>
	/// Handles clicking on menu items teacherClasses, courses or tasks.
	/// </summary>
	///
	/// <param name="target">clicked button name.</param>
	public void clickedBtn(string target){
		
		//cases: button of each class, course, task
		switch(target){
		case "btnKlassen":	//show overview of teacherClasses
							overviewKlassen.SetActive(true);
							overviewKurse.SetActive(false);
							overviewTasks.SetActive(false);
							menuTeacherClass.GetComponent<Button> ().interactable = false;
							menuUserClass.GetComponent<Button> ().interactable = true;
							menuTasks.GetComponent<Button> ().interactable = true;
							dbinterface.getMeineKlassen("Klassen", userid, gameObject);
							break;
		case "btnKurse": 	//show overview of courses
							overviewKlassen.SetActive(false);
							overviewKurse.SetActive(true);
							overviewTasks.SetActive(false);
							menuTeacherClass.GetComponent<Button> ().interactable = true;
							menuUserClass.GetComponent<Button> ().interactable = false;
							menuTasks.GetComponent<Button> ().interactable = true;
							dbinterface.getMeineKurse("Kurse", userid, gameObject);
							break;
		case "btnTasks":	//show overview of tasks
							overviewKlassen.SetActive(false);
							overviewKurse.SetActive(false);
							overviewTasks.SetActive(true);
							menuTeacherClass.GetComponent<Button> ().interactable = true;
							menuUserClass.GetComponent<Button> ().interactable = true;
							menuTasks.GetComponent<Button> ().interactable = false;
							dbinterface.getMeineTasks("Tasks", userid, gameObject);
							break;
		}
	}

	// <summary>
	/// Handles clicking on a certain teacherClass, course or task
	/// </summary>
	///
	/// <param name="target">type of clicked button.</param>
	/// <param name="id">task id.</param>
	/// <param name="type">task form type.</param>
	public void clickedBtn(string target, int id, string type = ""){
		switch(target){
		case "openTeacherClass":
							//getClassId to load corresponding class
							main.eventHandler("openTeacherClass", id);
							break;
		case "openUserClass":
							//getClassId to load corresponding class
							main.eventHandler("openUserClass", id);
							break;
		case "openTaskForm": switch(type){
			case "Zuordnung": main.eventHandler("openPanelFormAssignment", id);
							break;
			case "Quiz": 	main.eventHandler("openPanelFormQuiz", id);
							break;
			case "Kategorie": main.eventHandler ("openPanelFormCategory", id);
							break;
							}
							break;
		
		}
			
	}

	/*public void openTaskForm(int task_id){
		switch (type) {
		case "Quiz":	main.eventHandler("openPanelFormQuiz", task_id);
						break;
		case "Assignment": main.eventHandler("openPanelFormAssignment", task_id);
						break;
		case "Category": main.eventHandler ("openPanelFormCategory", task_id);
						break;

		}

	}*/

	/// <summary>
	/// Handles incoming data from the database
	/// </summary>
	/// 
	/// <param name="response">response data from the database.</param>
	public void dbInputHandler(string[] response){
		GameObject generatedBtn;
		string target = response[0];
		string data = response[1];
		JSONNode parsedData = JSONParser.JSONparse(data);
		Debug.Log ("in dbinputhandler of profile, target "+target);
		switch(target){
		case "userData": 	//save in user object
							user = new User(parsedData[0]);
							main.setHeaderText(user.getFirstName());
			
							//activate first overview: teacherClasses
							overviewKurse.SetActive(false);
							overviewTasks.SetActive(false);
							//get classes of user
							dbinterface.getMeineKlassen("Klassen", userid, gameObject);
							break;
						
		case "Klassen":		//delete old buttons and clear all references
							teacherClasses.Clear();
							foreach(GameObject b in teacherClassesBtns){
								Destroy(b);
							}	
							teacherClassesBtns.Clear();
			
							//split parsed data into data packages for one teacherClass)
							for(int i = 0; i < parsedData.Count; i++){
								JSONNode n = parsedData[i];
								if(n.Count > 0){
									TeacherClass temp = new TeacherClass(userid,n);
									//add teacherClass to list
									teacherClasses.Add(temp);
									//generate button for teacherClass and add to button list
									generatedBtn = Instantiate(buttonTeacherClass, Vector3.zero, Quaternion.identity) as GameObject;
									generatedBtn.transform.parent = GameObject.Find("ContentKlassen").transform;
									generatedBtn.transform.FindChild("Button/Text").GetComponent<Text>().text = temp.getClassname()
										+ "\n" + temp.getSubjectName();
									teacherClassesBtns.Add(generatedBtn);
					
									//set method to be called at onclick event for main button ("Button") and delete button ("ButtonDelete") on button object
									generatedBtn.transform.FindChild("Button").GetComponent<Button>().onClick.AddListener(() => {clickedBtn("openTeacherClass", temp.getClassId());});
									generatedBtn.transform.FindChild("ButtonDelete").GetComponent<Button>().onClick.AddListener(() => {confirmDeleteClass(temp.getClassId());});
								}
							}
							break;

		case "Kurse":		//delete old buttons and clear all references
							userClasses.Clear ();
							foreach(GameObject b in userClassesBtns){
								Destroy(b);
							}	
							userClassesBtns.Clear();

							//generate buttons
							for(int i = 0; i < parsedData.Count; i++){
								JSONNode n = parsedData[i];
								if(n.Count > 0){	
									UserClass temp = new UserClass(userid,n);
									userClasses.Add(temp);
									generatedBtn = Instantiate(buttonUserClass, Vector3.zero, Quaternion.identity) as GameObject;
									generatedBtn.transform.parent = GameObject.Find("ContentKurse").transform;
									generatedBtn.transform.FindChild("Button/Text").GetComponent<Text>().text = temp.getClassname();
									userClassesBtns.Add(generatedBtn);
									//set method to be called at onclick event
									generatedBtn.transform.FindChild("Button").GetComponent<Button>().onClick.AddListener(() => {clickedBtn("openUserClass", temp.getClassId());});
								}
							}
							break;
						
		case "Tasks":		//delete old buttons and clear all references
							tasks.Clear ();
							foreach(GameObject b in tasksBtns){
								Destroy(b);
							}	
							tasksBtns.Clear();

							//generate buttons
							for(int i = 0; i < parsedData.Count; i++){
								JSONNode n = parsedData[i];
								if(n.Count > 0){	
									TaskOverview temp = new TaskOverview(n);
									tasks.Add(temp);
									generatedBtn = Instantiate(buttonTasks, Vector3.zero, Quaternion.identity) as GameObject;
									generatedBtn.transform.parent = GameObject.Find("ContentTasks").transform;
									generatedBtn.transform.FindChild("Button/Text").GetComponent<Text>().text = temp.getTaskName()
										+ "\n" + temp.getSubjectName()
										+ " ; " + temp.getTypeName();
									tasksBtns.Add(generatedBtn);
									//set method to be called at onclick event
									generatedBtn.transform.FindChild("Button").GetComponent<Button>().onClick.AddListener(() => {clickedBtn("openTaskForm",temp.getTaskId(), temp.getTypeName());});
					generatedBtn.transform.FindChild("ButtonDelete").GetComponent<Button>().onClick.AddListener(() => {confirmDeleteTask(temp.getTaskId());});
									
								}
							}			
							break;
			
		case "deletedClass":
							if(int.Parse(parsedData[0]["success"]) == 1){ //success
								//refresh overview
								dbinterface.getMeineKlassen("Klassen", userid, gameObject);
							} else {
								main.dbErrorHandler("deleteClass", "Löschen fehlgeschlagen");
							}
							break;
		case "addedClass":	//TODO check if successfull, else: call main.dberrorhandler
							//panelCreateClass.SetActive(false);
							main.back();
							dbinterface.getMeineKlassen("Klassen", userid, gameObject);
							break;
		case "deletedTask": 
							if(int.Parse(parsedData[0]["success"]) == 1){ //success
								//refresh overview
								dbinterface.getMeineTasks("Tasks", userid, gameObject);
							} else {
								main.dbErrorHandler("deleteClass", "Löschen fehlgeschlagen");
							}
							break;
		case "addedTask":	
							//panelCreateTask.SetActive(false);
							main.back();
							dbinterface.getMeineTasks("Tasks", userid, gameObject);
							break;
		case "registered": 	
							//panelRegistration.SetActive(false);
							main.back();
							//TODO check if successfull, else: call main.dberrorhandler
							dbinterface.getMeineKurse("Kurse", userid, gameObject);
							break;
		}
	}

	/// <summary>
	/// Deletes profile data at log out.
	/// </summary>
	public void clear(){
		userClasses.Clear ();
		teacherClasses.Clear ();
		userClassesBtns.Clear ();
		teacherClassesBtns.Clear ();
		tasksBtns.Clear ();
		setUserId (0);
	}

	/// <summary>
	/// Shows task creation panel.
	/// </summary>
	public void showTaskCreation(){
		Debug.Log ("button clicked for show task creation");
		panelCreateTask.SetActive (true);
		main.addToPanelStack (panelCreateTask);
	}

	/// <summary>
	/// Shows creation form for class.
	/// </summary>
	public void showCreationFormForClass(){
		Debug.Log ("button clicked, show creation form");	
		//activate form to insert class data
		panelCreateClass.SetActive(true);
		main.addToPanelStack (panelCreateClass);
		//wait until button "create" is clicked (button calls addClass)
	}

	/// <summary>
	/// Add class from the form.
	/// </summary>
	public void addClass(){
		//insert class into db if for filled correctly
		int subject_id = 0;
		for (int i = 0; i < panelCreateClass.transform.FindChild ("panelSubject/subjects").childCount; i++){
			Transform tr = panelCreateClass.transform.FindChild ("panelSubject/subjects").GetChild (i);
			if(tr.GetComponent<Toggle>().isOn){
				subject_id = idhandler.getFromName(tr.Find ("Label").GetComponent<Text>().text, "subjects");
			}
		}
		dbinterface.createClass("addedClass", this.createClassForm, userid, subject_id, gameObject); 
	}

	/// <summary>
	/// Add task from the form.
	/// </summary>
	public void addTask(){
		//add task into db
		//TODO: toggle for privacy
		int type = 0;
		if (toggleCategory.isOn) {
			type = idhandler.getFromName("Kategorie", "tasktypes");
		}
		if (toggleAssignment.isOn) {
			type = idhandler.getFromName("Zuordnung", "tasktypes");
		}
		if (toggleQuiz.isOn) {
			type = idhandler.getFromName("Quiz", "tasktypes");
		}

		int subject_id = 0;
		for (int i = 0; i < panelCreateTask.transform.FindChild ("panelSubject/subjects").childCount; i++){
			Transform tr = panelCreateTask.transform.FindChild ("panelSubject/subjects").GetChild (i);
			if(tr.GetComponent<Toggle>().isOn){
				subject_id = idhandler.getFromName(tr.Find ("Label").GetComponent<Text>().text, "subjects");
			}
		}
		//public toggle not supported now
		int publicToggle = 1;//int.Parse (taskpublic.GetComponent<Text> ().text);
		dbinterface.createTask("addedTask", this.createTaskForm, publicToggle, userid, subject_id, type, gameObject); 
	}

	/// <summary>
	/// Confirm delete of class.
	/// </summary>
	///
	/// <param name="class_id">class_id.</param>
	public void confirmDeleteClass(int class_id){
		Debug.Log ("button clicked, try to delete class");	
		main.activateDialogbox ("Do you want to delete this class?", class_id, gameObject, "deleteClass");

	}

	/// <summary>
	/// Delete a class.
	/// </summary>
	///
	/// <param name="temp">answer and class id.</param>
	public void deleteClass(int[] temp){
		int answer = temp [0];
		int id = temp [1];
		if (answer == 1) {
			dbinterface.deleteClass ("deletedClass", id, gameObject);
		}
	}

	/// <summary>
	/// Confirm delete of task.
	/// </summary>
	///
	/// <param name="task_id">task_id.</param>
	public void confirmDeleteTask(int task_id){
		Debug.Log ("button clicked, try to delete task");	
		main.activateDialogbox ("Do you want to delete this task?", task_id, gameObject, "deleteTask");
	}

	/// <summary>
	/// Delete a task.
	/// </summary>
	///
	/// <param name="temp">answer and task id.</param>
	public void deleteTask(int[] temp){
		int answer = temp [0];
		int id = temp [1];
		if (answer == 1) {
			dbinterface.deleteTask("deletedTask", id, gameObject);
		}
	}

	/// <summary>
	/// Show panel with registration form.
	/// </summary>
	public void showRegisterToClassForm(){
		Debug.Log ("button clicked, show registration form");	
		panelRegistration.SetActive(true);
		main.addToPanelStack (panelRegistration);
		//wait until button "register" is clicked (button calls register) 
	}

	/// <summary>
	/// Register to class.
	/// </summary>
	public void register(){
		Debug.Log ("button clicked, try to register");	
		dbinterface.registerUserToClass("registered", userid, classcode.GetComponent<Text>().text, gameObject);	
	}

	/// <summary>
	/// Set userid.
	/// </summary>
	/// 
	/// <param name="id">user id.</param>
	public void setUserId(int id){
		userid = id;
	}

	public void setDBInterface(DBInterface dbInterface){
		this.dbinterface = dbInterface;
	}
}
