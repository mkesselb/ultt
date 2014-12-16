using System;
public class DateValidator : IValidator
{
	private string format = "yyyy-MM-dd HH:mm:ss";

	public DateValidator(string format = ""){
		if (format.Length > 0) {
			this.setDateFormat(format);
		}
	}

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

	public void setDateFormat(string format){
		this.format = format;
	}
}