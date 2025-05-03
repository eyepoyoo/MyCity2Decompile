using System;
using System.Collections.Generic;
using System.Globalization;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

public class TimeManager : InitialisationObject
{
	private const string LOG_PREFIX = "TimeManager: ";

	private const string SAVE_KEY = "TimeEvent_";

	private const string TIME_KEY = "_Time";

	private const string START_KEY = "_StartTime";

	private const string ID_KEY = "_Id";

	private static bool DO_DEBUG = true;

	private static TimeManager _instance;

	public Action<string> OnTimeEvent;

	public string _timeCheckUrl;

	public bool _doAllowFaliedTimeCheckToProceed;

	public bool _doAutoFail;

	private ObscuredLong _gameStartTime;

	private ObscuredFloat _startTimeMod;

	private DownloadRequest _fetchCurrentTimeRequest;

	private List<TimeEvent> _timeEvents = new List<TimeEvent>();

	private ObscuredBool _hasValidatedTime = false;

	private ObscuredBool _doUpdateEvents = false;

	private DateTime _currentTime;

	private long _tempLong;

	private int _lastUpdate = -1;

	public static TimeManager Instance
	{
		get
		{
			return _instance;
		}
	}

	public bool _pHasValidatedTime
	{
		get
		{
			return _hasValidatedTime;
		}
	}

	public static string FormatTime(int seconds, bool doConcatenate = false, bool doForceTwoDecimalPlaces = true, bool doUseTimeChars = true)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
		string text = string.Empty;
		if (timeSpan.TotalDays >= 1.0 && (timeSpan.TotalDays != 0.0 || !doConcatenate))
		{
			text = text + ((!doForceTwoDecimalPlaces) ? timeSpan.Days.ToString() : timeSpan.Days.ToString("D2")) + ((!doUseTimeChars) ? string.Empty : "d");
		}
		if (timeSpan.TotalHours >= 1.0 && (timeSpan.Hours != 0 || !doConcatenate))
		{
			text = text + ((text.Length <= 0) ? string.Empty : ":") + ((!doForceTwoDecimalPlaces) ? timeSpan.Hours.ToString() : timeSpan.Hours.ToString("D2")) + ((!doUseTimeChars) ? string.Empty : "h");
		}
		if (timeSpan.TotalMinutes >= 1.0 && (timeSpan.Minutes != 0 || !doConcatenate))
		{
			text = text + ((text.Length <= 0) ? string.Empty : ":") + ((!doForceTwoDecimalPlaces) ? timeSpan.Minutes.ToString() : timeSpan.Minutes.ToString("D2")) + ((!doUseTimeChars) ? string.Empty : "m");
		}
		if (timeSpan.TotalSeconds >= 1.0 && (timeSpan.Seconds != 0 || !doConcatenate))
		{
			text = text + ((text.Length <= 0) ? string.Empty : ":") + ((!doForceTwoDecimalPlaces) ? timeSpan.Seconds.ToString() : timeSpan.Seconds.ToString("D2")) + ((!doUseTimeChars) ? string.Empty : "s");
		}
		return text;
	}

	public static DateTime GetCurrentTime()
	{
		if (Instance == null || !Instance._pHasValidatedTime)
		{
			return DateTime.Now;
		}
		return Instance._currentTime;
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
		if (InitialisationFacade.Instance == null)
		{
			startInitialising();
			return;
		}
		InitialisationFacade.Instance.addToQueue(this);
		_currentState = InitialisationState.WAITING_TO_START;
	}

	protected override void OnDestroy()
	{
		if (!(_instance != this))
		{
			_instance = null;
		}
	}

	private void Update()
	{
		if (_currentState == InitialisationState.FINISHED)
		{
			updateCurrentSession();
			updateTimeEvents();
		}
	}

	public override void startInitialising()
	{
		if (DO_DEBUG)
		{
			Debug.Log("TimeManager: Init started.");
		}
		_currentState = InitialisationState.INITIALISING;
		startNewCurrentTimeRequest();
		loadState();
	}

	public bool HasTimeEvent(string id)
	{
		for (int i = 0; i < _timeEvents.Count; i++)
		{
			if (!(_timeEvents[i].eventId != (ObscuredString)id))
			{
				return true;
			}
		}
		return false;
	}

	public bool HasTimeEventEndingWith(string id)
	{
		for (int i = 0; i < _timeEvents.Count; i++)
		{
			if (_timeEvents[i].eventId.ToString().EndsWith(id))
			{
				return true;
			}
		}
		return false;
	}

	public float GetDecimalProgress(string id)
	{
		if (!HasTimeEvent(id))
		{
			return -1f;
		}
		for (int i = 0; i < _timeEvents.Count; i++)
		{
			if (!(_timeEvents[i].eventId != (ObscuredString)id))
			{
				DateTime dateTime = new DateTime(_timeEvents[i].eventTime);
				TimeSpan timeSpan = dateTime - _currentTime;
				DateTime dateTime2 = new DateTime(_timeEvents[i].eventStartTime);
				TimeSpan timeSpan2 = dateTime - dateTime2;
				return 1f - Mathf.Clamp01((float)timeSpan.TotalSeconds / (float)timeSpan2.TotalSeconds);
			}
		}
		return -1f;
	}

	public float GetSecondsRemaining(string id)
	{
		if (!HasTimeEvent(id))
		{
			return -1f;
		}
		for (int i = 0; i < _timeEvents.Count; i++)
		{
			if (!(_timeEvents[i].eventId != (ObscuredString)id))
			{
				DateTime dateTime = new DateTime(_timeEvents[i].eventTime);
				return (float)(dateTime - _currentTime).TotalSeconds;
			}
		}
		return -1f;
	}

	public void AddTimeEvent(TimeEvent newEvent)
	{
		if (newEvent == null)
		{
			if (DO_DEBUG)
			{
				Debug.Log("TimeManager: New event was null.");
			}
			return;
		}
		_tempLong = newEvent.eventTime;
		if (_timeEvents.Count == 0 || _tempLong > (long)_timeEvents[_timeEvents.Count - 1].eventTime)
		{
			if (DO_DEBUG)
			{
				Debug.Log("TimeManager: New event [" + (string)newEvent.eventId + "]. Added at index [" + _timeEvents.Count + "].");
			}
			_timeEvents.Add(newEvent);
			saveState();
			return;
		}
		for (int i = 0; i < _timeEvents.Count; i++)
		{
			if (_tempLong <= (long)_timeEvents[i].eventTime)
			{
				if (DO_DEBUG)
				{
					Debug.Log("TimeManager: New event [" + (string)newEvent.eventId + "]. Inserted at index [" + i + "/" + (_timeEvents.Count - 1) + "].");
				}
				_timeEvents.Insert(i, newEvent);
				saveState();
				break;
			}
		}
	}

	public void removeTimeEvent(string id)
	{
		for (int i = 0; i < _timeEvents.Count; i++)
		{
			if (!(_timeEvents[i].eventId != (ObscuredString)id))
			{
				_timeEvents.RemoveAt(i);
				saveState();
				break;
			}
		}
	}

	public void delayTimeEvent(string id, int secondsDelay)
	{
		TimeEvent timeEvent = null;
		for (int i = 0; i < _timeEvents.Count; i++)
		{
			if (!(_timeEvents[i].eventId != (ObscuredString)id))
			{
				timeEvent = _timeEvents[i];
				_timeEvents.RemoveAt(i);
				break;
			}
		}
		if (timeEvent != null)
		{
			TimeSpan value = new TimeSpan(0, 0, secondsDelay);
			DateTime dateTime = new DateTime(timeEvent.eventTime);
			DateTime dateTime2 = new DateTime(timeEvent.eventStartTime);
			dateTime = dateTime.Add(value);
			timeEvent.eventStartTime = dateTime2.Add(value).Ticks;
			timeEvent.eventTime = dateTime.Ticks;
			AddTimeEvent(timeEvent);
		}
	}

	public void CancelAllEvents()
	{
		_timeEvents.Clear();
	}

	private void loadState()
	{
		if (DO_DEBUG)
		{
			Debug.Log("TimeManager: Loading events.");
		}
		_timeEvents.Clear();
		int num = 0;
		while (ObscuredPrefs.HasKey("TimeEvent_" + num + "_Id"))
		{
			TimeEvent timeEvent = new TimeEvent();
			timeEvent.eventId = ObscuredPrefs.GetString("TimeEvent_" + num + "_Id");
			timeEvent.eventStartTime = ObscuredPrefs.GetLong("TimeEvent_" + num + "_StartTime");
			timeEvent.eventTime = ObscuredPrefs.GetLong("TimeEvent_" + num + "_Time");
			_timeEvents.Add(timeEvent);
			if (DO_DEBUG)
			{
				Debug.Log(string.Concat("TimeManager: Loaded event in progress [", timeEvent.eventId, "]."));
			}
			num++;
		}
	}

	private void saveState()
	{
		if (_currentState == InitialisationState.FINISHED)
		{
			clearPlayerPrefs();
			for (int i = 0; i < _timeEvents.Count; i++)
			{
				ObscuredPrefs.SetString("TimeEvent_" + i + "_Id", _timeEvents[i].eventId);
				ObscuredPrefs.SetLong("TimeEvent_" + i + "_StartTime", _timeEvents[i].eventStartTime);
				ObscuredPrefs.SetLong("TimeEvent_" + i + "_Time", _timeEvents[i].eventTime);
			}
		}
	}

	private void clearPlayerPrefs()
	{
		if (_currentState == InitialisationState.FINISHED)
		{
			for (int i = 0; ObscuredPrefs.HasKey("TimeEvent_" + i + "_Id") || i < 10; i++)
			{
				ClearPref("TimeEvent_" + i + "_Id");
				ClearPref("TimeEvent_" + i + "_StartTime");
				ClearPref("TimeEvent_" + i + "_Time");
			}
		}
	}

	private void updateCurrentSession()
	{
		float f = Time.realtimeSinceStartup - (float)_startTimeMod;
		if (_lastUpdate != Mathf.FloorToInt(f))
		{
			_lastUpdate = Mathf.FloorToInt(f);
			_doUpdateEvents = true;
			_currentTime = new DateTime(_gameStartTime);
			_currentTime = _currentTime.Add(new TimeSpan(0, 0, _lastUpdate));
		}
	}

	private void updateTimeEvents()
	{
		if (_timeEvents.Count == 0 || !_doUpdateEvents)
		{
			return;
		}
		_doUpdateEvents = false;
		_tempLong = _currentTime.Ticks;
		bool flag = false;
		while (_timeEvents.Count > 0 && (long)_timeEvents[0].eventTime <= _tempLong)
		{
			if (DO_DEBUG)
			{
				Debug.Log(string.Concat("TimeManager: Time event complete [", _timeEvents[0].eventId, "]."));
			}
			string obj = _timeEvents[0].eventId;
			_timeEvents.RemoveAt(0);
			if (OnTimeEvent != null)
			{
				OnTimeEvent(obj);
			}
			flag = true;
		}
		if (flag)
		{
			saveState();
		}
	}

	private void startNewCurrentTimeRequest()
	{
		if (string.IsNullOrEmpty(_timeCheckUrl))
		{
			if (DO_DEBUG)
			{
				Debug.Log("TimeManager: Time check url was NULL or empty!");
			}
			OnFetchTimeFail(null);
			return;
		}
		if (DO_DEBUG)
		{
			Debug.Log("TimeManager: Starting new time check.");
		}
		_hasValidatedTime = false;
		_fetchCurrentTimeRequest = new DownloadRequest_RemoteOnly();
		_fetchCurrentTimeRequest.requestUrl = _timeCheckUrl;
		_fetchCurrentTimeRequest.doDebug = true;
		_fetchCurrentTimeRequest.onSuccessCallBack += OnFetchTimeSuccess;
		_fetchCurrentTimeRequest.onFailCallback += OnFetchTimeFail;
		if (_doAutoFail)
		{
			OnFetchTimeFail(_fetchCurrentTimeRequest);
		}
		else
		{
			_fetchCurrentTimeRequest.download();
		}
	}

	private void OnFetchTimeSuccess(DownloadRequest download)
	{
		if (!download._pHasResponse)
		{
			if (DO_DEBUG)
			{
				Debug.Log("TimeManager: Time check failed! Empty or missing response text.");
			}
			OnFetchTimeFail(download);
			return;
		}
		DateTime result = default(DateTime);
		if (!DateTime.TryParse(download._pResponseString, new CultureInfo("en-GB"), DateTimeStyles.None, out result))
		{
			if (DO_DEBUG)
			{
				Debug.Log("TimeManager: Time check failed! Unable to parse [" + download._pResponseString + "].");
			}
			OnFetchTimeFail(download);
			return;
		}
		if (DO_DEBUG)
		{
			Debug.Log("TimeManager: Current time [" + result.ToString() + "]. Month [" + result.Month + "].");
		}
		_gameStartTime = result.Ticks;
		_startTimeMod = Time.realtimeSinceStartup;
		_hasValidatedTime = true;
		if (DO_DEBUG)
		{
			Debug.Log("TimeManager: Time check succeded!");
		}
		OnFetchComplete();
	}

	private void OnFetchTimeFail(DownloadRequest download)
	{
		if (download != null && !string.IsNullOrEmpty(download._pErrorString) && DO_DEBUG)
		{
			Debug.Log("TimeManager: Time check failed! Error [" + download._pErrorString + "].");
		}
		if (DO_DEBUG)
		{
			Debug.Log("TimeManager: Using DateTime.Now. Current time [" + DateTime.Now.ToString() + "]. Month [" + DateTime.Now.Month + "]");
		}
		_gameStartTime = DateTime.Now.Ticks;
		_startTimeMod = Time.realtimeSinceStartup;
		_hasValidatedTime = _doAllowFaliedTimeCheckToProceed;
		OnFetchComplete();
	}

	private void OnFetchComplete()
	{
		_fetchCurrentTimeRequest = null;
		if (DO_DEBUG)
		{
			Debug.Log("TimeManager: Init finished.");
		}
		_currentState = InitialisationState.FINISHED;
	}

	private void ClearPref(string key)
	{
		if (ObscuredPrefs.HasKey(key))
		{
			ObscuredPrefs.DeleteKey(key);
		}
	}
}
