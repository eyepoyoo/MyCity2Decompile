using System;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardRequest
{
	public const string NOT_SET = "NotSet";

	public int _startIdx = -1;

	public int _length = -1;

	public string _uniqueId = "NotSet";

	public string _leaderboardName = "NotSet";

	public bool _isFriendsOnlyLeaderboard;

	public Action<List<LeaderboardEntry>> _onFinalSuccessCallback;

	public Action<DownloadRequest> _onFinalFailCallback;

	public Action<LeaderboardRequest> _onSuccess;

	public Action<LeaderboardRequest> _onFail;

	public DatabaseRequest _response;

	public void startHighScoresRequest()
	{
		DatabaseFacade.Instance.getHighScores(_leaderboardName, _startIdx, _length, _isFriendsOnlyLeaderboard, onRequestComplete, onRequestFail);
	}

	public void startUserPositionRequest()
	{
		DatabaseFacade.Instance.getUserPosition(_leaderboardName, _uniqueId, _length, _isFriendsOnlyLeaderboard, onRequestComplete, onRequestFail);
	}

	private void onRequestComplete(DownloadRequest response)
	{
		Debug.Log("Leaderboard Request: Complete.");
		_response = response as DatabaseRequest;
		_onSuccess(this);
	}

	private void onRequestFail(DownloadRequest response)
	{
		Debug.Log("Leaderboard Request: Failed.");
		_response = response as DatabaseRequest;
		_onFail(this);
	}
}
