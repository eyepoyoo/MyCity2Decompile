using System;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
	private const string SLOTTED_SCENARIOS_SAVE_KEY = "SlottedScenarios";

	private const string SELECTED_SCENARIO_SAVE_KEY = "SelectedScenarios";

	private static ScenarioManager _instance;

	public Scenario[] allScenarios;

	private Scenario _currentScenario;

	private Scenario[] _slottedScenarios = new Scenario[3];

	private Action<Scenario[]> _onOtherPlayerDataRecieved;

	private string _requestedPlayerDataID;

	private bool _isFreshSave;

	public static ScenarioManager _pInstance
	{
		get
		{
			return _instance;
		}
	}

	public Scenario _pCurrentScenario
	{
		get
		{
			return _currentScenario;
		}
	}

	public bool _pIsFreshSave
	{
		get
		{
			return _isFreshSave;
		}
		set
		{
			_isFreshSave = value;
		}
	}

	private void Awake()
	{
		_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.hasKey("SlottedScenarios"))
		{
			string[] stringArray = AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getStringArray("SlottedScenarios");
			if (stringArray.Length == _slottedScenarios.Length)
			{
				int i = 0;
				for (int num = _slottedScenarios.Length; i < num; i++)
				{
					_slottedScenarios[i] = GetScenario(stringArray[i]);
				}
			}
		}
		else
		{
			FindAvailableScenarios();
		}
		if (AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.hasKey("SelectedScenarios"))
		{
			_currentScenario = _slottedScenarios[AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getInt("SelectedScenarios")];
		}
		else
		{
			_currentScenario = _slottedScenarios[0];
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setInt("SelectedScenarios", 0);
			_pIsFreshSave = true;
		}
		addDebugMenuOptions();
	}

	private void OnDestroy()
	{
		_instance = null;
	}

	private void Update()
	{
		if (_onOtherPlayerDataRecieved != null)
		{
			HandleOtherPlayerScenarioDataLoad();
		}
	}

	public void ResetAll()
	{
		for (int i = 0; i < allScenarios.Length; i++)
		{
			allScenarios[i].ResetScenario();
		}
		for (int j = 0; j < _slottedScenarios.Length; j++)
		{
			_slottedScenarios[j] = null;
		}
		FindAvailableScenarios();
		_currentScenario = _slottedScenarios[0];
		_pIsFreshSave = true;
		AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setInt("SelectedScenarios", 0);
	}

	public void GetOtherPlayerScenarioData(string playerID, Action<Scenario[]> OnDataRecieved)
	{
		_requestedPlayerDataID = playerID;
		_onOtherPlayerDataRecieved = OnDataRecieved;
	}

	public void HandleOtherPlayerScenarioDataLoad()
	{
		if (_onOtherPlayerDataRecieved == null)
		{
			return;
		}
		Scenario[] array = new Scenario[allScenarios.Length];
		Dictionary<CityManager.ECITYBUILDINGS, int> dictionary = ProcessOtherPlayerProgressData();
		if (dictionary != null)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Scenario();
				array[i].brickMaterial = allScenarios[i].brickMaterial;
				array[i].building = allScenarios[i].building;
				array[i].guiStudMaterial = allScenarios[i].guiStudMaterial;
				array[i].inGameStudMaterial = allScenarios[i].inGameStudMaterial;
				array[i].isAvailable = allScenarios[i].isAvailable;
				array[i].scenarioDescription = allScenarios[i].scenarioDescription;
				array[i].scenarioDescriptionComplete = allScenarios[i].scenarioDescriptionComplete;
				array[i].scenarioName = allScenarios[i].scenarioName;
				array[i].totalBricksRequired = allScenarios[i].totalBricksRequired;
				array[i]._pIsLocalPlayerData = false;
				array[i].AddBricks(dictionary[array[i].building]);
			}
			_onOtherPlayerDataRecieved(array);
			_onOtherPlayerDataRecieved = null;
		}
	}

	public Dictionary<CityManager.ECITYBUILDINGS, int> ProcessOtherPlayerProgressData()
	{
		Dictionary<CityManager.ECITYBUILDINGS, int> dictionary = null;
		if (_requestedPlayerDataID == "Hope42")
		{
			dictionary = new Dictionary<CityManager.ECITYBUILDINGS, int>();
			dictionary[CityManager.ECITYBUILDINGS.FIRE_STATION] = 800;
			dictionary[CityManager.ECITYBUILDINGS.LIGHTHOUSE] = 800;
			dictionary[CityManager.ECITYBUILDINGS.FIRE_CONTAINERS] = 250;
			dictionary[CityManager.ECITYBUILDINGS.POLICE_ISLAND_HQ] = 0;
			dictionary[CityManager.ECITYBUILDINGS.POLICE_BOATS] = 0;
			dictionary[CityManager.ECITYBUILDINGS.JETSKI_PATROL] = 0;
			dictionary[CityManager.ECITYBUILDINGS.HELICOPTER_REQUESTED] = 0;
			dictionary[CityManager.ECITYBUILDINGS.LAVA_LIFTERS_NEEDED] = 0;
			dictionary[CityManager.ECITYBUILDINGS.LAVA_LAB_PERIL] = 0;
		}
		else
		{
			dictionary = new Dictionary<CityManager.ECITYBUILDINGS, int>();
			dictionary[CityManager.ECITYBUILDINGS.FIRE_STATION] = 0;
			dictionary[CityManager.ECITYBUILDINGS.LIGHTHOUSE] = 0;
			dictionary[CityManager.ECITYBUILDINGS.FIRE_CONTAINERS] = 0;
			dictionary[CityManager.ECITYBUILDINGS.POLICE_ISLAND_HQ] = 250;
			dictionary[CityManager.ECITYBUILDINGS.POLICE_BOATS] = 250;
			dictionary[CityManager.ECITYBUILDINGS.JETSKI_PATROL] = 800;
			dictionary[CityManager.ECITYBUILDINGS.HELICOPTER_REQUESTED] = 0;
			dictionary[CityManager.ECITYBUILDINGS.LAVA_LIFTERS_NEEDED] = 0;
			dictionary[CityManager.ECITYBUILDINGS.LAVA_LAB_PERIL] = 0;
		}
		return dictionary;
	}

	public void SelectScenario(int slotID)
	{
		if (slotID >= 0 && slotID < 3)
		{
			_currentScenario = _slottedScenarios[slotID];
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setInt("SelectedScenarios", slotID);
		}
	}

	public Scenario GetSlottedScenario(int slotID)
	{
		if (slotID < 0 || slotID >= 3)
		{
			return null;
		}
		return _slottedScenarios[slotID];
	}

	public void AddBrickReward(int numBricks)
	{
		_currentScenario.AddBricks(numBricks);
	}

	public void FindAvailableScenarios()
	{
		List<Scenario> list = new List<Scenario>();
		List<Scenario> list2 = new List<Scenario>();
		List<Scenario> list3 = new List<Scenario>();
		int num = allScenarios.Length;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < num; i++)
		{
			bool flag = allScenarios[i] == _slottedScenarios[0] || allScenarios[i] == _slottedScenarios[1] || allScenarios[i] == _slottedScenarios[2];
			if (allScenarios[i].isAvailable && allScenarios[i]._pProgress == 0f && !flag)
			{
				list.Add(allScenarios[i]);
				continue;
			}
			num2++;
			if (flag && allScenarios[i]._pProgress == 0f)
			{
				list2.Add(allScenarios[i]);
				list3.Add(allScenarios[i]);
				num3++;
			}
			else if (!flag && allScenarios[i]._pProgress == 1f)
			{
				list2.Add(allScenarios[i]);
			}
		}
		if (list.Count < 3)
		{
			Debug.Log("Less than three available scenarios [discarded: " + num2 + " of " + num + "]");
			Debug.Log("There are: " + list2.Count + " possible restart candidates");
			int num4 = 3 - list.Count;
			if (num3 < num4)
			{
				Debug.Log("We need " + num4 + " but only " + num3 + " candidates are already slotted, so shuffling the list");
				list2 = Shuffle(list2);
			}
			else
			{
				Debug.Log("We have enough items that are already slotted with no progress to make up the shortfall");
				list2 = list3;
				list2 = Shuffle(list2);
			}
			int count = list2.Count;
			int num5 = 0;
			for (int j = 0; j < count; j++)
			{
				if (list2[j]._pProgress == 0f)
				{
					num5++;
				}
			}
			if (num5 >= num4)
			{
				for (int k = 0; k < count; k++)
				{
					if (list2[k]._pProgress == 0f)
					{
						Debug.Log("We have enough 0 progress restart candidates to make up the shortfall");
						list.Add(list2[k]);
					}
				}
			}
			else
			{
				List<Scenario> list4 = new List<Scenario>();
				for (int l = 0; l < count; l++)
				{
					if (list2[l]._pProgress == 0f)
					{
						Debug.Log("Enforcing 0 progress priority");
						list.Add(list2[l]);
						list4.Add(list2[l]);
					}
				}
				int count2 = list4.Count;
				for (int m = 0; m < count2; m++)
				{
					list2.Remove(list4[m]);
				}
				num4 = 3 - list.Count;
				for (int n = 0; n < num4; n++)
				{
					Debug.Log("Recycling: " + list2[n]);
					list.Add(list2[n]);
				}
			}
		}
		list = Shuffle(list);
		if (_slottedScenarios[0] == null || _slottedScenarios[1] == null || _slottedScenarios[2] == null)
		{
			_slottedScenarios[0] = list[0];
			_slottedScenarios[1] = list[1];
			_slottedScenarios[2] = list[2];
			_slottedScenarios[0] = allScenarios[5];
			_slottedScenarios[1] = allScenarios[1];
			_slottedScenarios[2] = allScenarios[0];
		}
		else
		{
			int curPossibleScenarioIndex = 0;
			curPossibleScenarioIndex = AttemptSlot(0, list, curPossibleScenarioIndex);
			curPossibleScenarioIndex = AttemptSlot(1, list, curPossibleScenarioIndex);
			curPossibleScenarioIndex = AttemptSlot(2, list, curPossibleScenarioIndex);
		}
		_currentScenario = _slottedScenarios[0];
		if (!AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.hasKey("SelectedScenarios"))
		{
			_pIsFreshSave = true;
		}
		AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setInt("SelectedScenarios", 0);
		AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setStringArray("SlottedScenarios", new string[3]
		{
			_slottedScenarios[0].scenarioName,
			_slottedScenarios[1].scenarioName,
			_slottedScenarios[2].scenarioName
		});
	}

	public Scenario GetScenarioFromBuildingID(CityManager.ECITYBUILDINGS buildingID)
	{
		for (int i = 0; i < allScenarios.Length; i++)
		{
			if (allScenarios[i].building == buildingID)
			{
				return allScenarios[i];
			}
		}
		return null;
	}

	public bool IsSlottedScenarioCity(CityManager.ECITYBUILDINGS buildingID)
	{
		return GetSlottedScenario(0).building == buildingID || GetSlottedScenario(1).building == buildingID || GetSlottedScenario(2).building == buildingID;
	}

	private Scenario GetScenario(string scenarioId)
	{
		int i = 0;
		for (int num = allScenarios.Length; i < num; i++)
		{
			if (!(allScenarios[i].scenarioName != scenarioId))
			{
				return allScenarios[i];
			}
		}
		return null;
	}

	private List<T> Shuffle<T>(List<T> listToShuffle)
	{
		int count = listToShuffle.Count;
		for (int i = 0; i < count; i++)
		{
			T value = listToShuffle[i];
			int index = UnityEngine.Random.Range(i, count);
			listToShuffle[i] = listToShuffle[index];
			listToShuffle[index] = value;
		}
		return listToShuffle;
	}

	private int AttemptSlot(int index, List<Scenario> scenarios, int curPossibleScenarioIndex)
	{
		int num = curPossibleScenarioIndex;
		if (_slottedScenarios[index] == null)
		{
			_slottedScenarios[index] = scenarios[num];
			num++;
		}
		else if (_slottedScenarios[index]._pProgress == 0f || _slottedScenarios[index]._pProgress == 1f)
		{
			_slottedScenarios[index] = scenarios[num];
			num++;
		}
		return num;
	}

	private void addDebugMenuOptions()
	{
		if (!AmuzoMonoSingleton<AmuzoDebugMenuManager>._pExists)
		{
			return;
		}
		Func<string> textAreaFunction = () => "CURRENT SCENARIO: Name [" + LocalisationFacade.Instance.GetString(_pCurrentScenario.scenarioName) + "], Bricks [" + _pCurrentScenario._pCurrentBricks + "/" + _pCurrentScenario.totalBricksRequired + "] Percent [" + _pCurrentScenario._pProgress * 100f + "%]";
		AmuzoDebugMenu amuzoDebugMenu = new AmuzoDebugMenu("ADD BRICKS");
		amuzoDebugMenu.AddInfoTextFunction(textAreaFunction);
		amuzoDebugMenu.AddButton(new AmuzoDebugMenuButton("ADD 10", delegate
		{
			AddBrickReward(10);
			refreshHUD();
		}));
		amuzoDebugMenu.AddButton(new AmuzoDebugMenuButton("ADD 100", delegate
		{
			AddBrickReward(100);
			refreshHUD();
		}));
		amuzoDebugMenu.AddButton(new AmuzoDebugMenuButton("ADD 1000", delegate
		{
			AddBrickReward(1000);
			refreshHUD();
		}));
		Func<string> textAreaFunction2 = delegate
		{
			string text = "CURRENT SCENARIO: Name [" + LocalisationFacade.Instance.GetString(_pCurrentScenario.scenarioName) + "]" + AmuzoDebugMenu.NEW_LINE;
			for (int i = 0; i < 3; i++)
			{
				string text2 = text;
				text = text2 + "SCENARIO SLOT " + i + ": Name [" + LocalisationFacade.Instance.GetString(_slottedScenarios[i].scenarioName) + "], Bricks [" + _slottedScenarios[i]._pCurrentBricks + "/" + _slottedScenarios[i].totalBricksRequired + "] Percent [" + _slottedScenarios[i]._pProgress * 100f + "%]" + AmuzoDebugMenu.NEW_LINE;
			}
			return text;
		};
		AmuzoDebugMenu amuzoDebugMenu2 = new AmuzoDebugMenu("SCENARIO MENU");
		amuzoDebugMenu2.AddInfoTextFunction(textAreaFunction2, false);
		amuzoDebugMenu2.AddButton(new AmuzoDebugMenuButton(amuzoDebugMenu));
		AmuzoDebugMenuManager.RegisterRootDebugMenu(amuzoDebugMenu2);
	}

	private void refreshHUD()
	{
		if (!(ScreenHub._pInstance == null))
		{
			ScreenHub._pInstance.RefreshAfterRewards();
		}
	}
}
