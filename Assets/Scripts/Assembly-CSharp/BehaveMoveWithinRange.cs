using UnityEngine;

public class BehaveMoveWithinRange : BehaveBase
{
	private enum STATE
	{
		MWR_UNSET = 0,
		MWR_STANDING = 1,
		MWR_MOVING_TOWARDS = 2,
		MWR_MOVING_AWAY = 3,
		NUM_MWR_STATES = 4
	}

	private const float _cRangeMinDefault = 10f;

	private const float _cRangeMaxDefault = 30f;

	private const float _cTimeBeforePathRePlan = 1.5f;

	private STATE _stateCurrent;

	private float _timerBehaviour;

	private float _timerState;

	private float _rangeMin = 10f;

	private float _rangeMax = 30f;

	private float _rangeMid = 20f;

	public override int _pBehaviourType
	{
		get
		{
			return 4;
		}
	}

	protected override bool _pShouldUpdate
	{
		get
		{
			return base._pShouldUpdate && _ownerAllegiance != EAllegiance.NEUTRAL;
		}
	}

	public BehaveMoveWithinRange()
	{
		_stateCurrent = STATE.MWR_UNSET;
		_timerBehaviour = 0f;
		_timerState = 0f;
	}

	public override void InitialiseData()
	{
		_rangeMid = _rangeMin + (_rangeMax - _rangeMin) * 0.5f;
	}

	protected override void OnUpdate()
	{
		if (base._pOwnerBrain == null && _stateCurrent != STATE.MWR_UNSET)
		{
			SwitchToState(STATE.MWR_UNSET);
			return;
		}
		_timerBehaviour += Time.deltaTime;
		_timerState += Time.deltaTime;
		switch (_stateCurrent)
		{
		case STATE.MWR_STANDING:
			UpdateStanding();
			break;
		case STATE.MWR_MOVING_TOWARDS:
			UpdateMovingTowards();
			break;
		case STATE.MWR_MOVING_AWAY:
			UpdateMovingAway();
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
		if (_stateCurrent != STATE.MWR_UNSET)
		{
			SwitchToState(STATE.MWR_UNSET);
		}
	}

	protected override void OnPause()
	{
		if (_stateCurrent != STATE.MWR_UNSET)
		{
			SwitchToState(STATE.MWR_UNSET);
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
		case STATE.MWR_MOVING_TOWARDS:
			CreateMovementTowards();
			return;
		case STATE.MWR_MOVING_AWAY:
			CreateMovementAway();
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
			SwitchToState(STATE.MWR_STANDING);
		}
	}

	private void UpdateStanding()
	{
		if (base._pOwnerBrain._pHasTarget)
		{
			if (base._pOwnerBrain._pTargetInfo._distToTarget > _rangeMax)
			{
				SwitchToState(STATE.MWR_MOVING_TOWARDS);
			}
			else if (base._pOwnerBrain._pTargetInfo._distToTarget < _rangeMin)
			{
				SwitchToState(STATE.MWR_MOVING_AWAY);
			}
		}
	}

	private void UpdateMovingTowards()
	{
		if (!base._pOwnerBrain._pHasTarget)
		{
			SwitchToState(STATE.MWR_STANDING);
		}
		else if (base._pOwnerBrain._pTargetInfo._distToTarget < _rangeMin)
		{
			SwitchToState(STATE.MWR_MOVING_AWAY);
		}
		else if (base._pOwnerBrain._pTargetInfo._distToTarget < _rangeMid)
		{
			SwitchToState(STATE.MWR_STANDING);
		}
		else if (!base._pOwnerBrain._pIsMoving && _timerState >= 1.5f)
		{
			SwitchToState(STATE.MWR_MOVING_TOWARDS);
		}
	}

	private void UpdateMovingAway()
	{
		if (!base._pOwnerBrain._pHasTarget)
		{
			SwitchToState(STATE.MWR_STANDING);
		}
		else if (base._pOwnerBrain._pTargetInfo._distToTarget > _rangeMax)
		{
			SwitchToState(STATE.MWR_MOVING_TOWARDS);
		}
		else if (base._pOwnerBrain._pTargetInfo._distToTarget > _rangeMid)
		{
			SwitchToState(STATE.MWR_STANDING);
		}
		else if (!base._pOwnerBrain._pIsMoving && _timerState >= 1.5f)
		{
			SwitchToState(STATE.MWR_MOVING_AWAY);
		}
	}

	private void CreateMovementTowards()
	{
		if (base._pOwnerBrain != null && base._pOwnerBrain._pHasTarget)
		{
			base._pOwnerBrain.IssueMoveOrder(base._pOwnerBrain._pTargetInfo._positionFloor);
		}
	}

	private void CreateMovementAway()
	{
		if (base._pOwnerBrain != null && base._pOwnerBrain._pHasTarget)
		{
			base._pOwnerBrain.IssueMoveOrder(base._pOwnerBrain._pTargetInfo._positionFloor + -base._pOwnerBrain._pTargetInfo._dirToTarget * _rangeMax);
		}
	}

	public override void OnDrawGizmos()
	{
	}
}
