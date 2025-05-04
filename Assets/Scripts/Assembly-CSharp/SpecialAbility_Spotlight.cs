using System;
using UnityEngine;

public class SpecialAbility_Spotlight : SpecialAbility
{
	private const float EASE_TO_TARGET_HALFLIFE_NO_TARGET = 0.1f;

	private const float EASE_TO_TARGET_HALFLIFE_TARGET = 0f;

	private const float EASE_TO_TARGET_HALFLIFES_DIFF = 0.1f;

	private const float UPDATE_TARGET_INTERVAL = 0.2f;

	private const float BEAM_FADE_TIME = 0.2f;

	private const float BEAM_FADE_SPEED = 5f;

	private const float MAX_ANGLE_VERTICAL_RAD = 0.34906584f;

	private const float MAX_ALPHA = 0.5f;

	public Transform[] _pivots;

	public Transform[] _lightBeams;

	public Material _lightBeamMat;

	public float _maxAimDistance;

	private Vehicle _target;

	private readonly RepeatedAction _repeatedAction_FindTarget = new RepeatedAction(0.2f);

	private float _maxAimDistanceSqrd;

	private Color _lightBeamColour;

	private float _easeToTargetHalflife;

	private float _normAlpha = -1f;

	private float _pNormAlpha
	{
		get
		{
			return _normAlpha;
		}
		set
		{
			if (value != _normAlpha)
			{
				_normAlpha = value;
				_lightBeamColour.a = 0.5f * value;
				_lightBeamMat.SetColor("_TintColor", _lightBeamColour);
			}
		}
	}

	public event Action<Vehicle> _onHitVehicle;

	protected override void Awake()
	{
		base.Awake();
		_maxAimDistanceSqrd = _maxAimDistance * _maxAimDistance;
		_lightBeamColour = _lightBeamMat.GetColor("_TintColor");
		_pNormAlpha = 0f;
		for (int num = _lightBeams.Length - 1; num >= 0; num--)
		{
			_lightBeams[num].localScale = new Vector3(1f, 1f, _maxAimDistance);
		}
	}

	protected override void Update()
	{
		base.Update();
		_pNormAlpha = Mathf.MoveTowards(_pNormAlpha, base._pIsInUse ? 1 : 0, 5f * Time.deltaTime);
		if (_repeatedAction_FindTarget.Update())
		{
			FindTarget();
		}
		if ((bool)_target && !IsValidTarget_Aim(_target))
		{
			ClearTarget();
		}
		if ((bool)_target)
		{
			_easeToTargetHalflife = Mathf.MoveTowards(_easeToTargetHalflife, 0f, Time.deltaTime * 0.1f);
		}
		else
		{
			_easeToTargetHalflife = 0.1f;
		}
		for (int num = _pivots.Length - 1; num >= 0; num--)
		{
			_pivots[num].transform.rotation = MathHelper.EaseTowardsRotation(_pivots[num].transform.rotation, Quaternion.LookRotation((!_target) ? base.transform.forward : (_target.transform.position - base.transform.position), base.transform.up), _easeToTargetHalflife, Time.deltaTime);
		}
		if (!_target)
		{
			return;
		}
		if (!_target._isInSpotlight)
		{
			if (base._pIsInUse && IsInRange(_target))
			{
				HitVehicle(_target);
			}
		}
		else if (!IsInRange(_target))
		{
			_target._isInSpotlight = false;
		}
	}

	private void FindTarget()
	{
		MinigameObjective closestObjective = MinigameController._pInstance._pMinigame.GetClosestObjective(IsValidMinigameObjective);
		Vehicle vehicle = ((!closestObjective) ? null : closestObjective.GetComponent<Vehicle>());
		if (vehicle == _target)
		{
			return;
		}
		if ((bool)_target)
		{
			ClearTarget();
		}
		if ((bool)vehicle)
		{
			_target = vehicle;
			_easeToTargetHalflife = 0.1f;
			if (base._pIsInUse)
			{
				HitVehicle(vehicle);
			}
		}
	}

	private void ClearTarget()
	{
		if ((bool)_target)
		{
			_target._isInSpotlight = false;
			_target = null;
		}
	}

	protected override void OnStarted()
	{
		base.OnStarted();
		if ((bool)_target && !_target._isInSpotlight && IsInRange(_target))
		{
			HitVehicle(_target);
		}
		if ((bool)SoundFacade._pInstance)
		{
			SoundFacade._pInstance.PlayOneShotSFX("UseSpotlight", 0f);
		}
	}

	protected override void OnEnded()
	{
		base.OnEnded();
		if ((bool)_target)
		{
			ClearTarget();
			FindTarget();
		}
		if ((bool)SoundFacade._pInstance)
		{
			SoundFacade._pInstance.PlayOneShotSFX("StopSpotlight", 0f);
		}
	}

	private void HitVehicle(Vehicle vehicle)
	{
		if ((bool)vehicle)
		{
			vehicle._isInSpotlight = true;
			if (vehicle._isInSpotlight && this._onHitVehicle != null)
			{
				this._onHitVehicle(vehicle);
			}
		}
	}

	private bool IsValidMinigameObjective(MinigameObjective mo)
	{
		Vehicle component = mo.GetComponent<Vehicle>();
		return component != null && IsValidTarget_Aim(component);
	}

	private bool IsValidTarget_Aim(Vehicle vehicle)
	{
		Vector3 position = base.transform.position;
		Vector3 position2 = vehicle.transform.position;
		return Vector3.Dot(base.transform.forward, position2 - position) > 0f && Mathf.Atan2(position2.y - position.y, MathHelper.DistXZ(position, position2)) % (float)Math.PI < 0.34906584f;
	}

	private bool IsInRange(Vehicle vehicle)
	{
		return MathHelper.DistXZSqrd(base.transform.position, vehicle.transform.position) < _maxAimDistanceSqrd;
	}
}
