using System;
using UnityEngine;

public class Vehicle_Air : Vehicle
{
	private const float Y_CORRECTION_ACCELERATION = 15f;

	private const float Y_CORRECTION_DAMPING = 3f;

	private const float TILT_CORRECTION_ACCELERATION = 0.8f;

	private const float TILT_CORRECTION_DAMPING = 0.1f;

	private const float TILT_ACCELERATION = 4f;

	private const float WOBBLE_FREQ_LINEAR = 1.5268141f;

	private const float WOBBLE_FREQ_PITCH = (float)Math.PI * 2f / 5f;

	private const float WOBBLE_FREQ_ROLL = 1.4112035f;

	private const float BRAKE_DECEL_HALFLIFE = 0.2f;

	private const float MAX_TURBULENCE_TOR = 190f;

	private const float MIN_TURBULENCE_TOR = 60f;

	private const float CAM_SPEED_SCALE = 1.5f;

	public float _centreOffsetFromBottom;

	[NonSerialized]
	public float _yOverride;

	private float _forwardSpeed;

	private float _steerInput;

	private float _steerInputEased;

	private float _accelBrakeInput;

	private float _accelBrakeInputEased;

	private AirVehicleEngineProperties _engineProps;

	private float _phaseOffsetLinear;

	private float _phaseOffsetRoll;

	private float _phaseOffsetPitch;

	private float _brakeVelMulti;

	private bool _inTurbulence;

	private float _turbulenceTick;

	private float _turbulenceY = 1f;

	private Vector3 _turbulenceDirection;

	private float _turbulenceLingerTime;

	private float _timeInTurbulence;

	private float _turbulenceTolerance = -1f;

	private float _turbulenceSlowdown;

	private float _speedFactor;

	public float _initY { get; private set; }

	private float _pTargetPitch
	{
		get
		{
			if (base._pIsBraking)
			{
				return 0f;
			}
			return _accelBrakeInputEased * _engineProps._pitchSpeedFactor + _engineProps._wobbleAmountAngular * Mathf.Sin(_phaseOffsetPitch + Time.time * ((float)Math.PI * 2f / 5f));
		}
	}

	private float _pTargetRoll
	{
		get
		{
			if (base._pIsBraking)
			{
				return 0f;
			}
			return (0f - _steerInputEased) * _engineProps._rollSteerFactor + _engineProps._wobbleAmountAngular * Mathf.Cos(_phaseOffsetRoll + Time.time * 1.4112035f);
		}
	}

	public override bool _pIsGrounded
	{
		get
		{
			return false;
		}
	}

	public override float _pBounceOffWallAngVelMulti
	{
		get
		{
			return 28f;
		}
	}

	public override float _pCentreOffsetFromBottom
	{
		get
		{
			return _centreOffsetFromBottom;
		}
	}

	public float _pTurbulenceTolerance
	{
		get
		{
			if (_turbulenceTolerance == -1f)
			{
				_turbulenceTolerance = Mathf.InverseLerp(0.5f, 2f, base._pRigidbody.mass);
			}
			return _turbulenceTolerance;
		}
	}

	private float _pTurbulenceSlowdown
	{
		get
		{
			if (_turbulenceSlowdown == 0f)
			{
				_turbulenceSlowdown = Mathf.Lerp(0.65f, 1f, _pTurbulenceTolerance);
			}
			return _turbulenceSlowdown;
		}
	}

	public bool _pTurbulence
	{
		get
		{
			return _inTurbulence;
		}
		set
		{
			_inTurbulence = value;
			if (_inTurbulence)
			{
				base._pRigidbody.velocity *= _pTurbulenceSlowdown;
				_turbulenceLingerTime = 0.25f;
				_timeInTurbulence = 0f;
			}
		}
	}

	public override float _pSpeedFactor
	{
		get
		{
			return _speedFactor;
		}
	}

	private AirVehicleEngineProperties _pEngineProps
	{
		get
		{
			return _engineProps;
		}
		set
		{
			if (value == _engineProps)
			{
				return;
			}
			_engineProps = value;
			if ((bool)MinigameController._pInstance)
			{
				MinigameController._pInstance._pCamera.RemoveSpeedScaleRequest(this);
				if ((bool)_engineProps && _engineProps._addCamSpeedScale)
				{
					MinigameController._pInstance._pCamera.AddSpeedScaleRequest(this, 1.5f);
				}
			}
		}
	}

	private float _pTargetY
	{
		get
		{
			if (_yOverride != 0f)
			{
				return _yOverride;
			}
			return _initY + _pCentreOffsetFromBottom + Mathf.Sin(_phaseOffsetLinear + Time.time * 1.5268141f) * _engineProps._wobbleAmountLinear;
		}
	}

	private void Start()
	{
		_phaseOffsetLinear = UnityEngine.Random.value * ((float)Math.PI * 2f);
		_phaseOffsetPitch = UnityEngine.Random.value * ((float)Math.PI * 2f);
		_phaseOffsetRoll = UnityEngine.Random.value * ((float)Math.PI * 2f);
		_brakeVelMulti = Mathf.Pow(0.5f, Time.fixedDeltaTime / 0.2f);
		_pEngineProps = GetComponentInChildren<AirVehicleEngineProperties>();
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		_forwardSpeed = Vector3.Dot(base._pRigidbody.velocity, base.transform.forward);
		_speedFactor = Mathf.InverseLerp(0f, 10f, Mathf.Abs(_forwardSpeed));
		_accelBrakeInputEased = Mathf.MoveTowards(_accelBrakeInputEased, _accelBrakeInput, 4f * Time.fixedDeltaTime);
		_steerInputEased = Mathf.MoveTowards(_steerInputEased, _steerInput, 4f * Time.fixedDeltaTime);
		if ((!_inTurbulence && _turbulenceLingerTime <= 0f) || _pTurbulenceTolerance == 1f)
		{
			ApplySteerAccel();
			UpdateTilt();
			base._pRigidbody.EaseToY(_pTargetY, 15f, 3f);
		}
		else
		{
			UpdateTurbulence();
		}
	}

	public override void Move(float steerInput, float accelBrakeInput)
	{
		if ((bool)base._pShouldCoast)
		{
			accelBrakeInput = 0f;
		}
		base.Move(steerInput, accelBrakeInput);
		_accelBrakeInput = accelBrakeInput;
		_steerInput = steerInput;
	}

	private void UpdateTurbulence()
	{
		float min = 60f;
		float max = Mathf.Lerp(190f, 60f, _pTurbulenceTolerance) * base._pRigidbody.mass * base._pRigidbody.mass;
		float num = UnityEngine.Random.Range(min, max) * _turbulenceY;
		_timeInTurbulence += Time.deltaTime;
		if (!_inTurbulence)
		{
			_turbulenceLingerTime -= Time.deltaTime;
		}
		if (_turbulenceDirection == Vector3.zero)
		{
			_turbulenceDirection = UnityEngine.Random.onUnitSphere;
			_turbulenceDirection.y = 0f;
		}
		_turbulenceTick -= Time.deltaTime;
		if (_turbulenceTick < 0f)
		{
			_turbulenceTick = UnityEngine.Random.Range(0.1f, 0.5f);
			_turbulenceDirection = UnityEngine.Random.onUnitSphere;
			_turbulenceDirection.y = 0f;
			_turbulenceY = 0f - _turbulenceY;
		}
		base._pRigidbody.AddTorque(Vector3.up * num, ForceMode.Force);
		if (_timeInTurbulence >= 0.5f)
		{
			base._pRigidbody.AddForce(_turbulenceDirection * 10f, ForceMode.Force);
		}
		if (Mathf.Abs(base._pRigidbody.transform.localEulerAngles.x) > 32f || Mathf.Abs(base._pRigidbody.transform.localEulerAngles.z) > 32f)
		{
			UpdateTilt();
		}
		if (_accelBrakeInput != 0f && _timeInTurbulence >= 0.5f)
		{
			Vector3 normalized = MathHelper.ClipVector3(base.transform.forward, Vector3.up).normalized;
			base._pRigidbody.AddForce(normalized * _accelBrakeInput * _engineProps._acceleration, ForceMode.Acceleration);
		}
	}

	private void UpdateTilt()
	{
		base._pRigidbody.EaseToAngle(_pTargetPitch, Vector3.right, 0.8f, 0.1f);
		base._pRigidbody.EaseToAngle(_pTargetRoll, Vector3.forward, 0.8f, 0.1f);
	}

	private void ApplySteerAccel()
	{
		if (base._pIsBraking)
		{
			base._pRigidbody.velocity *= _brakeVelMulti;
			return;
		}
		if (_accelBrakeInput != 0f)
		{
			Vector3 normalized = MathHelper.ClipVector3(base.transform.forward, Vector3.up).normalized;
			base._pRigidbody.AddForce(normalized * _accelBrakeInput * _engineProps._acceleration, ForceMode.Acceleration);
		}
		if (_steerInput != 0f)
		{
			base._pRigidbody.AddTorque(Vector3.up * _steerInput * _engineProps._accelerationTurn, ForceMode.Acceleration);
		}
	}

	public override void Reset()
	{
		base.Reset();
		_initY = base.transform.position.y;
	}

	public override void OnPartAttached(VehiclePart part)
	{
		base.OnPartAttached(part);
		if (part.slotType == VehiclePart.EPART_SLOT_TYPE.BODY && part.turbulenceToleranceOverride != -1f)
		{
			_turbulenceTolerance = part.turbulenceToleranceOverride;
		}
		_pEngineProps = part.GetComponent<AirVehicleEngineProperties>() ?? _engineProps;
	}

	public override void OnCoastingRequestsChanged(bool newState)
	{
	}
}
