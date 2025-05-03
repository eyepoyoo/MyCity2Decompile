using System;
using System.Runtime.Serialization;

namespace LEGO.CoreSDK.Legal
{
	// Token: 0x0200004A RID: 74
	[Serializable]
	public class PolicyParseFailedException : Exception
	{
		// Token: 0x060000D2 RID: 210 RVA: 0x00005320 File Offset: 0x00003520
		public PolicyParseFailedException(string message)
			: base(message)
		{
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000532C File Offset: 0x0000352C
		protected PolicyParseFailedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
