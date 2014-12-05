using UnityEngine;
using System.Collections;
using SimpleJSON;

public class Topic{

	private int topic_id;
	private string topic_name;
	
	public Topic(string[] data){
		topic_id = int.Parse(data[1]);
		topic_name = data[3];
	}

	public Topic(JSONNode topic){
		topic_id = int.Parse (topic ["class_topic_id"]);
		topic_name = topic ["topic_name"];
	}
	
	public string getName(){
		return topic_name;	
	}
	
	public int getId(){
		return topic_id;	
	}
}
