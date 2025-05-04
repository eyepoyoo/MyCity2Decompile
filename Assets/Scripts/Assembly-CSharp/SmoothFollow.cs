using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SmoothFollow : MonoBehaviour
{
	private const float DIST_MULTI_CHANGE_HALFLIFE = 0.2f;

	private const float RUMBLE_FREQUENCY_SCALE_FACTOR = 5f;

	private const float RUMBLE_FREQUENCY_MAX = 10f;

	private const float RUMBLE_MAGNITUDE_SCALE_FACTOR = 0.01f;

	private const float RUMBLE_MAGNITUDE_MAX = 0.07f;

	private const float RUMBLE_ROTATION_SCALE_FACTOR = 0.05f;

	private const float RUMBLE_ROTATION_MAX = 0.3f;

	private const float BUMP_DURATION = 0.4f;

	private const float BUMP_WOBBLE_FREQ = 2.5f;

	private const float BUMP_FORCE_SCALE_FACTOR = 0.07f;

	private const float OVERRIDE_SWITCH_TIME = 0.5f;

	private const float OVERRIDE_SWITCH_SPEED = 2f;

	private const float RAYCAST_INTERVAL = 0.2f;

	private const float ANTI_CLIP_DIST_SPEED = 10f;

	private const float ANTI_CLIP_BUFFER = 1f;

	public bool fixedUpdate;

	public Transform target;

	public float distance = 10f;

	public float height = 5f;

	public float heightDamping = 2f;

	public float rotationDamping = 3f;

	public Vector3 _positionOffset;

	public Vector3 _lookOffset;

	public float _distScale = 1f;

	private Transform _targetPrev;

	private float _initFov;

	private float _speedScale = 1f;

	private float _targetSpeedScale = 1f;

	private readonly Dictionary<object, float> _speedMultiRequests = new Dictionary<object, float>();

	private Vector3 _posDefault;

	private float _rumbleTimer;

	private float _diggerTimer;

	private float _playerSpeedEased;

	private float _lastBumpTime = float.NegativeInfinity;

	private Vector3 _lastBumpForce;

	private Vector3 _positionOffsetInit;

	private Vector3 _lookOffsetInit;

	private Quaternion _rotDefault;

	private float _fovDefault;

	private Camera _override;

	private Vector3 _posOverride;

	private Quaternion _rotOverride;

	private float _fovOverride;

	private float _overrideWeight;

	private float _raycastTimer;

	private int _raycastLayerMask;

	private float _antiClipDistCurr = float.PositiveInfinity;

	private bool _isClipping;

	public bool _pRumbleEnabled { get; private set; }

	public bool _pDiggerEnabled { get; set; }

	public Camera _pCamera { get; private set; }

	private float _pTimeSinceLastBump
	{
		get
		{
			return Time.time - _lastBumpTime;
		}
	}

	private Vector3 _pBumpOffset
	{
		get
		{
			return _lastBumpForce * MathHelper.Sqr(1f - _pTimeSinceLastBump / 0.4f) * Mathf.Sin(_pTimeSinceLastBump * ((float)Math.PI * 2f) * 2.5f);
		}
	}

	private Vector3 _pRumbleOffsetPos
	{
		get
		{
			return Vector3.up * Mathf.Sin(_rumbleTimer) * Mathf.Min(0.01f * _playerSpeedEased, 0.07f);
		}
	}

	private Vector3 _pDiggerOffsetPos
	{
		get
		{
			return Vector3.up * Mathf.Sin(_diggerTimer) * 0.1f;
		}
	}

	private Quaternion _pRumbleOffsetRot
	{
		get
		{
			return Quaternion.AngleAxis(Mathf.Sin(_rumbleTimer * 0.73f) * Mathf.Min(0.05f * _playerSpeedEased, 0.3f), Vector3.forward);
		}
	}

	public Camera _pOverride
	{
		get
		{
			return _override;
		}
		set
		{
			if (!(value == _override))
			{
				_override = value;
			}
		}
	}

	private void OnEnable()
	{
		_pCamera = GetComponent<Camera>();
		_initFov = _pCamera.fieldOfView;
		_posDefault = base.transform.position;
		_rotDefault = base.transform.rotation;
		_fovDefault = _pCamera.fieldOfView;
		_positionOffsetInit = _positionOffset;
		_lookOffsetInit = _lookOffset;
		_raycastLayerMask = LayerMask.GetMask("Geometry");
	}

	private void Start()
	{
		MinigameController._pInstance._onPlayerDestroyedCollateral += OnPlayerDestroyedCollateral;
		VehicleController_Player._pInstance._pVehicle._onHitVehicle += OnPlayerHitVehicle;
		PlayerTrigger._onEnter += OnPlayerTriggerEntered;
		PlayerTrigger._onExit += OnPlayerTriggerExited;
	}

	private void Update()
	{
		if (!fixedUpdate)
		{
			_Update(Time.deltaTime);
		}
	}

	private void FixedUpdate()
	{
		if (fixedUpdate)
		{
			_Update(Time.fixedDeltaTime);
		}
	}

	private void _Update(float dt)
	{
		if ((bool)target)
		{
			if (target != _targetPrev)
			{
				OnTargetChanged();
				_targetPrev = target;
			}
			float value = _rotDefault.eulerAngles.y;
			MathHelper.EaseTowardsAngle(ref value, target.eulerAngles.y, rotationDamping, dt);
			Quaternion quaternion = Quaternion.Euler(0f, value, 0f);
			float value2 = _posDefault.y;
			float to = target.position.y + height * _distScale;
			MathHelper.EaseTowards(ref value2, to, heightDamping, dt);
			MathHelper.EaseTowards(ref _speedScale, _targetSpeedScale, 0.2f, dt, 0.01f);
			_fovDefault = _initFov * _speedScale;
			_posDefault = target.position - quaternion * Vector3.forward * distance * _distScale;
			_posDefault.y = value2;
			_rotDefault = Quaternion.LookRotation(target.position + _lookOffset * _distScale - _posDefault);
			_posDefault += quaternion * _positionOffset;
			MathHelper.EaseTowards(ref _playerSpeedEased, VehicleController_Player._pInstance._pVehicle._pRigidbody.velocity.magnitude, 0.1f, dt);
			_overrideWeight = Mathf.MoveTowards(_overrideWeight, _pOverride ? 1 : 0, 2f * dt);
			if ((bool)_pOverride)
			{
				_posOverride = _pOverride.transform.position;
				_rotOverride = _pOverride.transform.rotation;
				_fovOverride = _pOverride.fieldOfView;
			}
			if (_overrideWeight == 0f)
			{
				base.transform.position = _posDefault;
				base.transform.rotation = _rotDefault;
				_pCamera.fieldOfView = _fovDefault;
			}
			else if (_overrideWeight == 1f)
			{
				base.transform.position = _posOverride;
				base.transform.rotation = _rotOverride;
				_pCamera.fieldOfView = _fovOverride;
			}
			else
			{
				float t = Easing.Ease(Easing.EaseType.EaseInOut, _overrideWeight, 1f, 0f, 1f);
				base.transform.position = Vector3.Lerp(_posDefault, _posOverride, t);
				base.transform.rotation = Quaternion.Lerp(_rotDefault, _rotOverride, t);
				_pCamera.fieldOfView = Mathf.Lerp(_fovDefault, _fovOverride, t);
			}
			if (_pRumbleEnabled)
			{
				float a = _playerSpeedEased * 5f;
				_rumbleTimer += dt * Mathf.Min(a, (float)Math.PI * 20f);
				base.transform.position += _pRumbleOffsetPos;
				base.transform.rotation *= _pRumbleOffsetRot;
			}
			if (_pDiggerEnabled)
			{
				_diggerTimer += dt * ((float)Math.PI * 2f) * 10f;
				base.transform.position += _pDiggerOffsetPos;
			}
			if (_pTimeSinceLastBump < 0.4f)
			{
				base.transform.position += _pBumpOffset;
			}
			UpdateAntiClip(dt, target.position + Vector3.up * 5f);
		}
	}

	private void OnDestroy()
	{
		PlayerTrigger._onEnter -= OnPlayerTriggerEntered;
		PlayerTrigger._onExit -= OnPlayerTriggerExited;
	}

	public void Snap()
	{
		_overrideWeight = (_pOverride ? 1 : 0);
		_posDefault.y = target.position.y + height * _distScale;
		_rotDefault = target.rotation;
		_Update(0f);
	}

	public void EaseToDefault()
	{
		_pOverride = null;
		_posOverride = base.transform.position;
		_rotOverride = base.transform.rotation;
		_fovOverride = _pCamera.fieldOfView;
		_overrideWeight = 1f;
	}

	public void AddSpeedScaleRequest(object target, float multi)
	{
		if (_speedMultiRequests.ContainsKey(target))
		{
			_speedMultiRequests[target] = multi;
		}
		else
		{
			_speedMultiRequests.Add(target, multi);
		}
		CalculateSpeedScale();
	}

	public void RemoveSpeedScaleRequest(object target)
	{
		_speedMultiRequests.Remove(target);
		CalculateSpeedScale();
	}

	private void CalculateSpeedScale()
	{
		_targetSpeedScale = 1f;
		foreach (float value in _speedMultiRequests.Values)
		{
			float num = value;
			_targetSpeedScale *= num;
		}
	}

	public void Bump(Vector3 force)
	{
		_lastBumpTime = Time.time;
		_lastBumpForce = force;
	}

	private void OnTargetChanged()
	{
		VehiclePart vehiclePart = Array.Find(target.GetComponentsInChildren<VehiclePart>(), (VehiclePart vp) => vp.slotType == VehiclePart.EPART_SLOT_TYPE.BODY);
		if ((bool)vehiclePart)
		{
			_distScale = vehiclePart._minigameCamDistScale;
		}
		_positionOffset = _positionOffsetInit;
		_lookOffset = _lookOffsetInit;
		Vehicle component = target.GetComponent<Vehicle>();
		if (!component)
		{
			return;
		}
		SpecialAbility_Lifter componentInChildren = component.GetComponentInChildren<SpecialAbility_Lifter>();
		if ((bool)componentInChildren)
		{
			Vector3 vector = target.InverseTransformPoint(componentInChildren._anchor.position);
			if (vector.z < 0f)
			{
				_positionOffset += Vector3.back * (0f - vector.z);
			}
		}
		if ((bool)component.GetComponentInChildren<SpecialAbility_Digger>())
		{
			_positionOffset += Vector3.back * 6f;
		}
		if ((bool)component.GetComponentInChildren<Grapple>())
		{
			_positionOffset += Vector3.back * 2.76f;
			_lookOffset.y = 0f;
		}
	}

	private void UpdateAntiClip(float dt, Vector3 targPos)
	{
		bool flag = false;
		if ((_raycastTimer += dt) > 0.2f)
		{
			_raycastTimer -= 0.2f;
			flag = true;
		}
		if (flag || _isClipping)
		{
			Vector3 hitPoint;
			if (DoAntiClipRaycast(out hitPoint, targPos))
			{
				_antiClipDistCurr = Vector3.Distance(hitPoint, targPos) - 1f;
				_isClipping = true;
			}
			else
			{
				_isClipping = false;
			}
		}
		Vector3 vector = base.transform.position - targPos;
		float magnitude = vector.magnitude;
		if (_antiClipDistCurr < magnitude)
		{
			base.transform.position = targPos + vector / magnitude * _antiClipDistCurr;
		}
		_antiClipDistCurr += dt * 10f;
	}

	private bool DoAntiClipRaycast(out Vector3 hitPoint, Vector3 targPos)
	{
		hitPoint = Vector3.zero;
		if (!target)
		{
			return false;
		}
		Vector3 vector = base.transform.position - targPos;
		float magnitude = vector.magnitude;
		RaycastHit hitInfo;
		if (Physics.Raycast(targPos, vector / magnitude, out hitInfo, magnitude + 1f + 1f, _raycastLayerMask) && hitInfo.collider.tag != "IgnoreRaycast")
		{
			hitPoint = hitInfo.point;
			return true;
		}
		return false;
	}

	private void OnPlayerDestroyedCollateral(Collateral collateral)
	{
		Bump(VehicleController_Player._pInstance._pVehicle._pRigidbody.velocity * 0.07f);
	}

	private void OnPlayerHitVehicle(Vehicle vehicle, Collision collision)
	{
		Bump(collision.relativeVelocity * 0.07f);
	}

	private void OnPlayerTriggerEntered(PlayerTrigger trigger)
	{
		if (trigger._rumbleCamera)
		{
			_pRumbleEnabled = true;
		}
	}

	private void OnPlayerTriggerExited(PlayerTrigger trigger)
	{
		if (trigger._rumbleCamera)
		{
			_pRumbleEnabled = false;
		}
	}
}
