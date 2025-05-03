using System;
using UnityEngine;

public class Tween : MonoBehaviour
{
	private float _duration;

	private Action<Tween> _onComplete;

	private Easing.EaseType _interpFunc;

	private Vector3 _posFrom;

	private Vector3 _posTo;

	private Quaternion _rotFrom;

	private Quaternion _rotTo;

	private bool _timescaleIndependent;

	private bool _isLocal;

	private float _timer;

	public bool _doTweenPos { get; private set; }

	public bool _doTweenRot { get; private set; }

	public Vector3 _pPosTo
	{
		get
		{
			return _posTo;
		}
		set
		{
			_posTo = value;
		}
	}

	public Vector3 _pPosFrom
	{
		get
		{
			return _posFrom;
		}
		set
		{
			_posFrom = value;
		}
	}

	public void Init(float duration, Action<Tween> onComplete = null, Vector3? posFrom = null, Vector3? posTo = null, Quaternion? rotFrom = null, Quaternion? rotTo = null, Easing.EaseType interpFunc = Easing.EaseType.Linear, bool timescaleIndependent = true, bool isLocal = false, float delay = 0f)
	{
		_duration = duration;
		_onComplete = onComplete;
		_interpFunc = interpFunc;
		_timescaleIndependent = timescaleIndependent;
		_isLocal = isLocal;
		_doTweenPos = posTo.HasValue;
		_posFrom = (posFrom.HasValue ? posFrom.Value : ((!_isLocal) ? base.transform.position : base.transform.localPosition));
		_posTo = (posTo.HasValue ? posTo.Value : ((!_isLocal) ? base.transform.position : base.transform.localPosition));
		_doTweenRot = rotTo.HasValue;
		_rotFrom = (rotFrom.HasValue ? rotFrom.Value : ((!_isLocal) ? base.transform.rotation : base.transform.localRotation));
		_rotTo = (rotTo.HasValue ? rotTo.Value : ((!_isLocal) ? base.transform.rotation : base.transform.localRotation));
		_timer = 0f - delay;
		base.enabled = true;
	}

	private void Update()
	{
		_timer += ((!_timescaleIndependent) ? Time.deltaTime : Time.unscaledDeltaTime);
		if (_timer < 0f)
		{
			return;
		}
		if (_timer > _duration)
		{
			_timer = _duration;
		}
		if (_doTweenPos)
		{
			Vector3 vector = Vector3.LerpUnclamped(_posFrom, _posTo, Easing.Ease(_interpFunc, _timer, _duration, 0f, 1f));
			if (_isLocal)
			{
				base.transform.localPosition = vector;
			}
			else
			{
				base.transform.position = vector;
			}
		}
		if (_doTweenRot)
		{
			Quaternion quaternion = Quaternion.LerpUnclamped(_rotFrom, _rotTo, Easing.Ease(_interpFunc, _timer, _duration, 0f, 1f));
			if (_isLocal)
			{
				base.transform.localRotation = quaternion;
			}
			else
			{
				base.transform.rotation = quaternion;
			}
		}
		if (_timer == _duration)
		{
			_timer = -1f;
			base.enabled = false;
			if (_onComplete != null)
			{
				_onComplete(this);
			}
		}
	}
}
