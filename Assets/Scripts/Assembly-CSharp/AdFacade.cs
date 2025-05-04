using System;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using LitJson;
using UnityEngine;

public class AdFacade : InitialisationObject, ILocationHandler
{
	private const string DISABLE_ADS_KEY = "AdFacade_DisableAds";

	private const string CACHE_INTERSTITIAL_FLOW_KEY = "CacheInterstitialLocation";

	private const string SHOW_INTERSTITIAL_FLOW_KEY = "ShowInterstitialLocation";

	private const string CACHE_BANNER_FLOW_KEY = "CacheBannerLocation";

	private const string SHOW_BANNER_FLOW_KEY = "ShowBannerLocation";

	private const string CACHE_VIDEO_FLOW_KEY = "CacheVideoLocation";

	private const string SHOW_VIDEO_FLOW_KEY = "ShowVideoLocation";

	private const string CACHE_MORE_APPS_FLOW_KEY = "CacheMoreAppsLocation";

	private const string SHOW_MORE_APPS_KEY = "ShowMoreAppsLocation";

	private Action<string, UnityEngine.Object> LOG = PersonalLogs.KayLog;

	private static AdFacade _instance;

	private List<AdManagerInterface> _adManagers = new List<AdManagerInterface>();

	public SupportedAdManagers[] _adManagersToUse;

	public bool _showAdOnQuit = true;

	private bool _doDisableAds;

	public static AdFacade Instance
	{
		get
		{
			return _instance;
		}
	}

	public int locationChangePriority
	{
		get
		{
			return -4;
		}
	}

	public bool _pAreAdsDisabled
	{
		get
		{
			return _doDisableAds;
		}
	}

	protected override void Awake()
	{
		if (_instance != null && _instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		_doDisableAds = ObscuredPrefs.HasKey("AdFacade_DisableAds");
		base.Awake();
	}

	protected override void OnDestroy()
	{
		if (_instance == this)
		{
			_instance = null;
		}
		base.OnDestroy();
	}

	private void OnApplicationQuit()
	{
		if (_showAdOnQuit)
		{
			showInterstitial("Quit");
		}
	}

	public override void startInitialising()
	{
		LOG("AD FACADE: Initialise Advert Facade", base.gameObject);
		_currentState = InitialisationState.INITIALISING;
		FlowFacade.AddLocationHandler(this);
		addAdManagers();
		int i = 0;
		for (int count = _adManagers.Count; i < count; i++)
		{
			_adManagers[i].init();
		}
		_currentState = InitialisationState.FINISHED;
	}

	public void cacheInterstitial(string location)
	{
		if (_doDisableAds || string.IsNullOrEmpty(location))
		{
			return;
		}
		foreach (AdManagerInterface adManager in _adManagers)
		{
			if (adManager._pIsInitialised && adManager._pDoesSupportInterstitial)
			{
				adManager.cacheInterstitial(location);
			}
		}
	}

	public void showInterstitial(string location)
	{
		if (_doDisableAds || string.IsNullOrEmpty(location))
		{
			return;
		}
		LOG("SHOW INTERSTITIAL: " + location, base.gameObject);
		foreach (AdManagerInterface adManager in _adManagers)
		{
			if (adManager._pIsInitialised && adManager._pDoesSupportInterstitial)
			{
				adManager.showInterstitial(location);
			}
		}
	}

	public void cacheBanner(string location)
	{
		if (_doDisableAds || string.IsNullOrEmpty(location))
		{
			return;
		}
		foreach (AdManagerInterface adManager in _adManagers)
		{
			if (adManager._pIsInitialised && adManager._pDoesSupportBanner)
			{
				adManager.cacheBanner(location);
			}
		}
	}

	public void showBanner(string location)
	{
		if (_doDisableAds || string.IsNullOrEmpty(location))
		{
			return;
		}
		LOG("SHOW BANNER: " + location, base.gameObject);
		foreach (AdManagerInterface adManager in _adManagers)
		{
			if (adManager._pIsInitialised && adManager._pDoesSupportBanner)
			{
				adManager.showBanner(location);
			}
		}
	}

	public void cacheMoreApps(string location)
	{
		foreach (AdManagerInterface adManager in _adManagers)
		{
			if (adManager._pIsInitialised && adManager._pDoesSupportMoreApps)
			{
				adManager.cacheMoreApps();
			}
		}
	}

	public void showMoreApps(string location)
	{
		LOG("SHOW MORE APPS", base.gameObject);
		foreach (AdManagerInterface adManager in _adManagers)
		{
			if (adManager._pIsInitialised && adManager._pDoesSupportMoreApps)
			{
				adManager.showMoreApps();
			}
		}
	}

	public void cacheVideo(string location)
	{
		if (_doDisableAds || string.IsNullOrEmpty(location))
		{
			return;
		}
		foreach (AdManagerInterface adManager in _adManagers)
		{
			if (adManager._pIsInitialised && adManager._pDoesSupportVideo)
			{
				adManager.cacheVideo(location);
			}
		}
	}

	public void showVideo(string location)
	{
		if (_doDisableAds || string.IsNullOrEmpty(location))
		{
			return;
		}
		LOG("SHOW VIDEO: " + location, base.gameObject);
		foreach (AdManagerInterface adManager in _adManagers)
		{
			if (adManager._pIsInitialised && adManager._pDoesSupportVideo)
			{
				adManager.showVideo(location);
			}
		}
	}

	public void trackEvent(string eventIdentifier)
	{
		if (string.IsNullOrEmpty(eventIdentifier))
		{
			return;
		}
		foreach (AdManagerInterface adManager in _adManagers)
		{
			if (adManager._pIsInitialised && adManager._pDoesSupportTracking)
			{
				adManager.trackEvent(eventIdentifier);
			}
		}
	}

	public void trackEventWithMetadata(string eventIdentifier, Dictionary<string, string> metadata)
	{
		if (string.IsNullOrEmpty(eventIdentifier))
		{
			return;
		}
		foreach (AdManagerInterface adManager in _adManagers)
		{
			if (adManager._pIsInitialised && adManager._pDoesSupportTracking)
			{
				adManager.trackEventWithMetadata(eventIdentifier, metadata);
			}
		}
	}

	public void trackEventWithValue(string eventIdentifier, float value)
	{
		if (string.IsNullOrEmpty(eventIdentifier))
		{
			return;
		}
		foreach (AdManagerInterface adManager in _adManagers)
		{
			if (adManager._pIsInitialised && adManager._pDoesSupportTracking)
			{
				adManager.trackEventWithValue(eventIdentifier, value);
			}
		}
	}

	public void trackEventWithValueAndMetadata(string eventIdentifier, float value, Dictionary<string, object> metadata)
	{
		if (string.IsNullOrEmpty(eventIdentifier))
		{
			return;
		}
		foreach (AdManagerInterface adManager in _adManagers)
		{
			if (adManager._pIsInitialised && adManager._pDoesSupportTracking)
			{
				adManager.trackEventWithValueAndMetadata(eventIdentifier, value, metadata);
			}
		}
	}

	public void ChangeLocation(string previous, ref string current, string linkName, JsonData linkData, JsonData currentLocationData)
	{
		LOG("AD FACADE: Checking Ads for location [" + current + "]", base.gameObject);
		checkForKey(currentLocationData, "CacheBannerLocation", cacheBanner);
		checkForKey(currentLocationData, "ShowBannerLocation", showBanner);
		checkForKey(currentLocationData, "CacheInterstitialLocation", cacheInterstitial);
		checkForKey(currentLocationData, "ShowInterstitialLocation", showInterstitial);
		checkForKey(currentLocationData, "CacheVideoLocation", cacheVideo);
		checkForKey(currentLocationData, "ShowVideoLocation", showVideo);
		checkForKey(currentLocationData, "CacheMoreAppsLocation", cacheMoreApps);
		checkForKey(currentLocationData, "ShowMoreAppsLocation", showMoreApps);
	}

	public void permanentlyDisableAdsOnThisDevice()
	{
		_doDisableAds = true;
		ObscuredPrefs.SetBool("AdFacade_DisableAds", true);
	}

	private void checkForKey(JsonData curentLocation, string keyToCheck, Action<string> callbackIfFound)
	{
		JsonData jsonData = curentLocation.TryGet(keyToCheck);
		if (jsonData != null && callbackIfFound != null)
		{
			callbackIfFound((string)jsonData);
		}
	}

	protected virtual void addAdManagers()
	{
		if (_adManagersToUse == null || _adManagersToUse.Length == 0)
		{
			LOG("No ad managers defined in Ad Facade.", base.gameObject);
			return;
		}
		int i = 0;
		for (int num = _adManagersToUse.Length; i < num; i++)
		{
			AdManagerInterface adManagerFromEnum = getAdManagerFromEnum(_adManagersToUse[i]);
			_adManagers.Add(adManagerFromEnum);
		}
	}

	private AdManagerInterface getAdManagerFromEnum(SupportedAdManagers adManager)
	{
		switch (adManager)
		{
		case SupportedAdManagers.CHARTBOOST:
			return new ChartboostAdManager();
		case SupportedAdManagers.ADMOB:
			return new AdMobAdManager();
		case SupportedAdManagers.IAD:
			return new IAdManager();
		default:
			return new DebugAdManager();
		}
	}
}
