using System;

namespace LEGO.CoreSDK.Native
{
	// Token: 0x02000030 RID: 48
	public interface INativeAPI
	{
		// Token: 0x06000076 RID: 118
		string GetDeviceLocale();

		// Token: 0x06000077 RID: 119
		string GetDisplayStringForLocale(string locale);
	}
}
