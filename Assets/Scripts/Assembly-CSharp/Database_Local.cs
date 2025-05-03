using System;
using System.Collections.Generic;
using GameDefines;
using LitJson;
using UnityEngine;

[Serializable]
public class Database_Local : Database
{
	[Serializable]
	public class LocalLeaderboard
	{
		public string _name;

		public LeaderboardEntry[] _leaderboard;
	}

	private const string SAVE_KEYS = "LOCAL_DATABASE_KEYS";

	private const string SAVE_KEY_TEMPLATE = "LOCLA_DATABASE_ENTRY_{0}";

	public DefaultResponseTemplate[] _defaultResponses;

	public TextAsset[] _JSONLeaderboards;

	public float _baseResponseTime = 1f;

	public float _variableResponseTime = 6f;

	public bool _doKeepChanges = true;

	[Range(0f, 1f)]
	public float _failureRate;

	public LocalLeaderboard[] _leaderboards;

	private List<LeaderboardEntry> extraEntries = new List<LeaderboardEntry>();

	public override void initialise(Action<bool> onFinishCallback)
	{
		_userUniqueId = GlobalDefines._pAmuzoDeviceUniqueId.ToString();
		_leaderboards = new LocalLeaderboard[_JSONLeaderboards.Length];
		int i = 0;
		for (int num = _JSONLeaderboards.Length; i < num; i++)
		{
			_leaderboards[i] = new LocalLeaderboard();
			if (_JSONLeaderboards[i] == null)
			{
				continue;
			}
			JsonData jsonData = Extensions.LoadJson(_JSONLeaderboards[i].text);
			if (!jsonData.IsArray || jsonData.Count == 0)
			{
				continue;
			}
			string name = _JSONLeaderboards[i].name.Replace("Leaderboard_", string.Empty);
			_leaderboards[i]._name = name;
			_leaderboards[i]._leaderboard = new LeaderboardEntry[jsonData.Count];
			int j = 0;
			for (int count = jsonData.Count; j < count; j++)
			{
				JsonData jsonData2 = jsonData[j];
				_leaderboards[i]._leaderboard[j] = new LeaderboardEntry();
				if (jsonData2.Contains("name"))
				{
					_leaderboards[i]._leaderboard[j]._name = jsonData2["name"].ToString();
				}
				if (jsonData2.Contains("uniqueId"))
				{
					_leaderboards[i]._leaderboard[j]._uniqueId = jsonData2["uniqueId"].ToString();
				}
				if (jsonData2.Contains("position"))
				{
					_leaderboards[i]._leaderboard[j].addData("position", jsonData2["position"].ToString());
				}
				if (jsonData2.Contains("score"))
				{
					_leaderboards[i]._leaderboard[j].addData("score", jsonData2["score"].ToString());
				}
			}
		}
		string text = AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.GetString("LOCAL_DATABASE_KEYS", string.Empty);
		if (text.Length != 0)
		{
			string[] array = text.Split(',');
			int k = 0;
			for (int num2 = array.Length; k < num2; k++)
			{
				UserData userData = new UserData();
				userData.LoadFromCSV(AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.GetString(string.Format("LOCLA_DATABASE_ENTRY_{0}", array[k])));
				saveUserData(userData, null, null);
			}
		}
		onFinishCallback(true);
	}

	public override void getUserPosition(string leaderboardName, string userId, int length, bool doFriendsOnly, Action<DownloadRequest> onSuccessCallback, Action<DownloadRequest> onFailCallback)
	{
		DatabaseFacade.Log("LOCAL Get user score.");
		GameObject gameObject = new GameObject("GetUserRequest");
		LocalDatabaseRequest localDatabaseRequest = gameObject.AddComponent<LocalDatabaseRequest>();
		localDatabaseRequest._onFinalSuccess = onSuccessCallback;
		localDatabaseRequest._onFinalFail = onFailCallback;
		localDatabaseRequest._metaData.Add("leaderboardName", leaderboardName);
		localDatabaseRequest._metaData.Add("uniqueId", userId);
		localDatabaseRequest._metaData.Add("length", length.ToString());
		localDatabaseRequest._metaData.Add("friendsOnly", doFriendsOnly.ToString());
		localDatabaseRequest.startRequest(_baseResponseTime + UnityEngine.Random.Range(0f, _variableResponseTime), getUserSuccess, getUserFail, _failureRate);
	}

	public override void getHighScores(string leaderboardName, int currentIndex, int length, bool doFriendsOnly, Action<DownloadRequest> onSuccessCallback, Action<DownloadRequest> onFailCallback)
	{
		DatabaseFacade.Log("LOCAL Get high scores.");
		GameObject gameObject = new GameObject("GetHighScoresRequest");
		LocalDatabaseRequest localDatabaseRequest = gameObject.AddComponent<LocalDatabaseRequest>();
		localDatabaseRequest._onFinalSuccess = onSuccessCallback;
		localDatabaseRequest._onFinalFail = onFailCallback;
		localDatabaseRequest._metaData.Add("leaderboardName", leaderboardName);
		localDatabaseRequest._metaData.Add("currentIdx", currentIndex.ToString());
		localDatabaseRequest._metaData.Add("length", length.ToString());
		localDatabaseRequest._metaData.Add("friendsOnly", doFriendsOnly.ToString());
		localDatabaseRequest.startRequest(_baseResponseTime + UnityEngine.Random.Range(0f, _variableResponseTime), getHighScoresSuccess, getHighScoresFail, _failureRate);
	}

	public override void saveUserData(UserData dataToSave, Action<DownloadRequest> onSuccess, Action<DownloadRequest> onFail)
	{
		DatabaseFacade.Log("LOCAL Saving user data.");
		LeaderboardEntry leaderboardEntry = new LeaderboardEntry();
		leaderboardEntry.clone(dataToSave);
		if (leaderboardEntry._uniqueId == DatabaseFacade.Instance._userData._uniqueId)
		{
			leaderboardEntry._uniqueId = _userUniqueId;
		}
		addToLocalData(leaderboardEntry);
		string data = leaderboardEntry.getData("leaderboardName");
		DatabaseFacade.Log("Saving [" + leaderboardEntry._name + "]'s score of [" + leaderboardEntry._score + "] to leaderboard [" + data + "]");
		LocalLeaderboard leaderboard = getLeaderboard(data);
		if (leaderboard == null)
		{
			if (onFail != null)
			{
				onFail(null);
			}
			return;
		}
		int i = 0;
		for (int num = leaderboard._leaderboard.Length; i < num; i++)
		{
			if (leaderboardEntry._uniqueId != leaderboard._leaderboard[i]._uniqueId)
			{
				continue;
			}
			LeaderboardEntry[] array = new LeaderboardEntry[leaderboard._leaderboard.Length - 1];
			int num2 = 0;
			int j = 0;
			for (int num3 = leaderboard._leaderboard.Length; j < num3; j++)
			{
				if (j != i)
				{
					array[num2] = leaderboard._leaderboard[j];
					num2++;
				}
			}
			leaderboard._leaderboard = array;
			break;
		}
		bool flag = false;
		int num4 = int.Parse(leaderboardEntry.getData("score"));
		int num5 = int.MaxValue;
		int num6 = int.MaxValue;
		int k = 0;
		for (int num7 = leaderboard._leaderboard.Length; k < num7; k++)
		{
			num5 = num6;
			num6 = int.Parse(leaderboard._leaderboard[k].getData("score"));
			if (num4 < num6 || num4 >= num5)
			{
				continue;
			}
			LeaderboardEntry[] array2 = new LeaderboardEntry[leaderboard._leaderboard.Length + 1];
			int num8 = 0;
			int l = 0;
			for (int num9 = array2.Length; l < num9; l++)
			{
				if (l == k)
				{
					array2[l] = leaderboardEntry;
					continue;
				}
				array2[l] = leaderboard._leaderboard[num8];
				num8++;
			}
			leaderboard._leaderboard = array2;
			flag = true;
			break;
		}
		if (!flag)
		{
			LeaderboardEntry[] array3 = new LeaderboardEntry[leaderboard._leaderboard.Length + 1];
			int m = 0;
			for (int num10 = leaderboard._leaderboard.Length; m < num10; m++)
			{
				array3[m] = leaderboard._leaderboard[m];
			}
			array3[array3.Length - 1] = leaderboardEntry;
			leaderboard._leaderboard = array3;
		}
		int n = 0;
		for (int num11 = leaderboard._leaderboard.Length; n < num11; n++)
		{
			leaderboard._leaderboard[n].addData("position", (n + 1).ToString());
		}
		DatabaseFacade.Log("LOCAL User data saved.");
		if (onSuccess != null)
		{
			onSuccess(null);
		}
	}

	private void addToLocalData(LeaderboardEntry dataToSave)
	{
		if (!_doKeepChanges)
		{
			return;
		}
		int i = 0;
		for (int count = extraEntries.Count; i < count; i++)
		{
			if (!(extraEntries[i]._uniqueId != dataToSave._uniqueId))
			{
				if (extraEntries[i]._score <= dataToSave._score)
				{
					return;
				}
				extraEntries.RemoveAt(i);
				break;
			}
		}
		extraEntries.Add(dataToSave);
		string text = string.Empty;
		int j = 0;
		for (int count2 = extraEntries.Count; j < count2; j++)
		{
			text = text + extraEntries[j]._uniqueId + ",";
		}
		text = text.Substring(0, text.Length - 1);
		AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.SetString("LOCAL_DATABASE_KEYS", text);
		AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.SetString(string.Format("LOCLA_DATABASE_ENTRY_{0}", dataToSave._uniqueId), dataToSave.toCSV());
	}

	private void getUserSuccess(LocalDatabaseRequest request)
	{
		DatabaseFacade.Log("LOCAL get user success");
		string leaderboardName = request._metaData["leaderboardName"];
		LocalLeaderboard leaderboard = getLeaderboard(leaderboardName);
		if (leaderboard == null || request._onFinalSuccess == null)
		{
			getUserFail(request);
			return;
		}
		string text = request._metaData["uniqueId"];
		int num = -1;
		int num2 = int.Parse(request._metaData["length"]);
		for (int i = 0; i < leaderboard._leaderboard.Length; i++)
		{
			if (!(leaderboard._leaderboard[i]._uniqueId != text))
			{
				num = ((num2 != 0) ? (i - i % num2) : i);
				break;
			}
		}
		if (num == -1)
		{
			getUserFail(request);
			return;
		}
		DatabaseRequest databaseRequest = new DatabaseRequest();
		num2 = Mathf.Min(leaderboard._leaderboard.Length - num, num2);
		for (int j = num; j < num + num2; j++)
		{
			leaderboard._leaderboard[j]._timestamp = TimeManager.GetCurrentTime().Ticks;
			databaseRequest.responseUserData.Add(leaderboard._leaderboard[j]);
		}
		DatabaseFacade.Log("LOCAL get user success 2");
		request._onFinalSuccess(databaseRequest);
	}

	private void getUserFail(LocalDatabaseRequest request)
	{
		DatabaseFacade.Log("LOCAL get user fail");
		if (request._onFinalFail != null)
		{
			request._onFinalFail(null);
		}
	}

	private void getHighScoresSuccess(LocalDatabaseRequest request)
	{
		DatabaseFacade.Log("LOCAL get high scores success");
		string leaderboardName = request._metaData["leaderboardName"];
		LocalLeaderboard leaderboard = getLeaderboard(leaderboardName);
		if (leaderboard == null)
		{
			getHighScoresFail(request);
		}
		else if (request._onFinalSuccess != null)
		{
			int num = Mathf.Clamp(int.Parse(request._metaData["currentIdx"]), 0, leaderboard._leaderboard.Length);
			int num2 = Mathf.Min(int.Parse(request._metaData["length"]), leaderboard._leaderboard.Length - num);
			DatabaseRequest databaseRequest = new DatabaseRequest();
			for (int i = num; i < num + num2; i++)
			{
				leaderboard._leaderboard[i]._timestamp = TimeManager.GetCurrentTime().Ticks;
				databaseRequest.responseUserData.Add(leaderboard._leaderboard[i]);
			}
			request._onFinalSuccess(databaseRequest);
		}
	}

	private void getHighScoresFail(LocalDatabaseRequest request)
	{
		if (request._onFinalFail != null)
		{
			request._onFinalFail(null);
		}
	}

	private LocalLeaderboard getLeaderboard(string leaderboardName)
	{
		int i = 0;
		for (int num = _leaderboards.Length; i < num; i++)
		{
			if (!(_leaderboards[i]._name != leaderboardName))
			{
				return _leaderboards[i];
			}
		}
		return null;
	}

	public override void addDebugMenu(AmuzoDebugMenu databaseManeu)
	{
		if (_doEnable)
		{
			AmuzoDebugMenu mainMenu = new AmuzoDebugMenu("LOCAL DATABASE");
			for (int i = 0; i < _leaderboards.Length; i++)
			{
				addDebugmenuForLeadeboard(i, ref mainMenu);
			}
			databaseManeu.AddButton(new AmuzoDebugMenuButton(mainMenu));
		}
	}

	private void addDebugmenuForLeadeboard(int index, ref AmuzoDebugMenu mainMenu)
	{
		if (_leaderboards.Length > index)
		{
			AmuzoDebugMenu_TwoInfoColumns amuzoDebugMenu_TwoInfoColumns = new AmuzoDebugMenu_TwoInfoColumns(_leaderboards[index]._name.ToUpper() + " BOARD");
			amuzoDebugMenu_TwoInfoColumns.AddInfoTextFunction(() => getStringForLeaderboard(index, 0, _leaderboards[index]._leaderboard.Length / 2), () => getStringForLeaderboard(index, _leaderboards[index]._leaderboard.Length / 2 + 1, _leaderboards[index]._leaderboard.Length));
			mainMenu.AddButton(new AmuzoDebugMenuButton(amuzoDebugMenu_TwoInfoColumns));
		}
	}

	private string getStringForLeaderboard(int index, int startPos, int endPos)
	{
		string text = string.Empty;
		int i = Mathf.Min(startPos, _leaderboards[index]._leaderboard.Length);
		for (int num = Mathf.Min(endPos, _leaderboards[index]._leaderboard.Length); i < num; i++)
		{
			string text2 = text;
			text = text2 + _leaderboards[index]._leaderboard[i]._position + ". " + _leaderboards[index]._leaderboard[i]._name + ": " + _leaderboards[index]._leaderboard[i]._score + AmuzoDebugMenu.NEW_LINE;
		}
		return text;
	}
}
