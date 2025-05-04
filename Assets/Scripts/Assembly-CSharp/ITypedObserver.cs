public interface ITypedObserver<K>
{
	void Notify(K data);

	void Notify();
}
