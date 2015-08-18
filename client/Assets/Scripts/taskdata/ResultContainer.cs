using System;
using System.Collections.Generic;
using SimpleJSON;

/// <summary>
/// Class which manages the results of task execution, so that they can be easily used in panels.
/// </summary>
public class ResultContainer{

	/// <summary>
	/// List of Result objects, representing results of task executions.
	/// </summary>
	private List<Result> resultList;

	/// <summary>
	/// A dict mapping relevant strings for exchanging .csv strings with the server.
	/// </summary>
	private static Dictionary<string, string> csvSwapValues = new Dictionary<string, string>();

	/// <summary>
	/// Initializes a new instance of the <see cref="ResultContainer"/> class.
	/// Fills the list of Result objects, parsing them from the parameter JSON of results.
	/// </summary>
	/// 
	/// <param name="results">JSON of results, received from the server.</param>
	public ResultContainer(JSONNode results){
		this.resultList = new List<Result> ();

		for (int i = 0; i < results.Count; i++) {
			JSONNode result = results[i];
			resultList.Add(new Result(int.Parse(result["user_id"]), DateTime.Parse(result["fulfill_time"]), 
			                          result["results"], int.Parse(result["task_for_class_id"]), 
			                          int.Parse(result["task_id"]),
			                          int.Parse(result["obligatory"])));
		}
	}

	/// <returns>The list of results.</returns>
	public List<Result> getResults(){
		return this.resultList;	
	}

	/// <summary>
	/// From the parameter list of results, extracts only those that match the parameter user id.
	/// </summary>
	/// 
	/// <returns>The extracted student results.</returns>
	/// 
	/// <param name="user_id">id of the user for which the results should be extracted.</param>
	/// <param name="res">list of results.</param>
	public List<Result> extractStudentResults(int user_id, List<Result> res){
		List<Result> results = new List<Result>();

		foreach (Result r in res) {
			if(r.getUserId() == user_id){
				results.Add(r);
			}
		}

		return results;
	}

	/// <summary>
	///	From the parameter list of results, extracts only those that match the parameter task-for-class id.
	/// </summary>
	/// 
	/// <returns>The extracted results.</returns>
	/// 
	/// <param name="task_for_class_id">id of task-for-class relationship.</param>
	/// <param name="res">list of results.</param>
	public List<Result> extractTaskClassResults(int task_for_class_id, List<Result> res){
		List<Result> results = new List<Result>();
		
		foreach (Result r in res) {
			if(r.getTaskForClassId() == task_for_class_id){
				results.Add(r);
			}
		}
		
		return results;
	}

	/// <summary>
	/// From the parameter list of results, extracts only those that match the parameter task id.
	/// </summary>
	/// 
	/// <returns>The extracted results.</returns>
	/// 
	/// <param name="task_id">id of task.</param>
	/// <param name="res">list of results.</param>
	public List<Result> extractTaskResults(int task_id, List<Result> res){
		List<Result> results = new List<Result>();
		
		foreach (Result r in res) {
			if(r.getTaskId() == task_id){
				results.Add(r);
			}
		}
		
		return results;
	}

	/// <summary>
	/// From the internal list of results, returns those that match the parameter user id.
	/// </summary>
	/// 
	/// <returns>The result list for a student.</returns>
	/// 
	/// <param name="user_id">id of user.</param>
	public List<Result> getResultOfStudent(int user_id){
		List<Result> results = new List<Result>();
		
		foreach (Result r in this.resultList) {
			if(r.getUserId() == user_id){
				results.Add(r);
			}
		}
		
		return results;
	}

	/// <summary>
	/// From the internal list of results, returns those that match the parameter user and task ids.
	/// </summary>
	/// 
	/// <returns>The result list, aggregated by student task id.</returns>
	/// 
	/// <param name="user_id">id of user.</param>
	/// <param name="task_id">id of task.</param>
	public List<int> getResultOfStudentOfTask(int user_id, int task_id){
		List<int> res = new List<int> ();
		foreach(Result r in this.getResults()){
			if(r.getTaskId() == task_id && r.getUserId() == user_id){
				res.Add (r.getResult());
			}
		}
		return res;
	}

	/// <summary>
	/// From the internal list of results, aggregates the average result of those matching the parametr user id.
	/// </summary>
	/// 
	/// <returns>The average result for a student.</returns>
	/// 
	/// <param name="user_id">the user id.</param>
	public int getAverageResultOfStudent(int user_id){
		//for each task_for_class, only the best result for the student should be counted
		//map task_for_class_id to results
		Dictionary<int, int> bestResults = new Dictionary<int, int>();
		foreach(Result r in getResultOfStudent(user_id)){
			//for student average results, only count exams:
			if(r.getObligatory() != 1){
				continue;
			}

			int res = r.getResult();
			int task_for_class_id = r.getTaskForClassId();
			if(bestResults.ContainsKey(task_for_class_id)){
				int val = bestResults[task_for_class_id];
				bestResults[task_for_class_id] = (res > val ? res : val);
			} else{
				bestResults.Add(task_for_class_id, res);
			}
		}

		int averageResult = 0;
		if (bestResults.Count != 0) {
						foreach (int r in bestResults.Values){
								averageResult += r;
						}
						averageResult = averageResult / bestResults.Count;
				}
		return averageResult;
	}

	/// <summary>
	/// From the internal list of results, aggregates the average result of those matching the parameter task id.
	/// </summary>
	/// 
	/// <returns>The average result for a task.</returns>
	/// 
	/// <param name="task_id">the task id.</param>
	public int getAverageResultOfTask(int task_id, int numStudents){
		//for each user, only the best result should be counted
		//map user_id to results
		Dictionary<int, int> bestResults = new Dictionary<int, int> ();
		foreach (Result r in getResultOfTask(task_id)){
			int res = r.getResult();
			int user_id = r.getUserId();
			if(bestResults.ContainsKey(user_id)){
				int val = bestResults[user_id];
				bestResults[user_id] = (res > val ? res : val);
			} else{
				bestResults.Add(user_id, res);
			}
		}

		int averageResult = 0;
		if (bestResults.Count != 0 && numStudents != 0) {
						foreach (int r in bestResults.Values) {
								averageResult += r;
						}
				averageResult = averageResult / numStudents;
		}
		return averageResult;
	}

	/// <summary>
	/// From the internal list of results, returns those that match the parameter task id.
	/// </summary>
	/// 
	/// <returns>The result list for the task id.</returns>
	/// 
	/// <param name="task_id">the task id.</param>
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