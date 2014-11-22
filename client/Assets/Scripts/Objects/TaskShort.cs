using UnityEngine;
using System.Collections;

public class TaskShort{

	private int topic_id;
	private int task_id;
	private string task_name;
	private string task_type;
	
	public TaskShort(string[] data){
		task_id = int.Parse(data[1]);
		task_name = data[3];
		topic_id = int.Parse (data[7]);
		task_type = "Quiz";
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

	public string getTaskType(){
		return task_type;	
	}
}
