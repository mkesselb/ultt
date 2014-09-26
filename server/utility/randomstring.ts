/* utility module generating random strings of specified length */
var possibleChars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
module.exports = function(len){
	var result = "";
    for (var i = len; i > 0; --i) result += possibleChars[Math.round(Math.random() * (possibleChars.length - 1))];
    return result;
}