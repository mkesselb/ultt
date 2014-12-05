using System;
using System.Collections;
using System.Collections.Generic;

public class AssignmentData : TaskData{

	public AssignmentData (string csv) : base(csv){
		//nothing else to do here?!
	}

	override public TaskQuestion constructTaskQuestion(string csvLine){
		string[] p = csvLine.Split (new char[]{','});
		return new AssignmentQuestion(CSVHelper.swapDecode(p[1]),
		                              CSVHelper.swapDecode(p[2]));
	}

	//index parameter is the index of the field, that is either 0 (first) or 1 (second)
	public AssignmentQuestion getQuestionWithField(string field, int index){
		foreach(AssignmentQuestion a in taskQuestions){
			List<string> ans = (List<string>)a.getAnswer();
			if(ans[index].Equals(field)){
				return a;
			}
		}
		//null should not happen in normal program flow, as only the already contained key words should be checked
		return null;
	}
}