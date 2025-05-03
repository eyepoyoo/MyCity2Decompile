using System.Collections.Generic;
using GameDefines;
using UnityEngine;

public class ScreenLeaderboard : ScreenBase
{
	private const float BUTTON_DELAY = 0.25f;

	public LeaderboardDataPanel[] _dataPanels;

	public LeaderboardDataPanel _playerPosition;

	public UISprite _waitingIcon;

	public UISprite _errorIcon;

	public UIWidget[] _connectivityProblemElems;

	public UIWidget _playerPanel;

	private LeaderboardType _currentType = LeaderboardType.GLOBAL;

	private float _currentButtonDelay;

	protected override void Update()
	{
		base.Update();
		_currentButtonDelay += Time.deltaTime;
	}

	public void OnBackPressed()
	{
		if (!(_currentButtonDelay < 0.25f))
		{
			Navigate("Back");
		}
	}

	protected override void OnShowScreen()
	{
		if (GlobalDefines._doUseInternetReachabilityVerifier && !InternetCheckFacade._pIsOnline)
		{
			DisplayNoConnection();
			return;
		}
		_playerPanel.gameObject.SetActive(true);
		int num = _connectivityProblemElems.Length;
		for (int i = 0; i < num; i++)
		{
			_connectivityProblemElems[i].gameObject.SetActive(false);
		}
		num = _dataPanels.Length;
		for (int j = 0; j < num; j++)
		{
			_dataPanels[j].gameObject.SetActive(true);
		}
		getLeaderboardData();
		_currentButtonDelay = 0f;
		if (_errorIcon != null)
		{
			_errorIcon.gameObject.SetActive(false);
		}
	}

	private void DisplayNoConnection()
	{
		_playerPanel.gameObject.SetActive(false);
		int num = _connectivityProblemElems.Length;
		for (int i = 0; i < num; i++)
		{
			_connectivityProblemElems[i].gameObject.SetActive(true);
		}
		num = _dataPanels.Length;
		for (int j = 0; j < num; j++)
		{
			_dataPanels[j].gameObject.SetActive(false);
		}
	}

	private void getLeaderboardData()
	{
		hideAllRows();
		showHideWaitingSymbol(true);
		Debug.Log("LEADER BOARD - Getting PositionS");
		LeaderboardFacade.Instance.getPositions((_currentType != LeaderboardType.LOCAL) ? DatabaseType.LEGO : DatabaseType.LOCAL, "CumulativeStuds", 0, 5, false, onLeaderboardDataReceived, onDataRequestFailed);
	}

	private void hideAllRows()
	{
		_playerPosition.hidePanel();
		for (int i = 0; i < _dataPanels.Length; i++)
		{
			if (!(_dataPanels[i] == null))
			{
				_dataPanels[i].hidePanel();
			}
		}
	}

	private void onLeaderboardDataReceived(List<LeaderboardEntry> leaderboardEntries)
	{
		Debug.Log("LEADER BOARD - Data received");
		if (!_isVisible)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < _dataPanels.Length; i++)
		{
			if (!(_dataPanels[i] == null) && i < leaderboardEntries.Count)
			{
				_dataPanels[i].SetPositionData(leaderboardEntries[i]);
				flag = flag || leaderboardEntries[i]._name == DatabaseFacade.Instance._userData._name;
			}
		}
		showHideWaitingSymbol(false);
		if (!flag && (_currentType != LeaderboardType.GLOBAL || (LEGOID._pInstance != null && LEGOID._pInstance._pAuthenticated)))
		{
			Debug.Log("LEADER BOARD - getting position");
			LeaderboardFacade.Instance.getPosition((_currentType != LeaderboardType.LOCAL) ? DatabaseType.LEGO : DatabaseType.LOCAL, "CumulativeStuds", 1, false, onPlayerDataReceived, null);
		}
	}

	private void onPlayerDataReceived(List<LeaderboardEntry> leaderboardEntries)
	{
		Debug.Log("LEADER BOARD - player data received");
		if (leaderboardEntries != null && leaderboardEntries.Count > 0)
		{
			_playerPosition.SetPositionData(leaderboardEntries[0]);
		}
	}

	private void onDataRequestFailed(DownloadRequest failedRequest)
	{
		Debug.Log("LEADER BOARD - Data reqest failed Screen");
		showHideWaitingSymbol(false);
		DisplayNoConnection();
		if (_errorIcon != null)
		{
			_errorIcon.gameObject.SetActive(true);
		}
	}

	private void showHideWaitingSymbol(bool doShow)
	{
		if (!(_waitingIcon == null))
		{
			_waitingIcon.gameObject.SetActive(doShow);
		}
	}
}
