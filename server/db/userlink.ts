/* module which works the database queries that mostly deal with users */
var logger = require('../logging/logging.ts');
var validator = require('../utility/inputvalidator.ts');

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
			+ " and u.class_id = c.class_id and c.subject_id = s.subject_id and c.user_id = us.user_id";
	
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
			"where c.user_id = " + requestData.user_id + " and c.subject_id = s.subject_id";
	
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
			logger.log(logger.logLevels["info"], "successful registered user to class");
			callback(null, {"class_id" : class_id});
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
	logger.log(logger.logLevels["debug"], "accepting user_id " + requestData.user_id 
			+ " into class_id " + requestData.class_id);
	dbConnection.query("update user_is_in_class set ? where ?", 
			[{"accepted" : 1}, {"user_id" : requestData.user_id, "class_id" : requestData.class_id}], 
			function(err, result){
		if(err){
			return callback(err);
		}
		logger.log(logger.logLevels["info"], "successful accepted user to class");
		callback(null, {"status" : 1});
	});
};

function createClass(dbConnection, requestData, callback){
	//TODO: implement
};

function deleteClass(dbConnection, requestData, callback){
	//TODO: implement
};

function assignTaskToTopic(dbConnection, requestData, callback){
	//TODO: implement
};

function createTask(dbConnection, requestData, callback){
	//TODO: implement
};

module.exports = {
		getUser				: getUser,
		getUserClasses 		: getUserClasses,
		getTeacherClasses	: getTeacherClasses,
		acceptUserInClass	: acceptUserInClass,
		registerUserToClass	: registerUserToClass
};