using UnityEngine;

[RequireComponent(typeof(Vehicle))]
public abstract class VehicleController : MonoBehaviour
{
	public bool _enabled = true;

	private Vehicle _vehicle;

	public Vehicle _pVehicle
	{
		get
		{
			return _vehicle ?? (_vehicle = GetComponent<Vehicle>());
		}
	}

	public virtual bool _pShouldZeroStearing
	{
		get
		{
			return !_enabled;
		}
	}

	public virtual bool _pShouldZeroAcceleration
	{
		get
		{
			return !_enabled;
		}
	}

	public virtual bool _pWasSpecialPressed
	{
		get
		{
			return false;
		}
	}

	public virtual bool _pIsSpecialDown
	{
		get
		{
			return false;
		}
	}

	public virtual bool _pWasSpecialReleased
	{
		get
		{
			return false;
		}
	}

	protected virtual void Awake()
	{
	}

	public virtual void SetSpecialPressed(bool pressed)
	{
	}
}
