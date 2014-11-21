using System;

public class AssignmentData : TaskData{

	public AssignmentData (string csv) : base(csv){
		//nothing else to do here?!
	}

	override public TaskQuestion constructTaskQuestion(string csvLine){
		string[] p = csvLine.Split (new Char[]{','});
		return new AssignmentQuestion(CSVHelper.swapDecode(p[1]),
		                              CSVHelper.swapDecode(p[2]));
	}
}