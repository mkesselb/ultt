using UnityEngine;
using System.Collections;

public class TaskOverview{

	private int task_id;
	private string task_name;

	
	public TaskOverview(string[] data){
		task_id = int.Parse(data[1]);
		task_name = data [3];
	}

	
	public int getTaskId(){
		return task_id;	
	}
	
	public string getTaskName(){
		return task_name;	
	}
	

}
