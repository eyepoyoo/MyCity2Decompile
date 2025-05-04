using System;
using System.Text;
using UnityEngine;

public class LEGOIDCallbackHandlerExample : LEGOIDCallbackHandler
{
	private const string ENDPOINT_CONFIG_KEY = "configEndpoint";

	public ScreenDebug debugger;

	[Range(0f, 1f)]
	public float xMin = 0.25f;

	[Range(0f, 1f)]
	public float yMin = 0.25f;

	[Range(0f, 1f)]
	public float width = 0.5f;

	[Range(0f, 1f)]
	public float height = 0.5f;

	private JsonObject logInInformation;

	public string experience = "fusion_townmaster";

	public string LogInName
	{
		get
		{
			if (logInInformation != null)
			{
				return ((JsonValue)logInInformation["Data"]["UserName"]).GetRaw();
			}
			return string.Empty;
		}
	}

	public bool Authenticated
	{
		get
		{
			if (logInInformation != null)
			{
				bool val;
				((JsonValue)logInInformation["Data"]["Authenticated"]).GetAsBool(out val);
				return val;
			}
			return false;
		}
	}

	private void OnGUI()
	{
		if (debugger != null)
		{
			debugger.ShowLog();
		}
	}

	public override void LEGOIDLogInUpdateImplementation(string json)
	{
		Debug.Log(json);
		logInInformation = JSonParser.Parse(json);
		if (debugger != null)
		{
			debugger.Log(LogInName);
			debugger.Log(Authenticated + string.Empty);
		}
	}

	public override void LEGOIDLoginFailedImplementation(string error)
	{
		if (debugger != null)
		{
			debugger.Log("LoginFailed - did you call the endpoint configurator?");
		}
	}

	public override void LEGOIDLogInDismissedImplementation()
	{
		if (debugger != null)
		{
			debugger.Log("LogInDismissed");
		}
	}

	public override void LEGOIDRegisterCompleteImplementation()
	{
		if (debugger != null)
		{
			debugger.Log("RegisterComplete");
		}
	}

	public override void LEGOIDRegisterFailedImplementation(string error)
	{
		if (debugger != null)
		{
			debugger.Log("RegisterFailed - did you call the endpoint configurator?");
		}
	}

	public override void LEGOIDRegisterDismissedImplementation()
	{
		if (debugger != null)
		{
			debugger.Log("Register Dismissed");
		}
	}

	public override void LEGOIDSetupEndpointCompleteImplementation()
	{
		if (debugger != null)
		{
			debugger.Log("Endpoint set up");
		}
	}

	public override void LEGOIDSetupEndpointFailedImplementation(string error)
	{
		if (debugger != null)
		{
			debugger.Log("Endpoint set up failed with error message: " + error);
		}
	}

	public override void LEGOIDSetupEndpointCanceledImplementation(string status)
	{
		if (debugger != null)
		{
			debugger.Log("Endpoint set up canceled with status: " + status);
		}
	}

	public override void LEGOIDAccountAPIBasePathImplementation(string basePath)
	{
		if (debugger != null)
		{
			if (basePath == string.Empty)
			{
				basePath = "Basepath was empty!";
			}
			debugger.Log("Account Api basepath : " + basePath);
		}
	}

	public override void LEGOIDLoggedOutImplementation(string json)
	{
		if (debugger != null)
		{
			debugger.Log(json);
			JsonObject jsonObject = JSonParser.Parse(json);
			jsonObject.Print();
		}
	}

	public override void LEGOIDLogOutFailedImplementation(string error)
	{
		if (debugger != null)
		{
			debugger.Log("Log out failed with error: " + error);
		}
	}

	public override void LEGOIDLogOutCanceledImplementation(string status)
	{
		if (debugger != null)
		{
			debugger.Log("Log out canceled with status: " + status);
		}
	}

	public override void LEGOIDCurrentUserImplementation(string currentUser)
	{
		if (debugger != null)
		{
			debugger.Log("Current user information: " + currentUser);
		}
	}

	public override void LEGOIDCurrentUserFailedImplementation(string error)
	{
		if (debugger != null)
		{
			debugger.Log("Current user information request failed with error: " + error);
		}
	}

	public override void LEGOIDCurrentUserCanceledImplementation(string status)
	{
		if (debugger != null)
		{
			debugger.Log("Current user information request canceled with status: " + status);
		}
	}

	public override void LEGOIDGetRequestUpdateImplementation(byte[] data)
	{
		try
		{
			string text = Encoding.UTF8.GetString(data);
			if (debugger != null)
			{
				debugger.Log("Get Request : " + text);
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.StackTrace);
		}
	}

	public override void LEGOIDGetRequestFailedImplementation(string error)
	{
		if (debugger != null)
		{
			debugger.Log("Get request failed with error: " + error);
		}
	}

	public override void LEGOIDGetProgressUpdateImplementation(string progress)
	{
		if (debugger != null)
		{
			debugger.Log("Get Progress Update :" + progress);
		}
	}

	public override void LEGOIDGetRequestCanceledImplementation(string status)
	{
		if (debugger != null)
		{
			debugger.Log("Get request canceled with status: " + status);
		}
	}

	public override void LEGOIDPostRequestUpdateImplementation(byte[] dataBytes)
	{
		string text = Encoding.UTF8.GetString(dataBytes);
		try
		{
			if (debugger != null)
			{
				debugger.Log(text);
			}
			JsonObject jsonObject = JSonParser.Parse(text);
			jsonObject.Print();
		}
		catch (Exception ex)
		{
			Debug.Log(ex.StackTrace);
		}
	}

	public override void LEGOIDPostRequestFailedImplementation(string error)
	{
		if (debugger != null)
		{
			debugger.Log("Post request failed with error: " + error);
		}
	}

	public override void LEGOIDPostProgressUpdateImplementation(string progress)
	{
		if (debugger != null)
		{
			debugger.Log("Post Progress Update :" + progress);
		}
	}

	public override void LEGOIDPostRequestCanceledImplementation(string status)
	{
		if (debugger != null)
		{
			debugger.Log("Post request canceled with status: " + status);
		}
	}

	public override void LEGOIDPostFileRequestUpdateImplementation(byte[] dataBytes)
	{
		string text = Encoding.UTF8.GetString(dataBytes);
		try
		{
			if (debugger != null)
			{
				debugger.Log(text);
			}
			JsonObject jsonObject = JSonParser.Parse(text);
			jsonObject.Print();
		}
		catch (Exception ex)
		{
			Debug.Log(ex.StackTrace);
		}
	}

	public override void LEGOIDPostFileRequestFailedImplementation(string error)
	{
		if (debugger != null)
		{
			debugger.Log("Post files request failed with error: " + error);
		}
	}

	public override void LEGOIDPostFileProgressUpdateImplementation(string progress)
	{
		if (debugger != null)
		{
			debugger.Log("Post File Progress Update :" + progress);
		}
	}

	public override void LEGOIDPostFileRequestCanceledImplementation(string status)
	{
		if (debugger != null)
		{
			debugger.Log("Post files request canceled with status: " + status);
		}
	}

	public override void LEGOIDGenericPostRequestUpdateImplementation(byte[] dataBytes)
	{
		string text = Encoding.UTF8.GetString(dataBytes);
		try
		{
			if (debugger != null)
			{
				debugger.Log(text);
			}
			JsonObject jsonObject = JSonParser.Parse(text);
			jsonObject.Print();
		}
		catch (Exception ex)
		{
			Debug.Log(ex.StackTrace);
		}
	}

	public override void LEGOIDGenericPostProgressUpdateImplementation(string progress)
	{
		if (debugger != null)
		{
			debugger.Log("GenericPostProgressUpdate :" + progress);
		}
	}

	public override void LEGOIDGenericPostRequestFailedImplementation(string error)
	{
		if (debugger != null)
		{
			debugger.Log("GenericPostRequestFailed with error: " + error);
		}
	}

	public override void LEGOIDGenericPostRequestCanceledImplementation(string status)
	{
		if (debugger != null)
		{
			debugger.Log("GenericPostRequestCanceled with status: " + status);
		}
	}

	public override void LEGOIDGenericViewLoadStartedImplementation()
	{
		if (debugger != null)
		{
			debugger.Log("GenericWebViewStartedComplete");
		}
	}

	public override void LEGOIDGenericViewLoadCompleteImplementation()
	{
		if (debugger != null)
		{
			debugger.Log("GenericWebViewLoadComplete");
		}
	}

	public override void LEGOIDGenericViewErrorImplementation(string message)
	{
		if (debugger != null)
		{
			debugger.Log("GenericWebViewError: " + message);
		}
	}

	public override void LEGOIDGenericViewClosedImplementation()
	{
		if (debugger != null)
		{
			debugger.Log("GenericWebViewClosed");
		}
	}

	public override void LEGOIDGetTextsUpdateImplementation(string keyValueJson)
	{
		if (debugger != null)
		{
			if (keyValueJson == string.Empty)
			{
				debugger.Log("No content in texts - did you call endpoint configurator?");
			}
			else
			{
				debugger.Log("GetTextsUpdate: " + keyValueJson);
			}
		}
	}

	public override void LEGOIDGetTextsFailedImplementation(string errorMessage)
	{
		if (debugger != null)
		{
			debugger.Log("GetTextsFailed: " + errorMessage);
		}
	}

	public override void LEGOIDSessionIDUpdateImplementation(string ssid)
	{
		if (debugger != null)
		{
			debugger.Log("SessionIDUpdate: " + ssid);
		}
	}

	public override void LEGOIDNoSessionIDImplementation()
	{
		if (debugger != null)
		{
			debugger.Log("NoSessionID");
		}
	}

	public override void LEGOIDCookiesUpdateImplementation(string cookies)
	{
		if (debugger != null)
		{
			debugger.Log("Value of cookies: " + cookies);
		}
	}

	public override void LEGOIDNoCookiesImplementation()
	{
		if (debugger != null)
		{
			debugger.Log("No cookie values present.");
		}
	}

	public override void LEGOIDCancelAllRequestsImplementation(string status)
	{
		if (debugger != null)
		{
			debugger.Log("CancelAllRequests With Status: " + status);
		}
	}

	public override void LEGOSDKLegalPrivacyPolicyFailedImplementation()
	{
		if (debugger != null)
		{
			debugger.Log("PrivacyPolicyFailed");
		}
	}

	public override void LEGOSDKLegalPrivacyPolicyDismissedImplementation()
	{
		if (debugger != null)
		{
			debugger.Log("PrivacyPolicyDismissed");
		}
	}

	public override void LEGOSDKLegalCookiePolicyFailedImplementation()
	{
		if (debugger != null)
		{
			debugger.Log("CookiePolicyFailed");
		}
	}

	public override void LEGOSDKLegalCookiePolicyDismissedImplementation()
	{
		debugger.Log("Cookie Policy Dismissed");
	}

	public override void LEGOSDKLegalTermsOfUseFailedImplementation()
	{
		if (debugger != null)
		{
			debugger.Log("TERMSOFUSEOFLEGOAPPSFailed");
		}
	}

	public override void LEGOSDKLegalTermsOfUseDismissedImplementation()
	{
		debugger.Log("Terms Of Use In Apps Dismissed");
	}

	public override void LEGOSDKLegalParentalGateCompletedImplementation(string completed)
	{
		if (debugger != null)
		{
			string text = ((int.Parse(completed) != 1) ? "false" : "true");
			string text2 = "ParentalGateCompleted " + text;
			debugger.Log(text2);
		}
	}

	public override void LEGOSDKLegalParentalGateFailedImplementation()
	{
		if (debugger != null)
		{
			debugger.Log("Parental gate failed..");
		}
	}

	public override void Init()
	{
		InitCallbacks();
	}

	public void InitCallbacks()
	{
		LEGOSDKFacade.Instance.SetUpCallbackParameters(base.gameObject.name, methodName);
	}

	public void SetExperienceAndEndpoint()
	{
		string endpointConfigurator = PlayerPrefs.GetString("configEndpoint");
		LEGOSDKFacade.Instance.SetUpConfigurationURLAndExperience(experience, endpointConfigurator);
	}
}
