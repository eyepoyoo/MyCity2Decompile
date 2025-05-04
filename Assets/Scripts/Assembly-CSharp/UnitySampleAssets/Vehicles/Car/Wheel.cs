using System;
using UnityEngine;

namespace UnitySampleAssets.Vehicles.Car
{
	[RequireComponent(typeof(WheelCollider))]
	public class Wheel : MonoBehaviour
	{
		private const float LO_QUAL_START_DIST = 50f;

		private const float LO_QUAL_START_DIST_SQRD = 2500f;

		private const float ZERO_QUAL_START_DIST = 80f;

		private const float ZERO_QUAL_START_DIST_SQRD = 6400f;

		private const float UPDATE_VISUAL_DIST = 40f;

		private const float UPDATE_VISUAL_DIST_SQRD = 1600f;

		public static float COASTING_DAMPING_RATE = 0.05f;

		public Transform wheelModel;

		public MeshRenderer uvScrollableTrack;

		public Vector2 uvScrollableTrackMask;

		public Transform[] additionalFixedRotatables;

		public Transform skidTrailPrefab;

		public static Transform skidTrailsDetachedParent;

		public bool steerable;

		public bool powered;

		[SerializeField]
		private float particleRate = 3f;

		[SerializeField]
		private float slideThreshold = 10f;

		public bool _hideIfBack;

		public bool _rotatable = true;

		public float _caterpillarTrackLength;

		private float spinAngle;

		private float particleEmit;

		private float sidewaysStiffness;

		private float forwardStiffness;

		private float spinoutFactor;

		private float sideSlideFactor;

		private float springCompression;

		private ParticleSystem skidSmokeSystem;

		private Rigidbody rb;

		private WheelFrictionCurve sidewaysFriction;

		private WheelFrictionCurve forwardFriction;

		private Transform skidTrail;

		private bool leavingSkidTrail;

		private RaycastHit hit;

		private Vector3 relativeVelocity;

		private float sideSlideFactorTarget;

		private float accelAmount;

		private float burnoutFactor;

		private bool ignore;

		private Vector3 originalWheelModelPosition;

		private static int _raycastLayerMask = -1;

		private bool _isPlayer;

		private JointSpring? _suspensionSpringBackup;

		private WheelFrictionCurve? _forwardFrictionBackup;

		private WheelFrictionCurve? _sidewaysFrictionBackup;

		public float Rpm
		{
			get
			{
				return wheelCollider.rpm;
			}
		}

		public float MaxRpm { get; private set; }

		public bool OnGround { get; private set; }

		public Transform Hub { get; set; }

		public WheelCollider wheelCollider { get; private set; }

		public Vehicle_Car car { get; private set; }

		public float _pInitWheelDampingRate { get; private set; }

		public float suspensionSpringPos { get; private set; }

		private bool _pShouldRaycast
		{
			get
			{
				float num = MathHelper.DistXZSqrd(Camera.main.transform.position, base.transform.position);
				if (num < 2500f)
				{
					return true;
				}
				if (!Camera.main.IsPointInFrustrum(base.transform.position, 0f))
				{
					return false;
				}
				return UnityEngine.Random.value < Mathf.InverseLerp(6400f, 2500f, num);
			}
		}

		private void Awake()
		{
			wheelCollider = GetComponent<WheelCollider>();
			_pInitWheelDampingRate = wheelCollider.wheelDampingRate;
			wheelCollider.wheelDampingRate = wheelCollider.wheelDampingRate;
			if (_raycastLayerMask == -1)
			{
				_raycastLayerMask = LayerMask.GetMask("Geometry");
			}
		}

		private void Start()
		{
			car = GetComponentInParent<Vehicle_Car>();
			_isPlayer = VehicleController_Player.IsPlayer(this);
			if (wheelModel != null)
			{
				originalWheelModelPosition = wheelModel.localPosition;
			}
			sidewaysFriction = wheelCollider.sidewaysFriction;
			forwardFriction = wheelCollider.forwardFriction;
			sidewaysStiffness = wheelCollider.sidewaysFriction.stiffness;
			forwardStiffness = wheelCollider.forwardFriction.stiffness;
			MaxRpm = car.MaxSpeed / ((float)Math.PI * wheelCollider.radius * 2f) * 60f;
			rb = wheelCollider.attachedRigidbody;
		}

		private void FixedUpdate()
		{
			if (_isPlayer)
			{
				relativeVelocity = base.transform.InverseTransformDirection(rb.velocity);
				sideSlideFactorTarget = Mathf.Clamp01(Mathf.Abs(relativeVelocity.x * slideThreshold / car.MaxSpeed) * (car.SpeedFactor * 0.5f + 0.5f));
				sideSlideFactor = ((!(sideSlideFactorTarget > sideSlideFactor)) ? Mathf.Lerp(sideSlideFactor, sideSlideFactorTarget, Time.deltaTime) : sideSlideFactorTarget);
				accelAmount = wheelCollider.motorTorque / car.MaxTorque;
				burnoutFactor = ((!powered) ? 0f : (burnoutFactor = (accelAmount - (1f - car.BurnoutTendency)) / (1f - car.BurnoutTendency)));
				DoRaycasts();
			}
		}

		private void Update()
		{
			if (_isPlayer || MathHelper.DistXZSqrd(base.transform.position, Camera.main.transform.position) < 1600f)
			{
				UpdateVisual();
			}
		}

		private void UpdateVisual()
		{
			spinAngle += Rpm * 6f * Time.deltaTime;
			Quaternion quaternion = Quaternion.AngleAxis(wheelCollider.steerAngle, Vector3.up);
			Quaternion quaternion2 = Quaternion.Euler(spinAngle, 0f, 0f);
			if (wheelModel != null)
			{
				wheelModel.localPosition = originalWheelModelPosition + Vector3.up * suspensionSpringPos / wheelModel.parent.lossyScale.y;
				if (_rotatable)
				{
					wheelModel.localRotation = quaternion * quaternion2;
				}
			}
			if (additionalFixedRotatables != null)
			{
				int num = additionalFixedRotatables.Length;
				for (int i = 0; i < num; i++)
				{
					if (!(additionalFixedRotatables[i] == null))
					{
						additionalFixedRotatables[i].localRotation = quaternion2;
					}
				}
			}
			if (uvScrollableTrack != null)
			{
				Material material = uvScrollableTrack.material;
				material.mainTextureOffset = uvScrollableTrackMask * spinAngle;
			}
		}

		private void DoRaycasts()
		{
			if (Physics.Raycast(base.transform.position, -base.transform.up, out hit, wheelCollider.suspensionDistance + wheelCollider.radius, _raycastLayerMask))
			{
				suspensionSpringPos = 0f - (hit.distance - wheelCollider.radius);
				springCompression = Mathf.InverseLerp(0f - wheelCollider.suspensionDistance, wheelCollider.suspensionDistance, suspensionSpringPos);
				OnGround = true;
			}
			else
			{
				suspensionSpringPos = 0f - wheelCollider.suspensionDistance;
				springCompression = 0f;
				OnGround = false;
			}
		}

		public void BackupSuspensionSpringDamper()
		{
			_suspensionSpringBackup = wheelCollider.suspensionSpring;
		}

		public void RestoreSuspensionSpringDamper()
		{
			if (_suspensionSpringBackup.HasValue)
			{
				wheelCollider.suspensionSpring = _suspensionSpringBackup.Value;
			}
		}

		public void SetSuspensionSpringDamper(float to, float restoreAfterSeconds = 0f)
		{
			CancelInvoke("RestoreSuspensionSpringDamper");
			RestoreSuspensionSpringDamper();
			BackupSuspensionSpringDamper();
			JointSpring suspensionSpring = wheelCollider.suspensionSpring;
			suspensionSpring.damper = to;
			wheelCollider.suspensionSpring = suspensionSpring;
			if (restoreAfterSeconds > 0f)
			{
				Invoke("RestoreSuspensionSpringDamper", restoreAfterSeconds);
			}
		}

		public void BackupForwardFriction()
		{
			_forwardFrictionBackup = wheelCollider.forwardFriction;
		}

		public void RestoreForwardFriction()
		{
			if (_forwardFrictionBackup.HasValue)
			{
				wheelCollider.forwardFriction = _forwardFrictionBackup.Value;
			}
		}

		public void MultiplyForwardFriction(float multi, float restoreAfterSeconds = 0f)
		{
			CancelInvoke("RestoreForwardFriction");
			RestoreForwardFriction();
			BackupForwardFriction();
			WheelFrictionCurve wheelFrictionCurve = wheelCollider.forwardFriction;
			wheelFrictionCurve.stiffness *= multi;
			wheelCollider.forwardFriction = wheelFrictionCurve;
			if (restoreAfterSeconds > 0f)
			{
				Invoke("RestoreForwardFriction", restoreAfterSeconds);
			}
		}

		public void BackupSidewaysFriction()
		{
			_sidewaysFrictionBackup = wheelCollider.sidewaysFriction;
		}

		public void RestoreSidewaysFriction()
		{
			if (_sidewaysFrictionBackup.HasValue)
			{
				wheelCollider.sidewaysFriction = _sidewaysFrictionBackup.Value;
			}
		}

		public void MultiplySidewaysFriction(float multi, float restoreAfterSeconds = 0f)
		{
			CancelInvoke("RestoreSidewaysFriction");
			RestoreSidewaysFriction();
			BackupSidewaysFriction();
			WheelFrictionCurve wheelFrictionCurve = wheelCollider.sidewaysFriction;
			wheelFrictionCurve.stiffness *= multi;
			wheelCollider.sidewaysFriction = wheelFrictionCurve;
			if (restoreAfterSeconds > 0f)
			{
				Invoke("RestoreSidewaysFriction", restoreAfterSeconds);
			}
		}

		public void SetLeft()
		{
		}

		public void SetRight()
		{
			base.transform.localPosition = new Vector3(0f - base.transform.localPosition.x, base.transform.localPosition.y, base.transform.localPosition.z);
			wheelModel.localPosition = new Vector3(0f - wheelModel.localPosition.x, wheelModel.localPosition.y, wheelModel.localPosition.z);
			wheelModel.localScale = new Vector3(0f - wheelModel.localScale.x, wheelModel.localScale.y, (0f - wheelModel.localScale.z) * (float)(_rotatable ? 1 : (-1)));
			BoxCollider component = GetComponent<BoxCollider>();
			if ((bool)component)
			{
				component.center = new Vector3(0f - component.center.x, component.center.y, component.center.z);
			}
		}

		public void SetFront()
		{
			powered = false;
			steerable = true;
		}

		public void SetBack()
		{
			powered = true;
			steerable = false;
			if (!_hideIfBack)
			{
				return;
			}
			bool flag = true;
			if (_caterpillarTrackLength > 0f)
			{
				Connectors componentInParent = GetComponentInParent<Connectors>();
				if ((bool)componentInParent && (bool)componentInParent._wheelFL && (bool)componentInParent._wheelBL)
				{
					float num = componentInParent._wheelFL.transform.localPosition.z - componentInParent._wheelBL.transform.localPosition.z;
					flag = num < _caterpillarTrackLength;
				}
			}
			if (flag)
			{
				base.transform.parent.SetMeshesVisible(false);
				BoxCollider component = GetComponent<BoxCollider>();
				if ((bool)component)
				{
					component.enabled = false;
				}
			}
		}

		public void SetFrontLeft()
		{
			SetFront();
			SetLeft();
		}

		public void SetFrontRight()
		{
			SetFront();
			SetRight();
		}

		public void SetBackLeft()
		{
			SetBack();
			SetLeft();
		}

		public void SetBackRight()
		{
			SetBack();
			SetRight();
		}

		public void SetForceAppPointDistToCarCentreOfMass(float offset = -0.1f)
		{
			Vehicle_Car componentInParent = GetComponentInParent<Vehicle_Car>();
			WheelCollider wheelCollider = GetComponent<Collider>() as WheelCollider;
			Vector3 centerOfMass = componentInParent._pRigidbody.centerOfMass;
			centerOfMass = componentInParent.transform.TransformPoint(centerOfMass);
			wheelCollider.forceAppPointDistance = base.transform.InverseTransformPoint(centerOfMass).y + wheelCollider.radius + wheelCollider.suspensionDistance * (1f - wheelCollider.suspensionSpring.targetPosition) + offset;
		}
	}
}
