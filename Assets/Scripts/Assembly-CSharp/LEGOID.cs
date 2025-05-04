using System;
using System.Text.RegularExpressions;
using GameDefines;
using LitJson;
using UnityEngine;

public class LEGOID : InitialisationObject
{
	public enum ELoginStatus
	{
		LOGIN_NOT_ATTEMPTED = 0,
		SETTING_UP_DATA = 1,
		SET_UP_DATA_FAIL = 2,
		SET_UP_DATA_SUCCESS = 3,
		LOGGING_IN = 4,
		LOGIN_SUCCESS = 5,
		LOGIN_FAIL = 6,
		LOGIN_DISABLED = 7
	}

	public enum EConfigUsed
	{
		NOT_LOADED = 0,
		LOCAL_IN_DATE = 1,
		LOCAL_OUT_OF_DATE = 2,
		REMOTE = 3,
		LEGO_SDK = 4
	}

	public class LegoIDButtonPayload
	{
		public string linkURL;

		public string titleText;

		public Texture2D buttonGraphic;
	}

	public const bool DO_DEBUG = true;

	private const string LEGO_CONFIG_URL_TEMPLATE = "https://lego.com/{basepath}/go/{num}/{version}/{type}";

	private const string LANG_KEY = "{basepath}";

	private const string PLATFORM_KEY = "{type}";

	private const string VERSION_KEY = "{version}";

	private const string GAME_NUM_KEY = "{num}";

	private const string DOMAIN = "http://www.lego.com";

	public const string LEGO_ID_PREFIX = "LEGOID: ";

	private static LEGOID _instance;

	public bool _activeOnAndroid = true;

	public bool _activeOnIOS = true;

	public bool _activeOnWeb = true;

	public bool _debugIsAuthenticated;

	private AmuzoLEGOIDCallbackHandler _handler;

	private ELoginStatus _status;

	private Action<ELoginStatus> _loginComplete;

	private Action<LegoIDButtonPayload> _legoIDButtonLoadComplete;

	private bool _isAuthenticatedOnWeb;

	private bool _loginRequested;

	private string _legoIdConfigEndpointUrl;

	private JsonData _legoIdConfigEndpointData;

	private string _currentUserEndpointURL;

	private string _loadedConfigXMLAsString;

	private EConfigUsed _configUsed;

	private JsonData _gameConfig;

	private MonoXmlElement _root;

	private JsonData _dummyJson;

	private MonoXmlElement _dummyXml;

	private string _dummyStr;

	private DownloadRequest _configurationRequest;

	private Action _onConfigureComplete;

	public static LEGOID _pInstance
	{
		get
		{
			return _instance;
		}
	}

	public bool _pCanSignIn
	{
		get
		{
			switch (_status)
			{
			case ELoginStatus.SETTING_UP_DATA:
			case ELoginStatus.SET_UP_DATA_FAIL:
			case ELoginStatus.LOGGING_IN:
			case ELoginStatus.LOGIN_SUCCESS:
			case ELoginStatus.LOGIN_DISABLED:
				return false;
			default:
				return true;
			}
		}
	}

	public bool _pSetUp
	{
		get
		{
			return LEGOSDKFacade.Instance.IsSetUp;
		}
	}

	public string _pLogInName
	{
		get
		{
			return _handler._pLogInName;
		}
	}

	public bool _pAuthenticated
	{
		get
		{
			_debugIsAuthenticated = _isAuthenticatedOnWeb;
			_debugIsAuthenticated = _handler._pAuthenticated;
			return _handler._pAuthenticated;
		}
	}

	public ELoginStatus _pLoginStatus
	{
		get
		{
			return _status;
		}
	}

	public AmuzoLEGOIDCallbackHandler _pHandler
	{
		get
		{
			return _handler;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		_instance = this;
		_handler = GetComponent<AmuzoLEGOIDCallbackHandler>();
		if (_handler != null)
		{
			_handler._legoIdLogInUpdate = LEGOIDLogInUpdate;
			_handler._legoIdLogInDidNotComplete = LEGOIDLogInFailed;
		}
		_status = ELoginStatus.SETTING_UP_DATA;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		AddDebugMenu();
	}

	private void Update()
	{
		if (_loginRequested)
		{
			Login();
		}
	}

	private void OnApplicationFocus(bool isPaused)
	{
		if (!isPaused && _status == ELoginStatus.LOGGING_IN)
		{
			OnModalClose();
		}
	}

	public override void startInitialising()
	{
		base.startInitialising();
		if (_handler == null)
		{
			Debug.Log("LEGOID: ERROR, no handler found, LEGOID will not work.");
			_currentState = InitialisationState.FINISHED;
			return;
		}
		string text = "https://lego.com/{basepath}/go/{num}/{version}/{type}";
		text = text.Replace("{type}", GetPlatform());
		text = text.Replace("{basepath}", GetCorrectLanguageCode());
		text = text.Replace("{version}", _handler._version);
		text = text.Replace("{num}", _handler._gameNum);
		_handler._pConfigEndpointUrl = text;
		if (GlobalDefines._pUseLegoSDK && LEGOSDKAmuzo._pInstance != null && LEGOSDKAmuzo._pInstance._pHasData)
		{
			_currentState = InitialisationState.FINISHED;
			_status = ELoginStatus.SET_UP_DATA_SUCCESS;
			ConfigureFromLegoSdk();
		}
		else
		{
			_currentState = InitialisationState.INITIALISING;
			Configure();
		}
	}

	public void Login(Action<ELoginStatus> onLoginComplete)
	{
		Debug.Log("LEGOID: registering Login Request.");
		_loginRequested = true;
		_loginComplete = onLoginComplete;
	}

	private void Login()
	{
		Debug.Log("LEGOID: attempting to Login now, status at start of procedure is: ." + _status);
		_loginRequested = false;
		if (!_pCanSignIn)
		{
			LoginComplete();
		}
		else if ((!_activeOnAndroid && Application.platform == RuntimePlatform.Android) || (!_activeOnIOS && Application.platform == RuntimePlatform.IPhonePlayer) || (!_activeOnWeb && (Application.platform == RuntimePlatform.WebGLPlayer || Application.platform == RuntimePlatform.WebGLPlayer)))
		{
			_status = ELoginStatus.LOGIN_DISABLED;
			LoginComplete();
		}
		else if (!_pAuthenticated)
		{
			if (!LEGOSDKFacade.Instance.IsSetUp)
			{
				Debug.Log("LEGOID: LEGOSDKFacade is not set up! Cannot sign in.");
				LoginComplete();
			}
			else
			{
				_status = ELoginStatus.LOGGING_IN;
				LEGOSDKFacade.Instance.LogIn();
			}
		}
		else
		{
			_status = ELoginStatus.LOGIN_SUCCESS;
			LoginComplete();
		}
	}

	private void Configure()
	{
		if (!(ExternalMessageHandler.Instance == null))
		{
			ConfigureLEGOID(OnConfigureSuccess, OnConfigureError);
		}
	}

	private void ConfigureFromLegoSdk()
	{
		_configUsed = EConfigUsed.LEGO_SDK;
		_handler._pExperience = LEGOSDKAmuzo._pInstance._pExperiance;
		Debug.Log("LEGOID: Set experience id: " + _handler._pExperience);
		_legoIdConfigEndpointUrl = LEGOSDKAmuzo._pInstance.GetStringFromConfigJson("configuration/commonconfigurations/legoid/endpoints/legoidconfiguration");
		Debug.Log("LEGOID: Setting endpoint url: " + _legoIdConfigEndpointUrl);
		_handler.SetExperienceAndEndpoint(_legoIdConfigEndpointUrl);
		if (_onConfigureComplete != null)
		{
			_onConfigureComplete();
		}
	}

	private void OnConfigureSuccess()
	{
		if (_handler == null || _handler._pConfigEndpointUrl == null || _handler._pExperience == null)
		{
			Debug.LogError("LEGOID: Configuration Failed");
			_status = ELoginStatus.SET_UP_DATA_FAIL;
		}
		else
		{
			Debug.Log("LEGOID: Got config");
		}
	}

	private void OnConfigureError(DownloadRequest request)
	{
		OnConfigureError(request._pErrorString);
	}

	private void OnConfigureError(string error)
	{
		if (_configurationRequest != null && (_configurationRequest._pDownloadLocation == DownloadLocation.REMOTE || (_configurationRequest._pDownloadLocation != DownloadLocation.REMOTE && _configurationRequest.doUseCacheTimeout && !_configurationRequest._pIsCacheInDate)))
		{
			Debug.Log("LEGOID: Using out-of-date local config!");
			_configurationRequest.doRemoteDownload = false;
			_configurationRequest.doLocalDownload = true;
			_configurationRequest.doCacheDownload = false;
			_configurationRequest.doCacheHeaderInfo = false;
			_configurationRequest.doLocalHeaderInfo = false;
			_configurationRequest.doUseCacheTimeout = false;
			_configurationRequest.ClearDownload();
			_configurationRequest.download();
			return;
		}
		Debug.Log("LEGOID: Using bundled config!");
		_configUsed = EConfigUsed.LOCAL_OUT_OF_DATE;
		string text = string.Empty;
		try
		{
			text = Resources.Load<TextAsset>("https_lego_com_en-gb_go_23_1_android").text;
		}
		catch (Exception ex)
		{
			Debug.Log("LEGOID: Unable to load emergency Resources LEGOID config! Error [" + ex.Message + "]");
		}
		if (!string.IsNullOrEmpty(text))
		{
			OnGameConfigurationLoad(text);
			return;
		}
		Debug.LogError("LEGOID: Launch Error : " + error);
		_status = ELoginStatus.SET_UP_DATA_FAIL;
		LoginComplete();
		_currentState = InitialisationState.FINISHED;
	}

	private void OnLEGOIDConfigJSONLoaded(string text, params object[] args)
	{
		if (string.IsNullOrEmpty(text))
		{
			Debug.Log("LEGOID: Game config JSON was NULL!");
			return;
		}
		_legoIdConfigEndpointData = Extensions.LoadJson(text);
		if (_legoIdConfigEndpointData == null)
		{
			Debug.Log("LEGOID: Unable to load game config JSON as JSON!");
			return;
		}
		JsonData jsonData = _legoIdConfigEndpointData["urls/legoid/currentUser"];
		if (jsonData == null)
		{
			Debug.Log("LEGOID: Unable to find node [urls/legoid/currentUser] in game config JSON!");
			return;
		}
		_currentUserEndpointURL = (string)jsonData;
		if (string.IsNullOrEmpty(_currentUserEndpointURL))
		{
			Debug.Log("LEGOID: Config node [urls/legoid/currentUser] was null or empty!");
			return;
		}
		TextLoaderBehaviour component = GetComponent<TextLoaderBehaviour>();
		component.LoadText(_currentUserEndpointURL, OnCurrentUserJSONLoaded);
	}

	private void OnCurrentUserJSONLoaded(string text, params object[] args)
	{
		_currentState = InitialisationState.FINISHED;
	}

	private bool OnKillSwitchDataLoaded(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return true;
		}
		int num = int.Parse(text);
		if (num > 0)
		{
			Debug.Log("LEGOID: KillSwitch Flag RAISED, running KillswitchProcedure now");
			GlobalDefines._killSwitchRaised = true;
		}
		else
		{
			Debug.Log("LEGOID: KillSwitch Flag low, no need to do anything.");
		}
		return !GlobalDefines._killSwitchRaised;
	}

	private bool OnMinVersionDataLoaded(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return true;
		}
		if (GlobalDefines._killSwitchRaised)
		{
			return true;
		}
		string[] array = text.Split('.');
		string[] array2 = AnvilBuildInfo._pBundleVersion.Split('.');
		int num = 0;
		int num2 = 0;
		Debug.Log("LEGOID: Comparing version in build to version in config data - Build: " + AnvilBuildInfo._pBundleVersion + " - Config: " + text);
		bool flag = false;
		for (int i = 0; i < array.Length && i < array2.Length; i++)
		{
			num = int.Parse(array[i]);
			num2 = int.Parse(array2[i]);
			if (num2 < num)
			{
				flag = true;
				break;
			}
			if (num2 > num)
			{
				break;
			}
		}
		if (flag)
		{
			Debug.Log("LEGOID: Build Version found to be old, running update procedure.");
			GlobalDefines._minVersionWrong = true;
		}
		else
		{
			Debug.Log("LEGOID: Build Version found to be up to date.");
		}
		return !flag;
	}

	public void OnPostResponse(string error, string text)
	{
		Debug.Log("LEGOID: Forceometer POST complete: txt[" + text + "] err[" + error + "]");
	}

	private void ConfigureLEGOID(Action onConfigured, Action<string> onError)
	{
		if (_handler == null)
		{
			onError("No handler found");
			return;
		}
		AmuzoLEGOIDCallbackHandler handler = _handler;
		handler._onSetupEndpointsComplete = (Action<bool>)Delegate.Combine(handler._onSetupEndpointsComplete, new Action<bool>(OnSetupEndpointComplete));
		_onConfigureComplete = onConfigured;
		_handler._pExperience = null;
		TextLoaderBehaviour component = GetComponent<TextLoaderBehaviour>();
		if (component == null)
		{
			onError("No text loader");
			return;
		}
		_gameConfig = JsonLoader.data("LEGOID_Config");
		if (_gameConfig == null)
		{
			onError("no config loaded");
			return;
		}
		Debug.Log("LEGOID: Endpoint used [" + _handler._pConfigEndpointUrl + "]");
		_configurationRequest = new DownloadRequest();
		_configurationRequest.requestUrl = _handler._pConfigEndpointUrl;
		_configurationRequest.onSuccessCallBack += OnGameConfigurationLoad;
		_configurationRequest.onFailCallback += OnConfigureError;
		_configurationRequest._requestTimeout = 3f;
		_configurationRequest.doDebug = true;
		_configurationRequest.doVerboseDebug = true;
		_configurationRequest.download();
	}

	private string GetPlatform()
	{
		return "android";
	}

	private string GetCorrectLanguageCode()
	{
		switch (LocalisationFacade.Instance.language)
		{
		case "CS":
			return "cs-cz";
		case "DA":
			return "da-dk";
		case "DE":
			return "de-de";
		case "ES":
			return "es-es";
		case "ES_MX":
			return "es-ar";
		case "FI":
			return "fi-fi";
		case "FR":
			return "fr-fr";
		case "HU":
			return "hu-hu";
		case "IT":
			return "it-it";
		case "JA":
		case "JP":
			return "ja-jp";
		case "KO":
			return "ko-kr";
		case "NL":
			return "nl-be";
		case "NO":
		case "NB":
			return "nb-no";
		case "PL":
			return "pl-pl";
		case "RU":
			return "ru-ru";
		case "SV":
			return "sv-se";
		case "ZH":
		case "ZHSI":
			return "zh-cn";
		case "EN":
		case "EN_UK":
			return "en-gb";
		default:
			return "en-us";
		}
	}

	private void OnGameConfigurationLoad(DownloadRequest request)
	{
		if (request == null || request._pCurrentStatus != DownloadStatus.SUCCEEDED)
		{
			OnConfigureError("Configuration request did not succeed!");
			return;
		}
		if (!request.doRemoteDownload)
		{
			_configUsed = EConfigUsed.LOCAL_OUT_OF_DATE;
		}
		else if (request._pDownloadLocation == DownloadLocation.REMOTE)
		{
			_configUsed = EConfigUsed.REMOTE;
		}
		else
		{
			_configUsed = EConfigUsed.LOCAL_IN_DATE;
		}
		OnGameConfigurationLoad(request._pResponseString);
	}

	private void OnGameConfigurationLoad(string loadedConfigXMLAsString)
	{
		if (string.IsNullOrEmpty(loadedConfigXMLAsString))
		{
			OnConfigureError("Configuration text returned was null or empty!");
			return;
		}
		if (loadedConfigXMLAsString.Contains("<html "))
		{
			OnConfigureError("Unable to parse configuration text! Looks like HTML!");
			return;
		}
		Debug.Log("LEGOID: CONFIGURE TEXT: " + loadedConfigXMLAsString);
		_root = Xml.Load(loadedConfigXMLAsString);
		if (_root == null)
		{
			OnConfigureError("Unable to parse configuration text into xml!");
			return;
		}
		_handler._pExperience = GetStringFromConfigXML("urls/legoid/experience");
		Debug.Log("LEGOID: Set experience id: " + _handler._pExperience);
		_legoIdConfigEndpointUrl = GetStringFromConfigXML("urls/legoid/LEGOIDConfig");
		Debug.Log("LEGOID: Setting endpoint url: " + _legoIdConfigEndpointUrl);
		_handler.SetExperienceAndEndpoint(_legoIdConfigEndpointUrl);
		if (!OnMinVersionDataLoaded(GetStringFromConfigXML("urls/legoid/minimumVersion")))
		{
			GlobalDefines._pExitAppMessage = GetStringFromConfigXML("urls/legoid/minimumVersionMessage");
		}
		if (!OnKillSwitchDataLoaded(GetStringFromConfigXML("urls/legoid/killswitch")))
		{
			GlobalDefines._pExitAppMessage = GetStringFromConfigXML("urls/legoid/killswitchMessage");
		}
		string stringFromConfigXML = GetStringFromConfigXML("urls/legoid/privacypolicy");
		if (!string.IsNullOrEmpty(stringFromConfigXML))
		{
			Debug.Log("LEGOID: Setting the privacy url: " + stringFromConfigXML);
			GlobalDefines._pUrlPrivacy = stringFromConfigXML;
		}
		string stringFromConfigXML2 = GetStringFromConfigXML("urls/legoid/cookiepolicy");
		if (!string.IsNullOrEmpty(stringFromConfigXML2))
		{
			Debug.Log("LEGOID: Setting the coockies url: " + stringFromConfigXML2);
			GlobalDefines._pUrlCookies = stringFromConfigXML2;
		}
		_dummyJson = null;
		_dummyXml = null;
		if (_onConfigureComplete != null)
		{
			_onConfigureComplete();
		}
	}

	public string GetStringFromConfigXML(string path)
	{
		if (_gameConfig == null || _root == null)
		{
			Debug.Log("LEGOID: Unable to get config data as either JSON or XML was NULL!");
			return string.Empty;
		}
		_dummyJson = _gameConfig.JsonAtPath(path);
		if (_dummyJson == null || !_dummyJson.IsString)
		{
			Debug.Log("LEGOID: Unable to find node in JSON [" + path + "]");
			return string.Empty;
		}
		_dummyXml = _root.GetChildAt((string)_dummyJson);
		if (_dummyXml == null)
		{
			Debug.Log("LEGOID: Unable to find node in XML [" + (string)_dummyJson + "] from reference in JSON [" + path + "]");
			return string.Empty;
		}
		_dummyStr = _dummyXml._pText;
		if (!string.IsNullOrEmpty(_dummyStr))
		{
			_dummyStr = Regex.Replace(_dummyStr, "\\s+", string.Empty);
		}
		Debug.Log("LEGOID: Found config data at path [" + path + "]. Value [" + _dummyStr + "]");
		return _dummyStr;
	}

	public void CancelLogin()
	{
		Debug.LogWarning("--- No impl for CloseNativeWindow in new LEGOID SDK");
		LEGOIDLogInUpdate();
	}

	public void LogOut()
	{
		LEGOSDKFacade.Instance.LogOutUser();
	}

	public void LoadButton(Action<LegoIDButtonPayload> onButtonLoadComplete)
	{
	}

	private void OnLEGOIDLoginSuccess()
	{
		Debug.Log("OnLEGOIDLoginSuccess");
		_isAuthenticatedOnWeb = true;
		ExternalMessageHandler.ExternalCall("LEGOIDGetProfile");
		_status = ELoginStatus.LOGIN_SUCCESS;
		LoginComplete();
	}

	private void OnLEGOIDLogout()
	{
		Debug.Log("OnLEGOIDLogout");
		_status = ELoginStatus.SET_UP_DATA_SUCCESS;
	}

	private void OnAuthenticationChecked(string arg)
	{
		if (arg.ToLower() == "true")
		{
			_isAuthenticatedOnWeb = true;
			Debug.Log("OnAuthenticationChecked - Authenticated - " + arg);
			_status = ELoginStatus.SET_UP_DATA_SUCCESS;
		}
		else
		{
			_isAuthenticatedOnWeb = false;
			Debug.Log("OnAuthenticationChecked - Not authenticated - " + arg);
			_status = ELoginStatus.SET_UP_DATA_FAIL;
		}
		_currentState = InitialisationState.FINISHED;
	}

	private void OnGetUserName(string userName)
	{
		Debug.Log("LEGOID: Got LEGOID username [" + userName + "]");
		if (!string.IsNullOrEmpty(userName))
		{
			_handler._pLogInName = userName;
		}
	}

	private void OnModalClose()
	{
		if (_status != ELoginStatus.LOGIN_SUCCESS)
		{
			_status = ELoginStatus.LOGIN_FAIL;
		}
		LoginComplete();
	}

	private void OnLEGOIDLoadButton(string payload)
	{
		string[] array = payload.Split('*');
		Debug.Log("OnLEGOIDLoadButton()");
		string linkURL = array[0];
		string titleText = array[1];
		string text = array[2];
		string[] array2 = text.Split('"');
		string[] array3 = array2[1].Split(',');
		byte[] data = Convert.FromBase64String(array3[1]);
		Texture2D texture2D = new Texture2D(4, 4);
		texture2D.LoadImage(data);
		texture2D.Apply();
		if (_legoIDButtonLoadComplete != null)
		{
			LegoIDButtonPayload legoIDButtonPayload = new LegoIDButtonPayload();
			legoIDButtonPayload.buttonGraphic = texture2D;
			legoIDButtonPayload.linkURL = linkURL;
			legoIDButtonPayload.titleText = titleText;
			_legoIDButtonLoadComplete(legoIDButtonPayload);
			_legoIDButtonLoadComplete = null;
		}
	}

	private void OnSetupEndpointComplete(bool isReady)
	{
		_status = ((!isReady) ? ELoginStatus.SET_UP_DATA_FAIL : ELoginStatus.SET_UP_DATA_SUCCESS);
		_currentState = InitialisationState.FINISHED;
	}

	private void LEGOIDLogInUpdate()
	{
		_status = ((!_pAuthenticated) ? ELoginStatus.LOGIN_FAIL : ELoginStatus.LOGIN_SUCCESS);
		LoginComplete();
	}

	private void LEGOIDLogInFailed(string errorMessgae)
	{
		Debug.Log("Error logging in. Message [" + errorMessgae + "]");
		_status = ((!_pAuthenticated) ? ELoginStatus.LOGIN_FAIL : ELoginStatus.LOGIN_SUCCESS);
		LoginComplete();
	}

	private void LoginComplete()
	{
		if (_loginComplete != null)
		{
			_loginComplete(_status);
		}
	}

	private void AddDebugMenu()
	{
		AmuzoDebugMenu amuzoDebugMenu = new AmuzoDebugMenu("OLD LEGO ID");
		Func<string> textAreaFunction = delegate
		{
			string empty = string.Empty;
			string text = empty;
			empty = string.Concat(text, "Current status: ", _status, AmuzoDebugMenu.NEW_LINE);
			text = empty;
			empty = text + "Set up? " + _pSetUp + AmuzoDebugMenu.NEW_LINE;
			empty = empty + "Platform: " + GetPlatform() + AmuzoDebugMenu.NEW_LINE;
			empty = empty + "Version: " + _handler._version + AmuzoDebugMenu.NEW_LINE;
			empty = empty + "Game Id: " + _handler._gameNum + AmuzoDebugMenu.NEW_LINE;
			empty = empty + "Config URL Used: " + _handler._pConfigEndpointUrl + AmuzoDebugMenu.NEW_LINE;
			text = empty;
			empty = text + "Can sign in? " + _pCanSignIn + AmuzoDebugMenu.NEW_LINE;
			text = empty;
			empty = text + "Authenticated? " + _pAuthenticated + AmuzoDebugMenu.NEW_LINE;
			empty = empty + "LEGO ID Username: " + _pLogInName + AmuzoDebugMenu.NEW_LINE;
			text = empty;
			empty = string.Concat(text, "Config used: ", _configUsed, AmuzoDebugMenu.NEW_LINE);
			text = empty;
			return text + "Loaded config XML: " + AmuzoDebugMenu.NEW_LINE + _loadedConfigXMLAsString + AmuzoDebugMenu.NEW_LINE;
		};
		amuzoDebugMenu.AddInfoTextFunction(textAreaFunction);
		AmuzoDebugMenuManager.RegisterRootDebugMenu(amuzoDebugMenu);
	}
}
