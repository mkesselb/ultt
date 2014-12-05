using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJSON;

public class PanelTeacherClass : MonoBehaviour {

	private Main main;
	private DBInterface dbinterface;
	
	public GameObject panelStudentList;

	//panelAddTopic
	public GameObject panelAddTopic;
	public Text fieldTopicToAdd;

	//panelAddTask
	public GameObject panelAddTask;
	public List<TaskShort> tasks;
	public List<TaskOverview> tasksOverview;
	public List<GameObject> tasksBtns;
	public GameObject buttonTasks;

	public GameObject topic;
	public GameObject btnTask;
	public GameObject studentInList_unaccepted, studentInList_accepted;
	
	public int class_id;
	public int task_id;
	public int currentTopic;
	private TeacherClass teacherClass;
	
	public Text fieldClassData;
	
	//contain Gameobjects (= buttons)
	public List<GameObject> topics;
	public List<GameObject> students;

	void Start () {
	
		main = GameObject.Find ("Scripts").GetComponent<Main>();
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();
		//teacherClass = GameObject.Find("Scripts").GetComponent<TeacherClass>();
		
		tasksBtns = new List<GameObject>();
		topics = new List<GameObject>();
		students = new List<GameObject>();
		tasksOverview = new List<TaskOverview> ();
		init ();
	}
	
	public void init(){
		//TODO dbinterface.getTeacherClassData("classData", class_id, gameObject);
		string[] temp = new string[]{"","1","","","","0","","","","","","","","","",""};
		teacherClass = new TeacherClass(class_id, temp);
		//TODO: probably, problem with gameObject here? at least for me
		dbinterface.getTopicsForClass("classTopics", class_id, gameObject); 
		
		panelStudentList.SetActive(false);
		panelAddTopic.SetActive (false);
		panelAddTask.SetActive (false);
	
		//and destroy all generated topics (with their tasks)
		foreach (GameObject t in topics){
			Destroy(t);	
		}
		topics.Clear();
	}
	
	public void dbInputHandler(string[] response){
		Debug.Log ("in dbinputhandler of PanelTeacherClass");
		string target = response[0];
		string data = response[1];
		GameObject generatedButton;
		GameObject generatedTopic;
		GameObject generatedStudentInList;
		JSONNode parsedData;
		Debug.Log ("with target: "+target);
		switch(target){	
		case "classData": 	parsedData = JSONParser.JSONparse(data);
							teacherClass = new TeacherClass(class_id, parsedData[0]);
							fieldClassData.GetComponent<Text>().text = teacherClass.getClassname()+"\nclasscode: "+teacherClass.getClassCode();
							
							dbinterface.getTopicsForClass("classTopics", class_id, gameObject); 
							break;
		case "classTopics": if(data != "[]"){
								parsedData = JSONParser.JSONparse(data);
								for(int i = 0; i < parsedData.Count; i++){
									JSONNode n = parsedData[i];
									Topic t = new Topic(n);
									teacherClass.addTopic(t);
								}
								dbinterface.getTasksForClass("classTasks", class_id, gameObject);
							}
							
							break;
		case "classTasks":	if(data != "[]"){
								parsedData = JSONParser.JSONparse(data);
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
												generatedButton.transform.FindChild("ButtonTask").GetComponent<Button>().onClick.AddListener(()=> {startTask(taskId);});
												generatedButton.transform.FindChild("ButtonDelete").GetComponent<Button>().onClick.AddListener(()=> {deleteTask(taskId);});
												
											}
										}
									}
					
								}
							}
							break;
		case "studentlist":	
							//delete old studentInList objects
							foreach (GameObject s in students){
								Destroy(s);	
							}
							students.Clear();

							panelStudentList.SetActive(true);
			
							//parse data in student objects (create student class)
							if(data != "[]"){
								parsedData = JSONParser.JSONparse(data);
								for(int i = 0; i < parsedData.Count; i++){
									JSONNode n = parsedData[i];
									Student student = new Student(n);
									teacherClass.addStudent(student);
					
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
									generatedStudentInList.transform.FindChild("Text").GetComponent<Text>().text = student.getName();
									
									students.Add(generatedStudentInList);
								}
							}
							break;
		case "acceptedStudent":
							parsedData = JSONParser.JSONparse(data);
							if(parsedData[0]["success"] == "1"){ //success
								//refresh overview
								showStudentList();
							} else {
								main.dbErrorHandler("acceptedStudent", "Schüler konnte nicht hinzugefügt werden");
							}
							break;
		case "tasks":		panelAddTask.SetActive(true);
							//generate buttons
							GameObject generatedBtn;
							//delete old buttons and clear all references
							tasksOverview = new List<TaskOverview>();
							foreach(GameObject b in tasksBtns){
								Destroy(b);
							}	
							tasksBtns.Clear();
							parsedData = JSONParser.JSONparse(data);

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
		case "startTask":	parsedData = JSONParser.JSONparse(data);
							Task taskToStart = new Task(task_id, parsedData[0]);
							break;
		}
		
	}
	
	public void showTasks(int topicId){
		Debug.Log ("Button clicked, try to add Task to Topic with id "+topicId);
		currentTopic = topicId;
		//TODO dbmethod
		dbinterface.getMeineTasks("tasks", teacherClass.getUserId(), gameObject);
	}

	public void addTask(int taskId, int topicId){
		//TODO change params
		dbinterface.assignTaskToTopic ("addedTaskToClass", class_id, taskId, topicId, 1, System.DateTime.Now.ToString(), 1, gameObject);
		panelAddTask.SetActive (false);
	}
		
	public void startTask(int id){
		Debug.Log ("Button clicked, try to start Task");
		task_id = id;
		dbinterface.getTask ("startTask", id, gameObject);
	}
	
	public void deleteTask(int id){
		Debug.Log ("Button clicked, TODO try to delete Task");	
		//delete task with target "deletedTask"
	}

	public void activatePanelAddTopic(){
		Debug.Log ("activate panel");
		panelAddTopic.SetActive (true);
	}
	
	public void addTopic(){
		string name = fieldTopicToAdd.GetComponent<Text> ().text;
		Debug.Log ("Button clicked, try to add Topic: "+name);	
		dbinterface.createClassTopic("addedTopic", class_id, name, gameObject);	
		panelAddTopic.SetActive (false);
	}
	
	public void deleteTopic(int id){
		Debug.Log ("Button clicked, try to delete Topic with id "+id);	
		dbinterface.deleteClassTopic ("deletedTopic", id, gameObject);
	}
	
	public void showStudentList(){
		Debug.Log ("Button clicked, show panel studentList");	
		dbinterface.getClassUsers("studentlist", class_id, gameObject);
	}
	
	public void acceptStudent(Student s){
		Debug.Log ("Button clicked, try to accept user");	
		dbinterface.acceptUserInClass("acceptedStudent", s.getId(), class_id, gameObject);	
	}
			
	public void setClassId(int id){
		class_id = id;
		
	}
}