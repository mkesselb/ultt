using System;
using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Utility class, providing methods to map database-ids of certain database-entities to their respective string names.
/// This class acts as a single point which fetches and stores ids and names of common objects, so that they do not have to be
/// fetched more often.
/// 
/// Currently, the following entities are supported: subjects, tasktypes.
/// </summary>
public class IdHandler : MonoBehaviour
{
	/// <summary>
	/// The identifier name mapping, in a nested dict. The outer dict maps the name of the type to be mapped.
	/// The inner dict maps the database-id values to the names.
	/// 
	/// This dict has to be dynamically filled from database values on start of the application.
	/// </summary>
	private Dictionary<string, Dictionary<int, string>> idNameMapping = new Dictionary<string, Dictionary<int, string>>();

	/// <summary>
	/// Reference to the database-interface object, which is used for data fetching from the server.
	/// </summary>
	private DBInterface dbinterface;

	/// <summary>
	/// Start this instance, by finding the databse interface object.
	/// </summary>
	void Start(){
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();
	}

	/// <summary>
	/// Setups the mapping dict. Single interface-point for setup.
	/// </summary>
	public void setupMapping(){
		StartCoroutine(setupMappingWait());
	}

	/// <summary>
	/// Asynchronous setup method, fetching the needed database values for setup of the mapping dict.
	/// </summary>
	IEnumerator setupMappingWait(){
		yield return new WaitForSeconds (3);
		this.setupSubjectMapping ();
		this.setupTasktypeMapping ();
	}

	/// <summary>
	/// Setups the subject mapping.
	/// </summary>
	public void setupSubjectMapping(){
		dbinterface.getSubjects ("subjects", gameObject);
	}

	/// <summary>
	/// Setups the tasktype mapping.
	/// </summary>
	public void setupTasktypeMapping(){
		dbinterface.getTaskTypes ("tasktypes", gameObject);
	}

	/// <summary>
	/// Fetches the database-id of a specified entity.
	/// 
	/// If no mapping could be found, returns -1.
	/// </summary>
	/// 
	/// <returns>The database-id specified by parameters.</returns>
	/// 
	/// <param name="name">the name of the entity.</param>
	/// <param name="type">the type of the entity.</param>
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

	/// <summary>
	/// Fetches the name of a specified entity.
	/// 
	/// If no mapping could be found, returns "".
	/// </summary>
	/// 
	/// <returns>The name of the specified entity.</returns>
	/// 
	/// <param name="id">the database-id of the entity.</param>
	/// <param name="type">the type of the entity.</param>
	public string getFromId(int id, string type){
		if (idNameMapping.ContainsKey (type) && idNameMapping [type].ContainsKey (id)) {
			return idNameMapping[type][id];
		}

		return "";
	}

	/// <summary>
	/// Returns a list of all names of the specified type.
	/// Important for the building of selection lists.
	/// </summary>
	/// 
	/// <returns>The list of names of the specified type.</returns>
	/// 
	/// <param name="type">the type of the entities.</param>
	public List<string> getAllNames(string type){
		List<string> vals = new List<string>();
		if (idNameMapping.ContainsKey (type)) {
			foreach(int key in idNameMapping[type].Keys){
				vals.Add(idNameMapping[type][key]);
			}

			return vals;
		}

		return null;
	}

	/// <summary>
	/// Central method to receive and handle database responses.
	/// 
	/// Listens on two responses:
	/// 1) subjects, where the ids and names of subjects are returned.
	/// 2) tasktypes, where the ids and names of task types are returned.
	/// </summary>
	/// 
	/// <param name="response">string array of the server response, passed from the database interface.</param>
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