var dir = process.argv[2];
var txt = process.argv[3];
txt = "." + txt;

var fs = require('fs');
var path = require('path');

fs.readdir(dir, function(err, list){
		for(i = 0; i < list.length-1; i++){
			if(txt === path.extname(list[i])){
				console.log(list[i]);
			}
		}
	});