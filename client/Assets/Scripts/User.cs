using UnityEngine;
using System.Collections;

public class User {
	
	private int user_id;
	private int token;
	private string name_first;
	private string name_last;
	private string email_id;
	private string created_at;
	private string school_id;
	
	
	//param: string[14] array
	public User(string[] data){
		user_id = int.Parse(data[1]);
		token = int.Parse(data[3]);
		name_first = data[5];
		name_last = data[7];
		email_id = data[9];
		created_at = data[11];
		school_id = data[13];
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
