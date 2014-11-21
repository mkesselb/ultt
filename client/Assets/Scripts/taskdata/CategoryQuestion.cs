using System;
using System.Collections;
using System.Collections.Generic;

public class CategoryQuestion : TaskQuestion{
	string categoryName;
	List<string> categoryMembers;
	
	public CategoryQuestion(string catName, List<string> catMembers) : base(catName){
		categoryName = catName;
		categoryMembers = catMembers;
	}

	public string getCategoryName(){
		return categoryName;
	}

	//checks for contain of single answer, e.g. when single phrases have to be matched to a category
	public double checkSingleAnswer(string answer){
		if (categoryMembers.Contains (answer)) {
			return 1;
		}
		return 0;
	}

	//checks all answers and returns a double fraction of correct answers
	override public double checkAnswer(TaskQuestion answer){
		CategoryQuestion ans = (CategoryQuestion)answer;
		if(!ans.getCategoryName().Equals(categoryName)){
			//false categoryQuestion was called to check answer
			return -1;
		}

		double correct = 0;
		List<string> aw = (List<string>)ans.getAnswer();
		foreach (string s in aw) {
			if(categoryMembers.Contains(s)){
				correct++;
			}
		}
		
		return (correct / categoryMembers.Count);
	}
	
	override public object getAnswer(){
		return categoryMembers;
	}
	
	override public string getCSVRepresentation(){
		string csv = CSVHelper.swapEncode(categoryName);
		foreach(string s in categoryMembers){
			csv += "," + CSVHelper.swapEncode(s);
		}

		return csv;
	}
}