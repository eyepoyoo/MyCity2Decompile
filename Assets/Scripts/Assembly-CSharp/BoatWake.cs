using System;
using UnityEngine;

[ExecuteInEditMode]
public class BoatWake : MonoBehaviour
{
	public float maxSpeed = 5f;

	private ParticleSystem _ps;

	private bool _isInWater = true;

	private float _speed = 5f;

	public bool _pIsInWater
	{
		get
		{
			return _isInWater;
		}
		set
		{
			_isInWater = value;
			ParticleSystem.EmissionModule emission = _pPS.emission;
			bool flag = _speed < -1f || _speed > 1f;
			emission.enabled = _isInWater && flag;
		}
	}

	public float _pSpeed
	{
		get
		{
			return _speed;
		}
		set
		{
			_speed = value;
			bool flag = _speed < -1f || _speed > 1f;
			ParticleSystem.EmissionModule emission = _pPS.emission;
			emission.enabled = _isInWater && flag;
			_pPS.startSpeed = Mathf.Clamp(_speed, 1f, maxSpeed);
		}
	}

	private ParticleSystem _pPS
	{
		get
		{
			if (_ps == null)
			{
				_ps = base.gameObject.GetComponent<ParticleSystem>();
			}
			return _ps;
		}
	}

	private void Update()
	{
		_pPS.startRotation = base.transform.eulerAngles.y * ((float)Math.PI / 180f);
	}
}
