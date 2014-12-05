using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class JSONParser : MonoBehaviour {

	public static JSONNode JSONparse(string json){
		Debug.Log ("call parse: " + json);
		return JSON.Parse (json);
	}	
}