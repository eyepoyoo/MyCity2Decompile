using System.Collections.Generic;
using KnowledgeSystem;
using LitJson;
using UnityEngine;

public class BrainActorBase
{
	private const int _cStartingNumOfBehaviours = 4;

	protected int _idActorBestTarget;

	protected bool _shouldUpdate;

	protected int _attackRequestedMelee;

	protected bool _attackRequestedRanged;

	protected float _arrivalRange;

	protected Vector3 _ourPos;

	protected Vector3 _ourForward;

	protected JsonData _brainData;

	protected ActorBase _ownerActor;

	protected TargetInfo _targetInfo;

	protected int[] _defaultBehaviours;

	protected KnowledgeSystemBase _knowledgeSystem;

	private bool _hadTarget;

	private bool _isAtDestination;

	private Vector3 _homePosition;

	private Vector3 _destinationPos;

	private VcsInfo _vcsInfo;

	private BehaveBase _dummyBehaviour;

	private EAllegiance _allegiance = EAllegiance.NEUTRAL;

	private List<BehaveBase> _dummyListBehave;

	private List<BehaveBase>[] _behaviour;

	public ActorBase _pOwnerActor
	{
		get
		{
			return _ownerActor;
		}
	}

	public JsonData _pDataBrain
	{
		get
		{
			return _brainData;
		}
	}

	public float _pArrivalRange
	{
		get
		{
			return _arrivalRange;
		}
		set
		{
			_arrivalRange = value;
		}
	}

	public Vector3 _pOurPos
	{
		get
		{
			return _ourPos;
		}
	}

	public Vector3 _pOurForward
	{
		get
		{
			return _ourForward;
		}
	}

	public Vector3 _pDestinationPos
	{
		get
		{
			return _destinationPos;
		}
		set
		{
			_destinationPos = value;
		}
	}

	public VcsInfo _pVcsInfo
	{
		get
		{
			return _vcsInfo;
		}
	}

	public TargetInfo _pTargetInfo
	{
		get
		{
			return _targetInfo;
		}
	}

	public virtual bool _pAllowAmmoReflection
	{
		get
		{
			return false;
		}
	}

	public virtual bool _pIsAtCurrentDestination
	{
		get
		{
			return _isAtDestination;
		}
	}

	public virtual bool _pHasAttacksRanged
	{
		get
		{
			return false;
		}
	}

	public virtual bool _pHasAttacksMelee
	{
		get
		{
			return false;
		}
	}

	public virtual float _pSpeedNormalised
	{
		get
		{
			if (_ownerActor == null)
			{
				return 0f;
			}
			return (!(_ownerActor._pSpeed > 0f)) ? 0f : 1f;
		}
	}

	public Vector3 _pHomePosition
	{
		get
		{
			return _homePosition;
		}
		set
		{
			_homePosition = value;
		}
	}

	public bool _pHasTarget
	{
		get
		{
			return _idActorBestTarget != -1 && _targetInfo._actorTarget != null;
		}
	}

	public virtual bool _pHasTargetValid
	{
		get
		{
			return _pHasTarget && _pOwnerActor != null;
		}
	}

	public bool _pIsMoving
	{
		get
		{
			if (_pOwnerActor == null)
			{
				return false;
			}
			return _pOwnerActor._pIsMoving;
		}
	}

	public int _pIdActorBestTarget
	{
		get
		{
			return _idActorBestTarget;
		}
		set
		{
			_idActorBestTarget = value;
			if (_targetInfo._actorTarget != null && _targetInfo._actorTarget._pIdActor != value)
			{
				_targetInfo.Reset();
			}
		}
	}

	public BehaveBase _pMoveBehaviour
	{
		get
		{
			if (_behaviour == null)
			{
				return null;
			}
			if (_behaviour[0] == null)
			{
				return null;
			}
			if (_behaviour[0].Count == 0)
			{
				return null;
			}
			return _behaviour[0][0];
		}
	}

	public virtual EAllegiance _pAllegiance
	{
		get
		{
			return _allegiance;
		}
		set
		{
			_allegiance = value;
		}
	}

	protected bool _pAttackRequestedMelee
	{
		get
		{
			return _attackRequestedMelee != 0;
		}
	}

	public BrainActorBase(ActorBase actorOwner)
	{
		_ownerActor = actorOwner;
		_shouldUpdate = false;
		_targetInfo = default(TargetInfo);
		_targetInfo.Reset();
	}

	public void Initialise()
	{
		if (_ownerActor != null)
		{
			_brainData = _ownerActor._pDataActor;
		}
		CreateAndInitialiseKnowledge();
		LookUpDefaultBehaviours();
		RevertToDefaultBehaviours();
		_shouldUpdate = true;
	}

	public void Kill()
	{
		if (_knowledgeSystem != null)
		{
			_knowledgeSystem.Shutdown();
		}
		_dummyBehaviour = null;
		_dummyListBehave = null;
		RemoveBehaviour();
		_targetInfo.Reset();
	}

	public void ShutDown()
	{
		_shouldUpdate = false;
		RemoveBehaviour();
	}

	protected virtual void CreateAndInitialiseKnowledge()
	{
		if (_knowledgeSystem == null)
		{
			_knowledgeSystem = new KnowledgeSystemBase();
			_knowledgeSystem.Initialise(_ownerActor, this, null);
		}
	}

	public virtual void Update()
	{
		if (_pOwnerActor == null || !_shouldUpdate)
		{
			return;
		}
		if (_knowledgeSystem != null)
		{
			_knowledgeSystem.Update();
		}
		UpdateDefaultLookDirection();
		UpdateTarget();
		if (_behaviour != null)
		{
			for (int i = 0; i < _behaviour.Length; i++)
			{
				if (_behaviour[i] != null && _behaviour[i].Count != 0 && _behaviour[i][0] != null)
				{
					_behaviour[i][0].Update();
				}
			}
		}
		UpdateAttacks();
	}

	public virtual void FixedUpdate()
	{
		if (!_shouldUpdate)
		{
			return;
		}
		if (_ownerActor != null)
		{
			_ourPos = _ownerActor._pPosition;
			_ourForward = _ownerActor._pForward;
		}
		_isAtDestination = (_destinationPos - _ourPos).sqrMagnitude <= _arrivalRange * _arrivalRange;
		UpdateTargetInfo();
		if (_behaviour == null)
		{
			return;
		}
		for (int i = 0; i < _behaviour.Length; i++)
		{
			if (_behaviour[i] != null && _behaviour[i].Count != 0 && _behaviour[i][0] != null)
			{
				_behaviour[i][0].FixedUpdate();
			}
		}
	}

	public virtual void LateUpdate()
	{
		if (!_shouldUpdate || _behaviour == null)
		{
			return;
		}
		for (int i = 0; i < _behaviour.Length; i++)
		{
			if (_behaviour[i] != null && _behaviour[i].Count != 0 && _behaviour[i][0] != null)
			{
				_behaviour[i][0].LateUpdate();
			}
		}
	}

	private void UpdateTarget()
	{
		if (!_shouldUpdate)
		{
			return;
		}
		if (_idActorBestTarget != -1)
		{
			ActorBase.FindActor(_idActorBestTarget, ref _targetInfo._actorTarget);
			if (_targetInfo._actorTarget == null || !_targetInfo._actorTarget._pIsAlive)
			{
				_pIdActorBestTarget = -1;
			}
		}
		if (!_hadTarget && _pHasTarget)
		{
			UpdateTargetInfo();
		}
		_hadTarget = _pHasTarget;
	}

	protected virtual void UpdateDefaultLookDirection()
	{
		if (_shouldUpdate && _pOwnerActor._pIsMoving)
		{
			_pOwnerActor._pForward = _ownerActor._pVelocity.normalized;
		}
	}

	private void UpdateTargetInfo()
	{
		if (_shouldUpdate && _pHasTarget && !(_ownerActor == null))
		{
			_targetInfo._fullyOnScreen = _targetInfo._actorTarget._pIsInCameraViewFull;
			_targetInfo._dummyTarget = ActorDummyTarget.DoesActorIdBelongToDummyTarget(_targetInfo._actorTarget._pIdActor);
			_targetInfo._moveDir = _targetInfo._actorTarget._pVelocity;
			_targetInfo._speed = _targetInfo._moveDir.magnitude;
			_targetInfo._moveDir.Normalize();
			_targetInfo._positionFloor = _targetInfo._actorTarget._pPosition;
			_targetInfo._positionTarget = _targetInfo._actorTarget._pTargetPos;
			_targetInfo._forward = _targetInfo._actorTarget._pForward;
			_targetInfo._dirToTarget = _targetInfo._positionTarget - _ownerActor._pLookOrigin;
			_targetInfo._dirToTargetFlat = _targetInfo._dirToTarget;
			_targetInfo._distToTarget = _targetInfo._dirToTarget.magnitude;
			_targetInfo._dirToTarget.Normalize();
			_targetInfo._dirToTargetFlat.y = 0f;
			_targetInfo._dirToTargetFlat.Normalize();
		}
	}

	protected virtual void UpdateAttacks()
	{
		_attackRequestedMelee = 0;
		_attackRequestedRanged = false;
	}

	public virtual void OnOwnerDeath()
	{
		if (_knowledgeSystem != null)
		{
			_knowledgeSystem.ClearAllKnowledge();
		}
		_shouldUpdate = false;
		StopAllMovement();
		RemoveBehaviour();
	}

	public virtual float ScoreKnowledgeBrainSpecific(ref Knowledge knowledge)
	{
		return 0f;
	}

	public void RequestAttackRanged()
	{
		_attackRequestedRanged = true;
	}

	public void RequestAttackMelee(int meleeId)
	{
		_attackRequestedMelee = meleeId;
	}

	public void StopAllMovement(bool force = false)
	{
		if (!(_ownerActor == null) && _ownerActor._pIsAlive)
		{
			_ownerActor.StopAllMovement();
		}
	}

	public virtual void IssueMoveOrder(Vector3 dest, float speedNorm = 1f)
	{
		if (!(_ownerActor == null) && _ownerActor._pIsAlive)
		{
			_destinationPos = dest;
			_ownerActor.IssueMoveOrder(ref dest, speedNorm);
		}
	}

	public bool VogonConstructorScriptEnter(ref PathNodeGroup pathNodeGroup)
	{
		if (_pOwnerActor == null)
		{
			return false;
		}
		if (pathNodeGroup == null)
		{
			return false;
		}
		if (_vcsInfo == null)
		{
			_vcsInfo = new VcsInfo();
			_vcsInfo.Reset();
		}
		_vcsInfo._pathNodePrevious = null;
		_vcsInfo._pathNodeCurrent = pathNodeGroup.GetHeadNode();
		if (_vcsInfo._pathNodeCurrent == null)
		{
			return false;
		}
		if (_vcsInfo._pathNodeCurrent.atNodeBehaviour == 0)
		{
			return false;
		}
		_pOwnerActor._pPosition = _vcsInfo._pathNodeCurrent.WorldPos;
		return VogonConstructorScriptPushBehaviour(_vcsInfo._pathNodeCurrent.atNodeBehaviour, false);
	}

	public void VogonConstructorScriptExit()
	{
		if (_vcsInfo != null)
		{
			if (_vcsInfo._pathNodeCurrent != null)
			{
				_pHomePosition = _vcsInfo._pathNodeCurrent.WorldPos;
			}
			_vcsInfo.Reset();
		}
	}

	public bool VogonConstructorScriptPushBehaviour(int VcsBehave, bool replace)
	{
		BehavePool._pInstance.GetBehaviour(VcsBehave, ref _dummyBehaviour, null);
		if (_dummyBehaviour == null)
		{
			return false;
		}
		if (_dummyBehaviour._pIsVcsBehaviour)
		{
			((VCSBehaveBase)_dummyBehaviour)._pVcsInfo = _vcsInfo;
		}
		else
		{
			Debug.LogWarning("BrainActorBase::VogonConstructorScriptPushBehaviour - was are entering a VCScript but are popping a non Vcs behaviour this should not happen, Behaviour: " + _dummyBehaviour._pBehaviourType);
		}
		PushBehaviour(_dummyBehaviour, EBType.MOVE, replace);
		return true;
	}

	public virtual int GetBehaviourEnumFromNameString(string name)
	{
		switch (name)
		{
		case "ATTACK_RANGED":
			return 3;
		case "MOVE_WITHIN_RANGE":
			return 4;
		case "MOVE_TO_TARGET":
			return 5;
		case "ATTACK_MELEE":
			return 6;
		case "MOVE_TO_POSIITON":
			return 7;
		case "VCS_AT_NODE_DEFAULT":
			return 8;
		case "VCS_TRAVERS_LINK_DEFAULT":
			return 9;
		case "VCS_TRAVERS_LINK_WAIT_ON_THREAT":
			return 10;
		case "RETURN_HOME":
			return 11;
		default:
			Debug.LogWarning("BrainActorBase::GetBehaviourEnumFromNameString - enum entry not found: " + name);
			return 0;
		}
	}

	public virtual void CreateBehaviour(int behaviour, ref BehaveBase newBehaviour)
	{
		switch (behaviour)
		{
		case 3:
			newBehaviour = new BehaveAttackRanged();
			break;
		case 4:
			newBehaviour = new BehaveMoveWithinRange();
			break;
		case 5:
			newBehaviour = new BehaveMoveToTarget();
			break;
		case 6:
			newBehaviour = new BehaveAttackMelee();
			break;
		case 7:
			newBehaviour = new BehaveMoveToPosition();
			break;
		case 8:
			newBehaviour = new VCSBehaveAtNodeDefault();
			break;
		case 9:
			newBehaviour = new VCSBehaveTraverseLinkDefault();
			break;
		case 10:
			newBehaviour = new VCSBehaveTraverseLinkWaitOnThreat();
			break;
		case 11:
			newBehaviour = new BehaveReturnHome();
			break;
		default:
			Debug.LogError("BrainActorBase::CreateBehaviour - ERROR, unknown Behaviour type: " + behaviour);
			break;
		}
	}

	public BehaveBase PushBehaviour(BehaveBase behaviour, EBType type, bool replace = false)
	{
		if (behaviour == null)
		{
			return null;
		}
		if (type == EBType.NUM_BTYPE)
		{
			Debug.LogWarning("BrainActorBase::PushBehaviour - not type was specified so we dont know which list to add the new behaviour to, nothing will be done. behaviour: " + behaviour.ToString());
			return null;
		}
		if (replace && _behaviour[(int)type].Count > 0)
		{
			PopBehaviour(_behaviour[(int)type][0]._pBehaviourType, type);
		}
		PopBehaviour(behaviour._pBehaviourType);
		if (_behaviour[(int)type].Count > 0)
		{
			_behaviour[(int)type][0].Pause();
		}
		behaviour._pOwnerBrain = this;
		_behaviour[(int)type].Insert(0, behaviour);
		behaviour._pBType = type;
		behaviour.InitialiseData();
		return behaviour;
	}

	public BehaveBase PushBehaviour(int behaviour, EBType type, bool replace = false)
	{
		_dummyBehaviour = null;
		if (type == EBType.NUM_BTYPE)
		{
			Debug.LogWarning("BrainActorBase::PushBehaviour - not type was specified so we dont know which list to add the new behaviour to, nothing will be done. behaviour: " + behaviour);
			return null;
		}
		if (_behaviour == null)
		{
			return null;
		}
		switch (behaviour)
		{
		case 0:
			RemoveBehaviour(type);
			return null;
		case 1:
			RevertToDefaultBehaviours(type);
			return null;
		default:
		{
			int num = -1;
			List<BehaveBase>[] behaviour2 = _behaviour;
			foreach (List<BehaveBase> list in behaviour2)
			{
				num++;
				if (list != null && list.Count > 0 && list[0]._pBehaviourType == behaviour)
				{
					Debug.Log("BrainActorBase::PushBehaviour - Behaviour not added because we already have one of that kind running. Behaviour Requested: " + behaviour + ", Type Requested: " + type.ToString() + ", Type Already Running: " + (EBType)num);
					return null;
				}
			}
			if (replace && _behaviour[(int)type].Count > 0)
			{
				PopBehaviour(_behaviour[(int)type][0]._pBehaviourType, type);
			}
			if (_behaviour[(int)type].Count > 0)
			{
				_behaviour[(int)type][0].Pause();
			}
			BehavePool._pInstance.GetBehaviour(behaviour, ref _dummyBehaviour, this);
			if (_dummyBehaviour != null)
			{
				_dummyBehaviour.InitialiseData();
				_dummyBehaviour._pBType = type;
				_behaviour[(int)type].Insert(0, _dummyBehaviour);
			}
			_dummyListBehave = null;
			return _dummyBehaviour;
		}
		}
	}

	public void PopBehaviour(int behaviour, EBType type = EBType.NUM_BTYPE)
	{
		if (_behaviour == null)
		{
			return;
		}
		if (type == EBType.NUM_BTYPE)
		{
			List<BehaveBase>[] behaviour2 = _behaviour;
			foreach (List<BehaveBase> list in behaviour2)
			{
				if (list != null && list.Count > 0 && list[0]._pBehaviourType == behaviour)
				{
					_dummyListBehave = list;
					break;
				}
			}
		}
		else
		{
			if (_behaviour[(int)type] == null || _behaviour[(int)type].Count == 0 || _behaviour[(int)type][0]._pBehaviourType != behaviour)
			{
				return;
			}
			_dummyListBehave = _behaviour[(int)type];
		}
		if (_dummyListBehave == null)
		{
			return;
		}
		_dummyBehaviour = _dummyListBehave[0];
		if (_dummyBehaviour == null)
		{
			_dummyListBehave = null;
			return;
		}
		_dummyBehaviour.Shutdown();
		_dummyListBehave.RemoveAt(0);
		BehavePool._pInstance.FreeBehaviour(behaviour, _dummyBehaviour._pIdBehave);
		if (_dummyListBehave.Count > 0)
		{
			_dummyListBehave[0].UnPause();
		}
		_dummyListBehave = null;
		_dummyBehaviour = null;
	}

	protected virtual void LookUpDefaultBehaviours()
	{
		if (_behaviour == null)
		{
			_behaviour = new List<BehaveBase>[4];
			for (int i = 0; i < 4; i++)
			{
				_behaviour[i] = new List<BehaveBase>(4);
			}
		}
		_defaultBehaviours = new int[4];
		for (int j = 0; j < 4; j++)
		{
			_defaultBehaviours[j] = 0;
		}
	}

	public void RevertToDefaultBehaviours(EBType type = EBType.NUM_BTYPE)
	{
		if (_behaviour == null)
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			if (_behaviour[i] == null || (type != EBType.NUM_BTYPE && type != (EBType)i))
			{
				continue;
			}
			if (_behaviour[i].Count > 0)
			{
				if (_behaviour[i][0]._pBehaviourType == _defaultBehaviours[i])
				{
					continue;
				}
				RemoveBehaviour((EBType)i);
			}
			if (_defaultBehaviours[i] != 0)
			{
				PushBehaviour(_defaultBehaviours[i], (EBType)i);
			}
		}
	}

	public void RemoveBehaviour(EBType type = EBType.NUM_BTYPE)
	{
		if (_behaviour == null)
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			if (_behaviour[i] == null || _behaviour[i].Count == 0 || (type != EBType.NUM_BTYPE && type != (EBType)i))
			{
				continue;
			}
			for (int num = _behaviour[i].Count - 1; num >= 0; num--)
			{
				if (_behaviour[i][num] != null)
				{
					_dummyBehaviour = _behaviour[i][num];
					_dummyBehaviour.Shutdown();
					_behaviour[i].RemoveAt(num);
					BehavePool._pInstance.FreeBehaviour(_dummyBehaviour._pBehaviourType, _dummyBehaviour._pIdBehave);
				}
			}
			_dummyBehaviour = null;
			_behaviour[i].Clear();
		}
	}

	public int GetCurrentBehaviourFromType(EBType type)
	{
		if (_behaviour == null)
		{
			return 0;
		}
		if (_behaviour.Length <= 0)
		{
			return 0;
		}
		for (int i = 0; i < 4; i++)
		{
			if (i == (int)type && _behaviour[i] != null && _behaviour[i].Count != 0 && _behaviour[i][0] != null)
			{
				return _behaviour[i][0]._pBehaviourType;
			}
		}
		return 0;
	}

	public virtual void OnDrawGizmos()
	{
		if (_ownerActor == null || !_ownerActor._renderDebugInfoBehaviours || _behaviour == null)
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			if (_behaviour[i] != null && _behaviour[i].Count != 0 && _behaviour[i][0] != null)
			{
				_behaviour[i][0].OnDrawGizmos();
			}
		}
	}
}
