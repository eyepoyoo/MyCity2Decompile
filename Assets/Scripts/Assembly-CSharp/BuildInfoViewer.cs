using UnityEngine;

public class BuildInfoViewer : MonoBehaviour
{
	private const string NEW_LINE = "\r\n";

	public string[] _validFlowLocationsToShow;

	public Rect _decimalScreenRectToDisplayIn;

	public Rect _decimalScreenRectToClickIn;

	public int _numClicksRequired;

	public GUIStyle _textStyle;

	public bool _showBuildDate;

	public bool _showBambooVersion;

	public bool _showProject;

	public bool _showProjectCode;

	public bool _showBuildId;

	public bool _showTitle;

	public bool _showPlatform;

	private string _textToShow;

	private int _numClicksInArea;

	private int _lastTouch = -1;

	private bool _isAtValidLocation;

	private bool _doShow;

	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		fetchText();
		_decimalScreenRectToDisplayIn = decimalRectToActual(_decimalScreenRectToDisplayIn);
		_decimalScreenRectToDisplayIn.y = (float)Screen.height - _decimalScreenRectToDisplayIn.y - _decimalScreenRectToDisplayIn.height;
		_decimalScreenRectToClickIn = decimalRectToActual(_decimalScreenRectToClickIn);
	}

	private void Update()
	{
		updateLocation();
		updateClickDetection();
	}

	private void OnGUI()
	{
		if (_doShow)
		{
			GUI.Label(_decimalScreenRectToDisplayIn, _textToShow, _textStyle);
		}
	}

	private Rect decimalRectToActual(Rect decimalRect)
	{
		decimalRect.x *= Screen.width;
		decimalRect.width *= Screen.width;
		decimalRect.y *= Screen.height;
		decimalRect.height *= Screen.height;
		return decimalRect;
	}

	private void updateClickDetection()
	{
		if (!_isAtValidLocation)
		{
			_numClicksInArea = 0;
			_doShow = false;
			return;
		}
		if (!Input.GetMouseButtonUp(0) && Input.touchCount <= 0)
		{
			_lastTouch = -1;
			return;
		}
		Vector2 zero = Vector2.zero;
		if (Input.touchCount > 0)
		{
			if (_lastTouch == Input.touches[0].fingerId)
			{
				return;
			}
			zero = Input.touches[0].position;
			_lastTouch = Input.touches[0].fingerId;
		}
		else
		{
			zero = Input.mousePosition;
		}
		if (_decimalScreenRectToClickIn.Contains(zero))
		{
			_numClicksInArea++;
		}
		_doShow = _numClicksInArea >= _numClicksRequired;
	}

	private void updateLocation()
	{
		if (Facades<FlowFacade>.Instance == null)
		{
			return;
		}
		string currentLocation = Facades<FlowFacade>.Instance.CurrentLocation;
		_isAtValidLocation = false;
		int i = 0;
		for (int num = _validFlowLocationsToShow.Length; i < num; i++)
		{
			if (!(_validFlowLocationsToShow[i] != currentLocation))
			{
				_isAtValidLocation = true;
				break;
			}
		}
	}

	private void fetchText()
	{
		if (string.IsNullOrEmpty(_textToShow))
		{
			_textToShow = string.Empty;
			if (_showBuildDate)
			{
				_textToShow = _textToShow + "DATE: " + AnvilBuildInfo._pBuildDate + "\r\n";
			}
			if (_showBambooVersion)
			{
				_textToShow = _textToShow + "BUILD NUMBER: " + AnvilBuildInfo._pBambooVersion + "\r\n";
			}
			if (_showProject)
			{
				_textToShow = _textToShow + "PROJECT: " + AnvilBuildInfo._pProject + "\r\n";
			}
			if (_showProjectCode)
			{
				_textToShow = _textToShow + "PROJECT CODE: " + AnvilBuildInfo._pProjectCode + "\r\n";
			}
			if (_showBuildId)
			{
				_textToShow = _textToShow + "BUILD ID: " + AnvilBuildInfo._pBuildId + "\r\n";
			}
			if (_showTitle)
			{
				_textToShow = _textToShow + "TITLE: " + AnvilBuildInfo._pTitle + "\r\n";
			}
			if (_showPlatform)
			{
				_textToShow = _textToShow + "PLATFORM: " + AnvilBuildInfo._pPlatform + "\r\n";
			}
		}
	}
}
