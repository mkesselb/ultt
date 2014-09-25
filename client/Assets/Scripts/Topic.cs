using UnityEngine;
using System.Collections;

public class Topic : MonoBehaviour {

	private int topic_id;
	private string topic_name;
	
	public Topic(string id, string name){
		topic_id = int.Parse(id);
		topic_name = name;
	}
	
	public string getName(){
		return topic_name;	
	}
	
	public int getId(){
		return topic_id;	
	}
}
