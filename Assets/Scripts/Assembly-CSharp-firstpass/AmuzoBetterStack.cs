using System;

// Token: 0x02000004 RID: 4
public class AmuzoBetterStack<T>
{
	// Token: 0x06000017 RID: 23 RVA: 0x00002790 File Offset: 0x00000990
	public AmuzoBetterStack()
	{
		this._list = new AmuzoBetterList<T>(32);
	}

	// Token: 0x06000018 RID: 24 RVA: 0x000027A8 File Offset: 0x000009A8
	public AmuzoBetterStack(int capacity)
	{
		this._list = new AmuzoBetterList<T>(capacity);
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000019 RID: 25 RVA: 0x000027BC File Offset: 0x000009BC
	public int Count
	{
		get
		{
			return this._list.size;
		}
	}

	// Token: 0x0600001A RID: 26 RVA: 0x000027CC File Offset: 0x000009CC
	public void Release()
	{
		this._list.Release();
	}

	// Token: 0x0600001B RID: 27 RVA: 0x000027DC File Offset: 0x000009DC
	public void Clear()
	{
		this._list.Clear();
	}

	// Token: 0x0600001C RID: 28 RVA: 0x000027EC File Offset: 0x000009EC
	public void Push(T val)
	{
		this._list.Add(val);
	}

	// Token: 0x0600001D RID: 29 RVA: 0x000027FC File Offset: 0x000009FC
	public T Peek()
	{
		return this._list[this._list.size - 1];
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002818 File Offset: 0x00000A18
	public T Pop()
	{
		T t = this._list[this._list.size - 1];
		this._list.RemoveAt(this._list.size - 1);
		return t;
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002858 File Offset: 0x00000A58
	public T[] ToArray()
	{
		return this._list.ToArray();
	}

	// Token: 0x04000009 RID: 9
	private AmuzoBetterList<T> _list;
}
