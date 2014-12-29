using System;
using System.Collections;
using System.Collections.Generic;

public class CategoryData : TaskData{

	public CategoryData (string csv) : base(csv){
		//nothing else
	}

	override public TaskQuestion constructTaskQuestion(string csvLine){
		string[] p = csvLine.Split (new char[]{','});
		List<string> members = new List<string>();
		for(int i = 2; i < p.Length; i++){
			members.Add(CSVHelper.swapDecode(p[i]));
		}
		return new CategoryQuestion(CSVHelper.swapDecode(p[1]), members);
	}

	public List<string> getCategories(){
		List<string> cats = new List<string> ();
		foreach (CategoryQuestion c in taskQuestions) {
			cats.Add(c.getCategoryName());
		}

		return cats;
	}

	public List<string> getAllPhrases(){
		List<string> phr = new List<string> ();
		foreach (CategoryQuestion c in taskQuestions) {
			phr.AddRange((List<string>)c.getAnswer());
		}
		
		return phr;
	}

	public CategoryQuestion getForCategory(string catName){
		foreach (CategoryQuestion c in taskQuestions) {
			if(c.getCategoryName() == catName){
				return c;
			}
		}

		return null;
	}
}