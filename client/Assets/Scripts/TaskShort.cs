using UnityEngine;
using System.Collections;

public class TaskShort : MonoBehaviour {

	private int topic_id;
	private int task_id;
	private string task_name;
	
	public TaskShort(string top_id, string tas_id, string name){
		topic_id = int.Parse(top_id);
		task_id = int.Parse(tas_id);
		task_name = name;
		
	}
	
	public int getTopicId(){
		return topic_id;	
	}
	
	public int getTaskId(){
		return task_id;	
	}
	
	public string getTaskName(){
		return task_name;	
	}
}
