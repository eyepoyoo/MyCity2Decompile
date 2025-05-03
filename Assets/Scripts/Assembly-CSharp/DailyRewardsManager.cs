using System;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

public class DailyRewardsManager : MonoBehaviour
{
	[Serializable]
	public class DailyReward
	{
		public Reward[] _rewards;

		public void rewardAchieved()
		{
			for (int i = 0; i < _rewards.Length; i++)
			{
				_rewards[i].givePlayerReward();
			}
		}
	}

	private const string FIRST_VIEW_SAVE_KEY = "FirstDailyReward";

	private const string LAST_REWARD_SAVE_KEY = "LastDailyReward";

	private static bool DO_DEBUG = true;

	public Action<DailyReward> _onRewardEarned;

	private static DailyRewardsManager _instance;

	public DailyReward[] _dailyRewards;

	private int _lastRewardEarned = -1;

	public static DailyRewardsManager Instance
	{
		get
		{
			return _instance;
		}
	}

	public int LastRewardIndex
	{
		get
		{
			return _lastRewardEarned;
		}
	}

	public DailyReward LastReward
	{
		get
		{
			return (_lastRewardEarned != -1) ? _dailyRewards[_lastRewardEarned] : null;
		}
	}

	private void Awake()
	{
		_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		addDebugMenuOptions();
	}

	private void OnDestroy()
	{
		_instance = null;
	}

	public bool processDailyRewards(bool doAutomaticallyAwardReward = true)
	{
		if (!hasKey("LastDailyReward"))
		{
			Log("No daily reward data found!");
			ResetDailyRewards();
			return false;
		}
		DateTime dateTime = new DateTime(getLong("FirstDailyReward"));
		DateTime dateTime2 = new DateTime(getLong("LastDailyReward"));
		DateTime currentTime = TimeManager.GetCurrentTime();
		int days = (dateTime2 - dateTime).Days;
		int days2 = (currentTime - dateTime).Days;
		int num = days2 - days;
		Log("Num days since start [" + days2 + "]. Num days since last reward [" + num + "]");
		if (num < 0)
		{
			Log("Days since start was negative. Something went wrong here...");
			ResetDailyRewards();
			return false;
		}
		if (num == 0)
		{
			if (DO_DEBUG)
			{
				DateTime dateTime3 = dateTime;
				dateTime3 += new TimeSpan(days2 + 1, 0, 0, 0);
				TimeSpan timeSpan = dateTime3 - currentTime;
				Log("It's the same day. No reward. Time till next reward [ " + timeSpan.Hours + "h " + timeSpan.Minutes + "m " + timeSpan.Seconds + "s ]");
			}
			return false;
		}
		if (num > 1)
		{
			Log("It's been more than 1 day. Fail, go back to the start.");
			ResetDailyRewards();
			return false;
		}
		setLong("LastDailyReward", currentTime.Ticks);
		Log("New reward earned!");
		for (int i = 0; i < _dailyRewards.Length; i++)
		{
			if (i + 1 == days2)
			{
				_lastRewardEarned = i;
				Log("Earned reward for day [" + (_lastRewardEarned + 1) + "]");
				if (Facades<TrackingFacade>.Instance != null)
				{
					Facades<TrackingFacade>.Instance.LogParameterMetric("Daily Reward", new Dictionary<string, string> { 
					{
						"Reward",
						(_lastRewardEarned + 1).ToString()
					} });
					Facades<TrackingFacade>.Instance.LogProgress("Reward_" + (_lastRewardEarned + 1) + "_collected");
				}
				if (doAutomaticallyAwardReward)
				{
					_dailyRewards[i].rewardAchieved();
				}
				if (_onRewardEarned != null)
				{
					_onRewardEarned(_dailyRewards[i]);
				}
				if (i == _dailyRewards.Length - 1)
				{
					ResetDailyRewards();
				}
				return true;
			}
		}
		Log("No rewards were found for day [" + days2 + "]. This should have already triggered a reset.");
		ResetDailyRewards();
		return false;
	}

	public static void moveDailyRewardBackADay()
	{
		if (!hasKey("FirstDailyReward"))
		{
			ResetDailyRewards();
			return;
		}
		Log("Daily rewards recorded times artifically pushed back 24 hours.");
		DateTime dateTime = new DateTime(getLong("FirstDailyReward"));
		DateTime dateTime2 = new DateTime(getLong("LastDailyReward"));
		dateTime = dateTime.Subtract(new TimeSpan(24, 0, 0));
		dateTime2 = dateTime2.Subtract(new TimeSpan(24, 0, 0));
		setLong("FirstDailyReward", dateTime.Ticks);
		setLong("LastDailyReward", dateTime2.Ticks);
	}

	public static void ResetDailyRewards()
	{
		Log("Resetting Daily Rewards");
		DateTime currentTime = TimeManager.GetCurrentTime();
		setLong("LastDailyReward", currentTime.Ticks);
		setLong("FirstDailyReward", currentTime.Ticks);
	}

	private static bool hasKey(string saveKey)
	{
		if (AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pExists)
		{
			return AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.hasKey(saveKey);
		}
		if (AmuzoScriptableSingleton<PlayerPrefsFacade>._pExists)
		{
			return AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.HasKey(saveKey);
		}
		return ObscuredPrefs.HasKey(saveKey);
	}

	private static long getLong(string saveKey)
	{
		if (AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pExists)
		{
			return AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getLong(saveKey);
		}
		if (AmuzoScriptableSingleton<PlayerPrefsFacade>._pExists)
		{
			return AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.GetLong(saveKey, 0L);
		}
		return ObscuredPrefs.GetLong(saveKey, 0L);
	}

	private static void setLong(string saveKey, long value)
	{
		if (AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pExists)
		{
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setLong(saveKey, value);
		}
		else if (AmuzoScriptableSingleton<PlayerPrefsFacade>._pExists)
		{
			AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.SetLong(saveKey, value);
		}
		else
		{
			ObscuredPrefs.SetLong(saveKey, value);
		}
	}

	private void addDebugMenuOptions()
	{
		if (!AmuzoMonoSingleton<AmuzoDebugMenuManager>._pExists)
		{
			return;
		}
		Func<string> textAreaFunction = delegate
		{
			string empty = string.Empty;
			DateTime dateTime = new DateTime(getLong("FirstDailyReward"));
			DateTime dateTime2 = new DateTime(getLong("LastDailyReward"));
			DateTime currentTime = TimeManager.GetCurrentTime();
			string text = empty;
			empty = text + "Reward Start [" + dateTime.ToLongDateString() + " " + dateTime.ToLongTimeString() + "]" + AmuzoDebugMenu.NEW_LINE;
			text = empty;
			empty = text + "Last Reward [" + dateTime2.ToLongDateString() + " " + dateTime2.ToLongTimeString() + "]" + AmuzoDebugMenu.NEW_LINE;
			text = empty;
			empty = text + "Current Time [" + currentTime.ToLongDateString() + " " + currentTime.ToLongTimeString() + "]" + AmuzoDebugMenu.NEW_LINE;
			int days = (dateTime2 - dateTime).Days;
			int days2 = (currentTime - dateTime).Days;
			int num = days2 - days;
			if (num >= 0)
			{
				switch (num)
				{
				case 0:
				{
					DateTime dateTime4 = dateTime;
					dateTime4 += new TimeSpan(days2 + 1, 0, 0, 0);
					TimeSpan timeSpan2 = dateTime4 - currentTime;
					text = empty;
					return text + "RESULT: It's the same day. No reward. Time till next reward [ " + timeSpan2.Hours + "h " + timeSpan2.Minutes + "m " + timeSpan2.Seconds + "s ]" + AmuzoDebugMenu.NEW_LINE;
				}
				case 1:
				{
					DateTime dateTime3 = dateTime;
					dateTime3 += new TimeSpan(days2 + 2, 0, 0, 0);
					TimeSpan timeSpan = dateTime3 - currentTime;
					text = empty;
					return text + "RESULT: Reward avalible. Expires in [ " + timeSpan.Hours + "h " + timeSpan.Minutes + "m " + timeSpan.Seconds + "s ]" + AmuzoDebugMenu.NEW_LINE;
				}
				default:
					return empty + "RESULT: It's been more than 1 day. Fail, go back to the start." + AmuzoDebugMenu.NEW_LINE;
				}
			}
			return empty + "RESULT: Days since start was negative. Something went wrong here..." + AmuzoDebugMenu.NEW_LINE;
		};
		AmuzoDebugMenu amuzoDebugMenu = new AmuzoDebugMenu("DAILY REWARDS");
		amuzoDebugMenu.AddInfoTextFunction(textAreaFunction);
		amuzoDebugMenu.AddButton(new AmuzoDebugMenuButton("Move Forward 24h", delegate
		{
			moveDailyRewardBackADay();
		}));
		amuzoDebugMenu.AddButton(new AmuzoDebugMenuButton("Reset", delegate
		{
			ResetDailyRewards();
		}));
		AmuzoDebugMenuManager.RegisterRootDebugMenu(amuzoDebugMenu);
	}

	public static void Log(string message, UnityEngine.Object o = null)
	{
		if (DO_DEBUG)
		{
			Debug.Log("Daily Rewards: " + message, o);
		}
	}
}
