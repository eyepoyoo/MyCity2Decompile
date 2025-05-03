using System.Collections.Generic;
using UnityEngine;

public class FrameRateMonitor : MonoBehaviour
{
	public class BarChartBar
	{
		public float decimalHeight = 1f;

		public Color barColour = Color.green;

		public float creationTime;
	}

	private const bool ALWAYS_ON = true;

	private static Color[] barGradient = new Color[5]
	{
		Color.green,
		Color.green,
		Color.yellow,
		Color.red,
		Color.red
	};

	private static float MAX_FPS = 60f;

	private static FrameRateMonitor _instance;

	public Rect _barChartPosition = new Rect(0f, 0f, 0.15f, 0.15f);

	public int _numBars = 15;

	public float _secondsPerBar = 1f;

	public bool _doShow;

	private List<BarChartBar> _barChart = new List<BarChartBar>();

	private float frameCount;

	private float frameTime;

	private Texture2D _blackTexture;

	private Texture2D _whiteTexture;

	private float _barWidth;

	private float _legendWidth;

	private float _barMaxHeight;

	private GUIStyle _zeroLegendStyle;

	private GUIStyle _midLegendStyle;

	private GUIStyle _maxLegendStyle;

	private Rect _zeroRect;

	private Rect _midRect;

	private Rect _maxRect;

	private Rect _currentRect;

	private bool _doneInit;

	public static FrameRateMonitor Instance
	{
		get
		{
			if (_instance == null)
			{
				GameObject gameObject = new GameObject("FrameRateMonitor");
				_instance = gameObject.AddComponent<FrameRateMonitor>();
			}
			return _instance;
		}
	}

	public int CurrentFrameRate
	{
		get
		{
			if (_barChart == null || _barChart.Count == 0)
			{
				return 0;
			}
			return Mathf.FloorToInt(_barChart[_barChart.Count - 1].decimalHeight * MAX_FPS);
		}
	}

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		else if (!_doneInit)
		{
			_instance = this;
			_doneInit = true;
			if (Application.targetFrameRate != -1)
			{
				MAX_FPS = Mathf.Min(MAX_FPS, Application.targetFrameRate);
			}
			_barChartPosition.x *= Screen.width;
			_barChartPosition.y *= Screen.height;
			_barChartPosition.width *= Screen.width;
			_barChartPosition.height *= Screen.height;
			_blackTexture = new Texture2D(1, 1);
			_blackTexture.SetPixel(0, 0, Color.black);
			_blackTexture.Apply();
			_whiteTexture = new Texture2D(1, 1);
			_whiteTexture.SetPixel(0, 0, Color.white);
			_whiteTexture.Apply();
			_legendWidth = _barChartPosition.width / 15f;
			_barWidth = (_barChartPosition.width - _legendWidth * 2f) / (float)_numBars;
			_barMaxHeight = _barChartPosition.height;
			int fontSize = Mathf.RoundToInt(0.0087890625f * (float)Screen.width);
			_maxLegendStyle = new GUIStyle();
			_maxLegendStyle.fontSize = fontSize;
			_maxLegendStyle.wordWrap = false;
			_maxLegendStyle.normal.textColor = Color.white;
			_midLegendStyle = new GUIStyle();
			_midLegendStyle.fontSize = fontSize;
			_midLegendStyle.wordWrap = false;
			_midLegendStyle.normal.textColor = Color.white;
			_midLegendStyle.alignment = TextAnchor.MiddleLeft;
			_zeroLegendStyle = new GUIStyle();
			_zeroLegendStyle.fontSize = fontSize;
			_zeroLegendStyle.wordWrap = false;
			_zeroLegendStyle.normal.textColor = Color.white;
			_zeroLegendStyle.alignment = TextAnchor.LowerLeft;
			_maxRect = _barChartPosition;
			_maxRect.width = _legendWidth;
			_maxRect.height = _barMaxHeight / 3f;
			_maxRect.y = _barChartPosition.y;
			_midRect = _barChartPosition;
			_midRect.width = _legendWidth;
			_midRect.height = _barMaxHeight / 3f;
			_midRect.y = _barChartPosition.y + _maxRect.height;
			_zeroRect = _barChartPosition;
			_zeroRect.width = _legendWidth;
			_zeroRect.height = _barMaxHeight / 3f;
			_zeroRect.y = _barChartPosition.y + _midRect.height + _maxRect.height;
			_currentRect = _barChartPosition;
			_currentRect.width = _legendWidth;
			_currentRect.height = _barMaxHeight / 3f;
			_currentRect.x = _barChartPosition.x + _barChartPosition.width - _legendWidth;
			_currentRect.y = _barChartPosition.y + _maxRect.height;
			Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	private void LateUpdate()
	{
		frameCount += 1f;
		frameTime = Time.realtimeSinceStartup;
		if ((_barChart.Count <= 0 && frameTime > _secondsPerBar) || (_barChart.Count > 0 && frameTime - _barChart[_barChart.Count - 1].creationTime > _secondsPerBar))
		{
			BarChartBar barChartBar = new BarChartBar();
			barChartBar.creationTime = frameTime;
			barChartBar.decimalHeight = Mathf.Clamp01(frameCount / _secondsPerBar / MAX_FPS);
			barChartBar.barColour = ScreenBase.getColourFromArray(barGradient, (1f - barChartBar.decimalHeight) * 0.75f);
			_barChart.Add(barChartBar);
			frameCount = 0f;
		}
		if (_barChart.Count > _numBars)
		{
			do
			{
				_barChart.RemoveAt(0);
			}
			while (_barChart.Count > _numBars);
		}
	}

	private void OnGUI()
	{
		if (!(_whiteTexture == null) && !(_blackTexture == null) && _barChart != null && _barChart.Count > 0 && 0 == 0)
		{
			GUI.DrawTexture(_barChartPosition, _blackTexture);
			GUI.Box(_maxRect, MAX_FPS.ToString(), _maxLegendStyle);
			GUI.Box(_midRect, Mathf.FloorToInt(MAX_FPS / 2f).ToString(), _midLegendStyle);
			GUI.Box(_zeroRect, "0", _zeroLegendStyle);
			GUI.Box(_currentRect, CurrentFrameRate.ToString(), _midLegendStyle);
			float num = 0f;
			Color color = GUI.color;
			float num2 = 0f;
			for (int i = 0; i < _barChart.Count; i++)
			{
				num = _barMaxHeight * _barChart[i].decimalHeight;
				num2 += _barChart[i].decimalHeight;
				GUI.color = _barChart[i].barColour;
				GUI.DrawTexture(new Rect(_barChartPosition.x + (float)i * _barWidth + _legendWidth, _barChartPosition.y + _barMaxHeight - num, _barWidth - 1f, num), _whiteTexture);
			}
			num2 /= (float)_barChart.Count;
			num2 *= _barMaxHeight;
			GUI.color = Color.white;
			GUI.DrawTexture(new Rect(_barChartPosition.x + _legendWidth, _barChartPosition.y + _barMaxHeight - num2, _barChartPosition.width - _legendWidth * 2f, 1f), _whiteTexture);
			GUI.color = color;
		}
	}
}
