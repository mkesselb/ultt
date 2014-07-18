var log4js = require('log4js');
log4js.configure({
	appenders: [
		{ type: 'console' },
		{ type: 'file', filename: "ultt.log", category: 'ultt' }
	]
});
var logger  = log4js.getLogger('ultt');
	logger.setLevel('DEBUG');
	Object.defineProperty(exports, "LOG", {
	value:logger,
});