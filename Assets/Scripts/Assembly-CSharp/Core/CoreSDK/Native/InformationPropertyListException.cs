using System;
using System.Runtime.Serialization;

namespace LEGO.CoreSDK.Native
{
	// Token: 0x0200002F RID: 47
	[Serializable]
	public class InformationPropertyListException : Exception
	{
		// Token: 0x06000074 RID: 116 RVA: 0x0000390C File Offset: 0x00001B0C
		public InformationPropertyListException(string message)
			: base(message)
		{
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003918 File Offset: 0x00001B18
		protected InformationPropertyListException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
