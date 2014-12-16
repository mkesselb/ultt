using System;
public class NumberValidator : IValidator
{
	public string validateInput(string input){
		int num;
		bool result = int.TryParse (input, out num);
		if (result) {
			//success
			return "";
		} else {
			return "Input '" + input + "' is not a number!";
		}
	}
}