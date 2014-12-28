using System;
using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;
using System.Collections;

public class IdHandler : MonoBehaviour
{
	private Dictionary<string, Dictionary<int, string>> idNameMapping = new Dictionary<string, Dictionary<int, string>>();
	private DBInterface dbinterface;

	void Start(){
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();
	}

	public void setupMapping(){
		StartCoroutine(setupMappingWait());
	}

	IEnumerator setupMappingWait(){
		yield return new WaitForSeconds (3);
		this.setupSubjectMapping ();
		this.setupTasktypeMapping ();
	}

	public void setupSubjectMapping(){
		dbinterface.getSubjects ("subjects", gameObject);
	}

	public void setupTasktypeMapping(){
		dbinterface.getTaskTypes ("tasktypes", gameObject);
	}

	public int getFromName(string name, string type){
		if (idNameMapping.ContainsKey (type)) {
			foreach(int id in idNameMapping[type].Keys){
				if(idNameMapping[type][id] == name){
					return id;
				}
			}
		}

		return -1;
	}

	public string getFromId(int id, string type){
		if (idNameMapping.ContainsKey (type) && idNameMapping [type].ContainsKey (id)) {
			return idNameMapping[type][id];
		}

		return "";
	}

	public void dbInputHandler(string[] response){
		GameObject generatedBtn;
		string target = response[0];
		string data = response[1];
		JSONNode parsedData = JSONParser.JSONparse(data);
		Dictionary<int, string> dict = new Dictionary<int, string> ();
		switch(target){
			case "subjects":
				for(int i = 0; i < parsedData.Count; i++){
					JSONNode n = parsedData[i];
					dict.Add(int.Parse(n["subject_id"]), n["subject_name"]);
				}
				break;
			case "tasktypes":
				for(int i = 0; i < parsedData.Count; i++){
					JSONNode n = parsedData[i];
					dict.Add(int.Parse(n["tasktype_id"]), n["type_name"]);
				}
				break;
		}

		idNameMapping.Add (target, dict);
	}
}