/* module handling the login sequence, which consists of:
 * 	-	receiving the username and password from the user input
 * 	-	invoking the db module to find the appropriate user if exists
 * 	-	if found, the input password has to be checked against the saved password,
 * 			preferably with salt and hash to be secure and state-of-the-art
 * 
 * if either the user does not exist or the input password does not match the db data, an appropriate error is returned
 * else if the user can be found and the input password matches the db password, the user_id is returned as JSON 
 */

var logger = require('../logging/logging.ts');
var parser = require('../utility/jsonparser.ts');
var db = require('../db/db.ts');

module.exports = function(dbConnection, userData, callback){
	//var user shall contain user.username and user.password
	var user = parser(userData);
	logger.log(logger.logLevels["debug"], "user data received: " + JSON.stringify(user));
	
	var pwFetch = [];
	pwFetch.push("purpose=get");
	pwFetch.push("table=user");
	pwFetch.push("username=" + user.username);
	pwFetch.push("password=null");
	pwFetch.push("user_id=null");
	
	db(dbConnection, pwFetch, function(error, pw){
		if(error){
			logger.log(logger.logLevels["error"], "error on fetching pw for login" + error.toString());
			return callback(error);
		}
		logger.log(logger.logLevels["debug"], "password fetched: " + JSON.stringify(pw));
		
		if(pw.length === 0){
			logger.log(logger.logLevels["debug"], "empty fetch result, likewise user does not exist");
			return callback(null, {"error" : 100});
		}
		
		//TODO: pw shall be hashed and the salt shall be within it
		//	-> also need a hashing library
		var pass = pw[0].password;
		if(pass === "" || pass !== user.password){
			logger.log(logger.logLevels["debug"], "input password did not match db password, returning failure");
			return callback(null, {"error" : 101});
		}
		
		logger.log(logger.logLevels["debug"], "input password did match db password, returning success");
		
		//returns id on success
		callback(null, {"user_id" : pw[0].user_id});
	});
};