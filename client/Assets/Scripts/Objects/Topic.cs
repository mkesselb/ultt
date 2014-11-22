using UnityEngine;
using System.Collections;

public class Topic : MonoBehaviour {

	private int topic_id;
	private string topic_name;
	
	public Topic(string[] data){
		topic_id = int.Parse(data[1]);
		topic_name = data[3];
	}
	
	public string getName(){
		return topic_name;	
	}
	
	public int getId(){
		return topic_id;	
	}
}
