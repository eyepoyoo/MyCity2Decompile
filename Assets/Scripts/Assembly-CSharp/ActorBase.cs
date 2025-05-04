using System.Collections.Generic;
using GameDefines;
using LitJson;
using UnityEngine;

public class ActorBase : MonoBehaviour
{
	public delegate void DOnDeath(ActorBase actor);

	public delegate void DOnTakeDamage(ActorBase actor);

	public delegate void DOnTakeDamageStatic(int actorId, float amount, EAllegiance allegiance);

	protected const float HEALTH_MAX_DEFAULT = 100f;

	private const float DEBUG_SPHERE_RADIUS = 0.25f;

	private static int _nextIdActor = 0;

	private static bool _actorListDirty = false;

	private static int[] _actorsIds = null;

	private static Dictionary<int, ActorBase> _actorsDict = new Dictionary<int, ActorBase>();

	public float _healthMaxBase = 100f;

	public Transform _targetMarker;

	public Transform _lookOrigin;

	public string[] _prefabsOnDeath;

	public SquadBase _initialSquad;

	public bool _renderDebugInfoActor;

	public bool _renderDebugInfoBehaviours;

	public bool _renderDebugInfoKnowledge;

	[SerializeField]
	protected bool _isPlayer;

	protected bool _alive = true;

	protected bool _inUse = true;

	protected bool _lookPosDefinedByUser;

	protected bool _isChildActor;

	protected bool _isInCameraViewPart = true;

	protected bool _isInCameraViewFull = true;

	protected bool _firstUpdate;

	protected float _speedCurrent;

	protected Vector3 _actorLastForward;

	protected Vector3 _actorLastPos;

	protected Vector3 _actorVelocity;

	protected Vector3 _dummyVec1;

	protected Vector3 _dummyVec2;

	protected JsonData _dataActor;

	protected SquadBase _squad;

	protected BrainActorBase _brain;

	protected List<ActorChild> _childrenActors;

	protected MovementSystemBase _movementSystem;

	private int _idActor = -1;

	private float _healthCurrent;

	public static int[] _pActorIds
	{
		get
		{
			if (_actorListDirty)
			{
				_actorListDirty = false;
				_actorsIds = new int[_actorsDict.Count];
				_actorsDict.Keys.CopyTo(_actorsIds, 0);
			}
			return _actorsIds;
		}
	}

	public static Dictionary<int, ActorBase> _pDictActors
	{
		get
		{
			return _actorsDict;
		}
	}

	public int _pIdActor
	{
		get
		{
			return _idActor;
		}
	}

	public bool _pIsChild
	{
		get
		{
			return _isChildActor;
		}
	}

	public bool _pIsInUse
	{
		get
		{
			return _inUse;
		}
	}

	public bool _pIsInCameraViewPart
	{
		get
		{
			return _isInCameraViewPart;
		}
	}

	public bool _pIsInCameraViewFull
	{
		get
		{
			return _isInCameraViewFull;
		}
	}

	public bool _pHasChildrenActors
	{
		get
		{
			return _childrenActors != null && _childrenActors.Count > 0;
		}
	}

	public virtual bool _pIsPlayer
	{
		get
		{
			return _isPlayer;
		}
		set
		{
			_isPlayer = value;
		}
	}

	public virtual bool _pIsMoving
	{
		get
		{
			return !GlobalDefines.IsApproximately(_actorVelocity, Vector3.zero, 1E-05f);
		}
	}

	public virtual bool _pIsAlive
	{
		get
		{
			return _alive && base.gameObject.activeInHierarchy;
		}
	}

	public virtual bool _pIsTargetable
	{
		get
		{
			return false;
		}
	}

	public float _pHealthNormalised
	{
		get
		{
			return _pHealth / _pHealthMax;
		}
	}

	public JsonData _pDataActor
	{
		get
		{
			return _dataActor;
		}
	}

	public float _pHealth
	{
		get
		{
			return _healthCurrent;
		}
		set
		{
			_healthCurrent = ((!(value < 0f)) ? value : 0f);
			if (_healthCurrent > _pHealthMax)
			{
				_healthCurrent = _pHealthMax;
			}
		}
	}

	public virtual float _pHealthMax
	{
		get
		{
			return _healthMaxBase;
		}
	}

	public float _pSpeed
	{
		get
		{
			return _speedCurrent;
		}
	}

	public Vector3 _pVelocity
	{
		get
		{
			return _actorVelocity;
		}
	}

	public SquadBase _pSquad
	{
		get
		{
			return _squad;
		}
		set
		{
			if (_squad != null)
			{
				if (_squad == value)
				{
					return;
				}
				_squad.RemoveMember(_pIdActor);
			}
			_squad = value;
		}
	}

	public bool _pIsMemberOfSquad
	{
		get
		{
			return _squad != null;
		}
	}

	public BrainActorBase _pBrain
	{
		get
		{
			return _brain;
		}
	}

	public MovementSystemBase _pMovementSystem
	{
		get
		{
			return _movementSystem;
		}
	}

	public virtual Vector3 _pLookOrigin
	{
		get
		{
			if (_lookOrigin != null)
			{
				return _lookOrigin.position;
			}
			return _pTargetPos;
		}
	}

	public virtual Vector3 _pPosition
	{
		get
		{
			return base.transform.position;
		}
		set
		{
			base.transform.position = value;
		}
	}

	public virtual Vector3 _pForward
	{
		get
		{
			return base.transform.forward;
		}
		set
		{
			base.transform.forward = value;
		}
	}

	public virtual Vector3 _pRight
	{
		get
		{
			return base.transform.right;
		}
		set
		{
			base.transform.right = value;
		}
	}

	public virtual Vector3 _pUp
	{
		get
		{
			return base.transform.up;
		}
		set
		{
			base.transform.up = value;
		}
	}

	public Vector3 _pTargetPos
	{
		get
		{
			if (_targetMarker != null)
			{
				return _targetMarker.position;
			}
			return _pPosition;
		}
	}

	public Quaternion _pTargetRot
	{
		get
		{
			if (_targetMarker != null)
			{
				return _targetMarker.rotation;
			}
			return base.transform.rotation;
		}
	}

	public bool _pHasTarget
	{
		get
		{
			if (_brain == null)
			{
				return false;
			}
			return _brain._pHasTarget;
		}
	}

	public bool _pHasTargetValid
	{
		get
		{
			if (_brain == null)
			{
				return false;
			}
			return _brain._pHasTargetValid;
		}
	}

	public virtual EAllegiance _pAllegiance
	{
		get
		{
			if (_pBrain == null)
			{
				return EAllegiance.NEUTRAL;
			}
			return _pBrain._pAllegiance;
		}
	}

	protected virtual bool _pShouldDestroyOnDeath
	{
		get
		{
			return true;
		}
	}

	public event DOnDeath _onDeath;

	public event DOnTakeDamage _onTakeDamageSpecificActor;

	public static event DOnTakeDamageStatic _onTakeDamageAnyActor;

	public static ActorBase CreateActor(string prefabName, Vector3 position)
	{
		return CreateActor(prefabName, position, Quaternion.identity);
	}

	public static ActorBase CreateActor(string prefabName, Vector3 position, Quaternion rotation)
	{
		if (string.IsNullOrEmpty(prefabName))
		{
			return null;
		}
		ActorBase actorBase = null;
		GameObject gameObject = Object.Instantiate(Resources.Load(prefabName)) as GameObject;
		if (gameObject != null)
		{
			actorBase = gameObject.GetComponent<ActorBase>();
			if (actorBase == null)
			{
				Object.Destroy(gameObject);
			}
			else
			{
				actorBase._pPosition = position;
				actorBase._actorLastPos = position;
				actorBase._pForward = rotation * Vector3.forward;
				actorBase.gameObject.SetActive(true);
			}
		}
		return actorBase;
	}

	public static bool FindActor(int idActor, ref ActorBase foundActor)
	{
		foundActor = null;
		if (_pDictActors == null)
		{
			return false;
		}
		if (_pDictActors.Count == 0)
		{
			return false;
		}
		if (!_pDictActors.ContainsKey(idActor))
		{
			return false;
		}
		foundActor = _pDictActors[idActor];
		return true;
	}

	public virtual void ReturnActor()
	{
		_inUse = false;
		Object.Destroy(base.gameObject);
	}

	protected virtual void Awake()
	{
		LookUpDataForActorTypeClass();
		if (_idActor == -1)
		{
			_idActor = _nextIdActor++;
			_actorsDict.Add(_idActor, this);
			_actorListDirty = true;
		}
		CreateAndInitialiseMovementSystem();
	}

	protected virtual void Start()
	{
		Initialise();
	}

	public virtual void Initialise()
	{
		_inUse = true;
		_alive = true;
		_firstUpdate = true;
		_pHealth = _pHealthMax;
		CreateAndInitialiseBrain();
		if (_childrenActors != null && _childrenActors.Count > 0)
		{
			for (int i = 0; i < _childrenActors.Count; i++)
			{
				if (!(_childrenActors[i] == null))
				{
					_childrenActors[i].gameObject.SetActive(true);
					_childrenActors[i].Initialise();
				}
			}
		}
		if (_initialSquad != null)
		{
			_initialSquad.AddMember(this);
		}
	}

	public virtual void NotifySpawnControllerOfDeathOrDespawn()
	{
	}

	protected virtual void OnDestroy()
	{
		if (_brain != null)
		{
			_brain.Kill();
		}
		if (_movementSystem != null)
		{
			_movementSystem.ShutDown();
		}
		_actorsDict.Remove(_idActor);
		_idActor = -1;
		_actorListDirty = true;
	}

	public virtual void OnDeath(bool quiet = false)
	{
		_alive = false;
		if (!quiet && _prefabsOnDeath != null && _prefabsOnDeath.Length > 0)
		{
			for (int i = 0; i < _prefabsOnDeath.Length; i++)
			{
				if (!string.IsNullOrEmpty(_prefabsOnDeath[i]))
				{
					Object.Instantiate(Resources.Load(_prefabsOnDeath[i]), _pTargetPos, _pTargetRot);
				}
			}
		}
		if (_pSquad != null)
		{
			_pSquad.OnSquadMemberDeath(_pIdActor);
		}
		if (_brain != null)
		{
			_brain.OnOwnerDeath();
		}
		if (_movementSystem != null)
		{
			_movementSystem.OnOwnerDeath();
		}
		NotifySpawnControllerOfDeathOrDespawn();
		if (this._onDeath != null)
		{
			this._onDeath(this);
		}
		if (!_pIsChild)
		{
			ReturnActor();
		}
	}

	protected virtual void Update()
	{
		if (_pIsAlive)
		{
			if (_firstUpdate)
			{
				_actorLastForward = _pForward;
				_actorLastPos = _pPosition;
				_firstUpdate = false;
			}
			UpdateInCameraView();
			if (_brain != null)
			{
				_brain.Update();
			}
			if (_movementSystem != null)
			{
				_movementSystem.Update();
			}
		}
	}

	protected virtual void FixedUpdate()
	{
		if (_pIsAlive)
		{
			if (_brain != null)
			{
				_brain.FixedUpdate();
			}
			if (_movementSystem != null)
			{
				_movementSystem.FixedUpdate();
			}
		}
	}

	protected virtual void LateUpdate()
	{
		if (_pIsAlive)
		{
			_actorVelocity = _pPosition - _actorLastPos;
			_actorVelocity *= 1f / Time.deltaTime;
			_speedCurrent = _actorVelocity.magnitude;
			if (_brain != null)
			{
				_brain.LateUpdate();
			}
			if (_movementSystem != null)
			{
				_movementSystem.LateUpdate();
			}
			_actorLastPos = _pPosition;
			_actorLastForward = _pForward;
		}
	}

	public virtual void TakeDamage(float amount, Vector3 pos, Vector3 normal, int damagerActorId = -1, bool quiet = false, int damageType = 0)
	{
		if (_healthMaxBase < 0f)
		{
			return;
		}
		float amount2 = amount;
		if (_squad != null)
		{
			_squad.OnSquadMemberTakeDamage(_pIdActor, ref amount2);
		}
		_pHealth -= amount2;
		if (_pHealth <= 0f)
		{
			OnDeath();
			return;
		}
		if (this._onTakeDamageSpecificActor != null)
		{
			this._onTakeDamageSpecificActor(this);
		}
		if (ActorBase._onTakeDamageAnyActor != null)
		{
			ActorBase._onTakeDamageAnyActor(_pIdActor, amount2, _pAllegiance);
		}
	}

	public virtual void RegisterChildActor(ActorChild child)
	{
		if (!(child == null))
		{
			if (_childrenActors == null)
			{
				_childrenActors = new List<ActorChild>(6);
			}
			if (!_childrenActors.Contains(child))
			{
				_childrenActors.Add(child);
			}
			child.Initialise();
		}
	}

	public virtual void Teleport(Vector3 position, Vector3 direction)
	{
		_pPosition = position;
		_pForward = direction;
		if (!_pHasChildrenActors)
		{
			return;
		}
		foreach (ActorChild childrenActor in _childrenActors)
		{
			if (!(childrenActor == null))
			{
				childrenActor.OnTeleport();
			}
		}
	}

	protected virtual void LookUpDataForActorTypeClass()
	{
		_dataActor = null;
	}

	protected virtual void CreateBrainForActorTypeClass()
	{
		_brain = null;
	}

	protected virtual void CreateMovementSystemForActorTypeClass()
	{
		_movementSystem = null;
	}

	public virtual void OnChildActorDeath(ActorChild child)
	{
	}

	public virtual void StopAllMovement()
	{
	}

	public virtual void IssueMoveOrder(ref Vector3 position, float speedNorm = 1f)
	{
	}

	protected virtual void UpdateInCameraView()
	{
	}

	protected virtual void OnDrawGizmos()
	{
		if (_renderDebugInfoBehaviours && _brain != null)
		{
			_brain.OnDrawGizmos();
		}
	}

	private void CreateAndInitialiseBrain()
	{
		if (_brain == null)
		{
			CreateBrainForActorTypeClass();
		}
		if (_brain != null)
		{
			_brain.Initialise();
		}
	}

	private void CreateAndInitialiseMovementSystem()
	{
		if (_movementSystem == null)
		{
			CreateMovementSystemForActorTypeClass();
		}
		if (_movementSystem != null)
		{
			_movementSystem.Initialise();
		}
	}
}
