/* module which works the database queries that mostly deal with users */
var logger = require('../logging/logging.ts');

/* fetches all user data. request parameter is user_id. */
function getUser(dbConnection, requestData, callback){
	if(isNaN(requestData.user_id) || requestData.user_id.length === 0){
		//malformed, user id has to be number
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
	if(isNaN(requestData.user_id) || requestData.user_id.length === 0){
		//malformed, user id has to be number
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
	if(isNaN(requestData.user_id) || requestData.user_id.length === 0){
		//malformed, user id has to be number
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

module.exports = {
		getUser				: getUser,
		getUserClasses 		: getUserClasses,
		getTeacherClasses	: getTeacherClasses 
};