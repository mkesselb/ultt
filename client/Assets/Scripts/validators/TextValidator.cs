using System;

/// <summary>
/// Validator class for text input fields. Text length can be specified.
/// Implements <see cref="IValidator"/>.
/// </summary>
public class TextValidator : IValidator
{
	/// <summary>
	/// The minimum number of characters allowed for text length.
	/// </summary>
	private int min = 1;

	/// <summary>
	/// The maximum number of characters allowed for text length.
	/// </summary>
	private int max = int.MaxValue;

	/// <summary>
	/// Initializes a new instance of the <see cref="TextValidator"/> class.
	/// The parameters make it possible to specify the allowed range of text inputs.
	/// </summary>
	/// 
	/// <param name="min">minimum of characters.</param>
	/// <param name="max">maximum of characters.</param>
	public TextValidator(int min = 1, int max = int.MaxValue){
		if (min > 0 && max > min) {
			this.setValidationLength(min, max);
		}
	}

	/// <summary>
	/// Returns the validation message after validating the input string against specified text rules.
	/// 
	/// Returns an empty string if validation is successful.
	/// </summary>
	/// 
	/// <returns>the validation message.</returns>
	/// 
	/// <param name="input">the input text to be validated as string</param>
	public string validateInput(string input){
		//text input shall be non-empty and shall not contain a \" character
		if (input.Contains ("\"")) {
			return "No \" input character allowed!";
		}

		int n = input.Length;
		if (n <= 0) {
			return "Empty input is not allowed!";
		}
		if (n < this.min || n > this.max) {
			return "Input not between length bounds of " + this.min + " - " + this.max + "!";
		}
		return "";
	}

	/// <summary>
	/// Sets the range of the allowed input text.
	/// </summary>
	/// 
	/// <param name="min">minimum of characters.</param>
	/// <param name="max">maximum of characters.</param>
	public void setValidationLength(int min, int max){
		this.min = min;
		this.max = max;
	}
}