using System;

namespace LitJson
{
	// Token: 0x02000026 RID: 38
	public enum JsonToken
	{
		// Token: 0x04000141 RID: 321
		None,
		// Token: 0x04000142 RID: 322
		ObjectStart,
		// Token: 0x04000143 RID: 323
		PropertyName,
		// Token: 0x04000144 RID: 324
		ObjectEnd,
		// Token: 0x04000145 RID: 325
		ArrayStart,
		// Token: 0x04000146 RID: 326
		ArrayEnd,
		// Token: 0x04000147 RID: 327
		Int,
		// Token: 0x04000148 RID: 328
		Long,
		// Token: 0x04000149 RID: 329
		Double,
		// Token: 0x0400014A RID: 330
		String,
		// Token: 0x0400014B RID: 331
		Boolean,
		// Token: 0x0400014C RID: 332
		Null
	}
}
