using UnityEngine;

[RequireComponent(typeof(Buoyancy))]
public class Vehicle_Boat : Vehicle
{
	private const float ROLL_FACTOR_SPEED = 2f;

	private const float BRAKE_DECEL_HALFLIFE = 0.2f;

	public ParticleSystem _landInWaterSplash;

	public float _motorAccel;

	public float _turnAccel;

	public float _lateralResistanceMulti;

	public float _rollSteerFactor;

	public float _pitchSpeedFactor;

	private float _steerInput;

	private float _accelBrakeInput;

	private float _initDrag;

	private float _forwardSpeed;

	private float _lateralSpeed;

	private float _steerInputEased;

	private float _brakeVelMulti;

	private float _speedFactor;

	public Buoyancy _pBuoyancy { get; private set; }

	public float _pForwardSpeed
	{
		get
		{
			return _forwardSpeed;
		}
	}

	public override bool _pIsGrounded
	{
		get
		{
			return (bool)_pBuoyancy && _pBuoyancy._pSubmersion > 0f;
		}
	}

	public override float _pBounceOffWallAngVelMulti
	{
		get
		{
			return 0.07f;
		}
	}

	public override float _pCentreOffsetFromBottom
	{
		get
		{
			return _pBuoyancy._centreBottomDist;
		}
	}

	public override float _pSpeedFactor
	{
		get
		{
			return _speedFactor;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		_pBuoyancy = GetComponent<Buoyancy>();
		_initDrag = base._pRigidbody.drag;
		_brakeVelMulti = Mathf.Pow(0.5f, Time.fixedDeltaTime / 0.2f);
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		base._pRigidbody.drag = ((!_pIsGrounded) ? 0f : _initDrag);
		_forwardSpeed = Vector3.Dot(base._pRigidbody.velocity, base.transform.forward);
		_lateralSpeed = Mathf.Abs(Vector3.Dot(base._pRigidbody.velocity, base.transform.right));
		_speedFactor = Mathf.InverseLerp(0f, 10f, Mathf.Abs(_forwardSpeed));
		if (_pIsGrounded)
		{
			ApplySteerAccel();
		}
		UpdateTilt(Time.fixedDeltaTime);
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

	private void UpdateTilt(float dt)
	{
		_steerInputEased = Mathf.MoveTowards(_steerInputEased, _steerInput, 2f * dt);
		_pBuoyancy._targetRoll = _rollSteerFactor * (0f - _steerInputEased);
		_pBuoyancy._targetPitch = _pitchSpeedFactor * Mathf.Min(0f, 0f - _forwardSpeed);
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
			base._pRigidbody.AddForce(normalized * _accelBrakeInput * _motorAccel, ForceMode.Acceleration);
		}
		if (_steerInput != 0f)
		{
			base._pRigidbody.AddTorque(Vector3.up * _steerInput * _turnAccel * Mathf.Sign(_forwardSpeed), ForceMode.Acceleration);
		}
		base._pRigidbody.AddForce(base._pRigidbody.velocity * (0f - _lateralSpeed) * _lateralResistanceMulti, ForceMode.Acceleration);
	}

	public void DisplaySplash()
	{
		if (_landInWaterSplash != null)
		{
			_landInWaterSplash.Play();
		}
	}

	public override void OnCoastingRequestsChanged(bool newState)
	{
	}
}
