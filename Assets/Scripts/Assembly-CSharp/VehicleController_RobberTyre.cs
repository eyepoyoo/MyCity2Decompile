using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VehicleController_RobberTyre : VehicleController
{
	private const float STROKE_FORCE_DURATION = 0.6f;

	private const float STROKE_TORQUE_TURN = 2f;

	private const float STROKE_TORQUE_WOBBLE = 5f;

	private const float STROKE_FORCE_FORWARD = 25f;

	private const float STROKE_FORCE_UP = 3f;

	public Animator _animToTrack;

	private Rigidbody _rigidbody;

	private Vector3 _strokeTorqueTurn;

	private Vector3 _strokeTorqueWobble;

	private Vector3 _strokeForceLinear;

	private float _strokeTimer;

	private float _strokeTimerPrev;

	private Animator[] _allAnims;

	private bool _animsEnabled;

	private bool _pAnimsEnabled
	{
		get
		{
			return _animsEnabled;
		}
		set
		{
			if (value != _animsEnabled)
			{
				_animsEnabled = value;
				for (int num = _allAnims.Length - 1; num >= 0; num--)
				{
					_allAnims[num].speed = (_animsEnabled ? 1 : 0);
				}
			}
		}
	}

	protected override void Awake()
	{
		base.Awake();
		_rigidbody = base.gameObject.GetComponent<Rigidbody>();
		RandomiseStrokeForces();
		_allAnims = GetComponentsInChildren<Animator>();
	}

	private void FixedUpdate()
	{
		_pAnimsEnabled = _enabled && !base._pVehicle._pIsBraking && !base._pVehicle._isInSpotlight;
		if (!_enabled)
		{
			return;
		}
		if (base._pVehicle._isInSpotlight)
		{
			Vector3 vector = VehicleController_Player._pInstance.transform.position - base.transform.position;
			float num = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
			float num2 = -2f * MathHelper.Wrap(base.transform.eulerAngles.y - num, -180f, 180f);
			float num3 = -0.9f * (Vector3.Dot(Vector3.up, _rigidbody.angularVelocity) + num2);
			_rigidbody.AddTorque(Vector3.up * (num2 + num3), ForceMode.Acceleration);
		}
		else if (!base._pVehicle._pIsBraking)
		{
			_strokeTimer = _animToTrack.GetCurrentAnimatorStateInfo(0).normalizedTime * 2f % 1f;
			if (_strokeTimer < _strokeTimerPrev)
			{
				RandomiseStrokeForces();
			}
			if (_strokeTimer > 0.6f)
			{
				ApplyStrokeForces();
			}
			_strokeTimerPrev = _strokeTimer;
		}
	}

	private void RandomiseStrokeForces()
	{
		_strokeTorqueTurn = Vector3.up * Random.Range(-2f, 2f);
		_strokeTorqueWobble = new Vector3(MathHelper._pPlusOrMinusOne, 0f, MathHelper._pPlusOrMinusOne) * 5f;
		_strokeForceLinear = base.transform.forward * 25f + Vector3.up * Random.Range(1f, 2f) * 3f;
	}

	private void ApplyStrokeForces()
	{
		_rigidbody.AddTorque(_strokeTorqueTurn, ForceMode.Acceleration);
		_rigidbody.AddRelativeTorque(_strokeTorqueWobble, ForceMode.Acceleration);
		_rigidbody.AddForce(_strokeForceLinear, ForceMode.Acceleration);
	}
}
