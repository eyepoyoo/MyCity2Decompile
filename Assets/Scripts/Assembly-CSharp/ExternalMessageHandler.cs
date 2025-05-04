using System;
using System.IO;
using System.Text;
using UnityEngine;

public class ExternalMessageHandler : InitialisationObject
{
	public class CallbackInfo
	{
		public UserNotificationCallback callback;

		public string name;
	}

	private const string NEW_LINE = "\r\n";

	private int attempt;

	[HideInInspector]
	public int selectedSaveKey;

	public string inputUserId;

	public string inputUsername;

	public bool doAutoFail;

	public CallbackInfo userNotificationCallback_accept = new CallbackInfo();

	public CallbackInfo userNotificationCallback_refuse = new CallbackInfo();

	private static ExternalMessageHandler _instance;

	private static string LOG_FILE_PATH = string.Empty;

	private byte[] newLineBytes;

	private FileStream logFileStream;

	private static bool _spinnerOn;

	public static ExternalMessageHandler Instance
	{
		get
		{
			if (_instance == null)
			{
				Debug.LogError("EXTERNAL MESSAGE HANDLER NEEDS TO BE SET UP IN THE MAIN SCENE!");
			}
			return _instance;
		}
	}

	private new void OnDestroy()
	{
		base.OnDestroy();
		finaliseLog("OnDestroy");
	}

	private void OnApplicationQuit()
	{
		finaliseLog("OnApplicationQuit");
	}

	private void OnApplicationFocus(bool isPaused)
	{
		if (isPaused && logFileStream != null)
		{
			writeToLogFile("Application was paused!");
			logFileStream.Flush();
		}
	}

	public void finaliseLog(string endOfFile)
	{
		if (logFileStream != null)
		{
			writeToLogFile("External Message Handler destroyed [" + endOfFile + "]");
			logFileStream.Flush();
			logFileStream = null;
		}
	}

	protected override void Awake()
	{
		if (_instance != null && _instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		base.Awake();
		LOG_FILE_PATH = Application.persistentDataPath + "/UnityLog.txt";
		UnityEngine.Object.DontDestroyOnLoad(this);
		Screen.sleepTimeout = -1;
		_instance = this;
		Debug.Log(LOG_FILE_PATH);
		Application.logMessageReceived += OnLog;
		newLineBytes = new UTF8Encoding(true).GetBytes("\r\n");
		logFileStream = new FileStream(LOG_FILE_PATH, FileMode.Create);
		string newMessage = SessionManager.bambooInfoString().Replace("\n", "\r\n");
		writeToLogFile(newMessage);
		logFileStream.Flush();
	}

	public override void startInitialising()
	{
		_currentState = InitialisationState.FINISHED;
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void OnLog(string message, string stacktrace, LogType type)
	{
		if (stacktrace != null && stacktrace.Length > 0 && stacktrace.IndexOf('\n') != -1)
		{
			writeToLogFile(message, stacktrace.Split('\n')[1]);
		}
		else
		{
			writeToLogFile(message);
		}
		if (logFileStream != null && type == LogType.Exception)
		{
			writeToLogFile("Exception detected! Flushing log file!");
			logFileStream.Flush();
		}
	}

	private void writeToLogFile(string newMessage, string stackTrace = null)
	{
		if (logFileStream != null)
		{
			byte[] bytes = new UTF8Encoding(true).GetBytes(newMessage);
			logFileStream.Write(bytes, 0, bytes.Length);
			logFileStream.Write(newLineBytes, 0, newLineBytes.Length);
			if (stackTrace == null || stackTrace.Length == 0)
			{
				logFileStream.Write(newLineBytes, 0, newLineBytes.Length);
				return;
			}
			bytes = new UTF8Encoding(true).GetBytes(stackTrace);
			logFileStream.Write(bytes, 0, bytes.Length);
			logFileStream.Write(newLineBytes, 0, newLineBytes.Length);
			logFileStream.Write(newLineBytes, 0, newLineBytes.Length);
		}
	}

	private void onVideoComplete()
	{
		Debug.Log("Got On Video Complete From Javascript");
	}

	public void RestartGame(int arg)
	{
		Debug.Log("RestartGame " + arg);
	}

	public void QuitGame(int arg)
	{
		Debug.Log("QuitGame " + arg);
	}

	public void ClearAllSaves()
	{
		if (Application.isPlaying)
		{
			Debug.Log("CLEARING ALL SAVES!!");
			PlayerPrefs.DeleteAll();
		}
	}

	public void UserNotificationDialog_Accept()
	{
		Debug.Log("UserNotificationDialog_Accept");
		UserAccountFacade.EndExternalUIAction();
		if (userNotificationCallback_accept.callback != null)
		{
			Debug.Log("Calling accept callback");
			userNotificationCallback_accept.callback();
		}
		else
		{
			Debug.LogWarning("No Callback Set!");
		}
	}

	public void UserNotificationDialog_Refuse()
	{
		Debug.Log("UserNotificationDialog_Refuse");
		UserAccountFacade.EndExternalUIAction();
		if (userNotificationCallback_refuse.callback != null)
		{
			Debug.Log("Calling refuse callback");
			userNotificationCallback_refuse.callback();
		}
		else
		{
			Debug.LogWarning("No Callback Set!");
		}
	}

	public void UserNotificationDialog(string name)
	{
		Debug.Log("UserNotificationDialog: " + name);
		if (userNotificationCallback_accept.callback != null && userNotificationCallback_accept.name == name)
		{
			userNotificationCallback_accept.callback();
		}
		else if (userNotificationCallback_refuse.callback != null && userNotificationCallback_refuse.name == name)
		{
			userNotificationCallback_refuse.callback();
		}
		UserAccountFacade.EndExternalUIAction();
	}

	public static void OnConnectRequestMessage()
	{
	}

	public static void ShowSpinner()
	{
		if (!_spinnerOn)
		{
		}
	}

	public static void HideSpinner()
	{
		if (_spinnerOn)
		{
		}
	}

	public void SetLanguage(string language)
	{
		Debug.Log("Setting Language to " + language);
		Facades<LocalisationFacade>.Instance.language = language;
	}

	public void ResetLink(Array userinfo)
	{
		if (userinfo != null)
		{
			inputUserId = userinfo.GetValue(0).ToString();
			inputUsername = userinfo.GetValue(1).ToString();
			Debug.Log("Reset Password for : " + inputUserId + " : " + inputUsername);
			Facades<FlowFacade>.Instance.SetFlowCondition("EmailRedirect", true);
			Facades<FlowFacade>.Instance.SetFlowCondition("ResetPassword", true);
		}
	}

	public void DeactivateLink(Array userinfo)
	{
		if (userinfo != null)
		{
			inputUserId = userinfo.GetValue(0).ToString();
			inputUsername = userinfo.GetValue(1).ToString();
			Debug.Log("Reset Password for : " + inputUserId + " : " + inputUsername);
			Facades<FlowFacade>.Instance.SetFlowCondition("EmailRedirect", true);
			Facades<FlowFacade>.Instance.SetFlowCondition("ResetPassword", false);
		}
	}

	public static void ExternalCall(string fn, params object[] args)
	{
		Application.ExternalCall(fn, args);
	}

	private void Update()
	{
	}

	private void LateUpdate()
	{
		if (logFileStream != null)
		{
			logFileStream.Flush();
		}
	}
}
