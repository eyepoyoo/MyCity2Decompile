using System;
using UnityEngine;

[Serializable]
public class VehiclePartProperties : VehiclePartPropertiesBase
{
	[Serializable]
	public class AltPart
	{
		public Vehicle.EType _type;

		public VehiclePartPropertiesAltPart _partProperties;
	}

	public AltPart[] _altParts;

	public bool _pHasBeenUnlocked
	{
		get
		{
			return GlobalInGameData.HasPartBeenUnlocked(uniqueID);
		}
	}

	public GameObject GetResource()
	{
		return Resources.Load(_resourceString) as GameObject;
	}

	public GameObject Spawn()
	{
		GameObject original = Resources.Load(_resourceString) as GameObject;
		return UnityEngine.Object.Instantiate(original);
	}

	public VehiclePart SpawnAsVehiclePart()
	{
		GameObject gameObject = Resources.Load(_resourceString) as GameObject;
		if (gameObject == null)
		{
			Debug.LogError("Could not load: " + _resourceString + " from resources");
			return null;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
		return gameObject2.GetComponent<VehiclePart>();
	}

	public VehiclePartProperties GetAltPartOrThis(Vehicle.EType type)
	{
		if (_altParts != null)
		{
			for (int num = _altParts.Length - 1; num >= 0; num--)
			{
				if (_altParts[num]._type == type)
				{
					return _altParts[num]._partProperties;
				}
			}
		}
		return this;
	}
}
