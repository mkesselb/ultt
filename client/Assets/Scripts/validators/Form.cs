using System;
using System.Collections;
using System.Collections.Generic;

public class Form{
	private Dictionary<string, string> formValues;
	private Dictionary<string, IValidator> formValidators;

	public Form(List<string> formValues, List<IValidator> formValidators){
		//both lists shall have equal length
		this.formValues = new Dictionary<string, string> ();
		this.formValidators = new Dictionary<string, IValidator> ();
		for (int i = 0; i < formValues.Count; i++) {
			this.formValues.Add(i.ToString(), formValues[i]);
			this.formValidators.Add(i.ToString(), formValidators[i]);
		}
	}

	public Form(Dictionary<string, string> formValues, Dictionary<string, IValidator> formValidators){
		//both dicts shall have the same key strings
		this.formValues = formValues;
		this.formValidators = formValidators;
	}

	public Dictionary<string, string> validateForm(){
		Dictionary<string, string> validation = new Dictionary<string, string> ();

		foreach (string key in formValues.Keys) {
			string v = formValidators[key].validateInput(formValues[key]);
			if(v.Length > 0){
				validation.Add(key, v);
			}
		}

		return validation;
	}
}