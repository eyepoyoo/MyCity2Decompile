using UnityEngine;

public class LayerBasedCuller : MonoBehaviour
{
	public float _cullDistanceStud;

	public float _cullDistanceCollateral;

	public float _cullDistanceVehicles;

	public bool _cullVehicles = true;

	private void Start()
	{
		Camera component = GetComponent<Camera>();
		float[] array = new float[32];
		array[LayerMask.NameToLayer("Stud")] = _cullDistanceStud;
		array[LayerMask.NameToLayer("CollateralStatic")] = (array[LayerMask.NameToLayer("CollateralDynamic2")] = _cullDistanceCollateral);
		if (_cullVehicles)
		{
			array[LayerMask.NameToLayer("Vehicle")] = (array[LayerMask.NameToLayer("VehicleAttachment")] = _cullDistanceVehicles);
		}
		component.layerCullDistances = array;
	}
}
