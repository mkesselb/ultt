/*module.exports = function(dir, txt, callback){
	fs.readdir(dir, function(err, data){
		if(err)
			return callback(err)
		var list = [];
		for(i = 0; i < data.length-1; i++){
			if("." + txt === path.extname(data[i])){
				list.push(data[i]);
			}
		}
		callback(null, list);
	});
};*/

var fs = require('fs');
var path = require('path');

module.exports = function (dir, txt, callback){
	fs.readdir(dir, function(err, data){
	if(err){
		return callback(err)
	}
	var list = [];
	for(i = 0; i <= data.length-1; i++){
		if("." + txt === path.extname(data[i])){
			list.push(data[i]);
		}
	}
	callback(null, list);
	});
}