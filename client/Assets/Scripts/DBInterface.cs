using UnityEngine;
using System.Collections;

public class DBInterface : MonoBehaviour {
	
	private Main main;
	
	private string url = "127.0.0.1/unity/db";

	void Start () {
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
       
		WWW www = new WWW("127.0.0.1/login", form);
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
	}
	
	
	
	
	
	IEnumerator WaitForRequest(WWW www, string target, GameObject receiver)
    {
        yield return www;
		//Debug.Log ("WaitForRequest, receiver: "+receiver.ToString()+", data: "+www.text);
	        if (www.error != null) {
				main.dbErrorHandler(target, www.error);
			} else { 
				Debug.Log ("got data: "+www.text);
				string[] temp = new string[]{target, www.text};
				receiver.SendMessage("dbInputHandler",temp);
			}  
			
    }    
	
}
