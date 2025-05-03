using UnityEngine;
using VacuumShaders.CurvedWorld;

public class BuildingClickableRegion : MonoBehaviour
{
	public CityManager.ECITYBUILDINGS buildingType;

	private Vector3 _originalPos;

	private void Awake()
	{
		_originalPos = base.transform.position;
	}

	private void Update()
	{
		if (!(CurvedWorld_Controller.get == null))
		{
			base.transform.position = CurvedWorld_Controller.get.TransformPoint(_originalPos);
		}
	}
}
