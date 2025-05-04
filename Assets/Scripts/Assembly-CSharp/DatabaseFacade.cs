using System;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseFacade : InitialisationObject
{
	public const string FACEBOOK_KEY = "facebookId";

	public const string FRIENDS_KEY = "friends";

	public const string CHALLENGES_KEY = "challenges";

	public const string SCORE_KEY = "score";

	public const string POSITION_KEY = "position";

	public const string LEADERBOARD_NAME_KEY = "leaderboardName";

	private static bool DO_DEBUG = true;

	private static DatabaseFacade _instance;

	[Space(10f)]
	public Database_Amuzo _amuzoDatabaseConfig;

	[Space(10f)]
	public Database_LEGOID _legoIdDatabaseConfig;

	[Space(10f)]
	public Database_Local _localDatabaseConfig;

	[Space(10f)]
	public DatabaseType _activeDatabase;

	[Space(10f)]
	public UserData _userData;

	public Dictionary<DatabaseType, Database> _databases = new Dictionary<DatabaseType, Database>();

	private List<Database> _allDatabases;

	private bool _isDatabaseReady;

	private int _initIndex = -1;

	public static DatabaseFacade Instance
	{
		get
		{
			return _instance;
		}
	}

	public bool _pIsReady
	{
		get
		{
			return _isDatabaseReady;
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
		base.OnDestroy();
		_instance = null;
	}

	public override void startInitialising()
	{
		if (_currentState == InitialisationState.INITIALISING)
		{
			Log("Already initalising. Ignoring new request to start.");
			return;
		}
		Log("Initalising...");
		_currentState = InitialisationState.INITIALISING;
		_allDatabases = new List<Database>();
		_allDatabases.Add(_amuzoDatabaseConfig);
		_allDatabases.Add(_legoIdDatabaseConfig);
		_allDatabases.Add(_localDatabaseConfig);
		onInitialisationComplete(true);
	}

	public void getUserPosition(DatabaseType dbType, string leaderboardName, string userId, int length, bool doFriendsOnly, Action<DownloadRequest> onSuccessCallback, Action<DownloadRequest> onFailCallback)
	{
		Log("Get user request recieved.");
		if (!_databases.ContainsKey(dbType) || !_pIsReady)
		{
			Log(_pIsReady ? string.Concat("Database [", dbType, "] is not enabled!") : "Database was not ready!");
			if (onFailCallback != null)
			{
				onFailCallback(null);
			}
		}
		else
		{
			_databases[dbType].getUserPosition(leaderboardName, userId, length, doFriendsOnly, onSuccessCallback, onFailCallback);
		}
	}

	public void getHighScores(DatabaseType dbType, string leaderboardName, int currentIndex, int length, bool doFriendsOnly, Action<DownloadRequest> onSuccessCallback, Action<DownloadRequest> onFailCallback)
	{
		Log("Get high score request recieved.");
		if (!_databases.ContainsKey(dbType) || !_pIsReady)
		{
			if (onFailCallback != null)
			{
				Log(_pIsReady ? string.Concat("Database [", dbType, "] is not enabled!") : "Database was not ready!");
				onFailCallback(null);
			}
		}
		else
		{
			_databases[dbType].getHighScores(leaderboardName, currentIndex, length, doFriendsOnly, onSuccessCallback, onFailCallback);
		}
	}

	public void saveUser(DatabaseType dbType, UserData userToSave, Action<DownloadRequest> onSuccessCallback, Action<DownloadRequest> onFailCallback)
	{
		Log("Save user request recieved.");
		if (!_databases.ContainsKey(dbType) || !_pIsReady)
		{
			if (onFailCallback != null)
			{
				Log(_pIsReady ? string.Concat("Database [", dbType, "] is not enabled!") : "Database was not ready!");
				onFailCallback(null);
			}
		}
		else
		{
			_databases[dbType].saveUserData(userToSave, onSuccessCallback, onFailCallback);
		}
	}

	private void onInitialisationComplete(bool isInitialised)
	{
		if (_initIndex >= 0 && isInitialised)
		{
			Log(string.Concat("Database [", _allDatabases[_initIndex]._type, "] was initilaised."));
			_databases.Add(_allDatabases[_initIndex]._type, _allDatabases[_initIndex]);
		}
		_initIndex++;
		if (_initIndex < _allDatabases.Count)
		{
			if (_allDatabases[_initIndex] == null || !_allDatabases[_initIndex]._doEnable)
			{
				onInitialisationComplete(false);
			}
			else
			{
				_allDatabases[_initIndex].initialise(onInitialisationComplete);
			}
		}
		else
		{
			AddDebugMenu();
			_isDatabaseReady = true;
			_currentState = InitialisationState.FINISHED;
			Log("... initialisation complete.");
		}
	}

	public static void Log(string message, UnityEngine.Object o = null)
	{
		if (DO_DEBUG)
		{
			Debug.Log("DatabaseFacade: " + message, o);
		}
	}

	private void AddDebugMenu()
	{
		AmuzoDebugMenu amuzoDebugMenu = new AmuzoDebugMenu("DATABASE FACADE");
		foreach (KeyValuePair<DatabaseType, Database> database in _databases)
		{
			if (database.Value._doEnable)
			{
				database.Value.addDebugMenu(amuzoDebugMenu);
			}
		}
		AmuzoDebugMenuManager.RegisterRootDebugMenu(amuzoDebugMenu);
	}

	public void getUserPosition(string leaderboardName, string userId, int length, bool doFriendsOnly, Action<DownloadRequest> onSuccessCallback, Action<DownloadRequest> onFailCallback)
	{
		getUserPosition(_activeDatabase, leaderboardName, userId, length, doFriendsOnly, onSuccessCallback, onFailCallback);
	}

	public void getHighScores(string leaderboardName, int currentIndex, int length, bool doFriendsOnly, Action<DownloadRequest> onSuccessCallback, Action<DownloadRequest> onFailCallback)
	{
		getHighScores(_activeDatabase, leaderboardName, currentIndex, length, doFriendsOnly, onSuccessCallback, onFailCallback);
	}

	public void saveUser(UserData userToSave, Action<DownloadRequest> onSuccessCallback, Action<DownloadRequest> onFailCallback)
	{
		saveUser(_activeDatabase, userToSave, onSuccessCallback, onFailCallback);
	}

	public void saveUserToAllActiveDatabases(UserData userToSave, Action<DownloadRequest> onSuccessCallback, Action<DownloadRequest> onFailCallback)
	{
		foreach (KeyValuePair<DatabaseType, Database> database in _databases)
		{
			if (database.Value._doEnable)
			{
				saveUser(database.Key, userToSave, onSuccessCallback, onFailCallback);
			}
		}
	}
}
