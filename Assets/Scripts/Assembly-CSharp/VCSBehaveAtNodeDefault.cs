using UnityEngine;

public class VCSBehaveAtNodeDefault : VCSBehaveBase
{
	private enum STATE
	{
		VCS_AND_UNSET = 0,
		VCS_AND_STANDING = 1,
		VCS_AND_LEAVENODE = 2,
		VCS_AND_FINISHED = 3,
		NUM_VCS_AND_STATES = 4
	}

	private STATE _stateCurrent;

	private float _timerBehaviour;

	private float _timerState;

	private float _timeAtNode;

	public override int _pBehaviourType
	{
		get
		{
			return 8;
		}
	}

	public VCSBehaveAtNodeDefault()
	{
		_stateCurrent = STATE.VCS_AND_UNSET;
		_timerBehaviour = 0f;
		_timerState = 0f;
	}

	public override void InitialiseData()
	{
	}

	protected override void OnUpdate()
	{
		if (base._pOwnerBrain == null && _stateCurrent != STATE.VCS_AND_UNSET)
		{
			SwitchToState(STATE.VCS_AND_UNSET);
			return;
		}
		_timerBehaviour += Time.deltaTime;
		_timerState += Time.deltaTime;
		switch (_stateCurrent)
		{
		case STATE.VCS_AND_STANDING:
			UpdateStanding();
			break;
		case STATE.VCS_AND_LEAVENODE:
			break;
		case STATE.VCS_AND_FINISHED:
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
		if (_stateCurrent != STATE.VCS_AND_UNSET)
		{
			SwitchToState(STATE.VCS_AND_UNSET);
		}
	}

	protected override void OnPause()
	{
		if (_stateCurrent != STATE.VCS_AND_UNSET)
		{
			SwitchToState(STATE.VCS_AND_UNSET);
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
		case STATE.VCS_AND_STANDING:
			_timeAtNode = ((base._pPathNode == null) ? 0f : Random.Range(base._pPathNode.stopTimeMin, base._pPathNode.stopTimeMax));
			return;
		case STATE.VCS_AND_LEAVENODE:
			LeaveNode();
			return;
		case STATE.VCS_AND_FINISHED:
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
		if (base._pOwnerBrain != null && !(base._pOwnerBrain._pOwnerActor == null))
		{
			if (base._pPathNode == null)
			{
				SwitchToState(STATE.VCS_AND_FINISHED);
			}
			else
			{
				SwitchToState(STATE.VCS_AND_STANDING);
			}
		}
	}

	private void UpdateStanding()
	{
		if (_timerState >= _timeAtNode)
		{
			SwitchToState(STATE.VCS_AND_LEAVENODE);
		}
	}

	private void LeaveNode()
	{
		if (base._pPathNode == null || base._pPathNode.nodeLinkData == null || base._pPathNode.nodeLinkData.Length == 0)
		{
			SwitchToState(STATE.VCS_AND_FINISHED);
			return;
		}
		base._pVcsInfo._pathNodeDummy = base._pVcsInfo._pathNodeCurrent.GetRandomNextNodeWithLink(false, base._pVcsInfo._pathNodePrevious, out base._pVcsInfo._nodeLinkCurrent);
		if (base._pVcsInfo._pathNodeDummy == null || base._pVcsInfo._nodeLinkCurrent == null)
		{
			SwitchToState(STATE.VCS_AND_FINISHED);
			return;
		}
		base._pVcsInfo._pathNodePrevious = base._pVcsInfo._pathNodeCurrent;
		base._pVcsInfo._pathNodeCurrent = base._pVcsInfo._pathNodeDummy;
		base._pVcsInfo._pathNodeDummy = null;
		if (!base._pOwnerBrain.VogonConstructorScriptPushBehaviour(base._pVcsInfo._nodeLinkCurrent.traverseLinkBehaviour, true))
		{
			SwitchToState(STATE.VCS_AND_FINISHED);
		}
	}

	public override void OnDrawGizmos()
	{
	}
}
