using System;
using UnityEngine;

[RequireComponent(typeof(Vehicle))]
public class VehicleController_WaypointFollower : VehicleController
{
	private const float ROAD_WIDTH = 11f;

	private const float LANE_WIDTH = 5.5f;

	private const float HALF_LANE_WIDTH = 2.75f;

	private const float MOVED_TO_SIDE_OFFSET_X = 4f;

	private const float MOVED_TO_SIDE_BREAK_TIMEOUT = 1f;

	public const float LANE_OFFSET_DEFAULT = 2.75f;

	public WaypointPath _path;

	public Waypoint _firstWaypoint;

	public float _wobbleAmount;

	public float _wobbleFreq = 0.8f;

	public bool _isMovedToSide;

	public float _maxAngle = 45f;

	public float _lookAheadDist = 5f;

	public bool _keepNearPath = true;

	public float _maxOffsetFromLine = 8f;

	[NonSerialized]
	public float _laneOffset = 2.75f;

	private OtherVehicleDetector _otherVehicleDetector;

	private Vector3 _closestPointOnLine;

	private Vector3 _thisToClosestPointOnLine;

	private Vector3 _targetPos;

	private Vector3 _thisToTargetPos;

	private float _offsetFromLine;

	private Waypoint _waypointNext;

	private Waypoint _waypointPrev;

	private Vector3 _waypointPrev2Pos;

	private Vector3 _waypointPrevPos;

	private Vector3 _waypointNextPos;

	private Vector3 _lineForward;

	private Vector3 _lineForwardPrev;

	private Vector3 _lineRight;

	private Vector3 _offsetLookAhead;

	private float _movedToSideTimer;

	private float _laneOffsetPrev;

	private float _laneOffsetNext;

	private float _normLineProgress;

	private float _maxOffsetFromLineSqrd;

	private Vector2 pos = default(Vector2);

	private Vector2 a = default(Vector2);

	private Vector2 b = default(Vector2);

	private Vector2 c = default(Vector2);

	private Vector2 ba = default(Vector2);

	private Vector2 bc = default(Vector2);

	private float _pCombinedOffsetRight
	{
		get
		{
			return Mathf.Lerp(_laneOffsetPrev, _laneOffsetNext, _normLineProgress) + _pWobbleDistance + ((!_isMovedToSide) ? 0f : 4f);
		}
	}

	private float _pWobbleDistance
	{
		get
		{
			if (_wobbleAmount == 0f)
			{
				return 0f;
			}
			return Mathf.Sin((float)Math.PI * 2f * Time.time * _wobbleFreq * 0.5f) * Mathf.Sin((float)Math.PI * 2f * Time.time * _wobbleFreq * 0.4321f) * Mathf.Sin((float)Math.PI * 2f * Time.time * _wobbleFreq * 0.321f) * _wobbleAmount;
		}
	}

	public float _pThisToTargetAngleOffset
	{
		get
		{
			return MathHelper.Wrap(Mathf.Atan2(_thisToTargetPos.x, _thisToTargetPos.z) * 57.29578f - base.transform.eulerAngles.y, -180f, 180f);
		}
	}

	public float _pOffsetFromTargetPos
	{
		get
		{
			return Vector3.Dot(_lineRight, -_thisToTargetPos);
		}
	}

	public override bool _pShouldZeroAcceleration
	{
		get
		{
			return base._pShouldZeroAcceleration || !_path || !_waypointNext || _movedToSideTimer > 1f;
		}
	}

	public override bool _pShouldZeroStearing
	{
		get
		{
			return base._pShouldZeroStearing || !_waypointNext;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		if ((bool)_path)
		{
			SetTargetWaypoint(_firstWaypoint ?? GetClosestWaypoint());
		}
		_maxOffsetFromLineSqrd = _maxOffsetFromLine * _maxOffsetFromLine;
	}

	private void Start()
	{
		_otherVehicleDetector = GetComponent<OtherVehicleDetector>();
	}

	private void FixedUpdate()
	{
		_closestPointOnLine = MathHelper.GetClosestPointOnLineSeg(_waypointPrevPos, _waypointNextPos, base.transform.position, out _normLineProgress, false);
		_targetPos = _closestPointOnLine + _lineForward * _lookAheadDist * ((!_isMovedToSide) ? 1f : 0.5f) + _lineRight * _pCombinedOffsetRight;
		_thisToTargetPos = _targetPos - base.transform.position;
		_thisToClosestPointOnLine = _closestPointOnLine - base.transform.position;
		_thisToClosestPointOnLine.y = 0f;
		_offsetFromLine = Vector3.Dot(_lineRight, -_thisToClosestPointOnLine);
		UpdateVehicleInput();
		if ((bool)_waypointNext && base.transform.position.x > _waypointNextPos.x - 10f && base.transform.position.x < _waypointNextPos.x + 10f && base.transform.position.z > _waypointNextPos.z - 10f && base.transform.position.z < _waypointNextPos.z + 10f)
		{
			OnReachedWaypoint();
		}
		if (_isMovedToSide)
		{
			_movedToSideTimer += Time.deltaTime;
		}
		else
		{
			_movedToSideTimer = 0f;
		}
		if (_keepNearPath && (bool)_waypointNext)
		{
			ClampToPath();
		}
	}

	private void UpdateVehicleInput()
	{
		float turnAngle = MathHelper.GetTurnAngle(base.transform, _targetPos);
		float steerInput = Mathf.Clamp(turnAngle, 0f - _maxAngle, _maxAngle) / _maxAngle;
		if (_pShouldZeroStearing)
		{
			steerInput = 0f;
		}
		float accelBrakeInput = 1f;
		if ((bool)_otherVehicleDetector && _otherVehicleDetector._pShouldBreak)
		{
			accelBrakeInput = ((!(Vector3.Dot(base._pVehicle._pRigidbody.velocity, base._pVehicle.transform.forward) > 0f)) ? 0f : (-0.5f));
		}
		if (_pShouldZeroAcceleration)
		{
			accelBrakeInput = 0f;
		}
		base._pVehicle.Move(steerInput, accelBrakeInput);
	}

	private void OnReachedWaypoint()
	{
		SetTargetWaypoint(_path.GetNextWaypoint(_waypointNext, _waypointPrev));
	}

	public void SetTargetWaypoint(Waypoint target, Waypoint prev = null, bool asd = false)
	{
		_waypointPrev2Pos = ((!asd) ? _waypointPrevPos : prev.transform.position);
		_waypointPrev = prev ?? _waypointNext;
		_waypointPrevPos = ((!_waypointPrev) ? base.transform.position : _waypointPrev.transform.position);
		_waypointNext = target;
		_waypointNextPos = _waypointNext.transform.position;
		if ((bool)target)
		{
			_laneOffsetPrev = ((!_waypointPrev) ? 0f : ((!_waypointPrev._includeLaneOffset) ? 0f : _laneOffset));
			_laneOffsetNext = ((!_waypointNext._includeLaneOffset) ? 0f : _laneOffset);
			_lineForward = (_waypointNextPos - _waypointPrevPos).normalized;
			_lineForwardPrev = (_waypointPrevPos - _waypointPrev2Pos).normalized;
			_lineRight = new Vector3(_lineForward.z, 0f, 0f - _lineForward.x);
		}
	}

	private void OnDrawGizmos()
	{
		if (_closestPointOnLine != Vector3.zero && _targetPos != Vector3.zero)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(base.transform.position + Vector3.up * 0.1f, _closestPointOnLine + Vector3.up * 0.1f);
			Gizmos.DrawSphere(_closestPointOnLine, 0.5f);
			Gizmos.color = Color.green;
			Gizmos.DrawLine(base.transform.position + Vector3.up * 0.1f, _targetPos + Vector3.up * 0.1f);
			Gizmos.DrawSphere(_targetPos, 0.5f);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(_waypointPrev2Pos + Vector3.up * 10f, 2f);
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(_waypointPrevPos + Vector3.up * 10f, 2f);
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(_waypointNextPos + Vector3.up * 10f, 2f);
		Vector2 targ = new Vector2(0f - _lineForwardPrev.x, 0f - _lineForwardPrev.z);
		Vector2 targ2 = new Vector2(_lineForward.x, _lineForward.z);
		Vector2 normalized = (targ.RotateRight90() + targ2.RotateLeft90()).normalized;
		Gizmos.DrawLine(_waypointPrevPos, _waypointPrevPos + new Vector3(normalized.x, 0f, normalized.y) * 10f);
	}

	public Waypoint GetClosestWaypoint()
	{
		return (!_path) ? null : _path.GetClosestWaypoint(base.transform.position);
	}

	public void ClampToPath()
	{
		pos.Set(base.transform.position.x, base.transform.position.z);
		a.Set(_waypointPrev2Pos.x, _waypointPrev2Pos.z);
		b.Set(_waypointPrevPos.x, _waypointPrevPos.z);
		c.Set(_waypointNextPos.x, _waypointNextPos.z);
		ba.Set(0f - _lineForwardPrev.x, 0f - _lineForwardPrev.z);
		bc.Set(_lineForward.x, _lineForward.z);
		bool flag;
		if (a == b)
		{
			flag = false;
		}
		else
		{
			Vector2 normalized = (ba.RotateRight90() + bc.RotateLeft90()).normalized;
			flag = Vector2.Dot(normalized.RotateRight90(), pos - b) < 0f;
		}
		Vector2 vector = ((!flag) ? bc : ba);
		Vector2 vector2 = vector.RotateRight90();
		Vector2 vector3 = b;
		Vector2 vector4 = ((!flag) ? c : a);
		Vector2 vector5 = pos - vector3;
		if (Vector2.Dot(vector5, vector) < 0f)
		{
			if (vector5.sqrMagnitude > _maxOffsetFromLineSqrd)
			{
				base.transform.position = new Vector3(vector3.x + Vector2.ClampMagnitude(vector5, _maxOffsetFromLine).x, base.transform.position.y, vector3.y + Vector2.ClampMagnitude(vector5, _maxOffsetFromLine).y);
				Vector2 vector6 = MathHelper.ClipVector2(base._pVehicle._pRigidbody.velocity, vector5.normalized);
				base._pVehicle._pRigidbody.velocity = new Vector3(vector6.x, base._pVehicle._pRigidbody.velocity.y, vector6.y);
			}
		}
		else if (Vector2.Dot(vector5, vector2) > _maxOffsetFromLine)
		{
			Vector2 vector7 = vector3 + vector * Vector2.Dot(vector5, vector);
			base.transform.position = new Vector3(vector7.x + vector2.x * _maxOffsetFromLine, base.transform.position.y, vector7.y + vector2.y * _maxOffsetFromLine);
			Vector2 vector8 = MathHelper.ClipVector2(base._pVehicle._pRigidbody.velocity, vector2.normalized);
			base._pVehicle._pRigidbody.velocity = new Vector3(vector8.x, base._pVehicle._pRigidbody.velocity.y, vector8.y);
		}
		else if (Vector3.Dot(vector5, -vector2) > _maxOffsetFromLine)
		{
			Vector2 vector9 = vector3 + vector * Vector2.Dot(vector5, vector);
			base.transform.position = new Vector3(vector9.x - vector2.x * _maxOffsetFromLine, base.transform.position.y, vector9.y - vector2.y * _maxOffsetFromLine);
			Vector2 vector10 = MathHelper.ClipVector2(base._pVehicle._pRigidbody.velocity, vector2.normalized);
			base._pVehicle._pRigidbody.velocity = new Vector3(vector10.x, base._pVehicle._pRigidbody.velocity.y, vector10.y);
		}
	}
}
