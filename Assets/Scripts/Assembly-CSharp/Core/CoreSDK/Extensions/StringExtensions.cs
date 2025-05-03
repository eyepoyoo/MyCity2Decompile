using System;
using System.Collections.Generic;

namespace LEGO.CoreSDK.Extensions
{
	// Token: 0x0200005A RID: 90
	public static class StringExtensions
	{
		// Token: 0x0600015D RID: 349 RVA: 0x0000728C File Offset: 0x0000548C
		public static IEnumerable<string> Split(this string str, int maxLength)
		{
			List<string> list = new List<string>(str.Split(new string[] { "\n\n" }, StringSplitOptions.None));
			List<string> list2 = new List<string>();
			if (str.Length > maxLength)
			{
				foreach (string text in list)
				{
					list2.Add(text + "\n");
				}
			}
			else
			{
				list2 = list;
			}
			return list2;
		}
	}
}
