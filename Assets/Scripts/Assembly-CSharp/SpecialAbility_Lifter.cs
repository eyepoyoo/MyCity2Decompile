using AmuzoPhysics;
using UnityEngine;

public class SpecialAbility_Lifter : SpecialAbility, IPickerUpper
{
	public Joint _rootJoint;

	public float _grabRange;

	public Transform _anchor;

	public Rigidbody _anchorRigidbody;

	public bool _applyPickupablesDrag;

	public bool _fixRootJointWhileNotGrabbing;

	private float _grabRangeSqrd;

	private float _lastDropTime = float.NegativeInfinity;

	public Pickupable _pCurrentObject { get; private set; }

	public float _pTimeSinceLastDrop
	{
		get
		{
			return Time.time - _lastDropTime;
		}
	}

	private bool _pRootJointFixed
	{
		set
		{
			ConfigurableJoint obj = (ConfigurableJoint)_rootJoint;
			ConfigurableJointMotion configurableJointMotion = ((!value) ? ConfigurableJointMotion.Limited : ConfigurableJointMotion.Locked);
			((ConfigurableJoint)_rootJoint).angularZMotion = configurableJointMotion;
			configurableJointMotion = configurableJointMotion;
			((ConfigurableJoint)_rootJoint).angularYMotion = configurableJointMotion;
			obj.angularXMotion = configurableJointMotion;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		_grabRangeSqrd = _grabRange * _grabRange;
		if (_fixRootJointWhileNotGrabbing)
		{
			if (!(_rootJoint is ConfigurableJoint))
			{
				_fixRootJointWhileNotGrabbing = false;
			}
			else
			{
				_pRootJointFixed = true;
			}
		}
	}

	protected override void Update()
	{
		if (_isAutomated && !_pCurrentObject)
		{
			PickUp(GetClosestPickupable());
		}
		base.Update();
	}

	public override void AssignToVehicle(Vehicle vehicle)
	{
		base.AssignToVehicle(vehicle);
		if (!(vehicle == null) && !(vehicle._pRigidbody == null))
		{
			PhysicsHelper.AttachTo(base.transform, _rootJoint, vehicle._pRigidbody);
		}
	}

	protected override void OnStarted()
	{
		base.OnStarted();
		if (!_pCurrentObject)
		{
			PickUpClosestObject();
		}
		else if (_pCurrentObject._canBeDroppedByPlayer)
		{
			Drop();
		}
	}

	private void PickUpClosestObject()
	{
		Pickupable pickupable = GetClosestPickupable();
		if (!pickupable)
		{
			ReplaceOnCollide closestRoC = GetClosestRoC();
			if ((bool)closestRoC)
			{
				pickupable = closestRoC.Replace().GetComponent<Pickupable>();
			}
		}
		if ((bool)pickupable)
		{
			PickUp(pickupable);
		}
	}

	public void PickUp(Pickupable obj)
	{
		if ((bool)obj && obj.enabled && !(Time.time < _lastDropTime + obj._dropPickupCooldown))
		{
			obj.PickUp(_anchor, (!(_anchorRigidbody != null)) ? base._pVehicle._pRigidbody : _anchorRigidbody, (!_applyPickupablesDrag) ? null : base._pVehicle);
			_pCurrentObject = obj;
			if (_fixRootJointWhileNotGrabbing)
			{
				_pRootJointFixed = false;
			}
		}
	}

	public void Drop()
	{
		if ((bool)_pCurrentObject)
		{
			_pCurrentObject.Drop();
			_pCurrentObject = null;
			_lastDropTime = Time.time;
			if (_fixRootJointWhileNotGrabbing)
			{
				_pRootJointFixed = true;
			}
		}
	}

	private Pickupable GetClosestPickupable()
	{
		float num = _grabRangeSqrd;
		Pickupable result = null;
		for (int num2 = Pickupable._all.Count - 1; num2 >= 0; num2--)
		{
			Pickupable pickupable = Pickupable._all[num2];
			if (MinigameObjective_PickUp.IsInRangeY(pickupable.transform.position, base._pVehicle.transform.position) && pickupable.enabled && !pickupable._pIsTweening)
			{
				float num3 = MathHelper.DistXZSqrd(base._pVehicle.transform.position, pickupable.transform.position);
				if (num3 < num)
				{
					num = num3;
					result = pickupable;
				}
			}
		}
		return result;
	}

	private ReplaceOnCollide GetClosestRoC()
	{
		float num = _grabRangeSqrd;
		ReplaceOnCollide result = null;
		for (int num2 = ReplaceOnCollide._all.Count - 1; num2 >= 0; num2--)
		{
			ReplaceOnCollide replaceOnCollide = ReplaceOnCollide._all[num2];
			if (MinigameObjective_PickUp.IsInRangeY(replaceOnCollide.transform.position, base._pVehicle.transform.position) && (bool)replaceOnCollide._replaceWith.GetComponent<Pickupable>() && replaceOnCollide._replaceWith.GetComponent<Pickupable>().enabled)
			{
				float num3 = MathHelper.DistXZSqrd(base._pVehicle.transform.position, replaceOnCollide.transform.position);
				if (num3 < num)
				{
					num = num3;
					result = replaceOnCollide;
				}
			}
		}
		return result;
	}
}
