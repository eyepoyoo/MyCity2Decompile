using System;
using UnityEngine;

public class PlanarTracker
{
	public Action _onInputEndedCallback;

	public Action _onInputStartedCallback;

	public bool _doRequireMouseDown;

	public bool _doReturnFrameRelativeValue;

	public bool _doNormaliseValues;

	private Rect _viewportTrackingRect;

	private Rect _screenTrackingRect;

	private bool _isMouseDown;

	private Vector2 _touchLastPos;

	private Vector2 _touchStartPos;

	private Vector2 _touchDelta;

	private Vector2 _mouseLastPos;

	private Vector2 _mouseStartPos;

	private Vector2 _mouseDelta;

	private float _fingerTouchStartTime;

	private float _mousePressStartTime;

	private int _touchTracked;

	private int _lastScreenWidth;

	private int _lastScreenHeight;

	public bool _pIsTracking
	{
		get
		{
			return _touchTracked != -1 || _isMouseDown;
		}
	}

	public Rect _pViewportTrackingRect
	{
		get
		{
			return _viewportTrackingRect;
		}
		set
		{
			_viewportTrackingRect = value;
			RefreshScreenRect();
		}
	}

	public Rect _pScreenTrackingRect
	{
		get
		{
			return _screenTrackingRect;
		}
	}

	public int _pInputID
	{
		get
		{
			if (_isMouseDown)
			{
				return 0;
			}
			if (_touchTracked != -1)
			{
				return _touchTracked;
			}
			return -1;
		}
	}

	public Vector2 _pDeltaPos
	{
		get
		{
			if (_isMouseDown)
			{
				return _mouseDelta;
			}
			if (_touchTracked != -1)
			{
				return _touchDelta;
			}
			return Vector2.zero;
		}
	}

	public Vector2 _pStartPos
	{
		get
		{
			if (_isMouseDown)
			{
				return _mouseStartPos;
			}
			if (_touchTracked != -1)
			{
				return _touchStartPos;
			}
			return Vector2.zero;
		}
	}

	public Vector2 _pCurrentPos
	{
		get
		{
			if (_isMouseDown)
			{
				return _mouseLastPos;
			}
			if (_touchTracked != -1)
			{
				return _touchLastPos;
			}
			return Vector2.zero;
		}
	}

	public Vector2 _pCurrentDiff
	{
		get
		{
			if (_isMouseDown)
			{
				return _mouseStartPos - _mouseLastPos;
			}
			if (_touchTracked != -1)
			{
				return _touchStartPos - _touchLastPos;
			}
			return Vector2.zero;
		}
	}

	public Vector2 Update()
	{
		if (_lastScreenWidth != Screen.width || _lastScreenHeight != Screen.height)
		{
			RefreshScreenRect();
		}
		Vector2 result = Vector2.zero;
		if (_touchTracked == -1)
		{
			result = TrackMouse(_screenTrackingRect);
		}
		if (!_isMouseDown)
		{
			result = TrackTouches(_screenTrackingRect);
		}
		return result;
	}

	public void StopTracking()
	{
		_touchTracked = -1;
		_isMouseDown = false;
	}

	private Vector2 TrackMouse(Rect screenRectToTrackIn)
	{
		if (_doRequireMouseDown && (!Input.GetMouseButton(0) || !screenRectToTrackIn.Contains(Input.mousePosition)))
		{
			if (_isMouseDown)
			{
				InputFinished();
			}
			_isMouseDown = false;
			return Vector2.zero;
		}
		Vector2 mouseLastPos = _mouseLastPos;
		_mouseLastPos = new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y);
		_mouseDelta = _mouseLastPos - mouseLastPos;
		if (!_isMouseDown)
		{
			_isMouseDown = true;
			_touchTracked = -1;
			_mousePressStartTime = Time.realtimeSinceStartup;
			_mouseStartPos = _mouseLastPos;
			_mouseDelta = Vector2.zero;
			InputStarted();
			return Vector2.zero;
		}
		if (_doReturnFrameRelativeValue)
		{
			return new Vector2(Input.GetAxis("Mouse X"), 0f - Input.GetAxis("Mouse Y"));
		}
		return (_mouseLastPos - _mouseStartPos).normalized;
	}

	private Vector2 TrackTouches(Rect screenRectToTrackIn)
	{
		if (_touchTracked != -1)
		{
			bool flag = false;
			Touch[] touches = Input.touches;
			for (int i = 0; i < touches.Length; i++)
			{
				Touch touch = touches[i];
				if (touch.fingerId == _touchTracked && touch.phase != TouchPhase.Ended)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				_touchTracked = -1;
				_touchDelta = Vector2.zero;
				InputFinished();
				return Vector2.zero;
			}
		}
		Touch[] touches2 = Input.touches;
		for (int j = 0; j < touches2.Length; j++)
		{
			Touch touch2 = touches2[j];
			if (touch2.fingerId == _touchTracked || (_touchTracked == -1 && screenRectToTrackIn.Contains(touch2.position)))
			{
				Vector2 touchLastPos = _touchLastPos;
				_touchLastPos = touch2.position;
				_touchDelta = _touchLastPos - touchLastPos;
				if (_touchTracked == -1)
				{
					_touchStartPos = _touchLastPos;
					_touchDelta = Vector2.zero;
					_fingerTouchStartTime = Time.realtimeSinceStartup;
					_touchTracked = touch2.fingerId;
					_isMouseDown = false;
					InputStarted();
					return Vector2.zero;
				}
				if (_doReturnFrameRelativeValue)
				{
					return (!_doNormaliseValues) ? (_touchLastPos - touchLastPos) : (_touchLastPos - touchLastPos).normalized;
				}
				return (_touchLastPos - _touchStartPos).normalized;
			}
		}
		return Vector2.zero;
	}

	private void InputStarted()
	{
		if (_onInputStartedCallback != null)
		{
			_onInputStartedCallback();
		}
	}

	private void InputFinished()
	{
		if (_onInputEndedCallback != null)
		{
			_onInputEndedCallback();
		}
	}

	private void RefreshScreenRect()
	{
		_screenTrackingRect = new Rect(_viewportTrackingRect.x * (float)Screen.width, _viewportTrackingRect.y * (float)Screen.height, _viewportTrackingRect.width * (float)Screen.width, _viewportTrackingRect.height * (float)Screen.height);
		_lastScreenWidth = Screen.width;
		_lastScreenHeight = Screen.height;
	}
}
