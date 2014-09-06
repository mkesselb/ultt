using UnityEngine;
using System.Collections;

public class DBInterface : MonoBehaviour {
	
	private Main main;
	
	private string url = "127.0.0.1/unity/db";

	// Use this for initialization
	void Start () {
		main = gameObject.GetComponent<Main>();
	}
	
	
	public void getUserData(string target, int id, GameObject receiver){
		WWWForm form = new WWWForm();
		form.AddField("purpose", "get");
		form.AddField("table", "user");
		form.AddField("user_id", id);
        form.AddField("token", "null");
		form.AddField("username", "null");
		form.AddField("password", "null");
		form.AddField("name_first", "null");
		form.AddField("name_last", "null");
		form.AddField("email_id", "null");
		form.AddField("created_at", "null");
		form.AddField("school_id", "null");
		
        WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, target, receiver));
		
	}
	
	public void sendLogInData(string target, string username, string password, GameObject receiver){
		Debug.Log ("DBInterface: sendLogInData(..)");
		WWWForm form = new WWWForm();
		form.AddField("username", username);
		form.AddField("password", password);

        WWW www = new WWW(url+"/login", form);
		StartCoroutine(WaitForRequest(www, target, receiver));
		
	}
	
	public void GetMyCourses(string target, int userid, GameObject receiver){
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
		StartCoroutine(WaitForRequest(www, target, receiver));
	}
	
	//TODO
	public void GetMeineKlassen(string target, int userid, GameObject receiver){}
	public void GetMeineKurse(string target, int userid, GameObject receiver){}
	public void GetMeineTasks(string target, int userid, GameObject receiver){}
	
	
	
	
	
	IEnumerator WaitForRequest(WWW www, string target, GameObject receiver)
    {
        yield return www;
	        if (www.error != null) {
				main.dbErrorHandler(target, www.error);
			} else { 
				Debug.Log ("DBInterface: request received");
				//main.dbInputHandler(target, www.text);
				string[] temp = new string[]{target, www.text};
				receiver.SendMessage("dbInputHandler",temp);
				Debug.Log ("Send message to receiver");
			}  
			
    }    
	
}
