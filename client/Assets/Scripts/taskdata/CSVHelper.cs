using System;
using System.Collections.Generic;

/// <summary>
/// Class which offers static methods to help building and sending CSVs to the server.
/// </summary>
public static class CSVHelper{

	/// <summary>
	/// A dict which maps string swap values to their alternative form.
	/// </summary>
	private static Dictionary<string, string> csvSwapValues = new Dictionary<string, string>();

	/// <summary>
	/// Adds the swap-value pair to the swap dict.
	/// </summary>
	/// 
	/// <param name="swap">swap string</param>
	/// <param name="value">alternative swap</param>
	public static void addSwap(string swap, string value){
		csvSwapValues.Add(swap, value);
	}

	/// <summary>
	/// Swaps all occurences of the swap values in parameter string to their alternative.
	/// </summary>
	/// 
	/// <returns>the encoded text</returns>
	/// 
	/// <param name="text">the text to be encoded</param>
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

	/// <summary>
	/// Swaps all occurences of the alternative values in parameter string to their swaps.
	/// </summary>
	/// 
	/// <returns>the decoded text</returns>
	/// 
	/// <param name="text">the text to be decoded</param>
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