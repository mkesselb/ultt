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
			                          result["results"], int.Parse(result["task_for_class_id"]), 
			                          int.Parse(result["task_id"])));
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

	public List<Result> extractTaskClassResults(int task_for_class_id, List<Result> res){
		List<Result> results = new List<Result>();
		
		foreach (Result r in res) {
			if(r.getTaskForClassId() == task_for_class_id){
				results.Add(r);
			}
		}
		
		return results;
	}
	
	public List<Result> extractTaskResults(int task_id, List<Result> res){
		List<Result> results = new List<Result>();
		
		foreach (Result r in res) {
			if(r.getTaskId() == task_id){
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

	public List<int> getResultOfStudentOfTask(int user_id, int task_id){
		List<int> res = new List<int> ();
		foreach(Result r in this.getResults()){
			if(r.getTaskId() == task_id && r.getUserId() == user_id){
				res.Add (r.getResult());
			}
		}
		return res;
	}

	public int getAverageResultOfStudent(int user_id){
		int averageResult = 0;
		if (getResultOfStudent (user_id).Count != 0) {
						foreach (Result r in getResultOfStudent (user_id)) {
								averageResult += r.getResult ();
						}
						averageResult = averageResult / getResultOfStudent (user_id).Count;
				}
		return averageResult;
	}

	public int getAverageResultOfTask(int task_id){
		int averageResult = 0;
		if (getResultOfTask (task_id).Count != 0) {
						foreach (Result r in getResultOfTask (task_id)) {
								averageResult += r.getResult ();
						}
						averageResult = averageResult / getResultOfTask (task_id).Count;
		}
		return averageResult;
	}

	public List<Result> getResultOfTask(int task_id){
		List<Result> results = new List<Result>();
		
		foreach (Result r in this.resultList) {
			if(r.getTaskId() == task_id){
				results.Add(r);
			}
		}
		
		return results;
	}

	/*public List<Result> getResultOfTask(int task_for_class_id){
		List<Result> results = new List<Result>();
		
		foreach (Result r in this.resultList) {
			if(r.getTaskForClassId() == task_for_class_id){
				results.Add(r);
			}
		}
		
		return results;
	}*/
}