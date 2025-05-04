using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Vehicle_Car))]
public class SelfRighter : MonoBehaviour
{
	private const float RIGHTING_FORCE_ANGLE_THRESHOLD = 45f;

	private const float RIGHTING_FORCE = 4f;

	private const float RESET_DELAY = 3f;

	private const float RESET_THRESH_VEL = 2f;

	private const float RESET_THRESH_VEL_SQRD = 4f;

	private const float RESET_THRESH_ANGLE = 15f;

	private const float UPDATE_TIMER_INTERVAL = 1f;

	private float _resetTimer;

	private Rigidbody _rigidbody;

	private Vehicle_Car _car;

	private int _layerMask = int.MaxValue;

	private float _angleFromUp;

	private readonly RepeatedAction _repeatedAction_UpdateTimer = new RepeatedAction(1f);

	private bool _isPlayer;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_car = GetComponent<Vehicle_Car>();
		_layerMask = LayerMask.GetMask("Geometry");
		_repeatedAction_UpdateTimer.RandomiseTimer();
	}

	private void Start()
	{
		_isPlayer = VehicleController_Player.IsPlayer(this);
	}

	private void FixedUpdate()
	{
		if (_repeatedAction_UpdateTimer.Update())
		{
			UpdateTimer();
		}
	}

	private void UpdateTimer()
	{
		if (_car.enabled)
		{
			_angleFromUp = Vector3.Angle(Vector3.up, base.transform.up);
			_resetTimer += 1f;
			if (_car._pAreAllWheelsGrounded || _rigidbody.velocity.sqrMagnitude > 4f || (_angleFromUp < 15f && (!_isPlayer || _car._pIsAnyDriveWheelGrounded)))
			{
				_resetTimer = 0f;
			}
			if (_resetTimer > 3f)
			{
				_resetTimer = 0f;
				Reset();
			}
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		if (base.enabled && ((1 << collision.gameObject.layer) & _layerMask) != 0 && _angleFromUp > 45f)
		{
			AddRightingForce();
		}
	}

	private void Reset()
	{
		base.transform.position += Vector3.up * 2f;
		float num = Vector3.Dot(Vector3.up, base.transform.forward);
		Vector3 vector = Vector3.up * num;
		base.transform.rotation = Quaternion.LookRotation(base.transform.forward - vector);
		base.transform.position -= base.transform.forward;
		_rigidbody.velocity = Vector3.zero;
		_rigidbody.angularVelocity = Vector3.zero;
	}

	private void AddRightingForce()
	{
		Vector3 up = base.transform.up;
		if (_angleFromUp > 90f)
		{
			float num = MathHelper.Wrap(base.transform.eulerAngles.z, 180f);
			num = ((!(num < 90f)) ? (180f - num) : num);
			if (num < 10f)
			{
				up += base.transform.right;
			}
			up.y = 0f;
			up.Normalize();
		}
		_rigidbody.AddForceAtPosition(Vector3.up * 4f, base.transform.position + up * 5f, ForceMode.Acceleration);
	}
}
