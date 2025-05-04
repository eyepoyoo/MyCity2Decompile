using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LEGOSDKFacade
{
	public class FileInformation
	{
		public string name;

		public byte[] data;

		public string contentType;

		public FileInformation(string name, byte[] data, string contentType = "multipart/form-data")
		{
			this.name = name;
			this.data = new byte[data.Length];
			data.CopyTo(this.data, 0);
			this.contentType = contentType;
		}
	}

	private static LEGOSDKFacade instance;

	private bool callBackSetUp;

	private WWW dummyInternetClass;

	private AndroidJavaObject commLayer;

	private AndroidJavaObject theEditor;

	public static LEGOSDKFacade Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new LEGOSDKFacade();
			}
			return instance;
		}
	}

	public bool IsSetUp
	{
		get
		{
			return callBackSetUp;
		}
	}

	private LEGOSDKFacade()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		theEditor = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		commLayer = new AndroidJavaObject("com.lego.android2unitywrapper.LEGOIDCommLayer", theEditor);
	}

	public void SetUpCallbackParameters(string callbackObjectName, string[] callbackMethods)
	{
		commLayer.Call("SetUpCallbackParameters", callbackObjectName, callbackMethods);
		callBackSetUp = true;
	}

	public void SetUpConfigurationURLAndExperience(string experience, string endpointConfigurator)
	{
		commLayer.Call("SetUpConfigurationURLAndExperience", experience, endpointConfigurator);
	}

	public void ChangeEndpointConfiguratorURL(string newURL)
	{
		commLayer.Call("ChangeEndpointConfigurator", newURL);
	}

	public void LogOutUser()
	{
		commLayer.Call("LogOut");
	}

	public void LogIn()
	{
		commLayer.Call("ShowLogInWindow");
	}

	public void Register()
	{
		commLayer.Call("ShowRegisterWindow");
	}

	public void GetCurrentUser()
	{
		commLayer.Call("CurrentUser");
	}

	public void SetTimeOut(int to)
	{
		commLayer.Call("SetTimeout", to);
	}

	~LEGOSDKFacade()
	{
	}

	public void GetRequest(string requestURLString, string contentTypeString)
	{
		commLayer.Call("MakeGetRequest", requestURLString, contentTypeString);
	}

	public void PostRequest(string url, Dictionary<string, string> body, string contentType = "application/x-www-form-urlencoded")
	{
		string[] array = new string[body.Count];
		string[] array2 = new string[body.Count];
		IEnumerator<KeyValuePair<string, string>> enumerator = body.GetEnumerator();
		int num = 0;
		while (enumerator.MoveNext())
		{
			array[num] = enumerator.Current.Key;
			array2[num] = enumerator.Current.Value;
			num++;
		}
		commLayer.Call("MakePostRequest", url, contentType, body.Count, array, array2);
	}

	public void GenericPostRequest(string url, byte[] data, string contentType = "application/x-www-form-urlencoded")
	{
		commLayer.Call("MakeGenericPostRequest", url, contentType, data);
	}

	public void PostFileRequest(string url, Dictionary<string, string> body, FileInformation[] files)
	{
		string[] array = new string[body.Count];
		string[] array2 = new string[body.Count];
		string[] array3 = new string[files.Length];
		int[] array4 = new int[files.Length];
		string[] array5 = new string[files.Length];
		string[] array6 = new string[files.Length];
		IEnumerator<KeyValuePair<string, string>> enumerator = body.GetEnumerator();
		int num = 0;
		while (enumerator.MoveNext())
		{
			array[num] = enumerator.Current.Key;
			array2[num] = enumerator.Current.Value;
			num++;
		}
		for (num = 0; num < files.Length; num++)
		{
			array3[num] = files[num].name;
			array4[num] = files[num].data.Length;
			array5[num] = Encoding.UTF8.GetString(files[num].data);
			array6[num] = files[num].contentType;
		}
		commLayer.Call("MakePostRequest", url, body.Count, array, array2, files.Length, array3, array4, array5, array6);
	}

	public void PresentGenericWebPage(string url)
	{
		commLayer.Call("LEGOIDPresentWebViewWithURL", url);
	}

	public void GetSessionID()
	{
		commLayer.Call("LEGOIDGetSessionID");
	}

	public void GetTexts()
	{
		commLayer.Call("LEGOIDGetTexts");
	}

	public void GetLEGOIDAccountAPIBasePath()
	{
		commLayer.Call("LEGOIDGetAccountAPIBasePath");
	}

	public void DeleteLEGOCookies()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.lego.android.sdk.legoid.Cookies");
		androidJavaClass.CallStatic("deleteAll", theEditor);
	}

	public void GetLEGOCookies()
	{
		commLayer.Call("LEGOIDGetCookies");
	}

	public void CancelAllRequests()
	{
		commLayer.Call("LEGOIDCancelAllRequests");
	}

	public void CancelPostFilesRequest()
	{
		commLayer.Call("LEGOIDCancelPostFilesRequest");
	}

	public void CancelPostRequest()
	{
		commLayer.Call("LEGOIDCancelPostRequest");
	}

	public void CancelGenericPostRequest()
	{
		commLayer.Call("LEGOIDCancelRawDataPostRequest");
	}

	public void CancelGetRequest()
	{
		commLayer.Call("LEGOIDCancelGetRequest");
	}

	public void CancelSetupEndpointConfigurator()
	{
		commLayer.Call("LEGOIDCancelSetupEndpointConfigurator");
	}

	public void CancelCurrentUserRequest()
	{
		commLayer.Call("LEGOIDCancelCurrentUser");
	}

	public void CancelLogout()
	{
		commLayer.Call("LEGOIDCancelLogout");
	}

	public void DismissLogin()
	{
		commLayer.Call("LEGOIDCancelShowLoginWebView");
	}

	public void DismissRegister()
	{
		commLayer.Call("LEGOIDCancelShowRegisterWebView");
	}

	public void PresentPrivacyPolicy(LEGOSDKLegalLanguages language)
	{
		commLayer.Call("ShowPrivacyPolicy", (int)language);
	}

	public void PresentCookiePolicy(LEGOSDKLegalLanguages language)
	{
		commLayer.Call("ShowCookiePolicy", (int)language);
	}

	public void PresentTermsOfUseInLEGOApps(LEGOSDKLegalLanguages language)
	{
		commLayer.Call("ShowTermsOfUse", (int)language);
	}

	public void PresentParentalGate()
	{
		commLayer.Call("ShowParentalGate");
	}
}
