using System;
using System.Collections.Generic;

public static class CSVHelper{

	private static Dictionary<string, string> csvSwapValues = new Dictionary<string, string>();

	public static void addSwap(string swap, string value){
		csvSwapValues.Add(swap, value);
	}

	public static String swapEncode(string text){
		string result = text;
		foreach (string s in csvSwapValues.Keys) {
			string val = "";
			if(csvSwapValues.TryGetValue(s, out val)){
				result = result.Replace(s, val);
			}
		}

		return result;
	}

	public static String swapDecode(string text){
		string result = text;
		foreach (string s in csvSwapValues.Keys) {
			string val = "";
			if(csvSwapValues.TryGetValue(s, out val)){
				result = result.Replace(val, s);
			}
		}
		
		return result;
	}
}