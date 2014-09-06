/* module handling the database connections */
/* two possible modes for database connection establishment:	
 * 		get and post separated (most like not possible due to Unity with WWW)
 * 		get and post to db have to be parsed from the receiving data object - most likely the way to go! */

//logging utility
var logger = require('../logging/logging.ts');
var parser = require('../utility/jsonparser.ts');
var userlink = require('./userlink.ts');
var classlink = require('./classlink.ts');


module.exports = function(dbConnection, dbData, callback){
	//parsing the dbdata, and deciding which purpose is the sent
	var parsedDbData = parser(dbData);
	logger.log(logger.logLevels["debug"], "full data received on db: " + JSON.stringify(parsedDbData));
	
	//deciding on route
	/* userlink */
	if(parsedDbData.method === "getUser"){
		userlink.getUser(dbConnection, parsedDbData, function(err, result){
			if(err){
				logger.log(logger.logLevels["error"], "error on method getUser");
				return callback(err);
			}
			return callback(null, result);
		});
	}
	
	if(parsedDbData.method === "getTeacherClasses"){
		userlink.getTeacherClasses(dbConnection, parsedDbData, function(err, result){
			if(err){
				logger.log(logger.logLevels["error"], "error on method getTeacherClasses");
				return callback(err);
			}
			return callback(null, result);
		});
	}
	
	if(parsedDbData.method === "getUserClasses"){
		userlink.getUserClasses(dbConnection, parsedDbData, function(err, result){
			if(err){
				logger.log(logger.logLevels["error"], "error on method getUserClasses");
				return callback(err);
			}
			return callback(null, result);
		});
	}
	
	/* classlink */
	if(parsedDbData.method === "getClassUsers"){
		classlink.getClassUsers(dbConnection, parsedDbData, function(err, result){
			if(err){
				logger.log(logger.logLevels["error"], "error on method getClassUsers");
				return callback(err);
			}
			return callback(null, result);
		});
	}
	
	if(parsedDbData.method === "getClassTasks"){
		classlink.getClassTasks(dbConnection, parsedDbData, function(err, result){
			if(err){
				logger.log(logger.logLevels["error"], "error on method getClassTasks");
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

//---deprecated---
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

//---deprecated---
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
	for(var d in fetchData){
		if(fetchData.hasOwnProperty(d)){
			query += d + ",";
		}
	}
	query = query.substr(0, query.length-1) + " from " + table + " where ";
	for(var d in matchData){
		if(matchData.hasOwnProperty(d)){
			var x = matchData[d];
			if(isNaN(x)){
				query += d + "='" + x + "' and ";
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

//---deprecated---
function update(dbConnection, dbData, callback){
	//TODO: update db data
}