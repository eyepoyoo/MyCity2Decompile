using UnityEngine;

public class CollisionProfiler : MonoBehaviour
{
	private static CollisionProfiler _instance;

	private DebugProfiler _profiler;

	private bool _isEnabled;

	public int _maxSections = 128;

	public float _outputBorder = 64f;

	private Rect _outputRect = new Rect(0f, 0f, Screen.width, Screen.height);

	public bool _isStartEnabled;

	public static bool IsEnabled
	{
		get
		{
			return _instance != null && _instance._isEnabled;
		}
		set
		{
			SetEnabled(value);
		}
	}

	public static bool IsShowAccumulated
	{
		get
		{
			return _instance != null && _instance._isShowAccumulated;
		}
		set
		{
			_instance._isShowAccumulated = value;
		}
	}

	private bool _isShowAccumulated
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

	private void Awake()
	{
		_profiler = new DebugProfiler(_maxSections);
	}

	private void Start()
	{
		if (_profiler != null)
		{
			_profiler.Reset();
		}
		_isEnabled = _isStartEnabled;
	}

	private void OnEnable()
	{
		if (_instance == null)
		{
			_instance = this;
		}
	}

	private void OnDisable()
	{
		if (_instance == this)
		{
			_instance = null;
		}
	}

	private void OnGUI()
	{
		if (_profiler != null && _isEnabled && Event.current.type == EventType.Repaint)
		{
			_profiler.FinalizeFrame();
			_outputRect.xMin = _outputBorder;
			_outputRect.yMin = _outputBorder;
			_outputRect.xMax = (float)Screen.width - _outputBorder;
			_outputRect.yMax = (float)Screen.height - _outputBorder;
			_profiler.RenderGui(_outputRect);
			_profiler.Reset();
		}
	}

	private void _Reset()
	{
		if (_profiler != null)
		{
			_profiler.Reset();
		}
	}

	private void _Finalize()
	{
		if (_profiler != null)
		{
			_profiler.FinalizeFrame();
		}
	}

	private void _BeginSection(string label)
	{
		if (_profiler != null)
		{
			_profiler.BeginSection(label);
		}
	}

	private void _EndSection(bool isDumpToConsole)
	{
		if (_profiler != null)
		{
			_profiler.EndSection(isDumpToConsole);
		}
	}

	private void _SetInt(string data, int value)
	{
		if (_profiler != null)
		{
			_profiler.SetInt(data, value);
		}
	}

	private void _Increment(string data)
	{
		if (_profiler != null)
		{
			_profiler.Increment(data);
		}
	}

	private string _Dump()
	{
		return (_profiler == null) ? string.Empty : _profiler.Dump();
	}

	private static void SetEnabled(bool isWantEnabled)
	{
		if (isWantEnabled && _instance == null)
		{
			GameObject gameObject = new GameObject("collision_profiler");
			if (gameObject != null)
			{
				gameObject.AddComponent<CollisionProfiler>();
			}
		}
		if (_instance != null)
		{
			_instance._isEnabled = isWantEnabled;
		}
	}

	public static void BeginSection(string label)
	{
		if (_instance != null && _instance._isEnabled)
		{
			_instance._BeginSection(label);
		}
	}

	public static void EndSection(bool isDumpToConsole = false)
	{
		if (_instance != null && _instance._isEnabled)
		{
			_instance._EndSection(isDumpToConsole);
		}
	}

	public static void SetInt(string data, int value)
	{
		if (_instance != null && _instance._isEnabled)
		{
			_instance._SetInt(data, value);
		}
	}

	public static void Increment(string data)
	{
		if (_instance != null && _instance._isEnabled)
		{
			_instance._Increment(data);
		}
	}

	public static string Dump()
	{
		if (_instance != null)
		{
			return _instance._Dump();
		}
		return string.Empty;
	}
}
