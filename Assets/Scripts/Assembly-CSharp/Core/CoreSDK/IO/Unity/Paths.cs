using System;
using System.IO;
using UnityEngine;

namespace LEGO.CoreSDK.IO.Unity
{
	// Token: 0x02000012 RID: 18
	public class Paths : IPaths
	{
		// Token: 0x0600002A RID: 42 RVA: 0x00002B44 File Offset: 0x00000D44
		public string CacheDirectory()
		{
			string text = Path.Combine(Application.persistentDataPath, "LEGO_SDK_Cache");
			Directory.CreateDirectory(text);
			return text;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002B6C File Offset: 0x00000D6C
		public string InjectionDirectory()
		{
			return Application.persistentDataPath;
		}
	}
}
