using System;
using UnityEngine;

public class CameraHUB : MonoBehaviour
{
	[Serializable]
	public class CameraRegions
	{
		public Material _skyBox;

		public CityManager.REGIONS _region;

		public Vector2 _constraintAreaSize;

		public Vector2 _constraintAreaOffset;

		public TargetFogSettings _fogSettings;
	}

	[Serializable]
	public class TargetFogSettings
	{
		public bool _changeFogSettings;

		public bool _useFog;

		public FogMode _fogMode;

		public float _exponentialFogDensity;

		public float _linearStartDistance;

		public float _linearEndDistance;

		public Color _fogColor;
	}

	public enum EFocusType
	{
		NONE = 0,
		PAN_ONLY = 1,
		SCENARIO_COMPLETE = 2,
		MANUAL = 3,
		PARTIAL_PROGRESS = 4
	}

	public const string CINEMATIC_INTRO_CAMERA_ANIM = "CinematicIntroAnim";

	public const string TOUR_OF_THE_CITY_CAMERA_ANIM = "TourOfTheCityAnim";

	private const float FADE_TARGET = 0.4f;

	private const float CAMERA_PAN_MAX_CUMULATIVE = 110f;

	private const float CAMERA_PAN_MAX_CUMULATIVE_SQRD_INVERTED = 8.264463E-05f;

	private const float CAMERA_SPEED_WINDDOWN_MULTIPLIER = 0.5f;

	private const float CAMERA_SPEED_BASE = 0.8f;

	private const float CAMERA_BOUNDRY_DECREASE_CUMULATIVE_MULTIP = 4f;

	private const float VELOCITY_HALFLIFE = 0.2f;

	private static Vector3 CAMERA_DEFAULT_POS = new Vector3(43.9f, 47.3f, -386.9f);

	public static Action OnCinematicIntroAnimationComplete;

	public static Action OnTourOfTheCityAnimationComplete;

	private static CameraHUB _instance;

	[SerializeField]
	private Animator _animator;

	public CameraRegions[] cameraRegions;

	public Camera specialRenderCam;

	public float focalDistance = 5f;

	public float cameraMoveSpeedDesktop = 1f;

	public float cameraMoveSpeedMobile = 0.1f;

	public MeshRenderer fadeOut;

	public bool _debugRender;

	private int _cinematicIntroAnimHash;

	private int _tourOfTheCityAnimHash;

	private bool _isCinematicIntroPlaying;

	private bool _isTourOfTheCityPlaying;

	private Camera _cameraRef;

	private Vector3 _touchDownPoint;

	private Vector3 _touchDownPosition;

	private bool _wasTouchDown;

	private Bounds _bounds;

	private Vector3 _dummyVector;

	private Vector3 _dummyVector2;

	private bool _playerCanControl = true;

	private bool _playerCanControlExternalControl = true;

	private Vector3 _focusPoint;

	private Vector3 _startMovePos;

	private Vector3 _endMovePos;

	private float _tweenStartTime;

	private bool _tweeningCamera;

	private float _dragStartTime;

	private float _dragDist;

	private float _camMoveSpeed;

	private Vector3 _lastKnownDragPos;

	private Vector3 _dragCumulative;

	private bool _zoomingCamera;

	private bool _shouldBuildScenarioBuilding;

	private float _zoomStart;

	private float _zoomEnd;

	private float _currentZoom;

	private float _regionChangeFadeTarget;

	private float _regionChangeFadeStart;

	private float _fadeTarget;

	private float _fadeStart;

	private EFocusType _curFocusType;

	private GameObject _rootObjectToIsolate;

	private int _rootObjectIsolateLayer;

	private CityManager.REGIONS _currentRegionIndex;

	private bool _wantFade;

	private bool _wantRegionChangeFade;

	private bool _awaitingManualControlZoomOut;

	private bool _resolveBuild;

	private Material _regionChangeSkyBox;

	private TargetFogSettings _regionChangeFogSettings;

	private Vector3 _mouseVel;

	private Vector3 _prevMousePos;

	private Vector3 _velocity;

	private float _cielingY;

	private Vector3 _cielingPos;

	private bool _fadingToNewRegion;

	public static CameraHUB _pInstance
	{
		get
		{
			if (_instance == null)
			{
				_instance = UnityEngine.Object.FindObjectOfType<CameraHUB>();
				if (_instance != null)
				{
					_instance.Awake();
				}
			}
			return _instance;
		}
	}

	public static bool _pExists
	{
		get
		{
			return _instance != null;
		}
	}

	public float _pDragDist
	{
		get
		{
			return _dragDist;
		}
	}

	public float _pDragStartTime
	{
		get
		{
			return _dragStartTime;
		}
	}

	public EFocusType _pCurFocusType
	{
		get
		{
			return _curFocusType;
		}
	}

	public bool _pCameraControllable
	{
		get
		{
			return _playerCanControlExternalControl;
		}
		set
		{
			_playerCanControlExternalControl = value;
		}
	}

	public bool _pPlayerCanControl
	{
		get
		{
			return _playerCanControl;
		}
	}

	public bool _pWantFade
	{
		get
		{
			return _wantFade;
		}
		set
		{
			_wantFade = value;
		}
	}

	public Camera _pCameraRef
	{
		get
		{
			if (_cameraRef == null)
			{
				_cameraRef = GetComponent<Camera>();
			}
			return _cameraRef;
		}
	}

	public float _pDragDelta
	{
		get
		{
			return _dragCumulative.sqrMagnitude;
		}
	}

	public bool _pZoomingCamera
	{
		get
		{
			return _zoomingCamera;
		}
	}

	public CityManager.REGIONS _pRegion
	{
		get
		{
			return _currentRegionIndex;
		}
	}

	public GameObject _pRootObjectToIsolate
	{
		get
		{
			return _rootObjectToIsolate;
		}
		set
		{
			_rootObjectToIsolate = value;
		}
	}

	public void ResetCamPos()
	{
		base.transform.position = CAMERA_DEFAULT_POS;
	}

	public void SnapToPoint(Vector3 point, EFocusType focusType, float zoomDistance = 44f)
	{
		_endMovePos = point - base.transform.forward * zoomDistance;
		if (_endMovePos.y > CAMERA_DEFAULT_POS.y)
		{
			Vector3 intersection;
			LinePlaneIntersection(out intersection, point, -base.transform.forward, Vector3.down, _cielingPos);
			_endMovePos = intersection;
		}
		base.transform.position = _endMovePos;
	}

	public void FocusPoint(Vector3 point, EFocusType focusType, float zoomDistance = 44f)
	{
		_curFocusType = focusType;
		if (!_wantFade)
		{
			fadeOut.enabled = false;
		}
		else
		{
			fadeOut.enabled = true;
			Color color = fadeOut.material.color;
			color.a = 0f;
			fadeOut.material.color = color;
			_fadeStart = 0f;
			_fadeTarget = 0.4f;
		}
		float num = focalDistance;
		_startMovePos = base.transform.position;
		_endMovePos = point - base.transform.forward * num;
		_focusPoint = point;
		_tweenStartTime = Time.time;
		if (_endMovePos.y > _cielingY)
		{
			Vector3 intersection;
			LinePlaneIntersection(out intersection, point, -base.transform.forward, Vector3.down, _cielingPos);
			_endMovePos = intersection;
			float magnitude = (intersection - point).magnitude;
			num = magnitude;
		}
		if (_startMovePos.y > _cielingY)
		{
			Vector3 intersection2;
			LinePlaneIntersection(out intersection2, _startMovePos, -base.transform.forward, Vector3.down, _cielingPos);
			_startMovePos = intersection2;
		}
		if (_wantRegionChangeFade)
		{
			_tweenStartTime += 0.5f;
		}
		_tweeningCamera = true;
		_zoomStart = num;
		_zoomEnd = zoomDistance;
		_playerCanControl = false;
		_awaitingManualControlZoomOut = false;
	}

	public void RestoreNormalZoom()
	{
		if (_wantFade)
		{
			_fadeStart = fadeOut.material.color.a;
			_fadeTarget = 0f;
		}
		_zoomingCamera = true;
		_zoomStart = _currentZoom;
		_zoomEnd = focalDistance;
		if ((_focusPoint - base.transform.forward * _zoomEnd).y > _cielingY)
		{
			Vector3 intersection;
			LinePlaneIntersection(out intersection, _focusPoint, -base.transform.forward, Vector3.down, _cielingPos);
			_zoomEnd = (intersection - _focusPoint).magnitude;
		}
		_tweenStartTime = Time.time;
		_shouldBuildScenarioBuilding = false;
		ScreenHub._pInstance.TweenInScreenFromManualPan();
	}

	public void OnBuildPartialComplete()
	{
		_curFocusType = EFocusType.NONE;
		RestoreNormalZoom();
	}

	public void PlayCinematicIntroAnimation()
	{
		_pCameraControllable = false;
		_isCinematicIntroPlaying = true;
		_animator.enabled = true;
		_animator.Play(_cinematicIntroAnimHash, 0, 0f);
	}

	private void OnCinematicIntroAnimComplete()
	{
		StopAnimating();
		if (OnCinematicIntroAnimationComplete != null)
		{
			OnCinematicIntroAnimationComplete();
		}
	}

	public void PlayTourOfTheCityAnimation()
	{
		_pCameraControllable = false;
		_isTourOfTheCityPlaying = true;
		_animator.enabled = true;
		_animator.Play(_tourOfTheCityAnimHash, 0, 0f);
	}

	private void OnTourOfTheCityAnimComplete()
	{
		StopAnimating();
		if (OnTourOfTheCityAnimationComplete != null)
		{
			OnTourOfTheCityAnimationComplete();
		}
	}

	public void StopAnimating()
	{
		if (_animator.enabled)
		{
			_animator.enabled = false;
			_isTourOfTheCityPlaying = false;
			_isCinematicIntroPlaying = false;
			_pCameraControllable = true;
		}
	}

	private void Awake()
	{
		_instance = this;
		_dummyVector = default(Vector3);
		_dummyVector2 = default(Vector3);
		_cinematicIntroAnimHash = Animator.StringToHash("CinematicIntroAnim");
		_tourOfTheCityAnimHash = Animator.StringToHash("TourOfTheCityAnim");
		fadeOut.enabled = false;
		UpdateCameraConstraint();
		_cielingY = base.transform.position.y;
		_cielingPos = base.transform.position;
		UICamera.fallThrough = base.gameObject;
	}

	private void UpdateCameraConstraint(int index = 0)
	{
		_dummyVector2.y = base.transform.position.y;
		_dummyVector2.x = cameraRegions[index]._constraintAreaOffset.x;
		_dummyVector2.z = cameraRegions[index]._constraintAreaOffset.y;
		_dummyVector.x = cameraRegions[index]._constraintAreaSize.x;
		_dummyVector.y = 500f;
		_dummyVector.z = cameraRegions[index]._constraintAreaSize.y;
		_bounds.center = _dummyVector2;
		_bounds.size = _dummyVector;
	}

	public void SetRegion(CityManager.REGIONS region, bool fromCityLoad = false)
	{
		int num = cameraRegions.Length;
		for (int i = 0; i < num; i++)
		{
			if (cameraRegions[i]._region != region)
			{
				continue;
			}
			UpdateCameraConstraint(i);
			if (region != _currentRegionIndex)
			{
				_currentRegionIndex = region;
				_wantRegionChangeFade = true;
				if (fromCityLoad)
				{
					_regionChangeFadeStart = 1f;
					_regionChangeFadeTarget = 1f;
					ScreenHub._pInstance.SetHubTransitionFade(1f);
				}
				else
				{
					_regionChangeFadeStart = 0f;
					_regionChangeFadeTarget = 1f;
				}
				if (cameraRegions[i]._skyBox != null)
				{
					_regionChangeSkyBox = cameraRegions[i]._skyBox;
					_regionChangeFogSettings = cameraRegions[i]._fogSettings;
				}
			}
			break;
		}
	}

	public void PanToRegion(CityManager.REGIONS region)
	{
		int num = cameraRegions.Length;
		for (int i = 0; i < num; i++)
		{
			if (cameraRegions[i]._region == region)
			{
				Vector3 focusPoint = _focusPoint;
				focusPoint.x = cameraRegions[i]._constraintAreaOffset.x;
				focusPoint.z = cameraRegions[i]._constraintAreaOffset.y;
				Debug.Log(string.Concat("Panning to region ", region, " [", focusPoint, "]"));
				SetRegion(region);
				FocusPoint(focusPoint, EFocusType.PAN_ONLY, 44f);
			}
		}
	}

	public void RestoreNormalZoomBackButton()
	{
		if (_awaitingManualControlZoomOut)
		{
			_fadeTarget = 0f;
			_fadeStart = 0.4f;
			_awaitingManualControlZoomOut = false;
			_curFocusType = EFocusType.NONE;
			RestoreNormalZoom();
		}
	}

	private void Update()
	{
		UpdateAnims();
		_mouseVel = (Input.mousePosition - _prevMousePos) / Time.deltaTime;
		_prevMousePos = Input.mousePosition;
		bool flag = Input.touchCount > 0;
		Vector3 vector = Vector3.zero;
		if (Input.touches != null && Input.touches.Length > 0)
		{
			vector = Input.touches[0].position;
		}
		float num = cameraMoveSpeedMobile;
		if (_wantRegionChangeFade)
		{
			float hubTransitionFade = ScreenHub._pInstance.GetHubTransitionFade();
			ScreenHub._pInstance.SetHubTransitionFade(Mathf.MoveTowards(hubTransitionFade, _regionChangeFadeTarget, Time.deltaTime * 5f));
			if (_regionChangeFadeTarget == 0f && hubTransitionFade == 0f)
			{
				_wantRegionChangeFade = false;
			}
		}
		specialRenderCam.enabled = !_wantRegionChangeFade;
		if (_tweeningCamera)
		{
			float num2 = Time.time - _tweenStartTime;
			float num3 = 1f;
			if (_wantRegionChangeFade)
			{
				num3 = 0.25f;
			}
			if (!(num2 < 0f))
			{
				if (num2 < num3)
				{
					float t = Easing.Ease(Easing.EaseType.EaseOutCircle, num2, num3, 0f, 1f);
					base.transform.position = Vector3.Lerp(_startMovePos, _endMovePos, t);
				}
				else
				{
					base.transform.position = _endMovePos;
					_tweeningCamera = false;
					if (_curFocusType == EFocusType.SCENARIO_COMPLETE)
					{
						_zoomingCamera = true;
						_shouldBuildScenarioBuilding = true;
						ScreenHub._pInstance.OnWantCompleteScenario();
					}
					else if (_curFocusType == EFocusType.MANUAL || _curFocusType == EFocusType.PARTIAL_PROGRESS)
					{
						Debug.Log("Manual focus now zooming cam");
						_zoomingCamera = true;
					}
					else
					{
						if (_curFocusType == EFocusType.PAN_ONLY)
						{
							_curFocusType = EFocusType.NONE;
						}
						if (_wantRegionChangeFade)
						{
							_regionChangeFadeTarget = 0f;
							if (_regionChangeSkyBox != null)
							{
								RenderSettings.skybox = _regionChangeSkyBox;
							}
							if (_regionChangeFogSettings != null && _regionChangeFogSettings._changeFogSettings)
							{
								RenderSettings.fog = _regionChangeFogSettings._useFog;
								RenderSettings.fogMode = _regionChangeFogSettings._fogMode;
								RenderSettings.fogColor = _regionChangeFogSettings._fogColor;
								RenderSettings.fogDensity = _regionChangeFogSettings._exponentialFogDensity;
								RenderSettings.fogEndDistance = _regionChangeFogSettings._linearEndDistance;
								RenderSettings.fogStartDistance = _regionChangeFogSettings._linearStartDistance;
							}
						}
						_playerCanControl = true;
						ScreenHub._pInstance.OnCompleteBasicCameraPan();
					}
					_tweenStartTime = Time.time;
				}
			}
		}
		else
		{
			_regionChangeFadeTarget = 0f;
			if (_regionChangeSkyBox != null)
			{
				RenderSettings.skybox = _regionChangeSkyBox;
			}
			if (_regionChangeFogSettings != null && _regionChangeFogSettings._changeFogSettings)
			{
				RenderSettings.fog = _regionChangeFogSettings._useFog;
				RenderSettings.fogMode = _regionChangeFogSettings._fogMode;
				RenderSettings.fogColor = _regionChangeFogSettings._fogColor;
				RenderSettings.fogDensity = _regionChangeFogSettings._exponentialFogDensity;
				RenderSettings.fogEndDistance = _regionChangeFogSettings._linearEndDistance;
				RenderSettings.fogStartDistance = _regionChangeFogSettings._linearStartDistance;
			}
		}
		if (_awaitingManualControlZoomOut && flag)
		{
			_fadeTarget = 0f;
			_fadeStart = 0.4f;
			_awaitingManualControlZoomOut = false;
			_curFocusType = EFocusType.NONE;
			RestoreNormalZoom();
		}
		if (_zoomingCamera)
		{
			float num4 = Time.time - _tweenStartTime;
			float num5 = 1f;
			if (_wantFade)
			{
				fadeOut.enabled = true;
			}
			if (num4 < num5)
			{
				float t2 = Easing.Ease(Easing.EaseType.EaseInOutSine, num4, num5, 0f, 1f);
				float num6 = (_currentZoom = Mathf.Lerp(_zoomStart, _zoomEnd, t2));
				base.transform.position = _focusPoint - base.transform.forward * num6;
				if (_wantFade)
				{
					Color color = fadeOut.material.color;
					color.a = Mathf.Lerp(_fadeStart, _fadeTarget, num4 / num5);
					fadeOut.material.color = color;
				}
			}
			else
			{
				base.transform.position = _focusPoint - base.transform.forward * _zoomEnd;
				_zoomingCamera = false;
				_currentZoom = _zoomEnd;
				if (_wantFade)
				{
					Color color2 = fadeOut.material.color;
					color2.a = _fadeTarget;
					fadeOut.material.color = color2;
				}
				if (_fadeTarget == 0f)
				{
					fadeOut.enabled = false;
				}
				if (!_shouldBuildScenarioBuilding)
				{
					if (_curFocusType == EFocusType.MANUAL)
					{
						_awaitingManualControlZoomOut = true;
					}
					else if (_curFocusType == EFocusType.PARTIAL_PROGRESS)
					{
						CityManager._pInstance.BuildPartialBuilding(ScenarioManager._pInstance._pCurrentScenario.building);
					}
					else
					{
						ScreenHub._pInstance._pTrackerIconFadeDirty = true;
						_playerCanControl = true;
					}
				}
			}
		}
		if (_resolveBuild)
		{
			float num7 = Time.time - _tweenStartTime;
			if (num7 < 0.5f)
			{
				float t3 = Easing.Ease(Easing.EaseType.EaseOutCircle, num7, 0.5f, 0f, 1f);
				base.transform.position = Vector3.Lerp(_startMovePos, _endMovePos, t3);
			}
			else
			{
				base.transform.position = _endMovePos;
				CityManager._pInstance.BuildEntireBuilding(ScenarioManager._pInstance._pCurrentScenario.building);
				_resolveBuild = false;
			}
		}
		if (!_playerCanControl || !_playerCanControlExternalControl)
		{
			_dragCumulative = Vector3.zero;
			return;
		}
		if (flag)
		{
			if (!_wasTouchDown)
			{
				_dragStartTime = Time.time;
				_dragDist = 0f;
				_touchDownPoint = vector;
				_touchDownPosition = base.transform.position;
				_lastKnownDragPos = vector;
				_prevMousePos = Input.mousePosition;
				_mouseVel = Vector3.zero;
			}
			_velocity = _mouseVel / Screen.height * -100f;
			Vector3 vector2 = _touchDownPoint - vector;
			Vector3 direction = vector2 * num;
			direction.z = direction.y;
			direction.y = 0f;
			direction = base.transform.TransformDirection(direction);
			direction.y = 0f;
			Vector3 point = _touchDownPosition + direction;
			if (!_bounds.Contains(point))
			{
				Vector3 vector3 = _bounds.ClosestPoint(point);
				vector3.y = point.y;
				point = vector3;
			}
			Vector3 vector4 = _lastKnownDragPos - vector;
			_lastKnownDragPos = vector;
			_dragDist += vector4.sqrMagnitude;
			if (_dragDist > 40f)
			{
				_dragCumulative += vector4;
				_dragCumulative.x = Mathf.Clamp(_dragCumulative.x, -110f, 110f);
				_dragCumulative.y = Mathf.Clamp(_dragCumulative.y, -110f, 110f);
				_dragCumulative.z = Mathf.Clamp(_dragCumulative.z, -110f, 110f);
			}
			_wasTouchDown = true;
		}
		else
		{
			_dragDist = 0f;
			_touchDownPosition = base.transform.position;
			_wasTouchDown = false;
		}
		Vector3 direction2 = _velocity * Time.deltaTime;
		direction2.z = direction2.y;
		direction2.y = 0f;
		direction2 = base.transform.TransformDirection(direction2);
		direction2.y = 0f;
		Vector3 vector5 = base.transform.position + direction2;
		bool flag2 = false;
		if (!_bounds.Contains(vector5))
		{
			Vector3 vector6 = _bounds.ClosestPoint(vector5);
			vector6.y = vector5.y;
			vector5 = vector6;
			flag2 = true;
		}
		base.transform.position = vector5;
		if (!flag)
		{
			_velocity *= Mathf.Pow(0.5f, Time.deltaTime / 0.2f);
		}
	}

	public void UpdateAnims()
	{
		if (_animator.enabled && !(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f))
		{
			_animator.enabled = false;
			if (_isCinematicIntroPlaying)
			{
				OnCinematicIntroAnimComplete();
			}
			if (_isTourOfTheCityPlaying)
			{
				OnTourOfTheCityAnimComplete();
			}
		}
	}

	public void ResolveBuild()
	{
		_resolveBuild = true;
		_tweenStartTime = Time.time;
		_startMovePos = base.transform.position;
		_endMovePos = CityManager._pInstance.GetCameraCenterFocusPos(ScenarioManager._pInstance._pCurrentScenario.building);
		_focusPoint = _endMovePos;
		_endMovePos -= base.transform.forward * _currentZoom;
	}

	private void FixedUpdate()
	{
	}

	public static bool LinePlaneIntersection(out Vector3 intersection, Vector3 linePoint, Vector3 lineVec, Vector3 planeNormal, Vector3 planePoint)
	{
		intersection = Vector3.zero;
		float num = Vector3.Dot(planePoint - linePoint, planeNormal);
		float num2 = Vector3.Dot(lineVec, planeNormal);
		if (num2 != 0f)
		{
			float size = num / num2;
			Vector3 vector = SetVectorLength(lineVec, size);
			intersection = linePoint + vector;
			return true;
		}
		return false;
	}

	public static Vector3 SetVectorLength(Vector3 vector, float size)
	{
		Vector3 vector2 = Vector3.Normalize(vector);
		return vector2 *= size;
	}

	public void OnPress(bool isDown)
	{
		CityManager._pInstance.OnPressNotOverGUI(isDown);
	}
}
