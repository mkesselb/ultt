/// <summary>
/// Interface for validator classes. Specifies a single validation-method which should be implemented by validator classes.
/// 
/// The validator classes are intended to be used on user input, before it is sent to the server.
/// </summary>
public interface IValidator{

	/// <summary>
	/// Returns the validation message after validating the input string with rules specified in the validator classes.
	/// 
	/// Returns an empty string if validation is successful.
	/// </summary>
	/// 
	/// <returns>the validation message.</returns>
	/// 
	/// <param name="input">the input to be validated as string</param>
	string validateInput(string input);
}