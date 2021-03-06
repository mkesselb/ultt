/* basic server application file, handling all requests and responses by giving them to the appropriate module */
var express = require('express');
var mysql = require('mysql');
var fs = require('fs');
var db = require('./db/db.ts');
var login = require('./login/login.ts');
var register = require('./login/register.ts');
var logger = require('./logging/logging.ts');
var decoder = require('./utility/decoder.ts');

/* building db connection */
//creating connection pool
var pool = mysql.createPool({
	host     : 'localhost',
	user     : 'root',
	password : '',
	database : 'ultt'
});
pool.getConnection(function(err, connection){
	//try to establish one connection to see whether db can be reached
	if (err){
		logger.log(logger.logLevels["error"], "error connecting to db: " + err.toString());
		throw err;
	}

	logger.log(logger.logLevels["info"], "connected to db");
	connection.release();
});

/* application server starting */
var port = 80;
if(process.argv.length > 2){
	//assuming that argument on index 2 is a specified port
	port = process.argv[2];
}
var ultt = express();

if(port == 443){
	//set https server
	//arguments on index 3-path to private key and 4-path to server certificate
	var https = require('https');
	var privateKey  = fs.readFileSync(process.argv[3]/*'sslcert/server.key'*/, 'utf8');
	var certificate = fs.readFileSync(process.argv[4]/*'sslcert/server.crt'*/, 'utf8');
	
	var credentials = {key: privateKey, cert: certificate};
	var httpsServer = https.createServer(credentials, ultt);
	
	httpsServer.listen(port, function(err){
	if(err){
		logger.log(logger.logLevels["error"], "error listening on port "
				+ port + ": " + err.toString());
		return;
	}
	
	logger.log(logger.logLevels["info"], "https: listening on port " + port)});
	
} else{
	//set http server
	var server = ultt.listen(port, function(err){
	if(err){
		logger.log(logger.logLevels["error"], "error listening on port "
				+ server.address().port + ": " + err.toString());
		return;
	}
	
	logger.log(logger.logLevels["info"], "http: listening on port " + server.address().port);
});
}

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

ultt.get('/favicon.ico', function(req, res){
	logger.log(logger.logLevels["info"], "serving request to /favicon.ico");
	fs.createReadStream('favicon.ico').pipe(res);
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
		logger.log(logger.logLevels["debug"], "received data: " + data);
		var s = data.toString().split('&');
		for(var i = 0; i < s.length; i++){;
			var ss = decoder.decodeEscapedString(s[i]);
			body.push(ss);
			logger.log(logger.logLevels["debug"], "--- decoded " + ss);
		}
	});
	req.on('end', function(){
		pool.getConnection(function(err, connection){
			if(err){
				logger.log(logger.logLevels["error"], "error getting connection from pool: " + err.toString()); 
				res.send({"error" : 403});
				return;
			}		
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
			connection.release();
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
			var ss = decoder.decodeEscapedString(s[i]);
			body.push(ss);
			logger.log(logger.logLevels["debug"], "--- decoded " + ss);
		}
	});
	req.on('end', function(){
		//login module is invoked, and response is sent
		pool.getConnection(function(err, connection){
			if(err){
				logger.log(logger.logLevels["error"], "error getting connection from pool: " + err.toString()); 
				res.send({"error" : 403});
				return;
			}			
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
			connection.release();
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
			var ss = decoder.decodeEscapedString(s[i]);
			body.push(ss);
			logger.log(logger.logLevels["debug"], "--- decoded " + ss);
		}
	});
	req.on('end', function(){
		//register module is invoked, and response is sent
		pool.getConnection(function(err, connection){
			if(err){
				logger.log(logger.logLevels["error"], "error getting connection from pool: " + err.toString()); 
				res.send({"error" : 403});
				return;
			}
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
			connection.release();
		});
	});
});