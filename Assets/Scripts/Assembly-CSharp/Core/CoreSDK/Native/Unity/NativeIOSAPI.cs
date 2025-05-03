using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace LEGO.CoreSDK.Native.Unity
{
	// Token: 0x0200002E RID: 46
	public class NativeIOSAPI : INativeAPI
	{
		// Token: 0x0600006C RID: 108 RVA: 0x0000382C File Offset: 0x00001A2C
		public NativeIOSAPI(IAlert alert)
		{
			IntPtr intPtr = NativeIOSAPI.ValidateInformationPropertyList();
			string text = Marshal.PtrToStringAuto(intPtr);
			JSONObject jsonobject = new JSONObject(text, -2, false, false);
			if (jsonobject != null && jsonobject.list != null && jsonobject.list.Count > 0)
			{
				string text2 = jsonobject.list.Select((JSONObject x) => x.str).Aggregate((string curr, string next) => curr + "\n " + next);
				alert.Show("Error", text2);
				throw new InformationPropertyListException(text2);
			}
		}

		// Token: 0x0600006D RID: 109
		[DllImport("__Internal")]
		private static extern IntPtr GetCurrentLocale();

		// Token: 0x0600006E RID: 110
		[DllImport("__Internal")]
		private static extern IntPtr GetLocaleDisplayString(string locale);

		// Token: 0x0600006F RID: 111
		[DllImport("__Internal")]
		private static extern IntPtr ValidateInformationPropertyList();

		// Token: 0x06000070 RID: 112 RVA: 0x000038D8 File Offset: 0x00001AD8
		public string GetDeviceLocale()
		{
			return Marshal.PtrToStringAuto(NativeIOSAPI.GetCurrentLocale());
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000038E4 File Offset: 0x00001AE4
		public string GetDisplayStringForLocale(string locale)
		{
			return Marshal.PtrToStringAuto(NativeIOSAPI.GetLocaleDisplayString(locale));
		}
	}
}
