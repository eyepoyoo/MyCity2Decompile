using System;
using System.Collections.Generic;

public class BooleanStateRequestsSimple
{
	private bool _defaultState;

	private Action<bool> _onStateChanged;

	private readonly List<object> _requestSources = new List<object>();

	private int _numRequests;

	public bool _pState { get; private set; }

	public BooleanStateRequestsSimple(bool defaultState, Action<bool> onStateChanged = null)
	{
		_defaultState = defaultState;
		_onStateChanged = onStateChanged;
	}

	public void AddContraryStateRequest(object source)
	{
		if (!_requestSources.Contains(source))
		{
			_requestSources.Add(source);
			_numRequests++;
			if (_numRequests == 1 && _onStateChanged != null)
			{
				_onStateChanged(!_defaultState);
			}
		}
		_pState = _numRequests == 0 == _defaultState;
	}

	public void RemoveContraryStateRequest(object source)
	{
		if (_requestSources.Contains(source))
		{
			_requestSources.Remove(source);
			_numRequests--;
			if (_numRequests == 0 && _onStateChanged != null)
			{
				_onStateChanged(_defaultState);
			}
		}
		_pState = _numRequests == 0 == _defaultState;
	}

	public static implicit operator bool(BooleanStateRequestsSimple targ)
	{
		return targ._pState;
	}
}
