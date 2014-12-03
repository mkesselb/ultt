using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PanelFormCategory : MonoBehaviour {
	private DBInterface dbinterface;
	private JSONParser jsonparser;

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
		dbinterface = GameObject.Find ("Scripts").GetComponent<DBInterface>();
		jsonparser = GameObject.Find ("Scripts").GetComponent<JSONParser>();
		btnAddCategory.GetComponent<Button> ().onClick.AddListener (() => {addCategoryForm ();});
		btnSave.GetComponent<Button> ().onClick.AddListener (() => {saveCategories();});
		init ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void init(){

		categories = new List<GameObject> ();
		members = new List<List<GameObject>> ();
		category_id = 0;
		//test id
		task_id = 6;
		//dbinterface.getTask ("taskData", task_id, gameObject);

	}

	public void dbInputHandler(string[] response){
		Debug.Log ("in dbinputhandler of PanelFormCategory");
		string target = response [0];
		string data = response [1];
		List<string[]> parsedData;
		switch (target) {
		case "taskData": parsedData = jsonparser.JSONparse(data);
						foreach (string s in parsedData[0]){
				Debug.Log ("data: "+ s);
						}
						//Task task = new Task(task_id, parsedData[0]); 
						//TODO: get csv data from task
						loadCategoriesFromTask("");
						break;
		}
		}

	public void loadCategoriesFromTask(string csv){
		CategoryData catData = new CategoryData (csv);
		foreach(CategoryQuestion catq in catData.getQuestions()){
			addCategoryForm(catq.getCategoryName());
			foreach(string s in (List<string>)catq.getAnswer()){
				addMemberForm ("category" + category_id, category_id, s);
			}
		}
	}

	public void addMemberForm(string memberName, int id, string memberText = "Phrase für Kategorie"){
		GameObject generatedMember = Instantiate (member, Vector3.zero, Quaternion.identity) as GameObject;
		generatedMember.transform.parent = GameObject.Find(memberName+"/members").transform;
		generatedMember.transform.FindChild ("InputField/Text").GetComponent<Text> ().text = memberText;
		members [id].Add (generatedMember);
	}

	public void addCategoryForm(string catName="Neue Kategorie"){
		GameObject generatedCat = Instantiate (category, Vector3.zero, Quaternion.identity) as GameObject;

		int id = category_id;
		generatedCat.name = "category" + id;
		generatedCat.transform.parent = GameObject.Find ("panelCategories/categories").transform;
		generatedCat.transform.FindChild("ButtonAdd").GetComponent<Button>().onClick.AddListener (() => {addMemberForm(generatedCat.name, id);});	
		generatedCat.transform.FindChild ("catName/Text").GetComponent<Text> ().text = catName;
		categories.Add (generatedCat);
		List<GameObject> membersForCat = new List<GameObject> ();
		members.Add (membersForCat);
		addMemberForm (generatedCat.name, id);
		category_id++;
	}

	public void saveCategories(){
		CategoryData catData = new CategoryData ("");
		for (int i = 0; i < categories.Count; i++){
			//save cat name
			Debug.Log (categories[i].ToString());
			string catName = categories[i].transform.FindChild("catName/Text").GetComponent<Text>().text;
			List<string> catMembers = new List<string>();
			//save members
			List<GameObject> membersList = members[i];
			foreach(GameObject obj in membersList){
				catMembers.Add (obj.transform.FindChild("InputField/Text").GetComponent<Text>().text);
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
