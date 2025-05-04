using System.Collections.Generic;
using UnityEngine;

public class ActorChild : ActorBase
{
	private const float _cMoveLookDirAngle = 2.5f;

	private const float OFFSET_LOOK_AT_POS = 50f;

	private static int _nextIdActorChild = 0;

	private static List<ActorChild> _actorChilds = new List<ActorChild>();

	public bool _forwardDirectionInverted;

	public Vector2 _xRotRange;

	public Vector2 _yRotRange;

	public Vector2 _zRotRange;

	protected ActorBase _parentActor;

	private int _idActorChildChar;

	private bool _needToRegisterOnUse = true;

	private Vector3 _currentRotation;

	private Vector3 _originalForwardLocal;

	private Vector3 _originalRightLocal;

	private Vector3 _localPosition;

	private Vector3 _lookAtPosActual;

	private Vector3 _lookAtPosDesired;

	public static List<ActorChild> _pListActorChildChars
	{
		get
		{
			return _actorChilds;
		}
	}

	public int _pIdActorChildChar
	{
		get
		{
			return _idActorChildChar;
		}
	}

	public override bool _pIsAlive
	{
		get
		{
			if (_healthMaxBase < 0f)
			{
				return _parentActor != null && _parentActor._pIsAlive;
			}
			return base._pIsAlive;
		}
	}

	public ActorBase _pParentActor
	{
		get
		{
			return _parentActor;
		}
	}

	public static bool FindActorChild(int idActor, ref ActorChild foundActorChildChar)
	{
		foreach (ActorChild pListActorChildChar in _pListActorChildChars)
		{
			if (pListActorChildChar == null || pListActorChildChar._pIdActor != idActor)
			{
				continue;
			}
			foundActorChildChar = pListActorChildChar;
			return true;
		}
		return false;
	}

	protected override void Awake()
	{
		_healthMaxBase = -1f;
		_isChildActor = true;
		_localPosition = base.transform.localPosition;
		base.Awake();
		if (!_actorChilds.Contains(this))
		{
			_actorChilds.Add(this);
			_idActorChildChar = _nextIdActorChild++;
		}
	}

	protected override void Start()
	{
		base.Start();
		_originalForwardLocal = base.transform.InverseTransformDirection(_pForward);
		_originalRightLocal = base.transform.InverseTransformDirection(_pRight);
		if (_forwardDirectionInverted)
		{
			_originalForwardLocal *= -1f;
			_originalRightLocal *= -1f;
		}
		FindAndRegisterWithParentActor();
	}

	private void OnEnable()
	{
		if (_needToRegisterOnUse)
		{
			FindAndRegisterWithParentActor();
		}
	}

	protected virtual void FindAndRegisterWithParentActor()
	{
		_parentActor = null;
		if (base.transform.parent != null)
		{
			GameObject gameObject = base.transform.parent.gameObject;
			if (gameObject != null)
			{
				_parentActor = gameObject.GetComponent<ActorBase>();
				if (_parentActor == null)
				{
					while (gameObject.transform.parent != null)
					{
						gameObject = gameObject.transform.parent.gameObject;
						if (gameObject != null)
						{
							_parentActor = gameObject.GetComponent<ActorBase>();
							if (_parentActor != null)
							{
								break;
							}
						}
					}
				}
			}
		}
		if (_parentActor == null)
		{
			Debug.LogWarning("ActorChild::FindAndRegisterWithParentActor - a child actor doesn't seem to have a parent actor. this actor child will not work correctly. ChildActorName: " + base.gameObject.name);
		}
		else
		{
			_parentActor.RegisterChildActor(this);
		}
	}

	public override void OnDeath(bool quiet = false)
	{
		base.OnDeath(quiet);
		if (_parentActor != null)
		{
			_parentActor.OnChildActorDeath(this);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_actorChilds.Remove(this);
	}

	protected override void Update()
	{
		base.Update();
		if (base.transform.localPosition != _localPosition)
		{
			base.transform.localPosition = _localPosition;
		}
		if (_pIsAlive)
		{
		}
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (_pIsAlive)
		{
		}
	}

	public void ResetLookDirection(bool force = false)
	{
		_dummyVec2 = base.transform.TransformDirection(_originalForwardLocal);
		_lookAtPosDesired = _pLookOrigin + _dummyVec2 * 50f;
		if (force)
		{
			_lookAtPosActual = _lookAtPosDesired;
			_pForward = base.transform.TransformDirection(_originalForwardLocal);
		}
	}

	protected void UpdateLookAtPos()
	{
		_currentRotation = base.transform.localEulerAngles;
		while (_currentRotation.x <= -180f)
		{
			_currentRotation.x += 360f;
		}
		while (_currentRotation.x > 180f)
		{
			_currentRotation.x -= 360f;
		}
		while (_currentRotation.y <= -180f)
		{
			_currentRotation.y += 360f;
		}
		while (_currentRotation.y > 180f)
		{
			_currentRotation.y -= 360f;
		}
		while (_currentRotation.z <= -180f)
		{
			_currentRotation.z += 360f;
		}
		while (_currentRotation.z > 180f)
		{
			_currentRotation.z -= 360f;
		}
		_currentRotation.x = Mathf.Clamp(_currentRotation.x, _xRotRange.x, _xRotRange.y);
		_currentRotation.y = Mathf.Clamp(_currentRotation.y, _yRotRange.x, _yRotRange.y);
		_currentRotation.z = Mathf.Clamp(_currentRotation.z, _zRotRange.x, _zRotRange.y);
		if (base.transform.localEulerAngles != _currentRotation)
		{
			base.transform.localEulerAngles = _currentRotation;
			_lookAtPosActual = _pLookOrigin + _pForward * 50f;
		}
	}

	public virtual void OnTeleport()
	{
		ResetLookDirection(true);
	}

	protected override void CreateBrainForActorTypeClass()
	{
		_brain = new BrainActorBase(this);
	}
}
