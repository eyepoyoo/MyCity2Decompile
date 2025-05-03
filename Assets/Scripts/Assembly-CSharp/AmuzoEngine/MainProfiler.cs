namespace AmuzoEngine
{
	public class MainProfiler : DebugProfilerBase
	{
		private const string LOG_TAG = "[MainProfiler] ";

		private static MainProfiler _instance;

		public static MainProfiler _pInstance
		{
			get
			{
				DebugProfilerBase.EnsureInstance(ref _instance);
				return _instance;
			}
		}
	}
}
