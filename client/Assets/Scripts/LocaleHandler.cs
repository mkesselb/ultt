using System;
using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Locale handler utility class, offering static methods that deal with the translation of messages.
/// Reads the needed locale file from the resources in './locales/localeName.txt', and sets up an internal map of localized messages.
/// </summary>
public class LocaleHandler
{
	private static string lang = "";

	/// <summary>
	/// A nested localization dict. The outer dict represents the mapping from locale-string to a dict of actual localized messages.
	/// The inner dict represents the mapping of the message-identifier strings to the actual localization, of the outer language.
	/// </summary>
	private static Dictionary<string, Dictionary<string, string>> localeTranslation = new Dictionary<string, Dictionary<string, string>>();

	/// <summary>
	/// Setups the localization mapping for the parameter locale-string.
	/// This is done by trying to fetch the 'locale.txt' file in the resources './locales/', and populating the localization dict for the parameter locale.
	/// 
	/// This method has to be called before a meaningful localization can happen for any locale.
	/// </summary>
	/// 
	/// <param name="locale">string name of the locale to be fetched.</param>
	public static void setupMapping(string locale){
		//read from ./locales/locale.txt into the dict of errorcodes
		Dictionary<string, string> dict = new Dictionary<string, string> ();
		
		string line = "";
		System.IO.StreamReader file = new System.IO.StreamReader("./Assets/Scripts/locales/" + locale + ".txt");
		while((line = file.ReadLine()) != null)
		{
			if(line.Length != 0 && !line.StartsWith("#")){
				string[] parts = line.Split(new char[]{';'});
				dict.Add(parts[0], parts[1]);
			}
		}
		
		localeTranslation.Add (locale, dict);
	}

	public static void setLang(string newLang){
		lang = newLang;
	}

	/// <summary>
	/// Returns the localized text for the parameter message-identifier and locale strings.
	/// 
	/// If the localization cannot be found (e.g. if the locale dict is not populated, or the localization files are outdated), 'undefined' is returned.
	/// </summary>
	/// 
	/// <returns>The localized text for the parameter message-identifier and locale strings.</returns>
	/// 
	/// <param name="ident">The message-identifier to be localized.</param>
	/// <param name="locale">The locale string.</param>
	public static string getText(string ident, string locale=""){
		if (locale == "") {
			locale = lang;
		}

		if (localeTranslation.ContainsKey (locale) && localeTranslation [locale].ContainsKey (ident)) {
			return localeTranslation[locale][ident];
		}

		return "undefined";
	}
}