/* module which works the database queries that mostly deal with users */
var logger = require('../logging/logging.ts');
var validator = require('../utility/inputvalidator.ts');
var randomstring = require('../utility/randomstring.ts');

/* fetches all user data. request parameter is user_id. */
function getUser(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.user_id)){
		//malformed user_id
		return callback({"error" : 300});
	}
	logger.log(logger.logLevels["debug"], "get user with id: " + requestData.user_id);
	query = "select user_id, token, username, name_first, name_last, email_id, picture, created_at, school_id " +
			"from user where user_id = " + requestData.user_id;
	
	dbConnection.query(query, function(err, user){
		if(err){
			return callback(err);
		}
		logger.log(logger.logLevels["info"], "successful user fetching");
		logger.log(logger.logLevels["debug"], "fetch result: " + JSON.stringify(user));
		callback(null, user);
	});
};

/* fetches all classes where a user is member. request parameter is user_id. */
function getUserClasses(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.user_id)){
		//malformed user_id
		return callback({"error" : 300});
	}
	logger.log(logger.logLevels["debug"], "get classes of user with id: " + requestData.user_id);
	
	var fetchClasses = "select c.class_id, c.classname, c.privacy, " +
			"c.school_year, c.classcode, s.subject_name, us.username, u.accepted "
			+ "from class c, user_is_in_class u, user us, subject s "
			+ "where u.user_id = " + requestData.user_id
			+ " and u.class_id = c.class_id and c.subject_id = s.subject_id and c.user_id = us.user_id "
			+ "and c.deleted = 0";
	
	dbConnection.query(fetchClasses, function(err, classes){
		if(err){
			return callback(err);
		}
		logger.log(logger.logLevels["info"], "successful classes fetching");
		logger.log(logger.logLevels["debug"], "fetch result: " + JSON.stringify(classes));
		callback(null, classes);
	});
};

/* fetches all classes where the specified user is the teacher. request parameter is user_id */
function getTeacherClasses(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.user_id)){
		//malformed user_id
		return callback({"error" : 300});
	}
	logger.log(logger.logLevels["debug"], "get classes of teacher user with id: " + requestData.user_id);
	
	var fetchTeacherClasses = "select c.class_id, c.classname, c.privacy, c.school_year, c.classcode, s.subject_name " +
			"from class c, subject s " +
			"where c.user_id = " + requestData.user_id + " and c.subject_id = s.subject_id and c.deleted = 0";
	
	dbConnection.query(fetchTeacherClasses, function(err, classes){
		if(err){
			return callback(err);
		}
		logger.log(logger.logLevels["info"], "successful teacher classes fetching");
		logger.log(logger.logLevels["debug"], "fetch result: " + JSON.stringify(classes));
		callback(null, classes);
	});
};

/* writes the specified user and class to the "user_is_in_class" relationship, if the entered code
 * matches a classcode.
 * request parameter are: user_id, classcode */
function registerUserToClass(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.user_id)){
		//user_id malformed
		return callback({"error" : 300});
	}
	//first, fetch class_id for the classcode
	dbConnection.query("select class_id from class where classcode = '" + requestData.classcode + "'", 
			function(err, cl){
		if(err){
			return callback(err);
		}
		if(cl.length === 0){
			logger.log(logger.logLevels["debug"], "no class_id found for classcode: " + requestData.classcode);
			return callback({"error" : 301});
		}
		//assuming unique classcode -> only one class can match
		var class_id = cl[0].class_id
		logger.log(logger.logLevels["debug"], "class_id " + class_id + " found for classcode " + requestData.classcode);
	
		//check if user is already registered in class or not
		dbConnection.query("select user_id from user_is_in_class where user_id = " + requestData.user_id 
				+ " and class_id = " + class_id, function(err, user){
			if(err){
				return callback(err);
			}
			if(user.length !== 0){
				logger.log(logger.logLevels["warning"], "user was already registered in class");
				return callback({"error" : 302});
			}
			
			//insert user to be accepted
			var insertData = {}
			insertData["user_id"] = requestData.user_id;
			insertData["class_id"] = class_id;
			insertData["accepted"] = 0;
			
			logger.log(logger.logLevels["debug"], "write user_id " + requestData.user_id 
					+ " and class_id " + class_id + " to user_is_in_class relationship");
			dbConnection.query("insert into user_is_in_class set ?", insertData, function(err, result){
				if(err){
					return callback(err);
				}
				logger.log(logger.logLevels["debug"], "db response: " + JSON.stringify(result));
				logger.log(logger.logLevels["info"], "successful registered user to class");
				callback(null, [{"class_id" : class_id}]);
			});
		});
	});
};

/* writes the acceptance of a user in the "user_is_in_class" relationship.
 * request parameter are: user_id, class_id */
function acceptUserInClass(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.user_id) || !validator.validateID(requestData.class_id)){
		//some id was malformed
		return callback({"error" : 300});
	}
	dbConnection.query("select user_id from user_is_in_class where user_id = " + requestData.user_id 
			+ " and class_id = " + requestData.class_id, function(err, user){
		if(err){
			return callback(err);
		}
		if(user.length === 0){
			logger.log(logger.logLevels["warning"], "user cannot be accepted, is not registered in class");
			return callback({"error" : 303});
		}
		
		//else, accept the user
		logger.log(logger.logLevels["debug"], "accepting user_id " + requestData.user_id 
				+ " into class_id " + requestData.class_id);
		dbConnection.query("update user_is_in_class set ? where user_id = ? and class_id = ?", 
				[{"accepted" : 1}, requestData.user_id, requestData.class_id], 
				function(err, result){
			if(err){
				return callback(err);
			}
			logger.log(logger.logLevels["debug"], "db response: " + JSON.stringify(result));
			logger.log(logger.logLevels["info"], "successful accepted user to class");
			callback(null, [{"status" : 1}]);
		});
	});
};

/* removes parameter student from a class-relationship.
 * required parameter: user_id, class_id */
function removeStudentFromClass(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.user_id) || !validator.validateID(requestData.class_id)){
		//malformed id
		return callback({"error" : 300});
	}
	logger.log(lologger.logLevels["debug"], "deleting user " + requestData.user_id
			+ " from class " + requestData.class_id);
	
	var ids = {};
	ids["user_id"] = requestData.user_id;
	ids["class_id"] = requestData.class_id;
	dbConnection.query("delete from user_is_in_class where ?", ids, function(callback, err){
		if(err){
			return callback(err);
		}
		
		logger.log(logger.logLevels["debug"], "db response: " + JSON.stringify(result));
		logger.log(logger.logLevels["debug"], "successful deleting user from user_is_in_class relation");
		callback(null, [{"success" : 1}]);
	});
};

/* creates a class. 
 * required parameter are classname, user_id (of the teacher), school_year, subject_id */
function createClass(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.user_id) || !validator.validateID(requestData.subject_id)){
		//malformed id
		return callback({"error" : 300});
	}
	logger.log(logger.logLevels["debug"], "creating class " + requestData.classname
			+ " for user_id " + requestData.user_id 
			+ ", subject_id " + requestData.subject_id
			+ " and school_year " + requestData.school_year);
	
	var classcode = randomstring(10);
	var insertData = {};
	insertData["classname"] = requestData.classname;
	insertData["user_id"] = requestData.user_id;
	insertData["school_year"] = requestData.school_year;
	insertData["classcode"] = classcode;
	insertData["subject_id"] = requestData.subject_id;
	logger.log(logger.logLevels["debug"], "insert data: " + JSON.stringify(insertData));
	
	dbConnection.query("insert into class set ?", insertData, function(err, result){
		if(err){
			return callback(err);
		}
		logger.log(logger.logLevels["debug"], "db response: " + JSON.stringify(result));
		logger.log(logger.logLevels["info"], "successful class creating");
		
		//now, fetch id of created class and return it
		dbConnection.query("select class_id from class where classcode ='" + classcode + "'", function(err, cl){
			if(err){
				return callback(err);
			}
			logger.log(logger.logLevels["debug"], "created class with id: " + cl[0].class_id 
					+ ", classcode: " + classcode);
			callback(null, [{"class_id" : cl[0].class_id, "classcode" : classcode}])
		});
	});
};

/* sets deleted flag of specified class. required parameter is class_id */
function deleteClass(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.class_id)){
		//malformed class_id
		return callback({"error" : 300});
	}
	logger.log(logger.logLevels["debug"], "deleting class with id: " + requestData.class_id);
	
	dbConnection.query("update class set ? where class_id = ?", 
			[{"deleted" : 1}, requestData.class_id], 
			function(err, result){
		if(err){
			return callback(err);
		}
		
		logger.log(logger.logLevels["debug"], "db response: " + JSON.stringify(result));
		logger.log(logger.logLevels["debug"], "successful setting delete flag of class");
		callback(null, [{"success" : 1}]);
	});
};

/* fetches the results of a single student.
 * required parameter: class_id, user_id, obligatory */
function getResultOfStudent(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.class_id) || !validator.validateID(requestData.user_id)){
		//malformed id
		return callback({"error" : 300});
	}
	logger.log(logger.logLevels["debug"], "fetching user result of class " + requestData.class_id 
			+ ", user" + requestData.user_id);
	
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
				"where f.user_id = " + requestData.user_id + 
				" and f.task_for_class_id in " + inIds + " and f.task_for_class_id = t.task_for_class_id",
				function(error, result){
			if(error){
				return callback(error);
			}
			logger.log(logger.logLevels["debug"], "db response: " + JSON.stringify(result));
			logger.log(logger.logLevels["debug"], "successful fetched user result");
			
			return callback(null, result);
		});
	});
}

/* fetches the results of all students of a class
 * required parameter: class_id, obligatory */
function getResultOfStudents(dbConnection, requestData, callback){
	if(!validator.validateID(requestData.class_id)){
		//malformed id
		return callback({"error" : 300});
	}
	logger.log(logger.logLevels["debug"], "fetching user results of class " + requestData.class_id);
	
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
			logger.log(logger.logLevels["debug"], "successful fetched user results");
			
			return callback(null, result);
		});
	});
}

module.exports = {
		getUser				: getUser,
		createClass			: createClass,
		deleteClass			: deleteClass,
		getUserClasses 		: getUserClasses,
		getTeacherClasses	: getTeacherClasses,
		acceptUserInClass	: acceptUserInClass,
		getResultOfStudent	: getResultOfStudent,
		getResultOfStudents	: getResultOfStudents,
		registerUserToClass	: registerUserToClass,
		removeStudentFromClass : removeStudentFromClass
};