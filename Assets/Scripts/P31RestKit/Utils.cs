using System;
using System.Text;
using UnityEngine;

namespace Prime31
{
	// Token: 0x02000004 RID: 4
	public static class Utils
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000026FF File Offset: 0x000008FF
		private static global::System.Random random
		{
			get
			{
				if (Utils._random == null)
				{
					Utils._random = new global::System.Random();
				}
				return Utils._random;
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000271C File Offset: 0x0000091C
		public static string randomString(int size = 38)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < size; i++)
			{
				char c = Convert.ToChar(Convert.ToInt32(Math.Floor(26.0 * Utils.random.NextDouble() + 65.0)));
				stringBuilder.Append(c);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002780 File Offset: 0x00000980
		public static void logObject(object obj)
		{
			string text = Json.encode(obj);
			Utils.prettyPrintJson(text);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000279C File Offset: 0x0000099C
		public static void prettyPrintJson(string json)
		{
			string text = string.Empty;
			if (json != null)
			{
				text = JsonFormatter.prettyPrint(json);
			}
			try
			{
				Debug.Log(text);
			}
			catch (Exception)
			{
				Console.WriteLine(text);
			}
		}

		// Token: 0x0400000B RID: 11
		private static global::System.Random _random;
	}
}
