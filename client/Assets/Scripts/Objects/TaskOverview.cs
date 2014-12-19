using UnityEngine;
using System.Collections;
using SimpleJSON;

public class TaskOverview{

	private int task_id;
	private string task_name;
	private bool pub;
	private string subject_name;
	private string type_name;

	
	public TaskOverview(string[] data){
		task_id = int.Parse(data[1]);
		task_name = data [3];
	}

	public TaskOverview(JSONNode to){
		task_id = int.Parse (to ["task_id"]);
		task_name = to ["taskname"];
		pub = (1==int.Parse( to ["public"]));
		subject_name = to ["subject_name"];
		type_name = to ["type_name"];
	}
	
	public int getTaskId(){
		return task_id;	
	}

	public string getTypeName(){
		return type_name;
	}

	public string getSubjectName(){
		return subject_name;
	}

	public bool isPublic(){
		return pub;
	}

	public string getTaskName(){
		return task_name;	
	}
	

}
