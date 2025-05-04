using UnityEngine;

public class BehaveReturnHome : BehaveBase
{
	private enum STATE
	{
		RH_UNSET = 0,
		RH_MOVING = 1,
		RH_FINISHED = 2,
		NUM_RH_STATES = 3
	}

	private STATE _stateCurrent;

	private float _timerBehaviour;

	private float _timerState;

	private bool _destroyOnArrival;

	private bool _hasMoved;

	private Vector3 _homePos = Vector3.zero;

	public override int _pBehaviourType
	{
		get
		{
			return 11;
		}
	}

	public bool _pDestroyOnArrival
	{
		set
		{
			_destroyOnArrival = value;
		}
	}

	public BehaveReturnHome()
	{
		_stateCurrent = STATE.RH_UNSET;
		_timerBehaviour = 0f;
		_timerState = 0f;
	}

	public override void InitialiseData()
	{
	}

	protected override void OnUpdate()
	{
		if (base._pOwnerBrain == null && _stateCurrent != STATE.RH_UNSET)
		{
			SwitchToState(STATE.RH_UNSET);
			return;
		}
		_timerBehaviour += Time.deltaTime;
		_timerState += Time.deltaTime;
		switch (_stateCurrent)
		{
		case STATE.RH_MOVING:
			UpdateMoving();
			break;
		case STATE.RH_FINISHED:
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
		if (_stateCurrent != STATE.RH_UNSET)
		{
			SwitchToState(STATE.RH_UNSET);
		}
	}

	protected override void OnPause()
	{
		if (_stateCurrent != STATE.RH_UNSET)
		{
			SwitchToState(STATE.RH_UNSET);
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
		case STATE.RH_MOVING:
			_hasMoved = false;
			CreateMovementTowards();
			break;
		case STATE.RH_FINISHED:
			if (_destroyOnArrival && base._pOwnerBrain != null && base._pOwnerBrain._pOwnerActor != null)
			{
				base._pOwnerBrain._pOwnerActor.ReturnActor();
				break;
			}
			Finish();
			if (base._pOwnerBrain != null)
			{
				base._pOwnerBrain.PushBehaviour(1, EBType.MOVE);
			}
			break;
		default:
			_homePos = Vector3.zero;
			if (base._pOwnerBrain != null)
			{
				base._pOwnerBrain.StopAllMovement();
			}
			break;
		}
	}

	private void UpdateUnset()
	{
		if (base._pOwnerBrain != null && !(base._pOwnerBrain._pOwnerActor == null))
		{
			_homePos = base._pOwnerBrain._pHomePosition;
			if (!(_homePos == Vector3.zero))
			{
				SwitchToState(STATE.RH_MOVING);
			}
		}
	}

	private void UpdateMoving()
	{
		if (base._pOwnerBrain._pIsAtCurrentDestination)
		{
			SwitchToState(STATE.RH_FINISHED);
		}
		else if (!_hasMoved)
		{
			_hasMoved = base._pOwnerBrain._pOwnerActor._pIsMoving;
		}
		else if (!base._pOwnerBrain._pOwnerActor._pIsMoving)
		{
			SwitchToState(STATE.RH_MOVING);
		}
	}

	private void CreateMovementTowards()
	{
		if (base._pOwnerBrain != null)
		{
			base._pOwnerBrain.IssueMoveOrder(_homePos);
		}
	}

	public override void OnDrawGizmos()
	{
	}
}
