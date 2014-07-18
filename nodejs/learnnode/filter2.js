var filterModule = require('./filterModule.js');

filterModule(process.argv[2], process.argv[3], function(err, list){
	if(err) 
		console.log("Error: " + err.toString())
	for(i = 0; i <= list.length-1; i++){
		console.log(list[i]);
	}
});