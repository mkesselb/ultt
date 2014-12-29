using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using SimpleJSON;

public class PanelTaskCategory : MonoBehaviour {
	private DBInterface dbinterface;

	public GameObject categoryToggle;
	public GameObject currentPhrase;
	public GameObject statusText;
	public GameObject btnNextPhrase;

	private CategoryData catData;
	private List<string> phrases;

	private double answers;
	private double points;

	public int task_id = 5;

	// Use this for initialization
	void Start () {
		btnNextPhrase.GetComponent<Button> ().onClick.AddListener (() => {invokeNextPhrase ();});
	}

	public void init(){
		//first, destroy all toggles
		for (int i = 0; i < gameObject.transform.FindChild("panelCategories/categories").childCount; i++){
			Destroy(gameObject.transform.FindChild("panelCategories/categories").GetChild (i).gameObject);
		}
		btnNextPhrase.transform.Find ("Text").GetComponent<Text> ().text = "nächstes Wort";

		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();

		answers = -1;
		points = 0;

		dbinterface.getTask ("taskData", task_id, gameObject);
	}

	public void invokeNextPhrase(bool first = false){
		//check current categorization
		if (!first && answers < phrases.Count) {
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

		if (answers < phrases.Count-1) {
			//update status
			answers += 1;
			statusText.GetComponent<Text>().text = "Bearbeitet: " + (answers) + " / " + phrases.Count + "\nRichtig: " + points;

			//get next phrase from list
			currentPhrase.GetComponent<Text>().text = phrases[(int)answers];
		} else {
			answers = phrases.Count;
			//ready to be handed in ;)
			statusText.GetComponent<Text>().text = "Bearbeitet: " + phrases.Count + " / " + phrases.Count + "\nRichtig: " + points;

			btnNextPhrase.transform.Find ("Text").GetComponent<Text> ().text = "Ergebnisse prüfen";
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