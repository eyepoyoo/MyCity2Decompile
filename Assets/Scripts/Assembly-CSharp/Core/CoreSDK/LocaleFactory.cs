using System;
using LEGO.CoreSDK.Native;

namespace LEGO.CoreSDK
{
	// Token: 0x02000039 RID: 57
	public class LocaleFactory : ILocaleFactory
	{
		// Token: 0x06000092 RID: 146 RVA: 0x00003BB8 File Offset: 0x00001DB8
		public LocaleFactory(INativeAPI nativeAPI)
		{
			this._nativeAPI = nativeAPI;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003BC8 File Offset: 0x00001DC8
		public Locale Create(string locale)
		{
			return new Locale(locale, this._nativeAPI.GetDisplayStringForLocale(locale));
		}

		// Token: 0x0400005E RID: 94
		private INativeAPI _nativeAPI;
	}
}
