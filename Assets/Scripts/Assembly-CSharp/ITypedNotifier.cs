public interface ITypedNotifier<T, K> where T : ITypedObserver<K>
{
	void Observe(T observer);

	void Remove(T observer);

	void Notify(K data);

	void Notify();
}
