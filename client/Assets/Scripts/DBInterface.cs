using UnityEngine;
using System.Collections;

public class DBInterface : MonoBehaviour {
	
	private Main main;
	
	private string baseUrl = "";

	private string url = "";

	void Start () {
		/*if (Application.isWebPlayer) {
			//set db url
			baseUrl = "";
		} else{
			baseUrl = "127.0.0.1/";
		}
		url = baseUrl + "unity/db";*/
		url = "127.0.0.1/unity/db";
		baseUrl = "127.0.0.1/";
		main = gameObject.GetComponent<Main>();
	}
	
	
	public void getUserData(string target, int id, GameObject receiver){
		WWWForm form = new WWWForm();
		form.AddField("method", "getUser");
		form.AddField("user_id", id);
		
        WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));
		
	}
	
	public void sendLogInData(string target, string username, string password, GameObject receiver){
		WWWForm form = new WWWForm();
		form.AddField("username", username);
		form.AddField("password", password);
		Debug.Log ("try to send request with: "+ username+", "+password);
       
		WWW www = new WWW(baseUrl + "login", form);
		StartCoroutine(WaitForRequest(www, target, receiver));
		
	}
	
	public void sendRegisterData(string target, WWWForm form, GameObject receiver){
		 WWW www = new WWW(baseUrl + "register", form);
		StartCoroutine(WaitForRequest(www, target, receiver));	
	}

	public void getMeineKlassen(string target, int userid, GameObject receiver){
		Debug.Log ("called getMeineKlassen");
		WWWForm form = new WWWForm();
		form.AddField("method", "getTeacherClasses");
		form.AddField("user_id", userid);

        WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));
	}
	
	public void getMeineKurse(string target, int userid, GameObject receiver){
		Debug.Log ("called getMeineKurse");
		WWWForm form = new WWWForm();
		form.AddField("method", "getUserClasses");
		form.AddField("user_id", userid);
		
        WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));
	}
	
	public void getMeineTasks(string target, int userid, GameObject receiver){
		Debug.Log ("called getMeineTasks");
		WWWForm form = new WWWForm();
		form.AddField("method", "getUserTasks");
		form.AddField("user_id", userid);
		
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));
	}
	
	//TODO
	public void getTeacherClassData(string target, int classid, GameObject receiver){
		Debug.Log ("dbinterface TODO: method on server");
	}
	
	public void getTopicsForClass(string target, int classid, GameObject receiver){
		//response: class_topic_id, topic_name
		WWWForm form = new WWWForm();
		form.AddField("method", "getClassTopics");
		form.AddField("class_id", classid);
		
        WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));	
	}
	
	public void createClassTopic(string target, int class_id, string topic_name, GameObject receiver){
		//response: class_topic_id
		WWWForm form = new WWWForm();
		form.AddField("method", "createClassTopic");
		form.AddField("class_id", class_id);
		form.AddField("topic_name", topic_name);
		
        WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));	
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
	
	public void createClass(string target, string classname, int user_id, int subject_id, string school_year, GameObject receiver){
		//response: class_id, classcode
		WWWForm form = new WWWForm();
		form.AddField("method", "createClass");
		form.AddField("classname", classname);
		form.AddField ("user_id", user_id);
		form.AddField("subject_id", subject_id);
		form.AddField ("school_year", school_year);
		
        WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));	
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

	public void createTask(string target, string taskname, int pub, int user_id, int subject_id, int tasktype_id, GameObject receiver){
		WWWForm form = new WWWForm();
		form.AddField("method", "createTask");
		form.AddField("taskname", taskname);
		form.AddField("public", pub);
		form.AddField("user_id", user_id);
		form.AddField("subject_id", subject_id);
		form.AddField("tasktype_id", tasktype_id);

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
			Debug.Log ("got data: "+www.text);
				string[] temp = new string[]{target, www.text};
				receiver.SendMessage("dbInputHandler",temp);

			}  
			
    }    
	
}
