using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PanelTeacherClass : MonoBehaviour {
	
	//TODO get Topics with class_id, generate topic-gameobjects for each topic
	//TODO get Tasks for each topic with topic id and generate button for each task as child of its topic
	
	private Main main;
	private DBInterface dbinterface;
	private JSONParser jsonparser;
	
	public GameObject topic;
	public GameObject btnTask;
	
	public int class_id;
	private TeacherClass teacherClass;
	
	public Text fieldClassData;
	
	private List<GameObject> topics;
	
	
	void Start () {
	
		main = GameObject.Find ("Scripts").GetComponent<Main>();
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();
		jsonparser = GameObject.Find ("Scripts").GetComponent<JSONParser>();
		
		dbinterface.getTeacherClassData("classData", class_id, gameObject);
		
		
		//TEST: generate Topics and Tasks
		topics = new List<GameObject>();
		GameObject generatedTopic;
		for (int i = 0; i < 4; i++){
			generatedTopic = Instantiate(topic, Vector3.zero, Quaternion.identity) as GameObject;
			generatedTopic.transform.parent = GameObject.Find("ContentTasksForTopic").transform;
			generatedTopic.transform.FindChild("TopicHeadline/Text").GetComponent<Text>().text = "Topic "+i;
			topics.Add(generatedTopic);
		}
		
		GameObject generatedButton;
		foreach (GameObject obj in topics){
			for(int i = 0; i < 3; i++){
				generatedButton = Instantiate(btnTask, Vector3.zero, Quaternion.identity) as GameObject;
				generatedButton.transform.parent = obj.transform;
				generatedButton.transform.FindChild("Text").GetComponent<Text>().text = "Start Task "+i;
			}
		}
		
	}
	
	public void dbInputHandler(string[] response){
		Debug.Log ("in dbinputhandler of PanelTeacherClass");
		string target = response[0];
		string data = response[1];
		GameObject generatedButton;
		GameObject generatedTopic;
		List<string[]> parsedData;
		switch(target){	
		case "classData": 	parsedData = jsonparser.JSONparse(data);
							teacherClass = new TeacherClass(class_id, parsedData[0]);
							fieldClassData.GetComponent<Text>().text = teacherClass.getClassname()+"\nclasscode: "+teacherClass.getClassCode();
							//TODO send request for topics 
							break;
		case "classTopics": parsedData = jsonparser.JSONparse(data);
							foreach( string[] s in parsedData){
								Topic t = new Topic(s[0],s[1]);
								teacherClass.addTopic(t);	
							}
							//TODO send request for tasks
							break;
		case "classTasks":	parsedData = jsonparser.JSONparse(data);
							foreach( string[] s in parsedData){
								TaskShort task = new TaskShort(s[0], s[1], s[2]);
								teacherClass.addTask(task);
							}
					
							foreach(Topic t in teacherClass.getTopicList()){
								//generate topic
								generatedTopic = Instantiate(topic, Vector3.zero, Quaternion.identity) as GameObject;
								generatedTopic.transform.parent = GameObject.Find("ContentTasksForTopic").transform;
								generatedTopic.transform.FindChild("TopicHeadline/Text").GetComponent<Text>().text = t.getName();
								topics.Add(generatedTopic);
								foreach(TaskShort ts in teacherClass.getTaskList()){
									if(ts.getTopicId() == t.getId()){
										//generate task
										generatedButton = Instantiate(btnTask, Vector3.zero, Quaternion.identity) as GameObject;
										generatedButton.transform.parent = generatedTopic.transform.parent;
										generatedButton.transform.FindChild("Text").GetComponent<Text>().text = "Start Task "+ts.getTaskName();
								
										generatedButton.GetComponent<Button>().onClick.AddListener(()=> {clickedBtn(ts.getTaskId());});
			
									}
								}
				
							}
								
		
											
			
							break;
		}
		
	}
	
	public void clickedBtn(int id){
		//Load task with id	
	}
	
			
	public void setClassId(int id){
		class_id = id;
	}
}
