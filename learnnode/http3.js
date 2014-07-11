var http = require('http');

var cb = 0;
var dd = [];

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
		dd[0] = s;
		cb++;
		if(cb === 3){
			for(i = 0; i < dd.length; i++){
				console.log(dd[i]);
			}
		}
	});
});
	
http.get(process.argv[3], function(response){
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
		dd[1] = s;
		cb++;
		if(cb === 3){
			for(i = 0; i < dd.length; i++){
				console.log(dd[i]);
			}
		}
	});
});
	
http.get(process.argv[4], function(response){
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
		dd[2] = s;
		cb++;
		if(cb === 3){
			for(i = 0; i < dd.length; i++){
				console.log(dd[i]);
			}
		}		
	});
});
