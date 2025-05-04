using System;
using UnityEngine;

public abstract class LEGOIDCallbackHandler : MonoBehaviour
{
	private enum CALLBACK_TYPES
	{
		LOG_IN_JSON_UPDATE = 0,
		LOG_IN_FAILED = 1,
		CANCEL_LOGIN = 2,
		REGISTER_COMPLETE = 3,
		REGISTER_PROCESS_FAILED = 4,
		CANCEL_REGISTER_PROCESS = 5,
		SET_UP_ENDPOINT_COMPLETE = 6,
		SET_UP_ENDPOINT_FAILED = 7,
		CANCEL_SET_UP_ENDPOINT = 8,
		ENDPOINT_ENTRIES_BASEPATH_ACCOUNTAPI = 9,
		LOG_OUT_JSON_UPDATE = 10,
		LOG_OUT_FAILED = 11,
		CANCEL_LOG_OUT = 12,
		CURRENT_USER = 13,
		CURRENT_USER_FAILED = 14,
		CANCEL_CURRENT_USER = 15,
		GET_REQUEST_UPDATE = 16,
		GET_REQUEST_FAILED = 17,
		GET_REQUEST_PROGRESS_UPDATE = 18,
		CANCEL_GET_REQUEST = 19,
		POST_REQUEST_UPDATE = 20,
		POST_REQUEST_FAILED = 21,
		POST_REQUEST_PROGRESS_UPDATE = 22,
		CANCEL_POST_REQUEST = 23,
		POST_FILE_REQUEST_UPDATE = 24,
		POST_FILE_REQUEST_FAILED = 25,
		POST_FILE_REQUEST_PROGRESS_UPDATE = 26,
		CANCEL_POST_FILE_REQUEST = 27,
		GENERIC_POST_REQUEST_UPDATE = 28,
		GENERIC_POST_REQUEST_FAILED = 29,
		GENERIC_POST_PROGRESS_UPDATE = 30,
		CANCEL_GENERIC_POST_REQUEST = 31,
		GENERIC_WEB_VIEW_LOAD_STARTED = 32,
		GENERIC_WEB_VIEW_LOAD_COMPLETE = 33,
		GENERIC_WEB_VIEW_ERROR = 34,
		GENERIC_WEB_VIEW_CLOSED = 35,
		GET_TEXTS_UPDATE = 36,
		GET_TEXTS_FAILED = 37,
		SESSION_ID_UPDATE = 38,
		NO_SESSION_ID = 39,
		COOKIES_UPDATE = 40,
		NO_COOKIES = 41,
		CANCEL_ALL = 42,
		LEGAL_PRIVACYPOLICY_FAILED = 43,
		LEGAL_PRIVACYPOLICY_DISMISSED = 44,
		LEGAL_COOKIEPOLICY_FAILED = 45,
		LEGAL_COOKIEPOLICY_DISMISSED = 46,
		LEGAL_TERMSOFUSEOFLEGOAPPS_FAILED = 47,
		LEGAL_TERMSOFUSEOFLEGOAPPS_DISMISSED = 48,
		LEGAL_PARENTALGATE_FAILED = 49,
		LEGAL_PARENTALGATE_COMPLETED = 50,
		TYPES = 51
	}

	protected string[] methodName = new string[51]
	{
		"LEGOIDLogInUpdate", "LEGOIDLoginFailed", "LEGOIDLogInDismissed", "LEGOIDRegisterComplete", "LEGOIDRegisterFailed", "LEGOIDRegisterDismissed", "LEGOIDSetupUpEndpointComplete", "LEGOIDSetUpEndpointFailed", "LEGOIDSetUpEndpointCanceled", "LEGOIDAccountAPIBasePath",
		"LEGOIDLoggedOut", "LEGOIDLogOutFailed", "LEGOIDLogOutCanceled", "LEGOIDCurrentUser", "LEGOIDCurrentUserFailed", "LEGOIDCurrentUserCanceled", "LEGOIDGetRequestUpdate", "LEGOIDGetRequestFailed", "LEGOIDGetProgressUpdate", "LEGOIDGetRequestCanceled",
		"LEGOIDPostRequestUpdate", "LEGOIDPostRequestFailed", "LEGOIDPostProgressUpdate", "LEGOIDPostRequestCanceled", "LEGOIDPostFileRequestUpdate", "LEGOIDPostFileRequestFailed", "LEGOIDPostFileProgressUpdate", "LEGOIDPostFileRequestCanceled", "LEGOIDGenericPostRequestUpdate", "LEGOIDGenericPostRequestFailed",
		"LEGOIDGenericPostProgressUpdate", "LEGOIDGenericPostRequestCanceled", "LEGOIDGenericViewLoadStarted", "LEGOIDGenericViewLoadComplete", "LEGOIDGenericViewError", "LEGOIDGenericViewClosed", "LEGOIDGetTextsUpdate", "LEGOIDGetTextsFailed", "LEGOIDSessionIDUpdate", "LEGOIDNoSessionID",
		"LEGOIDCookieUpdate", "LEGOIDNoCookies", "LEGOIDCancelAllRequests", "LEGOSDKLegalPrivacyPolicyFailed", "LEGOSDKLegalPrivacyPolicyDismissed", "LEGOSDKLegalCookiePolicyFailed", "LEGOSDKLegalCookiePolicyDismissed", "LEGOSDKLegalTERMSOFUSEOFLEGOAPPSFailed", "LEGOSDKTERMSOFUSEOFLEGOAPPSDismissed", "LEGOSDKLegalParentalGateFailed",
		"LEGOSDKLegalParentalGateCompleted"
	};

	private void Start()
	{
		LEGOSDKFacade.Instance.SetUpCallbackParameters(base.gameObject.name, methodName);
		Init();
	}

	public void LEGOIDLogInUpdate(string json)
	{
		LEGOIDLogInUpdateImplementation(json);
	}

	public void LEGOIDLoginFailed(string error)
	{
		LEGOIDLoginFailedImplementation(error);
	}

	public void LEGOIDLogInDismissed()
	{
		LEGOIDLogInDismissedImplementation();
	}

	public void LEGOIDRegisterComplete()
	{
		LEGOIDRegisterCompleteImplementation();
	}

	public void LEGOIDRegisterFailed(string error)
	{
		LEGOIDRegisterFailedImplementation(error);
	}

	public void LEGOIDRegisterDismissed()
	{
		LEGOIDRegisterDismissedImplementation();
	}

	public void LEGOIDSetupUpEndpointComplete()
	{
		LEGOIDSetupEndpointCompleteImplementation();
	}

	public void LEGOIDSetUpEndpointFailed(string error)
	{
		LEGOIDSetupEndpointFailedImplementation(error);
	}

	public void LEGOIDSetUpEndpointCanceled(string status)
	{
		LEGOIDSetupEndpointCanceledImplementation(status);
	}

	public void LEGOIDAccountAPIBasePath(string basePath)
	{
		LEGOIDAccountAPIBasePathImplementation(basePath);
	}

	public void LEGOIDLoggedOut(string json)
	{
		LEGOIDLoggedOutImplementation(json);
	}

	public void LEGOIDLogOutFailed(string error)
	{
		LEGOIDLogOutFailedImplementation(error);
	}

	public void LEGOIDLogOutCanceled(string status)
	{
		LEGOIDLogOutCanceledImplementation(status);
	}

	public void LEGOIDCurrentUser(string currentUser)
	{
		LEGOIDCurrentUserImplementation(currentUser);
	}

	public void LEGOIDCurrentUserFailed(string error)
	{
		LEGOIDCurrentUserFailedImplementation(error);
	}

	public void LEGOIDCurrentUserCanceled(string status)
	{
		LEGOIDCurrentUserCanceled(status);
	}

	public void LEGOIDGetRequestUpdate(string data)
	{
		LEGOIDGetRequestUpdateImplementation(Convert.FromBase64String(data));
	}

	public void LEGOIDGetRequestFailed(string error)
	{
		LEGOIDGetRequestFailedImplementation(error);
	}

	public void LEGOIDGetProgressUpdate(string progress)
	{
		LEGOIDGetProgressUpdateImplementation(progress);
	}

	public void LEGOIDGetRequestCanceled(string status)
	{
		LEGOIDGetRequestCanceledImplementation(status);
	}

	public void LEGOIDPostRequestUpdate(string data)
	{
		LEGOIDPostRequestUpdateImplementation(Convert.FromBase64String(data));
	}

	public void LEGOIDPostRequestFailed(string error)
	{
		LEGOIDPostRequestFailedImplementation(error);
	}

	public void LEGOIDPostProgressUpdate(string progress)
	{
		LEGOIDPostProgressUpdateImplementation(progress);
	}

	public void LEGOIDPostRequestCanceled(string status)
	{
		LEGOIDPostRequestCanceledImplementation(status);
	}

	public void LEGOIDPostFileRequestUpdate(string data)
	{
		LEGOIDPostFileRequestUpdateImplementation(Convert.FromBase64String(data));
	}

	public void LEGOIDPostFileRequestFailed(string error)
	{
		LEGOIDPostFileRequestFailedImplementation(error);
	}

	public void LEGOIDPostFileProgressUpdate(string progress)
	{
		LEGOIDPostFileProgressUpdateImplementation(progress);
	}

	public void LEGOIDPostFileRequestCanceled(string status)
	{
		LEGOIDPostFileRequestCanceledImplementation(status);
	}

	public void LEGOIDGenericPostRequestUpdate(string data)
	{
		LEGOIDGenericPostRequestUpdateImplementation(Convert.FromBase64String(data));
	}

	public void LEGOIDGenericPostProgressUpdate(string progress)
	{
		LEGOIDGenericPostProgressUpdateImplementation(progress);
	}

	public void LEGOIDGenericPostRequestFailed(string error)
	{
		LEGOIDGenericPostRequestFailedImplementation(error);
	}

	public void LEGOIDGenericPostRequestCanceled(string status)
	{
		LEGOIDGenericPostRequestCanceledImplementation(status);
	}

	public void LEGOIDGenericViewLoadStarted()
	{
		LEGOIDGenericViewLoadStartedImplementation();
	}

	public void LEGOIDGenericViewLoadComplete()
	{
		LEGOIDGenericViewLoadCompleteImplementation();
	}

	public void LEGOIDGenericViewError(string message)
	{
		LEGOIDGenericViewErrorImplementation(message);
	}

	public void LEGOIDGenericViewClosed()
	{
		LEGOIDGenericViewClosedImplementation();
	}

	public void LEGOIDGetTextsUpdate(string keyValueJson)
	{
		LEGOIDGetTextsUpdateImplementation(keyValueJson);
	}

	public void LEGOIDGetTextsFailed(string errorMessage)
	{
		LEGOIDGetTextsFailedImplementation(errorMessage);
	}

	public void LEGOIDSessionIDUpdate(string ssid)
	{
		LEGOIDSessionIDUpdateImplementation(ssid);
	}

	public void LEGOIDNoSessionID()
	{
		LEGOIDNoSessionIDImplementation();
	}

	public void LEGOIDCookieUpdate(string cookies)
	{
		LEGOIDCookiesUpdateImplementation(cookies);
	}

	public void LEGOIDNoCookies()
	{
		LEGOIDNoCookiesImplementation();
	}

	public void LEGOIDCancelAllRequests(string status)
	{
		LEGOIDCancelAllRequestsImplementation(status);
	}

	public void LEGOSDKLegalPrivacyPolicyFailed()
	{
		LEGOSDKLegalPrivacyPolicyFailedImplementation();
	}

	public void LEGOSDKLegalPrivacyPolicyDismissed()
	{
		LEGOSDKLegalPrivacyPolicyDismissedImplementation();
	}

	public void LEGOSDKLegalCookiePolicyFailed()
	{
		LEGOSDKLegalCookiePolicyFailedImplementation();
	}

	public void LEGOSDKLegalCookiePolicyDismissed()
	{
		LEGOSDKLegalCookiePolicyDismissedImplementation();
	}

	public void LEGOSDKLegalTERMSOFUSEOFLEGOAPPSFailed()
	{
		LEGOSDKLegalTermsOfUseFailedImplementation();
	}

	public void LEGOSDKTERMSOFUSEOFLEGOAPPSDismissed()
	{
		LEGOSDKLegalTermsOfUseDismissedImplementation();
	}

	public void LEGOSDKLegalParentalGateCompleted(string completed)
	{
		LEGOSDKLegalParentalGateCompletedImplementation(completed);
	}

	public void LEGOSDKLegalParentalGateFailed()
	{
		LEGOSDKLegalParentalGateFailedImplementation();
	}

	public abstract void LEGOIDLogInUpdateImplementation(string json);

	public abstract void LEGOIDLoginFailedImplementation(string error);

	public abstract void LEGOIDLogInDismissedImplementation();

	public abstract void LEGOIDRegisterCompleteImplementation();

	public abstract void LEGOIDRegisterFailedImplementation(string error);

	public abstract void LEGOIDRegisterDismissedImplementation();

	public abstract void LEGOIDSetupEndpointCompleteImplementation();

	public abstract void LEGOIDSetupEndpointFailedImplementation(string error);

	public abstract void LEGOIDSetupEndpointCanceledImplementation(string status);

	public abstract void LEGOIDLoggedOutImplementation(string json);

	public abstract void LEGOIDLogOutFailedImplementation(string error);

	public abstract void LEGOIDLogOutCanceledImplementation(string status);

	public abstract void LEGOIDCurrentUserImplementation(string currentUser);

	public abstract void LEGOIDCurrentUserFailedImplementation(string error);

	public abstract void LEGOIDCurrentUserCanceledImplementation(string status);

	public abstract void LEGOIDGetRequestUpdateImplementation(byte[] data);

	public abstract void LEGOIDGetRequestFailedImplementation(string error);

	public abstract void LEGOIDGetProgressUpdateImplementation(string progress);

	public abstract void LEGOIDGetRequestCanceledImplementation(string status);

	public abstract void LEGOIDPostRequestUpdateImplementation(byte[] data);

	public abstract void LEGOIDPostRequestFailedImplementation(string error);

	public abstract void LEGOIDPostProgressUpdateImplementation(string progress);

	public abstract void LEGOIDPostRequestCanceledImplementation(string status);

	public abstract void LEGOIDPostFileRequestUpdateImplementation(byte[] data);

	public abstract void LEGOIDPostFileRequestFailedImplementation(string error);

	public abstract void LEGOIDPostFileProgressUpdateImplementation(string progress);

	public abstract void LEGOIDPostFileRequestCanceledImplementation(string status);

	public abstract void LEGOIDGenericPostRequestUpdateImplementation(byte[] data);

	public abstract void LEGOIDGenericPostProgressUpdateImplementation(string progress);

	public abstract void LEGOIDGenericPostRequestFailedImplementation(string error);

	public abstract void LEGOIDGenericPostRequestCanceledImplementation(string status);

	public abstract void LEGOIDGenericViewLoadStartedImplementation();

	public abstract void LEGOIDGenericViewLoadCompleteImplementation();

	public abstract void LEGOIDGenericViewErrorImplementation(string message);

	public abstract void LEGOIDGenericViewClosedImplementation();

	public abstract void LEGOIDGetTextsUpdateImplementation(string keyValueJson);

	public abstract void LEGOIDGetTextsFailedImplementation(string errorMessage);

	public abstract void LEGOIDSessionIDUpdateImplementation(string ssid);

	public abstract void LEGOIDNoSessionIDImplementation();

	public abstract void LEGOIDCookiesUpdateImplementation(string cookies);

	public abstract void LEGOIDNoCookiesImplementation();

	public abstract void LEGOIDAccountAPIBasePathImplementation(string basePath);

	public abstract void LEGOIDCancelAllRequestsImplementation(string status);

	public abstract void LEGOSDKLegalPrivacyPolicyFailedImplementation();

	public abstract void LEGOSDKLegalPrivacyPolicyDismissedImplementation();

	public abstract void LEGOSDKLegalCookiePolicyFailedImplementation();

	public abstract void LEGOSDKLegalCookiePolicyDismissedImplementation();

	public abstract void LEGOSDKLegalTermsOfUseFailedImplementation();

	public abstract void LEGOSDKLegalTermsOfUseDismissedImplementation();

	public abstract void LEGOSDKLegalParentalGateFailedImplementation();

	public abstract void LEGOSDKLegalParentalGateCompletedImplementation(string completed);

	public abstract void Init();
}
