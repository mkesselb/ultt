using System;
public class TextValidator : IValidator
{
	private int min = 1;
	private int max = int.MaxValue;

	public TextValidator(int min = 1, int max = int.MaxValue){
		if (min > 0 && max > min) {
			this.setValidationLength(min, max);
		}
	}

	public string validateInput(string input){
		//text input shall be non-empty and shall not contain a \" character
		int n = input.Length;
		if (input.Length <= 0) {
			return "Empty input is not allowed!";
		}
		if (input.Contains ("\"")) {
			return "No \" input character allowed!";
		}
		if (n < this.min || n > this.max) {
			return "Input not between length bounds of " + this.min + " - " + this.max + "!";
		}
		return "";
	}

	public void setValidationLength(int min, int max){
		this.min = min;
		this.max = max;
	}
}