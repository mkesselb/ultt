using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJSON;

public class PanelUserClass : MonoBehaviour {
	
	/// <summary>
	/// The main class.
	/// </summary>
	private Main main;

	/// <summary>
	/// The database interface.
	/// </summary>
	private DBInterface dbinterface;

	/// <summary>
	/// The topic prefab.
	/// </summary>
	public GameObject topic;

	/// <summary>
	/// The task button prefab.
	/// </summary>
	public GameObject btnTask;

	/// <summary>
	/// The class_id.
	/// </summary>
	public int class_id;

	/// <summary>
	/// The task_id.
	/// </summary>
	public int task_id;

	/// <summary>
	/// The Userclass object.
	/// </summary>
	private UserClass userClass;

	/// <summary>
	/// The class data textfield.
	/// </summary>
	public Text fieldClassData;
	
	/// <summary>
	/// The list of topic objects.
	/// </summary>
	public List<GameObject> topics;

	void Start () {
		main = GameObject.Find ("Scripts").GetComponent<Main>();
		topics = new List<GameObject>();
	}
	
	public void init(){
		//dbinterface.getUserClassData("classData", class_id, gameObject);
		//string[] temp = new string[]{"","1","","","","0","","","","","","","","","",""};
		//userClass = new UserClass(class_id, temp);
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();

		//destroy userClassObject because it has wrong id
		//Destroy(userClass);
		//and destroy all generated topics (with their tasks)
		foreach (GameObject t in topics){
			Destroy(t);	
		}
		topics.Clear();	
		dbinterface.getTeacherClassData("classData", class_id, gameObject); 
	}

	/// <summary>
	/// Handles incoming data from the database
	/// </summary>
	/// 
	/// <param name="response">response data from the database.</param>
	public void dbInputHandler(string[] response){
		string target = response[0];
		string data = response[1];
		Debug.Log ("in dbinputhandler of PanelUserClass with target: "+target);
		GameObject generatedButton;
		GameObject generatedTopic;
		GameObject generatedStudentInList;
		JSONNode parsedData = JSONParser.JSONparse(data);
		switch(target){	
		case "classData": 	
							userClass = new UserClass(class_id, parsedData[0], true);
							fieldClassData.GetComponent<Text>().text = userClass.getClassname();
							
							dbinterface.getTopicsForClass("classTopics", class_id, gameObject); 
							break;
		case "classTopics": if(data != "[]"){
								for(int i = 0; i < parsedData.Count; i++){
									JSONNode n = parsedData[i];
									Topic t = new Topic(n);
									userClass.addTopic(t);
								}
								dbinterface.getTasksForClass("classTasks", class_id, gameObject);
							}
							break;
		case "classTasks":	
							if(data != "[]"){
								for(int i = 0; i < parsedData.Count; i++){
									JSONNode n = parsedData[i];
									TaskShort task = new TaskShort(n);
									task.setObligatory(int.Parse (n["obligatory"]));
									userClass.addTask(task);
								}
							}
							
							if(userClass.getTopicList().Count>0){
								foreach(Topic t in userClass.getTopicList()){
									//generate topic, add it to hierarchy and change shown text
									generatedTopic = Instantiate(topic, Vector3.zero, Quaternion.identity) as GameObject;
									generatedTopic.transform.parent = GameObject.Find("ContentTasksForTopic").transform;
									generatedTopic.transform.FindChild("TopicHeadline/Text").GetComponent<Text>().text = t.getName();
									//define button actions: add task and delete topic
									int topicId = t.getId();
									topics.Add(generatedTopic);
									if(userClass.getTaskList().Count>0){
										foreach(TaskShort ts in userClass.getTaskList()){
											//find all tasks that belong to this topic
											if(ts.getTopicId() == topicId){
												//generate task, add it to hierarchy and change shown text
												generatedButton = Instantiate(btnTask, Vector3.zero, Quaternion.identity) as GameObject;
												generatedButton.transform.parent = generatedTopic.transform;
												generatedButton.transform.FindChild("ButtonTask/Text").GetComponent<Text>().text = userClass.getTaskName(ts.getTaskId());
												//define button actions: start task and delete task
												int taskId = ts.getTaskId();
												generatedButton.transform.FindChild("ButtonTask").GetComponent<Button>().onClick.AddListener(()=> {startTask(taskId, topicId);});
											}
										}
									}
									
								}
							}
							break;
		case "startTask":	
							Task taskToStart = new Task(task_id, parsedData[0]);
							int task_for_class_id = int.Parse (parsedData[0]["task_for_class_id"]);
							if(taskToStart.getTypeName() == "Zuordnung"){
								main.eventHandler("startTaskAssign", task_id, task_for_class_id);
							}
							if(taskToStart.getTypeName() == "Kategorie"){
								main.eventHandler("startTaskCategory", task_id, task_for_class_id);
							}
							if(taskToStart.getTypeName() == "Quiz"){
								main.eventHandler("startTaskQuiz", task_id, task_for_class_id);
							}
							break;
		}	
	}

	/*public void startTask(int id, string type){
		Debug.Log ("Button clicked, try to start Task");
		if (type == "Quiz") {
			main.eventHandler("startQuiz", id);
		}
		if(type == "Zuordnung"){
			main.eventHandler("startTaskAssign", id);
		}
		if(type == "Kategorie"){
			main.eventHandler("startTaskCategory", id);
		}
	}*/

	/// <summary>
	/// Starts task.
	/// </summary>
	/// 
	/// <param name="id">task id.</param>
	/// <param name="topicId">topic id.</param>
	public void startTask(int id, int topicId){
		/*task_id = id;
		Debug.Log ("Button clicked, try to start Task");
		dbinterface.getTask ("startTask", id, gameObject);*/
		Debug.Log ("Button clicked, try to start Task");
		Debug.Log (id + ";" + topicId + ";" + class_id);
		task_id = id;
		//dbinterface.getTask ("startTask", id, gameObject);
		dbinterface.getTaskForClass ("startTask", id, class_id, topicId, gameObject);
	}
		
	/// <summary>
	/// Set class id.
	/// </summary>
	/// 
	/// <param name="id">class id.</param>
	public void setClassId(int id){
		class_id = id;	
	}
}