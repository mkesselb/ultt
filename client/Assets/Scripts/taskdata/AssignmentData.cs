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
	//list contains all questions which have the field in the index
	public List<AssignmentQuestion> getQuestionWithField(string field, int index){
		List<AssignmentQuestion> aq = new List<AssignmentQuestion> ();

		foreach(AssignmentQuestion a in taskQuestions){
			List<string> ans = (List<string>)a.getAnswer();
			if(ans[index].Equals(field)){
				aq.Add (a);
			}
		}

		//list should not be empty!
		return aq;
	}
}