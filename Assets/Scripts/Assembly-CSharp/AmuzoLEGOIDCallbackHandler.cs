using System;
using System.Text;
using UnityEngine;

public class AmuzoLEGOIDCallbackHandler : LEGOIDCallbackHandler
{
	private static AmuzoLEGOIDCallbackHandler _instance;

	public static Action _onUserNameUpdated;

	public static Action<string> _onGetRequestSuccess;

	public static Action<string> _onGetRequestFail;

	public static Action<string> _onPostRequestSuccess;

	public static Action<string> _onPostRequestFail;

	public static Action<string> _onGenericPostRequestSuccess;

	public static Action<string> _onGenericPostRequestFail;

	public string _version = "1";

	public string _gameNum = "20";

	public ScreenDebug _debugger;

	public Action _legoIdLogInUpdate;

	public Action<bool> _onSetupEndpointsComplete;

	public Action<string> _legoIdLogInDidNotComplete;

	private string _experience = string.Empty;

	private string _legoIdUsername = string.Empty;

	private string _configEndpointURL = string.Empty;

	private JsonObject _logInInformation;

	public static AmuzoLEGOIDCallbackHandler _pInstance
	{
		get
		{
			return _instance;
		}
	}

	public string _pLogInName
	{
		get
		{
			return _legoIdUsername;
		}
		set
		{
			_legoIdUsername = value;
			if (_onUserNameUpdated != null)
			{
				_onUserNameUpdated();
			}
		}
	}

	public string _pConfigEndpointUrl
	{
		get
		{
			return _configEndpointURL;
		}
		set
		{
			_configEndpointURL = value;
		}
	}

	public string _pExperience
	{
		get
		{
			return _experience;
		}
		set
		{
			_experience = value;
		}
	}

	public bool _pAuthenticated
	{
		get
		{
			if (_logInInformation != null)
			{
				bool val;
				((JsonValue)_logInInformation["Data"]["Authenticated"]).GetAsBool(out val);
				return val;
			}
			return false;
		}
	}

	private void Awake()
	{
		_instance = this;
	}

	public void InitCallbacks()
	{
		LEGOSDKFacade.Instance.SetUpCallbackParameters(base.gameObject.name, methodName);
	}

	public void SetExperienceAndEndpoint(string endpointURL)
	{
		LEGOSDKFacade.Instance.SetUpConfigurationURLAndExperience(_experience, endpointURL);
	}

	public override void LEGOIDLogInUpdateImplementation(string json)
	{
		Debug.LogWarning("---Cannot call CloseNativeWindow() as there isn't an impl. in LEGOSDKFacade---");
		Debug.Log(json);
		_logInInformation = JSonParser.Parse(json);
		_pLogInName = ((JsonValue)_logInInformation["Data"]["UserName"]).GetRaw();
		if (_debugger != null)
		{
			_debugger.Log(_pLogInName);
			_debugger.Log("Raw Login JSON: " + json);
		}
		if (_legoIdLogInUpdate != null)
		{
			_legoIdLogInUpdate();
		}
	}

	public override void LEGOIDLoginFailedImplementation(string error)
	{
		if (_debugger != null)
		{
			_debugger.Log(error);
		}
		if (_legoIdLogInDidNotComplete != null)
		{
			_legoIdLogInDidNotComplete(error);
		}
	}

	public override void LEGOIDLogInDismissedImplementation()
	{
		if (_debugger != null)
		{
			_debugger.Log("Log in dismissed");
		}
		if (_legoIdLogInDidNotComplete != null)
		{
			_legoIdLogInDidNotComplete("Log In Dismissed");
		}
	}

	public override void LEGOIDRegisterCompleteImplementation()
	{
		if (_debugger != null)
		{
			_debugger.Log("Signup complete");
		}
		Debug.LogWarning("---Cannot call CloseNativeWindow() as there isn't an impl. in LEGOSDKFacade---");
	}

	public override void LEGOIDRegisterFailedImplementation(string error)
	{
		if (_debugger != null)
		{
			_debugger.Log(error);
		}
	}

	public override void LEGOIDRegisterDismissedImplementation()
	{
		if (_debugger != null)
		{
			_debugger.Log("Register dismissed");
		}
	}

	public override void LEGOIDSetupEndpointCompleteImplementation()
	{
		if (_debugger != null)
		{
			_debugger.Log("Endpoint set up");
		}
		if (_onSetupEndpointsComplete != null)
		{
			_onSetupEndpointsComplete(true);
		}
	}

	public override void LEGOIDSetupEndpointFailedImplementation(string error)
	{
		if (_debugger != null)
		{
			_debugger.Log("Endpoint set up failed with error message: " + error);
		}
		if (_onSetupEndpointsComplete != null)
		{
			_onSetupEndpointsComplete(false);
		}
	}

	public override void LEGOIDSetupEndpointCanceledImplementation(string status)
	{
		if (_debugger != null)
		{
			_debugger.Log("Endpoint set up canceled with status: " + status);
		}
		if (_onSetupEndpointsComplete != null)
		{
			_onSetupEndpointsComplete(false);
		}
	}

	public override void LEGOIDLoggedOutImplementation(string json)
	{
		if (_debugger != null)
		{
			_debugger.Log(json);
			JsonObject jsonObject = JSonParser.Parse(json);
			jsonObject.Print();
		}
	}

	public override void LEGOIDLogOutFailedImplementation(string error)
	{
		if (_debugger != null)
		{
			_debugger.Log("Log out failed with error: " + error);
		}
	}

	public override void LEGOIDLogOutCanceledImplementation(string status)
	{
		if (_debugger != null)
		{
			_debugger.Log("Log out canceled with status: " + status);
		}
	}

	public override void LEGOIDCurrentUserImplementation(string currentUser)
	{
		if (_debugger != null)
		{
			_debugger.Log("Current user information: " + currentUser);
		}
	}

	public override void LEGOIDCurrentUserFailedImplementation(string error)
	{
		if (_debugger != null)
		{
			_debugger.Log("Current user information request failed with error: " + error);
		}
	}

	public override void LEGOIDCurrentUserCanceledImplementation(string status)
	{
		if (_debugger != null)
		{
			_debugger.Log("Current user information request canceled with status: " + status);
		}
	}

	public override void LEGOIDGetRequestUpdateImplementation(byte[] data)
	{
		try
		{
			string text = Encoding.UTF8.GetString(data);
			if (_debugger != null)
			{
				_debugger.Log("Get Request : " + text);
			}
			if (_onGetRequestSuccess != null)
			{
				_onGetRequestSuccess(text);
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.StackTrace);
		}
	}

	public override void LEGOSDKLegalPrivacyPolicyFailedImplementation()
	{
		if (_debugger != null)
		{
			_debugger.Log("LEGOSDKLegalPrivacyPolicyFailedImplementation()");
		}
	}

	public override void LEGOSDKLegalPrivacyPolicyDismissedImplementation()
	{
		if (_debugger != null)
		{
			_debugger.Log("LEGOSDKLegalPrivacyPolicyDismissedImplementation()");
		}
	}

	public override void LEGOSDKLegalCookiePolicyFailedImplementation()
	{
		if (_debugger != null)
		{
			_debugger.Log("LEGOSDKLegalCookiePolicyFailedImplementation()");
		}
	}

	public override void LEGOSDKLegalCookiePolicyDismissedImplementation()
	{
		if (_debugger != null)
		{
			_debugger.Log("LEGOSDKLegalCookiePolicyDismissedImplementation()");
		}
	}

	public override void LEGOSDKLegalTermsOfUseFailedImplementation()
	{
		if (_debugger != null)
		{
			_debugger.Log("LEGOSDKLegalTermsOfUseFailedImplementation()");
		}
	}

	public override void LEGOSDKLegalTermsOfUseDismissedImplementation()
	{
		if (_debugger != null)
		{
			_debugger.Log("LEGOSDKLegalTermsOfUseDismissedImplementation()");
		}
	}

	public override void LEGOSDKLegalParentalGateFailedImplementation()
	{
		if (_debugger != null)
		{
			_debugger.Log("LEGOSDKLegalParentalGateFailedImplementation()");
		}
	}

	public override void LEGOSDKLegalParentalGateCompletedImplementation(string completed)
	{
		if (_debugger != null)
		{
			_debugger.Log("LEGOSDKLegalParentalGateCompletedImplementation( " + completed + ")");
		}
	}

	public override void LEGOIDGetRequestFailedImplementation(string error)
	{
		if (_onGetRequestFail != null)
		{
			_onGetRequestFail(error);
		}
	}

	public override void LEGOIDGetProgressUpdateImplementation(string progress)
	{
		if (_debugger != null)
		{
			_debugger.Log("Get Progress Update :" + progress);
		}
	}

	public override void LEGOIDGetRequestCanceledImplementation(string status)
	{
		if (_onGetRequestFail != null)
		{
			_onGetRequestFail(status);
		}
	}

	public override void LEGOIDPostRequestUpdateImplementation(byte[] dataBytes)
	{
		if (_onPostRequestSuccess != null)
		{
			_onPostRequestSuccess("Success");
		}
	}

	public override void LEGOIDPostRequestFailedImplementation(string error)
	{
		if (_debugger != null)
		{
			_debugger.Log("Post request failed with error: " + error);
		}
		if (_onPostRequestFail != null)
		{
			_onPostRequestFail(error);
		}
	}

	public override void LEGOIDPostProgressUpdateImplementation(string progress)
	{
		if (_debugger != null)
		{
			_debugger.Log("Post Progress Update :" + progress);
		}
	}

	public override void LEGOIDPostRequestCanceledImplementation(string status)
	{
		if (_debugger != null)
		{
			_debugger.Log("Post request canceled with status: " + status);
		}
		if (_onPostRequestFail != null)
		{
			_onPostRequestFail(status);
		}
	}

	public override void LEGOIDPostFileRequestUpdateImplementation(byte[] dataBytes)
	{
		if (_debugger != null)
		{
			string text = Encoding.UTF8.GetString(dataBytes);
			try
			{
				_debugger.Log(text);
				JsonObject jsonObject = JSonParser.Parse(text);
				jsonObject.Print();
			}
			catch (Exception ex)
			{
				Debug.Log(ex.StackTrace);
			}
		}
	}

	public override void LEGOIDPostFileRequestFailedImplementation(string error)
	{
		if (_debugger != null)
		{
			_debugger.Log("Post files request failed with error: " + error);
		}
	}

	public override void LEGOIDPostFileProgressUpdateImplementation(string progress)
	{
		if (_debugger != null)
		{
			_debugger.Log("Post File Progress Update :" + progress);
		}
	}

	public override void LEGOIDPostFileRequestCanceledImplementation(string status)
	{
		if (_debugger != null)
		{
			_debugger.Log("Post files request canceled with status: " + status);
		}
	}

	public override void LEGOIDGenericPostRequestUpdateImplementation(byte[] dataBytes)
	{
		if (_onGenericPostRequestSuccess != null)
		{
			_onGenericPostRequestSuccess("Success");
		}
	}

	public override void LEGOIDGenericPostProgressUpdateImplementation(string progress)
	{
		if (_debugger != null)
		{
			_debugger.Log("Generic Post Progress Update :" + progress);
		}
	}

	public override void LEGOIDGenericPostRequestFailedImplementation(string error)
	{
		if (_debugger != null)
		{
			_debugger.Log("Generic post request failed with error: " + error);
		}
		if (_onGenericPostRequestFail != null)
		{
			_onGenericPostRequestFail(error);
		}
	}

	public override void LEGOIDGenericPostRequestCanceledImplementation(string status)
	{
		if (_debugger != null)
		{
			_debugger.Log("Generic post request canceled with status: " + status);
		}
	}

	public override void LEGOIDGenericViewLoadStartedImplementation()
	{
		if (_debugger != null)
		{
			_debugger.Log("Generic view Load Started");
		}
	}

	public override void LEGOIDGenericViewLoadCompleteImplementation()
	{
		if (_debugger != null)
		{
			_debugger.Log("Generic view Load Complete");
		}
	}

	public override void LEGOIDGenericViewErrorImplementation(string message)
	{
		if (_debugger != null)
		{
			_debugger.Log("Generic View failed loading with error: " + message);
		}
	}

	public override void LEGOIDGenericViewClosedImplementation()
	{
		if (_debugger != null)
		{
			_debugger.Log("Closing generic window");
		}
	}

	public override void LEGOIDGetTextsUpdateImplementation(string keyValueJson)
	{
		if (_debugger != null)
		{
			_debugger.Log("Json message: " + keyValueJson);
			_debugger.Log(((JsonValue)JSonParser.Parse(keyValueJson)["HeaderTexts"]["LogoutText"]).GetRaw());
		}
	}

	public override void LEGOIDGetTextsFailedImplementation(string errorMessage)
	{
		if (_debugger != null)
		{
			_debugger.Log("Get text calls failed with the error message " + errorMessage);
		}
	}

	public override void LEGOIDSessionIDUpdateImplementation(string ssid)
	{
		if (_debugger != null)
		{
			_debugger.Log("Session ID is: " + ssid);
		}
	}

	public override void LEGOIDNoSessionIDImplementation()
	{
		if (_debugger != null)
		{
			_debugger.Log("No session id");
		}
	}

	public override void LEGOIDCancelAllRequestsImplementation(string status)
	{
		if (_debugger != null)
		{
			_debugger.Log("All calls canceled with status: " + status);
		}
	}

	public override void Init()
	{
		InitCallbacks();
	}

	public override void LEGOIDCookiesUpdateImplementation(string cookies)
	{
		if (_debugger != null)
		{
			_debugger.Log("Value of cookies: " + cookies);
		}
	}

	public override void LEGOIDNoCookiesImplementation()
	{
		if (_debugger != null)
		{
			_debugger.Log("No cookie values present.");
		}
	}

	public override void LEGOIDAccountAPIBasePathImplementation(string basePath)
	{
		if (_debugger != null)
		{
			if (basePath == string.Empty)
			{
				basePath = "Basepath was empty!";
			}
			_debugger.Log("Account Api basepath : " + basePath);
		}
	}
}
