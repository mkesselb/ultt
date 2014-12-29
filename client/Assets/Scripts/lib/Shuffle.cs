using System;
using System.Collections.Generic;

public static class Shuffle
{
	public static void shuffle<T>(this IList<T> list)  
	{  
		Random rng = new Random();  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = rng.Next(n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}
}