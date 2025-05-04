using UnityEngine;

public class BehaveMoveToTarget : BehaveBase
{
	private enum STATE
	{
		MTT_UNSET = 0,
		MTT_STANDING = 1,
		MTT_MOVING_TOWARDS = 2,
		NUM_MTT_STATES = 3
	}

	private const float _cTimeBeforePathRePlan = 1.5f;

	private const float _cRangeDefault = 10f;

	private const float _cTargetMovementToRePathPlan = 50f;

	private STATE _stateCurrent;

	private float _timerBehaviour;

	private float _timerState;

	private float _rangeToAimFor = 10f;

	private float _replanDistSqrd = 2500f;

	private Vector3 _posAtPathPlanTarget = Vector3.zero;

	private ActorBase _targetActor;

	public override int _pBehaviourType
	{
		get
		{
			return 5;
		}
	}

	protected override bool _pShouldUpdate
	{
		get
		{
			return base._pShouldUpdate && _ownerAllegiance != EAllegiance.NEUTRAL;
		}
	}

	private bool _pHasTarget
	{
		get
		{
			return _targetActor != null;
		}
	}

	public BehaveMoveToTarget()
	{
		_stateCurrent = STATE.MTT_UNSET;
		_timerBehaviour = 0f;
		_timerState = 0f;
	}

	public override void InitialiseData()
	{
	}

	protected override void OnUpdate()
	{
		if (base._pOwnerBrain == null && _stateCurrent != STATE.MTT_UNSET)
		{
			SwitchToState(STATE.MTT_UNSET);
			return;
		}
		_targetActor = null;
		if (base._pOwnerBrain._pHasTarget)
		{
			_targetActor = base._pOwnerBrain._pTargetInfo._actorTarget;
		}
		_timerBehaviour += Time.deltaTime;
		_timerState += Time.deltaTime;
		switch (_stateCurrent)
		{
		case STATE.MTT_STANDING:
			UpdateStanding();
			break;
		case STATE.MTT_MOVING_TOWARDS:
			UpdateMovingTowards();
			break;
		default:
			UpdateUnset();
			break;
		}
	}

	protected override void OnFixedUpdate()
	{
	}

	protected override void OnLateUpdate()
	{
	}

	protected override void OnShutdown()
	{
		if (_stateCurrent != STATE.MTT_UNSET)
		{
			SwitchToState(STATE.MTT_UNSET);
		}
	}

	protected override void OnPause()
	{
		if (_stateCurrent != STATE.MTT_UNSET)
		{
			SwitchToState(STATE.MTT_UNSET);
		}
	}

	protected override void OnUnPause()
	{
	}

	private void SwitchToState(STATE stateNew)
	{
		switch (_stateCurrent)
		{
		}
		_timerState = 0f;
		_stateCurrent = stateNew;
		switch (_stateCurrent)
		{
		case STATE.MTT_MOVING_TOWARDS:
			CreateMovementTowards();
			return;
		}
		if (base._pOwnerBrain != null)
		{
			base._pOwnerBrain.StopAllMovement();
		}
	}

	private void UpdateUnset()
	{
		if (base._pOwnerBrain != null && !(base._pOwnerBrain._pOwnerActor == null))
		{
			SwitchToState(STATE.MTT_STANDING);
		}
	}

	private void UpdateStanding()
	{
		if (_pHasTarget && base._pOwnerBrain._pTargetInfo._distToTarget > _rangeToAimFor)
		{
			SwitchToState(STATE.MTT_MOVING_TOWARDS);
		}
	}

	private void UpdateMovingTowards()
	{
		if (!_pHasTarget)
		{
			SwitchToState(STATE.MTT_STANDING);
		}
		else if (base._pOwnerBrain._pTargetInfo._distToTarget < _rangeToAimFor * (2f / 3f))
		{
			SwitchToState(STATE.MTT_STANDING);
		}
		else if (!base._pOwnerBrain._pIsMoving && _timerState >= 1.5f)
		{
			SwitchToState(STATE.MTT_MOVING_TOWARDS);
		}
		else if ((_targetActor._pPosition - _posAtPathPlanTarget).sqrMagnitude >= _replanDistSqrd)
		{
			SwitchToState(STATE.MTT_MOVING_TOWARDS);
		}
	}

	private void CreateMovementTowards()
	{
		if (base._pOwnerBrain != null && _pHasTarget)
		{
			base._pOwnerBrain.IssueMoveOrder(_targetActor._pPosition);
			_posAtPathPlanTarget = _targetActor._pPosition;
		}
	}

	public override void OnDrawGizmos()
	{
	}
}
