using UnityEngine;

public class DirectorArrow : MonoBehaviour
{
	private const float HOVER_OVER_TARGET_DIST = 80f;

	private const float HOVER_OVER_TARGET_DIST_SQRD = 6400f;

	private const float DURATION_CHANGE_POS = 0.3f;

	private const float SPEED_CHANGE_POS = 3.3333333f;

	private const float DURATION_HIDE = 0.2f;

	private const float SPEED_HIDE = 5f;

	private const float RAYCAST_INTERVAL = 0.2f;

	private static readonly Vector3 OFFSET_HIDDEN_MAX = Vector3.up * 5f;

	private static readonly Vector3 OFFSET_ABOVE_TARGET = Vector3.up * 5f;

	public Transform _model;

	public Camera _camera;

	public Material _matNormal;

	public Material _matNoDepth;

	private Vector3 _modelPosInit;

	private Transform _target;

	private Vector3 _targetPos;

	private Vector3 _targetPosLocalToCam;

	private float _normShown;

	private float _normOverTarget;

	private bool _canCamSee;

	private float _lastRaycastTime = float.NegativeInfinity;

	private bool _isCamObstructedCached;

	private int _raycastLayerMask;

	private bool _doRenderOnTop;

	private float _initFov;

	public BooleanStateRequestsSimple _pHideRequests { get; private set; }

	private Vector3 _pPosInHud
	{
		get
		{
			return _modelPosInit + OFFSET_HIDDEN_MAX * (1f - _normShown);
		}
	}

	private Vector3 _pPosOverTarget
	{
		get
		{
			return _targetPosLocalToCam + Camera.main.transform.InverseTransformDirection(OFFSET_ABOVE_TARGET);
		}
	}

	private Quaternion _pRotInHud
	{
		get
		{
			return Quaternion.LookRotation(_targetPosLocalToCam - (_model.localPosition - OFFSET_HIDDEN_MAX * (1f - _normShown)));
		}
	}

	private Quaternion _pRotOverTarget
	{
		get
		{
			return Quaternion.Euler(90f - Camera.main.transform.eulerAngles.x, 0f, Time.time * 360f * 1.5f);
		}
	}

	public Transform _pTarget
	{
		get
		{
			return _target;
		}
		set
		{
			if (!(value == _target))
			{
				_target = value;
				Update();
			}
		}
	}

	private bool _pDoShow
	{
		get
		{
			return _target != null && !_pHideRequests;
		}
	}

	private bool _pDoHoverOverTarget
	{
		get
		{
			return (bool)_target && _pDoShow && Camera.main.IsPointInFrustrum(_targetPos, 0.1f) && _targetPosLocalToCam.SqrMagnitudeXZ() < 6400f && !_pIsCamObstructed;
		}
	}

	private bool _pIsCamObstructed
	{
		get
		{
			if (!_target)
			{
				return false;
			}
			if (Time.time >= _lastRaycastTime + 0.2f)
			{
				Vector3 direction = _target.position - Camera.main.transform.position;
				_isCamObstructedCached = Physics.Raycast(Camera.main.transform.position, direction, direction.magnitude - 1f, _raycastLayerMask);
				_lastRaycastTime = Time.time;
			}
			return _isCamObstructedCached;
		}
	}

	public bool _pDoRenderOnTop
	{
		get
		{
			return _doRenderOnTop;
		}
		set
		{
			_doRenderOnTop = value;
			_model.GetComponentInChildren<Renderer>().sharedMaterial = ((!_doRenderOnTop) ? _matNormal : _matNoDepth);
		}
	}

	private void Awake()
	{
		_modelPosInit = _model.localPosition;
		_raycastLayerMask = LayerMask.GetMask("Geometry");
		_initFov = _camera.fieldOfView;
		_pHideRequests = new BooleanStateRequestsSimple(false);
	}

	private void Update()
	{
		_normShown = Mathf.MoveTowards(_normShown, _pDoShow ? 1 : 0, 5f * Time.deltaTime);
		_normOverTarget = Mathf.MoveTowards(_normOverTarget, _pDoHoverOverTarget ? 1 : 0, 3.3333333f * Time.deltaTime);
		if ((bool)_target)
		{
			_targetPos = _target.position;
			_targetPosLocalToCam = Camera.main.transform.InverseTransformPoint(_targetPos);
		}
		if (_normOverTarget == 0f)
		{
			_model.transform.localPosition = _pPosInHud;
			_model.transform.rotation = _pRotInHud;
			_camera.fieldOfView = _initFov;
		}
		else if (_normOverTarget == 1f)
		{
			_model.transform.localPosition = _pPosOverTarget;
			_model.transform.rotation = _pRotOverTarget;
			_camera.fieldOfView = Camera.main.fieldOfView;
		}
		else
		{
			float t = Easing.Ease(Easing.EaseType.EaseInOut, _normOverTarget, 1f, 0f, 1f);
			_model.transform.localPosition = Vector3.Lerp(_pPosInHud, _pPosOverTarget, t);
			_model.rotation = Quaternion.Lerp(_pRotInHud, _pRotOverTarget, t);
			_camera.fieldOfView = Mathf.Lerp(_initFov, Camera.main.fieldOfView, t);
		}
	}
}
