using System;
using UnityEngine;

public class SimpleTargetCamBase : MonoBehaviour
{
	public enum UpdateType
	{
		Normal = 0,
		Late = 1,
		Fixed = 2
	}

	private Camera _unityCam;

	public GameObject _targetObject;

	public Vector3 _targetLocalOffset;

	public Vector3 _cameraShakeStrength = new Vector3(0.1f, 0.1f, 0.1f);

	private Vector3 _lastTargetPos;

	public float _targetPosSmooth = 0.05f;

	public float _cameraPosSmooth = 0.01f;

	public float _smoothFps;

	private bool _isUseWantCameraPos;

	private Vector3 _wantCameraPos;

	public float _targetScreenPosY;

	public bool _isTargetTrackingEnabled = true;

	private float _shakeDurationRemaining;

	public UpdateType _updateType;

	public Camera UnityCamera
	{
		get
		{
			return _unityCam;
		}
	}

	public GameObject TargetObject
	{
		get
		{
			return _targetObject;
		}
	}

	public Vector3 TargetPosition
	{
		get
		{
			return (!(_targetObject != null)) ? Vector3.zero : _ApplyTargetScreenPos(_targetObject.transform.TransformPoint(_targetLocalOffset));
		}
	}

	public Vector3 CameraPosition
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

	public Vector3 WantCameraPosition
	{
		set
		{
			_wantCameraPos = value;
			_isUseWantCameraPos = true;
		}
	}

	public void SetTargetObject(GameObject go, Vector3 offset)
	{
		if (_targetObject != go)
		{
			_targetObject = go;
			_lastTargetPos = ((!(go != null)) ? Vector3.zero : go.transform.TransformPoint(offset));
		}
		_targetLocalOffset = offset;
	}

	private void Awake()
	{
		_unityCam = GetComponent<Camera>();
		OnAwake();
	}

	protected virtual void OnAwake()
	{
		_wantCameraPos = base.transform.position;
	}

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		if (_updateType == UpdateType.Normal)
		{
			OnUpdate(deltaTime);
		}
	}

	private void LateUpdate()
	{
		float deltaTime = Time.deltaTime;
		if (_updateType == UpdateType.Late)
		{
			OnUpdate(deltaTime);
		}
	}

	private void FixedUpdate()
	{
		float fixedDeltaTime = Time.fixedDeltaTime;
		if (_updateType == UpdateType.Fixed)
		{
			OnUpdate(fixedDeltaTime);
		}
	}

	public void ShakeCamera(float duration)
	{
		Debug.Log("Shake Camera:" + duration);
		_shakeDurationRemaining = Mathf.Max(duration, _shakeDurationRemaining);
	}

	protected void SetLookingAtTarget()
	{
		_lastTargetPos = TargetPosition;
		base.transform.LookAt(_lastTargetPos);
		OnTransformUpdated();
	}

	protected virtual void OnUpdate(float dt)
	{
		if (!(_targetObject != null) || !_isTargetTrackingEnabled)
		{
			return;
		}
		float num = ((!(_smoothFps > 0f)) ? 1f : (dt * _smoothFps));
		if (_isUseWantCameraPos)
		{
			if (_cameraPosSmooth > 0f)
			{
				base.transform.position += (_wantCameraPos - base.transform.position) * _cameraPosSmooth * num;
			}
			else
			{
				base.transform.position = _wantCameraPos;
			}
		}
		Vector3 vector = TargetPosition;
		if (_targetPosSmooth > 0f)
		{
			vector = _lastTargetPos + (vector - _lastTargetPos) * _targetPosSmooth * num;
		}
		base.transform.LookAt(vector);
		if (_shakeDurationRemaining > 0f)
		{
			_shakeDurationRemaining -= Time.deltaTime;
			base.transform.position = base.transform.position + new Vector3(UnityEngine.Random.Range(0f - _cameraShakeStrength.x, _cameraShakeStrength.x), UnityEngine.Random.Range(0f - _cameraShakeStrength.y, _cameraShakeStrength.y), UnityEngine.Random.Range(0f - _cameraShakeStrength.z, _cameraShakeStrength.z));
		}
		else
		{
			_shakeDurationRemaining = 0f;
		}
		_lastTargetPos = vector;
		OnTransformUpdated();
	}

	protected virtual void OnTransformUpdated()
	{
	}

	private Vector3 _ApplyTargetScreenPos(Vector3 pos)
	{
		if (_unityCam != null)
		{
			float f = 0.5f * _unityCam.fieldOfView * ((float)Math.PI / 180f);
			float num = Mathf.Tan(f);
			num *= _targetScreenPosY;
			f = Mathf.Atan(num);
			pos -= base.transform.position;
			float magnitude = pos.magnitude;
			magnitude *= Mathf.Cos(f);
			Vector3 axis = Vector3.Cross(Vector3.up, pos);
			axis.Normalize();
			pos = MathHelper.RotateVector3(pos, axis, f);
			pos.Normalize();
			pos *= magnitude;
			pos += base.transform.position;
		}
		return pos;
	}
}
