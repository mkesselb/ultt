using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class main : MonoBehaviour {
	
	private string url = "127.0.0.1/unity/db", name = "name", age = "age", data = "name                   age\n\nCorinna                19\nPatze                   37";
		
	void OnGUI() {
		url = GUI.TextArea(new Rect((Screen.width-300)/2,60,300,30),url);
		name = GUI.TextArea(new Rect((Screen.width-100)/2,100,100,30),name);
		age = GUI.TextArea(new Rect((Screen.width-50)/2,140,100,30),age);
		
		GUI.TextArea (new Rect((Screen.width-300)/2,240,300,200),data);	
		
		if(GUI.Button (new Rect((Screen.width-140)/2,200,60,30), "save")) {
			WWWForm form = new WWWForm();
			form.AddField("purpose", "post");
			form.AddField("table", "persons");
	        form.AddField("name", name);
			form.AddField("age", age);
	        WWW www = new WWW(url, form);
			StartCoroutine(WaitForRequest(www, false));
		}
		
		if(GUI.Button (new Rect((Screen.width+20)/2,200,60,30), "show")) {
			WWWForm form = new WWWForm();
			form.AddField("purpose", "get");
			form.AddField("table", "persons");
			form.AddField("name", name);
			form.AddField("age", "null");
			WWW www = new WWW(url, form);
			StartCoroutine(WaitForRequest(www, true));
		}

		if(GUI.Button (new Rect((Screen.width+200)/2,200,60,30), "login")) {
			WWWForm form = new WWWForm();
			form.AddField("username", name);
			form.AddField("password", age);
			WWW www = new WWW(url, form);
			StartCoroutine(WaitForRequestLogin(www));
		}
	}

	IEnumerator WaitForRequestLogin(WWW www)
	{
		yield return www;
		if (www.error == null) {
				Debug.Log ("WWW Ok!: " + www.data);
		} else {
				Debug.Log ("WWW Error: " + www.error);
		}
		data = www.text;
	}
	
	IEnumerator WaitForRequest(WWW www, bool get)
    {
        yield return www;
        if (www.error == null) { Debug.Log("WWW Ok!: " + www.data); }
		else { Debug.Log("WWW Error: "+ www.error); }    
		if(get){
			data = www.text;
		}
    }    

	//TODO: json parsing module, maybe parsing the json strings in classes for better access to the parsed fields
	private string[] parseJSON(string json){
		return Regex.Split(json, "},{");
	}
}