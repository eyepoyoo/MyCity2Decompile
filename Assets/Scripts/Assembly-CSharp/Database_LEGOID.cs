using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using GameDefines;
using UnityEngine;

[Serializable]
public class Database_LEGOID : Database
{
	public class SaveRequest
	{
		public UserData dataToSave;

		public int attempt;

		public Action<DownloadRequest> onSuccess;

		public Action<DownloadRequest> onFail;
	}

	public class PositionRequest
	{
		public bool isUserPosition;

		public string leaderboardName;

		public string userId;

		public int currentIndex;

		public int length;

		public bool doFriendsOnly;

		public Action<DownloadRequest> onSuccess;

		public Action<DownloadRequest> onFail;
	}

	[Serializable]
	public class LeaderboardNameToGameId
	{
		public string _leaderboardName;

		public string _legoIdGameId;
	}

	private const string HIGHSCORES_TEST_TOKEN_URL = "https://wwwsecure.lego.com/legacygames/services/token.svc/get";

	private const string HIGHSCORES_TEST_URL = "https://wwwsecure.lego.com/legacygames/services/highscoreV2.svc/updatescore/{gameid}/{score}/{token}";

	private const string GET_HIGH_SCORE_ENDPOINT = "urls/legoservices/getHighscore";

	private const string GET_TOP_HIGH_SCORES_ENDPOINT = "urls/legoservices/getTopHighscores";

	private const string SET_HIGHS_CORE_ENDPOINT = "urls/legoservices/saveHighscore";

	private const string GET_TOKEN_ENDPOINT = "urls/legoservices/getToken";

	private const string KEY_HIGH_SCORE_ENDPOINT = "highscore-get";

	private const string KEY_TOP_HIGH_SCORES_ENDPOINT = "highscore-leaderboard";

	private const string KEY_MY_TOP_HIGH_SCORES_ENDPOINT = "highscore-myleaderboard";

	private const string KEY_HIGHS_CORE_ENDPOINT = "highscore-save";

	private const string KEY_TOKEN_ENDPOINT = "token-get";

	private const string GET_TOKEN_RESPONSE = "response";

	private const string HASH_KEY = "calculatedhash";

	private const string GAME_ID_KEY = "{gameid}";

	private const string AMMOUNT_KEY = "{amount}";

	private const string SCORE_KEY = "{score}";

	private const string TOKEN_KEY = "{token}";

	private const int NUM_RETRYS = 3;

	private string _hashkey = "d5719003-c127-4d6b-8a6a-cb1b15055e59";

	private byte[] _hashKeyBytes;

	public LeaderboardNameToGameId[] _leaderboardGameIds;

	private Action<DownloadRequest> onGetUserSuccessCallback;

	private Action<DownloadRequest> onGetUserFailCallback;

	private Action<DownloadRequest> onGetHighScoresSuccessCallback;

	private Action<DownloadRequest> onGetHighScoresFailCallback;

	private List<SaveRequest> _saveRequestQueue = new List<SaveRequest>();

	private List<PositionRequest> _positionRequestQueue = new List<PositionRequest>();

	private SaveRequest _saveRequestInProgress;

	private PositionRequest _positionRequestInProgress;

	public bool _pIsRequestInProgress
	{
		get
		{
			return _positionRequestInProgress != null || _saveRequestInProgress != null;
		}
	}

	public bool _pIsInitialised
	{
		get
		{
			return LEGOID._pInstance != null && LEGOID._pInstance._pSetUp && LEGOID._pInstance._pAuthenticated && LEGOID._pInstance._pLoginStatus == LEGOID.ELoginStatus.LOGIN_SUCCESS;
		}
	}

	public Database_LEGOID()
	{
		AmuzoLEGOIDCallbackHandler._onUserNameUpdated = (Action)Delegate.Combine(AmuzoLEGOIDCallbackHandler._onUserNameUpdated, new Action(onUserNameUpdated));
		_hashKeyBytes = Encoding.Unicode.GetBytes(_hashkey);
		_hashkey = null;
	}

	public override void initialise(Action<bool> onFinishCallback)
	{
		if (onFinishCallback != null)
		{
			onFinishCallback(true);
		}
	}

	public override void getUserPosition(string leaderboardName, string userId, int length, bool doFriendsOnly, Action<DownloadRequest> onSuccessCallback, Action<DownloadRequest> onFailCallback)
	{
		if (!_pIsInitialised)
		{
			bool flag = LEGOID._pInstance == null;
			bool flag2 = false;
			if (!flag)
			{
				flag2 = LEGOID._pInstance._pAuthenticated;
			}
			DatabaseFacade.Log("LEGOIDFacade is null or has not been set up or we are not signed in! LEGOID Null? " + flag + " isAuth? " + flag2 + " loginStatus=" + LEGOID._pInstance._pLoginStatus);
			if (onFailCallback != null)
			{
				onFailCallback(null);
			}
		}
		else
		{
			PositionRequest positionRequest = new PositionRequest();
			positionRequest.isUserPosition = true;
			positionRequest.leaderboardName = leaderboardName;
			positionRequest.userId = userId;
			positionRequest.length = length;
			positionRequest.doFriendsOnly = doFriendsOnly;
			positionRequest.onSuccess = onSuccessCallback;
			positionRequest.onFail = onFailCallback;
			startPositionRequest(positionRequest);
		}
	}

	public override void getHighScores(string leaderboardName, int currentIndex, int length, bool doFriendsOnly, Action<DownloadRequest> onSuccessCallback, Action<DownloadRequest> onFailCallback)
	{
		if (!_pIsInitialised)
		{
			bool flag = LEGOID._pInstance == null;
			bool flag2 = false;
			if (!flag)
			{
				flag2 = LEGOID._pInstance._pAuthenticated;
			}
			DatabaseFacade.Log("LEGOIDFacade is null or has not been set up or we are not signed in! LEGOID Null? " + flag + " isAuth? " + flag2 + " loginStatus=" + LEGOID._pInstance._pLoginStatus);
			if (onFailCallback != null)
			{
				onFailCallback(null);
			}
		}
		else
		{
			PositionRequest positionRequest = new PositionRequest();
			positionRequest.isUserPosition = false;
			positionRequest.leaderboardName = leaderboardName;
			positionRequest.currentIndex = currentIndex;
			positionRequest.length = length;
			positionRequest.doFriendsOnly = doFriendsOnly;
			positionRequest.onSuccess = onSuccessCallback;
			positionRequest.onFail = onFailCallback;
			startPositionRequest(positionRequest);
		}
	}

	private void startPositionRequest(PositionRequest nextRequest)
	{
		if (_pIsRequestInProgress)
		{
			_positionRequestQueue.Add(nextRequest);
			return;
		}
		_positionRequestInProgress = nextRequest;
		string text = ((!GlobalDefines._pUseLegoSDK) ? LEGOID._pInstance.GetStringFromConfigXML((!nextRequest.isUserPosition) ? "urls/legoservices/getTopHighscores" : "urls/legoservices/getHighscore") : LEGOSDKAmuzo._pInstance.GetEndpointFromApplicationConfig((!nextRequest.isUserPosition) ? "highscore-leaderboard" : "highscore-get"));
		if (string.IsNullOrEmpty(text))
		{
			DatabaseFacade.Log("Endpoint was not found! Unable to make request.");
			if (nextRequest.onFail != null)
			{
				nextRequest.onFail(null);
			}
			finishedPositionRequest();
		}
		text = text.Replace("{gameid}", getGameId(nextRequest.leaderboardName));
		if (nextRequest.isUserPosition)
		{
			onGetUserSuccessCallback = nextRequest.onSuccess;
			onGetUserFailCallback = nextRequest.onFail;
		}
		else
		{
			text = text.Replace("{amount}", Mathf.Clamp(nextRequest.currentIndex + nextRequest.length, 0, 100).ToString());
			onGetHighScoresSuccessCallback = nextRequest.onSuccess;
			onGetHighScoresFailCallback = nextRequest.onFail;
		}
		DatabaseFacade.Log("Endpoint used [" + text + "]");
		if (nextRequest.isUserPosition)
		{
			addLEGOIDCallbacksForGetUser();
		}
		else
		{
			addLEGOIDCallbacksForGetHighScores();
		}
		LEGOSDKFacade.Instance.GetRequest(text, "application/xml");
	}

	public override void saveUserData(UserData dataToSave, Action<DownloadRequest> onSuccess, Action<DownloadRequest> onFail)
	{
		if (!_pIsInitialised)
		{
			DatabaseFacade.Log("LEGOIDFacade is null or has not been set up or we are not signed in!");
			if (onFail != null)
			{
				onFail(null);
			}
		}
		else
		{
			SaveRequest saveRequest = new SaveRequest();
			saveRequest.dataToSave = dataToSave;
			saveRequest.onSuccess = onSuccess;
			saveRequest.onFail = onFail;
			saveUserData(saveRequest);
		}
	}

	private void saveUserData(SaveRequest newSaveRequest)
	{
		if (_pIsRequestInProgress)
		{
			_saveRequestQueue.Add(newSaveRequest);
			return;
		}
		DatabaseFacade.Log("LEGOID Saving user data.");
		_saveRequestInProgress = newSaveRequest;
		_saveRequestInProgress.attempt++;
		if (!newSaveRequest.dataToSave.hasData("leaderboardName"))
		{
			DatabaseFacade.Log("LEGOID Not able to save non-leaderboard data at this time.");
			if (_saveRequestInProgress.onFail != null)
			{
				_saveRequestInProgress.onFail(null);
			}
			finishSavingUser();
		}
		else
		{
			string requestURLString = ((!GlobalDefines._pUseLegoSDK) ? LEGOID._pInstance.GetStringFromConfigXML("urls/legoservices/getToken") : LEGOSDKAmuzo._pInstance.GetEndpointFromApplicationConfig("token-get"));
			addLEGOIDCallbacksForToken();
			LEGOSDKFacade.Instance.GetRequest(requestURLString, "application/xml");
		}
	}

	public void saveUserDataPart2(DownloadRequest request)
	{
		saveUserDataPart2(request._pResponseString);
	}

	public void saveUserDataPart2(string responseBody)
	{
		responseBody = processResponse(responseBody);
		if (string.IsNullOrEmpty(responseBody))
		{
			DatabaseFacade.Log("Download response was null or empty!");
			if (_saveRequestInProgress.onFail != null)
			{
				_saveRequestInProgress.onFail(null);
			}
			finishSavingUser();
			return;
		}
		removeLEGOIDCallbacksForToken();
		MonoXmlElement monoXmlElement = Xml.Load(responseBody);
		monoXmlElement = monoXmlElement.GetChildAt("response");
		string text = string.Empty;
		if (monoXmlElement != null)
		{
			text = monoXmlElement._pText;
		}
		if (string.IsNullOrEmpty(text))
		{
			DatabaseFacade.Log("Unable to extract save token from [" + responseBody + "]");
			if (_saveRequestInProgress.onFail != null)
			{
				_saveRequestInProgress.onFail(null);
			}
			finishSavingUser();
			return;
		}
		DatabaseFacade.Log("Got save user token [" + text + "]");
		string text2 = ((!GlobalDefines._pUseLegoSDK) ? LEGOID._pInstance.GetStringFromConfigXML("urls/legoservices/saveHighscore") : LEGOSDKAmuzo._pInstance.GetEndpointFromApplicationConfig("highscore-save"));
		string gameId = getGameId(_saveRequestInProgress.dataToSave.getData("leaderboardName"));
		string data = _saveRequestInProgress.dataToSave.getData("score");
		text2 = text2.Replace("{gameid}", gameId);
		text2 = text2.Replace("{score}", data);
		text2 = text2.Replace("{token}", text);
		string plainText = gameId + data + text;
		byte[] inArray = ComputeHash(plainText, _hashKeyBytes);
		string text3 = Convert.ToBase64String(inArray);
		try
		{
			DatabaseFacade.Log("Sending save POST request. Checksum [" + text3 + "]. Endpoint [" + text2 + "]");
		}
		catch (Exception ex)
		{
			DatabaseFacade.Log("Sending save POST request.. Failed to log checksum! Error [" + ex.Message + "]");
		}
		addLEGOIDCallbacksForSave();
		LEGOSDKFacade.Instance.GenericPostRequest(text2, new UTF8Encoding().GetBytes(text3));
	}

	private string processResponse(string responseString)
	{
		if (!Utils.isBase64Encoded(responseString))
		{
			return responseString;
		}
		byte[] array = Convert.FromBase64String(responseString);
		return Encoding.UTF8.GetString(array, 0, array.Length);
	}

	private void addLEGOIDCallbacksForGetHighScores()
	{
		AmuzoLEGOIDCallbackHandler._onGetRequestSuccess = (Action<string>)Delegate.Combine(AmuzoLEGOIDCallbackHandler._onGetRequestSuccess, new Action<string>(onGetHighScoresSuccess));
		AmuzoLEGOIDCallbackHandler._onGetRequestFail = (Action<string>)Delegate.Combine(AmuzoLEGOIDCallbackHandler._onGetRequestFail, new Action<string>(onGetHighScoresFail));
	}

	private void removeLEGOIDCallbacksForGetHighScores()
	{
		AmuzoLEGOIDCallbackHandler._onGetRequestSuccess = (Action<string>)Delegate.Remove(AmuzoLEGOIDCallbackHandler._onGetRequestSuccess, new Action<string>(onGetHighScoresSuccess));
		AmuzoLEGOIDCallbackHandler._onGetRequestFail = (Action<string>)Delegate.Remove(AmuzoLEGOIDCallbackHandler._onGetRequestFail, new Action<string>(onGetHighScoresFail));
	}

	private void addLEGOIDCallbacksForGetUser()
	{
		AmuzoLEGOIDCallbackHandler._onGetRequestSuccess = (Action<string>)Delegate.Combine(AmuzoLEGOIDCallbackHandler._onGetRequestSuccess, new Action<string>(onGetUserSuccess));
		AmuzoLEGOIDCallbackHandler._onGetRequestFail = (Action<string>)Delegate.Combine(AmuzoLEGOIDCallbackHandler._onGetRequestFail, new Action<string>(onGetUserFail));
	}

	private void removeLEGOIDCallbacksForGetUser()
	{
		AmuzoLEGOIDCallbackHandler._onGetRequestSuccess = (Action<string>)Delegate.Remove(AmuzoLEGOIDCallbackHandler._onGetRequestSuccess, new Action<string>(onGetUserSuccess));
		AmuzoLEGOIDCallbackHandler._onGetRequestFail = (Action<string>)Delegate.Remove(AmuzoLEGOIDCallbackHandler._onGetRequestFail, new Action<string>(onGetUserFail));
	}

	private void addLEGOIDCallbacksForSave()
	{
		AmuzoLEGOIDCallbackHandler._onGenericPostRequestSuccess = (Action<string>)Delegate.Combine(AmuzoLEGOIDCallbackHandler._onGenericPostRequestSuccess, new Action<string>(saveUserSuccess));
		AmuzoLEGOIDCallbackHandler._onGenericPostRequestFail = (Action<string>)Delegate.Combine(AmuzoLEGOIDCallbackHandler._onGenericPostRequestFail, new Action<string>(onSaveUserFail));
	}

	private void removeLEGOIDCallbacksForSave()
	{
		AmuzoLEGOIDCallbackHandler._onGenericPostRequestSuccess = (Action<string>)Delegate.Remove(AmuzoLEGOIDCallbackHandler._onGenericPostRequestSuccess, new Action<string>(saveUserSuccess));
		AmuzoLEGOIDCallbackHandler._onGenericPostRequestFail = (Action<string>)Delegate.Remove(AmuzoLEGOIDCallbackHandler._onGenericPostRequestFail, new Action<string>(onSaveUserFail));
	}

	private void addLEGOIDCallbacksForToken()
	{
		AmuzoLEGOIDCallbackHandler._onGetRequestSuccess = (Action<string>)Delegate.Combine(AmuzoLEGOIDCallbackHandler._onGetRequestSuccess, new Action<string>(saveUserDataPart2));
		AmuzoLEGOIDCallbackHandler._onGetRequestFail = (Action<string>)Delegate.Combine(AmuzoLEGOIDCallbackHandler._onGetRequestFail, new Action<string>(onSaveUserFail));
	}

	private void removeLEGOIDCallbacksForToken()
	{
		AmuzoLEGOIDCallbackHandler._onGetRequestSuccess = (Action<string>)Delegate.Remove(AmuzoLEGOIDCallbackHandler._onGetRequestSuccess, new Action<string>(saveUserDataPart2));
		AmuzoLEGOIDCallbackHandler._onGetRequestFail = (Action<string>)Delegate.Remove(AmuzoLEGOIDCallbackHandler._onGetRequestFail, new Action<string>(onSaveUserFail));
	}

	private void onUserNameUpdated()
	{
		_userUniqueId = LEGOID._pInstance._pLogInName;
		DatabaseFacade.Instance._userData._name = LEGOID._pInstance._pLogInName;
	}

	private byte[] ComputeHash(string plainText, byte[] saltBytes)
	{
		byte[] bytes = Encoding.Unicode.GetBytes(plainText);
		byte[] array = new byte[bytes.Length + saltBytes.Length];
		bytes.CopyTo(array, 0);
		saltBytes.CopyTo(array, bytes.Length - 1);
		HashAlgorithm hashAlgorithm = new SHA512Managed();
		return hashAlgorithm.ComputeHash(array);
	}

	private void finishedPositionRequest()
	{
		_positionRequestInProgress = null;
		startNextRequest();
	}

	private void finishSavingUser()
	{
		_saveRequestInProgress = null;
		startNextRequest();
	}

	private void startNextRequest()
	{
		if (_positionRequestQueue.Count > 0)
		{
			PositionRequest nextRequest = _positionRequestQueue[0];
			_positionRequestQueue.RemoveAt(0);
			startPositionRequest(nextRequest);
		}
		else if (_saveRequestQueue.Count > 0)
		{
			SaveRequest newSaveRequest = _saveRequestQueue[0];
			_saveRequestQueue.RemoveAt(0);
			saveUserData(newSaveRequest);
		}
	}

	private string getGameId(string leaderboardName)
	{
		int i = 0;
		for (int num = _leaderboardGameIds.Length; i < num; i++)
		{
			if (!(_leaderboardGameIds[i]._leaderboardName != leaderboardName))
			{
				return _leaderboardGameIds[i]._legoIdGameId;
			}
		}
		return "NotFound";
	}

	private void saveUserSuccess(DownloadRequest request)
	{
		saveUserSuccess(request._pResponseString);
	}

	private void saveUserSuccess(string response)
	{
		response = processResponse(response);
		removeLEGOIDCallbacksForSave();
		DatabaseFacade.Log("SaveUser success! Response [" + response + "]");
		if (_saveRequestInProgress.onSuccess != null)
		{
			_saveRequestInProgress.onSuccess(null);
		}
		finishSavingUser();
	}

	private void onSaveUserFail(DownloadRequest request)
	{
		onSaveUserFail(request._pErrorString);
	}

	private void onSaveUserFail(string response)
	{
		removeLEGOIDCallbacksForToken();
		removeLEGOIDCallbacksForSave();
		if (_saveRequestInProgress.attempt < 3)
		{
			DatabaseFacade.Log("SaveUser unsuccessful! Making attempt [" + _saveRequestInProgress.attempt + "]. Response [" + ((!string.IsNullOrEmpty(response)) ? response : "NULL") + "]");
			SaveRequest saveRequestInProgress = _saveRequestInProgress;
			_saveRequestInProgress = null;
			saveUserData(saveRequestInProgress);
			return;
		}
		DatabaseFacade.Log("SaveUser fail! Error [" + response + "]. We made [" + 3 + "] attempts.");
		if (_saveRequestInProgress.onFail != null)
		{
			_saveRequestInProgress.onFail(null);
		}
		finishSavingUser();
	}

	private void onGetUserSuccess(DownloadRequest response)
	{
		onGetUserSuccess(response._pResponseString);
	}

	private void onGetUserSuccess(string response)
	{
		response = processResponse(response);
		if (onGetUserSuccessCallback == null)
		{
			finishedPositionRequest();
			return;
		}
		DatabaseFacade.Log("Get user success! [" + response + "]");
		DatabaseRequest databaseRequest = new DatabaseRequest();
		MonoXmlElement monoXmlElement = Xml.Load(response);
		MonoXmlElement childAt = monoXmlElement.GetChildAt("response/highscore");
		string text = (-99999999).ToString();
		string text2 = (-99999999).ToString();
		if (childAt != null)
		{
			MonoXmlElement childAt2 = childAt.GetChildAt("position");
			if (childAt2 != null && !string.IsNullOrEmpty(childAt2._pText))
			{
				text = childAt2._pText;
			}
			MonoXmlElement childAt3 = childAt.GetChildAt("score");
			if (childAt3 != null && !string.IsNullOrEmpty(childAt3._pText))
			{
				text2 = childAt3._pText;
			}
		}
		if (text == (-99999999).ToString() || text2 == (-99999999).ToString())
		{
			onGetUserFail("Unable to find position and score in User data received!");
			return;
		}
		UserData userData = new UserData();
		userData.clone(DatabaseFacade.Instance._userData);
		userData._uniqueId = _userUniqueId;
		userData._name = DatabaseFacade.Instance._userData._name;
		userData._timestamp = TimeManager.GetCurrentTime().Ticks;
		userData.addData("position", text);
		userData.addData("score", text2);
		databaseRequest.responseData.Add(new FormDataEntry("position", text));
		databaseRequest.responseData.Add(new FormDataEntry("score", text2));
		databaseRequest.responseUserData.Add(userData);
		DatabaseFacade.Log("Get user success! Score [" + text2 + "]. Position [" + text + "]. Unique Id [" + userData._uniqueId + "]. getUserSuccessCallback exists? [" + (onGetUserSuccessCallback != null) + "]");
		removeLEGOIDCallbacksForGetUser();
		onGetUserSuccessCallback(databaseRequest);
		finishedPositionRequest();
	}

	private void onGetUserFail(DownloadRequest errorMessage)
	{
		onGetUserFail(errorMessage._pErrorString);
	}

	private void onGetUserFail(string errorMessage)
	{
		removeLEGOIDCallbacksForGetUser();
		DatabaseFacade.Log("Get user request Failed! Error [" + errorMessage + "]");
		if (onGetUserFailCallback != null)
		{
			onGetUserFailCallback(null);
		}
		finishedPositionRequest();
	}

	private void onGetHighScoresSuccess(DownloadRequest response)
	{
		onGetHighScoresSuccess(response._pResponseString);
	}

	private void onGetHighScoresSuccess(string response)
	{
		response = processResponse(response);
		AmuzoLEGOIDCallbackHandler._onGetRequestSuccess = (Action<string>)Delegate.Remove(AmuzoLEGOIDCallbackHandler._onGetRequestSuccess, new Action<string>(onGetHighScoresSuccess));
		AmuzoLEGOIDCallbackHandler._onGetRequestFail = (Action<string>)Delegate.Remove(AmuzoLEGOIDCallbackHandler._onGetRequestFail, new Action<string>(onGetHighScoresFail));
		DatabaseFacade.Log("Get high scores success! Response [" + response + "]");
		if (onGetHighScoresSuccessCallback == null)
		{
			finishedPositionRequest();
			return;
		}
		DatabaseRequest databaseRequest = new DatabaseRequest();
		MonoXmlElement monoXmlElement = Xml.Load(response);
		MonoXmlElement childAt = monoXmlElement.GetChildAt("response/highscores");
		if (childAt == null || childAt._pCount <= 0)
		{
			onGetHighScoresFail("Request returned no users!");
			return;
		}
		string text = "No Name";
		string text2 = "0";
		string text3 = "0";
		int i = 0;
		for (int pCount = childAt._pCount; i < pCount; i++)
		{
			MonoXmlElement childAtIndex = childAt.GetChildAtIndex(i);
			if (childAtIndex != null)
			{
				databaseRequest.responseData.Add(new FormDataEntry(i.ToString(), childAtIndex.ToString()));
				text = childAtIndex.GetChildAt("u")._pText;
				text2 = childAtIndex.GetChildAt("p")._pText;
				text3 = childAtIndex.GetChildAt("s")._pText;
				DatabaseFacade.Log("User identified. Name [" + text + "]. Position [" + text2 + "]. Score [" + text3 + "]");
				UserData userData = new UserData();
				userData._timestamp = TimeManager.GetCurrentTime().Ticks;
				userData._name = text;
				userData._uniqueId = userData._name;
				userData.addData("position", text2);
				userData.addData("score", text3);
				databaseRequest.responseUserData.Add(userData);
			}
		}
		databaseRequest.responseUserData.Sort(sortUserDataByPosition);
		removeLEGOIDCallbacksForGetHighScores();
		onGetHighScoresSuccessCallback(databaseRequest);
		finishedPositionRequest();
	}

	private int sortUserDataByPosition(UserData userA, UserData userB)
	{
		if (userA == null && userB == null)
		{
			return 0;
		}
		if (userA == null)
		{
			return 1;
		}
		if (userB == null)
		{
			return -1;
		}
		int result = int.MaxValue;
		int result2 = int.MaxValue;
		int.TryParse(userA.getData("position"), out result);
		int.TryParse(userB.getData("position"), out result2);
		if (result == int.MaxValue && result2 == int.MaxValue)
		{
			return 0;
		}
		if (result == int.MaxValue && result2 != int.MaxValue)
		{
			return 1;
		}
		if (result != int.MaxValue && result2 == int.MaxValue)
		{
			return -1;
		}
		return (result != result2) ? ((result2 < result) ? 1 : (-1)) : 0;
	}

	private void onGetHighScoresFail(DownloadRequest errorMessage)
	{
		onGetHighScoresFail(errorMessage._pErrorString);
	}

	private void onGetHighScoresFail(string errorMessage)
	{
		removeLEGOIDCallbacksForGetHighScores();
		DatabaseFacade.Log("Get high scores request Failed! Error message [" + errorMessage + "]");
		if (onGetHighScoresFailCallback != null)
		{
			onGetHighScoresFailCallback(null);
		}
		finishedPositionRequest();
	}
}
