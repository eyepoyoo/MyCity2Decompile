using System.Collections.Generic;
using UnityEngine;

public class OnScreenLog
{
	public class Layout
	{
		public Rect buttonNormRect;

		public Rect detailLogNormRect;

		public Rect logNormRect;
	}

	private const string LOG_TAG = "[OnScreenLog] ";

	private Layout _layoutLeft = new Layout();

	private Layout _layoutRight = new Layout();

	private Layout _currLayout;

	private int _numLines;

	private bool _isExpanded;

	private LogData.ILine _focusLine;

	public OnScreenLog(int numLines)
	{
		_layoutLeft = new Layout();
		_layoutLeft.buttonNormRect = new Rect(0.5f, 0f, 0.25f, 1f / 32f);
		_layoutLeft.detailLogNormRect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
		_layoutLeft.logNormRect = new Rect(0f, 0f, 0.5f, 1f);
		_layoutRight = new Layout();
		_layoutRight.buttonNormRect = new Rect(0f, 0f, 0.25f, 1f / 32f);
		_layoutRight.detailLogNormRect = new Rect(0f, 0.5f, 0.5f, 0.5f);
		_layoutRight.logNormRect = new Rect(0.5f, 0f, 0.5f, 1f);
		_currLayout = _layoutLeft;
		_numLines = numLines;
	}

	public void OnGUI(LogData logData)
	{
		if (_currLayout == null)
		{
			return;
		}
		Rect position = new Rect((float)Screen.width * _currLayout.buttonNormRect.x, (float)Screen.height * _currLayout.buttonNormRect.y, (float)Screen.width * _currLayout.buttonNormRect.width, (float)Screen.height * _currLayout.buttonNormRect.height);
		if (GUI.Button(position, (!_isExpanded) ? "Show Log" : "Hide Log"))
		{
			_isExpanded = !_isExpanded;
			_focusLine = null;
		}
		if (!_isExpanded)
		{
			return;
		}
		position.x += position.width;
		position.width *= 0.5f;
		if (GUI.Button(position, (!logData._pIsLoggingPaused) ? "Pause Log" : "Resume Log"))
		{
			logData._pIsLoggingPaused = !logData._pIsLoggingPaused;
		}
		position.x += position.width;
		if (GUI.Button(position, "Switch Layout"))
		{
			SwitchLayout();
		}
		Rect rect = new Rect((float)Screen.width * _currLayout.logNormRect.x, (float)Screen.height * _currLayout.logNormRect.y, (float)Screen.width * _currLayout.logNormRect.width, (float)Screen.height * _currLayout.logNormRect.height);
		float num = rect.height / (float)_numLines;
		float num2 = 0f;
		foreach (LogData.ILine item in (IEnumerable<LogData.ILine>)logData)
		{
			Rect position2 = rect;
			position2.height = num;
			position2.y = num2;
			if (GUI.Button(position2, string.Empty))
			{
				_focusLine = item;
			}
			GUI.Label(position2, item._pPartMessage);
			num2 += num;
		}
		if (_focusLine != null)
		{
			Rect rect2 = new Rect((float)Screen.width * _currLayout.detailLogNormRect.x, (float)Screen.height * _currLayout.detailLogNormRect.y, (float)Screen.width * _currLayout.detailLogNormRect.width, (float)Screen.height * _currLayout.detailLogNormRect.height);
			GUI.Box(rect2, string.Empty);
			Rect position2 = rect2;
			position2.height = rect2.height * 0.4f;
			position2.y = rect2.y;
			GUI.Label(position2, _focusLine._pFullMessage);
			position2.height = rect2.height * 0.1f;
			position2.y = rect2.y + rect2.height * 0.4f;
			GUI.Label(position2, _focusLine._pLogType.ToString());
			position2.height = rect2.height * 0.5f;
			position2.y = rect2.y + rect2.height * 0.5f;
			GUI.Label(position2, _focusLine._pStackTrace);
		}
	}

	public void OnLogLineRemoved(LogData.ILine line)
	{
		if (_focusLine == line)
		{
			_focusLine = null;
		}
	}

	private void SwitchLayout()
	{
		if (_currLayout == _layoutLeft)
		{
			_currLayout = _layoutRight;
		}
		else
		{
			_currLayout = _layoutLeft;
		}
	}
}
