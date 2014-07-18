var http = require('http');

http.get(process.argv[2], function(response){
	var datas = [];
	response.on("error", function(err){
		console.log("Error: " + err.toString());});
	response.on("data", function(data){
		datas.push(data);});
	response.on("end", function(end){
		var s = "";
		for(i = 0; i < datas.length; i++){
			s += datas[i].toString();
		}
		console.log(s.length);
		console.log(s);});
	});