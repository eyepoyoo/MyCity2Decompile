using System;
using System.Collections.Generic;

namespace LEGO.CoreSDK
{
	// Token: 0x0200005B RID: 91
	internal static class MyExtensions
	{
		// Token: 0x0600015E RID: 350 RVA: 0x0000732C File Offset: 0x0000552C
		public static void Shuffle<T>(this IList<T> list)
		{
			Random random = new Random();
			int i = list.Count;
			while (i > 1)
			{
				i--;
				int num = random.Next(i + 1);
				T t = list[num];
				list[num] = list[i];
				list[i] = t;
			}
		}
	}
}
