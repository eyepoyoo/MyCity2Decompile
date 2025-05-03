using System;
using System.Collections.Generic;
using GameDefines;
using UnityEngine;

[Serializable]
public class BuildingData
{
	[Serializable]
	public class BuildingLevel
	{
		[HideInInspector]
		public int _upgradeSeconds;

		public ResourceValue[] _upgradeCost;

		public TroopData.BuildingRequirement[] _requirements;

		public ResourceValue[] _resourceGeneration;

		public ResourceValue[] _resourceStorage;

		public ResourceValue getFirstStorageCapacity()
		{
			int i = 0;
			for (int num = _resourceStorage.Length; i < num; i++)
			{
				if (_resourceStorage[i]._value > 0)
				{
					return _resourceStorage[i];
				}
			}
			return null;
		}

		public ResourceValue getStorageCapacity(CurrencyDefines.CurrencyType type)
		{
			int i = 0;
			for (int num = _resourceStorage.Length; i < num; i++)
			{
				if (_resourceStorage[i]._type == type)
				{
					return _resourceStorage[i];
				}
			}
			return null;
		}

		public ResourceValue getFirstGenerationRate()
		{
			int i = 0;
			for (int num = _resourceGeneration.Length; i < num; i++)
			{
				if (_resourceGeneration[i]._value > 0)
				{
					return _resourceGeneration[i];
				}
			}
			return null;
		}

		public ResourceValue getGenerationRate(CurrencyDefines.CurrencyType type)
		{
			int i = 0;
			for (int num = _resourceGeneration.Length; i < num; i++)
			{
				if (_resourceGeneration[i]._type == type)
				{
					return _resourceGeneration[i];
				}
			}
			return null;
		}
	}

	[Serializable]
	public class ResourceValue
	{
		[HideInInspector]
		public CurrencyDefines.CurrencyType _type;

		public int _value;
	}

	private const char ENTRY_DELIMITER = ',';

	[HideInInspector]
	public string _debugName = string.Empty;

	public BuildingDefines.EBuildingType _type;

	public BuildingLevel[] _levels;

	public bool _doesContributeToGlobalStorage;

	public float _resourceCollectionThreshold = 0.2f;

	[HideInInspector]
	public List<BuildingInstance> _buildingInstances = new List<BuildingInstance>();

	public string _pLocalisedName
	{
		get
		{
			if (LocalisationFacade.Instance == null)
			{
				return _type.ToString();
			}
			return LocalisationFacade.Instance.GetString("BuildingNames." + _type);
		}
	}

	public bool _pHasInstances
	{
		get
		{
			return _buildingInstances != null && _buildingInstances.Count > 0;
		}
	}

	public string SaveState()
	{
		if (!_pHasInstances)
		{
			return string.Empty;
		}
		string text = _type.ToString();
		for (int i = 0; i < _buildingInstances.Count; i++)
		{
			if (_buildingInstances[i] != null)
			{
				_buildingInstances[i].SaveState();
				text = text + _buildingInstances[i]._uniqueId + ((i != _buildingInstances.Count - 1) ? (string.Empty + ',') : string.Empty);
			}
		}
		return text;
	}

	public void Load(string saveString, Action<BuildingInstance> _onLoadCallback)
	{
		if (!saveString.StartsWith(_type.ToString()))
		{
			return;
		}
		string text = saveString.Replace(_type.ToString(), string.Empty);
		string[] array = text.Split(',');
		for (int i = 0; i < array.Length; i++)
		{
			BuildingInstance buildingInstance = new BuildingInstance();
			buildingInstance._buildingDefinition = this;
			int.TryParse(array[i], out buildingInstance._uniqueId);
			_buildingInstances.Add(buildingInstance);
			_buildingInstances[i].LoadState();
			if (_onLoadCallback != null)
			{
				_onLoadCallback(buildingInstance);
			}
		}
	}

	public void Update()
	{
		for (int i = 0; i < _buildingInstances.Count; i++)
		{
			if (_buildingInstances[i] != null)
			{
				_buildingInstances[i].UpdateStores();
				if (BuildingManager.Instance._doAutomaticallyCollectResources)
				{
					_buildingInstances[i].CollectResources();
				}
			}
		}
	}

	public BuildingLevel getLevel(int level)
	{
		if (level < 0 || level >= _levels.Length)
		{
			return null;
		}
		return _levels[Mathf.Clamp(level, 0, _levels.Length - 1)];
	}

	public int getMaxStorage(CurrencyDefines.CurrencyType type, BuildingLevel buildingLevel)
	{
		if (buildingLevel == null)
		{
			return 0;
		}
		ResourceValue storageCapacity = buildingLevel.getStorageCapacity(type);
		return (!BuildingManager.Instance._useLocalCap && storageCapacity != null) ? storageCapacity._value : BuildingManager.Instance.getStorageCap(type)._value;
	}

	public bool hasStorage(BuildingLevel buildingLevel)
	{
		if (buildingLevel == null || buildingLevel._resourceStorage.Length <= 0)
		{
			return false;
		}
		int i = 0;
		for (int num = buildingLevel._resourceStorage.Length; i < num; i++)
		{
			if (buildingLevel._resourceStorage[i]._value > 0)
			{
				return true;
			}
		}
		return false;
	}

	public bool hasProduction(BuildingLevel buildingLevel)
	{
		if (buildingLevel == null || buildingLevel._resourceGeneration.Length <= 0)
		{
			return false;
		}
		int i = 0;
		for (int num = buildingLevel._resourceGeneration.Length; i < num; i++)
		{
			if (buildingLevel._resourceGeneration[i]._value > 0)
			{
				return true;
			}
		}
		return false;
	}

	public void applyBalanceModifiers()
	{
		int i = 0;
		for (int num = _levels.Length; i < num; i++)
		{
			int j = 0;
			for (int num2 = _levels[i]._upgradeCost.Length; j < num2; j++)
			{
				_levels[i]._upgradeCost[j]._value = Mathf.FloorToInt((float)_levels[i]._upgradeCost[j]._value * BuildingManager.Instance.GLOBAL_COST_BALANCE_MODIFIER);
			}
			_levels[i]._upgradeSeconds = Mathf.FloorToInt((float)_levels[i]._upgradeSeconds * BuildingManager.Instance.GLOBAL_TIME_BALANCE_MODIFIER);
		}
	}

	public int getMaxStorage(CurrencyDefines.CurrencyType type, int buildingLevel)
	{
		return getMaxStorage(type, getLevel(buildingLevel));
	}

	public bool hasStorage(int buildingLevel)
	{
		return hasStorage(getLevel(buildingLevel));
	}

	public bool hasProduction(int buildingLevel)
	{
		return hasProduction(getLevel(buildingLevel));
	}
}
