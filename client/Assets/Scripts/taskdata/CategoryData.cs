using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class which represents category tasks. It provides methods to gather and manage individual task questions of category tasks.
/// </summary>
public class CategoryData : TaskData{

	/// <summary>
	/// Initializes a new instance of the <see cref="CategoryData"/> class.
	/// The parameter .csv string is parsed to construct the task questions.
	/// </summary>
	/// 
	/// <param name="csv">csv string of an category task.</param>
	public CategoryData (string csv) : base(csv){
		//nothing else
	}

	/// <summary>
	/// Constructs the task question from parameter .csv line.
	/// See the CategoryQuestion.getCSVRepresentation() for the csv structure.
	/// </summary>
	/// 
	/// <returns>The task question object of a category.</returns>
	/// 
	/// <param name="csvLine">the .csv string line.</param>
	override public TaskQuestion constructTaskQuestion(string csvLine){
		string[] p = csvLine.Split (new char[]{','});
		List<string> members = new List<string>();
		for(int i = 2; i < p.Length; i++){
			members.Add(CSVHelper.swapDecode(p[i]));
		}
		return new CategoryQuestion(CSVHelper.swapDecode(p[1]), members);
	}

	/// <returns>A list of string category names.</returns>
	public List<string> getCategories(){
		List<string> cats = new List<string> ();
		foreach (CategoryQuestion c in taskQuestions) {
			cats.Add(c.getCategoryName());
		}

		return cats;
	}

	/// <returns>A list of all phrases to be matched.</returns>
	public List<string> getAllPhrases(){
		List<string> phr = new List<string> ();
		foreach (CategoryQuestion c in taskQuestions) {
			phr.AddRange((List<string>)c.getAnswer());
		}
		
		return phr;
	}

	/// <returns>The CategoryQuestion matching the parameter string name.</returns>
	public CategoryQuestion getForCategory(string catName){
		foreach (CategoryQuestion c in taskQuestions) {
			if(c.getCategoryName() == catName){
				return c;
			}
		}

		return null;
	}

	/// <returns>The full reachable points - the number of phrases.</returns>
	override public int getFullPoints(){
		int points = 0;

		foreach (TaskQuestion t in this.taskQuestions) {
			points += ((List<String>) t.getAnswer()).Count;
		}

		return points;
	}
}