using System;
using System.Collections.Generic;

// Token: 0x02000006 RID: 6
public class ObjectPool<T> where T : class
{
	// Token: 0x06000028 RID: 40 RVA: 0x0000290C File Offset: 0x00000B0C
	public ObjectPool(int size, ObjectPool<T>.DInternalAlloc internalAllocFn, ObjectPool<T>.DInternalFree internalFreeFn)
	{
		this._freeList = new LinkedList<T>();
		this._activeList = new LinkedList<T>();
		this._internalAlloc = internalAllocFn;
		this._internalFree = internalFreeFn;
		this.EnsureInternalAllocFree();
		this.SetSize(size);
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000029 RID: 41 RVA: 0x00002948 File Offset: 0x00000B48
	private int _size
	{
		get
		{
			return this._numFree + this._numActive;
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x0600002A RID: 42 RVA: 0x00002958 File Offset: 0x00000B58
	// (set) Token: 0x0600002B RID: 43 RVA: 0x00002960 File Offset: 0x00000B60
	public int Size
	{
		get
		{
			return this._size;
		}
		set
		{
			this.SetSize(value);
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x0600002C RID: 44 RVA: 0x0000296C File Offset: 0x00000B6C
	public LinkedList<T> ActiveList
	{
		get
		{
			return this._activeList;
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x0600002D RID: 45 RVA: 0x00002974 File Offset: 0x00000B74
	private int _numFree
	{
		get
		{
			return this._freeList.Count;
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x0600002E RID: 46 RVA: 0x00002984 File Offset: 0x00000B84
	private int _numActive
	{
		get
		{
			return this._activeList.Count;
		}
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002994 File Offset: 0x00000B94
	public T Allocate()
	{
		LinkedListNode<T> first = this._freeList.First;
		if (first != null)
		{
			this._freeList.Remove(first);
			this._activeList.AddLast(first);
			return first.Value;
		}
		return (T)((object)null);
	}

	// Token: 0x06000030 RID: 48 RVA: 0x000029D8 File Offset: 0x00000BD8
	public void Free(T obj)
	{
		if (obj != null)
		{
			LinkedListNode<T> activeListNode = this.GetActiveListNode(obj);
			if (activeListNode != null)
			{
				this.FreeNode(activeListNode);
			}
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00002A08 File Offset: 0x00000C08
	public void FreeOldest(int count = 1)
	{
		while (this._activeList.First != null && count > 0)
		{
			this.FreeNode(this._activeList.First);
			count--;
		}
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00002A48 File Offset: 0x00000C48
	public void FreeAll()
	{
		while (this._activeList.First != null)
		{
			this.FreeNode(this._activeList.First);
		}
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00002A7C File Offset: 0x00000C7C
	protected virtual LinkedListNode<T> GetActiveListNode(T obj)
	{
		return this._activeList.Find(obj);
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00002A8C File Offset: 0x00000C8C
	private void FreeNode(LinkedListNode<T> objNode)
	{
		this._activeList.Remove(objNode);
		this._freeList.AddLast(objNode);
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002AA8 File Offset: 0x00000CA8
	private void SetSize(int newSize)
	{
		int size = this._size;
		if (newSize > size)
		{
			this.InternalAllocateObjects(newSize - size);
		}
		else if (newSize < size)
		{
			this.InternalFreeObjects(size - newSize);
		}
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002AE4 File Offset: 0x00000CE4
	private void InternalAllocateObjects(int numObjects)
	{
		while (numObjects > 0)
		{
			this._freeList.AddLast(this._internalAlloc());
			numObjects--;
		}
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00002B1C File Offset: 0x00000D1C
	private void InternalFreeObjects(int numObjects)
	{
		numObjects = this.InternalFreeObjects(ref this._freeList, numObjects);
		if (numObjects > 0)
		{
			this.InternalFreeObjects(ref this._activeList, numObjects);
		}
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00002B50 File Offset: 0x00000D50
	private int InternalFreeObjects(ref LinkedList<T> list, int numObjects)
	{
		while (numObjects > 0)
		{
			this._internalFree(list.Last.Value);
			list.RemoveLast();
			numObjects--;
		}
		return numObjects;
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00002B90 File Offset: 0x00000D90
	private void EnsureInternalAllocFree()
	{
		if (this._internalAlloc == null)
		{
			this._internalAlloc = () => (T)((object)null);
		}
		if (this._internalFree == null)
		{
			this._internalFree = delegate(T obj)
			{
			};
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600003A RID: 58 RVA: 0x00002BFC File Offset: 0x00000DFC
	public IEnumerable<T> ActiveObjects
	{
		get
		{
			LinkedListNode<T> nextNode;
			for (LinkedListNode<T> objNode = this._activeList.First; objNode != null; objNode = nextNode)
			{
				nextNode = objNode.Next;
				yield return objNode.Value;
			}
			yield break;
		}
	}

	// Token: 0x0400000B RID: 11
	private LinkedList<T> _freeList;

	// Token: 0x0400000C RID: 12
	private LinkedList<T> _activeList;

	// Token: 0x0400000D RID: 13
	private ObjectPool<T>.DInternalAlloc _internalAlloc;

	// Token: 0x0400000E RID: 14
	private ObjectPool<T>.DInternalFree _internalFree;

	// Token: 0x02000054 RID: 84
	// (Invoke) Token: 0x0600050C RID: 1292
	public delegate T DInternalAlloc();

	// Token: 0x02000055 RID: 85
	// (Invoke) Token: 0x06000510 RID: 1296
	public delegate void DInternalFree(T obj);
}
