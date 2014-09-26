/* module which works the database queries that mostly deal with classes */
var logger = require('../logging/logging.ts');
var validator = require('../utility/inputvalidator.ts');

/* fetches the users of the specified class. request parameter is class_id. */
function getClassUsers(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.class_id)){
		//malformed class_id
		return callback({"error" : 300});
	}
	logger.log(logger.logLevels["debug"], "get users of class with id: " + requestData.class_id);
	var fetchUsers = "select u.user_id, u.username, uc.accepted "
		+ "from user u, user_is_in_class uc " 
		+ "where uc.class_id = " + requestData.class_id + " and u.user_id = uc.user_id";
	
	dbConnection.query(fetchUsers, function(err, users){
		if(err){
			return callback(err);
		}
		logger.log(logger.logLevels["info"], "successful users fetching");
		logger.log(logger.logLevels["debug"], "fetch result: " + JSON.stringify(users));
		callback(null, users);
	});
};

/* fetches the tasks of the specified class. request parameter is class_id. */
function getClassTasks(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.class_id)){
		//malformed class_id
		return callback({"error" : 300});
	}
	logger.log(logger.logLevels["debug"], "get tasks of class with id: " + requestData.class_id);
	
	//tasktype_id + type_name in table tasktype
	var fetchTasks = "select t.task_id, t.taskname, tt.type_name, tc.class_topic_id "
		+ "from task t, tasktype tt, task_for_class tc "
		+ "where tc.class_id = " + requestData.class_id + " and tc.task_id = t.task_id "
		+ "and t.tasktype_id = tt.tasktype_id";
	
	dbConnection.query(fetchTasks, function(err, tasks){
		if(err){
			return callback(err);
		}
		logger.log(logger.logLevels["info"], "successful tasks fetching");
		logger.log(logger.logLevels["debug"], "fetch result: " + JSON.stringify(tasks));
		callback(null, tasks);
	});
};

/* fetches the topics of the specified class. request parameter is class_id. */
function getClassTopics(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.class_id)){
		//malformed class_id
		return callback({"error" : 300});
	}
	logger.log(logger.logLevels["debug"], "get topics of class with id: " + requestData.class_id);
	//TODO: what order are the topics?
	//maybe impose a creation order or some kind of assigned order... 
	var fetchTopics = "select class_topic_id, topic_name "
			+ "from class_topic where class_id = " + requestData.class_id 
			+ " and deleted = 0";
	
	dbConnection.query(fetchTopics, function(err, topics){
		if(err){
			return callback(err);
		}
		logger.log(logger.logLevels["info"], "successful topics fetching");
		logger.log(logger.logLevels["debug"], "fetch result: " + JSON.stringify(topics));
		callback(null, topics);
	});
};

/* creates a topic for the specified class. request parameter are class_id, topic_name */
function createClassTopic(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.class_id)){
		//malformed class_id
		return callback({"error" : 300});
	}
	logger.log(logger.logLevels["debug"], "creating topic " + requestData.topic_name 
			+ " for class_id " + requestData.class_id);
	//also needs some means of place-distinguish...
	var insertData = {};
	insertData["class_id"] = requestData.class_id;
	insertData["topic_name"] = requestData.topic_name;
	dbConnection.query("insert into class_topic set ?", insertData, function(err, result){
		if(err){
			return callback(err);
		}
		logger.log(logger.logLevels["debug"], "db response: " + JSON.stringify(result));
		logger.log(logger.logLevels["info"], "successful class topic creating");
		
		//fetch and return created topic id
		var t = "select class_topic_id from class_topic where class_id = " + requestData.class_id 
			+ " and topic_name = '" + requestData.topic_name + "'"; 
		dbConnection.query(t, function(err, topic){
			if(err){
				return callback(err);
			}
			logger.log(logger.logLevels["debug"], "created class_topic_id: " + JSON.stringify(topic));
			callback(null, topic);
		});
	});
};

/* sets "deleted" flag of specified class_topic. request parameter is class_topic_id */
function deleteClassTopic(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.class_topic_id)){
		//malformed class_topic_id
		return callback({"error" : 300});
	}
	logger.log(logger.logLevels["debug"], "deleting class topic with id: " + requestData.class_topic_id);
	
	dbConnection.query("update class_topic set ? where class_topic_id = ?", 
			[{"deleted" : 1}, requestData.class_topic_id], 
			function(err, result){
		if(err){
			return callback(err);
		}
		
		logger.log(logger.logLevels["debug"], "db response: " + JSON.stringify(result));
		logger.log(logger.logLevels["debug"], "successful setting delete flag of class_topic");
		callback(null, {"success" : 1});
	});
}

module.exports = {
		getClassUsers	: getClassUsers,
		getClassTasks	: getClassTasks,
		getClassTopics	: getClassTopics,
		createClassTopic: createClassTopic,
		deleteClassTopic: deleteClassTopic
};