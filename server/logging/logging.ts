//require logging lib
var log4js = require('log4js');
log4js.configure({
	appenders: [
		{type: 'console', category: 'console'},
		{type: 'file', filename: "ultt.log", category: 'ultt'}
	]
});

//set default logging
var logFile = log4js.getLogger('ultt');
var logCons = log4js.getLogger('console');
var logLevel = 2;
var file = true;
var cons = true;

logLevels = {
	"all" : 0,
	"log" : 1,
	"debug" : 2,
	"info" : 3,
	"warn" : 4,
	"error" : 5
};
logLevelLookup = {
		0 : "all",
		1 : "log",
		2 : "debug",
		3 : "info",
		4 : "warn",
		5 : "error"
} 

logFile.setLevel('DEBUG');
logCons.setLevel('DEBUG');

function setLogLevel(logL){
	if(logL < 0 || logL > 5){
		logLevel = 2;
	} else{
		logLevel = logL
	}
}

function setLogging(logF, logC){
	file = logF;
	cons = logC;
}

//decides logging path
function log(logL, logData){
	if(logL >= logLevel){
		var L = logLevelLookup[logL];
		if(file){
			logToFile(L, logData);
		}
		if(cons){
			logToConsole(L, logData);
		}
	}
}

//write to file
function logToFile(logL, logData){
	logFile[logL](logData);
}

//write to console
function logToConsole(logL, logData){
	logCons[logL](logData);
}

module.exports = {
	log 		: log,
	setLogLevel : setLogLevel,
	logLevels 	: logLevels,
	setLogging 	: setLogging
};