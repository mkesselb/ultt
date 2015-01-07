using System;
using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;
using System.Collections;

public class LocaleHandler
{
	private static Dictionary<string, Dictionary<string, string>> localeTranslation = new Dictionary<string, Dictionary<string, string>>();

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

	public static string getText(string ident, string locale){
		if (localeTranslation.ContainsKey (locale) && localeTranslation [locale].ContainsKey (ident)) {
			return localeTranslation[locale][ident];
		}

		return "undefined";
	}
}