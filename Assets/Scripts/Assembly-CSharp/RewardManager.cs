using System;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
	public enum EBRICK_BAG_CATEGORY
	{
		INVALID = 0,
		SMALL_CHEST = 1,
		MEDIUM_CHEST = 2,
		LARGE_CHEST = 3
	}

	public class BrickBagRewardData
	{
		public int brickRewardColor;

		public int brickCount;

		public VehiclePartProperties rewardedPart;
	}

	private const string SAVE_KEY_REWARD_HISTORY = "RewardHistory";

	public int numInitialGoldChests = 5;

	public int rewardChestPhase = 3;

	private static RewardManager _instance;

	public int[] rewardTiers;

	public VehiclePart.EUNIQUE_ID[] priorityUnlocks;

	private List<VehiclePartProperties> _dummyRewardList = new List<VehiclePartProperties>();

	private List<BrickBagRewardData> _levelUpRewardedHistory = new List<BrickBagRewardData>();

	private bool _hasLoadedData;

	public static RewardManager _pInstance
	{
		get
		{
			return _instance;
		}
	}

	public List<BrickBagRewardData> _pLevelUpRewardedHistory
	{
		get
		{
			if (!_hasLoadedData)
			{
				LoadHistory();
				_hasLoadedData = true;
			}
			return _levelUpRewardedHistory;
		}
	}

	private void Awake()
	{
		_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void ResetAll()
	{
		_levelUpRewardedHistory.Clear();
	}

	public void LoadHistory()
	{
		int[] intArray = AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getIntArray("RewardHistory");
		_levelUpRewardedHistory.Clear();
		if (intArray == null)
		{
			return;
		}
		int num = intArray.Length;
		Debug.Log("<color=blue>LoadHistory - NumItems: " + num + "</color>");
		int num2;
		for (num2 = 0; num2 < num; num2++)
		{
			BrickBagRewardData brickBagRewardData = new BrickBagRewardData();
			brickBagRewardData.brickCount = intArray[num2];
			num2++;
			brickBagRewardData.brickRewardColor = intArray[num2];
			num2++;
			if (intArray[num2] != 0)
			{
				VehiclePartProperties partPropertiesFromPartID = VehiclePartManager._pInstance.GetPartPropertiesFromPartID((VehiclePart.EUNIQUE_ID)intArray[num2]);
				brickBagRewardData.rewardedPart = partPropertiesFromPartID;
			}
			_levelUpRewardedHistory.Add(brickBagRewardData);
		}
	}

	public void SaveHistory()
	{
		int[] array = new int[_levelUpRewardedHistory.Count * 3];
		int count = _levelUpRewardedHistory.Count;
		Debug.Log("<color=blue>Save History: " + count + " items</color>");
		int num = 0;
		for (int i = 0; i < count; i++)
		{
			array[num] = _levelUpRewardedHistory[i].brickCount;
			num++;
			array[num] = _levelUpRewardedHistory[i].brickRewardColor;
			num++;
			if (_levelUpRewardedHistory[i].rewardedPart == null)
			{
				array[num] = 0;
			}
			else
			{
				array[num] = (int)_levelUpRewardedHistory[i].rewardedPart.uniqueID;
			}
			num++;
		}
		AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setIntArray("RewardHistory", array);
	}

	public EBRICK_BAG_CATEGORY GetRewardChestForLevel(int level)
	{
		EBRICK_BAG_CATEGORY result = EBRICK_BAG_CATEGORY.INVALID;
		if (level < numInitialGoldChests)
		{
			result = EBRICK_BAG_CATEGORY.LARGE_CHEST;
		}
		else
		{
			int num = 0;
			if (rewardChestPhase != 0)
			{
				num = level % rewardChestPhase;
			}
			switch (num)
			{
			case 1:
				result = EBRICK_BAG_CATEGORY.SMALL_CHEST;
				break;
			case 2:
				result = EBRICK_BAG_CATEGORY.MEDIUM_CHEST;
				break;
			case 0:
				result = EBRICK_BAG_CATEGORY.LARGE_CHEST;
				break;
			}
		}
		return result;
	}

	public EBRICK_BAG_CATEGORY IssueLevelUpRewardFromLevel(int level, Action<BrickBagRewardData> onIssuedAction)
	{
		EBRICK_BAG_CATEGORY rewardChestForLevel = GetRewardChestForLevel(level);
		BrickBagRewardData brickBagRewardData = IssueReward(rewardChestForLevel, level);
		_levelUpRewardedHistory.Add(brickBagRewardData);
		SaveHistory();
		if (onIssuedAction != null)
		{
			onIssuedAction(brickBagRewardData);
		}
		return rewardChestForLevel;
	}

	public BrickBagRewardData IssueReward(EBRICK_BAG_CATEGORY rewardType, int level)
	{
		Debug.Log("Issuing Reward: " + rewardType);
		switch (rewardType)
		{
		case EBRICK_BAG_CATEGORY.SMALL_CHEST:
			return RewardSmallChest(level);
		case EBRICK_BAG_CATEGORY.MEDIUM_CHEST:
			return RewardMediumChest(level);
		case EBRICK_BAG_CATEGORY.LARGE_CHEST:
			return RewardLargeChest(level);
		default:
			return null;
		}
	}

	private int CurrentBrickRewardColor()
	{
		return (int)ScenarioManager._pInstance._pCurrentScenario.brickType;
	}

	private BrickBagRewardData RewardSmallChest(int level)
	{
		BrickBagRewardData brickBagRewardData = new BrickBagRewardData();
		brickBagRewardData.brickCount = 20;
		brickBagRewardData.brickRewardColor = CurrentBrickRewardColor();
		ScenarioManager._pInstance.AddBrickReward(brickBagRewardData.brickCount);
		GlobalInGameData._pCumulativeStuds += brickBagRewardData.brickCount * 10;
		return brickBagRewardData;
	}

	private BrickBagRewardData RewardMediumChest(int level)
	{
		BrickBagRewardData brickBagRewardData = new BrickBagRewardData();
		brickBagRewardData.brickCount = 75;
		brickBagRewardData.brickRewardColor = CurrentBrickRewardColor();
		ScenarioManager._pInstance.AddBrickReward(brickBagRewardData.brickCount);
		GlobalInGameData._pCumulativeStuds += brickBagRewardData.brickCount * 10;
		return brickBagRewardData;
	}

	private BrickBagRewardData RewardLargeChest(int level)
	{
		VehiclePartProperties vehiclePartUnlock = GetVehiclePartUnlock(level);
		BrickBagRewardData brickBagRewardData = new BrickBagRewardData();
		if (vehiclePartUnlock != null)
		{
			brickBagRewardData.rewardedPart = vehiclePartUnlock;
			GlobalInGameData.MarkItemUnlocked(vehiclePartUnlock);
		}
		else
		{
			brickBagRewardData.brickCount = 100;
			brickBagRewardData.brickRewardColor = CurrentBrickRewardColor();
			ScenarioManager._pInstance.AddBrickReward(brickBagRewardData.brickCount);
			GlobalInGameData._pCumulativeStuds += brickBagRewardData.brickCount * 10;
		}
		return brickBagRewardData;
	}

	private VehiclePartProperties GetVehiclePartUnlock(int level)
	{
		int num = VehiclePartManager._pInstance.allVehiclePartProperties.Length;
		_dummyRewardList.Clear();
		int num2 = 0;
		if (priorityUnlocks != null)
		{
			int num3 = priorityUnlocks.Length;
			for (int i = 0; i < num3; i++)
			{
				if (!GlobalInGameData.HasPartBeenUnlocked(priorityUnlocks[i]))
				{
					return VehiclePartManager._pInstance.GetPartPropertiesFromPartID(priorityUnlocks[i]);
				}
			}
		}
		for (int j = 0; j < num; j++)
		{
			VehiclePartProperties vehiclePartProperties = VehiclePartManager._pInstance.allVehiclePartProperties[j];
			bool flag = GlobalInGameData.HasPartBeenUnlocked(vehiclePartProperties.uniqueID);
			bool flag2 = level >= vehiclePartProperties.minimumLevelToUnlock;
			if (!flag && flag2 && vehiclePartProperties.partIsAvailable)
			{
				num2 += vehiclePartProperties.unlockWeight;
				_dummyRewardList.Add(vehiclePartProperties);
			}
		}
		int num4 = UnityEngine.Random.Range(0, num2);
		int count = _dummyRewardList.Count;
		Debug.Log("Trying " + count + " items. Diceroll = " + num4 + " / " + num2);
		int count2 = _dummyRewardList.Count;
		int num5 = 0;
		for (int k = 0; k < count2; k++)
		{
			int num6 = num5;
			int num7 = num5 + _dummyRewardList[k].unlockWeight;
			if (num4 >= num6 && num4 < num7)
			{
				Debug.Log("Found VehiclePart To Unlock:" + _dummyRewardList[k].uniqueID);
				return _dummyRewardList[k];
			}
			num5 = num7;
		}
		return null;
	}

	public float GetLevelProgressFromEXP(int exp)
	{
		int num = rewardTiers.Length;
		int num2 = num - 1;
		int num3 = rewardTiers[num2];
		if (exp == 0)
		{
			return 0f;
		}
		if (exp == num3)
		{
			return 0f;
		}
		if (exp < num3)
		{
			for (int i = 0; i < num; i++)
			{
				if (rewardTiers[i] > exp)
				{
					int num4 = rewardTiers[i];
					int num5 = 0;
					if (i != 0)
					{
						num5 = rewardTiers[i - 1];
					}
					int num6 = exp - num5;
					int num7 = num4 - num5;
					return (float)num6 / (float)num7;
				}
			}
		}
		else if (exp > num3)
		{
			float num8 = num3 - rewardTiers[num2 - 1];
			float num9 = exp;
			num9 -= (float)num3;
			bool flag = true;
			while (flag)
			{
				num9 -= num8;
				if (num9 < 0f)
				{
					num9 += num8;
					flag = false;
				}
			}
			return num9 / num8;
		}
		return 0f;
	}

	public int GetLevelFromEXP(int exp)
	{
		int num = rewardTiers.Length;
		int num2 = num - 1;
		int num3 = rewardTiers[num2];
		if (exp == num3)
		{
			return num2 + 1;
		}
		if (exp < num3)
		{
			int num4 = 0;
			for (int i = 0; i < num; i++)
			{
				if (rewardTiers[i] > exp)
				{
					return num4;
				}
				num4++;
			}
		}
		int num5 = num3 - rewardTiers[num - 2];
		int num6 = exp - num3;
		int num7 = num6 / num5 + 1;
		return num7 + num2;
	}
}
