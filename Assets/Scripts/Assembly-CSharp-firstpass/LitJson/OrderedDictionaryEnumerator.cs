using System;
using System.Collections;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x02000022 RID: 34
	internal class OrderedDictionaryEnumerator : IEnumerator, IDictionaryEnumerator
	{
		// Token: 0x06000250 RID: 592 RVA: 0x0000E7B4 File Offset: 0x0000C9B4
		public OrderedDictionaryEnumerator(IEnumerator<KeyValuePair<string, JsonData>> enumerator)
		{
			this.list_enumerator = enumerator;
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000E7C4 File Offset: 0x0000C9C4
		public object Current
		{
			get
			{
				return this.Entry;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000E7D4 File Offset: 0x0000C9D4
		public DictionaryEntry Entry
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return new DictionaryEntry(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0000E800 File Offset: 0x0000CA00
		public object Key
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Key;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000E820 File Offset: 0x0000CA20
		public object Value
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Value;
			}
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000E840 File Offset: 0x0000CA40
		public bool MoveNext()
		{
			return this.list_enumerator.MoveNext();
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000E850 File Offset: 0x0000CA50
		public void Reset()
		{
			this.list_enumerator.Reset();
		}

		// Token: 0x0400013D RID: 317
		private IEnumerator<KeyValuePair<string, JsonData>> list_enumerator;
	}
}
