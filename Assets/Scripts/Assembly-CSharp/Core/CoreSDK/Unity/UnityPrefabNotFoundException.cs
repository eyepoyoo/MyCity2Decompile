using System;
using System.Runtime.Serialization;

namespace LEGO.CoreSDK.Unity
{
	// Token: 0x0200003D RID: 61
	[Serializable]
	public class UnityPrefabNotFoundException : Exception
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x00003DEC File Offset: 0x00001FEC
		public UnityPrefabNotFoundException(string message)
			: base(message)
		{
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00003DF8 File Offset: 0x00001FF8
		protected UnityPrefabNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
