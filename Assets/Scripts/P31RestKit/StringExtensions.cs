using System;
using System.Collections.Generic;

namespace Prime31
{
	// Token: 0x02000009 RID: 9
	public static class StringExtensions
	{
		// Token: 0x06000023 RID: 35 RVA: 0x00002EDC File Offset: 0x000010DC
		public static Dictionary<string, string> parseQueryString(this string self)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string[] array = self.Split(new char[] { '?' });
			string[] array2;
			if (array.Length != 2)
			{
				array2 = self.Split(new char[] { '&' });
			}
			else
			{
				array2 = array[1].Split(new char[] { '&' });
			}
			foreach (string text in array2)
			{
				string[] array4 = text.Split(new char[] { '=' });
				dictionary.Add(array4[0], array4[1]);
			}
			return dictionary;
		}
	}
}
