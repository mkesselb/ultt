using UnityEngine;
using System.Collections;

public class TaskShort : MonoBehaviour {

	private int topic_id;
	private int task_id;
	private string task_name;
	
	public TaskShort(string[] data){
		task_id = int.Parse(data[1]);
		task_name = data[3];
		topic_id = int.Parse (data[7]);
		
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
