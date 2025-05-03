public interface IFastPoolItem
{
	void OnFastInstantiate(FastPool pool);

	void OnFastDestroy();

	void OnCloned(FastPool pool);
}
