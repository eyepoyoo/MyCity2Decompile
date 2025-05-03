using UnityEngine;

public class BehaveMoveToPosition : BehaveBase
{
	private enum STATE
	{
		MTP_UNSET = 0,
		MTP_MOVING = 1,
		MTP_FINISHED = 2,
		NUM_MTP_STATES = 3
	}

	private STATE _stateCurrent;

	private float _timerBehaviour;

	private float _timerState;

	private bool _finishOnDamage;

	private bool _finishOnThreat;

	private bool _hasMoved;

	private float _healthOnStart;

	private Vector3 _moveToPos = Vector3.zero;

	public override int _pBehaviourType
	{
		get
		{
			return 7;
		}
	}

	public bool _pFiniahOnDamage
	{
		set
		{
			_finishOnDamage = value;
		}
	}

	public bool _pFiniahOnThreat
	{
		set
		{
			_finishOnThreat = value;
		}
	}

	public Vector3 _pMoveToPos
	{
		set
		{
			_moveToPos = value;
		}
	}

	public BehaveMoveToPosition()
	{
		_stateCurrent = STATE.MTP_UNSET;
		_timerBehaviour = 0f;
		_timerState = 0f;
	}

	public override void InitialiseData()
	{
		if (base._pOwnerBrain != null && !(base._pOwnerBrain._pOwnerActor == null))
		{
			_healthOnStart = base._pOwnerBrain._pOwnerActor._pHealth;
		}
	}

	protected override void OnUpdate()
	{
		if (base._pOwnerBrain == null && _stateCurrent != STATE.MTP_UNSET)
		{
			SwitchToState(STATE.MTP_UNSET);
			return;
		}
		_timerBehaviour += Time.deltaTime;
		_timerState += Time.deltaTime;
		if (_stateCurrent != STATE.MTP_FINISHED)
		{
			if (_finishOnDamage && base._pOwnerBrain._pOwnerActor._pHealth != _healthOnStart)
			{
				SwitchToState(STATE.MTP_FINISHED);
				return;
			}
			if (_finishOnThreat && base._pOwnerBrain._pHasTargetValid)
			{
				SwitchToState(STATE.MTP_FINISHED);
				return;
			}
		}
		switch (_stateCurrent)
		{
		case STATE.MTP_MOVING:
			UpdateMoving();
			break;
		case STATE.MTP_FINISHED:
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
		if (_stateCurrent != STATE.MTP_UNSET)
		{
			SwitchToState(STATE.MTP_UNSET);
		}
	}

	protected override void OnPause()
	{
		if (_stateCurrent != STATE.MTP_UNSET)
		{
			SwitchToState(STATE.MTP_UNSET);
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
		case STATE.MTP_MOVING:
			_hasMoved = false;
			CreateMovementTowards();
			return;
		case STATE.MTP_FINISHED:
			_moveToPos = Vector3.zero;
			Finish();
			return;
		}
		if (base._pOwnerBrain != null)
		{
			base._pOwnerBrain.StopAllMovement();
		}
	}

	private void UpdateUnset()
	{
		if (base._pOwnerBrain != null && !(base._pOwnerBrain._pOwnerActor == null) && !(_moveToPos == Vector3.zero))
		{
			SwitchToState(STATE.MTP_MOVING);
		}
	}

	private void UpdateMoving()
	{
		if (base._pOwnerBrain._pIsAtCurrentDestination)
		{
			SwitchToState(STATE.MTP_FINISHED);
		}
		else if (!_hasMoved)
		{
			_hasMoved = base._pOwnerBrain._pOwnerActor._pIsMoving;
		}
		else if (!base._pOwnerBrain._pOwnerActor._pIsMoving)
		{
			SwitchToState(STATE.MTP_MOVING);
		}
	}

	private void CreateMovementTowards()
	{
		if (base._pOwnerBrain != null)
		{
			base._pOwnerBrain.IssueMoveOrder(_moveToPos);
		}
	}

	public override void OnDrawGizmos()
	{
	}
}
