using System.Collections.Generic;
using UnityEngine;

public abstract class SquadBase : MonoBehaviour
{
	private const float _cMinTargetingTime = 1.5f;

	private const float _cScoreKickThreashold = 500f;

	protected static readonly int _cDefaultCapacity = 15;

	public int _maxTargetersPerTarget = 15;

	public bool _renderDebugInfo;

	protected ActorBase _dummyActor;

	protected SquadMember _dummyMember;

	protected SquadTarget _dummyTarget;

	internal Dictionary<int, SquadMember> _members = new Dictionary<int, SquadMember>(_cDefaultCapacity);

	internal Dictionary<int, SquadTarget> _targets = new Dictionary<int, SquadTarget>(_cDefaultCapacity);

	private int _numMembersTotal;

	private int _numMembersIndividual;

	private bool _anyMembersVisiblePart;

	private bool _anyMembersVisibleFull;

	private bool _allMembersVisiblePart;

	private bool _allMembersVisibleFull;

	private Vector3 _dummyVector = default(Vector3);

	private List<int> _deleteList = new List<int>(_cDefaultCapacity);

	private List<SquadMember> _membersUnused = new List<SquadMember>(_cDefaultCapacity);

	private List<SquadTarget> _targetsUnused = new List<SquadTarget>(_cDefaultCapacity);

	public int _pNumMembers
	{
		get
		{
			return _numMembersIndividual;
		}
	}

	public int _pNumMembersTotal
	{
		get
		{
			return _numMembersTotal;
		}
	}

	public int _pNumTargets
	{
		get
		{
			return _targets.Count;
		}
	}

	public bool _pHasMembers
	{
		get
		{
			return _pNumMembers != 0;
		}
	}

	public bool _pAnyMembersVisiblePart
	{
		get
		{
			return _anyMembersVisiblePart;
		}
	}

	public bool _pAnyMembersVisibleFull
	{
		get
		{
			return _anyMembersVisibleFull;
		}
	}

	public bool _pAllMembersVisiblePart
	{
		get
		{
			return _allMembersVisiblePart;
		}
	}

	public bool _pAllMembersVisibleFull
	{
		get
		{
			return _allMembersVisibleFull;
		}
	}

	public int _pMaxTargetersPerTarget
	{
		get
		{
			if (_maxTargetersPerTarget < 0)
			{
				return _numMembersTotal;
			}
			return _maxTargetersPerTarget;
		}
		set
		{
			_maxTargetersPerTarget = value;
		}
	}

	public abstract bool CanMemberBeAddedToThisSquad(ref ActorBase actorMember);

	public abstract bool CanTargetBeAddedToThisSquad(ref ActorBase actorTarget);

	protected abstract bool IsValidTarget(int targetIdActor);

	protected virtual void Start()
	{
	}

	protected virtual void OnDestroy()
	{
		_dummyActor = null;
		_dummyMember = null;
		_dummyTarget = null;
		ClearAllTargets();
		ClearAllMembers(true);
	}

	protected virtual void Update()
	{
	}

	protected virtual void FixedUpdate()
	{
		UpdateMemberVisibility();
		RemoveInvalidSquadTargets();
		RemoveInvalidSquadMembers();
	}

	protected void UpdateMemberNumbers()
	{
		_numMembersTotal = _members.Count;
		_numMembersIndividual = 0;
		foreach (KeyValuePair<int, SquadMember> member in _members)
		{
			if (member.Value != null && !(member.Value._memberActor == null) && !member.Value._memberActor._pIsChild)
			{
				_numMembersIndividual++;
			}
		}
	}

	private void UpdateMemberVisibility()
	{
		_allMembersVisiblePart = true;
		_allMembersVisibleFull = true;
		_anyMembersVisiblePart = false;
		_anyMembersVisibleFull = false;
		foreach (KeyValuePair<int, SquadMember> member in _members)
		{
			if (member.Value != null && !(member.Value._memberActor == null) && !member.Value._memberActor._pIsChild)
			{
				_allMembersVisibleFull &= member.Value._memberActor._pIsInCameraViewFull;
				_allMembersVisiblePart &= member.Value._memberActor._pIsInCameraViewPart;
				if (member.Value._memberActor._pIsInCameraViewFull)
				{
					_anyMembersVisiblePart = true;
				}
				if (member.Value._memberActor._pIsInCameraViewPart)
				{
					_anyMembersVisiblePart = true;
				}
			}
		}
	}

	public virtual void AddMember(ActorBase actor)
	{
		if (!(actor == null) && !IsMemberOfSquad(actor._pIdActor) && CanMemberBeAddedToThisSquad(ref actor))
		{
			SquadMember squadMember = null;
			if (_membersUnused.Count > 0)
			{
				squadMember = _membersUnused[0];
				_membersUnused.RemoveAt(0);
			}
			else
			{
				squadMember = new SquadMember();
			}
			if (squadMember != null)
			{
				squadMember.Reset();
				squadMember._idActorMember = actor._pIdActor;
				squadMember._memberActor = actor;
				squadMember._memberActor._pSquad = this;
				_members.Add(squadMember._idActorMember, squadMember);
				OnNumMembersChanged(1);
			}
		}
	}

	public virtual void RemoveMember(int idActor, bool freeActorAsWell = false)
	{
		if (!IsMemberOfSquad(idActor))
		{
			return;
		}
		_dummyMember = _members[idActor];
		if (_dummyMember != null)
		{
			if (_dummyMember._pHasTarget)
			{
				RemoveTargeter(idActor);
			}
			_dummyActor = _dummyMember._memberActor;
			_dummyMember.Reset();
			_membersUnused.Add(_dummyMember);
			_members.Remove(idActor);
			if (_dummyActor != null)
			{
				_dummyActor._pSquad = null;
				if (freeActorAsWell)
				{
					_dummyActor.OnDeath(true);
				}
			}
			OnNumMembersChanged(-1);
		}
		_dummyMember = null;
		_dummyActor = null;
	}

	public void AddTarget(ref ActorBase actor)
	{
		if (!(actor == null) && !IsTargetOfSquad(actor._pIdActor) && CanTargetBeAddedToThisSquad(ref actor))
		{
			SquadTarget squadTarget = null;
			if (_targetsUnused.Count > 0)
			{
				squadTarget = _targetsUnused[0];
				_targetsUnused.RemoveAt(0);
			}
			else
			{
				squadTarget = new SquadTarget();
			}
			if (squadTarget != null)
			{
				squadTarget.Reset();
				squadTarget._idActor = actor._pIdActor;
				squadTarget._targetActor = actor;
				_targets.Add(squadTarget._idActor, squadTarget);
			}
		}
	}

	private void RemoveTarget(int idActor)
	{
		if (IsTargetOfSquad(idActor))
		{
			_dummyTarget = _targets[idActor];
			if (_dummyTarget != null)
			{
				_dummyTarget.Reset();
				_targetsUnused.Add(_dummyTarget);
				_targets.Remove(idActor);
			}
			_dummyTarget = null;
		}
	}

	private void RemoveInvalidSquadMembers()
	{
		_deleteList.Clear();
		foreach (KeyValuePair<int, SquadMember> member in _members)
		{
			if (member.Value._memberActor == null || !member.Value._memberActor._pIsAlive)
			{
				_deleteList.Add(member.Key);
			}
		}
		foreach (int delete in _deleteList)
		{
			RemoveMember(delete);
		}
	}

	private void RemoveInvalidSquadTargets()
	{
		_deleteList.Clear();
		foreach (KeyValuePair<int, SquadTarget> target in _targets)
		{
			if (target.Value._targetActor == null || !target.Value._targetActor._pIsAlive || target.Value._numTargeters == 0)
			{
				_deleteList.Add(target.Key);
			}
		}
		foreach (int delete in _deleteList)
		{
			RemoveTarget(delete);
		}
	}

	protected virtual void OnNumMembersChanged(int change)
	{
		UpdateMemberNumbers();
	}

	protected bool IsMemberOfSquad(int idActor)
	{
		if (_members == null)
		{
			return false;
		}
		if (_members.Count == 0)
		{
			return false;
		}
		return _members.ContainsKey(idActor);
	}

	private bool IsTargetOfSquad(int idActor)
	{
		if (_targets == null)
		{
			return false;
		}
		if (_targets.Count == 0)
		{
			return false;
		}
		return _targets.ContainsKey(idActor);
	}

	public virtual void OnSquadMemberDeath(int idActorMember)
	{
		RemoveMember(idActorMember);
	}

	public virtual void OnSquadMemberTakeDamage(int idActorMember, ref float amount)
	{
	}

	private void AddTargeter(int idActorTargeter, int idActorTarget, float score = 0f)
	{
		if (!IsMemberOfSquad(idActorTargeter))
		{
			return;
		}
		_dummyMember = _members[idActorTargeter];
		if (_dummyMember == null)
		{
			return;
		}
		_dummyActor = null;
		ActorBase.FindActor(idActorTarget, ref _dummyActor);
		if (_dummyActor == null)
		{
			return;
		}
		if (CanBeTargeted(idActorTarget))
		{
			bool flag = false;
			if (_dummyMember._pHasTarget && _dummyMember._idActorTarget != idActorTarget)
			{
				RemoveTargeter(idActorTargeter);
			}
			if (!_dummyMember._pHasTarget || !IsTargetOfSquad(idActorTarget))
			{
				AddTarget(ref _dummyActor);
				flag = true;
			}
			if (_targets.ContainsKey(idActorTarget))
			{
				_dummyTarget = _targets[idActorTarget];
				if (_dummyTarget != null)
				{
					_dummyMember._idActorTarget = idActorTarget;
					_dummyMember._targetScore = score;
					if (flag)
					{
						_dummyTarget.IncrementNumTargeters();
						_dummyMember._targetingStartTime = Time.time;
					}
				}
			}
		}
		_dummyMember = null;
		_dummyActor = null;
		_dummyTarget = null;
	}

	private void RemoveTargeter(int idActorTargeter, int idActorTarget = -1)
	{
		if (!IsMemberOfSquad(idActorTargeter))
		{
			return;
		}
		_dummyMember = _members[idActorTargeter];
		if (_dummyMember == null || !_dummyMember._pHasTarget)
		{
			return;
		}
		if (idActorTarget == -1)
		{
			idActorTarget = _dummyMember._idActorTarget;
		}
		if (IsTargetOfSquad(idActorTarget))
		{
			_dummyTarget = _targets[idActorTarget];
			if (_dummyTarget != null)
			{
				_dummyTarget.DecrementNumTargeters();
				if (_dummyTarget._numTargeters == 0)
				{
					_dummyTarget = null;
					RemoveTarget(idActorTarget);
				}
			}
		}
		_dummyMember._idActorTarget = -1;
		_dummyMember._targetingStartTime = 0f;
		_dummyMember._targetScore = 0f;
	}

	private bool CanBeTargeted(int targetIdActor, int targeterIdActor = -1)
	{
		if (!IsValidTarget(targetIdActor))
		{
			return false;
		}
		if (!IsTargetOfSquad(targetIdActor))
		{
			return true;
		}
		if (_targets[targetIdActor]._numTargeters < _pMaxTargetersPerTarget)
		{
			return true;
		}
		return false;
	}

	public int GetNearestSquadMember(ref Vector3 refPosition, int memberIdToIgnore = -1)
	{
		int result = -1;
		if (!_pHasMembers)
		{
			return result;
		}
		float num = float.PositiveInfinity;
		foreach (KeyValuePair<int, SquadMember> member in _members)
		{
			if (member.Value != null && !(member.Value._memberActor == null) && member.Value._idActorMember != memberIdToIgnore && !member.Value._memberActor._pIsChild)
			{
				_dummyVector = member.Value._memberActor._pPosition - refPosition;
				float sqrMagnitude = _dummyVector.sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					result = member.Value._idActorMember;
				}
			}
		}
		return result;
	}

	public Vector3 GetNearestSquadMemberPosition(ref Vector3 refPosition, int memberIdToIgnore = -1)
	{
		if (!_pHasMembers)
		{
			return Vector3.zero;
		}
		int nearestSquadMember = GetNearestSquadMember(ref refPosition, memberIdToIgnore);
		if (nearestSquadMember != -1 && _members.ContainsKey(nearestSquadMember) && _members[nearestSquadMember]._memberActor != null)
		{
			return _members[nearestSquadMember]._memberActor._pPosition;
		}
		return Vector3.zero;
	}

	public float GetDistanceToNearestSquadMember(ref Vector3 refPosition, ref int nearestMemberIdActor, int memberIdToIgnore = -1)
	{
		float result = 0f;
		if (!_pHasMembers)
		{
			return result;
		}
		nearestMemberIdActor = GetNearestSquadMember(ref refPosition, memberIdToIgnore);
		if (nearestMemberIdActor != -1 && _members.ContainsKey(nearestMemberIdActor) && _members[nearestMemberIdActor]._memberActor != null)
		{
			result = (_members[nearestMemberIdActor]._memberActor._pPosition - refPosition).magnitude;
		}
		return result;
	}

	public Vector3 GetVectorToNearestSquadMember(ref Vector3 refPosition, ref int nearestMemberIdActor, int memberIdToIgnore = -1)
	{
		Vector3 result = Vector3.zero;
		if (!_pHasMembers)
		{
			return result;
		}
		nearestMemberIdActor = GetNearestSquadMember(ref refPosition, memberIdToIgnore);
		if (nearestMemberIdActor != -1 && _members.ContainsKey(nearestMemberIdActor) && _members[nearestMemberIdActor]._memberActor != null)
		{
			result = _members[nearestMemberIdActor]._memberActor._pPosition - refPosition;
		}
		return result;
	}

	public void GetMemberActorByIndex(int index, ref ActorBase actor)
	{
		if (index >= _pNumMembers)
		{
			actor = null;
			return;
		}
		int num = 0;
		foreach (SquadMember value in _members.Values)
		{
			if (num == index && value != null)
			{
				actor = value._memberActor;
				break;
			}
			num++;
		}
	}

	public virtual int IssueBestTargetFromCollection(ref List<Knowledge> knowledgeList, int idActorMember)
	{
		if (!IsMemberOfSquad(idActorMember))
		{
			return -1;
		}
		_dummyMember = _members[idActorMember];
		if (_dummyMember == null)
		{
			return -1;
		}
		if (knowledgeList == null || knowledgeList.Count == 0)
		{
			RemoveTargeter(idActorMember);
			return -1;
		}
		if (_dummyMember._pHasTarget && !IsTargetOfSquad(_dummyMember._idActorTarget))
		{
			RemoveTargeter(idActorMember);
		}
		if (_dummyMember._pHasTarget && Time.time - _dummyMember._targetingStartTime < 1.5f)
		{
			return _dummyMember._idActorTarget;
		}
		if (_dummyMember._waitingToKick)
		{
			RemoveTargeter(idActorMember);
			_dummyMember._waitingToKick = false;
		}
		int num = -1;
		float score = 0f;
		foreach (Knowledge knowledge in knowledgeList)
		{
			if (!(knowledge == null))
			{
				if (!IsTargetOfSquad(knowledge._idActorTarget))
				{
					AddTargeter(_dummyMember._idActorMember, knowledge._idActorTarget, knowledge._threatValue);
					return knowledge._idActorTarget;
				}
				_dummyTarget = _targets[knowledge._idActorTarget];
				if (_dummyTarget != null && _dummyTarget._numTargeters <= 1 && _dummyMember._idActorTarget == knowledge._idActorTarget)
				{
					_dummyMember._targetScore = knowledge._threatValue;
					return knowledge._idActorTarget;
				}
				if (CanBeTargeted(knowledge._idActorTarget, idActorMember) && num == -1)
				{
					num = knowledge._idActorTarget;
					score = knowledge._threatValue;
				}
				if (ShouldWeKickLowPriorityTargeter(idActorMember, knowledge._idActorTarget, knowledge._threatValue, true, false))
				{
					AddTargeter(idActorMember, knowledge._idActorTarget, knowledge._threatValue);
					return knowledge._idActorTarget;
				}
			}
		}
		if (num != -1)
		{
			AddTargeter(_dummyMember._idActorMember, num, score);
			return num;
		}
		RemoveTargeter(_dummyMember._idActorMember);
		return -1;
	}

	private bool ShouldWeKickLowPriorityTargeter(int idActorMember, int idActorTarget, float targetScore, bool bDoIt = true, bool bCheckNumTargeters = true)
	{
		if (!IsMemberOfSquad(idActorMember))
		{
			return false;
		}
		if (bCheckNumTargeters && CanBeTargeted(idActorTarget))
		{
			return false;
		}
		if (_members[idActorMember]._idActorTarget == idActorTarget)
		{
			return false;
		}
		foreach (KeyValuePair<int, SquadMember> member in _members)
		{
			if (member.Value != null && member.Key != idActorMember && member.Value._idActorTarget == idActorTarget && member.Value._targetScore < targetScore && !(500f < targetScore - member.Value._targetScore))
			{
			}
		}
		return false;
	}

	public void RestoreAllMemberHealth()
	{
		foreach (KeyValuePair<int, SquadMember> member in _members)
		{
			if (member.Value != null && !(member.Value._memberActor == null))
			{
				member.Value._memberActor._pHealth = member.Value._memberActor._pHealthMax;
			}
		}
	}

	public void DamageAllMembers(float amount, int type)
	{
		foreach (SquadMember value in _members.Values)
		{
			if (value != null && !(value._memberActor == null) && !value._memberActor._pIsChild)
			{
				value._memberActor.TakeDamage(amount, value._memberActor._pPosition, Vector3.up, -1, false, type);
			}
		}
	}

	private int IsMemberTargeting(int idActorMember)
	{
		int result = -1;
		if (!IsMemberOfSquad(idActorMember))
		{
			return result;
		}
		_dummyMember = _members[idActorMember];
		if (_dummyMember != null && _dummyMember._pHasTarget)
		{
			result = _dummyMember._idActorTarget;
		}
		return result;
	}

	private bool IsAnyMemberTargetingTarget(int idActorTarget)
	{
		bool result = false;
		if (!IsTargetOfSquad(idActorTarget))
		{
			return result;
		}
		_dummyTarget = _targets[idActorTarget];
		if (_dummyTarget != null)
		{
			result = _dummyTarget._numTargeters > 0;
		}
		return false;
	}

	public void ClearAllMembers(bool freeAsWell = false)
	{
		List<int> list = new List<int>(_pNumMembers);
		foreach (KeyValuePair<int, SquadMember> member in _members)
		{
			list.Add(member.Key);
		}
		foreach (int item in list)
		{
			RemoveMember(item, freeAsWell);
		}
		_members.Clear();
	}

	private void ClearAllTargets()
	{
		foreach (KeyValuePair<int, SquadTarget> target in _targets)
		{
			if (target.Value != null)
			{
				target.Value.Reset();
			}
		}
		_targets.Clear();
	}

	protected virtual void OnDrawGizmos()
	{
		if (_renderDebugInfo)
		{
		}
	}
}
