using System;
using System.Collections;
using System.Collections.Generic;

public class QuizData : TaskData{
	public QuizData(string csv) : base(csv){
		//nothing else
	}

	override public TaskQuestion constructTaskQuestion(string csvLine){
		string[] p = csvLine.Split (new char[]{','});
		List<string> answers = new List<string>();
		List<int> weights = new List<int>();
		for(int i = 2; i < p.Length; i = i+2){
			answers.Add(CSVHelper.swapDecode(p[i]));
			weights.Add(int.Parse(CSVHelper.swapDecode(p[i+1])));
		}
		return new QuizQuestion(CSVHelper.swapDecode(p[1]), answers, weights);
	}

	override public int getFullPoints(){
		//TODO: decide on full points == num questions or num answers?
		return this.taskQuestions.Count;
	}
}