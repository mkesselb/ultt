using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Form{
	private Dictionary<string, string> formValues;

	private Color defaultColor;
	private Color errorColor;
	private Dictionary<string, GameObject> formFields;
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

	public Form(Dictionary<string, string> formFields, Dictionary<string, IValidator> formValidators){
		//both dicts shall have the same key strings
		this.formValues = formValues;
		this.formValidators = formValidators;
	}
	
	public Form(List<string> formKeys, List<GameObject> formFields, List<IValidator> formValidators, Color defaultColor, Color errorColor){
		//all lists shall correspond in positions
		this.formFields = new Dictionary<string, GameObject>(); 
		this.formValidators = new Dictionary<string, IValidator>(); 
		this.defaultColor = defaultColor;
		this.errorColor = errorColor;
		for(int i = 0; i < formKeys.Count; i++){
			//reset all colors
			formFields[i].GetComponent<Image>().color = defaultColor;
			this.formFields.Add(formKeys[i], formFields[i]);
			this.formValidators.Add(formKeys[i], formValidators[i]);
		}
	}

	public string getValue(string key){
		if (formFields.ContainsKey (key)) {
			if(formFields[key].GetComponent<InputField>().contentType == InputField.ContentType.Password){
				return formFields[key].GetComponent<InputField>().text;
			} else{
				return formFields[key].transform.FindChild("Text").GetComponent<Text> ().text;
			}
		}

		return "";
	}

	public Dictionary<string, string> validateForm(){
		Dictionary<string, string> validation = new Dictionary<string, string> ();

		foreach (string key in formFields.Keys) {
			string text = getValue (key);
			/*string text = (formFields[key].transform.FindChild("Text").GetComponent<Text> ().text == null 
			              	? "" : formFields[key].transform.FindChild("Text").GetComponent<Text> ().text);*/
			string v = formValidators[key].validateInput(text);
			if(v.Length > 0){
				//validation error, paint error color
				validation.Add(key, v);
				formFields[key].GetComponent<Image>().color = this.errorColor;
			} else{
				formFields[key].GetComponent<Image>().color = this.defaultColor;
			}
		}

		return validation;
	}
}