/* module handling the register process, which consists of:
 * 	-	receiving desired username and password from user input
 * 	-	checking if username and password can be taken (username free + password min length) 
 * 	-	giving appropriate response
 * 
 * if username does already exists, "user exists" is returned
 * if password is smaller minimum length or greater than max length, "bad password" is returned" 
 * else the user is written to db and "success" is returned 
 */

//logging utility
var logger = require('../logging/logging.ts');
var parser = require('../utility/jsonparser.ts');
var db = require('../db/db.ts');

module.exports = function(dbConnection, userData, callback){
	var user = parser(userData);
	//TODO: decide on password requirements
	//check password requirements
	if(user.password.length < 6){
		return callback(null, {"error" : 200});
	}
	if(user.password.length > 12){
		return callback(null, {"error" : 201});
	}
	if(user.username.length === 0){
		return callback(null, {"error" : 202});
	}
	
	//prepare fetch
	var pwFetch = [];
	pwFetch.push("purpose=get");
	pwFetch.push("table=user");
	pwFetch.push("username=" + user.username);
	pwFetch.push("password=null");
	
	db(dbConnection, pwFetch, function(error, pw){
		if(error){
			logger.log(logger.logLevels["error"], "error on fetching pw for register: " + error.toString());
			return callback(err);
		}
		logger.log(logger.logLevels["debug"], "password fetched: " + JSON.stringify(pw));
		
		if(pw.length === 0){
			logger.log(logger.logLevels["debug"], "empty fetch result, username not taken");
		} else{
			logger.log(logger.logLevels["debug"], "fetch result, user does exist");
			return callback(null, {"error" : 203});
		}
	});
	
	//prepare post
	var insertUser = [];
	insertUser.push("purpose=post");
	insertUser.push("table=user");
	pwFetch.push("username=" + user.username);
	pwFetch.push("password=" + user.password);
	
	db(dbConnection, insertUser, function(error, result){
		if(error){
			logger.log(logger.logLevels["error"], "error on inserting new user: " + error.toString());
			return callback(error);
		}
		
		logger.log(logger.logLevels["debug"], "user successful inserted");
		
		//fetch and return id
		var idFetch = [];
		pwFetch.push("purpose=get");
		pwFetch.push("table=user");
		pwFetch.push("username=" + user.username);
		pwFetch.push("user_id=null");
		
		db(dbConnection, idFetch, function(err, id){
			if(err){
				logger.log(logger.logLevels["error"], "error fetching id of new user: " + err.toString());
				return callback(err);
			}
			
			logger.log(logger.logLevels["debug"], "user_id of new user fetched");
			return callback(null, id[0]);
		});
	});
}