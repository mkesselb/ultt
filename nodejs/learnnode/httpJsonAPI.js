var http = require('http');
var url = require('url');

var routes = {
	"/api/parsetime": function(parsedUrl){
		d = new Date(parsedUrl.query.iso);
		return {
			hour: d.getHours(),
			minute: d.getMinutes(),
			second: d.getSeconds(),
		};
	},
	"/api/unixtime": function(parsedUrl){
		return {
			unixtime: (new Date(parsedUrl.query.iso)).getTime(),
		};
	}
}

var server = http.createServer(function (request, response){
		var r = url.parse(request.url, true);
		resource = routes[r.pathname];
		if(resource){
			response.writeHead(200, {'Content-Type' : 'application/json'});
			response.end(JSON.stringify(resource(r)));
		} else{
			response.writeHead(404);
			response.end();
		}
		
	});

server.listen(process.argv[2]);