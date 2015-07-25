using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class represents a form used to collect and validate user input.
/// The form is used to validate the input elements based on the hooked validators.
/// It is intended to be used in user input panels, before passing the input to the database interface.
/// </summary>
public class Form{

	/// <summary>
	/// This dict saves a mapping from the string name-identifiers of the form fields to the values of the form fields.
	/// This is a deprecated way of validating the form, and is only supported for legacy reasons.
	/// Now, formFields should be used.
	/// </summary>
	private Dictionary<string, string> formValues;

	/// <summary>
	/// The default color of input fields.
	/// </summary>
	private Color defaultColor;

	/// <summary>
	/// The color which is used to mark erroneous input fields.
	/// </summary>
	private Color errorColor;

	/// <summary>
	/// This dict saves a mapping from the string name-identifiers of the form fields to the actual form fields (game objects).
	/// </summary>
	private Dictionary<string, GameObject> formFields;

	/// <summary>
	/// This dict saves a mapping from the string name-identifires of the form fields to the configured validators.
	/// </summary>
	private Dictionary<string, IValidator> formValidators;

	/// <summary>
	/// Initializes a new instance of the <see cref="Form"/> class.
	/// This constructor is deprecated and should not be used anymore.
	/// It relates a list of form input values to a list of validators, which should be in same place.
	/// 
	/// Internally, the map is filled with integer form keys.
	/// </summary>
	/// 
	/// <param name="formValues">list of string form input values.</param>
	/// <param name="formValidators">list of configured form validators.</param>
	public Form(List<string> formValues, List<IValidator> formValidators){
		//both lists shall have equal length
		this.formValues = new Dictionary<string, string> ();
		this.formValidators = new Dictionary<string, IValidator> ();
		for (int i = 0; i < formValues.Count; i++) {
			this.formValues.Add(i.ToString(), formValues[i]);
			this.formValidators.Add(i.ToString(), formValidators[i]);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Form"/> class.
	/// This constructor takes all arguments as-is. This is not the recommended constructor.
	/// The caller has to take care that the keys of the supplied dicts are fine.
	/// </summary>
	/// 
	/// <param name="formFields">dict mapping of keys to form values.</param>
	/// <param name="formValidators">dict mapping of keys to form validators.</param>
	public Form(Dictionary<string, string> formFields, Dictionary<string, IValidator> formValidators){
		//both dicts shall have the same key strings
		this.formValues = formValues;
		this.formValidators = formValidators;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Form"/> class.
	/// Creates the form dicts between the keys-fields and keys-validators.
	/// All of the supplied lists are assumed to have the same order.
	/// </summary>
	/// 
	/// <param name="formKeys">list of string keys for the form dicts.</param>
	/// <param name="formFields">list of form input fields (GameObjects).</param>
	/// <param name="formValidators">list of form validators.</param>
	/// <param name="defaultColor">default color for non-erroneous fields.</param>
	/// <param name="errorColor">color for marking of erronous fields.</param>
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

	/// <summary>
	/// With parameter key, fetches the corresponding value from the formField dict.
	/// 
	/// If no value can be found, "" is returned.
	/// </summary>
	/// 
	/// <returns>the string value matching the key.</returns>
	/// 
	/// <param name="key">the string form key.</param>
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

	/// <summary>
	/// Main validation method. Fetches all input values of the registered form fields, and returns a dict mapping of form keys to error messages.
	/// Each of the registered fields is validated against the specified validator.
	/// 
	/// If the validation is successful (contains no errors), the returned dict is empty.
	/// </summary>
	/// 
	/// <returns>the dict mapping of form keys to error messages.</returns>
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