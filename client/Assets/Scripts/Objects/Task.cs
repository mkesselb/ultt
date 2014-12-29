using UnityEngine;
using System.Collections;
using SimpleJSON;

public class Task {

	private int id;
	private int isPublic;
	private string name;
	private int user_id;
	private string datafile;
	private string subjectname;
	private string typename;
	private string description;

	public Task(string task_name,int taskIsPublic, int user_id, string task_datafile, string subjectname, string typename, string task_description){
		isPublic = taskIsPublic;
		name = task_name;
		this.user_id = user_id;
		datafile = task_datafile;
		this.subjectname = subjectname;
		this.typename = typename;
		description = task_description;
	
	}
	public Task(int id, string[] data){
		this.id = id;
		name = data [1];
		this.user_id = int.Parse (data [5]);
		datafile = data [7];
		//this.subjectname = data [7];
		//this.typename = data [9];
		//description = data [11];
	}

	public Task(int id_, JSONNode task){
		this.id = id_;
		name = task ["taskname"];
		if (task ["public"] != null) {
			isPublic = int.Parse (task ["public"]);
		} else {
			//default public
			isPublic = 1;
		}
		user_id = int.Parse (task ["user_id"]);
		if (task ["data_file"] != null){
			datafile = task ["data_file"];
		} else {
			datafile = "";
		}
		subjectname = task ["subject_name"];
		typename = task ["type_name"];
		description = task ["description"];
	}

	public void setId(int task_id){
		id = task_id;
	}
	public int getId(){
		return id;
	}
	public string getName(){
		return name;
	}
	public int getUserId(){
		return id;
	}
	public string getDatafile(){
		return datafile;
	}
	public string getSubjectName(){
		return subjectname;
	}
	public string getTypeName(){
		return typename;
	}
	public string getDescription(){
		return description;
	}
}