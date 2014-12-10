using System;
using System.Collections.Generic;

public class ErrorHandler
{
	private Dictionary<string, Dictionary<int, string>> errorcodes = new Dictionary<string, Dictionary<int, string>>();

	public ErrorHandler (string lang)
	{
		this.setupErrorCodes (lang);
	}

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

	public string getErrorMessage(int errorcode, string lang){
		if (errorcodes.ContainsKey (lang) && errorcodes [lang].ContainsKey (errorcode)) {
			return errorcodes [lang] [errorcode];
		}

		//default
		return "Sorry, there was a problem";
	}
}