/* utility class which handles decoding input sent to the server */

/* takes the encoded, sent form string and returns a decoded string */
function decodeEscapedString(decodedString){
	if(decodedString == null){
		return "";
	}
	return decodeURIComponent(decodedString.split("+").join(" "))
}

module.exports = {
		decodeEscapedString : decodeEscapedString  
};