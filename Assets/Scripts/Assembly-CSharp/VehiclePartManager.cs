using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class VehiclePartManager : MonoBehaviour
{
	private static VehiclePartManager _instance;

	public VehiclePart[] allVehiclePartPrefabs;

	public VehiclePartProperties[] allVehiclePartProperties;

	private Dictionary<VehiclePart.EUNIQUE_ID, VehiclePartProperties> _partLookup = new Dictionary<VehiclePart.EUNIQUE_ID, VehiclePartProperties>();

	private Dictionary<MinigameManager.EVEHICLE_TYPE, List<VehiclePartProperties>> _terrainTypeLookup = new Dictionary<MinigameManager.EVEHICLE_TYPE, List<VehiclePartProperties>>();

	private Dictionary<VehiclePart.EPART_SLOT_TYPE, List<VehiclePartProperties>> _slotLookup = new Dictionary<VehiclePart.EPART_SLOT_TYPE, List<VehiclePartProperties>>();

	private Dictionary<MinigameManager.EVEHICLE_TYPE, Dictionary<VehiclePart.EPART_SLOT_TYPE, List<VehiclePartProperties>>> _terrainTypeAndSlotLookup = new Dictionary<MinigameManager.EVEHICLE_TYPE, Dictionary<VehiclePart.EPART_SLOT_TYPE, List<VehiclePartProperties>>>();

	public static VehiclePartManager _pInstance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		int num = allVehiclePartProperties.Length;
		for (int i = 0; i < num; i++)
		{
			_partLookup[allVehiclePartProperties[i].uniqueID] = allVehiclePartProperties[i];
			if (!_slotLookup.ContainsKey(allVehiclePartProperties[i].slotType))
			{
				_slotLookup[allVehiclePartProperties[i].slotType] = new List<VehiclePartProperties>();
			}
			if (allVehiclePartProperties[i].partIsAvailable)
			{
				_slotLookup[allVehiclePartProperties[i].slotType].Add(allVehiclePartProperties[i]);
			}
			int num2 = allVehiclePartProperties[i].vehicleTypeCategories.Length;
			for (int j = 0; j < num2; j++)
			{
				MinigameManager.EVEHICLE_TYPE key = allVehiclePartProperties[i].vehicleTypeCategories[j];
				if (!_terrainTypeLookup.ContainsKey(key))
				{
					_terrainTypeLookup[key] = new List<VehiclePartProperties>();
				}
				if (allVehiclePartProperties[i].partIsAvailable)
				{
					_terrainTypeLookup[key].Add(allVehiclePartProperties[i]);
				}
				if (!_terrainTypeAndSlotLookup.ContainsKey(key))
				{
					_terrainTypeAndSlotLookup[key] = new Dictionary<VehiclePart.EPART_SLOT_TYPE, List<VehiclePartProperties>>();
				}
				if (!_terrainTypeAndSlotLookup[key].ContainsKey(allVehiclePartProperties[i].slotType))
				{
					_terrainTypeAndSlotLookup[key][allVehiclePartProperties[i].slotType] = new List<VehiclePartProperties>();
				}
				if (allVehiclePartProperties[i].partIsAvailable)
				{
					_terrainTypeAndSlotLookup[key][allVehiclePartProperties[i].slotType].Add(allVehiclePartProperties[i]);
				}
			}
		}
		AddDebugMenuOptions();
	}

	public VehiclePartProperties GetPartPropertiesFromPartID(VehiclePart.EUNIQUE_ID uid)
	{
		return _partLookup[uid];
	}

	public List<VehiclePartProperties> GetAllPartPropertiesForTerrainType(MinigameManager.EVEHICLE_TYPE vehicleType)
	{
		return _terrainTypeLookup[vehicleType];
	}

	public List<VehiclePartProperties> GetAllPartPropertiesForSlotType(VehiclePart.EPART_SLOT_TYPE slotType)
	{
		return _slotLookup[slotType];
	}

	public List<VehiclePartProperties> GetAllPartPropertiesForSlotAndTerrainType(MinigameManager.EVEHICLE_TYPE vehicleType, VehiclePart.EPART_SLOT_TYPE slotType)
	{
		Debug.Log("GetAllPrefabsForSlotAndTerrainType: " + vehicleType);
		return _terrainTypeAndSlotLookup[vehicleType][slotType];
	}

	public bool IsPartUnlocked(VehiclePart.EUNIQUE_ID partID)
	{
		return GlobalInGameData.HasPartBeenUnlocked(partID);
	}

	public void UnlockAll()
	{
		int num = allVehiclePartProperties.Length;
		for (int i = 0; i < num; i++)
		{
			GlobalInGameData.MarkItemUnlocked(allVehiclePartProperties[i]);
		}
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
			int num = allVehiclePartProperties.Length;
			for (int i = 0; i < num; i++)
			{
				stringBuilder.Append(allVehiclePartProperties[i]._resourceString);
				stringBuilder.Append(" [Available: ");
				stringBuilder.Append(allVehiclePartProperties[i].partIsAvailable);
				stringBuilder.Append("] [Unlocked: ");
				stringBuilder.Append(allVehiclePartProperties[i]._pHasBeenUnlocked);
				stringBuilder.Append("]\r\n");
			}
			return stringBuilder.ToString();
		};
		AmuzoDebugMenu amuzoDebugMenu = new AmuzoDebugMenu("UNLOCK VEHICLE PARTS");
		amuzoDebugMenu.AddInfoTextFunction(textAreaFunction);
		amuzoDebugMenu.AddButton(new AmuzoDebugMenuButton("UNLOCK ALL", delegate
		{
			UnlockAll();
		}));
		AmuzoDebugMenuManager.RegisterRootDebugMenu(amuzoDebugMenu);
	}
}
