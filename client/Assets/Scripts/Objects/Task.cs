using UnityEngine;
using System.Collections;

public class Task {

	private int id;
	private int isPublic;
	private string name;
	private int user_id;
	private QuizData datafile;
	private string subjectname;
	private string typename;
	private string description;

	public Task(string task_name,int taskIsPublic, int user_id, QuizData task_datafile, string subjectname, string typename, string task_description){
		isPublic = taskIsPublic;
		name = task_name;
		this.user_id = user_id;
		datafile = task_datafile;
		this.subjectname = subjectname;
		this.typename = typename;
		description = task_description;
	
	}
	public Task(int id, string[] data){
		name = data [1];
		this.user_id = int.Parse (data [3]);
		//datafile = new QuizData(data [5]);
		//this.subjectname = data [7];
		//this.typename = data [9];
		//description = data [11];
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
	public TaskData getDatafile(){
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
