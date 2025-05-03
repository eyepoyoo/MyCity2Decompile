using System.Collections.Generic;
using UnityEngine;
using UnitySampleAssets.Vehicles.Car;

public class CarouselBehaviour : MonoBehaviour
{
	[HideInInspector]
	public bool _isInCarousel;

	[SerializeField]
	[HideInInspector]
	private bool _isCarouselMode;

	[HideInInspector]
	[SerializeField]
	private List<Component> _disabledComponents = new List<Component>();

	private VehiclePart _vehiclePartRef;

	private bool _isHeightManualOverrideSet;

	private float _heightManualOverride;

	private bool _isSilhouetted;

	public bool _pIsSilhouetted
	{
		get
		{
			return _isSilhouetted;
		}
		set
		{
			_isSilhouetted = value;
		}
	}

	public bool _pIsCarouselMode
	{
		get
		{
			return _isCarouselMode;
		}
		set
		{
			if (value != _isCarouselMode)
			{
				_isCarouselMode = value;
				if (value)
				{
					DisableUnnecessaryComponents();
				}
				else
				{
					RestoreUnnecessaryComponents();
				}
			}
		}
	}

	public float _pOffset
	{
		get
		{
			if (_vehiclePartRef == null)
			{
				_vehiclePartRef = GetComponent<VehiclePart>();
			}
			return _vehiclePartRef.carouselHeightOffset + _heightManualOverride;
		}
	}

	public VehiclePart _pVehiclePartRef
	{
		get
		{
			if (_vehiclePartRef == null)
			{
				_vehiclePartRef = GetComponent<VehiclePart>();
			}
			return _vehiclePartRef;
		}
	}

	public float _pHeightManualOverride
	{
		get
		{
			return _heightManualOverride;
		}
	}

	public static CarouselBehaviour GetCarouselReadyClone(GameObject original)
	{
		original.gameObject.SetActive(false);
		GameObject gameObject = Object.Instantiate(original.gameObject);
		original.gameObject.SetActive(true);
		CarouselBehaviour carouselBehaviour = gameObject.AddComponent<CarouselBehaviour>();
		carouselBehaviour._pIsCarouselMode = true;
		gameObject.SetActive(true);
		return carouselBehaviour;
	}

	public void SetHeightOverride(float heightOverride)
	{
		_isHeightManualOverrideSet = true;
		_heightManualOverride = heightOverride;
	}

	private void DisableUnnecessaryComponents()
	{
		Component[] componentsInChildren = GetComponentsInChildren<Component>(true);
		int num = componentsInChildren.Length;
		for (int i = 0; i < num; i++)
		{
			Component component = componentsInChildren[i];
			bool flag = component is VehiclePart || component is CarouselBehaviour || component is Connectors || component is Vehicle || component is Wheel || component is SpecialAbility || component is Buoyancy || component is Animator;
			if (component is Behaviour && !flag)
			{
				Object.DestroyImmediate(component);
				continue;
			}
			if (component is Behaviour && flag && !(component is Animator))
			{
				((Behaviour)component).enabled = false;
			}
			if (component is Collider)
			{
				if (component is WheelCollider)
				{
					((WheelCollider)component).enabled = false;
				}
				else
				{
					Object.DestroyImmediate(component);
				}
			}
			if (component is Joint)
			{
				Object.DestroyImmediate(component);
			}
			if (component is Animator)
			{
				((Animator)component).speed = 0f;
			}
		}
		for (int j = 0; j < num; j++)
		{
			Component component2 = componentsInChildren[j];
			if (component2 is Rigidbody)
			{
				if (component2.GetComponent<Buoyancy>() != null)
				{
					((Rigidbody)component2).useGravity = false;
				}
				else
				{
					Object.DestroyImmediate(component2);
				}
			}
		}
	}

	private void RestoreUnnecessaryComponents()
	{
	}

	public void UpdateOverride()
	{
		if (_isHeightManualOverrideSet)
		{
			_isHeightManualOverrideSet = false;
		}
		else if (_heightManualOverride != 0f)
		{
			float num = Time.deltaTime * 5f * 3f;
			if (Mathf.Abs(_heightManualOverride) < num)
			{
				_heightManualOverride = 0f;
			}
			else
			{
				_heightManualOverride = Mathf.MoveTowards(_heightManualOverride, 0f, num);
			}
		}
	}
}
