using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;

public class DBInterface : MonoBehaviour {
	
	private Main main;
	
	private string baseUrl = "";

	private string url = "";

	void Start () {
		if (Application.isWebPlayer) {
			//set db url
			baseUrl = "";
		} else{
			baseUrl = "127.0.0.1/";
		}
		url = baseUrl + "unity/db";
		//url = "127.0.0.1/unity/db";
		//baseUrl = "127.0.0.1/";
		Debug.Log (url + ";" + baseUrl);
		main = gameObject.GetComponent<Main>();
	}

	public void getSubjects(string target, GameObject receiver){
		WWWForm form = new WWWForm ();
		form.AddField("method", "getSubjects");
		Debug.Log (target);
		WWW www = new WWW (url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));
	}

	public void getTaskTypes(string target, GameObject receiver){
		WWWForm form = new WWWForm ();
		form.AddField("method", "getTaskTypes");
		Debug.Log (target);
		WWW www = new WWW (url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));
	}

	public void getUserData(string target, int id, GameObject receiver){
		WWWForm form = new WWWForm();
		form.AddField("method", "getUser");
		form.AddField("user_id", id);
		
        WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));
	}
	
	public void sendLogInData(string target, Form vform, GameObject receiver){
		if (validateInputForm (vform)) {
			WWWForm form = new WWWForm();
			form.AddField("username", vform.getValue("username"));
			form.AddField("password", vform.getValue("password"));
			
			WWW www = new WWW(baseUrl + "login", form);
			StartCoroutine(WaitForRequest(www, target, receiver));
		}
	}
	
	public void sendRegisterData(string target, Form vform, GameObject receiver){
		if (validateInputForm (vform)) {
			WWWForm form = new WWWForm();
			form.AddField("name_first", vform.getValue("name_first"));
			form.AddField("name_last", vform.getValue("name_last"));
			form.AddField("username", vform.getValue("username"));
			form.AddField("password", vform.getValue("password"));
			form.AddField("email_id", vform.getValue("email_id"));
			form.AddField("school_id", vform.getValue("school_id"));

			WWW www = new WWW(baseUrl + "register", form);
			StartCoroutine(WaitForRequest(www, target, receiver));	
		}
	}

	public void getMeineKlassen(string target, int userid, GameObject receiver){
		WWWForm form = new WWWForm();
		form.AddField("method", "getTeacherClasses");
		form.AddField("user_id", userid);

        WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));
	}
	
	public void getMeineKurse(string target, int userid, GameObject receiver){
		WWWForm form = new WWWForm();
		form.AddField("method", "getUserClasses");
		form.AddField("user_id", userid);
		
        WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));
	}
	
	public void getMeineTasks(string target, int userid, GameObject receiver){
		WWWForm form = new WWWForm();
		form.AddField("method", "getUserTasks");
		form.AddField("user_id", userid);
		
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));
	}

	public void getTeacherClassData(string target, int classid, GameObject receiver){
		WWWForm form = new WWWForm();
		form.AddField("method", "getClass");
		form.AddField("class_id", classid);

		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));	
	}
	
	public void getTopicsForClass(string target, int classid, GameObject receiver){
		//response: class_topic_id, topic_name
		WWWForm form = new WWWForm();
		form.AddField("method", "getClassTopics");
		form.AddField("class_id", classid);
		
        WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));	
	}
	
	public bool createClassTopic(string target, int class_id, Form vform, GameObject receiver){
		//response: class_topic_id
		if (validateInputForm (vform)) {
			WWWForm form = new WWWForm();
			form.AddField("method", "createClassTopic");
			form.AddField("class_id", class_id);
			form.AddField("topic_name", vform.getValue("topic_name"));
			
			WWW www = new WWW(url, form);
			StartCoroutine(WaitForRequest(www, target, receiver));	
			return true;
		}
		return false;
	}
	
	public void deleteClassTopic(string target, int class_topic_id, GameObject receiver){
		//response: {"success": 1}
		WWWForm form = new WWWForm();
		form.AddField("method", "deleteClassTopic");
		form.AddField("class_topic_id", class_topic_id);
		
        WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));	
	}
	
	public void getTasksForClass(string target, int classid, GameObject receiver){
		//response: task_id, taskname, tasktype.type_name, class_topic_id
		WWWForm form = new WWWForm();
		form.AddField("method", "getClassTasks");
		form.AddField("class_id", classid);
		
        WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));	
	}
	
	public void registerUserToClass(string target, int user_id, string classcode, GameObject receiver){
		//response: class_id
		WWWForm form = new WWWForm();
		form.AddField("method", "registerUserToClass");
		form.AddField("user_id", user_id);
		form.AddField("classcode", classcode);
		
        WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));
	}
	
	public void acceptUserInClass(string target, int user_id, int class_id, GameObject receiver){
		//response: {"success": 1}
		WWWForm form = new WWWForm();
		form.AddField("method", "acceptUserInClass");
		form.AddField("user_id", user_id);
		form.AddField("class_id", class_id);
		
        WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));
	}

	public void removeStudentFromClass(string target, int user_id, int class_id, GameObject receiver){
		//response: {"success": 1}
		WWWForm form = new WWWForm();
		form.AddField("method", "removeStudentFromClass");
		form.AddField("user_id", user_id);
		form.AddField("class_id", class_id);
		
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));
	}
	
	public void createClass(string target, Form vform, int user_id, int subject_id, GameObject receiver){
		//response: class_id, classcode
		if (validateInputForm (vform)) {
			WWWForm form = new WWWForm();
			form.AddField("method", "createClass");
			form.AddField("user_id", user_id);
			form.AddField("subject_id", subject_id);
			form.AddField("classname", vform.getValue("classname"));
			form.AddField("school_year", vform.getValue("school_year"));
			
			WWW www = new WWW(url, form);
			StartCoroutine(WaitForRequest(www, target, receiver));	
		}
	}
	
	public void deleteClass(string target, int class_id, GameObject receiver){
		//response: {"success": 1}
		WWWForm form = new WWWForm();
		form.AddField("method", "deleteClass");
		form.AddField("class_id", class_id);
		
        WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));	
	}
	
	public void getClassUsers(string target, int class_id, GameObject receiver){
		//response: user_id, username, user.accepted
		WWWForm form = new WWWForm();
		form.AddField("method", "getClassUsers");
		form.AddField("class_id", class_id);
		
        WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));
	}

	public void createTask(string target, Form vform, int pub, int user_id, int subject_id, int tasktype_id, GameObject receiver){
		if (validateInputForm (vform)) {
			WWWForm form = new WWWForm();
			form.AddField("method", "createTask");
			form.AddField("taskname", vform.getValue("taskname"));
			form.AddField("public", pub);
			form.AddField("user_id", user_id);
			form.AddField("subject_id", subject_id);
			form.AddField("tasktype_id", tasktype_id);
			
			WWW www = new WWW (url, form);
			StartCoroutine(WaitForRequest(www, target, receiver));
		}
	}

	public void deleteTask(string target, int task_id, GameObject receiver){
		WWWForm form = new WWWForm();
		form.AddField("method", "deleteTask");
		form.AddField("task_id", task_id);

		WWW www = new WWW (url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));
	}

	public bool assignTaskToTopic(string target, int class_id, int task_id, int class_topic_id, int obligatory, string deadline, Form vform, GameObject receiver){
		//response: {"success" : 1}
		if (validateInputForm (vform)) {
			WWWForm form = new WWWForm();
			form.AddField("method", "assignTaskToTopic");
			form.AddField("class_id", class_id);
			form.AddField("task_id", task_id);
			form.AddField("class_topic_id", class_topic_id);
			form.AddField("obligatory", obligatory);
			form.AddField("deadline", deadline);
			form.AddField ("max_attempts", vform.getValue("max_attempts"));
			
			WWW www = new WWW (url, form);
			StartCoroutine(WaitForRequest(www, target, receiver));
			return true;
		}
		return false;
	}

	public void deleteTaskFromTopic(string target, int class_id, int task_id, int class_topic_id, GameObject receiver){
		WWWForm form = new WWWForm();
		form.AddField("method", "deleteTaskFromTopic");
		form.AddField("class_id", class_id);
		form.AddField("task_id", task_id);
		form.AddField("class_topic_id", class_topic_id);
		
		WWW www = new WWW (url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));
	}

	public void getTask(string target, int task_id, GameObject receiver){
		//response: taskname, public, user_id, data_file, subject_name, type_name, description
		WWWForm form = new WWWForm();
		form.AddField("method", "getTask");
		form.AddField("task_id", task_id);
		
		WWW www = new WWW (url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));
	}

	public void getTaskForClass(string target, int task_id, int class_id, int class_topic_id, GameObject receiver){
		//response: taskname, public, user_id, data_file, subject_name, type_name, description, task_for_cass_id
		WWWForm form = new WWWForm();
		form.AddField("method", "getTaskForClass");
		form.AddField("task_id", task_id);
		form.AddField("class_id", class_id);
		form.AddField("class_topic_id", class_topic_id);
		
		WWW www = new WWW (url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));
	}

	public void editTask(string target, int task_id, string description, string data_file, GameObject receiver){
		//response: {"success" : 1} 
		WWWForm form = new WWWForm();
		form.AddField("method", "editTask");
		form.AddField("task_id", task_id);
		form.AddField ("description", description);
		form.AddField ("data_file", data_file);
		
		WWW www = new WWW (url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));
	}

	public void saveTask(string target, int user_id, int task_for_class_id, string results, GameObject receiver){
		//response: {"success" : 1}
		WWWForm form = new WWWForm();
		form.AddField("method", "saveTask");
		form.AddField("user_id", user_id);
		form.AddField("task_for_class_id", task_for_class_id);
		form.AddField("results", results);

		WWW www = new WWW (url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));
	}

	public void getResultOfStudents(string target, int class_id, int obligatory, GameObject receiver){
		//response: {"success" : 1} 
		WWWForm form = new WWWForm();
		form.AddField("method", "getResultOfStudents");
		form.AddField("class_id", class_id);
		form.AddField ("obligatory", obligatory);
		
		WWW www = new WWW (url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));
	}

	public void getResultOfStudent(string target, int class_id, int user_id, int obligatory, GameObject receiver){
		//response: {"success" : 1} 
		WWWForm form = new WWWForm();
		form.AddField("method", "getResultOfStudent");
		form.AddField("class_id", class_id);
		form.AddField ("user_id", user_id);
		form.AddField ("obligatory", obligatory);
		
		WWW www = new WWW (url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));
	}
	
	IEnumerator WaitForRequest(WWW www, string target, GameObject receiver)
    {
        yield return www;
		Debug.Log ("WaitForRequest, receiver: "+receiver.ToString()+", data: "+www.text);
        if (www.error != null) {
			main.dbErrorHandler(target, www.error);
		} else { 
			JSONNode resp = JSONParser.JSONparse(www.text);
			if(resp["error"] != null){
				//do not send message to receiver, because there was an error
				//show error box
				main.dbResponseHandler(int.Parse(resp["error"]));
			} else{
				string[] temp = new string[]{target, www.text};
				receiver.SendMessage("dbInputHandler",temp);
			}
		}  
    }

	public bool validateInputForm(Form form){
		//validate method automatically renders errors on input fields, just show any errors here
		Dictionary<string, string> validate = form.validateForm ();
		if (validate.Count == 0) {
			return true;
		} else {
			string errors = LocaleHandler.getText("valid-errors")+ "\n";
			foreach(string s in validate.Keys){
				errors += LocaleHandler.getText(s) + ": " + validate[s] + "\n";
			}
			Debug.Log(errors);
			main.writeToMessagebox(errors);
			return false;
		}
	}
}