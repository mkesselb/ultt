/* module which works the database queries that mostly deal with classes */
var logger = require('../logging/logging.ts');

function getClassUsers(dbConnection, requestData, callback){
	if(isNaN(requestData.class_id) || requestData.class_id.length === 0){
		//malformed, user id has to be number
		return callback("malformed class id: " + requestData.class_id);
	}
	logger.log(logger.logLevels["debug"], "get users of class with id: " + requestData.class_id);
	query = "select u.*, uc.accepted from user u, user_is_in_class uc where uc.class_id = " + requestData.class_id 
		+ " and u.user_id = uc.user_id";
	
	dbConnection.query(query, function(err, users){
		if(err){
			return callback(err);
		}
		logger.log(logger.logLevels["info"], "successful users fetching");
		logger.log(logger.logLevels["debug"], "fetch result: " + JSON.stringify(users));
		callback(null, users);
	});
}

module.exports = {
		getClassUsers	: getClassUsers 
}