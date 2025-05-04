using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

public class TroopManager : InitialisationObject
{
	[Serializable]
	public class TroopCapacity
	{
		public TroopData.eTroopDisplayStatus _troopType;

		public TroopCapAndBuilding[] _levels;
	}

	[Serializable]
	public class TroopCapAndBuilding
	{
		public int _capacity;

		public TroopData.BuildingRequirement[] _requirements;
	}

	public class TroopNumbersStorage
	{
		public string _troopName;

		public ObscuredInt _num;
	}

	private const string TRAINING_MAIN_SCREEN = "FrontEndFlow.BaseBuilding";

	private const string FREEBIE_KEY = "TutorialFreebie";

	private const string TIME_EVENT_PREFIX = "Event_{0}";

	public const string TRAINING_PREFIX = "Slot_{0}_{1}";

	public static bool IS_STARTING_TUTORIAL_MODE = false;

	public static bool IS_TUTORIAL_MODE = false;

	public static bool HAS_GOTTEN_TUTORIAL_FREEBIE = false;

	private static TroopManager _instance;

	public TroopCapacity[] _troopCaps;

	public TroopData[] _troops;

	private static List<string> _unhandledTimeEvents = new List<string>();

	private List<TroopNumbersStorage> _troopNumbersStorage = new List<TroopNumbersStorage>();

	private bool _hasInited;

	[CompilerGenerated]
	private static Dictionary<string, int> _003C_003Ef__switch_0024map15;

	public static TroopManager Instance
	{
		get
		{
			return _instance;
		}
	}

	public int _pMaxSimultaneousTraining
	{
		get
		{
			return 1;
		}
	}

	public int _pNumTroopsTraining
	{
		get
		{
			int num = 0;
			for (int i = 0; i < _troops.Length; i++)
			{
				if (_troops[i] != null && _troops[i]._pIsTraining)
				{
					num++;
				}
			}
			return num;
		}
	}

	public bool _pCanTrainMore
	{
		get
		{
			return _pNumTroopsTraining < _pMaxSimultaneousTraining;
		}
	}

	public int _pTotalNumberOfTroops
	{
		get
		{
			int num = 0;
			for (int i = 0; i < _troops.Length; i++)
			{
				if (_troops[i] != null && _troops[i]._troopDisplayStatus != TroopData.eTroopDisplayStatus.INVISIBLE)
				{
					num += _troops[i]._pNumStock;
				}
			}
			return num;
		}
	}

	protected override void Awake()
	{
		_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		InitialisationFacade.Instance.addToQueue(this);
		_currentState = InitialisationState.WAITING_TO_START;
	}

	private void Start()
	{
		if (TimeManager.Instance != null)
		{
			TimeManager instance = TimeManager.Instance;
			instance.OnTimeEvent = (Action<string>)Delegate.Combine(instance.OnTimeEvent, new Action<string>(OnTimeEvent));
		}
	}

	protected override void OnDestroy()
	{
		endTutorialMode();
		SaveState();
		if (TimeManager.Instance != null)
		{
			TimeManager instance = TimeManager.Instance;
			instance.OnTimeEvent = (Action<string>)Delegate.Remove(instance.OnTimeEvent, new Action<string>(OnTimeEvent));
		}
		if (!(_instance != this))
		{
			_instance = null;
		}
	}

	public void startTutorialMode()
	{
		if (IS_TUTORIAL_MODE)
		{
			return;
		}
		SaveState();
		IS_STARTING_TUTORIAL_MODE = true;
		_troopNumbersStorage.Clear();
		for (int i = 0; i < _troops.Length; i++)
		{
			if (_troops[i] != null && _troops[i]._troopDisplayStatus != TroopData.eTroopDisplayStatus.INVISIBLE)
			{
				TroopNumbersStorage troopNumbersStorage = new TroopNumbersStorage();
				troopNumbersStorage._troopName = _troops[i]._name;
				troopNumbersStorage._num = _troops[i]._pNumStock;
				_troopNumbersStorage.Add(troopNumbersStorage);
			}
		}
		clearAllTroops();
		IS_STARTING_TUTORIAL_MODE = false;
		IS_TUTORIAL_MODE = true;
	}

	public void endTutorialMode()
	{
		if (!IS_TUTORIAL_MODE)
		{
			return;
		}
		clearAllTroops();
		for (int i = 0; i < _troops.Length; i++)
		{
			if (_troops[i] != null && _troops[i]._troopDisplayStatus != TroopData.eTroopDisplayStatus.INVISIBLE)
			{
				_troops[i].AddTroop(_troopNumbersStorage[i]._num);
			}
		}
		IS_TUTORIAL_MODE = false;
	}

	public int getNumTroopsByType(TroopData.eTroopDisplayStatus _type)
	{
		if (_troops == null || _troops.Length == 0)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < _troops.Length; i++)
		{
			if (_troops[i] != null && _troops[i]._troopDisplayStatus == _type)
			{
				num += _troops[i]._pNumStock;
			}
		}
		return num;
	}

	public int getTroopCapacityByType(TroopData.eTroopDisplayStatus _type)
	{
		if (_troops == null || _troops.Length == 0 || _troopCaps == null || Instance._troopCaps.Length == 0)
		{
			return int.MaxValue;
		}
		TroopCapacity troopCapacity = null;
		for (int i = 0; i < _troopCaps.Length; i++)
		{
			if (_troopCaps[i]._troopType == _type)
			{
				troopCapacity = _troopCaps[i];
			}
		}
		if (troopCapacity == null)
		{
			return int.MaxValue;
		}
		int result = 0;
		bool flag = true;
		int num = 0;
		for (int j = 0; j < troopCapacity._levels.Length; j++)
		{
			if (troopCapacity._levels[j] == null || troopCapacity._levels[j]._requirements == null)
			{
				continue;
			}
			flag = true;
			for (int k = 0; k < troopCapacity._levels[j]._requirements.Length; k++)
			{
				if (troopCapacity._levels[j]._requirements[k] != null)
				{
					num = BuildingManager.Instance.getHighestLevelOfBuildingType(troopCapacity._levels[j]._requirements[k]._buildingType);
					if (num < troopCapacity._levels[j]._requirements[k]._buildingLevelNeeded)
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				result = troopCapacity._levels[j]._capacity;
			}
		}
		return result;
	}

	public string[] getAllTroopNames(bool onlyIncludeTroopStocks = true)
	{
		List<string> list = new List<string>();
		for (int i = 0; i < _troops.Length; i++)
		{
			if (_troops[i] != null && _troops[i]._troopDisplayStatus != TroopData.eTroopDisplayStatus.INVISIBLE)
			{
				list.Add(_troops[i]._name);
			}
		}
		return list.ToArray();
	}

	public override void startInitialising()
	{
		if (!TimeManager.Instance._pHasValidatedTime)
		{
			_currentState = InitialisationState.FINISHED;
			return;
		}
		_currentState = InitialisationState.INITIALISING;
		LoadState();
		HandleTimeEvents();
		HAS_GOTTEN_TUTORIAL_FREEBIE = ObscuredPrefs.GetBool("TutorialFreebie", false);
		_hasInited = true;
	}

	public void GotTutorialFreebie()
	{
		HAS_GOTTEN_TUTORIAL_FREEBIE = true;
		ObscuredPrefs.SetBool("TutorialFreebie", true);
	}

	public void HandleTimeEvents()
	{
		if (_unhandledTimeEvents.Count != 0)
		{
			for (int num = _unhandledTimeEvents.Count - 1; num >= 0; num--)
			{
				string troopId = _unhandledTimeEvents[num].Replace(string.Format("Slot_{0}_{1}", 0, string.Empty), string.Empty);
				Instance.addTroop(troopId);
				_unhandledTimeEvents.RemoveAt(num);
			}
		}
	}

	public void SaveState()
	{
		if (!_hasInited || IS_TUTORIAL_MODE)
		{
			return;
		}
		for (int i = 0; i < _troops.Length; i++)
		{
			if (_troops[i] != null)
			{
				_troops[i].SaveState();
			}
		}
		ClearPlayerPrefs();
		if (_unhandledTimeEvents.Count != 0)
		{
			for (int j = 0; j < _unhandledTimeEvents.Count; j++)
			{
				ObscuredPrefs.SetString(string.Format("Event_{0}", j.ToString()), _unhandledTimeEvents[j]);
			}
		}
	}

	public void LoadState()
	{
		for (int i = 0; i < _troops.Length; i++)
		{
			if (_troops[i] != null)
			{
				_troops[i].LoadState();
			}
		}
		int num = 0;
		while (ObscuredPrefs.HasKey(string.Format("Event_{0}", num.ToString())))
		{
			_unhandledTimeEvents.Add(ObscuredPrefs.GetString(string.Format("Event_{0}", num.ToString())));
			num++;
		}
		ClearPlayerPrefs();
		_currentState = InitialisationState.FINISHED;
	}

	public void ClearPlayerPrefs()
	{
		int num = 0;
		while (ObscuredPrefs.HasKey(string.Format("Event_{0}", num.ToString())))
		{
			ObscuredPrefs.DeleteKey(string.Format("Event_{0}", num.ToString()));
			num++;
		}
	}

	public TroopData getTroop(string troopId)
	{
		if (string.IsNullOrEmpty(troopId))
		{
			return null;
		}
		for (int i = 0; i < _troops.Length; i++)
		{
			if (_troops[i] != null && !(_troops[i]._name != troopId))
			{
				return _troops[i];
			}
		}
		return null;
	}

	public TroopData getTroopByPrefabName(string troopId)
	{
		if (string.IsNullOrEmpty(troopId))
		{
			return null;
		}
		for (int i = 0; i < _troops.Length; i++)
		{
			if (_troops[i] != null && !(_troops[i]._prefabName != troopId))
			{
				return _troops[i];
			}
		}
		return null;
	}

	public void StartTraining(int trainingSlot, TroopData troopToUpgrade)
	{
		if (troopToUpgrade == null)
		{
			PersonalLogs.BobLog("troop was null");
			return;
		}
		TroopData.TrainingCost troopCost = troopToUpgrade._troopCost;
		if (troopCost == null)
		{
			PersonalLogs.BobLog("training cost was null");
			return;
		}
		troopToUpgrade._lastTrainingIndex = trainingSlot;
		TimeSpan value = new TimeSpan(0, 0, troopCost._trainingSeconds);
		TimeEvent timeEvent = new TimeEvent();
		timeEvent.eventId = string.Format("Slot_{0}_{1}", trainingSlot.ToString(), troopToUpgrade._name);
		timeEvent.eventStartTime = TimeManager.GetCurrentTime().Ticks;
		timeEvent.eventTime = TimeManager.GetCurrentTime().Add(value).Ticks;
		TimeManager.Instance.AddTimeEvent(timeEvent);
	}

	public void InstantlyFinishTraining(TroopData troopData)
	{
		if (troopData != null)
		{
			troopData.stopTraining();
			troopData.AddTroop();
		}
	}

	public void InstantlyAddTroop(TroopData troopToAdd, int quantity = 1)
	{
		if (troopToAdd != null)
		{
			for (int i = 0; i < quantity; i++)
			{
				addTroop(troopToAdd);
			}
		}
	}

	public void SuspendTroop(TroopData troopToSuspend)
	{
		if (troopToSuspend != null)
		{
			troopToSuspend.SuspendTroop();
		}
	}

	public void DeductTroop(TroopData troopToDeductFrom)
	{
		if (troopToDeductFrom != null)
		{
			troopToDeductFrom.DeductTroop();
		}
	}

	public void UnsuspendAllTroops()
	{
		for (int i = 0; i < _troops.Length; i++)
		{
			if (_troops[i] != null)
			{
				_troops[i].UnsuspendAllTroops();
			}
		}
	}

	public TroopData getTroopInTraining()
	{
		for (int i = 0; i < _troops.Length; i++)
		{
			if (_troops[i] != null && _troops[i]._pIsTraining)
			{
				return _troops[i];
			}
		}
		return null;
	}

	public string getLocalisedTroopName(string troopId)
	{
		TroopData troop = getTroop(troopId);
		if (troop == null)
		{
			return troopId;
		}
		return troop._pLocalisedName;
	}

	private void addTroop(TroopData troopToAdd)
	{
		if (troopToAdd == null)
		{
			return;
		}
		if (IS_TUTORIAL_MODE)
		{
			for (int i = 0; i < _troopNumbersStorage.Count; i++)
			{
				if (!(_troopNumbersStorage[i]._troopName != troopToAdd._name))
				{
					++_troopNumbersStorage[i]._num;
					break;
				}
			}
		}
		else
		{
			troopToAdd.AddTroop();
		}
	}

	private static void OnTimeEvent(string eventId)
	{
		if (Instance != null && Instance._hasInited)
		{
			string troopId = eventId.Replace(string.Format("Slot_{0}_{1}", 0, string.Empty), string.Empty);
			Instance.addTroop(troopId);
		}
		else
		{
			_unhandledTimeEvents.Add(eventId);
		}
	}

	private void clearAllTroops()
	{
		for (int i = 0; i < _troops.Length; i++)
		{
			if (_troops[i] != null && _troops[i]._troopDisplayStatus != TroopData.eTroopDisplayStatus.INVISIBLE)
			{
				_troops[i].DeductTroop(_troops[i]._pNumStock);
			}
		}
	}

	public void SuspendTroop(string troopId)
	{
		SuspendTroop(getTroop(troopId));
	}

	public void DeductTroop(string troopId)
	{
		DeductTroop(getTroop(troopId));
	}

	public void StartTraining(string troopId)
	{
		StartTraining(0, getTroop(troopId));
	}

	public void InstantlyAddTroop(string troopId, int quantity = 1)
	{
		InstantlyAddTroop(getTroop(troopId), quantity);
	}

	public void InstantlyFinishTraining(string troopName)
	{
		InstantlyFinishTraining(getTroop(troopName));
	}

	private void addTroop(string troopId)
	{
		addTroop(getTroop(troopId));
	}

	public int getSoldierPoints(string troopId)
	{
		if (troopId != null)
		{
			if (_003C_003Ef__switch_0024map15 == null)
			{
				_003C_003Ef__switch_0024map15 = new Dictionary<string, int>(0);
			}
			int value;
			if (!_003C_003Ef__switch_0024map15.TryGetValue(troopId, out value))
			{
			}
		}
		return 10;
	}
}
