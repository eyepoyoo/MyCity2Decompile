using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Vehicle_Car))]
public class MinigameObjective_Destroyable_Car : MinigameObjective_Destroyable
{
	private const float BUMP_SPEED = 20f;

	private const float SKID_TIME = 0.5f;

	private const float MIN_FORCE_1HP = 0f;

	private const float MIN_FORCE_2HP = 300f;

	private const float MIN_FORCE_3HP = 600f;

	private const float TAUNT_INTERVAL = 10f;

	public Transform facePos;

	private Vehicle_Car _car;

	private float _tauntTimer;

	protected override float _pInvulnDuration
	{
		get
		{
			return 1f;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		_car = GetComponent<Vehicle_Car>();
	}

	private void Update()
	{
		if (MinigameController._pInstance._pStage != MinigameController.EStage.Waiting)
		{
			_tauntTimer += Time.deltaTime;
		}
		if (_tauntTimer > 10f)
		{
			if (!MinigameController._pInstance._pCamera._pCamera.IsPointInFrustrum(base.transform.position, 0f))
			{
				_tauntTimer = 10f;
				return;
			}
			EmoticonSystem.OnCrookTaunt(facePos);
			_tauntTimer = 0f;
		}
	}

	protected override void OnHitByPlayer(Collision collision)
	{
		float magnitude = (collision.impulse / Time.fixedDeltaTime).magnitude;
		float num = collision.rigidbody.mass / base._pRigidbody.mass;
		float num2 = magnitude * num;
		int hitpointsToTake = ((num2 >= 600f) ? 3 : ((num2 > 300f) ? 2 : ((num2 > 0f) ? 1 : 0)));
		TakeDamage(hitpointsToTake, collision);
		_car.MultiplyWheelsForwardFriction(0f, 0.5f);
		_car.MultiplyWheelsSidewaysFriction(0f, 0.5f);
		base._pRigidbody.AddForceAtPosition(GetBoostVector(base._pRigidbody), GetBoostOrigin(base._pRigidbody), ForceMode.Impulse);
		collision.rigidbody.AddForceAtPosition(GetBoostVector(collision.rigidbody), GetBoostOrigin(collision.rigidbody), ForceMode.Impulse);
		EmoticonSystem.OnPlayerHitCrook(facePos);
		_tauntTimer = 0f;
	}

	private Vector3 GetBoostVector(Rigidbody rigidbody)
	{
		return ((!(rigidbody.transform == base.transform)) ? (-rigidbody.transform.forward) : base.transform.forward) * 20f;
	}

	private Vector3 GetBoostOrigin(Rigidbody rigidbody)
	{
		return rigidbody.worldCenterOfMass;
	}

	protected override void Kill()
	{
		_car.Immobilize();
		_car.enabled = false;
		_car._pController._enabled = false;
		base.Kill();
	}

	public void ResetTauntTimer()
	{
		_tauntTimer = 0f;
	}
}
