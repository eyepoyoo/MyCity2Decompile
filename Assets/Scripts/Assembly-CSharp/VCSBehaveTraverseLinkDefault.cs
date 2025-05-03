using UnityEngine;

public class VCSBehaveTraverseLinkDefault : VCSBehaveBase
{
	private enum STATE
	{
		VCS_TLD_UNSET = 0,
		VCS_TLD_MOVING = 1,
		VCS_TLD_ARRIVEATNODE = 2,
		VCS_TLD_FINISHED = 3,
		NUM_VCS_TLD_STATES = 4
	}

	private STATE _stateCurrent;

	private float _timerBehaviour;

	private float _timerState;

	private bool _hasMoved;

	private Vector3 _moveToPos;

	public override int _pBehaviourType
	{
		get
		{
			return 9;
		}
	}

	public VCSBehaveTraverseLinkDefault()
	{
		_stateCurrent = STATE.VCS_TLD_UNSET;
		_timerBehaviour = 0f;
		_timerState = 0f;
	}

	public override void InitialiseData()
	{
	}

	protected override void OnUpdate()
	{
		if (base._pOwnerBrain == null && _stateCurrent != STATE.VCS_TLD_UNSET)
		{
			SwitchToState(STATE.VCS_TLD_UNSET);
			return;
		}
		_timerBehaviour += Time.deltaTime;
		_timerState += Time.deltaTime;
		switch (_stateCurrent)
		{
		case STATE.VCS_TLD_MOVING:
			UpdateMoving();
			break;
		case STATE.VCS_TLD_ARRIVEATNODE:
			break;
		case STATE.VCS_TLD_FINISHED:
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
		base.OnShutdown();
		if (_stateCurrent != STATE.VCS_TLD_UNSET)
		{
			SwitchToState(STATE.VCS_TLD_UNSET);
		}
	}

	protected override void OnPause()
	{
		if (_stateCurrent != STATE.VCS_TLD_UNSET)
		{
			SwitchToState(STATE.VCS_TLD_UNSET);
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
		case STATE.VCS_TLD_MOVING:
			_hasMoved = false;
			CreateMovementTowards();
			return;
		case STATE.VCS_TLD_ARRIVEATNODE:
			FinishLinkTraverse();
			return;
		case STATE.VCS_TLD_FINISHED:
			Finish();
			return;
		}
		_moveToPos = Vector3.zero;
		if (base._pOwnerBrain != null)
		{
			base._pOwnerBrain.StopAllMovement();
		}
	}

	private void UpdateUnset()
	{
		if (base._pOwnerBrain != null && !(base._pOwnerBrain._pOwnerActor == null))
		{
			if (base._pPathNode != null)
			{
				_moveToPos = base._pPathNode.WorldPos;
			}
			if (!(_moveToPos == Vector3.zero))
			{
				SwitchToState(STATE.VCS_TLD_MOVING);
			}
		}
	}

	private void UpdateMoving()
	{
		if (base._pOwnerBrain._pIsAtCurrentDestination)
		{
			SwitchToState(STATE.VCS_TLD_ARRIVEATNODE);
		}
		else if (!_hasMoved)
		{
			_hasMoved = base._pOwnerBrain._pOwnerActor._pIsMoving;
		}
		else if (!base._pOwnerBrain._pOwnerActor._pIsMoving)
		{
			SwitchToState(STATE.VCS_TLD_MOVING);
		}
	}

	private void CreateMovementTowards()
	{
		if (base._pOwnerBrain != null)
		{
			base._pOwnerBrain.IssueMoveOrder(_moveToPos);
		}
	}

	private void FinishLinkTraverse()
	{
		if (base._pPathNode == null || base._pVcsInfo == null)
		{
			SwitchToState(STATE.VCS_TLD_FINISHED);
			return;
		}
		base._pVcsInfo._nodeLinkCurrent = null;
		if (!base._pPathNode._pShouldStopAtNode)
		{
			SkipToNextNode();
		}
		else if (!base._pOwnerBrain.VogonConstructorScriptPushBehaviour(base._pVcsInfo._pathNodeCurrent.atNodeBehaviour, true))
		{
			SwitchToState(STATE.VCS_TLD_FINISHED);
		}
	}

	private void SkipToNextNode()
	{
		if (base._pPathNode == null || base._pPathNode.nodeLinkData == null || base._pPathNode.nodeLinkData.Length == 0)
		{
			SwitchToState(STATE.VCS_TLD_FINISHED);
			return;
		}
		base._pVcsInfo._pathNodeDummy = base._pVcsInfo._pathNodeCurrent.GetRandomNextNodeWithLink(false, base._pVcsInfo._pathNodePrevious, out base._pVcsInfo._nodeLinkCurrent);
		if (base._pVcsInfo._pathNodeDummy == null || base._pVcsInfo._nodeLinkCurrent == null)
		{
			SwitchToState(STATE.VCS_TLD_FINISHED);
			return;
		}
		base._pVcsInfo._pathNodePrevious = base._pVcsInfo._pathNodeCurrent;
		base._pVcsInfo._pathNodeCurrent = base._pVcsInfo._pathNodeDummy;
		base._pVcsInfo._pathNodeDummy = null;
		SwitchToState(STATE.VCS_TLD_UNSET);
	}

	public override void OnDrawGizmos()
	{
	}
}
