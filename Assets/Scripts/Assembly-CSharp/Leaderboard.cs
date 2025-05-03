using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Leaderboard
{
	private const bool DO_DEBUG = false;

	public string _leaderboardName;

	public bool _isFriendsOnlyLeaderboard;

	public DatabaseType _databaseType;

	public LeaderboardEntry[] _entries;

	private List<LeaderboardRequest> _requestsInProgress = new List<LeaderboardRequest>();

	public void getPosition(string uniqueId, int length, Action<List<LeaderboardEntry>> onSuccessCallback, Action<DownloadRequest> onFailCallback)
	{
		if (_entries != null && _databaseType != DatabaseType.LOCAL)
		{
			int i = 0;
			for (int num = _entries.Length; i < num; i++)
			{
				if (!(_entries[i]._uniqueId != uniqueId) && LeaderboardFacade.Instance._pCurrentTicks - _entries[i]._timestamp < LeaderboardFacade.Instance._pCacheTimeout)
				{
					onSuccessCallback(new List<LeaderboardEntry> { _entries[i] });
					return;
				}
			}
		}
		DatabaseFacade.Instance._activeDatabase = _databaseType;
		LeaderboardRequest leaderboardRequest = new LeaderboardRequest();
		leaderboardRequest._uniqueId = uniqueId;
		leaderboardRequest._onFinalSuccessCallback = onSuccessCallback;
		leaderboardRequest._onSuccess = onDatabaseRequestComplete;
		leaderboardRequest._onFinalFailCallback = onFailCallback;
		leaderboardRequest._onFail = onDatabaseRequestFailed;
		leaderboardRequest._leaderboardName = _leaderboardName;
		leaderboardRequest._length = length;
		leaderboardRequest._isFriendsOnlyLeaderboard = _isFriendsOnlyLeaderboard;
		leaderboardRequest.startUserPositionRequest();
		_requestsInProgress.Add(leaderboardRequest);
	}

	public void getPositions(int startIdx, int length, Action<List<LeaderboardEntry>> onSuccessCallback, Action<DownloadRequest> onFailCallback)
	{
		long num = long.MaxValue;
		bool flag = true;
		if (_entries != null && _databaseType != DatabaseType.LOCAL)
		{
			int i = startIdx;
			for (int num2 = startIdx + length; i < num2; i++)
			{
				bool flag2 = false;
				int j = 0;
				for (int num3 = _entries.Length; j < num3; j++)
				{
					if (_entries[j]._position == i)
					{
						flag2 = true;
						num = ((num >= _entries[j]._timestamp) ? _entries[j]._timestamp : num);
					}
				}
				if (!flag2)
				{
					flag = false;
					break;
				}
			}
		}
		else
		{
			flag = false;
		}
		if (flag && LeaderboardFacade.Instance._pCurrentTicks - num < LeaderboardFacade.Instance._pCacheTimeout)
		{
			List<LeaderboardEntry> list = new List<LeaderboardEntry>();
			int k = startIdx;
			for (int num4 = startIdx + length; k < num4; k++)
			{
				int l = 0;
				for (int num5 = _entries.Length; l < num5; l++)
				{
					if (_entries[l]._position == k)
					{
						list.Add(_entries[l]);
					}
				}
			}
			onSuccessCallback(list);
		}
		else
		{
			DatabaseFacade.Instance._activeDatabase = _databaseType;
			LeaderboardRequest leaderboardRequest = new LeaderboardRequest();
			leaderboardRequest._startIdx = startIdx;
			leaderboardRequest._length = length;
			leaderboardRequest._onFinalSuccessCallback = onSuccessCallback;
			leaderboardRequest._onFinalFailCallback = onFailCallback;
			leaderboardRequest._onSuccess = onDatabaseRequestComplete;
			leaderboardRequest._onFail = onDatabaseRequestFailed;
			leaderboardRequest._leaderboardName = _leaderboardName;
			leaderboardRequest._isFriendsOnlyLeaderboard = _isFriendsOnlyLeaderboard;
			leaderboardRequest.startHighScoresRequest();
			_requestsInProgress.Add(leaderboardRequest);
		}
	}

	public LeaderboardEntry getPositionImmediate(string uniqueId)
	{
		if (_entries == null)
		{
			return null;
		}
		int i = 0;
		for (int num = _entries.Length; i < num; i++)
		{
			if (_entries[i] != null && !(_entries[i]._uniqueId != uniqueId))
			{
				return _entries[i];
			}
		}
		return null;
	}

	public int getPositionOf(string uniqueId)
	{
		int i = 0;
		for (int num = _entries.Length; i < num; i++)
		{
			if (!(_entries[i]._uniqueId != uniqueId))
			{
				return _entries[i]._position;
			}
		}
		return -1;
	}

	private void onDatabaseRequestFailed(LeaderboardRequest request)
	{
		if (_requestsInProgress.Contains(request))
		{
			_requestsInProgress.Remove(request);
		}
		if (request._onFinalFailCallback != null)
		{
			request._onFinalFailCallback(request._response);
		}
	}

	private void onDatabaseRequestComplete(LeaderboardRequest request)
	{
		if (_requestsInProgress.Contains(request))
		{
			_requestsInProgress.Remove(request);
		}
		if (_entries != null)
		{
			int num = request._startIdx;
			int num2 = Mathf.Max(1, request._length);
			if (num == -1)
			{
				int i = 0;
				for (int count = request._response.responseUserData.Count; i < count; i++)
				{
					if (!(request._response.responseUserData[i]._uniqueId != request._uniqueId))
					{
						num = int.Parse(request._response.responseUserData[i].getData("position"));
						break;
					}
				}
			}
			if (num == -1)
			{
				request._onFinalFailCallback(request._response);
				return;
			}
			int num3 = 0;
			int j = 0;
			for (int num4 = _entries.Length; j < num4; j++)
			{
				if (_entries[j] != null && _entries[j]._position >= num && _entries[j]._position <= num + num2)
				{
					num3++;
				}
			}
			LeaderboardEntry[] entries = _entries;
			_entries = new LeaderboardEntry[_entries.Length - num3];
			int num5 = 0;
			int k = 0;
			for (int num6 = entries.Length; k < num6; k++)
			{
				if (entries[k] != null && (entries[k]._position < num || entries[k]._position > num + num2))
				{
					_entries[num5] = entries[k];
					num5++;
				}
			}
		}
		int num7 = 0;
		if (_entries == null)
		{
			_entries = new LeaderboardEntry[request._response.responseUserData.Count];
		}
		else
		{
			int num8 = 0;
			int l = 0;
			for (int count2 = request._response.responseUserData.Count; l < count2; l++)
			{
				int m = 0;
				for (int num9 = _entries.Length; m < num9; m++)
				{
					if (_entries[m] != null && !(_entries[m]._uniqueId != request._response.responseUserData[l]._uniqueId))
					{
						num8++;
						break;
					}
				}
			}
			LeaderboardEntry[] entries2 = _entries;
			_entries = new LeaderboardEntry[entries2.Length + (request._response.responseUserData.Count - num8)];
			int n = 0;
			for (int num10 = entries2.Length; n < num10; n++)
			{
				_entries[n] = entries2[n];
			}
			num7 = entries2.Length;
		}
		int num11 = 0;
		for (int count3 = request._response.responseUserData.Count; num11 < count3; num11++)
		{
			bool flag = false;
			int num12 = 0;
			for (int num13 = _entries.Length; num12 < num13; num12++)
			{
				if (_entries[num12] != null && !(_entries[num12]._uniqueId != request._response.responseUserData[num11]._uniqueId))
				{
					flag = true;
					_entries[num12].clone(request._response.responseUserData[num11]);
				}
			}
			if (!flag)
			{
				_entries[num7] = new LeaderboardEntry();
				_entries[num7].clone(request._response.responseUserData[num11]);
				num7++;
			}
		}
		sortEntries();
		if (request._uniqueId != "NotSet" && request._length <= 1)
		{
			int num14 = 0;
			for (int num15 = _entries.Length; num14 < num15; num14++)
			{
				if (_entries[num14] != null && !(_entries[num14]._uniqueId != request._uniqueId))
				{
					request._onFinalSuccessCallback(new List<LeaderboardEntry> { _entries[num14] });
					return;
				}
			}
			request._onFinalFailCallback(request._response);
		}
		else
		{
			if (request._startIdx < 0 && request._uniqueId != "NotSet")
			{
				request._startIdx = getPositionOf(request._uniqueId) - 1;
				request._startIdx -= request._startIdx % request._length;
				request._startIdx++;
			}
			List<LeaderboardEntry> list = new List<LeaderboardEntry>();
			int num16 = 0;
			for (int num17 = _entries.Length; num16 < num17; num16++)
			{
				if (_entries[num16] != null && _entries[num16]._position >= request._startIdx && _entries[num16]._position <= request._startIdx + request._length)
				{
					list.Add(_entries[num16]);
				}
			}
			request._onFinalSuccessCallback(list);
		}
		LeaderboardFacade.Instance.leaderboardUpdated(this);
	}

	private void sortEntries()
	{
		if (_entries.Length <= 1)
		{
			if (_entries.Length == 1 && _entries[0] == null)
			{
				_entries = new LeaderboardEntry[0];
			}
			return;
		}
		bool flag = false;
		while (!flag)
		{
			flag = true;
			int i = 0;
			for (int num = _entries.Length - 2; i < num; i++)
			{
				if (_entries[i + 1] != null && (_entries[i] == null || _entries[i]._position > _entries[i + 1]._position))
				{
					flag = false;
					LeaderboardEntry leaderboardEntry = _entries[i + 1];
					_entries[i + 1] = _entries[i];
					_entries[i] = leaderboardEntry;
				}
			}
		}
		int num2 = 0;
		int num3 = _entries.Length - 1;
		while (num3 >= 0 && _entries[num3] == null)
		{
			num2++;
			num3--;
		}
		if (num2 != 0)
		{
			LeaderboardEntry[] entries = _entries;
			_entries = new LeaderboardEntry[entries.Length - num2];
			int j = 0;
			for (int num4 = _entries.Length; j < num4; j++)
			{
				_entries[j] = entries[j];
			}
		}
	}
}
