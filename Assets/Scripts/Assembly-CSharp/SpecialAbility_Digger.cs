using UnityEngine;

public class SpecialAbility_Digger : SpecialAbility
{
	private const float CHECK_COLLISIONS_INTERVAL = 0.5f;

	private const string DIGGING_STATE_NAME = "IsDigging";

	public float _radius;

	public Animator _animator;

	public Transform _contactPoint;

	public Transform _shockwavePrefab;

	public float _startDelay = 0.3f;

	private float _radiusSqrd;

	private float _lastCollisionCheckTime;

	private int _raycastMask;

	private float _startTime;

	private FastPool _shockwavePool;

	protected override void Awake()
	{
		base.Awake();
		_radiusSqrd = _radius * _radius;
		_raycastMask = LayerMask.GetMask("Geometry");
		if (FastPoolManager.Instance != null)
		{
			_shockwavePool = FastPoolManager.GetPool(_shockwavePrefab);
			_shockwavePool.NotificationType = PoolItemNotificationType.Interface;
			_shockwavePool.ParentOnCache = true;
		}
	}

	protected override void Update()
	{
		base.Update();
		bool flag = base._pIsInUse && Time.time > _startTime + _startDelay && base._pVehicle._pIsGrounded;
		if (flag && Time.time > _lastCollisionCheckTime + 0.5f)
		{
			CheckCollisions();
		}
		MinigameController._pInstance._pCamera._pDiggerEnabled = flag;
	}

	private void CheckCollisions()
	{
		for (int num = MinigameObjective_Destroyable._all.Count - 1; num >= 0; num--)
		{
			MinigameObjective_Destroyable minigameObjective_Destroyable = MinigameObjective_Destroyable._all[num];
			if ((bool)minigameObjective_Destroyable && MathHelper.DistXZSqrd(_contactPoint.position, minigameObjective_Destroyable.transform.position) < _radiusSqrd)
			{
				minigameObjective_Destroyable.TakeDamage(1, minigameObjective_Destroyable.transform.position, Vector3.zero);
			}
		}
		for (int num2 = ReplaceOnCollide._all.Count - 1; num2 >= 0; num2--)
		{
			ReplaceOnCollide replaceOnCollide = ReplaceOnCollide._all[num2];
			if (MathHelper.DistXZSqrd(_contactPoint.position, replaceOnCollide.transform.position) < _radiusSqrd)
			{
				Transform transform = replaceOnCollide.Replace();
				if ((bool)transform)
				{
					Rigidbody component = transform.GetComponent<Rigidbody>();
					if ((bool)component)
					{
						component.AddForceAtPosition(Vector3.up * 5f, _contactPoint.transform.position, ForceMode.VelocityChange);
					}
				}
			}
		}
		Invoke("Shockwave", 0f);
		Invoke("Shockwave", 0.1f);
		Invoke("Shockwave", 0.2f);
		_lastCollisionCheckTime = Time.time;
	}

	private void Shockwave()
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(_contactPoint.position + Vector3.up * 5f, Vector3.down, out hitInfo, 10f, _raycastMask))
		{
			Transform transform = _shockwavePool.FastInstantiate<Transform>();
			transform.localScale = new Vector3(_radius, 1f, _radius);
			transform.rotation = Quaternion.LookRotation(Vector3.forward, hitInfo.normal);
			transform.position = hitInfo.point + Vector3.up * 0.5f;
		}
	}

	protected override void OnStarted()
	{
		base.OnStarted();
		_startTime = Time.time;
		_animator.SetBool("IsDigging", true);
		base._pVehicle.Brake(999f);
	}

	protected override void OnEnded()
	{
		base.OnEnded();
		_animator.SetBool("IsDigging", false);
		base._pVehicle.UnBrake();
	}
}
