using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

/// <summary>
/// JSON parser utility class, offering an easy interface to parse JSON responses from the server into JSON structure objects.
/// This object only offers static methods, no initialization is needed.
/// </summary>
public class JSONParser : MonoBehaviour {

	/// <summary>
	/// Parses the parameter string that is received from the database into a JSON structure object,
	/// for further consumption of the calling method.
	/// </summary>
	/// 
	/// <returns>The parent node of the parsed JSON object, representing the received string.</returns>
	/// 
	/// <param name="json">A string of data, received from the database in JSON format.</param>
	public static JSONNode JSONparse(string json){
		Debug.Log ("call parse: " + json);
		return JSON.Parse (json);
	}	
}