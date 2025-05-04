using System;
using System.Collections.Generic;

namespace Prime31.Reflection
{
	// Token: 0x02000024 RID: 36
	public class SafeDictionary<TKey, TValue>
	{
		// Token: 0x060000DF RID: 223 RVA: 0x00007AC8 File Offset: 0x00005CC8
		public bool tryGetValue(TKey key, out TValue value)
		{
			return this._dictionary.TryGetValue(key, out value);
		}

		// Token: 0x1700000F RID: 15
		public TValue this[TKey key]
		{
			get
			{
				return this._dictionary[key];
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00007AE5 File Offset: 0x00005CE5
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<TKey, TValue>>)this._dictionary).GetEnumerator();
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00007AF4 File Offset: 0x00005CF4
		public void add(TKey key, TValue value)
		{
			object padlock = this._padlock;
			lock (padlock)
			{
				if (!this._dictionary.ContainsKey(key))
				{
					this._dictionary.Add(key, value);
				}
			}
		}

		// Token: 0x0400005F RID: 95
		private readonly object _padlock = new object();

		// Token: 0x04000060 RID: 96
		private readonly Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();
	}
}
