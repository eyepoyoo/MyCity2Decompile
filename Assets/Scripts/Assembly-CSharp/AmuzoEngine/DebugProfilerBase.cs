using UnityEngine;

namespace AmuzoEngine
{
	public class DebugProfilerBase : MonoBehaviour
	{
		private const string LOG_TAG = "[DebugProfilerBase] ";

		public int _maxSections = 128;

		public Rect _outputRectNorm = new Rect(0f, 0f, 1f, 1f);

		private DebugProfiler _profiler;

		private Rect _outputRectScreen = default(Rect);

		public bool _pIsEnabled
		{
			get
			{
				return base.gameObject.activeSelf;
			}
			set
			{
				base.gameObject.SetActive(value);
			}
		}

		public bool _pIsShowAccumulated
		{
			get
			{
				return _profiler != null && _profiler.IsShowAccumulated;
			}
			set
			{
				if (_profiler != null)
				{
					_profiler.IsShowAccumulated = value;
				}
			}
		}

		protected static void EnsureInstance<T>(ref T instance) where T : DebugProfilerBase
		{
			if (!(instance != null))
			{
				GameObject gameObject = new GameObject(typeof(T).ToString());
				instance = gameObject.AddComponent<T>();
				gameObject.SetActive(false);
			}
		}

		protected virtual void Awake()
		{
			_profiler = new DebugProfiler(_maxSections);
		}

		protected virtual void Start()
		{
			if (_profiler != null)
			{
				_profiler.Reset();
			}
		}

		private void OnGUI()
		{
			if (_profiler != null && Event.current.type == EventType.Repaint)
			{
				_profiler.FinalizeFrame();
				_outputRectScreen.xMin = _outputRectNorm.xMin * (float)Screen.width;
				_outputRectScreen.yMin = _outputRectNorm.yMin * (float)Screen.height;
				_outputRectScreen.xMax = _outputRectNorm.xMax * (float)Screen.width;
				_outputRectScreen.yMax = _outputRectNorm.yMax * (float)Screen.height;
				_profiler.RenderGui(_outputRectScreen);
				_profiler.Reset();
			}
		}

		public void BeginSection(string label)
		{
			if (_pIsEnabled && _profiler != null)
			{
				_profiler.BeginSection(label);
			}
		}

		public void EndSection(bool isDumpToConsole = false)
		{
			if (_pIsEnabled && _profiler != null)
			{
				_profiler.EndSection(isDumpToConsole);
			}
		}

		public void SetInt(string data, int value)
		{
			if (_pIsEnabled && _profiler != null)
			{
				_profiler.SetInt(data, value);
			}
		}

		public void Increment(string data)
		{
			if (_pIsEnabled && _profiler != null)
			{
				_profiler.Increment(data);
			}
		}

		public string Dump()
		{
			if (_profiler == null)
			{
				return string.Empty;
			}
			return _profiler.Dump();
		}
	}
}
