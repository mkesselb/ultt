using System;

/// <summary>
/// Validator class for number input fields. Is configurable for both float and integer inputs.
/// Max and min values for the allowed range can also be specified.
/// Implements <see cref="IValidator"/>.
/// </summary>
public class NumberValidator : IValidator
{
	/// <summary>
	/// Flag whether float values shall also be allowed (true), or only integer.
	/// </summary>
	private bool allowFloat;

	/// <summary>
	/// The minimum allowed value.
	/// </summary>
	private int min;

	/// <summary>
	/// The maximum allowed value.
	/// </summary>
	private int max;

	/// <summary>
	/// Initializes a new instance of the <see cref="NumberValidator"/> class.
	/// The parameters make it possible to tailor the validation for different number input fields.
	/// </summary>
	/// 
	/// <param name="allowFloat">if set to <c>true</c>, allow float.</param>
	/// <param name="min">min value to be allowed.</param>
	/// <param name="max">max value to be allowed</param>
	public NumberValidator(bool allowFloat = false, int min = int.MinValue, int max = int.MaxValue){
		this.allowFloat = allowFloat;
		this.min = min;
		this.max = max;
	}

	/// <summary>
	/// Returns the validation message after validating the input string against the number validation parameters of the validator.
	/// 
	/// Returns an empty string if validation is successful.
	/// </summary>
	/// 
	/// <returns>the validation message.</returns>
	/// 
	/// <param name="input">the input number to be validated as string</param>
	public string validateInput(string input){
		float num;
		bool result;
		if (allowFloat) {
			result = float.TryParse (input, out num);
		} else {
			int n2;
			result = int.TryParse(input, out n2);
			num = (float)n2;
		}
		if (result) {
			//success
			if(this.min < num && num < this.max){
				return "";
			} else{
				//return "Input '" + input + "' is not in the range " + min + "-" + max + "!";
				return LocaleHandler.getText("num-invalid-range") + min + "-" + max;
			}
		} else {
			//return "Input '" + input + "' is not a valid number!";
			return LocaleHandler.getText("num-invalid") + input;
		}
	}
}