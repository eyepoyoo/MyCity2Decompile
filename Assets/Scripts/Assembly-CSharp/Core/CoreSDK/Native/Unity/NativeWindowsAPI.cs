using System;

namespace LEGO.CoreSDK.Native.Unity
{
	// Token: 0x0200002C RID: 44
	public class NativeWindowsAPI : INativeAPI
	{
		// Token: 0x06000065 RID: 101 RVA: 0x000037F8 File Offset: 0x000019F8
		public string GetDeviceLocale()
		{
			return "en_US";
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003800 File Offset: 0x00001A00
		public string GetDisplayStringForLocale(string locale)
		{
			return "USA";
		}
	}
}
