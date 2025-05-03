using AmuzoPhysics;
using UnityEngine;

public class SpecialAbility_Ladder : SpecialAbility
{
	public Joint _rootJoint;

	public HingeJoint _base;

	public HingeJoint _tilt;

	public ConfigurableJoint _ladder;

	public Transform _tip;

	private Vector3 _initSliderTargetPosition;

	protected override void Awake()
	{
		base.Awake();
		if (!(_ladder == null))
		{
			_initSliderTargetPosition = _ladder.targetPosition;
			_ladder.transform.localPosition = _ladder.connectedBody.transform.localPosition + _ladder.connectedAnchor + new Vector3(_ladder.targetPosition.x, _ladder.targetPosition.y, Mathf.Clamp(_ladder.targetPosition.z, 0f - _ladder.linearLimit.limit, _ladder.linearLimit.limit));
		}
	}

	public override void AssignToVehicle(Vehicle vehicle)
	{
		base.AssignToVehicle(vehicle);
		if (base._pVehicle != null && base._pVehicle._pRigidbody != null && _rootJoint != null)
		{
			PhysicsHelper.AttachTo(base.transform, _rootJoint, base._pVehicle._pRigidbody);
		}
	}

	protected override void OnStarted()
	{
		base.OnStarted();
		_tilt.useMotor = true;
	}

	protected override void OnEnded()
	{
		base.OnEnded();
		_tilt.useMotor = false;
		_ladder.targetPosition = _initSliderTargetPosition;
	}

	protected override void Update()
	{
		base.Update();
		if (base._pIsInUse)
		{
			CheckPickupCollisions();
		}
	}

	private void CheckPickupCollisions()
	{
		int numItemsReturned = 0;
		Pickup[] pickupsAtPos = Pickup.GetPickupsAtPos(base.transform.position, out numItemsReturned);
		for (int i = 0; i < numItemsReturned; i++)
		{
			Pickup pickup = pickupsAtPos[i];
			if (pickup.enabled && pickup.gameObject.activeInHierarchy)
			{
				float num = MathHelper.DistSqrd(pickup.transform.position, _tip.position);
				if (!pickup._pIsGravitating && num < 0.1f)
				{
					pickup.Collect(pickup.transform.position);
				}
				else if (num < 10f)
				{
					pickup.Gravitate(_tip);
				}
			}
		}
	}
}
