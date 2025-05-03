using System.Collections.Generic;
using LitJson;
using UnityEngine;

public abstract class BehaveBase
{
	private static int _nextIdBehave = 0;

	private static List<BehaveBase> _behaves = new List<BehaveBase>();

	protected Vector3 _ourPosCurrent;

	protected Vector3 _ourPosLast;

	protected Vector3 _ourForward;

	protected JsonData _dummyData;

	protected EAllegiance _ownerAllegiance = EAllegiance.NEUTRAL;

	protected bool _paused;

	private int _idBehave;

	private EBType _type = EBType.NUM_BTYPE;

	private BrainActorBase _ownerBrain;

	public static List<BehaveBase> _pListBehaves
	{
		get
		{
			return _behaves;
		}
	}

	public int _pIdBehave
	{
		get
		{
			return _idBehave;
		}
	}

	public EBType _pBType
	{
		get
		{
			return _type;
		}
		set
		{
			_type = value;
		}
	}

	public virtual bool _pIsVcsBehaviour
	{
		get
		{
			return false;
		}
	}

	public virtual bool _pShouldPauseOnBuildPointActive
	{
		get
		{
			return true;
		}
	}

	public BrainActorBase _pOwnerBrain
	{
		get
		{
			return _ownerBrain;
		}
		set
		{
			_ownerBrain = value;
			if (_ownerBrain != null)
			{
				_ownerAllegiance = _ownerBrain._pAllegiance;
			}
		}
	}

	protected bool _pHasOwner
	{
		get
		{
			return _ownerBrain != null && _ownerBrain._pOwnerActor != null;
		}
	}

	protected ActorBase _pOwnerActor
	{
		get
		{
			return (!_pHasOwner) ? null : _ownerBrain._pOwnerActor;
		}
	}

	protected virtual bool _pShouldUpdate
	{
		get
		{
			return !_paused && _pHasOwner && _ownerBrain._pOwnerActor._pIsAlive;
		}
	}

	public abstract int _pBehaviourType { get; }

	public BehaveBase()
	{
		if (!_behaves.Contains(this))
		{
			_behaves.Add(this);
			_idBehave = _nextIdBehave++;
		}
		_paused = false;
	}

	public abstract void InitialiseData();

	public abstract void OnDrawGizmos();

	protected abstract void OnUpdate();

	protected abstract void OnFixedUpdate();

	protected abstract void OnLateUpdate();

	protected abstract void OnShutdown();

	protected abstract void OnPause();

	protected abstract void OnUnPause();

	~BehaveBase()
	{
		_behaves.Remove(this);
	}

	public void Update()
	{
		if (_pShouldUpdate)
		{
			_ownerAllegiance = _ownerBrain._pAllegiance;
			_ourPosLast = _ourPosCurrent;
			_ourPosCurrent = _ownerBrain._pOurPos;
			_ourForward = _ownerBrain._pOurForward;
			OnUpdate();
		}
	}

	public void FixedUpdate()
	{
		if (_pShouldUpdate)
		{
			OnFixedUpdate();
		}
	}

	public void LateUpdate()
	{
		if (_pShouldUpdate)
		{
			OnLateUpdate();
		}
	}

	public void Shutdown()
	{
		OnShutdown();
		_paused = false;
		_ownerBrain = null;
	}

	public void Pause()
	{
		_paused = true;
		OnPause();
	}

	public void UnPause()
	{
		_paused = false;
		OnUnPause();
	}

	protected virtual void Finish()
	{
		_paused = false;
		_type = EBType.NUM_BTYPE;
		if (_pOwnerBrain != null)
		{
			_pOwnerBrain.PopBehaviour(_pBehaviourType, _type);
		}
	}
}
