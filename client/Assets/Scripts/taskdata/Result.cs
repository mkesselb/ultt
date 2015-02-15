using System;
using System.Collections.Generic;

public class Result{

	private int user_id;

	private DateTime fulfillTime;

	private int result;

	private String answers;

	private int taskForClassId;

	//private int task_id;

	public Result(int userid, DateTime fulfillT, string res, int taskForClassid){
		this.user_id = userid;
		this.fulfillTime = fulfillT;
		this.result = int.Parse(res.Substring (0, res.IndexOf ("\n")));
		this.answers = res.Substring(res.IndexOf("\n")+1);
		this.taskForClassId = taskForClassid;
		//this.task_id = taskid;
	}

	public int getUserId(){
		return this.user_id;
	}

	public DateTime getFulfillTime(){
		return this.fulfillTime;
	}

	public int getResult(){
		return this.result;
	}

	public string getAnswer(){
		return this.answers;
	}

	public int getTaskForClassId(){
		return this.taskForClassId;
	}

	/*public int getTaskId(){
		return this.task_id;
	}*/
}