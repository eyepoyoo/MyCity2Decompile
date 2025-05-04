using System;
using System.Collections.Generic;
using System.Text;
using CodeStage.AntiCheat.ObscuredTypes;
using LitJson;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
	private const string KEY_ACHIEVEMENTS = "ACHIEVEMENTS";

	private const string KEY_QUESTS = "QUESTS";

	private const string KEY_UNCLAIMED_QUEST_REWARDS = "UNCLAIMED_QUEST_REWARDS";

	private const string LAST_QUEST_ISSUE_TIME = "LAST_QUEST_ISSUE_TIME";

	private const string NONE = "NONE";

	public bool debugGoBackInTime;

	private string[] _unclaimedRewards;

	private static QuestHandler _instance;

	public static QuestHandler _pInstance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		AddDebugMenuOptions();
		_instance = this;
	}

	private void Update()
	{
		if (debugGoBackInTime)
		{
			debugGoBackInTime = false;
			SetLong("LAST_QUEST_ISSUE_TIME", new DateTime(GetLong("LAST_QUEST_ISSUE_TIME")).AddDays(-1.0).Ticks);
		}
	}

	private void AddDebugMenuOptions()
	{
		if (!AmuzoMonoSingleton<AmuzoDebugMenuManager>._pExists)
		{
			return;
		}
		Func<string> textAreaFunction = delegate
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Unclaimed Rewards: ");
			for (int i = 0; i < _unclaimedRewards.Length; i++)
			{
				stringBuilder.Append("\n");
				stringBuilder.Append(_unclaimedRewards[i]);
			}
			stringBuilder.Append("\nActive: ");
			if (QuestSystem._pInstance._pCurrentList != null)
			{
				stringBuilder.Append(" (");
				stringBuilder.Append(QuestSystem._pInstance._pCurrentList.Length);
				stringBuilder.Append(")");
				for (int j = 0; j < QuestSystem._pInstance._pCurrentList.Length; j++)
				{
					stringBuilder.Append("\n");
					stringBuilder.Append(QuestSystem._pInstance._pCurrentList[j]);
				}
			}
			else
			{
				stringBuilder.Append(" (null)");
			}
			return stringBuilder.ToString();
		};
		AmuzoDebugMenu amuzoDebugMenu = new AmuzoDebugMenu("QUEST CHEATS");
		amuzoDebugMenu.AddInfoTextFunction(textAreaFunction);
		amuzoDebugMenu.AddButton(new AmuzoDebugMenuButton("MOVE BY 24h", delegate
		{
			SetLong("LAST_QUEST_ISSUE_TIME", new DateTime(GetLong("LAST_QUEST_ISSUE_TIME")).AddDays(-1.0).Ticks);
		}));
		AmuzoDebugMenuManager.RegisterRootDebugMenu(amuzoDebugMenu);
	}

	private void Start()
	{
		_unclaimedRewards = new string[QuestSystem._pInstance._numQuestsToPresentSimultaneously];
		int num = _unclaimedRewards.Length;
		for (int i = 0; i < num; i++)
		{
			_unclaimedRewards[i] = "NONE";
		}
		QuestSystem._pInstance.RegisterForQuestCompleteNotifications(OnQuestCompleted);
	}

	public string GetQuestName(int element)
	{
		int num = 0;
		for (int i = 0; i < _unclaimedRewards.Length; i++)
		{
			if (_unclaimedRewards[i] != null && _unclaimedRewards[i] != "NONE")
			{
				if (num == element)
				{
					return _unclaimedRewards[i];
				}
				num++;
			}
		}
		return null;
	}

	public int LoadSaveData()
	{
		string text = AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getString("ACHIEVEMENTS", string.Empty);
		if (text != null && text != string.Empty)
		{
			JsonData data = JsonMapperLite.ToObject(new JsonReader(text));
			AchievementSystem._pInstance.Load(data);
		}
		string text2 = AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getString("QUESTS", string.Empty);
		if (text2 != null && text2 != string.Empty)
		{
			JsonData data2 = JsonMapperLite.ToObject(new JsonReader(text2));
			QuestSystem._pInstance.Load(data2);
		}
		string[] stringArray = AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getStringArray("UNCLAIMED_QUEST_REWARDS");
		if (stringArray != null && stringArray.Length != 0)
		{
			_unclaimedRewards = stringArray;
		}
		return SeekNewQuests();
	}

	public int SeekNewQuests()
	{
		DateTime currentTime = TimeManager.GetCurrentTime();
		int numActiveQuests = QuestSystem._pInstance.GetNumActiveQuests();
		if (!HasKey("LAST_QUEST_ISSUE_TIME"))
		{
			Debug.Log("First time!");
			int result = QuestSystem._pInstance.FindNewQuests(1);
			int numActiveQuests2 = QuestSystem._pInstance.GetNumActiveQuests();
			if (numActiveQuests2 > numActiveQuests)
			{
				SetLong("LAST_QUEST_ISSUE_TIME", currentTime.Ticks);
				SaveAll();
				return result;
			}
			return 0;
		}
		DateTime dateTime = new DateTime(GetLong("LAST_QUEST_ISSUE_TIME"));
		int days = (currentTime - dateTime).Days;
		if (days < 0)
		{
			Debug.LogWarning("We appear to have gone back in time.... aborting SeekNewQuests()...");
			return 0;
		}
		if (days >= 1)
		{
			Debug.Log("We have gained a day since the last quest issue time, let's issue a new quest");
			int num = numActiveQuests + 1;
			if (num > QuestSystem._pInstance._numQuestsToPresentSimultaneously)
			{
				Debug.Log("Player should finish quests before being issued some more, no new quests were issued");
				return 0;
			}
			int num2 = 0;
			int num3 = _unclaimedRewards.Length;
			for (int i = 0; i < num3; i++)
			{
				if (_unclaimedRewards[i] != "NONE")
				{
					num2++;
				}
			}
			if (num2 >= QuestSystem._pInstance._numQuestsToPresentSimultaneously)
			{
				Debug.Log("Can't find new quests, they have too many unclaimed rewards");
				return 0;
			}
			if (num2 + QuestSystem._pInstance.GetNumActiveQuests() >= QuestSystem._pInstance._numQuestsToPresentSimultaneously)
			{
				Debug.Log("Can't find new quests, too many unclaimed rewards");
				return 0;
			}
			int result2 = QuestSystem._pInstance.FindNewQuests(1);
			numActiveQuests = QuestSystem._pInstance.GetNumActiveQuests();
			if (numActiveQuests == num)
			{
				Debug.Log("Found new quests, updating timestamp");
				SetLong("LAST_QUEST_ISSUE_TIME", currentTime.Ticks);
				SaveAll();
				return result2;
			}
			Debug.Log("No quests were issued");
			return 0;
		}
		return 0;
	}

	private bool CompareParts(VehiclePart part, VehiclePart.EUNIQUE_ID id)
	{
		if (part == null)
		{
			return id == VehiclePart.EUNIQUE_ID.INVALID;
		}
		return part.uniqueID == id;
	}

	public void OnStartMission(MinigameManager.MinigameData minigameData, VehiclePart part1, VehiclePart part2, VehiclePart part3)
	{
		Debug.Log(string.Concat("On Start Mission: ", minigameData.minigameType, " / ", minigameData.minigameCategory));
		if (part1 != null)
		{
			Debug.Log("Part1 = " + part1.uniqueID);
		}
		if (part2 != null)
		{
			Debug.Log("Part2 = " + part2.uniqueID);
		}
		if (part3 != null)
		{
			Debug.Log("Part3 = " + part3.uniqueID);
		}
		if (minigameData.minigameType == MinigameManager.EMINIGAME_TYPE.CATCH_THE_CROOKS)
		{
			if (CompareParts(part1, minigameData.vehicleTemplates[0].bodyPart) && CompareParts(part2, minigameData.vehicleTemplates[0].wheelPart) && CompareParts(part3, minigameData.vehicleTemplates[0].accessoryPart))
			{
				QuestSystem._pInstance.SetStatBoolean("CatchTheCrooksWithPoliceCar", true);
			}
			else if (CompareParts(part1, minigameData.vehicleTemplates[1].bodyPart) && CompareParts(part2, minigameData.vehicleTemplates[1].wheelPart) && CompareParts(part3, minigameData.vehicleTemplates[1].accessoryPart))
			{
				QuestSystem._pInstance.SetStatBoolean("CatchTheCrooksWithBuggy", true);
			}
		}
		else if (minigameData.minigameType == MinigameManager.EMINIGAME_TYPE.FIRE_FRENZY)
		{
			if (CompareParts(part1, minigameData.vehicleTemplates[0].bodyPart) && CompareParts(part2, minigameData.vehicleTemplates[0].wheelPart) && CompareParts(part3, minigameData.vehicleTemplates[0].accessoryPart))
			{
				QuestSystem._pInstance.SetStatBoolean("FireFrenzyWithFireEngine", true);
			}
			else if (CompareParts(part1, minigameData.vehicleTemplates[1].bodyPart) && CompareParts(part2, minigameData.vehicleTemplates[1].wheelPart) && CompareParts(part3, minigameData.vehicleTemplates[1].accessoryPart))
			{
				QuestSystem._pInstance.SetStatBoolean("FireFrenzyWithFireQuadBike", true);
			}
		}
		else if (minigameData.minigameType == MinigameManager.EMINIGAME_TYPE.WATER_DUMP)
		{
			if (CompareParts(part1, minigameData.vehicleTemplates[0].bodyPart) && CompareParts(part2, minigameData.vehicleTemplates[0].attachmentPart) && CompareParts(part3, minigameData.vehicleTemplates[0].accessoryPart))
			{
				QuestSystem._pInstance.SetStatBoolean("WaterDumpWithLargeFireHeli", true);
			}
			else if (CompareParts(part1, minigameData.vehicleTemplates[1].bodyPart) && CompareParts(part2, minigameData.vehicleTemplates[1].attachmentPart) && CompareParts(part3, minigameData.vehicleTemplates[1].accessoryPart))
			{
				QuestSystem._pInstance.SetStatBoolean("WaterDumpWithSmallFireHeli", true);
			}
		}
		else if (minigameData.minigameType == MinigameManager.EMINIGAME_TYPE.ROLLIN_ROCKS)
		{
			if (CompareParts(part1, minigameData.vehicleTemplates[0].bodyPart) && CompareParts(part2, minigameData.vehicleTemplates[0].wheelPart) && CompareParts(part3, minigameData.vehicleTemplates[0].accessoryPart))
			{
				QuestSystem._pInstance.SetStatBoolean("RollinRocksWithExplorerTruck", true);
			}
			else if (CompareParts(part1, minigameData.vehicleTemplates[1].bodyPart) && CompareParts(part2, minigameData.vehicleTemplates[1].wheelPart) && CompareParts(part3, minigameData.vehicleTemplates[1].accessoryPart))
			{
				QuestSystem._pInstance.SetStatBoolean("RollinRocksWithMiniTractor", true);
			}
		}
		else if (minigameData.minigameType == MinigameManager.EMINIGAME_TYPE.EXPLORER_EVACUATION)
		{
			if (CompareParts(part1, minigameData.vehicleTemplates[0].bodyPart) && CompareParts(part2, minigameData.vehicleTemplates[0].attachmentPart) && CompareParts(part3, minigameData.vehicleTemplates[0].accessoryPart))
			{
				QuestSystem._pInstance.SetStatBoolean("ExplorerEvacWithExplorerHeli", true);
			}
			else if (CompareParts(part1, minigameData.vehicleTemplates[1].bodyPart) && CompareParts(part2, minigameData.vehicleTemplates[1].attachmentPart) && CompareParts(part3, minigameData.vehicleTemplates[1].accessoryPart))
			{
				QuestSystem._pInstance.SetStatBoolean("ExplorerEvacWithExplorerChinook", true);
			}
		}
		if (part1.uniqueID == VehiclePart.EUNIQUE_ID.BODY_PORTALOO)
		{
			QuestSystem._pInstance.SetStatBoolean("PlayWithPortaloo", true);
		}
		if (part1.uniqueID == VehiclePart.EUNIQUE_ID.BODY_FLOATING_TYRE)
		{
			QuestSystem._pInstance.SetStatBoolean("PlayWithFloatingTyre", true);
		}
		if (part1.uniqueID == VehiclePart.EUNIQUE_ID.BODY_EXPLORER_UAV)
		{
			QuestSystem._pInstance.SetStatBoolean("PlayWithExplorerUAV", true);
		}
		if (minigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.FIRE)
		{
			QuestSystem._pInstance.IncrementStatInteger("numFireMissionPlays", 1);
		}
		else if (minigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.POLICE)
		{
			QuestSystem._pInstance.IncrementStatInteger("numPoliceMissionPlays", 1);
		}
		else if (minigameData.minigameCategory == MinigameManager.EMINIGAME_CATEGORY.VOLCANO)
		{
			QuestSystem._pInstance.IncrementStatInteger("numVolcanoMissionPlays", 1);
		}
		ResetMinigameSessionStats();
	}

	public void ResetMinigameSessionStats()
	{
		QuestSystem._pInstance.ResetStat("studCountSingleMission", Statistics.STAT_TYPE.INTEGER);
	}

	public void SessionStudsGained(int studsGained)
	{
		QuestSystem._pInstance.IncrementStatInteger("studCountSingleMission", studsGained);
		QuestSystem._pInstance.IncrementStatInteger("studCountMultiMission", studsGained);
	}

	public void SaveAll()
	{
		AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setString("ACHIEVEMENTS", AchievementSystem._pInstance.GetSaveString());
		AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setString("QUESTS", QuestSystem._pInstance.GetSaveString());
		AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setStringArray("UNCLAIMED_QUEST_REWARDS", _unclaimedRewards);
		for (int i = 0; i < _unclaimedRewards.Length; i++)
		{
			Debug.Log("Saving Unclaimed Reward " + i + " = " + _unclaimedRewards[i]);
		}
	}

	public void OnQuestCompleted(Achievement achi)
	{
		int num = _unclaimedRewards.Length;
		for (int i = 0; i < num; i++)
		{
			if (_unclaimedRewards[i] == "NONE")
			{
				_unclaimedRewards[i] = achi.achievementName;
				_pInstance.SaveAll();
				break;
			}
		}
	}

	public void OnQuestRewarded(string questName)
	{
		int num = _unclaimedRewards.Length;
		for (int i = 0; i < num; i++)
		{
			if (_unclaimedRewards[i] == questName)
			{
				if (Facades<TrackingFacade>.Instance != null)
				{
					Facades<TrackingFacade>.Instance.LogParameterMetric("Daily Task: Collected", new Dictionary<string, string> { { "Collected", questName } });
					Facades<TrackingFacade>.Instance.LogProgress("Task_" + questName + "_collected");
				}
				Debug.Log("On Quest Rewarded: " + questName + " marked down");
				_unclaimedRewards[i] = "NONE";
				break;
			}
		}
	}

	public int NumRewardsToClaim()
	{
		int num = 0;
		int num2 = _unclaimedRewards.Length;
		for (int i = 0; i < num2; i++)
		{
			if (_unclaimedRewards[i] != "NONE")
			{
				Debug.Log("Unclaimed Reward: " + _unclaimedRewards[i]);
				num++;
			}
		}
		return num;
	}

	private static bool HasKey(string saveKey)
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

	private static long GetLong(string saveKey)
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

	private static void SetLong(string saveKey, long value)
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
}
