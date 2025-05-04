using System;

namespace LitJson
{
	// Token: 0x0200002D RID: 45
	internal enum ParserToken
	{
		// Token: 0x04000184 RID: 388
		None = 65536,
		// Token: 0x04000185 RID: 389
		Number,
		// Token: 0x04000186 RID: 390
		True,
		// Token: 0x04000187 RID: 391
		False,
		// Token: 0x04000188 RID: 392
		Null,
		// Token: 0x04000189 RID: 393
		CharSeq,
		// Token: 0x0400018A RID: 394
		Char,
		// Token: 0x0400018B RID: 395
		Text,
		// Token: 0x0400018C RID: 396
		Object,
		// Token: 0x0400018D RID: 397
		ObjectPrime,
		// Token: 0x0400018E RID: 398
		Pair,
		// Token: 0x0400018F RID: 399
		PairRest,
		// Token: 0x04000190 RID: 400
		Array,
		// Token: 0x04000191 RID: 401
		ArrayPrime,
		// Token: 0x04000192 RID: 402
		Value,
		// Token: 0x04000193 RID: 403
		ValueRest,
		// Token: 0x04000194 RID: 404
		String,
		// Token: 0x04000195 RID: 405
		End,
		// Token: 0x04000196 RID: 406
		Epsilon
	}
}
