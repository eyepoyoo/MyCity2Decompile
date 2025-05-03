using System;
using System.Collections.Generic;

// Token: 0x0200005C RID: 92
public static class LINQExtensions
{
	// Token: 0x0600015F RID: 351 RVA: 0x00007380 File Offset: 0x00005580
	public static T? FirstOrNull<T>(this IEnumerable<T> list, Func<T, bool> condition) where T : struct
	{
		foreach (T t in list)
		{
			if (condition(t))
			{
				return new T?(t);
			}
		}
		return null;
	}
}
