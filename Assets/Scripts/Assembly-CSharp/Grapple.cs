using System;
using System.Collections.Generic;
using AmuzoPhysics;
using UnityEngine;

public class Grapple : MonoBehaviour, IPickerUpper
{
	private const float LIFT_DELAY = 0.5f;

	private const float DROP_DELAY = 2f;

	public const float LENGTH = 5f;

	public Joint _rootJoint;

	public Rigidbody _hookRigidbody;

	public Transform _hookAnchor;

	private Vehicle_Air _vehicle;

	private bool _isLifting;

	private bool _isDropping;

	private float _liftTimer;

	private float _dropTimer;

	private float _dropVehicleStartY;

	private float _dropVehicleEndY;

	private Dictionary<ConfigurableJoint, SoftJointLimitSpring> _initJointStiffnesses;

	private SoftJointLimitSpring _stabilisingSpring;

	private Action<Pickupable> _onPickedUp;

	private Action<Pickupable> _onDropped_Once;

	private float _initHookDrag;

	private float _lastDropTime = float.NegativeInfinity;

	private float _dragToAdd;

	public Pickupable _pCurrentObject { get; private set; }

	private bool _pIsStabilising
	{
		set
		{
			if (value)
			{
				_hookRigidbody.drag = 10f;
				{
					foreach (KeyValuePair<ConfigurableJoint, SoftJointLimitSpring> initJointStiffness in _initJointStiffnesses)
					{
						initJointStiffness.Key.angularXLimitSpring = _stabilisingSpring;
						initJointStiffness.Key.angularYZLimitSpring = _stabilisingSpring;
					}
					return;
				}
			}
			_hookRigidbody.drag = _initHookDrag;
			foreach (KeyValuePair<ConfigurableJoint, SoftJointLimitSpring> initJointStiffness2 in _initJointStiffnesses)
			{
				initJointStiffness2.Key.angularXLimitSpring = initJointStiffness2.Value;
				initJointStiffness2.Key.angularYZLimitSpring = initJointStiffness2.Value;
			}
		}
	}

	public float _pTimeSinceLastDrop
	{
		get
		{
			return Time.time - _lastDropTime;
		}
	}

	private void Awake()
	{
		_initHookDrag = _hookRigidbody.drag;
		_initJointStiffnesses = new Dictionary<ConfigurableJoint, SoftJointLimitSpring>();
		ConfigurableJoint[] componentsInChildren = GetComponentsInChildren<ConfigurableJoint>();
		foreach (ConfigurableJoint configurableJoint in componentsInChildren)
		{
			_initJointStiffnesses.Add(configurableJoint, configurableJoint.angularXLimitSpring);
		}
		_stabilisingSpring = default(SoftJointLimitSpring);
		_stabilisingSpring.spring = 50f;
		_stabilisingSpring.damper = 1f;
	}

	public void AssignTo(Vehicle vehicle)
	{
		_vehicle = (Vehicle_Air)vehicle;
		base.transform.position = vehicle.transform.position - vehicle.transform.up * vehicle._pCentreOffsetFromBottom;
		base.transform.rotation = vehicle.transform.rotation;
		base.transform.parent = vehicle.transform;
		PhysicsHelper.AttachTo(base.transform, _rootJoint, vehicle._pRigidbody);
		_dragToAdd = Mathf.Lerp(0.5f, 0f, Mathf.InverseLerp(0.5f, 2f, vehicle._pRigidbody.mass));
	}

	public void PickUp(Pickupable obj, Action<Pickupable> onPickedUp = null)
	{
		if (!_isLifting && !_isDropping && (bool)obj && obj.enabled && !(Time.time < _lastDropTime + obj._dropPickupCooldown))
		{
			_pCurrentObject = obj;
			_pIsStabilising = true;
			_onPickedUp = onPickedUp;
			_isLifting = true;
			_liftTimer = 0f;
			_vehicle._pRigidbody.drag += _dragToAdd;
		}
	}

	public void DropDelayed(Action<Pickupable> onDropped = null, float y = 0f)
	{
		if (!_isLifting && !_isDropping && (bool)_pCurrentObject)
		{
			_pIsStabilising = true;
			_onDropped_Once = onDropped;
			_isDropping = true;
			_dropTimer = 0f;
			_dropVehicleStartY = _vehicle.transform.position.y;
			_dropVehicleEndY = y + 5f + 4f + _vehicle._pCentreOffsetFromBottom;
		}
	}

	public void Drop()
	{
		if ((bool)_pCurrentObject)
		{
			_isDropping = false;
			_dropTimer = 0f;
			_pCurrentObject.Drop();
			if (_onDropped_Once != null)
			{
				_onDropped_Once(_pCurrentObject);
				_onDropped_Once = null;
			}
			_pCurrentObject = null;
			_pIsStabilising = false;
			_vehicle._pRigidbody.drag = _vehicle._pInitDrag;
			_lastDropTime = Time.time;
			_vehicle._yOverride = 0f;
		}
	}

	private void Update()
	{
		if (_isLifting && (_liftTimer += Time.deltaTime) > 0.5f)
		{
			_isLifting = false;
			_liftTimer = 0f;
			if ((bool)_pCurrentObject)
			{
				_pCurrentObject.PickUp(_hookAnchor, _hookRigidbody, null, Easing.EaseType.EaseIn, delegate
				{
					_vehicle._yOverride = 0f;
				});
				if (_onPickedUp != null)
				{
					_onPickedUp(_pCurrentObject);
				}
				_pIsStabilising = false;
			}
		}
		if (_isDropping)
		{
			_vehicle._yOverride = Mathf.Lerp(_dropVehicleStartY, _dropVehicleEndY, _dropTimer / 2f * 1.5f);
			if ((_dropTimer += Time.deltaTime) > 2f)
			{
				Drop();
			}
		}
	}
}
