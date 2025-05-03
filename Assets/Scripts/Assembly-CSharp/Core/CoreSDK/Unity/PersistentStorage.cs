using System;
using UnityEngine;

namespace LEGO.CoreSDK.Unity
{
	// Token: 0x02000055 RID: 85
	public class PersistentStorage : IPersistentStorage, IKeyValueStore
	{
		// Token: 0x06000148 RID: 328 RVA: 0x0000718C File Offset: 0x0000538C
		public string GetString(string key)
		{
			return PlayerPrefs.GetString(key);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00007194 File Offset: 0x00005394
		public void SetString(string key, string str)
		{
			PlayerPrefs.SetString(key, str);
		}
	}
}
