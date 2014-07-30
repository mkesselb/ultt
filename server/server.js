var express = require('express');
var mysql = require('mysql');
var fs = require('fs');

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
        console.error('error connecting: ' + err.toString());
        throw err;
    }

    console.log('connected to db');
});

/* application server starting */
var ultt = express();
var server = ultt.listen(80, function (err) {
    if (err) {
        console.log("error listening: " + err.toString());
        throw err;
    }

    console.log('Listening on port ' + server.address().port);
});

/* defining routes */
/* get routes */
ultt.get('/', function (req, res) {
    console.log('serving request to /');
    fs.createReadStream('index.html').pipe(res);
});

ultt.get('/unity', function (req, res) {
    console.log('serving request to /unity');
    fs.createReadStream('unity.html').pipe(res);
});

ultt.get('/info', function (req, res) {
    console.log('serving request to /info');
    res.send('sp mkesselb, comoessl, stoffl1024');
});

ultt.get('/db', function (req, res) {
    //todo: fill in db handling module
    console.log('serving request to /db');

    connection.query('select id, name, age from persons;', function (err, rows) {
        if (err) {
            res.send(["error connecting", err.toString()]);
            throw err;
        }

        for (var i = 0; i < rows.length; i++) {
            console.log(rows[i].id + ";" + rows[i].name + ";" + rows[i].age);
        }

        res.send(rows);
    });
});

ultt.get('/unity/db', function (req, res) {
    //todo: fill in db handling module
    console.log('serving request to /unity/db');

    connection.query('select id, name, age from persons;', function (err, rows) {
        if (err) {
            res.send(["error connecting", err.toString()]);
            throw err;
        }

        for (var i = 0; i < rows.length; i++) {
            console.log(rows[i].id + ";" + rows[i].name + ";" + rows[i].age);
        }

        res.send(rows);
    });
});

/* post routes */
ultt.post('/db', function (req, res) {
    //todo: fill in db handling module
    console.log('serving post to /db');
    var body = [];
    req.on('data', function (data) {
        var s = data.toString().split('&');
        for (var i = 0; i < s.length; i++) {
            body.push(s[i].split('=')[1]);
        }
    });
    req.on('end', function () {
        //todo: fill in db handling module
        var person = { name: body[0], age: body[1] };
        connection.query('insert into persons set ?', person, function (err, result) {
            if (err) {
                console.log('error inserting rows');
                throw err;
            }
            console.log('inserted: ' + person.name + ";" + person.age);
            fs.createReadStream('index.html').pipe(res);
        });
    });
});

ultt.post('/unity/db', function (req, res) {
    //todo: fill in db handling module
    console.log('serving post to /unity/db');
    var body = [];
    req.on('data', function (data) {
        var s = data.toString().split('&');
        for (var i = 0; i < s.length; i++) {
            body.push(s[i].split('=')[1]);
        }
    });
    req.on('end', function () {
        //todo: fill in db handling module
        var person = { name: body[0], age: body[1] };
        connection.query('insert into persons set ?', person, function (err, result) {
            if (err) {
                console.log('error inserting rows');
                throw err;
            }
            console.log('inserted: ' + person.name + ";" + person.age);
            fs.createReadStream('unity.html').pipe(res);
        });
    });
});
