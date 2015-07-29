using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJSON;

public class PanelTeacherClass : MonoBehaviour {
	/// <summary>
	/// The main class.
	/// </summary>
	private Main main;

	/// <summary>
	/// The database interface.
	/// </summary>
	private DBInterface dbinterface;

	/// <summary>
	/// The panel for the student list.
	/// </summary>
	public GameObject panelStudentList;

	/// <summary>
	/// The panel for the student detail list.
	/// </summary>
	public GameObject panelStudentListDetail;

	/// <summary>
	/// The panel with form to add topic.
	/// </summary>
	public GameObject panelAddTopic;

	/// <summary>
	/// The topic text field in the form.
	/// </summary>
	public Text fieldTopicToAdd;

	/// <summary>
	/// The panel with form to add topic.
	/// </summary>
	public GameObject panelAddTask;

	/// <summary>
	/// The list of tasks.
	/// </summary>
	public List<TaskShort> tasks;
	public List<TaskOverview> tasksOverview;

	/// <summary>
	/// The list of task buttons.
	/// </summary>
	public List<GameObject> tasksBtns;

	/// <summary>
	/// The task button prefab.
	/// </summary>
	public GameObject buttonTasks;

	/// <summary>
	/// The topic prefab.
	/// </summary>
	public GameObject topic;

	/// <summary>
	/// The task button prefab.
	/// </summary>
	public GameObject btnTask;

	/// <summary>
	/// The student list prefabs.
	/// </summary>
	public GameObject studentInList_unaccepted, studentInList_accepted;

	/// <summary>
	/// The student detail list prefab.
	/// </summary>
	public GameObject studentDetailEntry;

	/// <summary>
	/// The class_id.
	/// </summary>
	public int class_id;

	/// <summary>
	/// The task_id.
	/// </summary>
	public int task_id;

	/// <summary>
	/// The student_id.
	/// </summary>
	public int student_id;

	/// <summary>
	/// The current topic id.
	/// </summary>
	public int currentTopic;

	/// <summary>
	/// The teacher class object.
	/// </summary>
	private TeacherClass teacherClass;

	/// <summary>
	/// The class dat textfield.
	/// </summary>
	public Text fieldClassData;

	/// <summary>
	/// The form to add task.
	/// </summary>
	private Form addTaskForm;

	/// <summary>
	/// The form to add topic.
	/// </summary>
	private Form topicForm;
	
	/// <summary>
	/// The list of topic objects.
	/// </summary>
	public List<GameObject> topics;

	/// <summary>
	/// The list of student objects.
	/// </summary>
	public List<GameObject> students;

	/// <summary>
	/// The list of student detail objects.
	/// </summary>
	public List<GameObject> studentDetailEntries;

	void Start () {
	
		main = GameObject.Find ("Scripts").GetComponent<Main>();
		//teacherClass = GameObject.Find("Scripts").GetComponent<TeacherClass>();

		//add localization of texts
		gameObject.transform.FindChild("btnShowStudents/Text").GetComponent<Text> ().text 
			= LocaleHandler.getText ("student-results", main.getLang());
		gameObject.transform.FindChild("OverviewTasksInClass/ContentTasksForTopic/ButtonAdd/Text").GetComponent<Text> ().text 
			= LocaleHandler.getText ("button-add-topic", main.getLang());
		gameObject.transform.FindChild("panelAddTopic/Text").GetComponent<Text> ().text 
			= LocaleHandler.getText ("topicname-info", main.getLang());
		gameObject.transform.FindChild("panelAddTopic/Button/Text").GetComponent<Text> ().text 
			= LocaleHandler.getText ("button-add-topicok", main.getLang());
		topic.transform.FindChild("btnAddTask/Text").GetComponent<Text> ().text 
			= LocaleHandler.getText ("button-add-tasktotopic", main.getLang());

		//add task to topic
		gameObject.transform.FindChild("panelAddTask/textTask").GetComponent<Text> ().text 
			= LocaleHandler.getText ("info-select-task", main.getLang());
		gameObject.transform.FindChild("panelAddTask/textAttempts").GetComponent<Text> ().text 
			= LocaleHandler.getText ("info-task-maxattempts", main.getLang());
		gameObject.transform.FindChild("panelAddTask/Panel/toggleNotObligatory/Label").GetComponent<Text> ().text 
			= LocaleHandler.getText ("info-notobligatory", main.getLang());
		gameObject.transform.FindChild("panelAddTask/Panel/toggleObligatory/Label").GetComponent<Text> ().text 
			= LocaleHandler.getText ("info-obligatory", main.getLang());

		tasksBtns = new List<GameObject>();
		topics = new List<GameObject>();
		students = new List<GameObject>();
		tasksOverview = new List<TaskOverview> ();
		//init ();
		initAddTaskForm ();
		initTopicForm ();
	}

	// <summary>
	/// Initialize the topic form.
	/// </summary>
	public void initTopicForm(){
		List<string> keys = new List<string> ();
		keys.Add ("topic_name");
		List<GameObject> formFields = new List<GameObject> ();
		formFields.Add (panelAddTopic.transform.FindChild("InputField").gameObject);
		List<IValidator> formValidators = new List<IValidator> ();
		formValidators.Add (new TextValidator());
		topicForm = new Form (keys, formFields, formValidators, new Color(0.75f,0.75f,0.75f,1), Color.red);
	}

	// <summary>
	/// Initialize the form to add a task.
	/// </summary>
	public void initAddTaskForm(){
		List<string> keys = new List<string> ();
		keys.Add ("max_attempts");
		List<GameObject> formFields = new List<GameObject> ();
		formFields.Add (panelAddTask.transform.FindChild("InputField").gameObject);
		List<IValidator> formValidators = new List<IValidator> ();
		formValidators.Add (new NumberValidator (false, 0, int.MaxValue));
		addTaskForm = new Form (keys, formFields, formValidators, new Color(0.75f,0.75f,0.75f,1), Color.red);
	}

	// <summary>
	/// Initialize the panel.
	/// </summary>
	public void init(){
		//string[] temp = new string[]{"","1","","","","0","","","","","","","","","",""};
		//teacherClass = new TeacherClass(class_id, temp);
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();
		dbinterface.getTeacherClassData("classData", class_id, gameObject);
		
		panelStudentList.SetActive(false);
		panelAddTopic.SetActive (false);
		panelAddTask.SetActive (false);
	
		//and destroy all generated topics (with their tasks)
		foreach (GameObject t in topics){
			Destroy(t);	
		}
		topics.Clear();
	}

	/// <summary>
	/// Handles incoming data from the database
	/// </summary>
	/// 
	/// <param name="response">response data from the database.</param>
	public void dbInputHandler(string[] response){
		Debug.Log ("in dbinputhandler of PanelTeacherClass");
		string target = response[0];
		string data = response[1];
		GameObject generatedButton;
		GameObject generatedTopic;
		GameObject generatedStudentInList;
		GameObject generatedStudentDetailEntry;
		JSONNode parsedData = JSONParser.JSONparse(data);
		Debug.Log ("with target: "+target);
		switch(target){	
		case "classData": 	
							teacherClass = new TeacherClass(class_id, parsedData[0]);
							fieldClassData.GetComponent<Text>().text = teacherClass.getClassname()
								+ " ; " + teacherClass.getSubjectName() + "\nClasscode: "+teacherClass.getClassCode();
							dbinterface.getTopicsForClass("classTopics", class_id, gameObject); 
							break;
		case "classTopics": if(data != "[]"){
								for(int i = 0; i < parsedData.Count; i++){
									JSONNode n = parsedData[i];
									Topic t = new Topic(n);
									teacherClass.addTopic(t);
								}
								dbinterface.getTasksForClass("classTasks", class_id, gameObject);
							}
							
							break;
		case "classTasks":	if(data != "[]"){
								for(int i = 0; i < parsedData.Count; i++){
									JSONNode n = parsedData[i];
									TaskShort task = new TaskShort(n);
									teacherClass.addTask(task);
								}
							}
							
							if(teacherClass.getTopicList().Count>0){
								foreach(Topic t in teacherClass.getTopicList()){
									//generate topic, add it to hierarchy and change shown text
									generatedTopic = Instantiate(topic, Vector3.zero, Quaternion.identity) as GameObject;
									generatedTopic.transform.parent = GameObject.Find("ContentTasksForTopic").transform;
									generatedTopic.transform.FindChild("TopicHeadline/Text").GetComponent<Text>().text = t.getName();
									//define button actions: add task and delete topic
									int topicId = t.getId();
									generatedTopic.transform.FindChild("btnAddTask").GetComponent<Button>().onClick.AddListener(()=> {showTasks(topicId);});
									generatedTopic.transform.FindChild("TopicHeadline/ButtonDelete").GetComponent<Button>().onClick.AddListener(()=> {deleteTopic(topicId);});
									topics.Add(generatedTopic);
									if(teacherClass.getTaskList().Count>0){
										foreach(TaskShort ts in teacherClass.getTaskList()){
											//find all tasks that belong to this topic
											if(ts.getTopicId() == topicId){
												//generate task, add it to hierarchy and change shown text
												generatedButton = Instantiate(btnTask, Vector3.zero, Quaternion.identity) as GameObject;
												generatedButton.transform.parent = generatedTopic.transform;
												generatedButton.transform.FindChild("ButtonTask/Text").GetComponent<Text>().text = ts.getTaskName();
												//define button actions: start task and delete task
												int taskId = ts.getTaskId();
												generatedButton.transform.FindChild("ButtonTask").GetComponent<Button>().onClick.AddListener(()=> {startTask(taskId, topicId);});
												generatedButton.transform.FindChild("ButtonDelete").GetComponent<Button>().onClick.AddListener(()=>{confirmDeleteTask(taskId, topicId);});
												
											}
										}
									}
					
								}
							}
							break;
		case "studentlist_students":	
							//delete old studentInList objects
							foreach (GameObject st in students){
								Debug.Log (st.name);
								Destroy(st);	
							}
							students.Clear();
							teacherClass.deleteStudentList();

							panelStudentList.SetActive(true);
							main.addToPanelStack(panelStudentList);
			
							//parse data in student objects (create student class)
							if(data != "[]"){
								for(int i = 0; i < parsedData.Count; i++){
									JSONNode n = parsedData[i];
									Student student = new Student(n);
									teacherClass.addStudent(student);
									/*Debug.Log ("result: "+n.ToString());
									ResultContainer resultContainer = new ResultContainer(n);
									//Student student = new Student(n);
									//teacherClass.addStudent(student);
									generatedStudentInList = Instantiate(studentInList_accepted, Vector3.zero, Quaternion.identity) as GameObject;
									//create studentInList object for each student
									if(student.isAccepted()){
										generatedStudentInList = Instantiate(studentInList_accepted, Vector3.zero, Quaternion.identity) as GameObject;
									} else {
										// studentInList_unaccepted object containa an add button
										generatedStudentInList = Instantiate(studentInList_unaccepted, Vector3.zero, Quaternion.identity) as GameObject;
										//define button action: add student to class
										generatedStudentInList.transform.FindChild("ButtonAdd").GetComponent<Button>().onClick.AddListener(()=> {acceptStudent(student);});
									}
									//add studentInList objects to hierarchy
									generatedStudentInList.transform.parent = gameObject.transform.FindChild("StudentList/ContentStudents").transform;
									generatedStudentInList.transform.FindChild("btnName/Text").GetComponent<Text>().text = 	"user";	//student.getName();
									generatedStudentInList.transform.FindChild("fieldResult/Text").GetComponent<Text>().text = resultContainer.getResultOfStudent(3)[0].ToString();

									//int student_id = student.getId();
									//generatedStudentInList.transform.FindChild("btnName").GetComponent<Button>().onClick.AddListener(()=> {clickedStudent(student_id);});
									students.Add(generatedStudentInList);*/

								}
							}
							dbinterface.getResultOfStudents ("studentlist_results", class_id, 0, gameObject);
							break;
		case "studentlist_results":
							ResultContainer resultContainer = new ResultContainer(parsedData);
							foreach(Student student in teacherClass.getStudentList()){

							Debug.Log("add student: "+student.getName());
								//create studentInList object for each student
								if(student.isAccepted()){
									generatedStudentInList = Instantiate(studentInList_accepted, Vector3.zero, Quaternion.identity) as GameObject;
								} else {
									// studentInList_unaccepted object containa an add button
									generatedStudentInList = Instantiate(studentInList_unaccepted, Vector3.zero, Quaternion.identity) as GameObject;
									//define button action: add student to class
									generatedStudentInList.transform.FindChild("ButtonAdd").GetComponent<Button>().onClick.AddListener(()=> {acceptStudent(student);});
								}
								//add studentInList objects to hierarchy
								generatedStudentInList.transform.parent = gameObject.transform.FindChild("StudentList/ContentStudents").transform;
								generatedStudentInList.transform.FindChild("btnName/Text").GetComponent<Text>().text = 	student.getName();
								int student_id = student.getId();
								generatedStudentInList.transform.FindChild("fieldResult").GetComponent<Text>().text = resultContainer.getAverageResultOfStudent(student_id)+"%";
								generatedStudentInList.transform.FindChild("btnName").GetComponent<Button>().onClick.AddListener(()=> {clickedStudent(student_id);});
								students.Add(generatedStudentInList);
							}

							break;
		case "studentlistDetail": /*TODO 	
											generate exam entries with result lists
											add entries to list of exam entries
											
											use ResultContainer: call getResultOfStudent/s; getResultOfTask/s
											give the returned jsonNode object into ResultContainer constructor
											-> then call methods to receive list of results of it
											
							*/

							//delete old studentDetailEntry objects
							foreach (GameObject s in studentDetailEntries){
								Destroy(s);	
							}
							studentDetailEntries.Clear();
							
							panelStudentListDetail.SetActive(true);
							main.addToPanelStack(panelStudentListDetail);
							
							ResultContainer resultContainerStudent = new ResultContainer(parsedData);
							 
							
							
							panelStudentListDetail.transform.FindChild("ContentStudentsDetail/Text").GetComponent<Text>().text = teacherClass.getUserName (student_id);			
			
							foreach(TaskShort t in teacherClass.getTaskList()){
								generatedStudentDetailEntry = Instantiate(studentDetailEntry, Vector3.zero, Quaternion.identity) as GameObject;
								generatedStudentDetailEntry.transform.parent = gameObject.transform.FindChild("StudentListDetail/ContentStudentsDetail").transform;
								generatedStudentDetailEntry.transform.FindChild("fieldExamName").GetComponent<Text>().text = teacherClass.getTaskName(t.getTaskId());
								string resultString = "";
								
								foreach(int result in resultContainerStudent.getResultOfStudentOfTask(student_id, t.getTaskId())){
									resultString += result+"%, ";
								}
								if(resultString == ""){
									resultString = "-";
								}

				generatedStudentDetailEntry.transform.FindChild("Text").GetComponent<Text>().text = resultString;
								studentDetailEntries.Add (generatedStudentDetailEntry);

							}
							

							/*foreach(Result r in resultContainerStudent.getResults()){					
							//create studentListDetailEntry object for each entry
								generatedStudentDetailEntry = Instantiate(studentDetailEntry, Vector3.zero, Quaternion.identity) as GameObject;
								generatedStudentDetailEntry.transform.parent = gameObject.transform.FindChild("StudentListDetail/ContentStudentsDetail").transform;
								generatedStudentDetailEntry.transform.FindChild("fieldExamName").GetComponent<Text>().text = teacherClass.getTaskName(r.getTaskId());
								generatedStudentDetailEntry.transform.FindChild("Text").GetComponent<Text>().text = r.getResult()+"%";
								studentDetailEntries.Add (generatedStudentDetailEntry);
							}*/
										
							break;
		case "examResults":	foreach (GameObject st in students){
								Debug.Log (st.name);
								Destroy(st);	
							}
								students.Clear();
								
								panelStudentList.SetActive(true);
								main.addToPanelStack(panelStudentList);
								ResultContainer resultContainer2 = new ResultContainer(parsedData);
								foreach(TaskShort task in teacherClass.getTaskList()){
								//create studentInList object for each task
								generatedStudentInList = Instantiate(studentInList_accepted, Vector3.zero, Quaternion.identity) as GameObject;
								//add objects to hierarchy
								generatedStudentInList.transform.parent = gameObject.transform.FindChild("StudentList/ContentStudents").transform;
								generatedStudentInList.transform.FindChild("btnName/Text").GetComponent<Text>().text = task.getTaskName();
								int task_id = task.getTaskId();
								generatedStudentInList.transform.FindChild("fieldResult").GetComponent<Text>().text = resultContainer2.getAverageResultOfTask(task_id)+"%";
								generatedStudentInList.transform.FindChild("btnName").GetComponent<Button>().onClick.AddListener(()=> {clickedTaskResultObject(task_id);});
								students.Add(generatedStudentInList);
							}

							break;
		case "examResultsDetail":
							foreach (GameObject s in studentDetailEntries){
								Destroy(s);	
							}
							studentDetailEntries.Clear();
							
							panelStudentListDetail.SetActive(true);
							main.addToPanelStack(panelStudentListDetail);
							
							ResultContainer resultContainerStudent2 = new ResultContainer(parsedData);
							panelStudentListDetail.transform.FindChild("ContentStudentsDetail/Text").GetComponent<Text>().text = teacherClass.getTaskName (task_id);
							
							
							foreach(Student s in teacherClass.getStudentList()){
								generatedStudentDetailEntry = Instantiate(studentDetailEntry, Vector3.zero, Quaternion.identity) as GameObject;
								generatedStudentDetailEntry.transform.parent = gameObject.transform.FindChild("StudentListDetail/ContentStudentsDetail").transform;
								generatedStudentDetailEntry.transform.FindChild("fieldExamName").GetComponent<Text>().text = teacherClass.getUserName(s.getId());
								string resultString = "";
								foreach(int result in resultContainerStudent2.getResultOfStudentOfTask(s.getId(), task_id)){
									resultString += result+"%, ";
								}
								if(resultString == ""){
									resultString = "-";
								}
								
								generatedStudentDetailEntry.transform.FindChild("Text").GetComponent<Text>().text = resultString;
								studentDetailEntries.Add (generatedStudentDetailEntry);
								
							}
							break;
		case "acceptedStudent":
							if(int.Parse(parsedData[0]["success"]) == 1){ //success
								//refresh overview
								showStudentList();
							} else {
								main.dbErrorHandler("acceptedStudent", "Schüler konnte nicht hinzugefügt werden");
							}
							break;
		case "tasks":		panelAddTask.SetActive(true);
							main.addToPanelStack(panelAddTask);
							//generate buttons
							GameObject generatedBtn;
							//delete old buttons and clear all references
							tasksOverview = new List<TaskOverview>();
							foreach(GameObject b in tasksBtns){
								Destroy(b);
							}	
							tasksBtns.Clear();

							for(int i = 0; i < parsedData.Count; i++){
								JSONNode n = parsedData[i];
								if(n.Count > 0){	
									TaskOverview temp = new TaskOverview(n);
									tasksOverview.Add(temp);
									generatedBtn = Instantiate(buttonTasks, Vector3.zero, Quaternion.identity) as GameObject;
									generatedBtn.transform.parent = GameObject.Find("ContentTasks").transform;
									generatedBtn.transform.FindChild("Text").GetComponent<Text>().text = temp.getTaskName();
									tasksBtns.Add(generatedBtn);
									//set method to be called at onclick event
									generatedBtn.GetComponent<Button>().onClick.AddListener(() => {addTask (temp.getTaskId(), currentTopic);});
								}
							}	
							break;
		case "changed":		//added or deleted task or topic --> refresh view
							init();
							break;
		case "addedTopic":		//added or deleted task or topic --> refresh view
							init();
							break;
		case "addedTaskToClass": init ();
							break;
		case "deletedTopic": init ();
							break;
		case "deletedTask": init ();
							break;
		case "startTask":	
							Task taskToStart = new Task(task_id, parsedData[0]);
							int task_for_class_id = int.Parse (parsedData[0]["task_for_class_id"]);
							if(taskToStart.getTypeName() == "Zuordnung"){
								main.eventHandler("startTaskAssign", task_id, task_for_class_id, true);
							}
							if(taskToStart.getTypeName() == "Kategorie"){
								main.eventHandler("startTaskCategory", task_id, task_for_class_id, true);
							}
							if(taskToStart.getTypeName() == "Quiz"){
								main.eventHandler("startTaskQuiz", task_id, task_for_class_id, true);
							}
							break;
		}
		
	}

	/// <summary>
	/// Shows tasks.
	/// </summary>
	/// 
	/// <param name="topicId">id of topic for the tasks.</param>
	public void showTasks(int topicId){
		Debug.Log ("Button clicked, try to add Task to Topic with id "+topicId);
		currentTopic = topicId;
		dbinterface.getMeineTasks("tasks", teacherClass.getUserId(), gameObject);
	}

	/// <summary>
	/// Adds task to teacher class.
	/// </summary>
	/// 
	/// <param name="taskId">task id</param>
	/// <param name="topicId">topic id</param>
	public void addTask(int taskId, int topicId){
		//TODO change param -> deadline
		int obligatory = 0;
		if (panelAddTask.transform.FindChild ("Panel/toggleObligatory").GetComponent<Toggle> ().isOn){
			obligatory = 1;
		}
		Debug.Log ("--------------- "+panelAddTask.transform.FindChild("InputField/Text").GetComponent<Text>().text);
		if (dbinterface.assignTaskToTopic ("addedTaskToClass", class_id, taskId, topicId, obligatory, System.DateTime.Now.ToString (), addTaskForm, gameObject)) {
			panelAddTask.SetActive (false);
		}
	}
		
	/// <summary>
	/// Starts task.
	/// </summary>
	/// 
	/// <param name="id">task id</param>
	/// <param name="topicId">topic id</param>
	public void startTask(int id, int topicId){
		Debug.Log ("Button clicked, try to start Task");
		Debug.Log (id + ";" + topicId + ";" + class_id);
		task_id = id;
		//dbinterface.getTask ("startTask", id, gameObject);
		dbinterface.getTaskForClass ("startTask", id, class_id, topicId, gameObject);
	}

	/// <summary>
	/// Confirms delete of a task.
	/// </summary>
	/// 
	/// <param name="task_id">task id</param>
	/// <param name="topicId">topic id</param>
	public void confirmDeleteTask(int task_id, int topic_id){
		Debug.Log ("button clicked, try to delete task");
		this.task_id = task_id;
		main.activateDialogbox ("Do you want to delete this task?", topic_id, gameObject, "deleteTask");
	}

	/// <summary>
	/// Delete a task.
	/// </summary>
	/// 
	/// <param name="temp">answer and topic id</param>
	public void deleteTask(int[] temp){
		int answer = temp [0];
		int id = temp [1];
		if (answer == 1) {
			dbinterface.deleteTaskFromTopic("deletedTask", class_id, task_id, id, gameObject);
		}
		task_id = -1;
	}

	/// <summary>
	/// Activate panel with form to add topic.
	/// </summary>
	public void activatePanelAddTopic(){
		Debug.Log ("activate panel");
		panelAddTopic.SetActive (true);
		main.addToPanelStack (panelAddTopic);
	}

	/// <summary>
	/// Add topic to teacher class
	/// </summary>
	public void addTopic(){
		string name = fieldTopicToAdd.GetComponent<Text> ().text;
		Debug.Log ("Button clicked, try to add Topic: "+name);
		if (dbinterface.createClassTopic ("addedTopic", class_id, topicForm, gameObject)) {
			panelAddTopic.SetActive (false);
		}
	}

	/// <summary>
	/// Delete a topic.
	/// </summary>
	/// 
	/// <param name="id">topic id</param>
	public void deleteTopic(int id){
		Debug.Log ("Button clicked, try to delete Topic with id "+id);	
		dbinterface.deleteClassTopic ("deletedTopic", id, gameObject);
	}

	/// <summary>
	/// Show lis of students.
	/// </summary>
	public void showStudentList(){
		Debug.Log ("Button clicked, show panel studentList");	
		dbinterface.getClassUsers ("studentlist_students", class_id, gameObject);
	}

	/// <summary>
	/// Accepts student in class
	/// </summary>
	///
	/// <param name="s">student to accept</param>
	public void acceptStudent(Student s){
		Debug.Log ("Button clicked, try to accept user");	
		dbinterface.acceptUserInClass("acceptedStudent", s.getId(), class_id, gameObject);	
	}

	// <summary>
	/// Set class id.
	/// </summary>
	/// 
	/// <param name="id">class id.</param>
	public void setClassId(int id){
		class_id = id;
	}

	// <summary>
	/// Handles click on student object.
	/// </summary>
	/// 
	/// <param name="id">student id.</param>
	public void clickedStudent(int id){
		student_id = id;
		Debug.Log ("clicked Student wit id: "+student_id);
		dbinterface.getResultOfStudent("studentlistDetail", class_id, student_id, 0, gameObject);
	}

	// <summary>
	/// Shows results of exams in the teacher class.
	/// </summary>
	public void showExamResults(){
		Debug.Log ("Button clicked, show exam results");	
		dbinterface.getResultOfStudents ("examResults", teacherClass.getClassId (), 0, gameObject);
	}

	// <summary>
	/// Handles click on task result object.
	/// </summary>
	/// 
	/// <param name="task_id">task id.</param>
	public void clickedTaskResultObject(int task_id){
		this.task_id = task_id;
		dbinterface.getResultOfStudents ("examResultsDetail", teacherClass.getClassId (), 0, gameObject);
	}
}