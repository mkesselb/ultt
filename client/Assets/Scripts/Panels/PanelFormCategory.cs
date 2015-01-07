using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using SimpleJSON;

public class PanelFormCategory : MonoBehaviour {
	private DBInterface dbinterface;
	private Main main;

	public GameObject category;
	public GameObject member;

	public int task_id;

	public List<GameObject> categories;
	public int category_id;

	//text fields
	public List<List<GameObject>> members;
	public GameObject btnAddCategory;
	public GameObject btnSave;

	// Use this for initialization
	void Start () {
		btnAddCategory.GetComponent<Button> ().onClick.AddListener (() => {addCategoryForm ();});
		btnSave.GetComponent<Button> ().onClick.AddListener (() => {saveCategories();});
		//init ();
	}

	public void init(){
		main = GameObject.Find ("Scripts").GetComponent<Main>();
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();

		foreach (GameObject go in categories) {
			Destroy (go);
		}
		
		btnAddCategory.transform.FindChild("Text").GetComponent<Text>().text
			= LocaleHandler.getText ("button-cat-addcat", main.getLang());
		category.transform.FindChild("ButtonAdd/Text").GetComponent<Text>().text
			= LocaleHandler.getText ("button-cat-addphrase", main.getLang());
		btnSave.transform.FindChild("Text").GetComponent<Text>().text
			= LocaleHandler.getText ("button-cat-save", main.getLang());

		categories = new List<GameObject> ();
		members = new List<List<GameObject>> ();
		category_id = 0;
		dbinterface.getTask ("taskData", task_id, gameObject);
	}

	public void dbInputHandler(string[] response){
		Debug.Log ("in dbinputhandler of PanelFormCategory");
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
		CategoryData catData = new CategoryData (csv);
		if (catData.getQuestions ().Count == 0) {
			addCategoryForm();
		} else {
			foreach(CategoryQuestion catq in catData.getQuestions()){
				addCategoryForm(catq.getCategoryName(), true);
				foreach(string s in (List<string>)catq.getAnswer()){
					addMemberForm ("category" + (category_id-1), category_id-1, s);
				}
			}
		}
	}

	public void addMemberForm(string memberName, int id, string memberText = "Phrase für Kategorie"){
		GameObject generatedMember = Instantiate (member, Vector3.zero, Quaternion.identity) as GameObject;
		generatedMember.transform.parent = GameObject.Find(memberName+"/members").transform;
		generatedMember.transform.FindChild ("InputField").GetComponent<InputField> ().text = memberText;
		members [id].Add (generatedMember);
	}

	public void addCategoryForm(string catName="Neue Kategorie", bool load = false){
		GameObject generatedCat = Instantiate (category, Vector3.zero, Quaternion.identity) as GameObject;

		int id = category_id;
		generatedCat.name = "category" + id;
		generatedCat.transform.parent = GameObject.Find ("panelCategories/categories").transform;
		generatedCat.transform.FindChild("ButtonAdd").GetComponent<Button>().onClick.AddListener (() => {addMemberForm(generatedCat.name, id);});	
		generatedCat.transform.FindChild ("catName").GetComponent<InputField> ().text = catName;
		categories.Add (generatedCat);
		List<GameObject> membersForCat = new List<GameObject> ();
		members.Add (membersForCat);
		if (!load) {
			addMemberForm (generatedCat.name, id);
		}
		category_id++;
	}

	public void saveCategories(){
		CategoryData catData = new CategoryData ("");
		for (int i = 0; i < categories.Count; i++){
			//save cat name
			Debug.Log (categories[i].ToString());
			string catName = categories[i].transform.FindChild("catName").GetComponent<InputField>().text;
			List<string> catMembers = new List<string>();
			//save members
			List<GameObject> membersList = members[i];
			foreach(GameObject obj in membersList){
				catMembers.Add (obj.transform.FindChild("InputField").GetComponent<InputField>().text);
			}
			Debug.Log (catMembers.ToString());
			CategoryQuestion quizQuestion = new CategoryQuestion(catName, catMembers);
			catData.addQuestion(quizQuestion);
		
		}
		//TODO fill in description
		dbinterface.editTask ("editTask", task_id, "", catData.getCSV(), gameObject);
	}

	public void setTaskId(int id){
		task_id = id;
	}
}
