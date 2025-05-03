using System;
using CodeStage.AntiCheat.ObscuredTypes;
using GameDefines;
using UnityEngine;

[Serializable]
public class TroopData
{
	public enum eTroopDisplayStatus
	{
		STANDARD = 0,
		TYPE_1 = 1,
		TYPE_2 = 2,
		INVISIBLE = 3
	}

	public enum eItems
	{
		NONE = 0,
		ITEM_1 = 1,
		ITEM_2 = 2,
		ITEM_3 = 3,
		ITEM_4 = 4,
		ITEM_5 = 5
	}

	public enum eTroopMoveBehaviour
	{
		BEHAVIOUR_1 = 0,
		BEHAVIOUR_2 = 1,
		BEHAVIOUR_3 = 2
	}

	public enum eTroopAttackBehaviour
	{
		BEHAVIOUR_1 = 0,
		BEHAVIOUR_2 = 1,
		BEHAVIOUR_3 = 2
	}

	[Serializable]
	public class TrainingCost
	{
		public int _trainingSeconds;

		public BuildingData.ResourceValue[] _trainingCost;
	}

	[Serializable]
	public class CombatStats
	{
		public int _hitPoints = 1;

		public int _attack = 1;

		public int _defense = 1;

		public int _speed = 1;

		public float _attackAoE;

		public float _range;

		public eTroopMoveBehaviour defaultMoveBehaviour;

		public eTroopAttackBehaviour defaultAttackBehaviour;

		public eItems[] attachedItems;

		public string deathSoundGroup = "BanditDie";

		public string takeHitSoundGroup = string.Empty;

		public string onMeleeSoundGroup = string.Empty;
	}

	[Serializable]
	public class BuildingRequirement
	{
		[HideInInspector]
		public string _debugName = string.Empty;

		public BuildingDefines.EBuildingType _buildingType;

		public int _buildingLevelNeeded;
	}

	private const string NUM_STOCK_SAVE_KEY = "{0}_numStock";

	private const string LAST_TRAINING_SLOT_SAVE_KEY = "{0}_lastTrainingSlot";

	public string _name;

	public string _prefabName;

	public string _largeIconName;

	public string _smallIconName;

	public TrainingCost _troopCost;

	public CombatStats _combatStats;

	public BuildingRequirement[] _buildingRequirements;

	public eTroopDisplayStatus _troopDisplayStatus;

	[HideInInspector]
	public ObscuredInt _lastTrainingIndex = 0;

	private ObscuredInt _numStock;

	public int _numStockForDebug;

	private ObscuredInt _numSuspended;

	public int _numSuspendedForDebug;

	public int _pNumStock
	{
		get
		{
			return (int)_numStock - (int)_numSuspended;
		}
	}

	public bool _pIsTraining
	{
		get
		{
			return TimeManager.Instance.HasTimeEventEndingWith(_name);
		}
	}

	public string _pTrainingTimeEvent
	{
		get
		{
			return string.Format("Slot_{0}_{1}", _lastTrainingIndex.ToString(), _name);
		}
	}

	public bool _pCanPlayerAffordTraining
	{
		get
		{
			int i = 0;
			for (int num = _troopCost._trainingCost.Length; i < num; i++)
			{
				if (_troopCost._trainingCost[i]._value > CurrencyDefines._currencies[_troopCost._trainingCost[i]._type]._displayValue)
				{
					return false;
				}
			}
			return true;
		}
	}

	public bool _pAreTrainingBuildingRequirementsSatisfied
	{
		get
		{
			if (_buildingRequirements == null || _buildingRequirements.Length == 0)
			{
				return true;
			}
			int num = 0;
			for (int i = 0; i < _buildingRequirements.Length; i++)
			{
				num = BuildingManager.Instance.getHighestLevelOfBuildingType(_buildingRequirements[i]._buildingType);
				if (num < _buildingRequirements[i]._buildingLevelNeeded)
				{
					return false;
				}
			}
			return true;
		}
	}

	public bool _pIsUnderTypeBasedUnitCap
	{
		get
		{
			eTroopDisplayStatus troopDisplayStatus = _troopDisplayStatus;
			int numTroopsByType = TroopManager.Instance.getNumTroopsByType(troopDisplayStatus);
			return numTroopsByType < TroopManager.Instance.getTroopCapacityByType(troopDisplayStatus);
		}
	}

	public bool _pCanPlayerTrainThisTroop
	{
		get
		{
			return _pAreTrainingBuildingRequirementsSatisfied && _pCanPlayerAffordTraining && _pIsUnderTypeBasedUnitCap && TroopManager.Instance._pCanTrainMore;
		}
	}

	public string _pLocalisedName
	{
		get
		{
			if (LocalisationFacade.Instance == null)
			{
				return _name;
			}
			return LocalisationFacade.Instance.GetString("TroopNames." + _name);
		}
	}

	public string getNameOfBuildingFromRequirementsNotMet()
	{
		if (_buildingRequirements == null || _buildingRequirements.Length == 0)
		{
			return string.Empty;
		}
		int num = 0;
		for (int i = 0; i < _buildingRequirements.Length; i++)
		{
			num = BuildingManager.Instance.getHighestLevelOfBuildingType(_buildingRequirements[i]._buildingType);
			if (num < _buildingRequirements[i]._buildingLevelNeeded)
			{
				return _buildingRequirements[i]._buildingType.ToString();
			}
		}
		return string.Empty;
	}

	public void SaveState()
	{
		if (!TroopManager.IS_TUTORIAL_MODE && !TroopManager.IS_STARTING_TUTORIAL_MODE)
		{
			ClearPlayerPrefs();
			SavePref("{0}_numStock", _numStock);
			SavePref("{0}_lastTrainingSlot", _lastTrainingIndex);
		}
	}

	public void LoadState()
	{
		_numStock = LoadPref("{0}_numStock");
		_numStockForDebug = _numStock;
		_lastTrainingIndex = LoadPref("{0}_lastTrainingSlot");
	}

	public void AddTroop(int num = 1)
	{
		if ((int)_numSuspended > 0)
		{
			--_numSuspended;
			_numSuspendedForDebug = _numSuspended;
		}
		else
		{
			_numStock = (int)_numStock + num;
			_numStockForDebug = _numStock;
			SaveState();
		}
	}

	public void SuspendTroop(int num = 1)
	{
		_numSuspended = (int)_numSuspended + num;
		_numSuspendedForDebug = _numSuspended;
	}

	public void DeductTroop(int num = 1)
	{
		if ((int)_numStock != 0 || (int)_numSuspended != 0)
		{
			_numSuspended = Mathf.Max(0, (int)_numSuspended - num);
			_numSuspendedForDebug = _numSuspended;
			_numStock = Mathf.Max(0, (int)_numStock - num);
			_numStockForDebug = _numStock;
			SaveState();
		}
	}

	public void UnsuspendAllTroops()
	{
		_numSuspended = 0;
		_numSuspendedForDebug = _numSuspended;
	}

	public void ClearPlayerPrefs()
	{
		ClearPref("{0}_numStock");
		ClearPref("{0}_lastTrainingSlot");
	}

	public float getTrainingProgress()
	{
		if (TimeManager.Instance == null || !_pIsTraining)
		{
			return -1f;
		}
		return TimeManager.Instance.GetDecimalProgress(_pTrainingTimeEvent);
	}

	public void stopTraining()
	{
		if (!(TimeManager.Instance == null) && _pIsTraining)
		{
			TimeManager.Instance.removeTimeEvent(_pTrainingTimeEvent);
		}
	}

	private void ClearPref(string suffix)
	{
		if (ObscuredPrefs.HasKey(_name + suffix))
		{
			ObscuredPrefs.DeleteKey(_name + suffix);
		}
	}

	private void SavePref(string suffix, int value)
	{
		if (value >= 0 || ObscuredPrefs.GetInt(string.Format(suffix, _name)) != 0)
		{
			ObscuredPrefs.SetInt(string.Format(suffix, _name), value);
		}
	}

	private int LoadPref(string suffix)
	{
		if (!ObscuredPrefs.HasKey(string.Format(suffix, _name)))
		{
			return 0;
		}
		return ObscuredPrefs.GetInt(string.Format(suffix, _name));
	}
}
