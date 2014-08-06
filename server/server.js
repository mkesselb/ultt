var express = require('express');
var mysql = require('mysql');
var fs = require('fs');
var db = require('./db/db.ts');
var logger = require('./logging/logging.ts');

/* building db connection */
//pooling connections also possible
var connection = mysql.createConnection({
    host: 'localhost',
    user: 'root',
    password: '',
    database: 'ultt'
});
connection.connect(function (err) {
    if (err) {
        logger.log(logger.logLevels["error"], "error connecting to db: " + err.toString());
        throw err;
    }

    logger.log(logger.logLevels["info"], "connected to db");
});

/* application server starting */
var ultt = express();
var server = ultt.listen(80, function (err) {
    if (err) {
        logger.log(logger.logLevels["error"], "error listening on port " + server.address().port + ": " + err.toString());
        throw err;
    }

    logger.log(logger.logLevels["info"], "Listening on port " + server.address().port);
});

/* defining routes */
/* get routes */
ultt.get('/', function (req, res) {
    logger.log(logger.logLevels["info"], "serving request to /");
    fs.createReadStream('ultt.html').pipe(res);
});

ultt.get('/ultt.unity3d', function (req, res) {
    logger.log(logger.logLevels["info"], "serving request to /ultt.unity3d");
    fs.createReadStream('ultt.unity3d').pipe(res);
});

ultt.get('/info', function (req, res) {
    logger.log(logger.logLevels["info"], "serving request to /info");
    res.send('sp mkesselb, comoessl, stoffl1024');
});

/* post routes */
ultt.post('/unity/db', function (req, res) {
    //todo: fill in db handling module
    logger.log(logger.logLevels["info"], "serving post to /unity/db");
    var body = [];
    req.on('data', function (data) {
        logger.log(logger.logLevels["debug"], "received data: " + data.toString());
        var s = data.toString().split('&');
        for (var i = 0; i < s.length; i++) {
            body.push(s[i]);
        }
    });
    req.on('end', function () {
        db(connection, body, function (err, result) {
            if (err) {
                logger.log(logger.logLevels["error"], "error posting to db: " + err.toString());
                throw err;
            }
            res.send(result);
        });
    });
});
