using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using SimpleJSON;

public class PanelTaskCategory : MonoBehaviour {
	//test parameter -> to be deleted if form is attached to rest of app
	private bool first = true;
	private DBInterface dbinterface;

	public GameObject categoryToggle;
	public GameObject currentPhrase;
	public GameObject statusText;
	public GameObject btnNextPhrase;

	private CategoryData catData;
	private List<string> phrases;

	private double answers = -1;
	private double points = 0;

	public int task_id = 5;

	// Use this for initialization
	void Start () {
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();
		btnNextPhrase.GetComponent<Button> ().onClick.AddListener (() => {invokeNextPhrase ();});
	}

	public void init(){
		dbinterface.getTask ("taskData", task_id, gameObject);
	}

	public void invokeNextPhrase(bool first = false){
		if (this.first) {
			this.first = false;
			this.task_id = 5;
			dbinterface.getTask ("taskData", task_id, gameObject);
			return;
		}
		//check current categorization
		if (!first) {
			string answ = currentPhrase.GetComponent<Text> ().text;
			string selectedCat = "";
			for (int i = 0; i < gameObject.transform.FindChild("panelCategories/categories").childCount; i++){
				Transform tr = gameObject.transform.FindChild("panelCategories/categories").GetChild (i);
				if(tr.GetComponent<Toggle>().isOn){
					selectedCat = tr.Find ("Label").GetComponent<Text>().text;
				}
			}
			double success = catData.getForCategory (selectedCat).checkSingleAnswer (answ);
			points += success;
		}

		if (answers < (phrases.Count-1)) {
			//update status
			statusText.GetComponent<Text>().text = "Bearbeitet: " + (++answers) + " / " + phrases.Count + "\nRichtig: " + points;

			//get next phrase from list
			currentPhrase.GetComponent<Text>().text = phrases[(int)answers];
		} else {
			//ready to be handed in ;)
			statusText.GetComponent<Text>().text = "Bearbeitet: " + phrases.Count + " / " + phrases.Count + "\nRichtig: " + points;
			//TODO: automatic handing-in or with button-press?
		}
	}

	public void dbInputHandler(string[] response){
		Debug.Log ("in dbinputhandler of PanelTaskCategory");
		string target = response [0];
		string data = response [1];
		JSONNode parsedData = JSONParser.JSONparse(data);
		switch (target) {
		case "taskData": 
			Task task = new Task(task_id, parsedData[0]); 
			loadCategoriesFromTask(task.getDatafile());
			break;
		}
	}

	public void loadCategoriesFromTask(string csv){
		this.catData = new CategoryData (csv);

		//populate toggle group
		bool first = true;
		foreach (string s in catData.getCategories()) {
			GameObject catT = Instantiate (categoryToggle, Vector3.zero, Quaternion.identity) as GameObject;
			catT.transform.parent = gameObject.transform.FindChild ("panelCategories/categories");
			catT.transform.FindChild ("Label").GetComponent<Text> ().text = s;
			catT.GetComponent<Toggle>().isOn = first;
			catT.GetComponent<Toggle>().group = catT.transform.parent.GetComponent<ToggleGroup>();
			if(first){
				first = false;
			}
		}

		//save all words in list + shuffle
		phrases = catData.getAllPhrases ();
		Shuffle.shuffle (phrases);
		invokeNextPhrase (true);
	}

	public void setTaskId(int id){
		task_id = id;
	}
}