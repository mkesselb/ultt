/* module handling the login sequence, which consists of:
 * 	-	receiving the username and password from the user input
 * 	-	invoking the db module to find the appropriate user if exists
 * 	-	if found, the input password has to be checked against the saved password,
 * 			preferably with salt and hash to be secure and state-of-the-art
 * 
 * if either the user does not exist or the input password does not match the db data, no success is returned
 * else if the user can be found and the input password matches the db password, a login success is returned 
 */

//logging utility
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
	
	db(dbConnection, pwFetch, function(error, pw){
		if(error){
			logger.log(logger.logLevels["error"], "error on fetching pw for login" + error.toString());
			return callback(err);
		}
		logger.log(logger.logLevels["debug"], "password fetched: " + JSON.stringify(pw));
		
		if(pw.length === 0){
			logger.log(logger.logLevels["debug"], "empty fetch result, likewise user does not exist");
			return callback(null, "failure");
		}
		
		//TODO: pw shall be hashed and the salt shall be within it
		//	-> also need a hashing library
		var pass = pw[0].password;
		if(pass === "" || pass !== user.password){
			logger.log(logger.logLevels["debug"], "input password did not match db password, returning failure");
			return callback(null, "failure");
		}
		
		logger.log(logger.logLevels["debug"], "input password did match db password, returning success");
		callback(null, "success");
	});
};