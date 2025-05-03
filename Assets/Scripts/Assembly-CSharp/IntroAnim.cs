using System;
using UnityEngine;

public class IntroAnim : MonoBehaviour
{
	private const float HALF_LIFE_TIME = 0.3f;

	private const int _numberOfNodePositions = 128;

	public BezierCurveManager _camPath;

	public float _startDelay;

	public float _camTrackTimeLength = 4f;

	public Vector3 _camRotationOffset;

	public Transform[] _pointsOfInterest;

	public Easing.EaseType _interpType = Easing.EaseType.EaseInOutSine;

	private SmoothFollow _gameCam;

	private float _camTrackTimer;

	private float _camTrackTimerNormalised;

	private float rotationDampener;

	private float _currentRotationTime;

	private Action _onComplete;

	private bool _hasStarted;

	private bool _introFinished;

	private Vector3[] _camPathNodePositions;

	private bool _hasSetNodePositions;

	private Transform _activePOI;

	private Transform _previousActivePOI;

	private PointOfInterest _poiScript;

	private bool _hasChangedPOI;

	private bool _isRotatingToPOI;

	private bool _isRotatingToSpline;

	private bool _isFocusedOnPOI;

	private float _poiFocusTime;

	private bool _hasFinishedCameraTransition = true;

	private Quaternion _targetRotation;

	private float _activePOItransitionTime = -1f;

	private float _activePOIfocusTime = -1f;

	private bool _isConsecutiveRotation;

	private Quaternion _currentRotation;

	private Quaternion _smoothedCameraTrack;

	public bool _pIntroFinished
	{
		get
		{
			return _introFinished;
		}
	}

	public void StartIntro(Action onComplete = null)
	{
		_hasStarted = true;
		_onComplete = onComplete;
		_gameCam = MinigameController._pInstance._pCamera;
		_camTrackTimer = 0f - _startDelay;
		_introFinished = false;
		_gameCam.GetComponent<SmoothFollow>().enabled = false;
		_camPathNodePositions = new Vector3[128];
		_hasSetNodePositions = false;
		for (int i = 0; i < 128; i++)
		{
			_camPathNodePositions[i] = _camPath.GetPositionAtDistance(_camPath.length * ((float)i / 128f));
		}
		_gameCam.transform.position = _camPathNodePositions[0];
		if (GetNearestPOIQuaternion() != Quaternion.identity)
		{
			_gameCam.transform.rotation = (_targetRotation = GetNearestPOIQuaternion());
			_previousActivePOI = _activePOI;
			_poiScript = _activePOI.GetComponent<PointOfInterest>();
			_activePOItransitionTime = _poiScript.transitionTime;
			_currentRotationTime = _activePOItransitionTime;
			_isFocusedOnPOI = true;
		}
		else
		{
			_gameCam.transform.rotation = (_targetRotation = GetCameraTrackQuaternion(0f));
		}
		LateUpdate();
	}

	private void LateUpdate()
	{
		if (!_hasStarted || _introFinished)
		{
			return;
		}
		if (_camTrackTimer < _camTrackTimeLength)
		{
			_camTrackTimer = Mathf.Min(_camTrackTimer + Time.deltaTime, _camTrackTimeLength);
		}
		_camTrackTimerNormalised = Easing.Ease(_interpType, Mathf.Max(0f, _camTrackTimer), _camTrackTimeLength, 0f, 1f);
		SetCameraPosition(_camTrackTimerNormalised);
		GetNearestPOIQuaternion();
		_hasChangedPOI = _activePOI != _previousActivePOI;
		if ((bool)_activePOI)
		{
			_targetRotation = GetNearestPOIQuaternion();
			LerpBetweenPOIandSplineQuaternions(_targetRotation);
		}
		else if (_hasFinishedCameraTransition)
		{
			_targetRotation = GetCameraTrackQuaternion(_camTrackTimerNormalised);
			_gameCam.transform.rotation = _targetRotation;
		}
		else
		{
			LerpBetweenPOIandSplineQuaternions(_targetRotation);
		}
		_previousActivePOI = _activePOI;
		if (_camTrackTimer == _camTrackTimeLength)
		{
			_introFinished = true;
		}
		if (_introFinished)
		{
			_gameCam.GetComponent<SmoothFollow>().enabled = true;
			if (_onComplete != null)
			{
				_onComplete();
			}
			base.enabled = false;
		}
	}

	private void SetCameraPosition(float normalisedTime)
	{
		float num = normalisedTime * 127f;
		int num2 = Mathf.FloorToInt(num);
		int num3 = Mathf.CeilToInt(num);
		_gameCam.transform.position = Vector3.Lerp(_camPathNodePositions[num2], _camPathNodePositions[num3], num % 1f);
	}

	private Quaternion GetCameraTrackQuaternion(float normalisedTime)
	{
		return Quaternion.Euler(_camPath.GetAnglesAtTime(normalisedTime)) * Quaternion.Euler(_camRotationOffset);
	}

	private Quaternion GetNearestPOIQuaternion()
	{
		Transform transform = null;
		float num = float.PositiveInfinity;
		bool flag = false;
		Transform[] pointsOfInterest = _pointsOfInterest;
		foreach (Transform transform2 in pointsOfInterest)
		{
			float num2 = Vector3.Distance(_gameCam.transform.position, transform2.position);
			if (num2 < num)
			{
				num = num2;
				transform = transform2;
			}
			flag = ((num < transform.GetComponent<PointOfInterest>().activeDistance) ? true : false);
		}
		if (flag)
		{
			_activePOI = transform;
			_poiScript = _activePOI.GetComponent<PointOfInterest>();
			return Quaternion.LookRotation(transform.position - _gameCam.transform.position);
		}
		_activePOI = null;
		_poiScript = null;
		return Quaternion.identity;
	}

	private void LerpToTargetQuaternion(Quaternion targetRotation)
	{
		rotationDampener = 1f - Mathf.Pow(0.5f, Time.deltaTime / 0.3f);
		Quaternion rotation = _gameCam.transform.rotation;
		Quaternion quaternion = Quaternion.Lerp(rotation, targetRotation, rotationDampener);
		_isConsecutiveRotation = false;
		_smoothedCameraTrack = quaternion;
		_gameCam.transform.rotation = quaternion;
	}

	private void LerpBetweenPOIandSplineQuaternions(Quaternion targetRotation)
	{
		if (_poiScript != null)
		{
			_activePOItransitionTime = _poiScript.transitionTime;
		}
		if (!_isRotatingToPOI && !_isRotatingToSpline && _hasChangedPOI)
		{
			_isRotatingToPOI = true;
		}
		if (_isRotatingToPOI)
		{
			_hasFinishedCameraTransition = false;
			_currentRotationTime += Time.deltaTime;
			if (_currentRotationTime >= _activePOItransitionTime)
			{
				_isFocusedOnPOI = true;
				_currentRotationTime = _activePOItransitionTime;
				if (_hasChangedPOI)
				{
					_isRotatingToPOI = false;
					_isRotatingToSpline = true;
				}
			}
		}
		if (_isRotatingToSpline)
		{
			if (_hasChangedPOI && _currentRotationTime < _activePOItransitionTime)
			{
				_isRotatingToPOI = true;
				_isRotatingToSpline = false;
				_currentRotationTime = 0.01f;
				_isConsecutiveRotation = true;
				_currentRotation = _gameCam.transform.rotation;
			}
			if (_currentRotationTime <= 0f)
			{
				_isRotatingToSpline = false;
				_isRotatingToPOI = false;
				_currentRotationTime = 0f;
				_hasFinishedCameraTransition = true;
			}
			_currentRotationTime -= Time.deltaTime;
		}
		float t = ((_activePOItransitionTime != 0f) ? Easing.Ease(Easing.EaseType.EaseInOutSine, _currentRotationTime, _activePOItransitionTime, 0f, 1f) : 1f);
		Quaternion rotation = ((!_isConsecutiveRotation) ? Quaternion.Lerp(GetCameraTrackQuaternion(_camTrackTimerNormalised), targetRotation, t) : Quaternion.Lerp(_currentRotation, targetRotation, t));
		_gameCam.transform.rotation = rotation;
	}
}
