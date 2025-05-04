using System;

namespace LEGO.CoreSDK
{
	// Token: 0x02000069 RID: 105
	public interface IApplicationConfigurationService
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000182 RID: 386
		// (remove) Token: 0x06000183 RID: 387
		event ApplicationConfigurationChangedEventHandler ApplicationConfigurationChanged;

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000184 RID: 388
		ConfigurationSource Source { get; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000185 RID: 389
		IApplicationConfiguration ApplicationConfiguration { get; }
	}
}
