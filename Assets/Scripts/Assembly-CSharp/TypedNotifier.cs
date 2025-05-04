using System.Collections.Generic;

public class TypedNotifier<T, K> : ITypedNotifier<T, K> where T : ITypedObserver<K>
{
	protected List<T> observers;

	public TypedNotifier()
	{
		observers = new List<T>();
	}

	public virtual void Observe(T observer)
	{
		observers.Add(observer);
	}

	public virtual void Remove(T observer)
	{
		observers.Remove(observer);
	}

	public virtual void Notify(K data)
	{
		IEnumerator<T> enumerator = observers.GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Notify(data);
		}
	}

	public virtual void Notify()
	{
		IEnumerator<T> enumerator = observers.GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Notify();
		}
	}
}
