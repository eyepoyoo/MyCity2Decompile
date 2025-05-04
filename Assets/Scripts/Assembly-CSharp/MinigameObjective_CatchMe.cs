using UnityEngine;

[RequireComponent(typeof(VehicleController))]
public class MinigameObjective_CatchMe : MinigameObjective
{
	public Transform _from;

	public Transform _to;

	public float _radius;

	private Vector3 _fromTo;

	private float _fromToDist;

	private float _radiusSqrd;

	private VehicleController _vehicleController;

	public float _pProgressPlayer
	{
		get
		{
			return GetNormProgress(VehicleController_Player._pInstance.transform);
		}
	}

	public float _pProgressThis
	{
		get
		{
			return GetNormProgress(base.transform);
		}
	}

	public override float _pNormProgress
	{
		get
		{
			return _pProgressThis;
		}
	}

	private void Start()
	{
		_fromTo = (_to.position - _from.position).normalized;
		_fromToDist = (_to.position - _from.position).magnitude;
		_radiusSqrd = _radius * _radius;
		_vehicleController = GetComponent<VehicleController>();
	}

	private void Update()
	{
		if (base._pEnabled)
		{
			Progress();
			if (MathHelper.DistXZSqrd(VehicleController_Player._pInstance.transform.position, base.transform.position) < _radiusSqrd)
			{
				_vehicleController._pVehicle.Brake(999f);
				_vehicleController._enabled = false;
				Complete();
			}
			if (_pProgressThis == 1f)
			{
				Fail();
			}
		}
	}

	private float GetNormProgress(Transform trans)
	{
		return Mathf.Clamp01(Vector3.Dot(_fromTo, trans.position - _from.position) / _fromToDist);
	}

	private void OnDrawGizmos()
	{
		GizmosPlus.drawCircle(base.transform.position, _radius);
	}
}
