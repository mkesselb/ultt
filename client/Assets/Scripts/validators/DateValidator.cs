using System;

/// <summary>
/// Validator class for date input fields. Date format can be specified.
/// Implements <see cref="IValidator"/>.
/// </summary>
public class DateValidator : IValidator
{
	/// <summary>
	/// The format.
	/// </summary>
	private string format = "yyyy-MM-dd HH:mm:ss";

	/// <summary>
	/// Initializes a new instance of the <see cref="DateValidator"/> class.
	/// The parameter format string makes it possible to override the default date format.
	/// </summary>
	/// 
	/// <param name="format">optional format string to use for the validator.</param>
	public DateValidator(string format = ""){
		if (format.Length > 0) {
			this.setDateFormat(format);
		}
	}

	/// <summary>
	/// Returns the validation message after validating the input string against the specified date format.
	/// 
	/// Returns an empty string if validation is successful.
	/// </summary>
	/// 
	/// <returns>the validation message.</returns>
	/// 
	/// <param name="input">the input date to be validated as string</param>
	public string validateInput(string input){
		//try to parse input into validator format
		DateTime dt;
		bool result = DateTime.TryParseExact (input, this.format, null, System.Globalization.DateTimeStyles.None, out dt);
		if (result) {
			return "";
		} else {
			return "Input '" + input + "' is not a date in format: " + this.format;
		}
	}

	/// <summary>
	/// Sets the date format of the validator to parameter date format.
	/// </summary>
	public void setDateFormat(string format){
		this.format = format;
	}
}