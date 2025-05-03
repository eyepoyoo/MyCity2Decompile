using System;
using System.Runtime.InteropServices;

namespace LEGO.CoreSDK.Native.Unity
{
	// Token: 0x0200002D RID: 45
	public class NativeMacAPI : INativeAPI
	{
		// Token: 0x06000068 RID: 104
		[DllImport("MacNative")]
		private static extern IntPtr GetCurrentLocale();

		// Token: 0x06000069 RID: 105
		[DllImport("MacNative")]
		private static extern IntPtr GetLocaleDisplayString(string locale);

		// Token: 0x0600006A RID: 106 RVA: 0x00003810 File Offset: 0x00001A10
		public string GetDeviceLocale()
		{
			return Marshal.PtrToStringAuto(NativeMacAPI.GetCurrentLocale());
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000381C File Offset: 0x00001A1C
		public string GetDisplayStringForLocale(string locale)
		{
			return Marshal.PtrToStringAuto(NativeMacAPI.GetLocaleDisplayString(locale));
		}
	}
}
