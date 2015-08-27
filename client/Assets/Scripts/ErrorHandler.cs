using System;
using System.Collections.Generic;

/// <summary>
/// Utility class, providing means to display localized error messages to the user.
/// Reads the needed locale file from the resources in './errorcodes/localeName.txt', and sets up an internal map of localized messages.
/// </summary>
public class ErrorHandler
{
	/// <summary>
	/// A nested localization dict for error messages. The outer dict represents the mapping from locale-string to a dict of actual localized messages.
	/// The inner dict represents the mapping of the message-identifier strings to the actual localization, of the outer language.
	/// </summary>
	private Dictionary<string, Dictionary<int, string>> errorcodes = new Dictionary<string, Dictionary<int, string>>();

	/// <summary>
	/// Initializes a new instance of the <see cref="ErrorHandler"/> class, setting up the localization dict with the parameter language.
	/// </summary>
	/// 
	/// <param name="lang">the language string to be fetched in to the internal localization dict.</param>
	public ErrorHandler (string lang)
	{
		this.setupErrorCodes (lang);
	}

	/// <summary>
	/// Setups the localized error codes for the parameter language string.
	/// This is done by trying to fetch the 'locale.txt' file in the resources './errorcodes/', and populating the localization dict for the parameter locale.
	/// </summary>
	/// 
	/// <param name="lang">the language string for which the messages should be fetched.</param>
	private void setupErrorCodes(string lang){
		//read from ./errorcodes/lang.txt into the dict of errorcodes
		Dictionary<int, string> dict = new Dictionary<int, string> ();

		string line = "";
		System.IO.StreamReader file = new System.IO.StreamReader("./Assets/Scripts/errorcodes/" + lang + ".txt");
		while((line = file.ReadLine()) != null)
		{
			if(line.Length != 0 && !line.StartsWith("#")){
				string[] parts = line.Split(new char[]{';'});
				dict.Add(int.Parse(parts[0]), parts[1]);
			}
		}

		errorcodes.Add (lang, dict);
	}

	/// <summary>
	/// Returns the localized text for the parameter message-identifier and locale strings.
	/// 
	/// If the localization cannot be found (e.g. if the locale dict is not populated, or the localization files are outdated), a default string is returned.
	/// </summary>
	/// 
	/// <returns>The localized error message.</returns>
	/// 
	/// <param name="errorcode">the integer error code number, received from the server.</param>
	/// <param name="lang">the language string.</param>
	public string getErrorMessage(int errorcode, string lang){
		if (errorcodes.ContainsKey (lang) && errorcodes [lang].ContainsKey (errorcode)) {
			return errorcodes [lang] [errorcode];
		}

		//default
		return LocaleHandler.getText("general-problem");
	}
}