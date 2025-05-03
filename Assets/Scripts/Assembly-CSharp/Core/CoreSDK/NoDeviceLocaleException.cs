using System;
using System.Runtime.Serialization;

namespace LEGO.CoreSDK
{
	// Token: 0x02000031 RID: 49
	[Serializable]
	public class NoDeviceLocaleException : Exception
	{
		// Token: 0x06000078 RID: 120 RVA: 0x00003924 File Offset: 0x00001B24
		public NoDeviceLocaleException(string message)
			: base(message)
		{
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003930 File Offset: 0x00001B30
		protected NoDeviceLocaleException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
