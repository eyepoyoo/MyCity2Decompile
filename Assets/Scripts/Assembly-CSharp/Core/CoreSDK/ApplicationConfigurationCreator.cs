using System;

namespace LEGO.CoreSDK
{
	// Token: 0x02000072 RID: 114
	internal class ApplicationConfigurationCreator : IConfigurationCreator<ApplicationConfiguration>
	{
		// Token: 0x060001A8 RID: 424 RVA: 0x00007DE4 File Offset: 0x00005FE4
		public ApplicationConfiguration Create(string json)
		{
			return new ApplicationConfiguration(json);
		}
	}
}
