using System;
using GameDefines;
using UnityEngine;

public class BuildingTestScreen : ScreenBase
{
	[Serializable]
	public class CurrencyLabel
	{
		public CurrencyDefines.CurrencyType _currency;

		public UILabel _titleLabel;

		public UILabel _valueLabel;
	}

	[Serializable]
	public class SpawnButton
	{
		public BuildingDefines.EBuildingType _type;

		public UIWidget _button;

		public UILabel _buttonLabel;
	}

	private static BuildingTestScreen _instance;

	public UIButton _selectButton;

	public UIButton _spawnButton;

	public CurrencyLabel[] _currencyLabels;

	public UIWidget _sidebarInfo;

	public UILabel _sidebarInfoTitle;

	public UILabel _sidebarInfoType;

	public UILabel _sidebarInfoUniqueId;

	public UILabel _sidebarInfoRelatedCurrency;

	public UILabel _sidebarInfoStorageValue;

	public UILabel _sidebarInfoProductionValue;

	public UILabel _sidebarInfoInternalStorageValue;

	public UIButton _collectButton;

	public UIButton _upgradeButton;

	public UIButton _sellButton;

	public CurrencyLabel[] _sidebarInfoUpgradeCosts;

	public UILabel _sidebarInfoUpgradeSeconds;

	public UIWidget _sidebarSpawn;

	public SpawnButton[] _sidebarSpawnButtons;

	public BuildingMode _currentMode;

	public BuildingInstance _instanceforInfo;

	private bool _hasSelectedSpawn;

	private BuildingDefines.EBuildingType _selectedSpawn;

	public static BuildingTestScreen Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		_instance = this;
	}

	public void gridLocationWasClickedOn(Point gridPoint)
	{
		if (UICamera.hoveredObject != null)
		{
			return;
		}
		switch (_currentMode)
		{
		case BuildingMode.SELECT:
			selectBuildingAt(gridPoint);
			break;
		case BuildingMode.SPAWN:
			if (_hasSelectedSpawn)
			{
				spawnBuildingAt(gridPoint);
			}
			break;
		}
	}

	public void OnBackPressed()
	{
		Facades<FlowFacade>.Instance.GoBack();
	}

	public void OnSelectPressed()
	{
		if (_currentMode != BuildingMode.SELECT)
		{
			_currentMode = BuildingMode.SELECT;
			_instanceforInfo = null;
			showSidebarInfo();
			setButtonStates();
		}
	}

	public void OnSpawnPressed()
	{
		if (_currentMode != BuildingMode.SPAWN)
		{
			_currentMode = BuildingMode.SPAWN;
			showSidebarSpawn();
			setButtonStates();
		}
	}

	public void CollectPressed()
	{
		if (_currentMode != BuildingMode.SPAWN && _instanceforInfo != null)
		{
			_instanceforInfo.CollectResources();
			RefreshInfo();
		}
	}

	public void UpgradePressed()
	{
		if (_currentMode != BuildingMode.SPAWN && _instanceforInfo != null && _instanceforInfo.getNextLevel() != null)
		{
			BuildingManager.Instance.StartUpgrade(_instanceforInfo);
			RefreshInfo();
		}
	}

	public void SellPressed()
	{
		if (_currentMode != BuildingMode.SPAWN && _instanceforInfo != null)
		{
			BuildingManager.Instance.RemoveBuilding(_instanceforInfo);
			_instanceforInfo = null;
			RefreshInfo();
		}
	}

	public void spawnButtonPressed(UIWidget button)
	{
		if (_currentMode != BuildingMode.SPAWN)
		{
			return;
		}
		for (int i = 0; i < _sidebarSpawnButtons.Length; i++)
		{
			if (!(_sidebarSpawnButtons[i]._button != button))
			{
				_hasSelectedSpawn = true;
				_selectedSpawn = _sidebarSpawnButtons[i]._type;
			}
		}
	}

	public void RefreshInfo()
	{
		if (_currentMode == BuildingMode.SELECT)
		{
			showSidebarInfo();
			if (_instanceforInfo != null)
			{
				showInfoFor(_instanceforInfo);
			}
		}
	}

	protected override void OnShowScreen()
	{
		setButtonStates();
		showSidebarInfo();
	}

	public override void PollUpdate()
	{
		updateCurrencyLabels();
		RefreshInfo();
	}

	private void spawnBuildingAt(Point gridPos)
	{
		if (!(BuildingGridPlacementManager.Instance == null) && !BuildingGridPlacementManager.Instance.isActiveBuildingAt(gridPos))
		{
			BuildingGridPlacementManager.Instance.spawnBuildingAt(gridPos, _selectedSpawn);
		}
	}

	private void selectBuildingAt(Point gridPos)
	{
		if (!(BuildingGridPlacementManager.Instance == null) && BuildingGridPlacementManager.Instance.isActiveBuildingAt(gridPos))
		{
			_instanceforInfo = BuildingGridPlacementManager.Instance.getBuildingAt(gridPos);
			RefreshInfo();
		}
	}

	private void showInfoFor(BuildingInstance building)
	{
		if (building == null || building._buildingDefinition == null)
		{
			return;
		}
		_instanceforInfo = building;
		_sidebarInfoType.text = building._buildingDefinition._type.ToString();
		_sidebarInfoUniqueId.text = building._uniqueId.ToString();
		_sidebarInfoTitle.text = "Level " + building._buildingLevel;
		BuildingData.BuildingLevel level = building.getLevel();
		BuildingData.BuildingLevel nextLevel = building.getNextLevel();
		if (building._pHasProduction)
		{
			BuildingData.ResourceValue firstGenerationRate = level.getFirstGenerationRate();
			_sidebarInfoRelatedCurrency.text = firstGenerationRate._type.ToString();
			_sidebarInfoProductionValue.text = firstGenerationRate._value + "/min";
		}
		if (building._pHasStorage)
		{
			BuildingData.ResourceValue firstStorageCapacity = level.getFirstStorageCapacity();
			_sidebarInfoRelatedCurrency.text = firstStorageCapacity._type.ToString();
			_sidebarInfoStorageValue.text = firstStorageCapacity._value.ToString();
		}
		if (building._currentStoredResources != null && building._currentStoredResources.Length > 0)
		{
			_sidebarInfoInternalStorageValue.text = string.Empty;
			for (int i = 0; i < building._currentStoredResources.Length; i++)
			{
				if (building._currentStoredResources[i]._value > 0)
				{
					_sidebarInfoInternalStorageValue.text = building._currentStoredResources[i]._value.ToString();
					break;
				}
			}
		}
		if (_instanceforInfo._pCanCollectedResources)
		{
			_collectButton.gameObject.SetActive(true);
			_collectButton.isEnabled = true;
		}
		else if (!BuildingManager.Instance._doAutomaticallyCollectResources && _instanceforInfo._pHasProduction)
		{
			_collectButton.gameObject.SetActive(true);
			_collectButton.isEnabled = false;
		}
		else
		{
			_collectButton.gameObject.SetActive(false);
		}
		if (_instanceforInfo._pIsMaxLevel || _instanceforInfo._pIsUpgrading)
		{
			_upgradeButton.gameObject.SetActive(false);
		}
		else
		{
			_upgradeButton.gameObject.SetActive(true);
			_upgradeButton.isEnabled = _instanceforInfo._pCanPlayerAffordUpgrade && _instanceforInfo._pHaveBuildingRequirementsForNextLevelBeenMet;
		}
		_sellButton.gameObject.SetActive(true);
		if (_instanceforInfo._pIsUpgrading && _sidebarInfoUpgradeCosts != null && _sidebarInfoUpgradeCosts.Length > 0 && _sidebarInfoUpgradeCosts[0] != null)
		{
			_sidebarInfoUpgradeCosts[0]._titleLabel.text = "Progess:";
			_sidebarInfoUpgradeCosts[0]._valueLabel.text = Mathf.FloorToInt(_instanceforInfo._pUpgradeProgess * 100f) + "%";
		}
		if (_sidebarInfoUpgradeCosts == null || _sidebarInfoUpgradeCosts.Length == 0 || nextLevel == null || _instanceforInfo._pIsUpgrading)
		{
			return;
		}
		for (int j = 0; j < _sidebarInfoUpgradeCosts.Length; j++)
		{
			if (_sidebarInfoUpgradeCosts[j] != null && nextLevel._upgradeCost.Length > j)
			{
				_sidebarInfoUpgradeCosts[j]._titleLabel.text = nextLevel._upgradeCost[j]._type.ToString();
				_sidebarInfoUpgradeCosts[j]._valueLabel.text = nextLevel._upgradeCost[j]._value.ToString();
			}
		}
		_sidebarInfoUpgradeSeconds.text = nextLevel._upgradeSeconds.ToString();
	}

	private void showSidebarSpawn()
	{
		_sidebarInfo.gameObject.SetActive(false);
		_sidebarSpawn.gameObject.SetActive(true);
		_hasSelectedSpawn = false;
		_instanceforInfo = null;
		for (int i = 0; i < _sidebarSpawnButtons.Length; i++)
		{
			if (!(_sidebarSpawnButtons[i]._button == null))
			{
				if (i >= BuildingDefines._allTypes.Length)
				{
					_sidebarSpawnButtons[i]._button.gameObject.SetActive(false);
					continue;
				}
				_sidebarSpawnButtons[i]._button.gameObject.SetActive(true);
				_sidebarSpawnButtons[i]._buttonLabel.text = BuildingDefines._allTypes[i].ToString();
				_sidebarSpawnButtons[i]._type = BuildingDefines._allTypes[i];
			}
		}
	}

	private void showSidebarInfo()
	{
		_hasSelectedSpawn = false;
		_sidebarInfo.gameObject.SetActive(true);
		_sidebarSpawn.gameObject.SetActive(false);
		_sidebarInfoType.text = string.Empty;
		_sidebarInfoUniqueId.text = string.Empty;
		_sidebarInfoRelatedCurrency.text = string.Empty;
		_sidebarInfoProductionValue.text = string.Empty;
		_sidebarInfoStorageValue.text = string.Empty;
		_sidebarInfoInternalStorageValue.text = string.Empty;
		_sidebarInfoUpgradeSeconds.text = string.Empty;
		_upgradeButton.gameObject.SetActive(false);
		_sellButton.gameObject.SetActive(false);
		_collectButton.gameObject.SetActive(false);
		if (_sidebarInfoUpgradeCosts == null || _sidebarInfoUpgradeCosts.Length == 0)
		{
			return;
		}
		for (int i = 0; i < _sidebarInfoUpgradeCosts.Length; i++)
		{
			if (_sidebarInfoUpgradeCosts[i] != null)
			{
				_sidebarInfoUpgradeCosts[i]._titleLabel.text = string.Empty;
				_sidebarInfoUpgradeCosts[i]._valueLabel.text = string.Empty;
			}
		}
	}

	private void setButtonStates()
	{
		_selectButton.isEnabled = _currentMode != BuildingMode.SELECT;
		_spawnButton.isEnabled = _currentMode != BuildingMode.SPAWN;
	}

	private void updateCurrencyLabels()
	{
		if (_currencyLabels == null || _currencyLabels.Length == 0)
		{
			return;
		}
		int i = 0;
		for (int num = _currencyLabels.Length; i < num; i++)
		{
			if (!(_currencyLabels[i]._valueLabel == null))
			{
				Currency currency = CurrencyDefines._currencies[_currencyLabels[i]._currency];
				_currencyLabels[i]._titleLabel.text = currency._pLocalisedName;
				_currencyLabels[i]._valueLabel.text = currency._displayValue + ((currency._displayMax <= 0) ? string.Empty : (" / " + currency._displayMax));
			}
		}
	}
}
