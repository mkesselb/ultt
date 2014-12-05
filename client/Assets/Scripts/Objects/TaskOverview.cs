using UnityEngine;
using System.Collections;
using SimpleJSON;

public class TaskOverview{

	private int task_id;
	private string task_name;

	
	public TaskOverview(string[] data){
		task_id = int.Parse(data[1]);
		task_name = data [3];
	}

	public TaskOverview(JSONNode to){
		task_id = int.Parse (to ["task_id"]);
		task_name = to ["taskname"];
	}
	
	public int getTaskId(){
		return task_id;	
	}
	
	public string getTaskName(){
		return task_name;	
	}
	

}
