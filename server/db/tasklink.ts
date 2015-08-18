/* module which works the database queries that mostly deal with tasks */
var logger = require('../logging/logging.ts');
var validator = require('../utility/inputvalidator.ts');
var randomstring = require('../utility/randomstring.ts');

/* fetches task information of the specified task. required parameter is task_id */
function getTask(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.task_id)){
		//malformed task_id
		return callback({"error" : 300});
	}
	logger.log(logger.logLevels["debug"], "fetching task information for task " + requestData.task_id);
	
	var fetchTask = "select t.taskname, t.public, t.user_id, t.data_file, s.subject_name, tt.type_name, t.description " 
		+ "from task t, subject s, tasktype tt "
		+ "where task_id = " + requestData.task_id + " and t.subject_id = s.subject_id "
		+ "and t.tasktype_id = tt.tasktype_id and t.deleted = 0";
	
	dbConnection.query(fetchTask, function(err, task){
		if(err){
			return callback(err);
		}
		
		logger.log(logger.logLevels["info"], "successful task data fetching");
		logger.log(logger.logLevels["debug"], "fetch result: " + JSON.stringify(task));
		return callback(null, task);
	});
};

function deleteTask(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.task_id)){
		//malformed task_id
		return callback({"error" : 300});
	}
	logger.log(logger.logLevels["debug"], "deleting task with id " + requestData.task_id);
	
	dbConnection.query("update task set ? where task_id = ?", 
			[{"deleted" : 1}, requestData.task_id], 
			function(err, result){
		if(err){
			return callback(err);
		}
		
		logger.log(logger.logLevels["debug"], "db response: " + JSON.stringify(result));
		logger.log(logger.logLevels["debug"], "successful setting delete flag of task");
		callback(null, [{"success" : 1}]);
	});
}

/* fetches the tasks of the specified user. required parameter is user_id */
function getUserTasks(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.user_id)){
		//malformed user_id
		return callback({"error" : 300});
	}
	logger.log(logger.logLevels["debug"], "fetching tasks for user " + requestData.user_id);
	
	var fetchTasks = "select t.task_id, t.taskname, t.public, s.subject_name, tt.type_name "
		+ "from task t, subject s, tasktype tt "
		+ "where user_id = " + requestData.user_id 
		+ " and t.subject_id = s.subject_id "
		+ "and t.tasktype_id = tt.tasktype_id and t.deleted = 0";
	
	dbConnection.query(fetchTasks, function(err, tasks){
		if(err){
			return callback(err);
		}
		
		logger.log(logger.logLevels["info"], "successful user tasks fetching");
		logger.log(logger.logLevels["debug"], "fetch result: " + JSON.stringify(tasks));
		return callback(null, tasks);
	});
};

/* writes a task with specified data into the db. 
 * required parameter are taskname, public, user_id, subject_id, tasktype_id */
function createTask(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.user_id)){
		//malformed user_id
		return callback({"error" : 300});
	}
	logger.log(logger.logLevels["debug"], "creating task " + requestData.taskname 
			+ " with subject_id " + requestData.subject_id 
			+ " and tasktype_id " + requestData.tasktype_id 
			+ " for user " + requestData.user_id);
	var insertData = {};
	insertData["taskname"] = requestData.taskname;
	insertData["public"] = requestData.public;
	insertData["user_id"] = requestData.user_id;
	insertData["subject_id"] = requestData.subject_id;
	insertData["tasktype_id"] = requestData.tasktype_id;
	//throws error if any referenced id does not exist...but should not happen in normal use
	dbConnection.query("insert into task set ?", insertData, function(err, result){
		if(err){
			return callback(err);
		}
		logger.log(logger.logLevels["debug"], "db response: " + JSON.stringify(result));
		logger.log(logger.logLevels["info"], "successful task creating");
		
		return callback(null, [{"success" : 1}]);
	});
};

/* writes the specified task and topic into the task_for_class relationship.
 * required parameter are class_id, task_id, class_topic_id, obligatory, deadline, max_attempts */
function assignTaskToTopic(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.class_id) 
			|| !validator.validateID(requestData.task_id) 
			|| !validator.validateID(requestData.class_topic_id)){
		//malformed id
		return callback({"error" : 300});
	}
	logger.log(logger.logLevels["debug"], "assign task " + requestData.task_id 
			+ " to class " + requestData.class_id 
			+ " and topic " + requestData.class_topic_id 
			+ " with following parameters: obligatory - " + requestData.obligatory 
			+ ", deadline " + requestData.deadline 
			+ ", max_attempts " + requestData.max_attempts);
	
	//first, perform fetch to see whether the task is already linked to same class+topic
	var fetch = "select task_for_class_id from task_for_class "
		+ "where class_id = " + requestData.class_id 
		+ " and task_id = " + requestData.task_id
		+ " and class_topic_id = " + requestData.class_topic_id
		+ " and deleted = 0";
	
	dbConnection.query(fetch, function(err, result){
		if(err){
			return callback(err);
		}
		
		if(result.length === 0){
			var insertData = {};
			insertData["task_id"] = requestData.task_id;
			insertData["class_id"] = requestData.class_id;
			insertData["class_topic_id"] = requestData.class_topic_id;
			insertData["obligatory"] = requestData.obligatory;
			insertData["deadline"] = requestData.deadline;
			insertData["max_attempts"] = requestData.max_attempts;
			
			dbConnection.query("insert into task_for_class set ?", insertData, function(err, result){
				if(err){
					return callback(err);
				}
				
				logger.log(logger.logLevels["debug"], "db response: " + JSON.stringify(result));
				logger.log(logger.logLevels["info"], "successful task to topic relationship writing");
				
				return callback(null, [{"success" : 1}]);
			});
		} else{
			logger.log(logger.logLevels["warning"], "task_for_class relationship already exists for the selected topic");
			return callback({"error" : 304});
		}
	});
};

/* updates deleted flag from task_for_class relationship.
 * required parameters: class_id, task_id, class_topic_id */
function deleteTaskFromTopic(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.class_topic_id)
			&& !validator.validateID(requestData.task_id)
			&& !validator.validateID(requestData.class_id)){
		//malformed class_topic_id
		return callback({"error" : 300});
	}
	logger.log(logger.logLevels["debug"], "deleting from task_for_class relationship");
	
	var ids = {};
	dbConnection.query("update task_for_class set ? where class_topic_id = ? and task_id = ? and class_id = ?", 
			[{"deleted" : 1}, requestData.class_topic_id, requestData.task_id, requestData.class_id], 
			function(err, result){
		if(err){
			return callback(err);
		}
		
		logger.log(logger.logLevels["debug"], "db response: " + JSON.stringify(result));
		logger.log(logger.logLevels["debug"], "successful setting delete flag of task_for_class");
		callback(null, [{"success" : 1}]);
	});
};

/* edits description and taskdata of specified task_id.
 * required data: task_id, description, data_file. */
function editTask(dbConnection, requestData, callback){
	//write task info + longblob
	//longblob consists of a .csv text
	//maybe, should take task_id, (description) and taskdata
	if(!validator.validateID(requestData.task_id)){
		//malformed task_id
		return callback({"error" : 300});
	}
	
	logger.log(logger.logLevels["debug"], "editing task " + requestData.task_id 
			+ " with data_file " + requestData.data_file);
	
	dbConnection.query("update task set ? where task_id = ?", 
			[{"data_file" : requestData.data_file, "description" : requestData.description}, requestData.task_id],
			function(err, result){
		if(err){
			return callback(err);
		}
		
		logger.log(logger.logLevels["debug"], "db response: " + JSON.stringify(result));
		logger.log(logger.logLevels["debug"], "successful edited task data");
		callback(null, [{"success" : 1}]);
	});
};

/* fetches all task types, that is id+name
 * required data: nothing */
function getTaskTypes(dbConnection, requestData, callback){
	logger.log(logger.logLevels["debug"], "fetching task types");
	
	dbConnection.query("select tasktype_id, type_name from tasktype", function(err, result){
		if(err){
			return callback(err);
		}
		
		logger.log(logger.logLevels["debug"], "db response: " + JSON.stringify(result));
		logger.log(logger.logLevels["debug"], "successful fetched task types");
		callback(null, result);
	});
}

/* fetches all subjects, that is id+name
 * required data: nothing */
function getSubjects(dbConnection, requestData, callback){
	logger.log(logger.logLevels["debug"], "fetching subjects");
	
	dbConnection.query("select subject_id, subject_name from subject", function(err, result){
		if(err){
			return callback(err);
		}
		
		logger.log(logger.logLevels["debug"], "db response: " + JSON.stringify(result));
		logger.log(logger.logLevels["debug"], "successful fetched subjects");
		callback(null, result);
	});
}

/* saves the task execution of a user.
 * required data: user_id, task_for_class_id, results */
function saveTask(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.user_id) || !validator.validateID(requestData.task_for_class_id)){
		//malformed id
		return callback({"error" : 300});
	}
	logger.log(logger.logLevels["debug"], "saving task with user_id " 
			+ requestData.user_id + ", task_for_class_id " + requestData.task_for_class_id);
	
	var insertData = {};
	insertData["user_id"] = requestData.user_id;
	insertData["results"] = requestData.results;
	insertData["task_for_class_id"] = requestData.task_for_class_id;
	dbConnection.query("insert into user_fulfill_task set ?", insertData, function(err, result){
		if(err){
			return callback(err);
		}
		logger.log(logger.logLevels["debug"], "db response: " + JSON.stringify(result));
		logger.log(logger.logLevels["info"], "successful task execution saving");
		
		//fetch and return created topic id
		callback(null, [{"success" : 1}]);
	});
};

/* fetches all results of a single task.
 * required data: class_id, task_id, obligatory */
function getResultOfTask(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.class_id) || !validator.validateID(requestData.task_id)){
		//malformed id
		return callback({"error" : 300});
	}
	logger.log(logger.logLevels["debug"], "fetching task result of class " + requestData.class_id 
			+ ", task " + requestData.task_id);
	
	//first, fetch all task_for_class_id's for this class.
	dbConnection.query("select task_for_class_id from task_for_class where class_id = " + requestData.class_id 
			+ " and task_id = " + requestData.task_id 
			//+ " and obligatory = " + requestData.obligatory 
			+ " and deleted = 0", function(err, ids){
		if(err){
			return callback(err);
		}
		logger.log(logger.logLevels["debug"], "db response: " + JSON.stringify(ids));
		logger.log(logger.logLevels["debug"], "successful fetched task_for_class ids");
		
		if(ids.length > 0){
			var inIds = "(";
			for(i = 0; i < ids.length; i++){
				inIds += ids[i].task_for_class_id;
				if(i < (ids.length-1)){
					inIds += ",";
				}
			}
			inIds += ")";
		} else{
			inIds = "(-1)";
		}
		logger.log(logger.logLevels["debug"], inIds);
		
		dbConnection.query("select u.user_id, u.fulfill_time, u.results, u.task_for_class_id, t.task_id, t.obligatory " +
				"from user_fulfill_task u, task_for_class t " +
				"where u.task_for_class_id in " + inIds +  " and u.task_for_class_id = t.task_for_class_id",
				function(error, result){
			if(error){
				return callback(error);
			}
			logger.log(logger.logLevels["debug"], "db response: " + JSON.stringify(result));
			logger.log(logger.logLevels["debug"], "successful fetched task result");
			
			return callback(null, result);
		});
	});
};

/* fetches the results of all tasks.
 * required data: class_id, obligatory */
function getResultOfTasks(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.class_id)){
		//malformed id
		return callback({"error" : 300});
	}
	logger.log(logger.logLevels["debug"], "fetching task results of class " + requestData.class_id);
	
	//first, fetch all task_for_class_id's for this class.
	dbConnection.query("select task_for_class_id from task_for_class where class_id = " + requestData.class_id 
			//+ " and obligatory = " + requestData.obligatory 
			+ " and deleted = 0", function(err, ids){
		if(err){
			return callback(err);
		}
		logger.log(logger.logLevels["debug"], "db response: " + JSON.stringify(ids));
		logger.log(logger.logLevels["debug"], "successful fetched task_for_class ids");
		
		if(ids.length > 0){
			var inIds = "(";
			for(i = 0; i < ids.length; i++){
				inIds += ids[i].task_for_class_id;
				if(i < (ids.length-1)){
					inIds += ",";
				}
			}
			inIds += ")";
		} else{
			inIds = "(-1)";
		}
		logger.log(logger.logLevels["debug"], inIds);
		
		dbConnection.query("select f.user_id, f.fulfill_time, f.results, t.task_id, t.task_for_class_id, t.obligatory " +
				"from user_fulfill_task f, task_for_class t " +
				"where f.task_for_class_id in " + inIds + " and f.task_for_class_id = t.task_for_class_id",
				function(error, result){
			if(error){
				return callback(error);
			}
			logger.log(logger.logLevels["debug"], "db response: " + JSON.stringify(result));
			logger.log(logger.logLevels["debug"], "successful fetched task results");
			
			return callback(null, result);
		});
	});
};

/* fetches data from the task_for_class relation.
 * required parameter: class_id, task_id, class_topic_id */
function getTaskForClass(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.class_id) || !validator.validateID(requestData.task_id)
			|| !validator.validateID(requestData.class_topic_id)){
		//malformed id
		return callback({"error" : 300});
	}
	logger.log(logger.logLevels["debug"], "fetching task in class relation");
	
	var fetchTask = "select t.taskname, t.public, t.user_id, t.data_file, s.subject_name, tt.type_name, t.description, tc.task_for_class_id, tc.obligatory " 
		+ "from task t, subject s, tasktype tt, task_for_class tc "
		+ "where t.task_id = " + requestData.task_id + " and t.subject_id = s.subject_id "
		+ "and t.tasktype_id = tt.tasktype_id and t.deleted = 0 "
		+ "and tc.task_id = t.task_id and tc.class_id = " + requestData.class_id  
		+ " and tc.class_topic_id = " + requestData.class_topic_id;
	
	dbConnection.query(fetchTask, function(err, result){
		if(err){
			return callback(err)
		}
		logger.log(logger.logLevels["debug"], "db response: " + JSON.stringify(result));
		logger.log(logger.logLevels["debug"], "successful fetched task data");
		
		return callback(null, result);
	});
};

module.exports = {
		getTask				: getTask,
		editTask			: editTask,
		saveTask			: saveTask,
		createTask			: createTask,
		deleteTask			: deleteTask,
		getSubjects			: getSubjects,
		getTaskTypes		: getTaskTypes,
		getUserTasks		: getUserTasks,
		getTaskForClass		: getTaskForClass,
		getResultOfTask		: getResultOfTask,
		getResultOfTasks	: getResultOfTasks,
		assignTaskToTopic	: assignTaskToTopic,
		deleteTaskFromTopic : deleteTaskFromTopic
};