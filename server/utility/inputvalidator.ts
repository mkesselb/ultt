/* utility class which handles validating user input */

/* returns true if validation is successful, false otherwise */
function validateID(id){
	//id shall be a number, and not zero-lenght
	if(isNaN(id) || id.lenght===0){
		return false;
	}
	return true;
}

module.exports = {
		validateID				: validateID 
};