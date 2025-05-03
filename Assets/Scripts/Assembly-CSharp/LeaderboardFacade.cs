using System;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardFacade : InitialisationObject
{
	public const int NO_POSITION = -99999999;

	private const float BAR_HEIGHT = 30f;

	private const float BAR_GAP = 10f;

	private static bool DO_SHOW_DEBUG_INTERFACE;

	private static LeaderboardFacade _instance;

	public int _cacheTimeout = 300;

	public Leaderboard[] _leaderboards;

	private long _timeout;

	private long _currentTicks;

	private string leaderboardName = "Class1";

	private bool doFriendsOnly;

	private int startIdx;

	private int length = 5;

	private DatabaseType currentType;

	private List<LeaderboardEntry> entries;

	public static LeaderboardFacade Instance
	{
		get
		{
			return _instance;
		}
	}

	public long _pCacheTimeout
	{
		get
		{
			return _timeout;
		}
	}

	public long _pCurrentTicks
	{
		get
		{
			return _currentTicks;
		}
	}

	private new void Awake()
	{
		_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		base.Awake();
	}

	private new void OnDestroy()
	{
		_instance = null;
		base.OnDestroy();
	}

	private void Update()
	{
		_currentTicks = ((!(TimeManager.Instance != null)) ? DateTime.Now.Ticks : TimeManager.GetCurrentTime().Ticks);
	}

	public override void startInitialising()
	{
		_timeout = new TimeSpan(0, 0, _cacheTimeout).Ticks;
		_currentState = InitialisationState.FINISHED;
	}

	public void getPosition(DatabaseType dbType, string leaderboardName, string uniqueId, int length, bool isFriendsOnly, Action<List<LeaderboardEntry>> onSuccessCallback, Action<DownloadRequest> onFailCallback)
	{
		Leaderboard leaderboard = getLeaderboard(dbType, leaderboardName, isFriendsOnly);
		PersonalLogs.KayLog("DATABASE POSITION : " + leaderboardName + " : " + isFriendsOnly + " : " + uniqueId);
		leaderboard.getPosition(uniqueId, length, onSuccessCallback, onFailCallback);
	}

	public void getPositions(DatabaseType dbType, string leaderboardName, int startIdx, int length, bool isFriendsOnly, Action<List<LeaderboardEntry>> onSuccessCallback, Action<DownloadRequest> onFailCallback)
	{
		Leaderboard leaderboard = getLeaderboard(dbType, leaderboardName, isFriendsOnly);
		leaderboard.getPositions(startIdx, length, onSuccessCallback, onFailCallback);
	}

	public virtual void leaderboardUpdated(Leaderboard leaderboardThatWasUpdated)
	{
	}

	protected Leaderboard getLeaderboard(DatabaseType dbType, string name, bool isFriendsOnly)
	{
		int i = 0;
		for (int num = _leaderboards.Length; i < num; i++)
		{
			if (_leaderboards[i]._databaseType == dbType && !(_leaderboards[i]._leaderboardName != name) && _leaderboards[i]._isFriendsOnlyLeaderboard == isFriendsOnly)
			{
				return _leaderboards[i];
			}
		}
		return addNewLeaderboard(dbType, name, isFriendsOnly);
	}

	private Leaderboard addNewLeaderboard(DatabaseType dbType, string name, bool isFriendsOnly)
	{
		Leaderboard[] array = new Leaderboard[_leaderboards.Length + 1];
		int num = _leaderboards.Length;
		for (int i = 0; i < num; i++)
		{
			array[i] = _leaderboards[i];
		}
		array[num] = new Leaderboard();
		array[num]._leaderboardName = name;
		array[num]._isFriendsOnlyLeaderboard = isFriendsOnly;
		array[num]._databaseType = dbType;
		_leaderboards = array;
		return _leaderboards[num];
	}

	public void getPosition(string leaderboardName, string uniqueId, int length, bool isFriendsOnly, Action<List<LeaderboardEntry>> onSuccessCallback, Action<DownloadRequest> onFailCallback)
	{
		getPosition(DatabaseFacade.Instance._activeDatabase, leaderboardName, uniqueId, length, isFriendsOnly, onSuccessCallback, onFailCallback);
	}

	public void getPosition(DatabaseType dbType, string leaderboardName, int length, bool isFriendsOnly, Action<List<LeaderboardEntry>> onSuccessCallback, Action<DownloadRequest> onFailCallback)
	{
		string uniqueId = string.Empty;
		if (DatabaseFacade.Instance._databases.ContainsKey(dbType))
		{
			uniqueId = DatabaseFacade.Instance._databases[dbType]._userUniqueId;
		}
		getPosition(dbType, leaderboardName, uniqueId, length, isFriendsOnly, onSuccessCallback, onFailCallback);
	}

	public void getPositions(string leaderboardName, int startIdx, int length, bool isFriendsOnly, Action<List<LeaderboardEntry>> onSuccessCallback, Action<DownloadRequest> onFailCallback)
	{
		getPositions(DatabaseFacade.Instance._activeDatabase, leaderboardName, startIdx, length, isFriendsOnly, onSuccessCallback, onFailCallback);
	}

	private void OnGUI()
	{
		if (!DO_SHOW_DEBUG_INTERFACE || DatabaseFacade.Instance == null || !DatabaseFacade.Instance._pIsReady)
		{
			return;
		}
		float num = 0f;
		Rect position = new Rect(20f, 10f * (num + 1f) + 30f * num, 80f, 30f);
		GUI.Label(position, "Databases");
		num += 1f;
		DatabaseType[] array = (DatabaseType[])Enum.GetValues(typeof(DatabaseType));
		foreach (DatabaseType databaseType in array)
		{
			position = new Rect(10f, 10f * (num + 1f) + 30f * num, 80f, 30f);
			if (currentType == databaseType)
			{
				position.x += 20f;
				GUI.Label(position, databaseType.ToString());
			}
			else if (GUI.Button(position, databaseType.ToString()))
			{
				currentType = databaseType;
			}
			num += 1f;
		}
		position = new Rect(10f, 10f * (num + 1f) + 30f * num, 100f, 30f);
		doFriendsOnly = GUI.Toggle(position, doFriendsOnly, "Friends Only?");
		num = 0f;
		position = new Rect(140f, 10f * (num + 1f) + 30f * num, 80f, 30f);
		GUI.Label(position, "Board Name:");
		position = new Rect(220f, 10f * (num + 1f) + 30f * num, 80f, 30f);
		leaderboardName = GUI.TextField(position, leaderboardName);
		num += 1f;
		position = new Rect(140f, 10f * (num + 1f) + 30f * num, 80f, 30f);
		GUI.Label(position, "StartIdx");
		num += 1f;
		position = new Rect(212f, 10f * (num + 1f) + 30f * num, 16f, 30f);
		GUI.Label(position, startIdx.ToString());
		position = new Rect(140f, 10f * (num + 1f) + 30f * num, 65f, 30f);
		if (GUI.Button(position, "<"))
		{
			startIdx = Mathf.Max(0, startIdx - 1);
		}
		position = new Rect(235f, 10f * (num + 1f) + 30f * num, 65f, 30f);
		if (GUI.Button(position, ">"))
		{
			startIdx++;
		}
		num += 1f;
		position = new Rect(140f, 10f * (num + 1f) + 30f * num, 80f, 30f);
		GUI.Label(position, "Length");
		num += 1f;
		position = new Rect(212f, 10f * (num + 1f) + 30f * num, 16f, 30f);
		GUI.Label(position, length.ToString());
		position = new Rect(140f, 10f * (num + 1f) + 30f * num, 65f, 30f);
		if (GUI.Button(position, "<"))
		{
			length = Mathf.Max(0, length - 1);
		}
		position = new Rect(235f, 10f * (num + 1f) + 30f * num, 65f, 30f);
		if (GUI.Button(position, ">"))
		{
			length++;
		}
		num = 0f;
		position = new Rect(310f, 10f * (num + 1f) + 30f * num, 180f, 30f);
		if (GUI.Button(position, "Make Higscore Request"))
		{
			Instance.getPositions(currentType, leaderboardName, startIdx, length, doFriendsOnly, onHighScoreRequestFinished, onFail);
			entries = null;
		}
		num += 1f;
		position = new Rect(310f, 10f * (num + 1f) + 30f * num, 180f, 30f);
		if (GUI.Button(position, "Make user Score Request"))
		{
			Instance.getPosition(currentType, leaderboardName, DatabaseFacade.Instance._databases[currentType]._userUniqueId, length, doFriendsOnly, onHighScoreRequestFinished, onFail);
			entries = null;
		}
		if (entries != null)
		{
			num = 0f;
			int j = 0;
			for (int count = entries.Count; j < count; j++)
			{
				position = new Rect(20f, 200f + 10f * (num + 1f) + 30f * num, 60f, 30f);
				GUI.Label(position, "#" + entries[j]._position);
				position = new Rect(80f, 200f + 10f * (num + 1f) + 30f * num, 140f, 30f);
				GUI.Label(position, entries[j]._name);
				position = new Rect(220f, 200f + 10f * (num + 1f) + 30f * num, 80f, 30f);
				GUI.Label(position, entries[j].getData("score"));
				num += 1f;
			}
		}
	}

	private void onHighScoreRequestFinished(List<LeaderboardEntry> entries)
	{
		this.entries = entries;
		Debug.Log("Success!");
	}

	private void onFail(DownloadRequest request)
	{
		Debug.Log("Fail");
	}
}
