using System;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using GameDefines;
using UnityEngine;

public class BuildingManager : InitialisationObject
{
	private const string BUILDING_MAIN_SCREEN = "FrontEndFlow.BaseBuilding";

	private const string BUILDING_INSTANCES_SAVE_KEY = "BuildingInstances";

	private const string TIME_EVENT_PREFIX = "Event_";

	public const char ENTRY_DELIMITER = ':';

	public static Action<BuildingInstance> _onBuildingLoaded;

	private static BuildingManager _instance;

	public float GLOBAL_COST_BALANCE_MODIFIER = 1f;

	public float GLOBAL_TIME_BALANCE_MODIFIER = 1f;

	public BuildingData[] _buildingDefinitions;

	public bool _doAutomaticallyCollectResources = true;

	public bool _useGlobalCap;

	public bool _useLocalCap;

	private static List<string> _unhandledTimeEvents = new List<string>();

	private bool _hasInited;

	[HideInInspector]
	public BuildingData.ResourceValue[] _storageCap;

	public static BuildingManager Instance
	{
		get
		{
			return _instance;
		}
	}

	public bool _pHasUnhandledTimeEvents
	{
		get
		{
			return _unhandledTimeEvents != null && _unhandledTimeEvents.Count > 0;
		}
	}

	public bool _pHasInitialised
	{
		get
		{
			return _hasInited;
		}
	}

	protected override void Awake()
	{
		if (_instance != null && _instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
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

	private void Update()
	{
		if (!_hasInited)
		{
			return;
		}
		for (int i = 0; i < _buildingDefinitions.Length; i++)
		{
			if (_buildingDefinitions[i]._pHasInstances)
			{
				_buildingDefinitions[i].Update();
			}
		}
	}

	public List<BuildingInstance> getAllBuildingInstances()
	{
		List<BuildingInstance> list = new List<BuildingInstance>();
		for (int i = 0; i < _buildingDefinitions.Length; i++)
		{
			if (_buildingDefinitions[i]._pHasInstances)
			{
				list.Add(_buildingDefinitions[i]._buildingInstances.ToArray());
			}
		}
		return list;
	}

	public BuildingDefines.EBuildingType[] getAllBuildingNames()
	{
		List<BuildingDefines.EBuildingType> list = new List<BuildingDefines.EBuildingType>();
		for (int i = 0; i < _buildingDefinitions.Length; i++)
		{
			if (_buildingDefinitions[i] != null)
			{
				list.Add(_buildingDefinitions[i]._type);
			}
		}
		return list.ToArray();
	}

	public override void startInitialising()
	{
		if (TimeManager.Instance == null || !TimeManager.Instance._pHasValidatedTime)
		{
			_currentState = InitialisationState.FINISHED;
			return;
		}
		_currentState = InitialisationState.INITIALISING;
		LoadState();
		setCurrencyLimits();
		UpdateVisuals();
		_hasInited = true;
	}

	public void cleanTimeEventList()
	{
		if (_unhandledTimeEvents.Count == 0)
		{
			return;
		}
		for (int num = _unhandledTimeEvents.Count - 1; num >= 0; num--)
		{
			BuildingData buildingDefinition = getBuildingDefinition(typeNameToEnum(_unhandledTimeEvents[num]));
			if (buildingDefinition == null)
			{
				_unhandledTimeEvents.RemoveAt(num);
				saveUnhandledTimeEvents();
			}
		}
	}

	public void HandleTimeEvents()
	{
		if (_unhandledTimeEvents.Count != 0)
		{
			for (int num = _unhandledTimeEvents.Count - 1; num >= 0; num--)
			{
				upgradeBuilding(_unhandledTimeEvents[num]);
				_unhandledTimeEvents.RemoveAt(num);
				saveUnhandledTimeEvents();
			}
		}
	}

	public string HandleOneTimeEvent()
	{
		if (_unhandledTimeEvents.Count == 0)
		{
			return null;
		}
		string text = _unhandledTimeEvents[0];
		_unhandledTimeEvents.RemoveAt(0);
		saveUnhandledTimeEvents();
		upgradeBuilding(text);
		return text;
	}

	public void SaveState()
	{
		if (_hasInited)
		{
			string text = string.Empty;
			for (int i = 0; i < _buildingDefinitions.Length; i++)
			{
				text = text + _buildingDefinitions[i].SaveState() + ((i != _buildingDefinitions.Length - 1) ? (string.Empty + ':') : string.Empty);
			}
			ObscuredPrefs.SetString("BuildingInstances", text);
			saveUnhandledTimeEvents();
		}
	}

	public void LoadState()
	{
		string text = ObscuredPrefs.GetString("BuildingInstances");
		if (!string.IsNullOrEmpty(text))
		{
			string[] array = text.Split(':');
			for (int i = 0; i < array.Length; i++)
			{
				if (string.IsNullOrEmpty(array[i]))
				{
					continue;
				}
				for (int j = 0; j < _buildingDefinitions.Length; j++)
				{
					if (array[i].StartsWith(_buildingDefinitions[j]._type.ToString()))
					{
						_buildingDefinitions[i].Load(array[i], _onBuildingLoaded);
						break;
					}
				}
			}
		}
		loadUnhandledTimeEvents();
		applyBalanceModifiers();
		_currentState = InitialisationState.FINISHED;
	}

	public BuildingInstance CreateNewBuilding(BuildingData buildingDefinition)
	{
		BuildingInstance buildingInstance = new BuildingInstance();
		buildingInstance._buildingDefinition = buildingDefinition;
		buildingInstance._uniqueId = generateNewUniqueBuildingId();
		buildingInstance.LoadState();
		buildingDefinition._buildingInstances.Add(buildingInstance);
		return buildingInstance;
	}

	public void RemoveBuilding(BuildingInstance instanceToRemove)
	{
		instanceToRemove._buildingDefinition._buildingInstances.Remove(instanceToRemove);
		instanceToRemove.Dispose();
		if (instanceToRemove._buildingDefinition._doesContributeToGlobalStorage)
		{
			setCurrencyLimits();
		}
	}

	public static void ClearUnhandledTimeEventsFromPlayerPrefs()
	{
		int num = 0;
		for (bool flag = ObscuredPrefs.HasKey("Event_" + num); flag || num < 10; flag = ObscuredPrefs.HasKey("Event_" + num))
		{
			if (flag)
			{
				ObscuredPrefs.DeleteKey("Event_" + num);
			}
			num++;
		}
	}

	public BuildingData getBuildingDefinition(BuildingDefines.EBuildingType typeId)
	{
		for (int i = 0; i < _buildingDefinitions.Length; i++)
		{
			if (_buildingDefinitions[i] != null && _buildingDefinitions[i]._type == typeId)
			{
				return _buildingDefinitions[i];
			}
		}
		return null;
	}

	public BuildingInstance getBuildingInstance(int instanceId)
	{
		if (instanceId < 0)
		{
			return null;
		}
		for (int i = 0; i < _buildingDefinitions.Length; i++)
		{
			for (int j = 0; j < _buildingDefinitions[i]._buildingInstances.Count; j++)
			{
				if (_buildingDefinitions[i]._buildingInstances[j] != null && _buildingDefinitions[i]._buildingInstances[j]._uniqueId == instanceId)
				{
					return _buildingDefinitions[i]._buildingInstances[j];
				}
			}
		}
		return null;
	}

	public BuildingInstance getFirstBuildingInstanceOfType(BuildingDefines.EBuildingType typeId)
	{
		BuildingData buildingDefinition = getBuildingDefinition(typeId);
		if (buildingDefinition == null || !buildingDefinition._pHasInstances)
		{
			return null;
		}
		return buildingDefinition._buildingInstances[0];
	}

	public int getHighestLevelOfBuildingType(BuildingDefines.EBuildingType typeId)
	{
		int num = 0;
		BuildingData buildingDefinition = getBuildingDefinition(typeId);
		for (int i = 0; i < buildingDefinition._buildingInstances.Count; i++)
		{
			if (buildingDefinition._buildingInstances[i] != null)
			{
				num = Mathf.Max(num, buildingDefinition._buildingInstances[i]._buildingLevel);
			}
		}
		return num;
	}

	public void StartUpgrade(BuildingInstance buildingToUpgrade)
	{
		if (buildingToUpgrade == null)
		{
			PersonalLogs.BobLog("building was null");
			return;
		}
		BuildingData.BuildingLevel nextLevel = buildingToUpgrade.getNextLevel();
		if (nextLevel == null)
		{
			PersonalLogs.BobLog("building level was null");
			return;
		}
		TimeSpan value = new TimeSpan(0, 0, nextLevel._upgradeSeconds);
		TimeEvent timeEvent = new TimeEvent();
		timeEvent.eventId = buildingToUpgrade._pUpgradeEventString;
		timeEvent.eventStartTime = TimeManager.GetCurrentTime().Ticks;
		timeEvent.eventTime = TimeManager.GetCurrentTime().Add(value).Ticks;
		TimeManager.Instance.AddTimeEvent(timeEvent);
	}

	public void InstantlyUpgradeBuilding(BuildingInstance buildingToUpgrade)
	{
		if (buildingToUpgrade != null)
		{
			if (TimeManager.Instance != null)
			{
				TimeManager.Instance.removeTimeEvent(buildingToUpgrade._pUpgradeEventString);
			}
			upgradeBuilding(buildingToUpgrade);
		}
	}

	public void UpdateVisuals()
	{
		for (int i = 0; i < _buildingDefinitions.Length; i++)
		{
			if (!_buildingDefinitions[i]._pHasInstances)
			{
				continue;
			}
			for (int j = 0; j < _buildingDefinitions[i]._buildingInstances.Count; j++)
			{
				if (_buildingDefinitions[i]._buildingInstances[j] != null)
				{
					_buildingDefinitions[i]._buildingInstances[j].UpdateVisuals();
				}
			}
		}
	}

	public BuildingInstance getFirstBuildingUpgradeInProgress()
	{
		for (int i = 0; i < _buildingDefinitions.Length; i++)
		{
			if (!_buildingDefinitions[i]._pHasInstances)
			{
				continue;
			}
			for (int j = 0; j < _buildingDefinitions[i]._buildingInstances.Count; j++)
			{
				if (_buildingDefinitions[i]._buildingInstances[j] != null && _buildingDefinitions[i]._buildingInstances[j]._pIsUpgrading)
				{
					return _buildingDefinitions[i]._buildingInstances[j];
				}
			}
		}
		return null;
	}

	public void setCurrencyLimits()
	{
		if (!_useGlobalCap && !_useLocalCap)
		{
			return;
		}
		BuildingData.ResourceValue[] array = new BuildingData.ResourceValue[CurrencyDefines._allTypes.Length];
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			array[i] = new BuildingData.ResourceValue();
			array[i]._type = CurrencyDefines._allTypes[i];
		}
		for (int j = 0; j < _buildingDefinitions.Length; j++)
		{
			if (!_buildingDefinitions[j]._pHasInstances || !_buildingDefinitions[j]._doesContributeToGlobalStorage)
			{
				continue;
			}
			for (int k = 0; k < _buildingDefinitions[j]._buildingInstances.Count; k++)
			{
				if (_buildingDefinitions[j]._buildingInstances[k] == null)
				{
					continue;
				}
				BuildingData.BuildingLevel level = _buildingDefinitions[j]._buildingInstances[k].getLevel();
				if (level._resourceStorage.Length == 0)
				{
					continue;
				}
				int l = 0;
				for (int num2 = level._resourceStorage.Length; l < num2; l++)
				{
					int m = 0;
					for (int num3 = array.Length; m < num3; m++)
					{
						if (level._resourceStorage[l]._type == array[m]._type)
						{
							array[m]._value += level._resourceStorage[l]._value;
							break;
						}
					}
				}
			}
		}
		setCurrencyLimits(array);
	}

	public string getLocalBuildingName(BuildingDefines.EBuildingType type)
	{
		return getBuildingDefinition(type)._pLocalisedName;
	}

	public BuildingData.ResourceValue getStorageCap(CurrencyDefines.CurrencyType type)
	{
		int i = 0;
		for (int num = _storageCap.Length; i < num; i++)
		{
			if (_storageCap[i]._type == type)
			{
				return _storageCap[i];
			}
		}
		return null;
	}

	private BuildingInstance findInstanceFromEvent(string eventString)
	{
		for (int i = 0; i < _buildingDefinitions.Length; i++)
		{
			if (!_buildingDefinitions[i]._pHasInstances)
			{
				continue;
			}
			int j = 0;
			for (int count = _buildingDefinitions[i]._buildingInstances.Count; j < count; j++)
			{
				if (!(_buildingDefinitions[i]._buildingInstances[j]._pUpgradeEventString != eventString))
				{
					return _buildingDefinitions[i]._buildingInstances[j];
				}
			}
		}
		return null;
	}

	private int generateNewUniqueBuildingId()
	{
		bool flag = true;
		int num = -1;
		do
		{
			num = UnityEngine.Random.Range(0, 1000000);
			for (int i = 0; i < _buildingDefinitions.Length; i++)
			{
				if (!_buildingDefinitions[i]._pHasInstances)
				{
					continue;
				}
				int j = 0;
				for (int count = _buildingDefinitions[i]._buildingInstances.Count; j < count; j++)
				{
					if (_buildingDefinitions[i]._buildingInstances[j]._uniqueId == num)
					{
						flag = false;
						break;
					}
				}
			}
		}
		while (!flag);
		return num;
	}

	private static void saveUnhandledTimeEvents()
	{
		ClearUnhandledTimeEventsFromPlayerPrefs();
		if (_unhandledTimeEvents.Count != 0)
		{
			for (int i = 0; i < _unhandledTimeEvents.Count; i++)
			{
				ObscuredPrefs.SetString("Event_" + i, _unhandledTimeEvents[i]);
			}
		}
	}

	private void loadUnhandledTimeEvents()
	{
		int num = 0;
		while (ObscuredPrefs.HasKey("Event_" + num))
		{
			_unhandledTimeEvents.Add(ObscuredPrefs.GetString("Event_" + num));
			num++;
		}
	}

	private void setCurrencyLimits(BuildingData.ResourceValue[] resources)
	{
		if (_useGlobalCap)
		{
			int i = 0;
			for (int num = resources.Length; i < num; i++)
			{
				if (resources[i]._value > 0)
				{
					CurrencyDefines._currencies[resources[i]._type].setLimit(resources[i]._value);
				}
			}
		}
		else if (_useLocalCap)
		{
			_storageCap = resources;
		}
	}

	private void applyBalanceModifiers()
	{
		int i = 0;
		for (int num = _buildingDefinitions.Length; i < num; i++)
		{
			if (_buildingDefinitions[i] != null)
			{
				_buildingDefinitions[i].applyBalanceModifiers();
			}
		}
	}

	private int toNearest(int baseValue, float decimalModifier, float roundingValue)
	{
		return Mathf.CeilToInt((float)baseValue * decimalModifier / roundingValue) * (int)roundingValue;
	}

	private void upgradeBuilding(BuildingInstance buildingToUpgrade)
	{
		if (buildingToUpgrade != null)
		{
			buildingToUpgrade.LevelUp();
			buildingToUpgrade.UpdateVisuals();
			if (buildingToUpgrade._buildingDefinition._doesContributeToGlobalStorage)
			{
				setCurrencyLimits();
			}
		}
	}

	private static void OnTimeEvent(string eventId)
	{
		if (Instance != null && Instance._hasInited)
		{
			Instance.upgradeBuilding(Instance.findInstanceFromEvent(eventId));
			return;
		}
		_unhandledTimeEvents.Add(eventId);
		saveUnhandledTimeEvents();
	}

	private BuildingDefines.EBuildingType typeNameToEnum(string typeName)
	{
		BuildingDefines.EBuildingType result = BuildingDefines.EBuildingType.RESOURCE_1_GENERATOR;
		try
		{
			result = (BuildingDefines.EBuildingType)(int)Enum.Parse(typeof(BuildingDefines.EBuildingType), typeName);
		}
		catch (ArgumentException)
		{
		}
		return result;
	}

	public BuildingInstance CreateNewBuilding(BuildingDefines.EBuildingType type)
	{
		return CreateNewBuilding(getBuildingDefinition(type));
	}

	public void StartUpgrade(int instanceId)
	{
		StartUpgrade(getBuildingInstance(instanceId));
	}

	public void InstantlyUpgradeBuilding(int instanceId)
	{
		InstantlyUpgradeBuilding(getBuildingInstance(instanceId));
	}

	private void upgradeBuilding(string eventString)
	{
		upgradeBuilding(findInstanceFromEvent(eventString));
	}

	private void upgradeBuilding(int instanceId)
	{
		upgradeBuilding(getBuildingInstance(instanceId));
	}

	public string getLocalBuildingName(string typeName)
	{
		return LocalisationFacade.Instance.GetString("BuildingNames." + typeName);
	}

	public string getLocalBuildingName(BuildingInstance instance)
	{
		return LocalisationFacade.Instance.GetString("BuildingNames." + instance._buildingDefinition._type);
	}

	public string getLocalBuildingName(BuildingData data)
	{
		return LocalisationFacade.Instance.GetString("BuildingNames." + data._type);
	}
}
