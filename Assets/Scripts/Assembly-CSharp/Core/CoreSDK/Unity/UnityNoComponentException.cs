using System;
using System.Runtime.Serialization;

namespace LEGO.CoreSDK.Unity
{
	// Token: 0x0200003E RID: 62
	[Serializable]
	public class UnityNoComponentException : Exception
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x00003E04 File Offset: 0x00002004
		public UnityNoComponentException(string message)
			: base(message)
		{
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003E10 File Offset: 0x00002010
		protected UnityNoComponentException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
