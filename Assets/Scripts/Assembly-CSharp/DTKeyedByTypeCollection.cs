using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class DTKeyedByTypeCollection<TItem> : KeyedCollection<Type, TItem>
{
	public DTKeyedByTypeCollection()
	{
	}

	public DTKeyedByTypeCollection(IEnumerable<TItem> items)
	{
		foreach (TItem item in items)
		{
			Add(item);
		}
	}

	public int GetIndexForKey<T>()
	{
		int num = 0;
		using (IEnumerator<TItem> enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				TItem current = enumerator.Current;
				if (current is T)
				{
					return num;
				}
				num++;
			}
		}
		return -1;
	}

	protected override Type GetKeyForItem(TItem item)
	{
		return item.GetType();
	}

	public T Find<T>()
	{
		using (IEnumerator<TItem> enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				TItem current = enumerator.Current;
				if (current is T)
				{
					return (T)(object)current;
				}
			}
		}
		return default(T);
	}

	public Collection<T> FindAll<T>()
	{
		Collection<T> collection = new Collection<T>();
		using (IEnumerator<TItem> enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				TItem current = enumerator.Current;
				if (current is T)
				{
					collection.Add((T)(object)current);
				}
			}
			return collection;
		}
	}

	protected override void InsertItem(int index, TItem kind)
	{
		base.InsertItem(index, kind);
	}

	protected override void SetItem(int index, TItem kind)
	{
		base.SetItem(index, kind);
	}

	public T Remove<T>()
	{
		using (IEnumerator<TItem> enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				TItem current = enumerator.Current;
				if (current is T)
				{
					Remove(current);
					return (T)(object)current;
				}
			}
		}
		return default(T);
	}

	public Collection<T> RemoveAll<T>()
	{
		return RemoveAll<T>();
	}
}
