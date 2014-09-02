/* module which works the database queries that mostly deal with users */
var logger = require('../logging/logging.ts');

function getUser(dbConnection, requestData, callback){
	if(isNaN(requestData.user_id) || requestData.user_id.length === 0){
		//malformed, user id has to be number
		return callback("malformed user id: " + requestData.user_id);
	}
	logger.log(logger.logLevels["debug"], "get user with id: " + requestData.user_id);
	query = "select * from user where user_id = " + requestData.user_id;
	
	dbConnection.query(query, function(err, user){
		if(err){
			return callback(err);
		}
		logger.log(logger.logLevels["info"], "successful user fetching");
		logger.log(logger.logLevels["debug"], "fetch result: " + JSON.stringify(user));
		callback(null, user);
	});
}

function getUserClasses(dbConnection, requestData, callback){
	if(isNaN(requestData.user_id)){
		//malformed, user id has to be number
		return callback("malformed user id: " + requestid.user_id);
	}
	logger.log(logger.logLevels["debug"], "get classes of user with id: " + requestData.user_id);
	
	var fetchClasses = "select c.*, u.accepted "
			+ "from class c, user_is_in_class u where u.user_id = " + requestData.user_id
			+ " and u.class_id = c.class_id";
	
	dbConnection.query(fetchClasses, function(err, classes){
		if(err){
			return callback(err);
		}
		logger.log(logger.logLevels["info"], "successful classes fetching");
		logger.log(logger.logLevels["debug"], "fetch result: " + JSON.stringify(classes));
		callback(null, classes);
	});
}

module.exports = {
		getUser			: getUser,
		getUserClasses 	: getUserClasses 
}