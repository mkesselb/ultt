using System;
using System.Collections;
using System.Collections.Generic;

public class AssignmentQuestion : TaskQuestion{
	private List<string> fields;

	public AssignmentQuestion(string alt1, string alt2) : base(""){
		fields = new List<string> ();
		fields.Add (alt1);
		fields.Add (alt2);
	}

	override public double checkAnswer(TaskQuestion answer){
		AssignmentQuestion ans = (AssignmentQuestion)answer;
		List<string> aw = (List<string>)ans.getAnswer();
		foreach (string s in fields) {
			if(!aw.Contains(s)){
				return 0;
			}
		}

		return 1;
	}

	override public object getAnswer(){
		return fields;
	}
	
	override public string getCSVRepresentation(){
		return CSVHelper.swapEncode(fields[0]) + "," + CSVHelper.swapEncode(fields[1]);
	}

	override public void shuffleAnswers(){
		return;
	}
}