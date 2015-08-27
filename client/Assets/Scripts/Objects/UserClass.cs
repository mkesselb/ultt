using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

/// <summary>
/// Container class, representing the general user classes.
/// </summary>
public class UserClass {

	/// <summary>
	/// The database-id of the user, used in various requests.
	/// </summary>
	private int user_id;

	/// <summary>
	/// The database-id of the user class, used in various requests.
	/// </summary>
	private int class_id;

	/// <summary>
	/// The string name of the class.
	/// </summary>
	private string classname;

	/// <summary>
	/// The privacy setting of the class.
	/// </summary>
	private int privacy;

	/// <summary>
	/// The string school year assigned to the class.
	/// </summary>
	private string school_year;

	/// <summary>
	/// The string classcode, used to invite other users to the class.
	/// </summary>
	private string classcode;

	/// <summary>
	/// The string name of the subject of the class.
	/// </summary>
	private string subject_name;

	/// <summary>
	/// The string user name of the class teacher.
	/// </summary>
	private string teacher_username;

	/// <summary>
	/// String of accepted-user status.
	/// </summary>
	private string user_accepted;	

	/// <summary>
	/// List of topics of the user class.
	/// </summary>
	private List<Topic> topics;

	/// <summary>
	/// List of TaskShort objects, representing minimal tasks.
	/// </summary>
	private List<TaskShort> tasks;

	/// <summary>
	/// Alternative constructor, should not be the first choice.
	/// Initializes a new instance of the <see cref="UserClass"/> class, by extracting the attribute values from the parameter string array.
	/// </summary>
	/// 
	/// <param name="id">database-id of the user.</param>
	/// <param name="data">array of strings, representing the values of the user class attributes.</param>
	public UserClass(int id, string[] data){
		user_id = id;
		class_id = int.Parse(data[1]);
		classname = data[3];
		privacy = int.Parse(data[5]);
		school_year = data[7];
		classcode= data[9];
		subject_name = data[11];
		teacher_username = data[13];
		user_accepted = data[15];
		topics = new List<Topic>();
		tasks = new List<TaskShort>();
	}

	/// <summary>
	/// Preferred constructor, should be used whenever possible.
	/// Initializes a new instance of the <see cref="UserClass"/> class, by extracting the attribute values from the parameter JSON node.
	/// The JSON node holds all class attributes, called by their database-names.
	/// </summary>
	/// 
	/// <param name="database-id of the user.</param>
	/// <param name="uc">JSON node of the user class, received from the database.</param>
	public UserClass(int id, JSONNode uc){
		user_id = id;
		class_id = int.Parse (uc ["class_id"]);
		classname = uc ["classname"];
		//privacy = int.Parse (uc ["privacy"]);
		school_year = uc ["school_year"];
		classcode = uc ["classcode"];
		subject_name = uc ["subject_name"];
		teacher_username = uc ["username"];
		user_accepted = uc ["accepted"];
		topics = new List<Topic>();
		tasks = new List<TaskShort>();

	}

	/// <summary>
	/// Secondary constructor, conditionally initializing the class attributes, depending whether the flag parameter is true or not.
	/// Initializes a new instance of the <see cref="UserClass"/> class.
	/// </summary>
	/// 
	/// <param name="class_id">database-id of the represented user class.</param>
	/// <param name="uc">JSON node of the user class, received from the database</param>
	/// <param name="inPanelUserClass">true if the user class is created in the corresponding panel.</param>
	public UserClass(int class_id, JSONNode uc, bool inPanelUserClass){
		if (inPanelUserClass) {
						class_id = class_id;
						user_id = int.Parse (uc ["user_id"]);
						classname = uc ["classname"];
						//privacy = int.Parse (uc ["privacy"]);
						school_year = uc ["school_year"];
						classcode = uc ["classcode"];
						subject_name = uc ["subject_name"];
						//teacher_username = uc ["username"];
						//user_accepted = uc ["accepted"];
						topics = new List<Topic> ();
						tasks = new List<TaskShort> ();
				}
		
	}

	/// <returns>the classname.</returns>
	public string getClassname(){
		return classname;	
	}

	/// <returns>the class identifier.</returns>
	public int getClassId(){
		return class_id;	
	}

	/// <summary>
	/// Adds the parameter topic to the list of class topics.
	/// </summary>
	/// 
	/// <param name="t">topic to be added.</param>
	public void addTopic(Topic t){
		topics.Add(t);	
	}

	/// <summary>
	/// Adds the parameter task to the list of class tasks.
	/// </summary>
	/// 
	/// <param name="t">task to be added.</param>
	public void addTask(TaskShort t){
		tasks.Add(t);	
	}

	/// <returns>the topic list.</returns>
	public List<Topic> getTopicList(){
		return  topics;	
	}

	/// <returns>the task list.</returns>
	public List<TaskShort> getTaskList(){
		return tasks;	
	}

	/// <summary>
	/// Gets the name of the task which matches the parameter task_id.
	/// </summary>
	/// 
	/// <returns>The task name matching the parameter task_id.</returns>
	/// 
	/// <param name="task_id">task_id to be matched.</param>
	public string getTaskName(int task_id){
		string name = "";
		foreach(TaskShort task in tasks){
			if(task.getTaskId() == task_id){
				name = task.getTaskName();
				if(task.getObligatory() >= 0){
					switch(task.getObligatory()){
					case 0:
						name += LocaleHandler.getText("task-exercise");
						break;
					case 1: 
						name += LocaleHandler.getText("task-exam");
						break;
					}
				}
			}
		}
		return name;
	}
}
