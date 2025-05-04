using UnityEngine;

public class PlayerChaser : MonoBehaviour
{
	public float _moveDist = 30f;

	public float _sleepDist = 53.6f;

	public float _moveSpeed = 5f;

	public float _easeToPlayerSpeed = 1f;

	public float _easeAwayFromPlayerSpeed = 1f;

	public float _stopMovingOnSnareDur = 2f;

	public GameObject[] objectsToDisableOnSleep;

	private Vector3 _rootPos;

	private Vector3 _targetPos;

	private float _easeToPlayer;

	private float _ignoreMoveTime;

	private float _prepareIgnoreMoveTime;

	private bool _sleepingObjectsActive = true;

	private void Awake()
	{
		_rootPos = base.transform.position;
		_targetPos = _rootPos;
		SetSleepingObjectsActive(false);
	}

	private void SetSleepingObjectsActive(bool active)
	{
		if (_sleepingObjectsActive == active || objectsToDisableOnSleep == null)
		{
			return;
		}
		_sleepingObjectsActive = active;
		int num = objectsToDisableOnSleep.Length;
		for (int i = 0; i < num; i++)
		{
			if (!(objectsToDisableOnSleep[i] == null))
			{
				objectsToDisableOnSleep[i].SetActive(active);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (VehicleController_Player.IsPlayer(other.transform))
		{
			_ignoreMoveTime = 0f;
			_prepareIgnoreMoveTime = 0.25f;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (VehicleController_Player.IsPlayer(other.transform))
		{
			_ignoreMoveTime = 0f;
			_prepareIgnoreMoveTime = 0f;
		}
	}

	private void Update()
	{
		Vector3 position = VehicleController_Player._pInstance.transform.position;
		position.y = _rootPos.y;
		float sqrMagnitude = (position - _rootPos).sqrMagnitude;
		if (sqrMagnitude >= _sleepDist * _sleepDist)
		{
			SetSleepingObjectsActive(false);
			return;
		}
		SetSleepingObjectsActive(true);
		if (_prepareIgnoreMoveTime > 0f)
		{
			_prepareIgnoreMoveTime -= Time.deltaTime;
			if (_prepareIgnoreMoveTime <= 0f)
			{
				_ignoreMoveTime = _stopMovingOnSnareDur;
			}
		}
		if (_ignoreMoveTime > 0f)
		{
			_ignoreMoveTime -= Time.deltaTime;
			return;
		}
		float sqrMagnitude2 = (base.transform.position - _targetPos).sqrMagnitude;
		float num = 1f;
		if (sqrMagnitude < _moveDist * _moveDist)
		{
			_easeToPlayer = Mathf.MoveTowards(_easeToPlayer, 1f, Time.deltaTime * _easeToPlayerSpeed);
			_targetPos = Vector3.Lerp(_targetPos, position, _easeToPlayer);
			num = 2f;
		}
		else if (sqrMagnitude2 < 0.5f)
		{
			_easeToPlayer = Mathf.MoveTowards(_easeToPlayer, 0f, Time.deltaTime * _easeAwayFromPlayerSpeed);
			_targetPos = _rootPos + Random.onUnitSphere * _moveDist;
			_targetPos.y = _rootPos.y;
		}
		base.transform.position = Vector3.MoveTowards(base.transform.position, _targetPos, Time.deltaTime * _moveSpeed * num);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		if (!Application.isPlaying)
		{
			GizmosPlus.drawCircle(base.transform.position, _moveDist);
			Gizmos.color = Color.black;
			GizmosPlus.drawCircle(base.transform.position, _sleepDist);
		}
		else
		{
			GizmosPlus.drawCircle(_rootPos, _moveDist);
			Gizmos.DrawSphere(_targetPos, 0.5f);
			Gizmos.color = Color.black;
			GizmosPlus.drawCircle(_rootPos, _sleepDist);
		}
	}
}
