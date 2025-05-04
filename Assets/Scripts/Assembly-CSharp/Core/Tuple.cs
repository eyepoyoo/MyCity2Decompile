using System;

// Token: 0x0200000A RID: 10
public struct Tuple<A, B>
{
	// Token: 0x06000014 RID: 20 RVA: 0x000025D0 File Offset: 0x000007D0
	public Tuple(A a, B b)
	{
		this.One = a;
		this.Two = b;
	}

	// Token: 0x04000019 RID: 25
	public A One;

	// Token: 0x0400001A RID: 26
	public B Two;
}
