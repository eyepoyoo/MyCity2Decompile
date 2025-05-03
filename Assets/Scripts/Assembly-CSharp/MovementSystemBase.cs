using System.Collections.Generic;
using GameDefines;
using UnityEngine;

public abstract class MovementSystemBase
{
	protected MoveBase _moveActive;

	protected Animator _animatorOwner;

	protected ActorBase _actorOwner;

	protected List<uint> _moveQueue = new List<uint>();

	protected Dictionary<uint, MoveBase> _moveChains = new Dictionary<uint, MoveBase>();

	public bool _pIsPlayingAnyMove
	{
		get
		{
			return _moveActive != null;
		}
	}

	public bool _pHasAnimatorValid
	{
		get
		{
			return _animatorOwner != null;
		}
	}

	public uint _pCurrentMoveHash
	{
		get
		{
			return (!_pIsPlayingAnyMove) ? 9898989u : _moveActive._pHashId;
		}
	}

	public MoveBase _pCurrentMove
	{
		get
		{
			return _moveActive;
		}
	}

	public ActorBase _pActorOwner
	{
		get
		{
			return _actorOwner;
		}
	}

	public Animator _pAnimatorOwner
	{
		get
		{
			return _animatorOwner;
		}
		set
		{
			_animatorOwner = value;
		}
	}

	protected bool _pHasQueuedMoves
	{
		get
		{
			return _moveQueue != null && _moveQueue.Count > 0;
		}
	}

	public abstract bool _pIsJumping { get; }

	public abstract bool _pIsAttacking { get; }

	public MovementSystemBase(ActorBase owner)
	{
		_actorOwner = owner;
	}

	protected abstract int GetMovePriority(uint hashId);

	protected abstract void SetUpMoveChains();

	protected abstract string GetMoveNameFromHash(uint hashId);

	public virtual void Initialise()
	{
		SetUpMoveChains();
	}

	public void AddMoveToQueue(uint hashId)
	{
		if (!_pIsPlayingAnyMove)
		{
			StartMoveChain(hashId);
		}
		else
		{
			QueueMove(hashId);
		}
	}

	public bool PlayMove(uint hashId, bool abortCurrentIfPoss = true, bool queueOtherwise = true)
	{
		if (_pIsPlayingAnyMove)
		{
			if (!abortCurrentIfPoss)
			{
				if (queueOtherwise)
				{
					QueueMove(hashId);
					return true;
				}
				return false;
			}
			if (abortCurrentIfPoss && _moveActive._pCanAbort)
			{
				if (AbortCurrentMove())
				{
					return StartMoveChain(hashId);
				}
				if (queueOtherwise)
				{
					QueueMove(hashId);
					return true;
				}
				return false;
			}
			if (abortCurrentIfPoss && !_moveActive._pCanAbort)
			{
				if (!queueOtherwise)
				{
					return false;
				}
				QueueMove(hashId);
				return true;
			}
		}
		return StartMoveChain(hashId);
	}

	public virtual void Update()
	{
		if (!(_actorOwner == null) && _actorOwner._pIsAlive)
		{
			if (!_pIsPlayingAnyMove && _pHasQueuedMoves)
			{
				FindAndStartQueuedMove();
			}
			else if (!_pIsPlayingAnyMove)
			{
				UpdateChainStarts();
				CheckShouldStarts();
			}
			else
			{
				UpdateCurrentChain();
			}
		}
	}

	public virtual void FixedUpdate()
	{
		if (!(_actorOwner == null) && _actorOwner._pIsAlive && _pIsPlayingAnyMove)
		{
			_moveActive.FixedUpdate();
		}
	}

	public virtual void LateUpdate()
	{
		if (!(_actorOwner == null) && _actorOwner._pIsAlive && _pIsPlayingAnyMove)
		{
			_moveActive.LateUpdate();
		}
	}

	public bool IsPlayingMove(uint moveHashId)
	{
		if (!_pIsPlayingAnyMove)
		{
			return false;
		}
		return _moveActive._pHashId == moveHashId;
	}

	public bool AbortCurrentMove()
	{
		if (_moveActive == null)
		{
			return true;
		}
		if (!_moveActive._pCanAbort)
		{
			return false;
		}
		_moveActive.Abort();
		_moveActive = null;
		return true;
	}

	public virtual void OnOwnerDeath()
	{
		ForceStopAllMoves();
	}

	public virtual void ShutDown()
	{
		ForceStopAllMoves();
	}

	public bool IsInAnimatorState(int stateHash, int layerIndex = -1)
	{
		if (_animatorOwner == null)
		{
			return false;
		}
		if (layerIndex < 0)
		{
			layerIndex = GlobalDefines.ANIM_LAYERINDEX_BASE;
		}
		if (_animatorOwner.layerCount <= layerIndex)
		{
			return false;
		}
		return _animatorOwner.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash == stateHash;
	}

	public bool IsBlendingIntoState(int stateHash, int layerIndex = -1)
	{
		if (_animatorOwner == null)
		{
			return false;
		}
		if (layerIndex < 0)
		{
			layerIndex = GlobalDefines.ANIM_LAYERINDEX_BASE;
		}
		if (_animatorOwner.layerCount <= layerIndex)
		{
			return false;
		}
		if (!_animatorOwner.IsInTransition(layerIndex))
		{
			return false;
		}
		return _animatorOwner.GetNextAnimatorStateInfo(layerIndex).fullPathHash == stateHash;
	}

	public bool IsBlendingOutOfState(int stateHash, int layerIndex = -1)
	{
		if (_animatorOwner == null)
		{
			return false;
		}
		if (layerIndex < 0)
		{
			layerIndex = GlobalDefines.ANIM_LAYERINDEX_BASE;
		}
		if (_animatorOwner.layerCount <= layerIndex)
		{
			return false;
		}
		if (!_animatorOwner.IsInTransition(layerIndex))
		{
			return false;
		}
		return _animatorOwner.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash == stateHash;
	}

	public bool IsBlendingOrInAnimatorState(int stateHash, int layerIndex = -1)
	{
		if (_animatorOwner == null)
		{
			return false;
		}
		if (layerIndex < 0)
		{
			layerIndex = GlobalDefines.ANIM_LAYERINDEX_BASE;
		}
		if (_animatorOwner.layerCount <= layerIndex)
		{
			return false;
		}
		if (_animatorOwner.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash == stateHash)
		{
			return true;
		}
		if (!_animatorOwner.IsInTransition(layerIndex))
		{
			return false;
		}
		return _animatorOwner.GetNextAnimatorStateInfo(layerIndex).fullPathHash == stateHash;
	}

	public void ForceStopAllMoves()
	{
		if (_pHasQueuedMoves)
		{
			_moveQueue.Clear();
		}
		if (_pIsPlayingAnyMove)
		{
			_moveActive.Finish();
			_moveActive = null;
		}
	}

	private bool StartMoveChain(uint hashId)
	{
		if (_moveChains == null)
		{
			return false;
		}
		if (!_moveChains.ContainsKey(hashId))
		{
			Debug.LogWarning("MovementSystemBase::StartMove - Move requested not present in dictionary. Move: " + GetMoveNameFromHash(hashId));
			return false;
		}
		if (_moveChains[hashId] == null)
		{
			Debug.LogWarning("MovementSystemBase::StartMove - Move requested has no chain antry. Move: " + GetMoveNameFromHash(hashId));
			_moveChains.Remove(hashId);
			return false;
		}
		if (_moveChains[hashId].Start())
		{
			_moveActive = _moveChains[hashId];
			return true;
		}
		return false;
	}

	private void FindAndStartQueuedMove()
	{
		if (!_pHasQueuedMoves)
		{
			return;
		}
		int num = int.MinValue;
		int num2 = -1;
		for (int i = 0; i < _moveQueue.Count; i++)
		{
			int movePriority = GetMovePriority(_moveQueue[i]);
			if (movePriority > num)
			{
				num = movePriority;
				num2 = i;
			}
		}
		if (num2 != -1)
		{
			uint hashId = _moveQueue[num2];
			_moveQueue.RemoveAt(num2);
			StartMoveChain(hashId);
		}
	}

	private void QueueMove(uint hashId)
	{
		if (_moveQueue != null && !_moveQueue.Contains(hashId))
		{
			_moveQueue.Insert(_moveQueue.Count, hashId);
		}
	}

	private void CheckShouldStarts()
	{
		if (_actorOwner == null || !_actorOwner._pIsAlive || _pIsPlayingAnyMove || _moveActive != null || _moveChains == null || _moveChains.Count == 0)
		{
			return;
		}
		int num = int.MinValue;
		uint num2 = 9898989u;
		foreach (MoveBase value in _moveChains.Values)
		{
			if (value != null && value._pShouldStart && value._pPriority > num)
			{
				num = value._pPriority;
				num2 = value._pHashId;
			}
		}
		if (num2 != 9898989)
		{
			StartMoveChain(num2);
		}
	}

	private void UpdateChainStarts()
	{
		if (_actorOwner == null || !_actorOwner._pIsAlive || _pIsPlayingAnyMove || _moveActive != null || _moveChains == null || _moveChains.Count == 0)
		{
			return;
		}
		foreach (MoveBase value in _moveChains.Values)
		{
			if (value != null)
			{
				value.Update();
			}
		}
	}

	private void UpdateCurrentChain()
	{
		if (_actorOwner == null || !_actorOwner._pIsAlive || !_pIsPlayingAnyMove || _moveActive == null)
		{
			return;
		}
		_moveActive.Update();
		if (!_moveActive._pHasMovesNext)
		{
			if (_moveActive._pHasCompleted)
			{
				_moveActive.Finish();
				_moveActive = null;
			}
			return;
		}
		int num = -1;
		int num2 = int.MinValue;
		for (int i = 0; i < _moveActive._pMovesNext.Count; i++)
		{
			if (_moveActive._pMovesNext[i] != null)
			{
				_moveActive._pMovesNext[i].Update();
				if (_moveActive._pMovesNext[i]._pShouldStart && _moveActive._pMovesNext[i]._pPriority > num2)
				{
					num2 = _moveActive._pMovesNext[i]._pPriority;
					num = i;
				}
			}
		}
		if (num != -1 && _moveActive._pMovesNext != null && num < _moveActive._pMovesNext.Count)
		{
			_moveActive.Finish();
			_moveActive = _moveActive._pMovesNext[num];
			if (_moveActive != null)
			{
				_moveActive.Start();
				return;
			}
		}
		if (_moveActive._pHasCompleted && num != -1)
		{
			_moveActive.Finish();
			_moveActive = null;
		}
	}
}
