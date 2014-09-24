/* basic server application file, handling all requests and responses by giving them to the appropriate module */
var express = require('express');
var mysql = require('mysql');
var fs = require('fs');
var db = require('./db/db.ts');
var login = require('./login/login.ts');
var register = require('./login/register.ts');
var logger = require('./logging/logging.ts');

/* building db connection */
//pooling connections also possible
var connection = mysql.createConnection({
	host     : 'localhost',
	user     : 'root',
	password : '',
	database : 'ultt'
});
connection.connect(function(err){
	if (err){
		logger.log(logger.logLevels["error"], "error connecting to db: " + err.toString());
		throw err;
	}

	logger.log(logger.logLevels["info"], "connected to db");
});


/* application server starting */
var ultt = express();
var server = ultt.listen(80, function(err){
	if(err){
		logger.log(logger.logLevels["error"], "error listening on port "
				+ server.address().port + ": " + err.toString());
		throw err;
	}
	
	logger.log(logger.logLevels["info"], "listening on port " + server.address().port);
});


/* defining routes */


/* get routes */
ultt.get('/', function(req, res){
	logger.log(logger.logLevels["info"], "serving request to /");
	fs.createReadStream('ultt.html').pipe(res);
});

ultt.get('/ultt.unity3d', function(req, res){
	logger.log(logger.logLevels["info"], "serving request to /ultt.unity3d");
	fs.createReadStream('ultt.unity3d').pipe(res);
});

ultt.get('/info', function(req, res){
	logger.log(logger.logLevels["info"], "serving request to /info");
	res.send('sp mkesselb, comoessl, stoffl1024');
});


/* post routes */
//full db handling is done in this post route
ultt.post('/unity/db', function(req, res){
	logger.log(logger.logLevels["info"], "serving post to /unity/db");
	var body = [];
	req.on('data', function(data){
		logger.log(logger.logLevels["debug"], "received data: " + data.toString());
		var s = data.toString().split('&');
		for(var i = 0; i < s.length; i++){
			body.push(s[i]);
		}
	});
	req.on('end', function(){
		db(connection, body, function(err, result){
			if(err){
				//check if err contains known db error code
				if(err.error){
					logger.log(logger.logLevels["error"], "error posting to db: " + JSON.stringify(err));
					res.send(err);
				} else{
					//else send unspecified db error
					logger.log(logger.logLevels["error"], "error posting to db: " + err.toString());
					res.send({"error" : 400});
				}
			} else{
				res.send(result);
			}
		});
	});
});

//login handling -> checking whether the entered user data can be found in db and returning result
ultt.post('/login', function(req, res){
	logger.log(logger.logLevels["info"], "serving post to /login");
	
	var body = [];
	req.on('data', function(data){
		logger.log(logger.logLevels["debug"], "received login data: " + data.toString());
		var s = data.toString().split('&');
		for(var i = 0; i < s.length; i++){
			body.push(s[i]);
		}
	});
	req.on('end', function(){
		//login module is invoked, and response is sent
		login(connection, body, function(err, result){
			if(err){
				//check if it contains known error
				if(err.error){
					logger.log(logger.logLevels["error"], "error checking login data: " + JSON.stringify(err));
					res.send(err);
				} else{
					//else send unspecified login error
					logger.log(logger.logLevels["error"], "error checking login data: " + err.toString());
					res.send({"error" : 401});
				}
			} else{
				res.send(result);
			}
		});
	});
});

ultt.post('/register', function(req, res){
	logger.log(logger.logLevels["info"], "serving post to /register");
	
	var body = [];
	req.on('data', function(data){
		logger.log(logger.logLevels["debug"], "received register data: " + data.toString());
		var s = data.toString().split('&');
		for(var i = 0; i < s.length; i++){
			body.push(s[i]);
		}
	});
	req.on('end', function(){
		//register module is invoked, and response is sent
		register(connection, body, function(err, result){
			if(err){
				//check if it contains known error
				if(err.error){
					logger.log(logger.logLevels["error"], "error checking register data: " + JSON.stringify(err));
					res.send(err);
				} else{
					logger.log(logger.logLevels["error"], "error checking register data: " + err.toString());
					res.send({"error" : 402});
				}
			} else{
				res.send(result);
			}
		});
	});
});