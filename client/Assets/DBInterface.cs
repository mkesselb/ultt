using UnityEngine;
using System.Collections;

public class DBInterface : MonoBehaviour {
	
	private Main main;
	private string url = "127.0.0.1/unity/db";

	// Use this for initialization
	void Start () {
		main = gameObject.GetComponent<Main>();
	}
	
	
	public void getUserData(string target, string username){
		WWWForm form = new WWWForm();
		form.AddField("purpose", "get");//TODO change "tryLogIn" to function on server
		form.AddField("table", "user");
		form.AddField("user_id", "null");
        form.AddField("token", "null");
		form.AddField("username", username);
		form.AddField("password", "null");
		form.AddField("name_first", "null");
		form.AddField("name_last", "null");
		form.AddField("email_id", "null");
		form.AddField("created_at", "null");
		form.AddField("school_id", "null");
		
        WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, target));
		
	}
	
	public void sendLogInData(string target, string username, string password){
		WWWForm form = new WWWForm();
		form.AddField("username", username);
		form.AddField("password", password);

        WWW www = new WWW(url+"/login", form);
		StartCoroutine(WaitForRequest(www, target));
		
	}
	
	public void GetMyCourses(string target, int userid){
		WWWForm form = new WWWForm();
		form.AddField("purpose", "get");//TODO change "tryLogIn" to function on server
		form.AddField("table", "class");
		form.AddField("class_id", "null");
        form.AddField("classname", "null");
		form.AddField("privacy", "null");
		form.AddField("user_id", userid);
		form.AddField("school_year", "null");
		form.AddField("classcode", "null");
		form.AddField("subject_id", "null");
		
        WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, target));
	}
	
	
	
	
	
	
	
	IEnumerator WaitForRequest(WWW www, string target)
    {
        yield return www;
		
        if (www.error != null) {
			main.dbErrorHandler(target, www.error);
		} else { 
			main.dbInputHandler(target, www.text);
		}    	
    }    
	
}
