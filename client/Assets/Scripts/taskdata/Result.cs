using System;
using System.Collections.Generic;

/// <summary>
/// Container class, representing results of task executions of users.
/// </summary>
public class Result{

	/// <summary>
	/// The database-id of the user.
	/// </summary>
	private int user_id;

	/// <summary>
	/// The DateTime of task fulfillment.
	/// </summary>
	private DateTime fulfillTime;

	/// <summary>
	/// The integer result score.
	/// </summary>
	private int result;

	/// <summary>
	/// A .csv string of the answers of the task result.
	/// </summary>
	private String answers;

	/// <summary>
	/// The database-id of the task-for-class relationship.
	/// </summary>
	private int taskForClassId;

	/// <summary>
	/// The database-id of the executed task.
	/// </summary>
	private int task_id;

	/// <summary>
	/// The obligatory flag.
	/// </summary>
	private int obligatory;

	/// <summary>
	/// Initializes a new instance of the <see cref="Result"/> class.
	/// </summary>
	/// 
	/// <param name="userid">id of the user.</param>
	/// <param name="fulfillT">dateTime of fulfillment.</param>
	/// <param name="res">answer string, fetched from the database.</param>
	/// <param name="taskForClassid">id of the task-for-class relationship.</param>
	/// <param name="taskid">id of the task.</param>
	/// <param name="obligatory">obligatory flag</param>
	public Result(int userid, DateTime fulfillT, string res, int taskForClassid, int taskid, int obligatory){
		this.user_id = userid;
		this.fulfillTime = fulfillT;
		this.result = int.Parse(res.Substring (0, res.IndexOf ("\n")));
		this.answers = res.Substring(res.IndexOf("\n")+1);
		this.taskForClassId = taskForClassid;
		this.task_id = taskid;
		this.obligatory = obligatory;
	}

	/// <returns>The database-id of the user.</returns>
	public int getUserId(){
		return this.user_id;
	}

	/// <returns>The fulfill dateTime.</returns>
	public DateTime getFulfillTime(){
		return this.fulfillTime;
	}

	/// <returns>The int task result.</returns>
	public int getResult(){
		return this.result;
	}

	/// <returns>The answer string of the result.</returns>
	public string getAnswer(){
		return this.answers;
	}

	/// <returns>The task for class identifier.</returns>
	public int getTaskForClassId(){
		return this.taskForClassId;
	}

	/// <returns>The database-id of the task.</returns>
	public int getTaskId(){
		return this.task_id;
	}

	/// <returns>The obligatory flag.</returns>
	public int getObligatory(){
		return this.obligatory;
	}
}