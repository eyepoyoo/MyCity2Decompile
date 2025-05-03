using System.Collections.Generic;
using GameDefines;
using LEGO;
using LEGO.CoreSDK;
using LEGO.CoreSDK.VPC;
using LitJson;
using UnityEngine;

public class LEGOSDKAmuzo : InitialisationObject
{
	public const string JSONADDRESS_MINVERSION_TEXT = "configuration/commonconfigurations/appdialogue/customvalues/minimumversiontext";

	public const string JSONADDRESS_KILLSWITCH_TEXT = "configuration/commonconfigurations/appdialogue/customvalues/killswitchtext";

	public const string JSONADDRESS_URL_COOKIE = "configuration/commonconfigurations/privacypolicy/endpoints/cookiepolicy";

	public const string JSONADDRESS_URL_PRIVACY = "configuration/commonconfigurations/privacypolicy/endpoints/privacypolicy";

	public const string JSONADDRESS_LEGOID_ENDPOINT = "configuration/commonconfigurations/legoid/endpoints/legoidconfiguration";

	private const string GAME_NUM_KEY = "{num}";

	private const string PLATFORM_KEY = "{type}";

	private const string VERSION_KEY = "{version}";

	private const string CONFIG_URL_TEMPLATE = "https://lego.com/go/{num}/{version}/{type}?format=json";

	private const string REGION_FALLBACK = "en_GB";

	private const string PLATFORM_IOS = "ios";

	private const string PLATFORM_ANDROID = "android";

	private const string PLATFORM_WEB = "web";

	private const string LOG_PREFIX_LEGOSDK = "LEGOSDK - ";

	private readonly string[] REGIONS_ACCEPTED = new string[19]
	{
		"cs_CZ", "da_DK", "de_DE", "es_ES", "es_AR", "fi_FI", "fr_FR", "hu_HU", "it_IT", "ja_JP",
		"ko_KR", "nl_BE", "nb_NO", "pl_PL", "ru_RU", "sv_SE", "zh_CN", "en_GB", "en_US"
	};

	private static LEGOSDKAmuzo _instance;

	public string _gameNum;

	public string _version;

	private bool _hasRecivedData;

	private string _configUrl;

	private string _gateResult;

	private string _dummyString = GlobalDefines.EMPTY_STRING;

	private JsonData _rawJsonData;

	private JsonData _dummyJson;

	public static LEGOSDKAmuzo _pInstance
	{
		get
		{
			return _instance;
		}
	}

	public bool _pHasData
	{
		get
		{
			return _hasRecivedData;
		}
	}

	public string _pExperiance
	{
		get
		{
			if (!_pHasData)
			{
				return null;
			}
			if (LEGOSDK.ApplicationConfigurationService.ApplicationConfiguration == null)
			{
				return null;
			}
			return LEGOSDK.ApplicationConfigurationService.ApplicationConfiguration.Experience;
		}
	}

	public static void OnPoliciesShown()
	{
		LEGOSDK.Log.Debug("LEGOSDK - Policies Shown");
	}

	public void Start()
	{
		_instance = this;
		LEGOSDK.Log.LogLevel = LogLevel.Verbose;
		_configUrl = "https://lego.com/go/{num}/{version}/{type}?format=json";
		_configUrl = _configUrl.Replace("{num}", _gameNum);
		_configUrl = _configUrl.Replace("{type}", GetPlatform());
		_configUrl = _configUrl.Replace("{version}", _version);
		LEGOSDK.Log.Debug("LEGOSDK - URL used [" + _configUrl + "]");
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public override void startInitialising()
	{
		LEGOSDK.Log.Debug("LEGOSDK - Starting initialisation");
		base.startInitialising();
		if (!GlobalDefines._isMobile || Application.isEditor)
		{
			LEGOSDK.Log.Debug("LEGOSDK - Web Platform OR Editor Environment: LEGOSDK will not be used. ");
			_currentState = InitialisationState.FINISHED;
			return;
		}
		LEGOSDK.Log.Debug("LEGOSDK - Mobil Platform: LEGOSDK will be used. ");
		_currentState = InitialisationState.INITIALISING;
		InisialiseOnline();
		SetUpConfig(LEGOSDK.ApplicationConfigurationService.ApplicationConfiguration);
		_currentState = InitialisationState.FINISHED;
	}

	public override InitialisationState updateInitialisation()
	{
		_currentState = InitialisationState.FINISHED;
		return _currentState;
	}

	public string GetEndpointFromApplicationConfig(string endPointKey)
	{
		string empty = string.Empty;
		LEGOSDK.Log.Debug("LEGOSDK - Endpoint requested by config key: " + endPointKey);
		if (!_pHasData || LEGOSDK.ApplicationConfigurationService.ApplicationConfiguration == null)
		{
			LEGOSDK.Log.Debug("LEGOSDK - LEGO SDK has no data yet, no endpont found");
			return empty;
		}
		if (LEGOSDK.ApplicationConfigurationService.ApplicationConfiguration.Endpoints == null || LEGOSDK.ApplicationConfigurationService.ApplicationConfiguration.Endpoints.Count <= 0 || !LEGOSDK.ApplicationConfigurationService.ApplicationConfiguration.Endpoints.ContainsKey(endPointKey))
		{
			LEGOSDK.Log.Debug("LEGOSDK - Endpoint Key not found in Gathered enpoints");
			return empty;
		}
		empty = LEGOSDK.ApplicationConfigurationService.ApplicationConfiguration.Endpoints[endPointKey];
		LEGOSDK.Log.Debug("LEGOSDK - Endpoint found, Value:" + empty);
		return empty;
	}

	public string GetStringFromConfigJson(string path)
	{
		if (!_pHasData)
		{
			return GlobalDefines.EMPTY_STRING;
		}
		if (_rawJsonData == null)
		{
			return GlobalDefines.EMPTY_STRING;
		}
		_dummyJson = null;
		_dummyJson = _rawJsonData.JsonAtPath(path);
		if (_dummyJson == null || !_dummyJson.IsString)
		{
			LEGOSDK.Log.Debug("LEGOSDK - GetStringFromConfigJson - path passed is not found in json config, Path: " + path);
			return GlobalDefines.EMPTY_STRING;
		}
		LEGOSDK.Log.Debug("LEGOSDK - GetStringFromConfigJson - Found config data at path [" + path + "]. Value [" + (string)_dummyJson + "]");
		return (string)_dummyJson;
	}

	private void OnLegoSdkAppConfigChanged(IApplicationConfiguration newConfig)
	{
		if (_currentState != InitialisationState.FINISHED)
		{
			_currentState = InitialisationState.FINISHED;
		}
		if (newConfig == null)
		{
			LEGOSDK.Log.Error("LEGOSDK - LEGO ID App Config is NULL");
			return;
		}
		LEGOSDK.Log.Debug(string.Concat("LEGOSDK - LEGO ID App Config Changed. Name [", newConfig.Name, "]. Source [", LEGOSDK.ApplicationConfigurationService.Source, "]"));
		LEGOSDK.Log.Debug("LEGOSDK - Config Content: " + newConfig.RawJson.ToString());
		SetUpConfig(newConfig);
	}

	private void InisialiseOnline()
	{
		LEGOSDK.Log.Debug("LEGOSDK - Initialising ONLINE, Internet found = " + InternetCheckFacade._pIsOnline);
		LEGOSDK.Initialize(delegate(CoreConfiguration config)
		{
			config.ApplicationConfigurationURL = _configUrl;
			config.SupportedLocales = REGIONS_ACCEPTED;
			config.FallbackLocale = "en_GB";
		});
		LEGOSDK.Log.Debug("LEGOSDK - Starting LEGOSDK, setting config change callback.");
		LEGOSDK.ApplicationConfigurationService.ApplicationConfigurationChanged += OnLegoSdkAppConfigChanged;
	}

	private void InisialiseOffline()
	{
		LEGOSDK.Log.Debug("LEGOSDK - Initialising OFFLINE");
		LEGOSDK.InitializeOffline();
	}

	private void SetUpConfig(IApplicationConfiguration newConfig)
	{
		if (newConfig != null && newConfig.Endpoints != null)
		{
			foreach (KeyValuePair<string, string> endpoint in newConfig.Endpoints)
			{
				LEGOSDK.Log.Debug("LEGOSDK - Enpoint found: KEY = " + endpoint.Key + ", VALUE = " + endpoint.Value);
			}
		}
		else
		{
			LEGOSDK.Log.Debug("LEGOSDK - No Endpoints found in config....");
		}
		_rawJsonData = Extensions.LoadJson(newConfig.RawJson.ToString());
		GlobalDefines._pExitAppMessage = GlobalDefines.EMPTY_STRING;
		CheckAndReportMinVersion(newConfig.MinimumVersion.ToString());
		CheckAndReportKillSwitch(newConfig.KillSwitch);
		_hasRecivedData = true;
		_dummyString = GlobalDefines.EMPTY_STRING;
	}

	private void CheckAndReportKillSwitch(bool killswitch)
	{
		GlobalDefines._killSwitchRaised = killswitch;
		if (GlobalDefines._killSwitchRaised)
		{
			GlobalDefines._pExitAppMessage = GetStringFromConfigJson("configuration/commonconfigurations/appdialogue/customvalues/killswitchtext");
		}
		LEGOSDK.Log.Debug("LEGOSDK - KillSwitch raised: " + GlobalDefines._killSwitchRaised);
	}

	private void CheckAndReportMinVersion(string minVersion)
	{
		string[] array = minVersion.Split('.');
		string[] array2 = AnvilBuildInfo._pBundleVersion.Split('.');
		int num = 0;
		int num2 = 0;
		bool flag = false;
		LEGOSDK.Log.Debug("LEGOSDK - Comparing version in build to version in config data - Build: " + AnvilBuildInfo._pBundleVersion + " - Config: " + minVersion.ToString());
		for (int i = 0; i < array.Length && i < array2.Length; i++)
		{
			num = int.Parse(array[i]);
			num2 = int.Parse(array2[i]);
			if (num2 < num)
			{
				flag = true;
			}
			else if (num2 > num)
			{
				break;
			}
		}
		if (flag)
		{
			GlobalDefines._minVersionWrong = true;
			GlobalDefines._pExitAppMessage = GetStringFromConfigJson("configuration/commonconfigurations/appdialogue/customvalues/minimumversiontext");
			LEGOSDK.Log.Debug("LEGOSDK - Verson is OLD and so INVALID");
		}
		else
		{
			GlobalDefines._minVersionWrong = false;
			LEGOSDK.Log.Debug("LEGOSDK - Version VALID");
		}
	}

	private void OnParentalGateAnswered(Result result)
	{
		_gateResult = result.ToString();
		LEGOSDK.Log.Debug(_gateResult);
	}

	private string GetPlatform()
	{
		return "android";
	}
}
