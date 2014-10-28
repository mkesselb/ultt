/* utility module which parses incoming www form data into a json object */

/* www form data is an array of key=value pairs, which are parsed into a JSON */
module.exports = function(data){
	var parsedData = {};
	data.forEach(function(dat){
		var split = dat.split("=");
		var k = split[0];
		var val = split[1];
		if(split.length > 2){
			for(i = 2; i < split.length; i++){
				val += "=" + split[i];
			}
		}
		var d = {
				key : k,
				value : val
		};
		parsedData[d.key] = d.value;
	});
	return parsedData;
};