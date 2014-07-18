var fs = require('fs');

var nl = 0;

if(process.argv.length > 2){
	fs.readFile(process.argv[2], 
		function (err, data){
			if (err) throw err;
			console.log(data.toString().split("\n").length-1);
		});
}

//console.log(nl);