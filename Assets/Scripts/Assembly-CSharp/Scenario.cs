using System;
using UnityEngine;

[Serializable]
public class Scenario
{
	public enum EBRICK_TYPE
	{
		RED = 0,
		BLUE = 1,
		YELLOW = 2,
		GREEN = 3
	}

	public int totalBricksRequired;

	public string scenarioName;

	public string scenarioDescription;

	public string scenarioDescriptionComplete;

	public CityManager.ECITYBUILDINGS building;

	public bool isAvailable = true;

	public Texture2D scenarioMinifigTexture;

	public Texture2D scenarioTexture;

	public bool scenarioTextureWide;

	public EBRICK_TYPE brickType;

	public Material brickMaterial;

	public Material brickMaterialHighRes;

	public Material guiStudMaterial;

	public Material inGameStudMaterial;

	private bool _isLocalPlayerData = true;

	private int _currentBricks;

	private float _lastSeenProgression;

	public bool _pIsLocalPlayerData
	{
		get
		{
			return _isLocalPlayerData;
		}
		set
		{
			_isLocalPlayerData = value;
		}
	}

	public int _pCurrentBricks
	{
		get
		{
			if (_currentBricks == 0 && _isLocalPlayerData)
			{
				_currentBricks = AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getInt(scenarioName);
			}
			return _currentBricks;
		}
		private set
		{
			_currentBricks = value;
			if (_currentBricks > totalBricksRequired)
			{
				_currentBricks = totalBricksRequired;
			}
			if (_isLocalPlayerData)
			{
				AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setInt(scenarioName, _currentBricks);
			}
		}
	}

	public float _pProgress
	{
		get
		{
			if (totalBricksRequired == 0)
			{
				Debug.LogError("0 Bricks required for scenario " + scenarioName + " - are you sure this is right?");
				return 0f;
			}
			return (float)_pCurrentBricks / (float)totalBricksRequired;
		}
	}

	public float _pLastSeenProgression
	{
		get
		{
			if (_lastSeenProgression == 0f && _isLocalPlayerData)
			{
				_lastSeenProgression = AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getFloat(scenarioName + "_lastProgress");
			}
			return _lastSeenProgression;
		}
		set
		{
			_lastSeenProgression = value;
			if (_isLocalPlayerData)
			{
				AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setFloat(scenarioName + "_lastProgress", _lastSeenProgression);
			}
		}
	}

	public bool _pIsComplete
	{
		get
		{
			return _currentBricks >= totalBricksRequired;
		}
	}

	public void ResetScenario()
	{
		Debug.Log("Resetting scenario with building:" + building);
		_pCurrentBricks = 0;
		_lastSeenProgression = 0f;
		if (_isLocalPlayerData)
		{
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setFloat(scenarioName + "_lastProgress", _lastSeenProgression);
		}
		if (CityManager._pInstance != null)
		{
			CityManager._pInstance.ResetBuilding(building);
		}
	}

	public void AddBricks(int numBricks)
	{
		_pCurrentBricks = _currentBricks + numBricks;
	}
}
