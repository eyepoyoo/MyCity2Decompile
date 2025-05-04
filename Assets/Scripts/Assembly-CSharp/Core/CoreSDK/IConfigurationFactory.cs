using System;
using LEGO.CoreSDK.Network;

namespace LEGO.CoreSDK
{
	// Token: 0x02000067 RID: 103
	public interface IConfigurationFactory<T> where T : struct
	{
		// Token: 0x0600017F RID: 383
		ConfigurationContainer<T> GetLocalModel();

		// Token: 0x06000180 RID: 384
		INetworkRequest Download(string configurationURL, bool applyLocale, Action<ConfigurationContainer<T?>> completionHandler);
	}
}
