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
}