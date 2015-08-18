using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using SimpleJSON;

public class PanelTaskCategory : MonoBehaviour {
	/// <summary>
	/// The database interface.
	/// </summary>
	private DBInterface dbinterface;

	/// <summary>
	/// The main class.
	/// </summary>
	private Main main;

	/// <summary>
	/// The toggle object.
	/// </summary>
	public GameObject categoryToggle;

	/// <summary>
	/// The phrase textfield
	/// </summary>
	public GameObject currentPhrase;

	/// <summary>
	/// The status textfield
	/// </summary>
	public GameObject statusText;

	/// <summary>
	/// The next button.
	/// </summary>
	public GameObject btnNextPhrase;

	/// <summary>
	/// The category data.
	/// </summary>
	private CategoryData catData;

	/// <summary>
	/// The list of phrases.
	/// </summary>
	private List<string> phrases;

	/// <summary>
	/// The number of answers.
	/// </summary>
	private double answers;

	/// <summary>
	/// The number of points.
	/// </summary>
	private double points;

	/// <summary>
	/// The correct answers.
	/// </summary>
	private Dictionary<string, List<int>> correctAnswers;

	/// <summary>
	/// The user answers.
	/// </summary>
	private CategoryData userAnswers;

	/// <summary>
	/// The task_id.
	/// </summary>
	public int task_id;

	/// <summary>
	/// The task_for_class_id.
	/// </summary>
	public int task_for_class_id;

	/// <summary>
	/// The user_id.
	/// </summary>
	public int user_id;

	/// <summary>
	/// The teacher flag.
	/// </summary>
	public bool isTeacher;

	// Use this for initialization
	void Start () {
		btnNextPhrase.GetComponent<Button> ().onClick.AddListener (() => {invokeNextPhrase ();});
	}

	public void init(){
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();
		main = GameObject.Find ("Scripts").GetComponent<Main>();

		//first, destroy all toggles
		for (int i = 0; i < gameObject.transform.FindChild("panelCategories/categories").childCount; i++){
			Destroy(gameObject.transform.FindChild("panelCategories/categories").GetChild (i).gameObject);
		}

		answers = -1;
		points = 0;

		gameObject.transform.FindChild("Text").GetComponent<Text> ().text 
			= LocaleHandler.getText ("info-nextword", main.getLang());
		btnNextPhrase.transform.Find ("Text").GetComponent<Text> ().text
			= LocaleHandler.getText ("button-cat-next", main.getLang());

		dbinterface.getTask ("taskData", task_id, gameObject);
	}

	/// <summary>
	/// Invoke next phrase.
	/// </summary>
	///
	/// <param name="first">flag for first phrase.</param>
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

			userAnswers.getForCategory(selectedCat).addMember(answ);
			correctAnswers[selectedCat].Add((int)success);
		}

		if (answers < phrases.Count-1) {
			//update status
			answers += 1;
			statusText.GetComponent<Text>().text = LocaleHandler.getText ("info-cat-num-answers", main.getLang()) 
				+ (answers) + " / " + phrases.Count 
				+ "\n" + LocaleHandler.getText ("info-cat-num-correct", main.getLang()) + points;

			//get next phrase from list
			currentPhrase.GetComponent<Text>().text = phrases[(int)answers];
		} else {
			answers = phrases.Count;
			//ready to be handed in ;)
			statusText.GetComponent<Text>().text = LocaleHandler.getText ("info-cat-num-answers", main.getLang()) 
				+ phrases.Count + " / " + phrases.Count
					+ "\n" + LocaleHandler.getText ("info-cat-num-correct", main.getLang()) + points;

			//btnNextPhrase.transform.Find ("Text").GetComponent<Text> ().text = LocaleHandler.getText ("button-cat-end", main.getLang());
			//automatic handing-in?
			int p = (catData.getFullPoints() == 0 ? 0 : (int)(100 * (double)points / catData.getFullPoints()));
			string results = p + "\n";
			foreach(string key in correctAnswers.Keys){
				results += key + ",";
				foreach(int i in correctAnswers[key]){
					results += i + ",";
				}
			}
			results += "\n" + userAnswers.getCSV();

			if(isTeacher){
				main.writeToMessagebox("Ergebnis: " + points + "/" + catData.getFullPoints());
				finishTask();
			} else{
				dbinterface.saveTask("savedTask", user_id, task_for_class_id, results, gameObject);
			}
		}
	}

	/// <summary>
	/// Handles incoming data from the database
	/// </summary>
	/// 
	/// <param name="response">response data from the database.</param>
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
		case "savedTask":
			main.writeToMessagebox("Ergebnis gespeichert: " + points + "/" + catData.getFullPoints());
			finishTask();
			break;
		}
	}

	/// <summary>
	/// Finishes task.
	/// </summary>
	public void finishTask(){
		main.eventHandler ("finishTask", task_id);
	}

	// <summary>
	/// Loads categories.
	/// </summary>
	/// 
	/// <param name="csv">category data.</param>
	public void loadCategoriesFromTask(string csv){
		this.catData = new CategoryData (csv);
		this.userAnswers = new CategoryData ("");
		this.correctAnswers = new Dictionary<string, List<int>> ();

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

			userAnswers.addQuestion(new CategoryQuestion(s, new List<string>()));
			correctAnswers.Add(s, new List<int>());
		}

		//save all words in list + shuffle
		phrases = catData.getAllPhrases ();
		Shuffle.shuffle (phrases);
		invokeNextPhrase (true);
	}

	/// <summary>
	/// Set task id.
	/// </summary>
	/// 
	/// <param name="id">task id.</param>
	public void setTaskId(int id){
		task_id = id;
	}

	/// <summary>
	/// Set task_for_class_id.
	/// </summary>
	/// 
	/// <param name="id">task_for_class_id.</param>
	public void setTaskForClassId(int id){
		task_for_class_id = id;
	}

	/// <summary>
	/// Set user_id.
	/// </summary>
	/// 
	/// <param name="id">user id.</param>
	public void setUserId(int id){
		user_id = id;
	}

	public void setIsTeacher(bool isteacher){
		this.isTeacher = isteacher;
	}
}