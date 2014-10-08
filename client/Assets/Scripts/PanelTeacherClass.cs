using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PanelTeacherClass : MonoBehaviour {
	
	
	private Main main;
	private DBInterface dbinterface;
	private JSONParser jsonparser;
	
	public GameObject panelStudentList;
	
	public GameObject topic;
	public GameObject btnTask;
	public GameObject studentInList_unaccepted, studentInList_accepted;
	
	public int class_id;
	private TeacherClass teacherClass;
	
	public Text fieldClassData;
	
	//contain Gameobjects (= buttons)
	public List<GameObject> topics;
	public List<GameObject> students;
	
	
	void Start () {
	
		main = GameObject.Find ("Scripts").GetComponent<Main>();
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();
		jsonparser = GameObject.Find ("Scripts").GetComponent<JSONParser>();
		//teacherClass = GameObject.Find("Scripts").GetComponent<TeacherClass>();
		
		
		topics = new List<GameObject>();
		students = new List<GameObject>();
		init ();
		
		
	}
	
	public void init(){
		//TODO dbinterface.getTeacherClassData("classData", class_id, gameObject);
		string[] temp = new string[]{"","1","","","","0","","","","","","","","","",""};
		teacherClass = new TeacherClass(class_id, temp);
		dbinterface.getTopicsForClass("classTopics", class_id, gameObject); 
		
		panelStudentList.SetActive(false);
		
		//destroy teacherClassObject because it has wrong id
		Destroy(teacherClass);
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
		List<string[]> parsedData;
		switch(target){	
		case "classData": 	parsedData = jsonparser.JSONparse(data);
							teacherClass = new TeacherClass(class_id, parsedData[0]);
							fieldClassData.GetComponent<Text>().text = teacherClass.getClassname()+"\nclasscode: "+teacherClass.getClassCode();
							
							dbinterface.getTopicsForClass("classTopics", class_id, gameObject); 
							break;
		case "classTopics": parsedData = jsonparser.JSONparse(data);
							if(parsedData[0][0] !="[]"){
								foreach( string[] s in parsedData){
									Topic t = new Topic(s);
									Debug.Log ("Topicid: "+t.getId()+" Name: "+t.getName());
									teacherClass.addTopic(t);
								}
								dbinterface.getTasksForClass("classTasks", class_id, gameObject);
							}
							break;
		case "classTasks":	parsedData = jsonparser.JSONparse(data);
							foreach( string[] s in parsedData){
								//TODO index out of range
								//create TaskShort objects (they contain all task data that is needed now), and add it to the teacherClass object
								TaskShort task = new TaskShort(s);
								teacherClass.addTask(task);
							}
					
							foreach(Topic t in teacherClass.getTopicList()){
								//generate topic, add it to hierarchy and change shown text
								generatedTopic = Instantiate(topic, Vector3.zero, Quaternion.identity) as GameObject;
								generatedTopic.transform.parent = GameObject.Find("ContentTasksForTopic").transform;
								generatedTopic.transform.FindChild("TopicHeadline/Text").GetComponent<Text>().text = t.getName();
								//define button actions: add task and delete topic
								generatedTopic.transform.FindChild("btnAddTask").GetComponent<Button>().onClick.AddListener(()=> {addTask();});
								generatedTopic.transform.FindChild("TopicHeadline/ButtonDelete").GetComponent<Button>().onClick.AddListener(()=> {deleteTopic(t.getId());});
								topics.Add(generatedTopic);
								foreach(TaskShort ts in teacherClass.getTaskList()){
									//find all tasks that belong to this topic
									if(ts.getTopicId() == t.getId()){
										//generate task, add it to hierarchy and change shown text
										generatedButton = Instantiate(btnTask, Vector3.zero, Quaternion.identity) as GameObject;
										generatedButton.transform.parent = generatedTopic.transform;
										generatedButton.transform.FindChild("ButtonTask/Text").GetComponent<Text>().text = ts.getTaskName();
										//define button actions: start task and delete task
										generatedButton.transform.FindChild("ButtonTask").GetComponent<Button>().onClick.AddListener(()=> {startTask(ts.getTaskId());});
										generatedButton.transform.FindChild("ButtonDelete").GetComponent<Button>().onClick.AddListener(()=> {deleteTask(ts.getTaskId());});
										
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
			
							//parse data in student objects (create student class)
							parsedData = jsonparser.JSONparse(data);
							foreach( string[] s in parsedData){
								Student student = new Student(s);
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
								panelStudentList.SetActive(true);
							}
						
							
							break;
		case "acceptedStudent":
							parsedData = jsonparser.JSONparse(data);
							string[] result = parsedData[0];
							if(result[1] == "1"){ //success
								//refresh overview
								showStudentList();
								
							} else {
								main.dbErrorHandler("acceptedStudent", "Schüler konnte nicht hinzugefügt werden");
							}
							break;
		
		case "addedTopic":
							break;
		}
		
	}
	
	public void addTask(){
		Debug.Log ("Button clicked, try to add Task");
		
	}
	
	
	public void startTask(int id){
		Debug.Log ("Button clicked, try to start Task");
		//Load task with id	
	}
	
	public void deleteTask(int id){
		Debug.Log ("Button clicked, try to delete Task");	
		//delete task
		//refresh panel from db or delete task from panelTeacherClass??
	}
	
	public void addTopic(){
		Debug.Log ("Button clicked, try to add Topic");	
		//dbinterface.createClassTopic("addedTopic", class_id, name, gameObject);	
	}
	
	public void deleteTopic(int id){
		Debug.Log ("Button clicked, try to delete Topic");	
		//delete topic
		//refresh panel from db or delete topic from panelTeacherClass??
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
