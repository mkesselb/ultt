var net = require('net');

var server = net.createServer(function (socket){
		//socket contains duplex stream (read / write)
		var date = new Date();
		var m = parseInt(date.getMonth())+1;
		var d = date.getDate();
		var h = date.getHours();
		var min = date.getMinutes();
		var data = date.getFullYear() + "-"
			+ (m < 10 ? "0" + m : m)
			+ "-" + (d < 10 ? "0" + d : d) + " "
			+ (h < 10 ? "0" + h : h)
			+ ":" + (min < 10 ? "0" + min : min) + "\n";
		socket.write(data);
		socket.end();
	});
	
server.listen(process.argv[2]);