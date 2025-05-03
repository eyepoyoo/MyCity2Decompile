using System;

namespace LEGO.CoreSDK
{
	// Token: 0x02000057 RID: 87
	[Obsolete("Use IPersistentStorage")]
	public interface IKeyValueStore
	{
		// Token: 0x0600014C RID: 332
		void SetString(string key, string str);

		// Token: 0x0600014D RID: 333
		string GetString(string key);
	}
}
