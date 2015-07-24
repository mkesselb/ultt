using UnityEngine;
using System.Collections;
using SimpleJSON;

/// <summary>
/// Container class for class topics. They have names assigned and are used to make the class more configurable to the user.
/// </summary>
public class Topic{

	/// <summary>
	/// The database-id of the topic, used in various requests.
	/// </summary>
	private int topic_id;

	/// <summary>
	/// The string name of the topic.
	/// </summary>
	private string topic_name;

	/// <summary>
	/// Alternative constructor, should be avoided if possible.
	/// Initializes a new instance of the <see cref="Topic"/> class, by extracting the attribute values from the parameter string array.
	/// </summary>
	/// 
	/// <param name="data">string array of values, representing the values of the topic attributes.</param>
	public Topic(string[] data){
		topic_id = int.Parse(data[1]);
		topic_name = data[3];
	}

	/// <summary>
	/// Preferred constructor, should be used whenever possible.
	/// Initializes a new instance of the <see cref="Topic"/> class, by extracting the attribute values from the parameter JSON node.
	/// The JSON node holds all topic attributes, called by their database-names.
	/// </summary>
	/// 
	/// <param name="topic">JSON node of the topic, received from the database.</param>
	public Topic(JSONNode topic){
		topic_id = int.Parse (topic ["class_topic_id"]);
		topic_name = topic ["topic_name"];
	}

	/// <returns>The name of the topic.</returns>
	public string getName(){
		return topic_name;	
	}

	/// <returns>The database-id of the topic.</returns>
	public int getId(){
		return topic_id;	
	}
}