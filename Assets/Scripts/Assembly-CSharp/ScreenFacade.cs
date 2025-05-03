using System;
using System.Collections.Generic;
using AmuzoEngine;
using LitJson;
using UnityEngine;

public class ScreenFacade : MonoBehaviour, ILocationHandler
{
	private class FlowData
	{
		public string[] _screens;

		public int _pNumScreens
		{
			get
			{
				return (_screens != null) ? _screens.Length : 0;
			}
		}

		public FlowData(JsonData locData)
		{
			Init(locData);
		}

		private void Init(JsonData locData)
		{
			if (locData == null || !locData.IsObject)
			{
				return;
			}
			JsonData jsonData = locData.TryGet("screen");
			if (jsonData == null)
			{
				return;
			}
			if (jsonData.IsString)
			{
				_screens = new string[1] { (string)jsonData };
			}
			else
			{
				if (!jsonData.IsArray)
				{
					return;
				}
				_screens = new string[jsonData.Count];
				for (int i = 0; i < jsonData.Count; i++)
				{
					if (jsonData[i] != null && jsonData[i].IsString)
					{
						_screens[i] = (string)jsonData[i];
					}
				}
			}
		}
	}

	private const bool DO_DEBUG = true;

	private const float POLL_TIME = 1f;

	public static Action _onFlowChangeComplete;

	private int? _flowSceneLoadBlockerHandle;

	private Dictionary<string, ScreenBase> _screens;

	private List<ScreenBase> _activeScreens = new List<ScreenBase>();

	private int _activeScreensLockedCount;

	private Action _onActiveScreensUnlocked;

	private float _pollTimer;

	public bool _pIsActiveScreensLocked
	{
		get
		{
			return _activeScreensLockedCount > 0;
		}
		set
		{
			SetActiveScreensLocked(value);
		}
	}

	public bool _pIsAnyScreenTweening
	{
		get
		{
			for (int i = 0; i < _activeScreens.Count; i++)
			{
				if (_activeScreens[i]._pCurrentTweenType != ScreenBase.ScreenTweenType.Idle)
				{
					return true;
				}
			}
			return false;
		}
	}

	public int locationChangePriority
	{
		get
		{
			return -1;
		}
	}

	public void Awake()
	{
		Facades<ScreenFacade>.Register(this);
		Log("Initialised.");
		_screens = new Dictionary<string, ScreenBase>();
		GameApplicationBinder.BindApplicationNeurons();
		RegisterWithFlowFacade();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void RegisterScreen(ScreenBase screen)
	{
		string text = screen.GetType().ToString();
		_screens[text] = screen;
		Log("Screen Registered [" + text + "]");
	}

	public T FindActiveScreenOfType<T>() where T : class
	{
		if (_activeScreens == null)
		{
			return (T)null;
		}
		if (_activeScreens.Count == 0)
		{
			return (T)null;
		}
		return _activeScreens.Find((ScreenBase s) => s is T) as T;
	}

	public ScreenBase GetScreen(string name)
	{
		return _screens[name];
	}

	public void RegisterWithFlowFacade()
	{
		FlowFacade.AddLocationHandler(this);
	}

	public void SetCurrentlyActiveScreen(ScreenBase screen)
	{
		Log("ScreenFacade::SetCurrentlyActiveScreen( " + screen.GetType().ToString() + " )");
		AddActiveScreen(screen);
	}

	public void OnScreenExitComplete(ScreenBase screen)
	{
		RemoveActiveScreen(screen);
	}

	private void AddActiveScreen(ScreenBase screen)
	{
		Action action = delegate
		{
			ScreenBase item = screen;
			if (_activeScreens != null && !_activeScreens.Contains(item))
			{
				_activeScreens.Add(item);
			}
		};
		if (_pIsActiveScreensLocked)
		{
			_onActiveScreensUnlocked = (Action)Delegate.Combine(_onActiveScreensUnlocked, action);
		}
		else
		{
			action();
		}
	}

	private void RemoveActiveScreen(ScreenBase screen)
	{
		Action action = delegate
		{
			ScreenBase item = screen;
			if (_activeScreens != null && _activeScreens.Contains(item))
			{
				_activeScreens.Remove(item);
			}
		};
		if (_pIsActiveScreensLocked)
		{
			_onActiveScreensUnlocked = (Action)Delegate.Combine(_onActiveScreensUnlocked, action);
		}
		else
		{
			action();
		}
	}

	private void SetActiveScreensLocked(bool wantLocked)
	{
		if (wantLocked)
		{
			_activeScreensLockedCount++;
		}
		else
		{
			_activeScreensLockedCount--;
		}
		if (_activeScreensLockedCount < 0)
		{
			Debug.LogError("[ScreenFacade] Active screens locking error, count = " + _activeScreensLockedCount, base.gameObject);
			_activeScreensLockedCount = 0;
		}
		else if (_activeScreensLockedCount == 0 && _onActiveScreensUnlocked != null)
		{
			_onActiveScreensUnlocked();
			_onActiveScreensUnlocked = null;
		}
	}

	public float ExitCurrentScreenWithFlowChange(string flowChange, bool immediate = false)
	{
		Facades<FlowFacade>.Instance.FollowLink(flowChange);
		return 0f;
	}

	private void ExitScreens(FlowData flowData, Action onComplete, out bool isOnCompleteHandled, out float highestExitTime)
	{
		List<string> list = new List<string>();
		for (int i = 0; i < flowData._pNumScreens; i++)
		{
			list.Add(flowData._screens[i]);
		}
		highestExitTime = 0f;
		for (int j = 0; j < _activeScreens.Count; j++)
		{
			if (!list.Contains(_activeScreens[j].name))
			{
				highestExitTime = Mathf.Max(highestExitTime, _activeScreens[j].getExitTime());
			}
		}
		isOnCompleteHandled = false;
		_pIsActiveScreensLocked = true;
		for (int k = 0; k < _activeScreens.Count; k++)
		{
			if (!list.Contains(_activeScreens[k].name) && _activeScreens[k]._pCurrentTweenType != ScreenBase.ScreenTweenType.TweenOut)
			{
				if (isOnCompleteHandled || _activeScreens[k].getExitTime() != highestExitTime)
				{
					_activeScreens[k].ExitScreen();
					continue;
				}
				_activeScreens[k].ExitScreen(onComplete);
				isOnCompleteHandled = true;
			}
		}
		_pIsActiveScreensLocked = false;
	}

	public void ChangeLocation(string previous, ref string current, string linkName, JsonData linkData, JsonData currentLocationData)
	{
		BlockSceneLoad(true);
		FlowData flowData = new FlowData(currentLocationData);
		bool isOnCompleteHandled;
		float highestExitTime;
		ExitScreens(flowData, delegate
		{
			FinalizeLocationChange(flowData);
		}, out isOnCompleteHandled, out highestExitTime);
		if (!isOnCompleteHandled)
		{
			FinalizeLocationChange(flowData);
		}
	}

	private void FinalizeLocationChange(FlowData flowData)
	{
		BlockSceneLoad(false);
		for (int i = 0; i < flowData._pNumScreens; i++)
		{
			showScreen(flowData._screens[i]);
		}
		if (_onFlowChangeComplete != null)
		{
			_onFlowChangeComplete();
		}
	}

	private void BlockSceneLoad(bool wantBlocked)
	{
		if (Singleton<FlowSceneLoader>._pExists)
		{
			if (wantBlocked)
			{
				Singleton<FlowSceneLoader>._pGet.RequestSceneLoadBlocker(ref _flowSceneLoadBlockerHandle, "ScreenFacade");
			}
			else
			{
				Singleton<FlowSceneLoader>._pGet.ReleaseSceneLoadBlocker(ref _flowSceneLoadBlockerHandle);
			}
		}
	}

	private void showScreen(JsonData screen)
	{
		if (screen == null)
		{
			Log("Screen was null. Cannot show.");
		}
		else
		{
			showScreen((string)screen);
		}
	}

	private void showScreen(string screenName)
	{
		if (!_screens.ContainsKey(screenName))
		{
			Log("Unable to find screen [" + screenName + "]. Cannot show.");
			return;
		}
		ScreenBase screenBase = _screens[screenName];
		if (!_activeScreens.Contains(screenBase))
		{
			_activeScreens.Add(screenBase);
			AddActiveScreen(screenBase);
			screenBase.gameObject.SetActive(true);
			Log("Showing Screen [" + screenName + "]");
			screenBase.ShowScreen();
		}
	}

	public string[] getNamesOfCurrentlyActiveScreens()
	{
		if (_activeScreens == null || _activeScreens.Count == 0)
		{
			return new string[0];
		}
		List<string> list = new List<string>();
		for (int i = 0; i < _activeScreens.Count; i++)
		{
			list.Add(_activeScreens[i].gameObject.name);
		}
		return list.ToArray();
	}

	public ScreenBase[] GetCurrentlyActiveScreens()
	{
		return _activeScreens.ToArray();
	}

	public void Update()
	{
		if (_pollTimer > 0f)
		{
			_pollTimer -= RealTime.deltaTime;
			return;
		}
		_pollTimer = 1f;
		_pIsActiveScreensLocked = true;
		for (int i = 0; i < _activeScreens.Count; i++)
		{
			_activeScreens[i].PollUpdate();
		}
		_pIsActiveScreensLocked = false;
	}

	public static void Log(string message, UnityEngine.Object o = null)
	{
	}
}
