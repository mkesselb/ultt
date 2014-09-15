using UnityEngine;
using System.Collections;

public class UserClass {

	private int user_id;
	private int class_id;
	private string classname;
	private int privacy;
	private string school_year;
	private string classcode;
	private string subject_name;
	private string teacher_username;
	private string user_accepted;	
	
	public UserClass(int id,string[] data){
		user_id = id;
		class_id = int.Parse(data[1]);
		classname = data[3];
		privacy = int.Parse(data[5]);
		school_year = data[7];
		classcode= data[9];
		subject_name = data[11];
		teacher_username = data[13];
		user_accepted = data[15];
	}
	
}
