using System;

namespace LEGO.CoreSDK
{
	// Token: 0x02000056 RID: 86
	public interface IPersistentStorage
	{
		// Token: 0x0600014A RID: 330
		void SetString(string key, string str);

		// Token: 0x0600014B RID: 331
		string GetString(string key);
	}
}
