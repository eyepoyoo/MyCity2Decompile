using System;

// Token: 0x02000005 RID: 5
public class AmuzoBetterFifo<T>
{
	// Token: 0x06000020 RID: 32 RVA: 0x00002868 File Offset: 0x00000A68
	public AmuzoBetterFifo()
	{
		this._list = new AmuzoBetterList<T>(32);
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00002880 File Offset: 0x00000A80
	public AmuzoBetterFifo(int capacity)
	{
		this._list = new AmuzoBetterList<T>(capacity);
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000022 RID: 34 RVA: 0x00002894 File Offset: 0x00000A94
	public int Count
	{
		get
		{
			return this._list.size;
		}
	}

	// Token: 0x06000023 RID: 35 RVA: 0x000028A4 File Offset: 0x00000AA4
	public void Release()
	{
		this._list.Release();
	}

	// Token: 0x06000024 RID: 36 RVA: 0x000028B4 File Offset: 0x00000AB4
	public void Clear()
	{
		this._list.Clear();
	}

	// Token: 0x06000025 RID: 37 RVA: 0x000028C4 File Offset: 0x00000AC4
	public void PushBack(T val)
	{
		this._list.Add(val);
	}

	// Token: 0x06000026 RID: 38 RVA: 0x000028D4 File Offset: 0x00000AD4
	public T PeekFront()
	{
		return this._list[0];
	}

	// Token: 0x06000027 RID: 39 RVA: 0x000028E4 File Offset: 0x00000AE4
	public T PopFront()
	{
		T t = this._list[0];
		this._list.RemoveAt(0);
		return t;
	}

	// Token: 0x0400000A RID: 10
	private AmuzoBetterList<T> _list;
}
