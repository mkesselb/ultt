using System;
public class NumberValidator : IValidator
{
	private bool allowFloat;
	private int min;
	private int max;

	public NumberValidator(bool allowFloat = false, int min = int.MinValue, int max = int.MaxValue){
		this.allowFloat = allowFloat;
		this.min = min;
		this.max = max;
	}

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
				return "Input '" + input + "' is not in the range " + min + "-" + max + "!";
			}
		} else {
		return "Input '" + input + "' is not a valid number!";
		}
	}
}