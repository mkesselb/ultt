/* utility module which parses incoming www form data into a json object */
module.exports = function(data){
	var parsedData = {};
	data.forEach(function(dat){
		var d = {
				key : dat.split("=")[0],
				value : dat.split("=")[1]
		};
		parsedData[d.key] = d.value;
	});
	return parsedData;
}