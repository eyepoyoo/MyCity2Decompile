using UnityEngine;

namespace UnitySampleAssets.Vehicles.Aeroplane
{
	[RequireComponent(typeof(Rigidbody))]
	public class AeroplaneController : MonoBehaviour
	{
		[SerializeField]
		private float maxEnginePower = 40f;

		[SerializeField]
		private float lift = 0.002f;

		[SerializeField]
		private float zeroLiftSpeed = 300f;

		[SerializeField]
		private float rollEffect = 1f;

		[SerializeField]
		private float pitchEffect = 1f;

		[SerializeField]
		private float yawEffect = 0.2f;

		[SerializeField]
		private float bankedTurnEffect = 0.5f;

		[SerializeField]
		private float aerodynamicEffect = 0.02f;

		[SerializeField]
		private float autoTurnPitch = 0.5f;

		[SerializeField]
		private float autoRollLevel = 0.2f;

		[SerializeField]
		private float autoPitchLevel = 0.2f;

		[SerializeField]
		private float airBrakesEffect = 3f;

		[SerializeField]
		private float throttleChangeSpeed = 0.3f;

		[SerializeField]
		private float dragIncreaseFactor = 0.001f;

		private float originalDrag;

		private float originalAngularDrag;

		private float aeroFactor;

		private bool immobilized;

		private float bankedTurnAmount;

		public float Altitude { get; private set; }

		public float Throttle { get; private set; }

		public bool AirBrakes { get; private set; }

		public float ForwardSpeed { get; private set; }

		public float EnginePower { get; private set; }

		public float MaxEnginePower
		{
			get
			{
				return maxEnginePower;
			}
		}

		public float RollAngle { get; private set; }

		public float PitchAngle { get; private set; }

		public float RollInput { get; private set; }

		public float PitchInput { get; private set; }

		public float YawInput { get; private set; }

		public float ThrottleInput { get; private set; }

		private void Start()
		{
			originalDrag = GetComponent<Rigidbody>().drag;
			originalAngularDrag = GetComponent<Rigidbody>().angularDrag;
		}

		public void Move(float rollInput, float pitchInput, float yawInput, float throttleInput, bool airBrakes)
		{
			RollInput = rollInput;
			PitchInput = pitchInput;
			YawInput = yawInput;
			ThrottleInput = throttleInput;
			AirBrakes = airBrakes;
			ClampInputs();
			CalculateRollAndPitchAngles();
			AutoLevel();
			CalculateForwardSpeed();
			ControlThrottle();
			CalculateDrag();
			CaluclateAerodynamicEffect();
			CalculateLinearForces();
			CalculateTorque();
			CalculateAltitude();
		}

		private void ClampInputs()
		{
			RollInput = Mathf.Clamp(RollInput, -1f, 1f);
			PitchInput = Mathf.Clamp(PitchInput, -1f, 1f);
			YawInput = Mathf.Clamp(YawInput, -1f, 1f);
			ThrottleInput = Mathf.Clamp(ThrottleInput, -1f, 1f);
		}

		private void CalculateRollAndPitchAngles()
		{
			Vector3 forward = base.transform.forward;
			forward.y = 0f;
			if (forward.sqrMagnitude > 0f)
			{
				forward.Normalize();
				Vector3 vector = base.transform.InverseTransformDirection(forward);
				PitchAngle = Mathf.Atan2(vector.y, vector.z);
				Vector3 direction = Vector3.Cross(Vector3.up, forward);
				Vector3 vector2 = base.transform.InverseTransformDirection(direction);
				RollAngle = Mathf.Atan2(vector2.y, vector2.x);
			}
		}

		private void AutoLevel()
		{
			bankedTurnAmount = Mathf.Sin(RollAngle);
			if (RollInput == 0f)
			{
				RollInput = (0f - RollAngle) * autoRollLevel;
			}
			if (PitchInput == 0f)
			{
				PitchInput = (0f - PitchAngle) * autoPitchLevel;
				PitchInput -= Mathf.Abs(bankedTurnAmount * bankedTurnAmount * autoTurnPitch);
			}
		}

		private void CalculateForwardSpeed()
		{
			ForwardSpeed = Mathf.Max(0f, base.transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity).z);
		}

		private void ControlThrottle()
		{
			if (immobilized)
			{
				ThrottleInput = -0.5f;
			}
			Throttle = Mathf.Clamp01(Throttle + ThrottleInput * Time.deltaTime * throttleChangeSpeed);
			EnginePower = Throttle * maxEnginePower;
		}

		private void CalculateDrag()
		{
			float num = GetComponent<Rigidbody>().velocity.magnitude * dragIncreaseFactor;
			GetComponent<Rigidbody>().drag = ((!AirBrakes) ? (originalDrag + num) : ((originalDrag + num) * airBrakesEffect));
			GetComponent<Rigidbody>().angularDrag = originalAngularDrag * ForwardSpeed;
		}

		private void CaluclateAerodynamicEffect()
		{
			if (GetComponent<Rigidbody>().velocity.magnitude > 0f)
			{
				aeroFactor = Vector3.Dot(base.transform.forward, GetComponent<Rigidbody>().velocity.normalized);
				aeroFactor *= aeroFactor;
				Vector3 velocity = Vector3.Lerp(GetComponent<Rigidbody>().velocity, base.transform.forward * ForwardSpeed, aeroFactor * ForwardSpeed * aerodynamicEffect * Time.deltaTime);
				GetComponent<Rigidbody>().velocity = velocity;
				GetComponent<Rigidbody>().rotation = Quaternion.Slerp(GetComponent<Rigidbody>().rotation, Quaternion.LookRotation(GetComponent<Rigidbody>().velocity, base.transform.up), aerodynamicEffect * Time.deltaTime);
			}
		}

		private void CalculateLinearForces()
		{
			Vector3 zero = Vector3.zero;
			zero += EnginePower * base.transform.forward;
			Vector3 normalized = Vector3.Cross(GetComponent<Rigidbody>().velocity, base.transform.right).normalized;
			float num = Mathf.InverseLerp(zeroLiftSpeed, 0f, ForwardSpeed);
			float num2 = ForwardSpeed * ForwardSpeed * lift * num * aeroFactor;
			zero += num2 * normalized;
			GetComponent<Rigidbody>().AddForce(zero);
		}

		private void CalculateTorque()
		{
			Vector3 zero = Vector3.zero;
			zero += PitchInput * pitchEffect * base.transform.right;
			zero += YawInput * yawEffect * base.transform.up;
			zero += (0f - RollInput) * rollEffect * base.transform.forward;
			zero += bankedTurnAmount * bankedTurnEffect * base.transform.up;
			GetComponent<Rigidbody>().AddTorque(zero * ForwardSpeed * aeroFactor);
		}

		private void CalculateAltitude()
		{
			Ray ray = new Ray(base.transform.position - Vector3.up * 10f, -Vector3.up);
			RaycastHit hitInfo;
			Altitude = ((!Physics.Raycast(ray, out hitInfo)) ? base.transform.position.y : (hitInfo.distance + 10f));
		}

		public void Immobilize()
		{
			immobilized = true;
		}

		public void Reset()
		{
			immobilized = false;
		}
	}
}
