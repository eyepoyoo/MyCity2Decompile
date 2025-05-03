using System;
using UnityEngine;

[RequireComponent(typeof(TextLoaderBehaviour))]
public sealed class SessionManager : InitialisationObject
{
	public string[] trustedDomains;

	public bool allowAllDomains = true;

	private static bool _isLoadFinished;

	private static bool _disableSaveOnExit;

	private bool _illegalDomain;

	private TextLoaderBehaviour _textLoader;

	public Action _onApplicationPause;

	public Action _onApplicationResume;

	public Action _onApplicationReconnect;

	public SignalSender onCompleteSignals;

	private static SessionManager _instance;

	public static SessionManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = UnityEngine.Object.FindObjectOfType(typeof(SessionManager)) as SessionManager;
			}
			return _instance;
		}
	}

	public TextLoaderBehaviour TextLoader
	{
		get
		{
			if (_textLoader == null)
			{
				_textLoader = GetComponent<TextLoaderBehaviour>();
			}
			return _textLoader;
		}
	}

	public static bool IsLoadFinished
	{
		get
		{
			return _isLoadFinished;
		}
	}

	public static void OutputBambooInfo()
	{
		Debug.Log(bambooInfoString());
	}

	public static string bambooInfoString()
	{
		return "BAMBOO_VERSION: " + AnvilBuildInfo._pBambooVersion + "\nBUILD_DATE: " + AnvilBuildInfo._pBuildDate + "\nBAMBOO_PLAN: " + AnvilBuildInfo.GetVar("BAMBOO_PLAN", "Plan info not found.") + "\nPROJECT: " + AnvilBuildInfo._pProject + "\nPROJECT_CODE: " + AnvilBuildInfo._pProjectCode + "\nBUILD_ID: " + AnvilBuildInfo._pBuildId + "\nTITLE: " + AnvilBuildInfo._pTitle + "\nPLATFORM: " + AnvilBuildInfo._pPlatform + "\nOS_VERSION: " + AnvilBuildInfo._pOsVersion + "\nVERSION: " + AnvilBuildInfo._pVersion + "\nBUNDLE_VERSION: " + AnvilBuildInfo._pBundleVersion + "\nDEVICE_CAPS: " + AnvilBuildInfo._pDeviceCapabilities + "\nBUNDLE_ID: " + AnvilBuildInfo._pBundleId + "\nCERTIFICATE: " + AnvilBuildInfo._pCertificate + "\nAPP_NAME: " + AnvilBuildInfo._pAppName;
	}

	public static void DebugQuitWithoutSaving()
	{
	}

	private bool CheckTrustedDomains()
	{
		if (Debug.isDebugBuild || Application.isEditor || allowAllDomains)
		{
			Debug.Log("Not Checking Trusted Domains");
			return true;
		}
		if (trustedDomains == null || trustedDomains.Length == 0)
		{
			return true;
		}
		string[] array = trustedDomains;
		foreach (string text in array)
		{
			string text2 = Application.absoluteURL.Replace("https://", string.Empty);
			text2 = text2.Replace("http://", string.Empty);
			int length = text2.IndexOf('/');
			text2 = text2.Substring(0, length);
			Debug.Log("Checking Domain: " + text2);
			if (text2.EndsWith(text))
			{
				Debug.Log("Application is running on allowed domain: " + text);
				return true;
			}
		}
		return false;
	}

	protected override void Awake()
	{
		base.Awake();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public override void startInitialising()
	{
		_currentState = InitialisationState.INITIALISING;
		OutputBambooInfo();
		allowAllDomains = Application.isEditor || Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
		SessionManager[] array = UnityEngine.Object.FindObjectsOfType(typeof(SessionManager)) as SessionManager[];
		if (array.Length > 1)
		{
			if (Debug.isDebugBuild)
			{
				Debug.Log("Session already initialised: skipping " + base.gameObject.name);
			}
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		_instance = this;
		if (!CheckTrustedDomains())
		{
			GameObject[] array2 = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
			GameObject[] array3 = array2;
			foreach (GameObject gameObject in array3)
			{
				if (gameObject != base.gameObject)
				{
					UnityEngine.Object.DestroyImmediate(gameObject);
				}
			}
			_illegalDomain = true;
			throw new Exception("ILLEGAL DOMAIN");
		}
		_currentState = InitialisationState.FINISHED;
	}

	private void OnApplicationQuit()
	{
		if (_illegalDomain || _disableSaveOnExit)
		{
			return;
		}
		Debug.Log("User Quit: Saving Locally!");
		if (Facades<SaveDataFacade>.Instance != null)
		{
			Facades<SaveDataFacade>.Instance.SaveAll(delegate
			{
				Debug.Log("Force Save All Complete!");
			}, Encryption.ExecutionMode.Synchronous, SaveDataFacade.SaveMode.Force);
		}
	}

	private void OnApplicationPause(bool focus)
	{
		if (focus && _onApplicationResume != null)
		{
			_onApplicationResume();
		}
		else if (!focus && _onApplicationPause != null)
		{
			_onApplicationPause();
		}
	}

	public void testResume()
	{
		OnApplicationPause(true);
	}
}
