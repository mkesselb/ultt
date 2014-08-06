/* module handling the database connections */
/* two possible modes for database connection establishment:	
 * 		get and post separated (most like not possible due to Unity with WWW)
 * 		get and post to db have to be parsed from the receiving data object - most likely the way to go! */

//logging utility
var logger = require('../logging/logging.ts');

module.exports = function(dbConnection, dbData, callback){
	//parsing the dbdata, and deciding which purpose is the sent
	var parsedDbData = parseDbData(dbData);
	logger.log(logger.logLevels["debug"], "full data received on db: " + JSON.stringify(parsedDbData));
	
	//deciding on route
	if(parsedDbData.purpose === "post"){
		post(dbConnection, parsedDbData, function(err, result){
			if(err){
				logger.log(logger.logLevels["error"], "error on db post");
				return callback(err);
			}
			callback(null, result);
		});
	}
	if(parsedDbData.purpose === "get"){
		get(dbConnection, parsedDbData, function(err, result){
			if(err){
				logger.log(logger.logLevels["error"], "error on db get");
				return callback(err);
			}
			callback(null, result);
		});
	}
	if(parsedDbData.purpose === "update"){
		update(dbConnection, parsedDbData, function(err, result){
			if(err){
				logger.log(logger.logLevels["error"], "error on db update");
				return callback(err);
			}
			callback(null, result);
		});
	}
};

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
		var response = {status : "success"};
		callback(null, response);
	});
}

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

function update(dbConnection, dbData, callback){
	//TODO: update db data
}

function parseDbData(dbData){
	var parsedDbData = {};
	dbData.forEach(function(data){
		var d = {
				key : data.split("=")[0],
				value : data.split("=")[1]
		};
		parsedDbData[d.key] = d.value;
	});
	return parsedDbData;
}