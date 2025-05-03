using UnityEngine;

public struct TargetInfo
{
	public bool _fullyOnScreen;

	public bool _dummyTarget;

	public ActorBase _actorTarget;

	public float _speed;

	public float _distToTarget;

	public Vector3 _positionFloor;

	public Vector3 _positionTarget;

	public Vector3 _forward;

	public Vector3 _moveDir;

	public Vector3 _dirToTarget;

	public Vector3 _dirToTargetFlat;

	public void Reset()
	{
		_fullyOnScreen = false;
		_dummyTarget = false;
		_actorTarget = null;
		_speed = 0f;
		_distToTarget = 0f;
		_positionFloor = Vector3.zero;
		_positionTarget = Vector3.zero;
		_forward = Vector3.zero;
		_moveDir = Vector3.zero;
		_dirToTarget = Vector3.zero;
		_dirToTargetFlat = Vector3.zero;
	}
}
