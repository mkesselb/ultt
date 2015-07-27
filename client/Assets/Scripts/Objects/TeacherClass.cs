using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

/// <summary>
/// Container class, representing the teacher classes.
/// </summary>
public class TeacherClass{

	/// <summary>
	/// The user_id.
	/// </summary>
	private int user_id;

	/// <summary>
	/// The class_id.
	/// </summary>
	private int class_id;

	/// <summary>
	/// The classname.
	/// </summary>
	private string classname;

	/// <summary>
	/// Privacy flag.
	/// </summary>
	private int privacy;

	/// <summary>
	/// The school_year.
	/// </summary>
	private string school_year;

	/// <summary>
	/// The classcode, randomly created by the server.
	/// </summary>
	private string classcode;

	/// <summary>
	/// The subject_name.
	/// </summary>
	private string subject_name;

	/// <summary>
	/// The list of topics, with which the class is structured.
	/// </summary>
	private List<Topic> topics;

	/// <summary>
	/// The list of tasks of the class.
	/// </summary>
	private List<TaskShort> tasks;

	/// <summary>
	/// The list of students registered for the class.
	/// </summary>
	private List<Student> students;

	/// <summary>
	/// Initializes a new instance of the <see cref="TeacherClass"/> class.
	/// Parses the class data from parameter string array. Should not be used anymore.
	/// </summary>
	/// 
	/// <param name="id">class identifier.</param>
	/// <param name="data">string array of class data.</param>
	public TeacherClass(int id,string[] data){
		user_id = id;
		class_id = int.Parse(data[1]);
		classname = data[3];
		if(data[5] != "null"){
			privacy = int.Parse(data[5]);
		}
		school_year = data[7];
		classcode= data[9];
		subject_name = data[11];
		topics = new List<Topic>();
		tasks = new List<TaskShort>();
		students = new List<Student>();
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="TeacherClass"/> class.
	/// Parses the class data from parameter JSONnode, which is received from the server.
	/// Preferred method for initialization of new classes.
	/// </summary>
	/// 
	/// <param name="id">class identifier.</param>
	/// <param name="clas">JSON node of class data, received from the server.</param>
	public TeacherClass(int id, JSONNode clas){
		if (clas ["user_id"] == null) {
			user_id = id;
			class_id = int.Parse (clas ["class_id"]);
		} else {
			class_id = id;
			user_id = int.Parse(clas ["user_id"]);
		}
		classname = clas ["classname"];
		school_year = clas ["schoolyear"];
		classcode = clas ["classcode"];
		string pr = clas ["privacy"];
		if (pr != "null" && clas["privacy"] != null) {
			privacy = int.Parse(pr);
		}
		subject_name = clas ["subject_name"];
		topics = new List<Topic>();
		tasks = new List<TaskShort>();
		students = new List<Student>();
	}

	/// <returns>The user identifier.</returns>
	public int getUserId(){
		return user_id;
	}

	/// <returns>The classname.</returns>
	public string getClassname(){
		return classname;	
	}

	/// <returns>The class identifier.</returns>
	public int getClassId(){
		return class_id;	
	}

	/// <returns>The class code.</returns>
	public string getClassCode(){
		return classcode;	
	}

	/// <returns>The subject name.</returns>
	public string getSubjectName(){
		return this.subject_name;
	}

	/// <summary>
	/// Adds the topic.
	/// </summary>
	public void addTopic(Topic t){
		topics.Add(t);	
	}

	/// <summary>
	/// Adds the task.
	/// </summary>
	public void addTask(TaskShort t){
		tasks.Add(t);	
	}

	/// <returns>The topic list.</returns>
	public List<Topic> getTopicList(){
		return  topics;	
	}

	/// <returns>The task list.</returns>
	public List<TaskShort> getTaskList(){
		return tasks;	
	}

	/// <returns>The student list.</returns>
	public List<Student> getStudentList(){
		return students;	
	}

	/// <summary>
	/// Adds the student.
	/// </summary>
	public void addStudent(Student s){
		students.Add(s);
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
			}
		}
		return name;
	}

	/// <summary>
	/// Gets the name of the student which matches the parameter user_id.
	/// </summary>
	/// 
	/// <returns>The user name matching the parameter user_id.</returns>
	/// 
	/// <param name="user_id">user_id to be matched.</param>
	public string getUserName(int user_id){
		string name = "";
		foreach(Student s in students){
			if(s.getId() == user_id){
				name = s.getName();
			}
		}
		return name;
	}
}