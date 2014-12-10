﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class TeacherClass{

	private int user_id;
	private int class_id;
	private string classname;
	private int privacy;
	private string school_year;
	private string classcode;
	private string subject_name;
	private List<Topic> topics;
	private List<TaskShort> tasks;
	private List<Student> students;
	
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

	public int getUserId(){
		return user_id;
	}

	public string getClassname(){
		return classname;	
	}
	
	public int getClassId(){
		return class_id;	
	}
	
	public string getClassCode(){
		return classcode;	
	}

	public string getSubjectName(){
		return this.subject_name;
	}
	
	public void addTopic(Topic t){
		topics.Add(t);	
	}
	
	public void addTask(TaskShort t){
		tasks.Add(t);	
	}
	
	public List<Topic> getTopicList(){
		return  topics;	
	}
	
	public List<TaskShort> getTaskList(){
		return tasks;	
	}
	
	public List<Student> getStudentList(){
		return students;	
	}
	
	public void addStudent(Student s){
		students.Add(s);
	}
}