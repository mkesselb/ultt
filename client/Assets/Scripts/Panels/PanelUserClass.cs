using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJSON;

public class PanelUserClass : MonoBehaviour {
	
	
	private Main main;
	private DBInterface dbinterface;
	
	public GameObject topic;
	public GameObject btnTask;
	
	public int class_id;
	public int task_id;
	private UserClass userClass;
	
	public Text fieldClassData;
	
	//contain Gameobjects (= buttons)
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
												generatedButton.transform.FindChild("ButtonTask/Text").GetComponent<Text>().text = ts.getTaskName();
												//define button actions: start task and delete task
												int taskId = ts.getTaskId();
												generatedButton.transform.FindChild("ButtonTask").GetComponent<Button>().onClick.AddListener(()=> {startTask(taskId);});
											}
										}
									}
									
								}
							}
							break;
		case "startTask":	
							Task taskToStart = new Task(task_id, parsedData[0]);
							if(taskToStart.getTypeName() == "Zuordnung"){
								main.eventHandler("startTaskAssign", taskToStart.getId());
							}
							if(taskToStart.getTypeName() == "Kategorie"){
								main.eventHandler("startTaskCategory", taskToStart.getId());
							}
							if(taskToStart.getTypeName() == "Quiz"){
								main.eventHandler("startTaskQuiz", taskToStart.getId());
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

	public void startTask(int id){
		task_id = id;
		Debug.Log ("Button clicked, try to start Task");
		dbinterface.getTask ("startTask", id, gameObject);
	}
		
	public void setClassId(int id){
		class_id = id;	
	}
}