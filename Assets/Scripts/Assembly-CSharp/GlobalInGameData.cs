using System;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInGameData
{
	private const string UNLOCKED_PARTS_SAVE_KEY = "UnlockedParts";

	public const string CURR_XP_SAVE_KEY = "CurrentXP";

	public const string CUMULATIVE_STUDS_SAVE_KEY = "CumulativeStuds";

	private const string CLAMIED_REWARDS_SAVE_KEY = "ClaimedRewards";

	private const string UNCLAMIED_REWARDS_SAVE_KEY = "UnclaimedRewards";

	private const string HAS_DONE_FIRST_VISIT_KEY = "HasDoneFirstVisit";

	private const string HAS_SEEN_EXP_TUTORIAL_KEY = "HasSeenExpTutorial";

	private const string HAS_SEEN_GARAGE_TUTORIAL_KEY = "HasSeenGarageTutorial";

	private const string HAS_COMPLETED_FTUE_KEY = "HasCompletedFTUE";

	private const string PARTS_VIEWED_KEY = "PartsViewed";

	private const string LEADERBOARD_NAME = "CumulativeStuds";

	private const string HUB_SCENE_NAME = "01HubCity";

	private const string EMPTY_SCENE_NAME = "Empty";

	public static Action _OnHUBWillBeUnloaded;

	public static Action _OnMiniGameWillBeUnloaded;

	private static bool _hasAssignedCallbacks;

	private static VehiclePart.EPART_SLOT_TYPE _garagePartType = VehiclePart.EPART_SLOT_TYPE.INVALID;

	public static Action<VehiclePart.EUNIQUE_ID> _onPartUnlocked;

	private static bool _hasCompletedFTUE = false;

	private static bool _hasDoneFirstTimeVisit = false;

	private static bool _hasSeenExpTutorial = false;

	private static bool _hasSeenGarageTutorial = false;

	private static bool _wantFullCarousel = false;

	private static int _unclaimedDailyRewards = 0;

	private static int _claimedRewards = 0;

	private static int _currentEXP = 0;

	private static int _cumulativeStuds = 0;

	private static bool _hasSeenTutorialVehicleBuilder = false;

	private static string _currentSocialCity = null;

	private static Dictionary<VehiclePart.EUNIQUE_ID, bool> _vehiclePartUnlockLookup;

	private static bool[] _partsViewedLookup;

	private static List<VehiclePart.EUNIQUE_ID> _unlockedParts = new List<VehiclePart.EUNIQUE_ID>();

	public static VehiclePart.EPART_SLOT_TYPE _pGaragePartType
	{
		get
		{
			return _garagePartType;
		}
		set
		{
			_garagePartType = value;
		}
	}

	public static int _pUnclaimedDailyRewardChests
	{
		get
		{
			if (_unclaimedDailyRewards == 0)
			{
				_unclaimedDailyRewards = AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getInt("UnclaimedRewards");
			}
			return _unclaimedDailyRewards;
		}
		set
		{
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setInt("UnclaimedRewards", value);
			_unclaimedDailyRewards = value;
		}
	}

	public static int _pClaimedRewards
	{
		get
		{
			if (_claimedRewards == 0)
			{
				_claimedRewards = AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getInt("ClaimedRewards");
			}
			return _claimedRewards;
		}
		set
		{
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setInt("ClaimedRewards", value);
			_claimedRewards = value;
		}
	}

	public static int _pCurrentExp
	{
		get
		{
			if (_currentEXP == 0)
			{
				_currentEXP = AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getInt("CurrentXP");
			}
			return _currentEXP;
		}
		set
		{
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setInt("CurrentXP", value);
			_currentEXP = value;
		}
	}

	public static int _pCumulativeStuds
	{
		get
		{
			if (_cumulativeStuds == 0)
			{
				_cumulativeStuds = AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getInt("CumulativeStuds");
			}
			return _cumulativeStuds;
		}
		set
		{
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setInt("CumulativeStuds", value);
			_cumulativeStuds = value;
			UserData userData = new UserData();
			userData._name = DatabaseFacade.Instance._userData._name;
			userData.addData("leaderboardName", "CumulativeStuds");
			userData.addData("score", _cumulativeStuds.ToString());
			DatabaseFacade.Instance.saveUserToAllActiveDatabases(userData, null, null);
		}
	}

	public static string _pCurrentSocialCity
	{
		get
		{
			return _currentSocialCity;
		}
		set
		{
			_currentSocialCity = value;
		}
	}

	public static bool _pHasSeenTutorialVehicleBuilder
	{
		get
		{
			return _hasSeenTutorialVehicleBuilder;
		}
		set
		{
			_hasSeenTutorialVehicleBuilder = value;
		}
	}

	public static bool _pWantFullCarousel
	{
		get
		{
			return _wantFullCarousel;
		}
		set
		{
			_wantFullCarousel = value;
		}
	}

	public static bool _pHasDoneFirstTimeVisit
	{
		get
		{
			if (!_hasDoneFirstTimeVisit)
			{
				_hasDoneFirstTimeVisit = AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getBool("HasDoneFirstVisit");
			}
			return _hasDoneFirstTimeVisit;
		}
		set
		{
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setBool("HasDoneFirstVisit", value);
			_hasDoneFirstTimeVisit = value;
		}
	}

	public static bool _pHasSeenEXPTutorial
	{
		get
		{
			if (!_hasSeenExpTutorial)
			{
				_hasSeenExpTutorial = AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getBool("HasSeenExpTutorial");
			}
			return _hasSeenExpTutorial;
		}
		set
		{
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setBool("HasSeenExpTutorial", value);
			_hasSeenExpTutorial = value;
		}
	}

	public static bool _pHasSeenGarageTutorial
	{
		get
		{
			if (!_hasSeenGarageTutorial)
			{
				_hasSeenGarageTutorial = AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getBool("HasSeenGarageTutorial");
			}
			return _hasSeenGarageTutorial;
		}
		set
		{
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setBool("HasSeenGarageTutorial", value);
			_hasSeenGarageTutorial = value;
		}
	}

	public static bool _pHasCompletedFTUE
	{
		get
		{
			if (!_hasCompletedFTUE)
			{
				_hasCompletedFTUE = AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getBool("HasCompletedFTUE");
			}
			return _hasCompletedFTUE;
		}
		set
		{
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setBool("HasCompletedFTUE", value);
			_hasCompletedFTUE = value;
		}
	}

	public static int _pTotalNumParts
	{
		get
		{
			if (_vehiclePartUnlockLookup == null)
			{
				GenerateVehiclePartLookupTruthTable();
			}
			return _vehiclePartUnlockLookup.Count;
		}
	}

	public static int _pNumPartsUnlocked
	{
		get
		{
			if (_vehiclePartUnlockLookup == null)
			{
				GenerateVehiclePartLookupTruthTable();
			}
			return _unlockedParts.Count;
		}
	}

	public static void AssignCallbacks()
	{
		if (!_hasAssignedCallbacks)
		{
			_hasAssignedCallbacks = true;
			SceneLoader._OnWillLoadLevel = (Action<string, string>)Delegate.Combine(SceneLoader._OnWillLoadLevel, new Action<string, string>(OnLevelWillLoad));
		}
	}

	public static void OnLevelWillLoad(string nextLevelName, string previousLevelName)
	{
		if (nextLevelName == "Empty")
		{
			if (_OnHUBWillBeUnloaded != null)
			{
				_OnHUBWillBeUnloaded();
			}
			if (_OnMiniGameWillBeUnloaded != null)
			{
				_OnMiniGameWillBeUnloaded();
			}
		}
		else if (previousLevelName == "01HubCity" && nextLevelName != "01HubCity")
		{
			if (_OnHUBWillBeUnloaded != null)
			{
				_OnHUBWillBeUnloaded();
			}
		}
		else if (previousLevelName != "01HubCity" && nextLevelName == "01HubCity" && _OnMiniGameWillBeUnloaded != null)
		{
			_OnMiniGameWillBeUnloaded();
		}
	}

	public static bool TriggeredCode(string codeName)
	{
		return AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getBool(codeName);
	}

	public static void SetCodeTriggered(string codeName, bool value)
	{
		AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setBool(codeName, value);
	}

	public static void ResetAll()
	{
		_unclaimedDailyRewards = 0;
		_claimedRewards = 0;
		_currentEXP = 0;
		_hasSeenTutorialVehicleBuilder = false;
		_currentSocialCity = null;
		_unlockedParts.Clear();
		int num = 90;
		if (_vehiclePartUnlockLookup != null)
		{
			for (int i = 0; i < num; i++)
			{
				_vehiclePartUnlockLookup[(VehiclePart.EUNIQUE_ID)i] = false;
			}
		}
		if (_partsViewedLookup != null)
		{
			for (int j = 0; j < _partsViewedLookup.Length; j++)
			{
				_partsViewedLookup[j] = false;
			}
		}
		_hasCompletedFTUE = false;
		_hasDoneFirstTimeVisit = false;
		_hasSeenExpTutorial = false;
		_hasSeenGarageTutorial = false;
		ScreenTutorialVehicleReverse._pHasShown = false;
	}

	public static bool HasPartBeenUnlocked(VehiclePart.EUNIQUE_ID partID)
	{
		if (_vehiclePartUnlockLookup == null)
		{
			GenerateVehiclePartLookupTruthTable();
		}
		return _vehiclePartUnlockLookup[partID];
	}

	public static void MarkItemUnlocked(VehiclePartProperties part)
	{
		if (_vehiclePartUnlockLookup == null)
		{
			GenerateVehiclePartLookupTruthTable();
		}
		if (_unlockedParts.Contains(part.uniqueID))
		{
			Debug.LogError(string.Concat("Part [", part.uniqueID, "] has already been unlocked!"));
			return;
		}
		_vehiclePartUnlockLookup[part.uniqueID] = true;
		_unlockedParts.Add(part.uniqueID);
		int[] array = new int[_unlockedParts.Count];
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			array[i] = (int)_unlockedParts[i];
		}
		AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setIntArray("UnlockedParts", array);
		AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setInt("CITY_GARAGE", 0);
		if (_onPartUnlocked != null)
		{
			_onPartUnlocked(part.uniqueID);
		}
	}

	private static void GenerateVehiclePartLookupTruthTable()
	{
		_vehiclePartUnlockLookup = new Dictionary<VehiclePart.EUNIQUE_ID, bool>();
		int num = 90;
		for (int i = 0; i < num; i++)
		{
			_vehiclePartUnlockLookup[(VehiclePart.EUNIQUE_ID)i] = false;
		}
		int[] intArray = AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getIntArray("UnlockedParts");
		if (intArray != null)
		{
			List<int> list = new List<int>(intArray);
			int j = 0;
			for (int count = list.Count; j < count; j++)
			{
				VehiclePart.EUNIQUE_ID item = (VehiclePart.EUNIQUE_ID)list[j];
				if (!_unlockedParts.Contains(item))
				{
					_unlockedParts.Add(item);
				}
			}
		}
		num = _unlockedParts.Count;
		for (int k = 0; k < num; k++)
		{
			_vehiclePartUnlockLookup[_unlockedParts[k]] = true;
		}
	}

	public static void SetPartViewed(VehiclePart.EUNIQUE_ID part)
	{
		LoadPartsViewed();
		_partsViewedLookup[(int)part] = true;
		AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setBoolArray("PartsViewed", _partsViewedLookup);
	}

	public static void SetCategoryViewed(VehiclePart.EPART_SLOT_TYPE category)
	{
		LoadPartsViewed();
		int num = VehiclePartManager._pInstance.allVehiclePartProperties.Length;
		for (int i = 0; i < num; i++)
		{
			if (VehiclePartManager._pInstance.allVehiclePartProperties[i].slotType == category)
			{
				bool partIsAvailable = VehiclePartManager._pInstance.allVehiclePartProperties[i].partIsAvailable;
				if (HasPartBeenUnlocked(VehiclePartManager._pInstance.allVehiclePartProperties[i].uniqueID) && partIsAvailable)
				{
					_partsViewedLookup[(int)VehiclePartManager._pInstance.allVehiclePartProperties[i].uniqueID] = true;
				}
			}
		}
		AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setBoolArray("PartsViewed", _partsViewedLookup);
	}

	public static bool IsPartNew(VehiclePart.EUNIQUE_ID part)
	{
		LoadPartsViewed();
		if (!HasPartBeenUnlocked(part))
		{
			return false;
		}
		int num = VehiclePartManager._pInstance.allVehiclePartProperties.Length;
		for (int i = 0; i < num; i++)
		{
			if (VehiclePartManager._pInstance.allVehiclePartProperties[i].uniqueID == part)
			{
				if (!VehiclePartManager._pInstance.allVehiclePartProperties[i].partIsAvailable)
				{
					return false;
				}
				break;
			}
		}
		return !_partsViewedLookup[(int)part];
	}

	public static bool IsAnyAvaliblePartNew()
	{
		LoadPartsViewed();
		int num = VehiclePartManager._pInstance.allVehiclePartProperties.Length;
		for (int i = 0; i < num; i++)
		{
			if (HasPartBeenUnlocked(VehiclePartManager._pInstance.allVehiclePartProperties[i].uniqueID) && VehiclePartManager._pInstance.allVehiclePartProperties[i].partIsAvailable && !_partsViewedLookup[(int)VehiclePartManager._pInstance.allVehiclePartProperties[i].uniqueID])
			{
				return true;
			}
		}
		return false;
	}

	private static void LoadPartsViewed()
	{
		int num = 91;
		if (_partsViewedLookup != null && _partsViewedLookup.Length == num)
		{
			return;
		}
		if (AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.hasKey("PartsViewed"))
		{
			_partsViewedLookup = AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getBoolArray("PartsViewed");
		}
		if (_partsViewedLookup != null && _partsViewedLookup.Length == num)
		{
			return;
		}
		if (_partsViewedLookup != null)
		{
			bool[] array = new bool[_partsViewedLookup.Length];
			_partsViewedLookup.CopyTo(array, 0);
			_partsViewedLookup = new bool[num];
			for (int i = 0; i < array.Length; i++)
			{
				if (_partsViewedLookup.Length > i)
				{
					_partsViewedLookup[i] = array[i];
				}
			}
		}
		else
		{
			_partsViewedLookup = new bool[num];
		}
		_partsViewedLookup[0] = true;
		_partsViewedLookup[90] = true;
	}
}
