﻿using UnityEngine;
using System.Collections;

public class Student : MonoBehaviour {

	private int user_id;
	private string username;
	private string accepted;
	
	public Student(string[] data){
		user_id = int.Parse (data[1]);
		username = data[3];
		accepted = data[5];
		Debug.Log ("new student: "+user_id+", "+username+", "+accepted);
	}
	
	public string getName(){
		return username;	
	}
	
	public int getId(){
		return user_id;	
	}
	
	public bool isAccepted(){
		if(accepted == "0"){
			return false;	
		} else {
			return true;	
		}
			
	}
}