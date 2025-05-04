using System.Collections.Generic;
using UnityEngine;

public abstract class MoveBase
{
	protected bool _firstUpdatePlaying = true;

	protected float _timeSinceStart;

	protected ActorBase _actorOwner;

	protected List<MoveBase> _movesNext;

	protected List<MoveBase> _movesPrevious;

	private bool _isPlaying;

	private MovementSystemBase _movementSystem;

	public bool _pIsPlaying
	{
		get
		{
			return _isPlaying;
		}
	}

	public bool _pShouldStart
	{
		get
		{
			return ShouldStart();
		}
	}

	public bool _pHasMovesNext
	{
		get
		{
			return _movesNext != null && _movesNext.Count > 0;
		}
	}

	public float _pTimeSinceStart
	{
		get
		{
			return _timeSinceStart;
		}
	}

	public virtual bool _pAllowsMovement
	{
		get
		{
			return false;
		}
	}

	public virtual bool _pAllowsOrientation
	{
		get
		{
			return false;
		}
	}

	public virtual bool _pAllowsInput
	{
		get
		{
			return false;
		}
	}

	public virtual bool _pHasCompleted
	{
		get
		{
			return false;
		}
	}

	public virtual bool _pCanAbort
	{
		get
		{
			return false;
		}
	}

	public List<MoveBase> _pMovesNext
	{
		get
		{
			return _movesNext;
		}
	}

	public MoveBase _pMoveNext
	{
		set
		{
			if (value != null)
			{
				if (_movesNext == null)
				{
					_movesNext = new List<MoveBase>();
				}
				if (!_movesNext.Contains(value))
				{
					_movesNext.Add(value);
					value._pMovePrevious = this;
				}
			}
		}
	}

	protected virtual bool _pSystemsCheckFull
	{
		get
		{
			return _pHasOwnerValid && _pHasMoveSysValid && _pHasAnimatorValid;
		}
	}

	protected virtual bool _pHasMoveSysValid
	{
		get
		{
			return _movementSystem != null;
		}
	}

	protected virtual bool _pHasAnimatorValid
	{
		get
		{
			return _pHasMoveSysValid && _pMoveSys._pAnimatorOwner != null;
		}
	}

	protected virtual bool _pHasOwnerValid
	{
		get
		{
			return _actorOwner != null && _actorOwner._pIsAlive;
		}
	}

	protected MovementSystemBase _pMoveSys
	{
		get
		{
			return _movementSystem;
		}
	}

	protected Animator _pAnimator
	{
		get
		{
			if (!_pHasMoveSysValid || !_pHasAnimatorValid)
			{
				return null;
			}
			return _pMoveSys._pAnimatorOwner;
		}
	}

	private MoveBase _pMovePrevious
	{
		set
		{
			if (value != null)
			{
				if (_movesPrevious == null)
				{
					_movesPrevious = new List<MoveBase>();
				}
				if (!_movesPrevious.Contains(value))
				{
					_movesPrevious.Add(value);
				}
			}
		}
	}

	public abstract int _pPriority { get; }

	public abstract uint _pHashId { get; }

	public MoveBase(MovementSystemBase movementSystem)
	{
		_movementSystem = movementSystem;
		if (_movementSystem != null)
		{
			_actorOwner = _movementSystem._pActorOwner;
		}
	}

	public virtual bool Start()
	{
		_timeSinceStart = 0f;
		_isPlaying = true;
		_firstUpdatePlaying = true;
		return true;
	}

	public void Update()
	{
		if (_isPlaying)
		{
			UpdatePlaying();
			if (_firstUpdatePlaying)
			{
				_firstUpdatePlaying = false;
			}
		}
		else
		{
			UpdateWaitingToStart();
		}
	}

	public void FixedUpdate()
	{
		if (_isPlaying)
		{
			FixedUpdatePlaying();
		}
	}

	public void LateUpdate()
	{
		if (_isPlaying)
		{
			LateUpdatePlaying();
		}
	}

	public virtual void Finish()
	{
		OnFinishAndAbort();
	}

	public virtual void Abort()
	{
		if (_pCanAbort)
		{
			OnFinishAndAbort();
		}
	}

	protected virtual void UpdatePlaying()
	{
		_timeSinceStart += Time.deltaTime;
	}

	protected virtual void FixedUpdatePlaying()
	{
	}

	protected virtual void LateUpdatePlaying()
	{
	}

	protected virtual void UpdateWaitingToStart()
	{
	}

	protected virtual bool ShouldStart()
	{
		if (_movesPrevious != null && _movesPrevious.Count > 0)
		{
			for (int i = 0; i < _movesPrevious.Count; i++)
			{
				if (_movesPrevious[i] != null && _movesPrevious[i]._pIsPlaying && _movesPrevious[i]._pHasCompleted)
				{
					return true;
				}
			}
		}
		return false;
	}

	protected virtual void OnFinishAndAbort()
	{
		_isPlaying = false;
	}
}
