using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
	public enum EMINIGAME_TYPE
	{
		INVALID = 0,
		CATCH_THE_CROOKS = 1,
		CROOK_ROUNDUP = 2,
		BALLOON_CHASE = 3,
		FIRE_FRENZY = 4,
		FIRE_ON_THE_WATER = 5,
		WATER_DUMP = 6,
		ROLLIN_ROCKS = 7,
		ROCK_CRACKER = 8,
		EXPLORER_EVACUATION = 9,
		AIR_TRAVEL = 10,
		GRAB_THE_LUGGAGE = 11
	}

	public enum EMINIGAME_CATEGORY
	{
		INVALID = 0,
		POLICE = 1,
		FIRE = 2,
		VOLCANO = 3,
		AIRPORT = 4
	}

	public enum EVEHICLE_TYPE
	{
		INVALID = 0,
		LAND = 1,
		AIR = 2,
		WATER = 3
	}

	[Serializable]
	public class VehicleTemplate
	{
		public string templateLocalisationID;

		public VehiclePart.EUNIQUE_ID bodyPart;

		public VehiclePart.EUNIQUE_ID wheelPart;

		public VehiclePart.EUNIQUE_ID accessoryPart;

		public VehiclePart.EUNIQUE_ID attachmentPart;

		public Texture templateRender;
	}

	[Serializable]
	public class MinigameData
	{
		public CityManager.REGIONS region;

		public EMINIGAME_TYPE minigameType;

		public EMINIGAME_CATEGORY minigameCategory;

		public EVEHICLE_TYPE minigameVehicle;

		public string minigameName;

		public string minigameDescription;

		public string minigameSceneName;

		public string vehicleSelectHint;

		public string cutsceneFlowLocation;

		public Texture2D menuBackingTexture;

		public string minigameTutorialLocation = "MinigameTutorialPolicePursuit";

		public string gameMusic = "Downtown";

		public VehicleTemplate[] vehicleTemplates;

		private int _personalHighScore;

		private int _numTimesCompleted;

		private bool _seenTutorialMessage;

		public bool _pSeenTutorialMessage
		{
			get
			{
				return _seenTutorialMessage;
			}
			set
			{
				_seenTutorialMessage = value;
			}
		}

		public int _pPersonalHighScore
		{
			get
			{
				if (_personalHighScore == 0)
				{
					_personalHighScore = AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getInt(minigameName);
				}
				return _personalHighScore;
			}
			set
			{
				if (value > _pPersonalHighScore)
				{
					AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setInt(minigameName, value);
					_personalHighScore = value;
				}
			}
		}

		public int _pNumTimesCompleted
		{
			get
			{
				if (_numTimesCompleted == 0)
				{
					_numTimesCompleted = AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getInt(minigameName + "_Times");
				}
				return _numTimesCompleted;
			}
			set
			{
				AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setInt(minigameName + "_Times", value);
				_numTimesCompleted = value;
			}
		}

		public void ResetScoreSaveData()
		{
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setInt(minigameName, 0);
			_personalHighScore = 0;
		}
	}

	private static MinigameManager _instance;

	public MinigameData[] minigameData;

	private EMINIGAME_TYPE _currentMinigameType;

	private Dictionary<EMINIGAME_TYPE, MinigameData> _dataLookup = new Dictionary<EMINIGAME_TYPE, MinigameData>();

	private int _selectedVehicleTemplate;

	public static MinigameManager _pInstance
	{
		get
		{
			return _instance;
		}
	}

	public EMINIGAME_TYPE _pCurrentMinigameType
	{
		get
		{
			return _currentMinigameType;
		}
		set
		{
			_currentMinigameType = value;
		}
	}

	public bool _pSeenTutorial
	{
		get
		{
			return _dataLookup[_currentMinigameType]._pSeenTutorialMessage;
		}
		set
		{
			_dataLookup[_currentMinigameType]._pSeenTutorialMessage = value;
		}
	}

	public string _pTutorialFlowLocation
	{
		get
		{
			return _dataLookup[_currentMinigameType].minigameTutorialLocation;
		}
	}

	public EVEHICLE_TYPE _pCurrentVehicleTypeForMinigame
	{
		get
		{
			int num = minigameData.Length;
			for (int i = 0; i < num; i++)
			{
				if (minigameData[i].minigameType == _currentMinigameType)
				{
					return minigameData[i].minigameVehicle;
				}
			}
			return EVEHICLE_TYPE.INVALID;
		}
	}

	public int _pSelectedVehicleTemplate
	{
		get
		{
			return _selectedVehicleTemplate;
		}
		set
		{
			_selectedVehicleTemplate = value;
		}
	}

	private void Awake()
	{
		int num = minigameData.Length;
		for (int i = 0; i < num; i++)
		{
			_dataLookup[minigameData[i].minigameType] = minigameData[i];
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		_instance = this;
	}

	private void Start()
	{
		AddDebugMenuOptions();
	}

	public void OnMinigameResults()
	{
		MinigameData currentMinigameData = GetCurrentMinigameData();
		if (currentMinigameData._pNumTimesCompleted == 1)
		{
			if (_pSelectedVehicleTemplate == 1)
			{
				currentMinigameData._pNumTimesCompleted++;
			}
		}
		else
		{
			currentMinigameData._pNumTimesCompleted++;
		}
	}

	public void ResetAll()
	{
		int num = minigameData.Length;
		for (int i = 0; i < num; i++)
		{
			_dataLookup[minigameData[i].minigameType]._pSeenTutorialMessage = false;
			_dataLookup[minigameData[i].minigameType]._pPersonalHighScore = 0;
			_dataLookup[minigameData[i].minigameType]._pNumTimesCompleted = 0;
			_dataLookup[minigameData[i].minigameType].ResetScoreSaveData();
		}
	}

	public VehicleTemplate GetCurrentVehicleTemplate()
	{
		if (GlobalInGameData._pWantFullCarousel)
		{
			VehicleTemplate vehicleTemplate = new VehicleTemplate();
			vehicleTemplate.bodyPart = VehiclePart.EUNIQUE_ID.BODY_POLICE_CAR;
			vehicleTemplate.accessoryPart = VehiclePart.EUNIQUE_ID.ACCESSORY_POLICE_SIREN;
			vehicleTemplate.wheelPart = VehiclePart.EUNIQUE_ID.WHEEL_SPEED;
			return vehicleTemplate;
		}
		MinigameData currentMinigameData = GetCurrentMinigameData();
		if (currentMinigameData == null)
		{
			return null;
		}
		return currentMinigameData.vehicleTemplates[_selectedVehicleTemplate];
	}

	public MinigameData GetCurrentMinigameData()
	{
		if (_currentMinigameType == EMINIGAME_TYPE.INVALID)
		{
			return null;
		}
		return _dataLookup[_currentMinigameType];
	}

	public MinigameData GetMinigameDataFromType(EMINIGAME_TYPE minigameType)
	{
		if (minigameType == EMINIGAME_TYPE.INVALID)
		{
			return null;
		}
		return _dataLookup[minigameType];
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
			stringBuilder.Append("-");
			return stringBuilder.ToString();
		};
		AmuzoDebugMenu amuzoDebugMenu = new AmuzoDebugMenu("MINIGAME CHEATS");
		amuzoDebugMenu.AddInfoTextFunction(textAreaFunction);
		amuzoDebugMenu.AddButton(new AmuzoDebugMenuButton("UNLOCK ALL TEMPLATES", delegate
		{
			int num = minigameData.Length;
			for (int i = 0; i < num; i++)
			{
				minigameData[i]._pNumTimesCompleted = Mathf.Max(minigameData[i]._pNumTimesCompleted, 3);
			}
		}));
		AmuzoDebugMenuManager.RegisterRootDebugMenu(amuzoDebugMenu);
	}
}
