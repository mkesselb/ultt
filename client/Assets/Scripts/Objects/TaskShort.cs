using UnityEngine;
using System.Collections;
using SimpleJSON;

/// <summary>
/// Container class, representing minimal view of task objects.
/// </summary>
public class TaskShort{

	/// <summary>
	/// The database-id of the assigned topic.
	/// </summary>
	private int topic_id;

	/// <summary>
	/// The task database-id.
	/// </summary>
	private int task_id;

	/// <summary>
	/// The string name of the task.
	/// </summary>
	private string task_name;

	/// <summary>
	/// The string task type name.
	/// </summary>
	private string task_type;

	/// <summary>
	/// The obligatory flag.
	/// </summary>
	private int obligatory = -1;

	/// <summary>
	/// Alternative constructor, should be avoided if possible.
	/// Initializes a new instance of the <see cref="TaskShort"/> class, by extracting the attribute values from the parameter string array.
	/// </summary>
	/// 
	/// <param name="data">string array of values, representing the values of the task attributes.</param>
	public TaskShort(string[] data){
		task_id = int.Parse(data[1]);
		task_name = data[3];
		topic_id = int.Parse (data[7]);
		task_type = "Quiz";
	}

	/// <summary>
	/// Preferred constructor, should be used whenever possible.
	/// Initializes a new instance of the <see cref="TaskShort"/> class, by extracting the attribute values from the parameter JSON node.
	/// The JSON node holds all class attributes, called by their database-names.
	/// </summary>
	/// 
	/// <param name="task">JSON node of the task, received from the database.</param>
	public TaskShort(JSONNode task){
		task_id = int.Parse (task["task_id"]);
		topic_id = int.Parse (task["class_topic_id"]);
		task_name = task["taskname"];
		task_type = task["type_name"];
	}

	/// <returns>The database-id of the assigned topic.</returns>
	public int getTopicId(){
		return topic_id;	
	}

	/// <returns>The task database-id.</returns>
	public int getTaskId(){
		return task_id;	
	}
	
	/// <returns>The string name of the task.</returns>
	public string getTaskName(){
		return task_name;	
	}
	
	/// <returns>The string name of the task type.</returns>
	public string getTaskType(){
		return task_type;	
	}

	/// <summary>
	/// Sets the obligatory flag.
	/// </summary>
	public void setObligatory(int obligatory){
		this.obligatory = obligatory;
	}

	/// <returns>The obligatory flag.</returns>
	public int getObligatory(){
		return obligatory;
	}
}
