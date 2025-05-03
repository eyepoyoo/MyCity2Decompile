using System.Collections.Generic;
using UnityEngine;

public class ChartboostAdManager : AdManagerInterface
{
	private const string CHARTBOOST_JSON_NAME = "chartboost";

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
			return false;
		}
	}

	public bool _pDoesSupportTracking
	{
		get
		{
			return false;
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
				Debug.Log("Chartboost Ad Manager: " + ((!_isFrozen) ? "Freeze" : "UnFreeze"));
			}
			_isFrozen = value;
		}
	}

	public void init()
	{
		_isInitialised = true;
		Debug.Log("Chartboost AdManager: Initalised");
	}

	public void cacheInterstitial(string location)
	{
	}

	public void showInterstitial(string location)
	{
	}

	public void cacheVideo(string location)
	{
	}

	public void showVideo(string location)
	{
	}

	public void showMoreApps()
	{
	}

	public void cacheMoreApps()
	{
	}

	public void cacheBanner(string location)
	{
	}

	public void showBanner(string location)
	{
	}

	public void trackEvent(string eventIdentifier)
	{
	}

	public void trackEventWithMetadata(string eventIdentifier, Dictionary<string, string> metadata)
	{
	}

	public void trackEventWithValue(string eventIdentifier, float value)
	{
	}

	public void trackEventWithValueAndMetadata(string eventIdentifier, float value, Dictionary<string, object> metadata)
	{
	}
}
