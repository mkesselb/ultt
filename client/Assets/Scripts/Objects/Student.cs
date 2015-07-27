using UnityEngine;
using System.Collections;
using SimpleJSON;

/// <summary>
/// Container class which represents a logged-in student.
/// </summary>
public class Student : MonoBehaviour {

	/// <summary>
	/// The user_id.
	/// </summary>
	private int user_id;

	/// <summary>
	/// The username.
	/// </summary>
	private string username;

	/// <summary>
	/// String whether the student is accepted in the current class.
	/// </summary>
	private string accepted;

	/// <summary>
	/// Initializes a new instance of the <see cref="Student"/> class.
	/// Parses the student data from a string array. Should not be used anymore.
	/// </summary>
	/// 
	/// <param name="data">The string array of student data.</param>
	public Student(string[] data){
		user_id = int.Parse (data[1]);
		username = data[3];
		accepted = data[5];
		Debug.Log ("new student: "+user_id+", "+username+", "+accepted);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Student"/> class.
	/// Parses the student data from a JSON node, which is received from the server.
	/// </summary>
	/// 
	/// <param name="stud">JSON node of student data, received from the server.</param>
	public Student(JSONNode stud){
		user_id = int.Parse(stud["user_id"]);
		username = stud["username"];
		accepted = stud["accepted"];
	}

	/// <returns>The name.</returns>
	public string getName(){
		return username;	
	}

	/// <returns>The identifier.</returns>
	public int getId(){
		return user_id;	
	}

	/// <returns><c>true</c>, if student is currently accepted, <c>false</c> otherwise.</returns>
	public bool isAccepted(){
		if(accepted == "0"){
			return false;	
		} else {
			return true;	
		}			
	}
}