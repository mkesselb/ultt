using UnityEngine;
using System.Collections;
using SimpleJSON;

/// <summary>
/// Container class representing tasks. 
/// </summary>
public class Task {

	/// <summary>
	/// The identifier.
	/// </summary>
	private int id;

	/// <summary>
	/// Int whether the task is public..
	/// </summary>
	private int isPublic;

	/// <summary>
	/// The name.
	/// </summary>
	private string name;

	/// <summary>
	/// The user_id.
	/// </summary>
	private int user_id;

	/// <summary>
	/// The string datafile of the task.
	/// </summary>
	private string datafile;

	/// <summary>
	/// The name of the subject.
	/// </summary>
	private string subjectname;

	/// <summary>
	/// The name of the task type.
	/// </summary>
	private string typename;

	/// <summary>
	/// The task description.
	/// </summary>
	private string description;

	/// <summary>
	/// Initializes a new instance of the <see cref="Task"/> class.
	/// </summary>
	/// 
	/// <param name="task_name">name of the task.</param>
	/// <param name="taskIsPublic">public flag.</param>
	/// <param name="user_id">User_id.</param>
	/// <param name="task_datafile">datafile of the task.</param>
	/// <param name="subjectname">name of the task subject.</param>
	/// <param name="typename">name of the task type.</param>
	/// <param name="task_description">Task_description.</param>
	public Task(string task_name,int taskIsPublic, int user_id, string task_datafile, string subjectname, string typename, string task_description){
		isPublic = taskIsPublic;
		name = task_name;
		this.user_id = user_id;
		datafile = task_datafile;
		this.subjectname = subjectname;
		this.typename = typename;
		description = task_description;
	
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Task"/> class.
	/// Parses minimal data from parameter string array. Should not be used anymore.
	/// </summary>
	/// 
	/// <param name="id">task identifier.</param>
	/// <param name="data">string array of task data.</param>
	public Task(int id, string[] data){
		this.id = id;
		name = data [1];
		this.user_id = int.Parse (data [5]);
		datafile = data [7];
		//this.subjectname = data [7];
		//this.typename = data [9];
		//description = data [11];
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Task"/> class.
	/// Parses the task data from a JSON node, which is received from the server.
	/// Preferred method of initializing tasks.
	/// </summary>
	/// 
	/// <param name="id_">task id.</param>
	/// <param name="task">JSON node representing the task data, received from the server.</param>
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

	/// <summary>
	/// Sets the task identifier.
	/// </summary>
	public void setId(int task_id){
		id = task_id;
	}

	/// <returns>The identifier.</returns>
	public int getId(){
		return id;
	}

	/// <returns>The name of the task.</returns>
	public string getName(){
		return name;
	}

	/// <returns>The user identifier.</returns>
	public int getUserId(){
		return id;
	}

	/// <returns>The string task representation.</returns>
	public string getDatafile(){
		return datafile;
	}

	/// <returns>The name of the subject task.</returns>
	public string getSubjectName(){
		return subjectname;
	}

	/// <returns>The name of the subject type.</returns>
	public string getTypeName(){
		return typename;
	}

	/// <returns>The description of the task.</returns>
	public string getDescription(){
		return description;
	}
}