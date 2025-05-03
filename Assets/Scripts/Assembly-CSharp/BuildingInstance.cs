using System;
using CodeStage.AntiCheat.ObscuredTypes;
using GameDefines;
using UnityEngine;

public class BuildingInstance
{
	public const string UPGRADE_KEY = "Upgrade";

	private const float POLL_FREQUENCY_BASE = 1f;

	private const float POLL_FREQUENCY_RANDOM = 0.2f;

	private const float SIXTIETH = 1f / 60f;

	public const char SAVE_DELIMITER = '^';

	private const string LEVEL_SAVE_KEY = "Level";

	private const string LAST_UPDATE_SAVE_KEY = "LastUpdate";

	private const string VISUALS_SAVE_KEY = "VisualData";

	private static string SAVE_TEMPLATE = "{0}" + '^' + "{1}" + '^' + "{2}";

	public int _uniqueId;

	public string _visualDataFromLastLoad = string.Empty;

	public BuildingData _buildingDefinition;

	public ObscuredInt _buildingLevel;

	public BuildingData.ResourceValue[] _currentStoredResources = new BuildingData.ResourceValue[0];

	public BuildingVisual _relatedVisuals;

	private ObscuredLong _lastStoresUpdate;

	private float _pollTimer;

	public bool _pHasStorage
	{
		get
		{
			return _buildingDefinition.hasStorage(_buildingLevel);
		}
	}

	public bool _pHasProduction
	{
		get
		{
			return _buildingDefinition.hasProduction(_buildingLevel);
		}
	}

	public bool _pHasUncollectedResources
	{
		get
		{
			BuildingData.BuildingLevel level = getLevel();
			int i = 0;
			for (int num = _currentStoredResources.Length; i < num; i++)
			{
				if (_currentStoredResources[i]._value <= 0)
				{
					continue;
				}
				if (_buildingDefinition._resourceCollectionThreshold > 0f)
				{
					BuildingData.ResourceValue storageCapacity = level.getStorageCapacity(_currentStoredResources[i]._type);
					if (storageCapacity == null)
					{
						return true;
					}
					return (float)_currentStoredResources[i]._value > (float)storageCapacity._value * _buildingDefinition._resourceCollectionThreshold;
				}
				return true;
			}
			return false;
		}
	}

	public bool _pCanCollectedResources
	{
		get
		{
			int i = 0;
			for (int num = _currentStoredResources.Length; i < num; i++)
			{
				if (_currentStoredResources[i]._value > 0)
				{
					return !CurrencyDefines._currencies[_currentStoredResources[i]._type]._pIsMaxedOut;
				}
			}
			return false;
		}
	}

	public bool _pIsMaxLevel
	{
		get
		{
			return (int)_buildingLevel >= _buildingDefinition._levels.Length - 1;
		}
	}

	public bool _pCanPlayerAffordUpgrade
	{
		get
		{
			BuildingData.BuildingLevel nextLevel = getNextLevel();
			if (nextLevel == null)
			{
				return false;
			}
			int i = 0;
			for (int num = nextLevel._upgradeCost.Length; i < num; i++)
			{
				if (nextLevel._upgradeCost[i]._value > CurrencyDefines._currencies[nextLevel._upgradeCost[i]._type]._displayValue)
				{
					return false;
				}
			}
			return true;
		}
	}

	public bool _pHaveBuildingRequirementsForNextLevelBeenMet
	{
		get
		{
			BuildingData.BuildingLevel nextLevel = getNextLevel();
			if (nextLevel == null)
			{
				return false;
			}
			if (nextLevel._requirements == null || nextLevel._requirements.Length == 0)
			{
				return true;
			}
			int num = 0;
			for (int i = 0; i < nextLevel._requirements.Length; i++)
			{
				num = BuildingManager.Instance.getHighestLevelOfBuildingType(nextLevel._requirements[i]._buildingType);
				if (num < nextLevel._requirements[i]._buildingLevelNeeded)
				{
					return false;
				}
			}
			return true;
		}
	}

	public bool _pIsUpgrading
	{
		get
		{
			return TimeManager.Instance.HasTimeEvent(getSaveString("Upgrade"));
		}
	}

	public float _pUpgradeProgess
	{
		get
		{
			return TimeManager.Instance.GetDecimalProgress(getSaveString("Upgrade"));
		}
	}

	public string _pLocalisedName
	{
		get
		{
			if (_buildingDefinition == null)
			{
				return string.Empty;
			}
			return _buildingDefinition._pLocalisedName;
		}
	}

	public string _pUpgradeEventString
	{
		get
		{
			return getSaveString("Upgrade");
		}
	}

	public void SaveState()
	{
		ClearPlayerPrefs();
		SavePref("Level", _buildingLevel);
		if (_relatedVisuals != null)
		{
			_visualDataFromLastLoad = _relatedVisuals.serialiseVisualData();
			SavePref("VisualData", _visualDataFromLastLoad);
		}
		else if (!string.IsNullOrEmpty(_visualDataFromLastLoad))
		{
			SavePref("VisualData", _visualDataFromLastLoad);
		}
		int i = 0;
		for (int num = _currentStoredResources.Length; i < num; i++)
		{
			SavePref(_currentStoredResources[i]._type.ToString(), _currentStoredResources[i]._value);
		}
		if (_pHasProduction && _pHasStorage)
		{
			ObscuredPrefs.SetLong(getSaveString("LastUpdate"), _lastStoresUpdate);
		}
	}

	public void LoadState()
	{
		_buildingLevel = LoadPref("Level");
		if (ObscuredPrefs.HasKey(getSaveString("VisualData")))
		{
			_visualDataFromLastLoad = ObscuredPrefs.GetString(getSaveString("VisualData"));
			if (_relatedVisuals != null)
			{
				_relatedVisuals.setVisualDataFromString(_visualDataFromLastLoad);
			}
		}
		int num = 0;
		int i = 0;
		for (int num2 = CurrencyDefines._allTypes.Length; i < num2; i++)
		{
			num = LoadPref(CurrencyDefines._allTypes[i].ToString());
			changeCurrencyStorage(CurrencyDefines._allTypes[i], num);
		}
		if (_pHasProduction && _pHasStorage && ObscuredPrefs.HasKey(getSaveString("LastUpdate")))
		{
			_lastStoresUpdate = ObscuredPrefs.GetLong(getSaveString("LastUpdate"));
		}
		resetPollTimer();
	}

	public void CollectResources()
	{
		int num = 0;
		int i = 0;
		for (int num2 = _currentStoredResources.Length; i < num2; i++)
		{
			if (_currentStoredResources[i]._value > 0)
			{
				num = _currentStoredResources[i]._value;
				_currentStoredResources[i]._value = 0;
				CurrencyDefines._currencies[_currentStoredResources[i]._type].AddToCurrency(num);
			}
		}
		if (!BuildingManager.Instance._doAutomaticallyCollectResources)
		{
			_lastStoresUpdate = TimeManager.GetCurrentTime().Ticks;
			SaveState();
			resetPollTimer();
		}
		UpdateVisuals();
	}

	public void UpdateVisuals()
	{
		if (!(_relatedVisuals == null))
		{
			_relatedVisuals.refreshVisuals();
		}
	}

	public void UpdateStores()
	{
		if (_pollTimer > 0f)
		{
			_pollTimer -= RealTime.deltaTime;
			return;
		}
		resetPollTimer();
		if ((!BuildingManager.Instance._doAutomaticallyCollectResources && !_pHasStorage) || !_pHasProduction)
		{
			return;
		}
		if ((long)_lastStoresUpdate == 0L)
		{
			_lastStoresUpdate = TimeManager.GetCurrentTime().Ticks;
		}
		DateTime dateTime = new DateTime(_lastStoresUpdate);
		TimeSpan timeElapsed = TimeManager.GetCurrentTime() - dateTime;
		bool pHasUncollectedResources = _pHasUncollectedResources;
		BuildingData.BuildingLevel level = getLevel();
		bool flag = false;
		int i = 0;
		for (int num = level._resourceGeneration.Length; i < num; i++)
		{
			int maxStorage = getMaxStorage(level._resourceGeneration[i]._type);
			if (level._resourceGeneration[i]._value > 0 && (BuildingManager.Instance._doAutomaticallyCollectResources || maxStorage > 0))
			{
				BuildingData.ResourceValue resourceStorage = getResourceStorage(level._resourceGeneration[i]._type);
				flag = flag || updateStoresFor(ref resourceStorage._value, level._resourceGeneration[i]._value, maxStorage, timeElapsed);
			}
		}
		if (!pHasUncollectedResources & _pHasUncollectedResources)
		{
			UpdateVisuals();
		}
		if (flag)
		{
			_lastStoresUpdate = TimeManager.GetCurrentTime().Ticks;
		}
	}

	public int getMaxStorage(CurrencyDefines.CurrencyType type)
	{
		BuildingData.ResourceValue storageCapacity = getLevel().getStorageCapacity(type);
		return (!BuildingManager.Instance._useLocalCap && storageCapacity != null) ? storageCapacity._value : BuildingManager.Instance.getStorageCap(type)._value;
	}

	public BuildingData.BuildingLevel getLevel()
	{
		if (_buildingDefinition._levels.Length == 0)
		{
			return null;
		}
		return _buildingDefinition._levels[Mathf.Clamp(_buildingLevel, 0, _buildingDefinition._levels.Length - 1)];
	}

	public BuildingData.BuildingLevel getNextLevel()
	{
		if ((int)_buildingLevel < 0 || (int)_buildingLevel + 1 >= _buildingDefinition._levels.Length)
		{
			return null;
		}
		return _buildingDefinition._levels[(int)_buildingLevel + 1];
	}

	public void LevelUp()
	{
		++_buildingLevel;
		SaveState();
		UpdateVisuals();
	}

	public void ClearPlayerPrefs()
	{
		ClearPref("Level");
		ClearPref("LastUpdate");
		ClearPref("VisualData");
		int i = 0;
		for (int num = CurrencyDefines._allTypes.Length; i < num; i++)
		{
			ClearPref(CurrencyDefines._allTypes[i].ToString());
		}
	}

	public void Dispose()
	{
		ClearPlayerPrefs();
		if (_pIsUpgrading)
		{
			TimeManager.Instance.removeTimeEvent(getSaveString("Upgrade"));
		}
		if (!(_relatedVisuals == null))
		{
			_relatedVisuals.Dispose();
			_relatedVisuals = null;
		}
	}

	public string getNameOfBuildingFromRequirementsNotMet()
	{
		BuildingData.BuildingLevel nextLevel = getNextLevel();
		if (nextLevel == null || nextLevel._requirements == null || nextLevel._requirements.Length == 0)
		{
			return string.Empty;
		}
		int num = 0;
		for (int i = 0; i < nextLevel._requirements.Length; i++)
		{
			num = BuildingManager.Instance.getHighestLevelOfBuildingType(nextLevel._requirements[i]._buildingType);
			if (num < nextLevel._requirements[i]._buildingLevelNeeded)
			{
				return nextLevel._requirements[i]._buildingType.ToString();
			}
		}
		return string.Empty;
	}

	public string getSaveString(string infoName)
	{
		return string.Format(SAVE_TEMPLATE, _buildingDefinition._type.ToString(), _uniqueId, infoName);
	}

	private void changeCurrencyStorage(CurrencyDefines.CurrencyType type, int ammount)
	{
		if (ammount != 0)
		{
			BuildingData.ResourceValue resourceStorage = getResourceStorage(type);
			resourceStorage._value = Mathf.Max(0, resourceStorage._value + ammount);
		}
	}

	private BuildingData.ResourceValue getResourceStorage(CurrencyDefines.CurrencyType type)
	{
		int i = 0;
		for (int num = _currentStoredResources.Length; i < num; i++)
		{
			if (_currentStoredResources[i]._type == type)
			{
				return _currentStoredResources[i];
			}
		}
		BuildingData.ResourceValue[] currentStoredResources = _currentStoredResources;
		_currentStoredResources = new BuildingData.ResourceValue[currentStoredResources.Length + 1];
		int j = 0;
		for (int num2 = currentStoredResources.Length; j < num2; j++)
		{
			_currentStoredResources[j] = currentStoredResources[j];
		}
		_currentStoredResources[_currentStoredResources.Length - 1] = new BuildingData.ResourceValue();
		_currentStoredResources[_currentStoredResources.Length - 1]._type = type;
		_currentStoredResources[_currentStoredResources.Length - 1]._value = 0;
		return _currentStoredResources[_currentStoredResources.Length - 1];
	}

	private void changeVisibilityOnGroup(GameObject[] buildingObjects, bool doShow)
	{
		if (buildingObjects == null && buildingObjects.Length > 0)
		{
			return;
		}
		for (int i = 0; i < buildingObjects.Length; i++)
		{
			if (!(buildingObjects[i] == null))
			{
				buildingObjects[i].SetActive(doShow);
			}
		}
	}

	private bool updateStoresFor(ref int store, int ratePerMin, int maxStores, TimeSpan timeElapsed)
	{
		int num = Mathf.FloorToInt((float)timeElapsed.TotalSeconds * (float)ratePerMin * (1f / 60f));
		if (num < 1)
		{
			return false;
		}
		if (!BuildingManager.Instance._doAutomaticallyCollectResources && store >= maxStores)
		{
			if (store > maxStores)
			{
				store = Mathf.Clamp(store, 0, maxStores);
				return true;
			}
			return false;
		}
		store = Mathf.Clamp(store + num, 0, (!BuildingManager.Instance._doAutomaticallyCollectResources) ? maxStores : int.MaxValue);
		return true;
	}

	private void resetPollTimer()
	{
		_pollTimer = 1f + UnityEngine.Random.value * 0.2f;
	}

	private void ClearPref(string suffix)
	{
		if (ObscuredPrefs.HasKey(getSaveString(suffix)))
		{
			ObscuredPrefs.DeleteKey(getSaveString(suffix));
		}
	}

	private void SavePref(string suffix, int value)
	{
		if (value > 0)
		{
			ObscuredPrefs.SetInt(getSaveString(suffix), value);
		}
	}

	private void SavePref(string suffix, string value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			ObscuredPrefs.SetString(getSaveString(suffix), value);
		}
	}

	private int LoadPref(string suffix)
	{
		if (!ObscuredPrefs.HasKey(getSaveString(suffix)))
		{
			return 0;
		}
		return ObscuredPrefs.GetInt(getSaveString(suffix));
	}
}
