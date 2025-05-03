using System;
using UnityEngine;
using UnitySampleAssets.Vehicles.Car;

public class Vehicle_Car : Vehicle
{
	[Serializable]
	public class Advanced
	{
		[Range(0f, 1f)]
		public float burnoutSlipEffect = 0.4f;

		[Range(0f, 1f)]
		public float burnoutTendency = 0.2f;

		[Range(0f, 1f)]
		public float spinoutSlipEffect = 0.5f;

		[Range(0f, 1f)]
		public float sideSlideEffect = 0.5f;

		public float downForce = 30f;

		public int numGears = 5;

		[Range(0f, 1f)]
		public float gearDistributionBias = 0.2f;

		public float steeringCorrection = 2f;

		public float oppositeLockSteeringCorrection = 4f;

		public float reversingSpeedFactor = 0.3f;

		public float skidGearLockFactor = 0.1f;

		public float accelChangeSmoothing = 2f;

		public float gearFactorSmoothing = 5f;

		[Range(0f, 1f)]
		public float revRangeBoundary = 0.8f;
	}

	private const float MAX_TORQUE = 70f;

	private const float BRAKE_DECEL_HALFLIFE = 0.2f;

	[SerializeField]
	private float maxSteerAngle = 28f;

	[SerializeField]
	private float steeringResponseSpeed = 200f;

	[Range(0f, 1f)]
	[SerializeField]
	private float maxSpeedSteerAngle = 0.23f;

	[SerializeField]
	[Range(0f, 1f)]
	private float maxSpeedSteerResponse = 0.5f;

	[SerializeField]
	private float maxSpeed = 60f;

	[SerializeField]
	private float brakePower = 40f;

	[SerializeField]
	private float adjustCentreOfMass = 0.25f;

	[SerializeField]
	private float adjustForwardCentreOfMass = 0.25f;

	[SerializeField]
	private Advanced advanced;

	[SerializeField]
	private bool preserveDirectionWhileInAir;

	[SerializeField]
	private bool _addDownwardForceToUngroundedWheels;

	private float[] gearDistribution;

	private Wheel[] wheels = new Wheel[0];

	private float accelBrake;

	private float smallSpeed;

	private float maxReversingSpeed;

	private bool immobilized;

	private float _centreOffsetFromGround = -1f;

	private float _accelBrakeInput;

	private float _steerInput;

	private float _timeAtMaxSteer;

	private float _skidFactorUnscaled;

	private bool anyOnGround;

	private bool isDriveWheelGrounded;

	private bool areAllWheelsGrounded;

	private float curvedSpeedFactor;

	private bool reversing;

	private float targetAccelInput;

	private float _maxTorqueMultiplier;

	private float _brakeVelMulti;

	public int GearNum { get; private set; }

	public float CurrentSpeed { get; private set; }

	public float CurrentSteerAngle { get; private set; }

	public float AccelInput { get; private set; }

	public float BrakeInput { get; private set; }

	public float GearFactor { get; private set; }

	public float AvgPowerWheelRpmFactor { get; private set; }

	public float SkidFactor { get; private set; }

	public float RevsFactor { get; private set; }

	public float SpeedFactor { get; private set; }

	public float SpeedFactor2 { get; private set; }

	public int NumGears
	{
		get
		{
			return advanced.numGears;
		}
	}

	public float MaxSpeed
	{
		get
		{
			return maxSpeed;
		}
		set
		{
			maxSpeed = value;
			smallSpeed = maxSpeed * 0.05f;
			maxReversingSpeed = maxSpeed * advanced.reversingSpeedFactor;
		}
	}

	public float MaxTorque
	{
		get
		{
			return 70f;
		}
	}

	public float BurnoutSlipEffect
	{
		get
		{
			return advanced.burnoutSlipEffect;
		}
	}

	public float BurnoutTendency
	{
		get
		{
			return advanced.burnoutTendency;
		}
	}

	public float SpinoutSlipEffect
	{
		get
		{
			return advanced.spinoutSlipEffect;
		}
	}

	public float SideSlideEffect
	{
		get
		{
			return advanced.sideSlideEffect;
		}
	}

	public float MaxSteerAngle
	{
		get
		{
			return maxSteerAngle;
		}
	}

	public override float _pCentreOffsetFromBottom
	{
		get
		{
			if (_centreOffsetFromGround == -1f)
			{
				Wheel wheel = wheels[0];
				_centreOffsetFromGround = 0f - base.transform.InverseTransformPoint(wheel.transform.TransformPoint(Vector3.zero)).y + wheel.wheelCollider.radius + wheel.wheelCollider.suspensionDistance * (1f - wheel.wheelCollider.suspensionSpring.targetPosition);
			}
			return _centreOffsetFromGround;
		}
	}

	public override bool _pIsGrounded
	{
		get
		{
			return anyOnGround;
		}
	}

	public bool _pIsAnyDriveWheelGrounded
	{
		get
		{
			return isDriveWheelGrounded;
		}
	}

	public bool _pAreAllWheelsGrounded
	{
		get
		{
			return areAllWheelsGrounded;
		}
	}

	public override float _pBounceOffWallAngVelMulti
	{
		get
		{
			return 0.1f;
		}
	}

	public override float _pSpeedFactor
	{
		get
		{
			return SpeedFactor;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		base.gameObject.SetActive(false);
		base.gameObject.SetActive(true);
		if (base._pRigidbody != null)
		{
			base._pRigidbody.centerOfMass = new Vector3(0f, adjustCentreOfMass, adjustForwardCentreOfMass);
		}
		SetUpGears();
		MaxSpeed = maxSpeed;
		_brakeVelMulti = Mathf.Pow(0.5f, Time.fixedDeltaTime / 0.2f);
	}

	protected override void Update()
	{
		base.Update();
		if (base._pIsPlayer)
		{
			UpdateSkidFactor(Time.deltaTime);
		}
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (_addDownwardForceToUngroundedWheels && _pIsGrounded && base._pRigidbody.velocity.y < 4f && Vector3.Angle(base.transform.up, Vector3.up) < 45f)
		{
			for (int num = wheels.Length - 1; num >= 0; num--)
			{
				if (!wheels[num].OnGround)
				{
					base._pRigidbody.AddForceAtPosition(-base.transform.up * 10f, wheels[num].transform.position);
				}
			}
		}
		if (base._pIsBraking && !_pIsGrounded)
		{
			base._pRigidbody.velocity = new Vector3(base._pRigidbody.velocity.x * _brakeVelMulti, base._pRigidbody.velocity.y, base._pRigidbody.velocity.z * _brakeVelMulti);
		}
	}

	public override void Move(float steerInput, float accelBrakeInput)
	{
		_steerInput = steerInput;
		_accelBrakeInput = accelBrakeInput;
		if (_stabiliseInMidAir && !_pAreAllWheelsGrounded && accelBrakeInput > 0f)
		{
			accelBrakeInput = 0f;
		}
		if (immobilized)
		{
			accelBrakeInput = 0f;
		}
		if ((bool)base._pShouldCoast)
		{
			accelBrakeInput = 0f;
		}
		base.Move(steerInput, accelBrakeInput);
		ConvertInputToAccelerationAndBraking(accelBrakeInput);
		CalculateSpeedValues();
		ProcessWheels(steerInput);
		PreserveDirectionInAir();
	}

	private void ConvertInputToAccelerationAndBraking(float accelBrakeInput)
	{
		if (base._pIsBraking)
		{
			float num = (AccelInput = 0f);
			targetAccelInput = num;
			BrakeInput = 1f;
			return;
		}
		reversing = false;
		if (accelBrakeInput > 0f)
		{
			if (CurrentSpeed > 0f - smallSpeed)
			{
				targetAccelInput = accelBrakeInput;
				BrakeInput = 0f;
			}
			else
			{
				BrakeInput = accelBrakeInput;
				targetAccelInput = 0f;
			}
		}
		else if (CurrentSpeed > smallSpeed)
		{
			BrakeInput = 0f - accelBrakeInput;
			targetAccelInput = 0f;
		}
		else
		{
			BrakeInput = 0f;
			targetAccelInput = accelBrakeInput;
			reversing = true;
		}
		AccelInput = Mathf.MoveTowards(AccelInput, targetAccelInput, Time.deltaTime * advanced.accelChangeSmoothing);
	}

	private void CalculateSpeedValues()
	{
		CurrentSpeed = base.transform.InverseTransformDirection(base._pRigidbody.velocity).z;
		SpeedFactor = Mathf.InverseLerp(0f, (!reversing) ? maxSpeed : maxReversingSpeed, Mathf.Abs(CurrentSpeed));
		SpeedFactor2 = Mathf.InverseLerp(0f, (!reversing) ? (maxSpeed * 1.15f) : maxReversingSpeed, Mathf.Abs(CurrentSpeed));
		curvedSpeedFactor = ((!reversing) ? CurveFactor(SpeedFactor) : 0f);
	}

	private void ProcessWheels(float steerInput)
	{
		AvgPowerWheelRpmFactor = 0f;
		float num = 0f;
		anyOnGround = false;
		isDriveWheelGrounded = false;
		areAllWheelsGrounded = true;
		for (int num2 = wheels.Length - 1; num2 >= 0; num2--)
		{
			Wheel wheel = wheels[num2];
			WheelCollider wheelCollider = wheel.wheelCollider;
			if (wheel.steerable)
			{
				float num3 = Mathf.Lerp(steeringResponseSpeed, steeringResponseSpeed * maxSpeedSteerResponse, curvedSpeedFactor);
				float num4 = Mathf.Lerp(maxSteerAngle, maxSteerAngle * maxSpeedSteerAngle, curvedSpeedFactor);
				if (steerInput == 0f)
				{
					num3 *= advanced.steeringCorrection;
				}
				if ((steerInput > 0f && CurrentSteerAngle < 0f) || (steerInput < 0f && CurrentSteerAngle > 0f))
				{
					num3 *= advanced.oppositeLockSteeringCorrection;
				}
				CurrentSteerAngle = Mathf.MoveTowardsAngle(CurrentSteerAngle, steerInput * num4, Time.deltaTime * num3);
				wheelCollider.steerAngle = CurrentSteerAngle;
			}
			if (wheel.powered)
			{
				float num5 = Mathf.Lerp(70f * _maxTorqueMultiplier, 0f, SpeedFactor2);
				wheelCollider.motorTorque = AccelInput * num5;
				AvgPowerWheelRpmFactor += wheel.Rpm / wheel.MaxRpm;
				num += 1f;
				if (wheel.OnGround)
				{
					isDriveWheelGrounded = true;
				}
			}
			wheelCollider.brakeTorque = BrakeInput * brakePower + base._pCoastingBrake;
			if (wheel.OnGround)
			{
				anyOnGround = true;
			}
			else
			{
				areAllWheelsGrounded = false;
			}
		}
		AvgPowerWheelRpmFactor /= num;
	}

	private void ApplyDownforce()
	{
		if (anyOnGround)
		{
			base._pRigidbody.AddForce(Vector3.down * curvedSpeedFactor * advanced.downForce);
		}
	}

	private void PreserveDirectionInAir()
	{
		if (!anyOnGround && preserveDirectionWhileInAir && base._pRigidbody.velocity.magnitude > smallSpeed)
		{
			base._pRigidbody.MoveRotation(Quaternion.Slerp(base._pRigidbody.rotation, Quaternion.LookRotation(base._pRigidbody.velocity), Time.deltaTime));
			base._pRigidbody.angularVelocity = Vector3.Lerp(base._pRigidbody.angularVelocity, Vector3.zero, Time.deltaTime);
		}
	}

	private float CurveFactor(float factor)
	{
		return 1f - (1f - factor) * (1f - factor);
	}

	private float ULerp(float from, float to, float value)
	{
		return (1f - value) * from + value * to;
	}

	private void SetUpGears()
	{
		gearDistribution = new float[advanced.numGears + 1];
		for (int i = 0; i <= advanced.numGears; i++)
		{
			float num = (float)i / (float)advanced.numGears;
			float b = num * num * num;
			float b2 = 1f - (1f - num) * (1f - num) * (1f - num);
			num = ((!(advanced.gearDistributionBias < 0.5f)) ? Mathf.Lerp(num, b2, (advanced.gearDistributionBias - 0.5f) * 2f) : Mathf.Lerp(num, b, 1f - advanced.gearDistributionBias * 2f));
			gearDistribution[i] = num;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(base.transform.position + base.transform.TransformVector(new Vector3(0f, adjustCentreOfMass, adjustForwardCentreOfMass)), 0.2f);
	}

	public void Immobilize()
	{
		immobilized = true;
		Wheel[] array = wheels;
		foreach (Wheel wheel in array)
		{
			wheel.wheelCollider.motorTorque = 0f;
		}
	}

	public override void Reset()
	{
		base.Reset();
		immobilized = false;
		for (int num = wheels.Length - 1; num >= 0; num--)
		{
			wheels[num].SetForceAppPointDistToCarCentreOfMass();
		}
		Brake();
	}

	public void ZeroWheelSuspensionSpringDampers(float restoreAfterSeconds = 0f)
	{
		for (int num = wheels.Length - 1; num >= 0; num--)
		{
			wheels[num].SetSuspensionSpringDamper(0f, restoreAfterSeconds);
		}
	}

	public void RestoreWheelSuspensionSpringDampers()
	{
		for (int num = wheels.Length - 1; num >= 0; num--)
		{
			wheels[num].RestoreSuspensionSpringDamper();
		}
	}

	public void MultiplyWheelsForwardFriction(float friction, float restoreAfterSeconds = 0f)
	{
		for (int num = wheels.Length - 1; num >= 0; num--)
		{
			wheels[num].MultiplyForwardFriction(friction, restoreAfterSeconds);
		}
	}

	public void RestoreWheelsForwardFriction()
	{
		for (int num = wheels.Length - 1; num >= 0; num--)
		{
			wheels[num].RestoreForwardFriction();
		}
	}

	public void MultiplyWheelsSidewaysFriction(float friction, float restoreAfterSeconds = 0f)
	{
		for (int num = wheels.Length - 1; num >= 0; num--)
		{
			wheels[num].MultiplySidewaysFriction(friction, restoreAfterSeconds);
		}
	}

	public void RestoreWheelsSidewaysFriction()
	{
		for (int num = wheels.Length - 1; num >= 0; num--)
		{
			wheels[num].RestoreSidewaysFriction();
		}
	}

	private void UpdateSkidFactor(float dt)
	{
		if (_steerInput == 1f || _steerInput == -1f)
		{
			_timeAtMaxSteer += Time.deltaTime;
		}
		else
		{
			_timeAtMaxSteer = 0f;
		}
		if (!_pIsGrounded)
		{
			_skidFactorUnscaled = 0f;
		}
		else if (_timeAtMaxSteer > 0.5f)
		{
			_skidFactorUnscaled = Mathf.Min(1f, _skidFactorUnscaled + dt * 2f);
		}
		else
		{
			_skidFactorUnscaled = Mathf.Max(0f, _skidFactorUnscaled - dt * 4f);
		}
		SkidFactor = ((!reversing) ? (_skidFactorUnscaled * Mathf.InverseLerp(0.25f, 1f, SpeedFactor)) : 0f);
	}

	public override void BounceAwayFromImpact(Vector3 normal, bool doSpinAway = true)
	{
		ZeroWheelSuspensionSpringDampers(0.1f);
		base.BounceAwayFromImpact(normal, doSpinAway);
	}

	public override void OnPartAttached(VehiclePart part)
	{
		base.OnPartAttached(part);
		if (part.slotType == VehiclePart.EPART_SLOT_TYPE.WHEEL)
		{
			Wheel componentInChildren = part.GetComponentInChildren<Wheel>();
			if (Array.IndexOf(wheels, componentInChildren) == -1)
			{
				wheels = wheels.Push(componentInChildren);
			}
			_maxTorqueMultiplier = Mathf.Pow(componentInChildren.GetComponent<WheelCollider>().radius, -1.335f);
		}
		if (base._pRigidbody != null)
		{
			base._pRigidbody.centerOfMass = new Vector3(0f, adjustCentreOfMass, adjustForwardCentreOfMass);
		}
	}

	public override void OnCoastingRequestsChanged(bool newState)
	{
		base.OnCoastingRequestsChanged(newState);
		for (int num = wheels.Length - 1; num >= 0; num--)
		{
			wheels[num].wheelCollider.wheelDampingRate = ((!newState) ? wheels[num]._pInitWheelDampingRate : Wheel.COASTING_DAMPING_RATE);
		}
	}
}
