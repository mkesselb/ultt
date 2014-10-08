using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JSONParser : MonoBehaviour {

	

	public List<string[]> JSONparse(string json){
		Debug.Log ("call parse");
		
		//parse string into strings for one object, delimiter: },{
		string[] delimiter = {"},{"};
		string[] objectData = json.Split(delimiter, System.StringSplitOptions.RemoveEmptyEntries);
		List<string[]> parsedObjectData = new List<string[]>();
		/*foreach(string s in objectData){
			Debug.Log ("parsing: "+s.ToString());	
		}*/
		int amountOfObjects = objectData.Length;
		
		for (int i = 0; i < amountOfObjects; i++){
			string[] temp = new string[30];
		
			string[] delimiters = { "[{\"", "\":\"", ",\"", "\":", "\",\"", "\"}]", "}]", "}", "{\"", "\"" };
			temp = objectData[i].Split(delimiters,System.StringSplitOptions.RemoveEmptyEntries);
			/*Debug.Log ("----------------------------");
			for (int j = 0; j < temp.Length; j++){
				Debug.Log ("data: "+ temp[j]);
			}*/
			parsedObjectData.Add(temp);
		}	
		
		
		
		return parsedObjectData;
	}
	
}
