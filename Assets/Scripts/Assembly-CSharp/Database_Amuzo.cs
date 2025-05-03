using System;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;

[Serializable]
public class Database_Amuzo : Database
{
	private const string DEVICE_ID_PREFS_KEY = "Amuzo_Database_Unique_Id";

	private const string START_KEY = "startIdx";

	private const string NUM_KEY = "numResults";

	private const string TOKEN_KEY = "vid";

	private const string NAME_KEY = "name";

	private const string BOARD_TYPE_KEY = "leaderboardType";

	private const string BY_INDEX_KEY = "byIndex";

	private const string BY_ID_KEY = "byId";

	public const string DEVICE_KEY = "deviceId";

	public const string COMMAND_KEY = "command";

	public const string NEW_AUTH_TOKEN_COMMAND = "init";

	public const string SAVE_USER_COMMAND = "saveUser";

	public const string HIGH_SCORES_COMMAND = "getLeaderboard";

	public const string FRIENDS_SCORES_COMMAND = "getLeaderboard";

	private const string ERROR_RESPONSE_KEY = "error";

	private const string AUTH_TOKEN_RESPONSE_KEY = "id";

	public string _authTokenUrl = "http://www.catoandmacrogame.com/database/actions.php";

	public string _requestUrl = "http://www.catoandmacrogame.com/database/actions.php";

	public string _requestTokenUrl = "http://www.catoandmacrogame.com/database/actions.php";

	private List<DatabaseRequest_Amuzo> _requestsInProgress = new List<DatabaseRequest_Amuzo>();

	private Action<bool> onInitialisationFinishCallback;

	private ObscuredString _authToken;

	public override void initialise(Action<bool> onFinishCallback)
	{
		onInitialisationFinishCallback = (Action<bool>)Delegate.Combine(onInitialisationFinishCallback, onFinishCallback);
		getAuthToken();
	}

	public override void getUserPosition(string leaderboardName, string userId, int length, bool doFriendsOnly, Action<DownloadRequest> onSuccessCallback, Action<DownloadRequest> onFailCallback)
	{
		DatabaseFacade.Log("Making user score request.");
		DatabaseRequest_Amuzo databaseRequest_Amuzo = new DatabaseRequest_Amuzo();
		databaseRequest_Amuzo.requestUrl = _requestUrl;
		databaseRequest_Amuzo._requestTimeout = 10f;
		databaseRequest_Amuzo.doLocalDownload = false;
		databaseRequest_Amuzo.onSuccessCallBack += onSuccessCallback;
		databaseRequest_Amuzo.onFailCallback += onFailCallback;
		databaseRequest_Amuzo.onSuccessCallBack += removeFromInProgress;
		databaseRequest_Amuzo.onFailCallback += removeFromInProgress;
		databaseRequest_Amuzo.formData.Add(new FormDataEntry("command", "getLeaderboard"));
		databaseRequest_Amuzo.formData.Add(new FormDataEntry("deviceId", _userUniqueId));
		databaseRequest_Amuzo.formData.Add(new FormDataEntry("numResults", length.ToString()));
		databaseRequest_Amuzo.formData.Add(new FormDataEntry("leaderboardType", "byIndex"));
		databaseRequest_Amuzo.formData.Add(new FormDataEntry("vid", _authToken));
		databaseRequest_Amuzo.formData.Add(new FormDataEntry("name", DatabaseFacade.Instance._userData._name));
		if (doFriendsOnly)
		{
			addUserDataAsFormData("friends", databaseRequest_Amuzo);
		}
		addUserDataAsFormData("facebookId", databaseRequest_Amuzo);
		databaseRequest_Amuzo.download();
		_requestsInProgress.Add(databaseRequest_Amuzo);
	}

	public override void saveUserData(UserData dataToSave, Action<DownloadRequest> onSuccess, Action<DownloadRequest> onFail)
	{
		DatabaseFacade.Log("Saving user data.");
		DatabaseRequest_Amuzo databaseRequest_Amuzo = new DatabaseRequest_Amuzo();
		databaseRequest_Amuzo.requestUrl = _requestUrl;
		databaseRequest_Amuzo._requestTimeout = 10f;
		databaseRequest_Amuzo.doLocalDownload = false;
		databaseRequest_Amuzo.onSuccessCallBack += onSuccess;
		databaseRequest_Amuzo.onFailCallback += onFail;
		databaseRequest_Amuzo.onSuccessCallBack += removeFromInProgress;
		databaseRequest_Amuzo.onFailCallback += removeFromInProgress;
		databaseRequest_Amuzo.formData.Add(new FormDataEntry("command", "saveUser"));
		if (!string.IsNullOrEmpty(dataToSave._uniqueId))
		{
			databaseRequest_Amuzo.formData.Add(new FormDataEntry("deviceId", dataToSave._uniqueId));
		}
		databaseRequest_Amuzo.formData.Add(new FormDataEntry("vid", _authToken));
		databaseRequest_Amuzo.formData.Add(new FormDataEntry("name", dataToSave._name));
		databaseRequest_Amuzo.formData.Add(new FormDataEntry("facebookId", dataToSave.getData("facebookId")));
		databaseRequest_Amuzo.formData.Add(new FormDataEntry("challenges", dataToSave.getData("challenges")));
		databaseRequest_Amuzo.formData.Add(new FormDataEntry("score", dataToSave.getData("score")));
		databaseRequest_Amuzo.download();
		_requestsInProgress.Add(databaseRequest_Amuzo);
	}

	public override void getHighScores(string leaderboardName, int currentIndex, int length, bool doFriendsOnly, Action<DownloadRequest> onSuccessCallback, Action<DownloadRequest> onFailCallback)
	{
		DatabaseFacade.Log("Making high scores request.");
		DatabaseRequest_Amuzo databaseRequest_Amuzo = new DatabaseRequest_Amuzo();
		databaseRequest_Amuzo.requestUrl = _requestUrl;
		databaseRequest_Amuzo._requestTimeout = 10f;
		databaseRequest_Amuzo.doLocalDownload = false;
		databaseRequest_Amuzo.onSuccessCallBack += onSuccessCallback;
		databaseRequest_Amuzo.onFailCallback += onFailCallback;
		databaseRequest_Amuzo.onSuccessCallBack += removeFromInProgress;
		databaseRequest_Amuzo.onFailCallback += removeFromInProgress;
		databaseRequest_Amuzo.formData.Add(new FormDataEntry("command", "getLeaderboard"));
		databaseRequest_Amuzo.formData.Add(new FormDataEntry("deviceId", _userUniqueId));
		databaseRequest_Amuzo.formData.Add(new FormDataEntry("numResults", length.ToString()));
		databaseRequest_Amuzo.formData.Add(new FormDataEntry("leaderboardType", (currentIndex < 0) ? "byId" : "byIndex"));
		databaseRequest_Amuzo.formData.Add(new FormDataEntry("vid", _authToken));
		databaseRequest_Amuzo.formData.Add(new FormDataEntry("name", DatabaseFacade.Instance._userData._name));
		if (currentIndex >= 0)
		{
			databaseRequest_Amuzo.formData.Add(new FormDataEntry("startIdx", currentIndex.ToString()));
		}
		if (doFriendsOnly)
		{
			addUserDataAsFormData("friends", databaseRequest_Amuzo);
		}
		addUserDataAsFormData("facebookId", databaseRequest_Amuzo);
		databaseRequest_Amuzo.download();
		_requestsInProgress.Add(databaseRequest_Amuzo);
	}

	public static string findIdIn(List<FormDataEntry[]> responses, string idToFind)
	{
		if (responses == null)
		{
			return null;
		}
		for (int i = 0; i < responses.Count; i++)
		{
			for (int j = 0; j < responses[i].Length; j++)
			{
				if (responses[i][j] != null && !(responses[i][j].key != idToFind))
				{
					return responses[i][j].value;
				}
			}
		}
		return null;
	}

	private void addUserDataAsFormData(string key, DatabaseRequest request, bool doEncrypt = true)
	{
		if (DatabaseFacade.Instance._userData.hasData(key))
		{
			request.formData.Add(new FormDataEntry(key, DatabaseFacade.Instance._userData.getData(key), doEncrypt));
		}
	}

	private void initFinished(bool didSucceed = true)
	{
		if (onInitialisationFinishCallback != null)
		{
			onInitialisationFinishCallback(didSucceed);
		}
	}

	private void getAuthToken()
	{
		DatabaseFacade.Log("Getting session authentication token. ");
		DatabaseRequest_Amuzo databaseRequest_Amuzo = new DatabaseRequest_Amuzo();
		databaseRequest_Amuzo._requestTimeout = 10f;
		databaseRequest_Amuzo.requestUrl = _authTokenUrl;
		databaseRequest_Amuzo.doLocalDownload = false;
		databaseRequest_Amuzo.onSuccessCallBack += onGetAuthTokenComplete;
		databaseRequest_Amuzo.onFailCallback += onGetAuthTokenComplete;
		databaseRequest_Amuzo.onSuccessCallBack += removeFromInProgress;
		databaseRequest_Amuzo.onFailCallback += removeFromInProgress;
		databaseRequest_Amuzo.formData.Add(new FormDataEntry("command", "init"));
		databaseRequest_Amuzo.download();
		_requestsInProgress.Add(databaseRequest_Amuzo);
	}

	private void onGetAuthTokenComplete(DownloadRequest request)
	{
		DatabaseRequest_Amuzo databaseRequest_Amuzo = request as DatabaseRequest_Amuzo;
		if (databaseRequest_Amuzo == null || databaseRequest_Amuzo.decryptedResponseVariables == null)
		{
			DatabaseFacade.Log("Error downloading authentication token. " + ((databaseRequest_Amuzo != null) ? "No response variables." : "Request was null"));
			initFinished(false);
			return;
		}
		string text = findIdIn(databaseRequest_Amuzo.decryptedResponseVariables, "id");
		if (string.IsNullOrEmpty(text))
		{
			DatabaseFacade.Log("Error downloading authentication token");
			initFinished(false);
			return;
		}
		_authToken = text;
		DatabaseFacade.Log("Got session authentication token.");
		if (ObscuredPrefs.HasKey("Amuzo_Database_Unique_Id"))
		{
			_userUniqueId = ObscuredPrefs.GetString("Amuzo_Database_Unique_Id");
			DatabaseFacade.Log(string.Concat("Existing device Id found! [", _userUniqueId, "]"));
			initFinished();
		}
		else
		{
			saveUserData(DatabaseFacade.Instance._userData, onDeviceIdRetreived, onDeviceIdRetreived);
		}
	}

	private void onDeviceIdRetreived(DownloadRequest request)
	{
		DatabaseRequest_Amuzo databaseRequest_Amuzo = request as DatabaseRequest_Amuzo;
		if (databaseRequest_Amuzo == null || databaseRequest_Amuzo.decryptedResponseVariables == null)
		{
			DatabaseFacade.Log("Error downloading deviceId.");
			initFinished(false);
			return;
		}
		string text = findIdIn(databaseRequest_Amuzo.decryptedResponseVariables, "error");
		string text2 = findIdIn(databaseRequest_Amuzo.decryptedResponseVariables, "deviceId");
		if (!string.IsNullOrEmpty(text) || text2 == null)
		{
			DatabaseFacade.Log("Error downloading deviceId [" + ((text == null) ? "DEVICE_KEY was NULL" : text) + "]");
			initFinished(false);
			return;
		}
		DatabaseFacade.Log("New device Id created! [" + text2 + "]");
		_userUniqueId = text2;
		ObscuredPrefs.SetString("Amuzo_Database_Unique_Id", _userUniqueId);
		initFinished();
	}

	private void removeFromInProgress(DownloadRequest request)
	{
		DatabaseRequest_Amuzo databaseRequest_Amuzo = request as DatabaseRequest_Amuzo;
		if (databaseRequest_Amuzo != null && _requestsInProgress.Contains(databaseRequest_Amuzo))
		{
			_requestsInProgress.Remove(databaseRequest_Amuzo);
		}
	}

	public void addDebugMenu(ref AmuzoDebugMenu databaseManeu)
	{
	}
}
