using UnityEngine;
using System.Collections;
using SimpleJSON;

public class User {
	
	private int user_id;
	private int token;
	private string username;
	private string password;
	private string name_first;
	private string name_last;
	private string email_id;
	private string picture;
	private string created_at;
	private string school_id;
	
	
	//param: string[14] array
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
	
	public int getUserId(){
		return user_id;	
	}
	
	
	public string getFirstName(){
		return name_first;	
	}
	
	public string getLastName(){
		return name_last;	
	}
	
}
