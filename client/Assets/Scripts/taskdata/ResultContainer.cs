using System;
using System.Collections.Generic;
using SimpleJSON;

public class ResultContainer{

	private List<Result> resultList;

	private static Dictionary<string, string> csvSwapValues = new Dictionary<string, string>();

	public ResultContainer(JSONNode results){
		this.resultList = new List<Result> ();

		for (int i = 0; i < results.Count; i++) {
			JSONNode result = results[i];
			resultList.Add(new Result(int.Parse(result["user_id"]), DateTime.Parse(result["fulfill_time"]), 
			                          result["results"], int.Parse(result["task_for_class_id"])));
		}
	}

	public List<Result> getResults(){
		return this.resultList;	
	}

	public List<Result> extractStudentResults(int user_id, List<Result> res){
		List<Result> results = new List<Result>();

		foreach (Result r in res) {
			if(r.getUserId() == user_id){
				results.Add(r);
			}
		}

		return results;
	}

	public List<Result> extractTaskResults(int task_for_class_id, List<Result> res){
		List<Result> results = new List<Result>();
		
		foreach (Result r in res) {
			if(r.getTaskForClassId() == task_for_class_id){
				results.Add(r);
			}
		}
		
		return results;
	}

	public List<Result> getResultOfStudent(int user_id){
		List<Result> results = new List<Result>();
		
		foreach (Result r in this.resultList) {
			if(r.getUserId() == user_id){
				results.Add(r);
			}
		}
		
		return results;
	}

	public int getAverageResultOfStudent(int user_id){
		int averageResult = 0;
		foreach (Result r in getResultOfStudent (user_id)) {
			averageResult += r.getResult();
		}
		averageResult = averageResult / getResultOfStudent (user_id).Count;
		return averageResult;
	}

	public List<Result> getResultOfTask(int task_for_class_id){
		List<Result> results = new List<Result>();
		
		foreach (Result r in this.resultList) {
			if(r.getTaskForClassId() == task_for_class_id){
				results.Add(r);
			}
		}
		
		return results;
	}
}