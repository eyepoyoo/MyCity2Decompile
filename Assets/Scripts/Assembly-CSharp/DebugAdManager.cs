using System.Collections.Generic;
using UnityEngine;

public class DebugAdManager : AdManagerInterface
{
	private bool _doLogInRelease = true;

	private bool _isFrozen;

	private bool _isInitialised;

	public bool _pDoesSupportInterstitial
	{
		get
		{
			return true;
		}
	}

	public bool _pDoesSupportVideo
	{
		get
		{
			return true;
		}
	}

	public bool _pDoesSupportMoreApps
	{
		get
		{
			return true;
		}
	}

	public bool _pDoesSupportBanner
	{
		get
		{
			return true;
		}
	}

	public bool _pDoesSupportTracking
	{
		get
		{
			return true;
		}
	}

	public bool _pIsInitialised
	{
		get
		{
			return _isInitialised;
		}
	}

	public bool _pIsFrozen
	{
		get
		{
			return _isFrozen;
		}
		set
		{
			if (_isInitialised && _isFrozen != value)
			{
				Log("Debug AdManager: " + ((!_isFrozen) ? "Freeze" : "UnFreeze"));
			}
			_isFrozen = value;
		}
	}

	public void init()
	{
		Debug.Log("Debug AdManager: Initialised. release logging? > " + _doLogInRelease);
		_isInitialised = true;
	}

	public void cacheInterstitial(string location)
	{
		Log(string.Format("Debug AdManager: Cache interstitial '{0}' ", location));
	}

	public void showInterstitial(string location)
	{
		Log(string.Format("Debug AdManager: Show interstitial '{0}' ", location));
	}

	public void cacheVideo(string location)
	{
		Log(string.Format("Debug AdManager: Cache video '{0}' ", location));
	}

	public void showVideo(string location)
	{
		Log(string.Format("Debug AdManager: Show video '{0}' ", location));
	}

	public void cacheBanner(string location)
	{
		Log(string.Format("Debug AdManager: Cache banner '{0}' ", location));
	}

	public void showBanner(string location)
	{
		Log(string.Format("Debug AdManager: Show banner '{0}' ", location));
	}

	public void showMoreApps()
	{
		Log(string.Format("Debug AdManager: Show more apps"));
	}

	public void cacheMoreApps()
	{
		Log(string.Format("Debug AdManager: Cache more apps"));
	}

	public void trackEvent(string eventIdentifier)
	{
		Log(string.Format("Debug AdManager: Track event '{0}' ", eventIdentifier));
	}

	public void trackEventWithMetadata(string eventIdentifier, Dictionary<string, string> metadata)
	{
		Log(string.Format("Debug AdManager: Track event with metadata '{0}' ", eventIdentifier));
	}

	public void trackEventWithValue(string eventIdentifier, float value)
	{
		Log(string.Format("Debug AdManager: Track event with value '{0}' '{1}'", eventIdentifier, value));
	}

	public void trackEventWithValueAndMetadata(string eventIdentifier, float value, Dictionary<string, object> metadata)
	{
		Log(string.Format("Debug AdManager: Track event with value and metadata '{0}' '{1}'", eventIdentifier, value));
	}

	private void Log(string log)
	{
		if (_doLogInRelease || Debug.isDebugBuild)
		{
			Debug.Log(log);
		}
	}

	private void LogWarning(string log)
	{
		if (_doLogInRelease || !Debug.isDebugBuild)
		{
			Debug.LogWarning(log);
		}
	}
}
