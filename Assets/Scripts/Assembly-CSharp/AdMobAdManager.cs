using System.Collections.Generic;
using UnityEngine;

public class AdMobAdManager : AdManagerInterface
{
	private const bool DO_ENABLE_ON_IOS = false;

	private const string INTERSTITIAL_AD_UNIT_ID = "ca-app-pub-8745323685651687/5135136650";

	private const string BANNER_AD_UNIT_ID = "ca-app-pub-8745323685651687/3658403455";

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
			return false;
		}
	}

	public bool _pDoesSupportMoreApps
	{
		get
		{
			return false;
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
				Debug.Log("AdMob AdManager: " + ((!_isFrozen) ? "Freeze" : "UnFreeze"));
			}
			_isFrozen = value;
		}
	}

	public void init()
	{
		Debug.Log("AdMob AdManager: Initialised");
		_isInitialised = true;
	}

	public void cacheInterstitial(string location)
	{
	}

	public void showInterstitial(string location)
	{
	}

	public void cacheBanner(string location)
	{
	}

	public void showBanner(string location)
	{
	}

	public void cacheMoreApps()
	{
	}

	public void showMoreApps()
	{
	}

	public void cacheVideo(string location)
	{
	}

	public void showVideo(string location)
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
