using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PanelUserClass : MonoBehaviour {
	
	
	private Main main;
	private DBInterface dbinterface;
	private JSONParser jsonparser;
	
	public GameObject topic;
	public GameObject btnTask;
	
	public int class_id;
	private UserClass userClass;
	
	public Text fieldClassData;
	
	//contain Gameobjects (= buttons)
	public List<GameObject> topics;

	
	
	void Start () {
	
		main = GameObject.Find ("Scripts").GetComponent<Main>();
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();
		jsonparser = GameObject.Find ("Scripts").GetComponent<JSONParser>();

		
		
		topics = new List<GameObject>();

		
		
	}
	
	public void init(){
		//dbinterface.getUserClassData("classData", class_id, gameObject);
		string[] temp = new string[]{"","1","","","","0","","","","","","","","","",""};
		userClass = new UserClass(class_id, temp);
		dbinterface.getTopicsForClass("classTopics", class_id, gameObject); 
			
		//destroy userClassObject because it has wrong id
		//Destroy(userClass);
		//and destroy all generated topics (with their tasks)
		foreach (GameObject t in topics){
			Destroy(t);	
		}
		topics.Clear();	
		
	}
	
	public void dbInputHandler(string[] response){
		Debug.Log ("in dbinputhandler of PanelUserClass");
		string target = response[0];
		string data = response[1];
		GameObject generatedButton;
		GameObject generatedTopic;
		GameObject generatedStudentInList;
		List<string[]> parsedData;
		switch(target){	
		case "classData": 	parsedData = jsonparser.JSONparse(data);
							userClass = new UserClass(class_id, parsedData[0]);
							fieldClassData.GetComponent<Text>().text = userClass.getClassname();
							
							dbinterface.getTopicsForClass("classTopics", class_id, gameObject); 
							break;
		case "classTopics": parsedData = jsonparser.JSONparse(data);
							if(parsedData[0][0] !="[]"){
								foreach( string[] s in parsedData){
									Topic t = new Topic(s);
									Debug.Log ("Topicid: "+t.getId()+" Name: "+t.getName());
									userClass.addTopic(t);
								}
								dbinterface.getTasksForClass("classTasks", class_id, gameObject);
							}
							break;
		case "classTasks":	parsedData = jsonparser.JSONparse(data);
							foreach( string[] s in parsedData){
								//create TaskShort objects (they contain all task data that is needed now), and add it to the teacherClass object
								TaskShort task = new TaskShort(s);
								Debug.Log ("Taskid: "+task.getTaskId());
								userClass.addTask(task);
							}
					
							foreach(Topic t in userClass.getTopicList()){
								//generate topic, add it to hierarchy and change shown text
								generatedTopic = Instantiate(topic, Vector3.zero, Quaternion.identity) as GameObject;
								generatedTopic.transform.parent = GameObject.Find("ContentTasksForTopic").transform;
								generatedTopic.transform.FindChild("TopicHeadline/Text").GetComponent<Text>().text = t.getName();
								
								topics.Add(generatedTopic);
								foreach(TaskShort ts in userClass.getTaskList()){
									//find all tasks that belong to this topic
									if(ts.getTopicId() == t.getId()){
										//generate task, add it to hierarchy and change shown text
										generatedButton = Instantiate(btnTask, Vector3.zero, Quaternion.identity) as GameObject;
										generatedButton.transform.parent = generatedTopic.transform;
										generatedButton.transform.FindChild("Text").GetComponent<Text>().text = ts.getTaskName();
										//define button actions: start task and delete task
										generatedButton.GetComponent<Button>().onClick.AddListener(()=> {startTask(ts.getTaskId(), ts.getTaskType());});
										
									}
								}
				
						}
							
		
											
			
							break;
		
		}	
	}
	
	
	
	public void startTask(int id, string type){
		Debug.Log ("Button clicked, try to start Task");
		if (type == "Quiz") {
			main.eventHandler("startQuiz", id);
		}

	}

	
			
	public void setClassId(int id){
		class_id = id;
		
	}
}
