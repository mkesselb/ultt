/* module handling the database connections */
/* two possible modes for database connection establishment:
 * 		get and post separated (most like not possible due to Unity with WWW)
 * 		get and post to db have to be parsed from the receiving data object - most likely the way to go!
 *
 * deprecated way: parsing data from www from and creating query on the fly
 * now also offering dedicated methods to often needed, possibly complex queries
 */

//logging utility
var logger = require('../logging/logging.ts');
var parser = require('../utility/jsonparser.ts');
var userlink = require('./userlink.ts');
var classlink = require('./classlink.ts');
var tasklink = require('./tasklink.ts');

var userMethods = [];
userMethods.push("getUser", "getTeacherClasses", "getUserClasses", 
		"registerUserToClass", "acceptUserInClass", "createClass", 
		"deleteClass", "removeStudentFromClass", "getResultOfStudent",
		"getResultOfStudents");
var classMethods = [];
classMethods.push("getClass", "getClassUsers", "getClassTasks", 
		"getClassTopics", "createClassTopic", "deleteClassTopic")
var taskMethods = [];
taskMethods.push("getTask", "editTask", "saveTask", 
		"createTask", "deleteTask", "getSubjects", 
		"getTaskTypes", "getUserTasks", "getTaskForClass", 
		"getResultOfTask", "getResultOfTasks", "assignTaskToTopic", 
		"deleteTaskFromTopic");

module.exports = function(dbConnection, dbData, callback){
	//parsing the dbdata, and deciding which purpose is the sent
	var parsedDbData = parser(dbData);
	logger.log(logger.logLevels["debug"], "full data received on db: " + JSON.stringify(parsedDbData));

	//deciding on route
	/* userlink */
	if(userMethods.indexOf(parsedDbData.method) > -1){
		userlink[parsedDbData.method](dbConnection, parsedDbData, function(err, result){
			if(err){
				logger.log(logger.logLevels["error"], "error on method " + parsedDbData.method);
				return callback(err);
			}
			return callback(null, result);
		});
	}

	/* tasklink */
	if(taskMethods.indexOf(parsedDbData.method) > -1){
		tasklink[parsedDbData.method](dbConnection, parsedDbData, function(err, result){
			if(err){
				logger.log(logger.logLevels["error"], "error on method " + parsedDbData.method);
				return callback(err);
			}
			return callback(null, result);
		});
	}
	
	/* classlink */
	if(classMethods.indexOf(parsedDbData.method) > -1){
		classlink[parsedDbData.method](dbConnection, parsedDbData, function(err, result){
			if(err){
				logger.log(logger.logLevels["error"], "error on method " + parsedDbData.method);
				return callback(err);
			}
			return callback(null, result);
		});
	}

	//---deprecated, but maybe preferable for getting / posting single values---
	//deciding on route
	if(parsedDbData.purpose === "post"){
		post(dbConnection, parsedDbData, function(err, result){
			if(err){
				logger.log(logger.logLevels["error"], "error on db post");
				return callback(err);
			}
			return callback(null, result);
		});
	}
	if(parsedDbData.purpose === "get"){
		get(dbConnection, parsedDbData, function(err, result){
			if(err){
				logger.log(logger.logLevels["error"], "error on db get");
				return callback(err);
			}
			return callback(null, result);
		});
	}
	if(parsedDbData.purpose === "update"){
		update(dbConnection, parsedDbData, function(err, result){
			if(err){
				logger.log(logger.logLevels["error"], "error on db update");
				return callback(err);
			}
			return callback(null, result);
		});
	}
};

/* ---deprecated--- */
function post(dbConnection, dbData, callback){
	/*
	 * 	in the dbData json, the following data is important:
	 *	table: 			indicates the table to write to
	 *	column name: 	gives the value of the column that shall be written
	 *
	 *	other data should not be present, the "purpose" entry is skipped
	 * */
	var insertData = {};
	var table = "";
	for(var d in dbData){
		if(dbData.hasOwnProperty(d)){
			if(d === "table"){
				table = dbData[d];
			} else{
				if(d !== "purpose"){
					insertData[d] = dbData[d];
				}
			}
		}
	}
	
	logger.log(logger.logLevels["debug"], "db insert data: " 
			+ JSON.stringify(insertData) + ", in table: " + table);
	dbConnection.query('insert into ' + table + ' set ?', insertData, function(err, result){
		if(err){
			return callback(err);
		}
		logger.log(logger.logLevels["info"], "successful db insert");
		callback(null, {"status" : 1});
	});
}

/* ---deprecated--- */
function get(dbConnection, dbData, callback){
	/*
	 * 	in the dbData json, the following data is important:
	 * 	table:		indicates the table to fetch data from
	 * 	column name with non-null value:	indicates data to be matched
	 * 	column name with null value:		indicates data to be fetched
	 * 
	 * 	the "purpose" entry is skipped
	 */
	var table = "";
	var fetchData = {};
	var matchData = {};
	for(var d in dbData){
		if(dbData.hasOwnProperty(d)){
			if(d === "table"){
				table = dbData[d];
			} else{
				if(d !== "purpose"){
					if(dbData[d] === "null"){
						fetchData[d] = dbData[d];
					} else{
						matchData[d] = dbData[d];
					}
				}
			}
		}
	}
	
	var query = "select ";
	for(d in fetchData){
		if(fetchData.hasOwnProperty(d)){
			query += d + ", ";
		}
	}
	query = query.substr(0, query.length-2) + " from " + table + " where ";
	for(d in matchData){
		if(matchData.hasOwnProperty(d)){
			var x = matchData[d];
			if(isNaN(x)){
				//escape double quotes in x
				query += d + "=\"" + x.split("\"").join('\\\"') + "\" and ";
			} else{
				query += d + "=" + x + " and ";
			}
		}
	}
	query = query.substr(0, query.length-5) + ";";
	logger.log(logger.logLevels["debug"], "db fetch query: " + query);
	
	dbConnection.query(query, function(err, dbResult){
		if(err){
			return callback(err);
		}
		logger.log(logger.logLevels["info"], "successful db fetching");
		logger.log(logger.logLevels["debug"], "fetch result: " + JSON.stringify(dbResult));
		callback(null, dbResult);
	});
}

/* ---deprecated--- */
function update(dbConnection, dbData, callback){
	//TODO: update db data
}