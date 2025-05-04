using System;
using System.Collections.Generic;

// Token: 0x02000008 RID: 8
public class ObjectPoolFF<T> : ObjectPool<T> where T : class, IObjectPoolFFItem<T>
{
	// Token: 0x0600003E RID: 62 RVA: 0x00002C2C File Offset: 0x00000E2C
	public ObjectPoolFF(int size, ObjectPool<T>.DInternalAlloc internalAllocFn, ObjectPool<T>.DInternalFree internalFreeFn)
		: base(size, internalAllocFn, internalFreeFn)
	{
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00002C38 File Offset: 0x00000E38
	protected override LinkedListNode<T> GetActiveListNode(T obj)
	{
		return obj._pObjectPoolListNode;
	}
}
