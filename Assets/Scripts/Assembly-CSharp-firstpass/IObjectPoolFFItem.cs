using System;
using System.Collections.Generic;

// Token: 0x02000007 RID: 7
public interface IObjectPoolFFItem<T>
{
	// Token: 0x1700000B RID: 11
	// (get) Token: 0x0600003D RID: 61
	LinkedListNode<T> _pObjectPoolListNode { get; }
}
