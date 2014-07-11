var fs = require('fs');

var nl = 0;

if(process.argv.length > 2){
	var buff = fs.readFileSync(process.argv[2]);
	
	var str = buff.toString();
	
	var s = str.split("\n");
	
	if(s[s.length-1] === ""){
		nl = s.length-1;
	} else{
		nl = s.length-1;
	}
}

console.log(nl);