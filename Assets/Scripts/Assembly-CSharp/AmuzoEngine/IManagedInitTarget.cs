namespace AmuzoEngine
{
	public interface IManagedInitTarget
	{
		bool _pIsInitialized { get; }

		void Initialize(EManagedInitType initType);
	}
}
