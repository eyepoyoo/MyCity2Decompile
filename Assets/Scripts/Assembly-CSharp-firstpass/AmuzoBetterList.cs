using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000003 RID: 3
public class AmuzoBetterList<T>
{
	// Token: 0x06000009 RID: 9 RVA: 0x000024B8 File Offset: 0x000006B8
	public AmuzoBetterList()
	{
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000024C0 File Offset: 0x000006C0
	public AmuzoBetterList(int capacity)
	{
		this.buffer = new T[capacity];
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000024D4 File Offset: 0x000006D4
	public IEnumerator<T> GetEnumerator()
	{
		if (this.buffer != null)
		{
			for (int i = 0; i < this.size; i++)
			{
				yield return this.buffer[i];
			}
		}
		yield break;
	}

	// Token: 0x17000001 RID: 1
	public T this[int i]
	{
		get
		{
			return this.buffer[i];
		}
		set
		{
			this.buffer[i] = value;
		}
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002510 File Offset: 0x00000710
	private void AllocateMore()
	{
		T[] array = ((this.buffer == null) ? new T[32] : new T[Mathf.Max(this.buffer.Length << 1, 32)]);
		if (this.buffer != null && this.size > 0)
		{
			this.buffer.CopyTo(array, 0);
		}
		this.buffer = array;
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002578 File Offset: 0x00000778
	private void Trim()
	{
		if (this.size > 0)
		{
			if (this.size < this.buffer.Length)
			{
				T[] array = new T[this.size];
				for (int i = 0; i < this.size; i++)
				{
					array[i] = this.buffer[i];
				}
				this.buffer = array;
			}
		}
		else
		{
			this.buffer = null;
		}
	}

	// Token: 0x06000010 RID: 16 RVA: 0x000025F0 File Offset: 0x000007F0
	public void Clear()
	{
		this.size = 0;
	}

	// Token: 0x06000011 RID: 17 RVA: 0x000025FC File Offset: 0x000007FC
	public void Release()
	{
		this.size = 0;
		this.buffer = null;
	}

	// Token: 0x06000012 RID: 18 RVA: 0x0000260C File Offset: 0x0000080C
	public void Add(T item)
	{
		if (this.buffer == null || this.size == this.buffer.Length)
		{
			this.AllocateMore();
		}
		this.buffer[this.size++] = item;
	}

	// Token: 0x06000013 RID: 19 RVA: 0x0000265C File Offset: 0x0000085C
	public void Remove(T item)
	{
		if (this.buffer != null)
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < this.size; i++)
			{
				if (@default.Equals(this.buffer[i], item))
				{
					this.size--;
					this.buffer[i] = default(T);
					for (int j = i; j < this.size; j++)
					{
						this.buffer[j] = this.buffer[j + 1];
					}
					return;
				}
			}
		}
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002700 File Offset: 0x00000900
	public void RemoveAt(int index)
	{
		if (this.buffer != null && index < this.size)
		{
			this.size--;
			this.buffer[index] = default(T);
			for (int i = index; i < this.size; i++)
			{
				this.buffer[i] = this.buffer[i + 1];
			}
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002778 File Offset: 0x00000978
	public T[] ToArray()
	{
		this.Trim();
		return this.buffer;
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000016 RID: 22 RVA: 0x00002788 File Offset: 0x00000988
	public int Count
	{
		get
		{
			return this.size;
		}
	}

	// Token: 0x04000007 RID: 7
	public T[] buffer;

	// Token: 0x04000008 RID: 8
	public int size;
}
