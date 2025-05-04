using UnityEngine;

public class MinigameObjective_Destroyable_LavaRock : MinigameObjective_Destroyable
{
	private const float MOVE_CRYSTAL_TO_PLAYER_DELAY = 1f;

	private const float MOVE_CRYSTAL_TO_PLAYER_DURATION = 0.4f;

	public Transform _shellL;

	public Transform _shellR;

	public Transform _crystal;

	private bool _doMoveCrystalToPlayer;

	private float _moveCrystalToPlayerStartTime;

	private Vector3 _moveCrystalToPlayerStartPos;

	private Rigidbody _rigidbody;

	private Collider _collider;

	protected override float _pInvulnDuration
	{
		get
		{
			return 0.4f;
		}
	}

	protected override int _pHpToDebrisMulti
	{
		get
		{
			return 5;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		_rigidbody = GetComponent<Rigidbody>();
		_collider = GetComponent<Collider>();
	}

	private void Update()
	{
		if (_doMoveCrystalToPlayer)
		{
			float num = Mathf.Clamp01(Easing.Ease(Easing.EaseType.EaseIn, Time.time - _moveCrystalToPlayerStartTime, 0.4f, 0f, 1f));
			_crystal.position = Vector3.Lerp(_moveCrystalToPlayerStartPos, VehicleController_Player._pInstance.transform.position, num);
			if (num == 1f)
			{
				_crystal.gameObject.SetActive(false);
				_doMoveCrystalToPlayer = false;
			}
		}
	}

	protected override void Kill()
	{
		_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		_collider.enabled = false;
		Vector3 vector = Quaternion.AngleAxis(base.transform.eulerAngles.y, Vector3.up) * new Vector3(-5f, 10f);
		_shellL.parent = null;
		_shellL.GetComponent<Collider>().enabled = true;
		Rigidbody rigidbody = _shellL.gameObject.AddComponent<Rigidbody>();
		rigidbody.velocity = vector;
		rigidbody.angularVelocity = Random.onUnitSphere * 10f;
		_shellR.parent = null;
		_shellR.GetComponent<Collider>().enabled = true;
		rigidbody = _shellR.gameObject.AddComponent<Rigidbody>();
		vector = Quaternion.AngleAxis(180f, Vector3.up) * vector;
		rigidbody.velocity = vector;
		rigidbody.angularVelocity = Random.onUnitSphere * 10f;
		_crystal.parent = null;
		rigidbody = _crystal.gameObject.AddComponent<Rigidbody>();
		rigidbody.constraints = (RigidbodyConstraints)10;
		rigidbody.velocity = Vector3.up * 16f;
		rigidbody.angularVelocity = Random.onUnitSphere * 10f;
		Invoke("MoveCrystalToPlayer", 1f * Time.timeScale);
		base.Kill();
	}

	public override void Regenerate()
	{
		base.Regenerate();
		_rigidbody.constraints = RigidbodyConstraints.None;
		_collider.enabled = true;
		_shellL.parent = base.transform;
		_shellL.localPosition = Vector3.zero;
		_shellL.localRotation = Quaternion.identity;
		_shellL.GetComponent<Collider>().enabled = false;
		Object.Destroy(_shellL.GetComponent<Rigidbody>());
		_shellR.parent = base.transform;
		_shellR.localPosition = Vector3.zero;
		_shellR.localRotation = Quaternion.identity;
		_shellR.GetComponent<Collider>().enabled = false;
		Object.Destroy(_shellR.GetComponent<Rigidbody>());
		_crystal.gameObject.SetActive(true);
		_crystal.parent = base.transform;
		_crystal.localPosition = Vector3.zero;
		_crystal.localRotation = Quaternion.identity;
		Object.Destroy(_crystal.GetComponent<Rigidbody>());
		_doMoveCrystalToPlayer = false;
		CancelInvoke("MoveCrystalToPlayer");
		_doMoveCrystalToPlayer = false;
	}

	private void MoveCrystalToPlayer()
	{
		_doMoveCrystalToPlayer = true;
		_moveCrystalToPlayerStartTime = Time.time;
		_moveCrystalToPlayerStartPos = _crystal.position;
	}

	public override void Reset(bool toInitialState = false)
	{
		base.Reset(toInitialState);
		if (toInitialState)
		{
			Regenerate();
		}
	}
}
