using UnityEngine;
using System.Collections;
using SimpleJSON;

/// <summary>
/// Container class which represents a task object on the profile overview.
/// </summary>
public class TaskOverview{

	/// <summary>
	/// The task_id.
	/// </summary>
	private int task_id;

	/// <summary>
	/// The task_name.
	/// </summary>
	private string task_name;

	/// <summary>
	/// Public flag.
	/// </summary>
	private bool pub;

	/// <summary>
	/// The subject_name of the task.
	/// </summary>
	private string subject_name;

	/// <summary>
	/// The type_name of the task.
	/// </summary>
	private string type_name;

	/// <summary>
	/// Initializes a new instance of the <see cref="TaskOverview"/> class.
	/// Data is parsed from parameter string array. Should not be used anymore.
	/// </summary>
	/// 
	/// <param name="data">string array of task data.</param>
	public TaskOverview(string[] data){
		task_id = int.Parse(data[1]);
		task_name = data [3];
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="TaskOverview"/> class.
	/// Data is parsed from parameter JSON node, which is received from the server.
	/// </summary>
	/// 
	/// <param name="to">JSON node received from server.</param>
	public TaskOverview(JSONNode to){
		task_id = int.Parse (to ["task_id"]);
		task_name = to ["taskname"];
		pub = (1==int.Parse( to ["public"]));
		subject_name = to ["subject_name"];
		type_name = to ["type_name"];
	}

	/// <returns>The task identifier.</returns>
	public int getTaskId(){
		return task_id;	
	}

	/// <returns>The type name.</returns>
	public string getTypeName(){
		return type_name;
	}


	/// <returns>The subject name.</returns>
	public string getSubjectName(){
		return subject_name;
	}

	/// <returns><c>true</c>, if public task, <c>false</c> otherwise.</returns>
	public bool isPublic(){
		return pub;
	}

	/// <returns>The task name.</returns>
	public string getTaskName(){
		return task_name;	
	}
}