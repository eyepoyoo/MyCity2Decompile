using System;
using System.Collections.Generic;

// Token: 0x0200005E RID: 94
public static class DictionaryExtensions
{
	// Token: 0x06000163 RID: 355 RVA: 0x00007490 File Offset: 0x00005690
	public static bool ContainsKeyWithValue<T, U>(this IDictionary<T, U> dictionary, T key, U value)
	{
		if (!dictionary.ContainsKey(key))
		{
			return false;
		}
		U u = dictionary[key];
		return u.Equals(value);
	}
}
