using UnityEngine;
using System.Collections;
using SimpleJSON;

/// <summary>
/// Container class, representing a general user.
/// </summary>
public class User {

	/// <summary>
	/// The database-id of the user.
	/// </summary>
	private int user_id;

	/// <summary>
	/// The token.
	/// </summary>
	private int token;

	/// <summary>
	/// The username.
	/// </summary>
	private string username;

	/// <summary>
	/// The password.
	/// </summary>
	private string password;

	/// <summary>
	/// The first name.
	/// </summary>
	private string name_first;

	/// <summary>
	/// The last name.
	/// </summary>
	private string name_last;

	/// <summary>
	/// The email_id.
	/// </summary>
	private string email_id;

	/// <summary>
	/// The picture.
	/// </summary>
	private string picture;

	/// <summary>
	/// The string timestamp of creation.
	/// </summary>
	private string created_at;

	/// <summary>
	/// The school_id.
	/// </summary>
	private string school_id;
	
	
	//param: string[14] array
	/// <summary>
	/// Alternative constructor, should be avoided if possible.
	/// Initializes a new instance of the <see cref="User"/> class, by extracting the attribute values from the parameter string array.
	/// </summary>
	/// 
	/// <param name="id">id</param>
	/// <param name="data">string[14] array, representing the values of the user attributes.</param>
	public User(int id, string[] data){
		user_id = int.Parse(data[1]);
		token = int.Parse(data[3]);
		username = data[5];
		name_first = data[7];
		name_last = data[9];
		email_id = data[11];
		picture = data[13];
		created_at = data[15];
		school_id = data[17];
	}

	/// <summary>
	/// Preferred constructor, should be used whenever possible.
	/// Initializes a new instance of the <see cref="User"/> class, by extracting the attribute values from the parameter JSON node.
	/// The JSON node holds all user attributes, called by their database-names.
	/// </summary>
	/// 
	/// <param name="user">JSON node of the user, received from the database.</param>
	public User(JSONNode user){
		user_id = int.Parse (user ["user_id"]);
		token = int.Parse (user ["token"]);
		username = user ["username"];
		name_first = user ["name_first"];
		name_last = user ["name_last"];
		email_id = user ["email_id"];
		picture = user ["picture"];
		created_at = user ["created_at"];
		school_id = user ["school_id"];
	}

	/// <returns>The database-id of the user.</returns>
	public int getUserId(){
		return user_id;	
	}

	/// <returns>The first name.</returns>
	public string getFirstName(){
		return name_first;	
	}

	/// <returns>The last name.</returns>
	public string getLastName(){
		return name_last;	
	}	
}