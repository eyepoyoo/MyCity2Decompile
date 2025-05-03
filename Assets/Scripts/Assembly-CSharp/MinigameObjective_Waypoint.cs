using System;
using UnityEngine;

public class MinigameObjective_Waypoint : MinigameObjective
{
	public Transform facePos;

	public float _radius = 5f;

	public float _radiusVisualMultiply = 1f;

	public float _completionDuration = 1f;

	public bool _doCollectPlayersPickupable;

	public Transform _collectPickupablePoint;

	public bool _onlyActivateIfCanSee;

	public VehiclePart.EObstacleToNegate[] _spedUpByNegatedObstacles;

	private float _radiusSqrd;

	private float _timer;

	private bool _hasEntered;

	private float _completionSpeedMulti = 1f;

	private float _pickupableCooldownPeriod;

	private SpecialAbility_Lifter _playersLifter;

	public override float _pNormProgress
	{
		get
		{
			return (_completionDuration != 0f) ? (_timer / _completionDuration) : ((float)(base._pCompleted ? 1 : 0));
		}
	}

	public override bool _pLockGameWhileInProgress
	{
		get
		{
			return true;
		}
	}

	public static event Action<MinigameObjective_Waypoint> _onSpedUp;

	public event Action<MinigameObjective_Waypoint> _onEnter;

	private void Start()
	{
		_radiusSqrd = _radius * _radius;
		_completionSpeedMulti = 1f;
		for (int num = _spedUpByNegatedObstacles.Length - 1; num >= 0; num--)
		{
			if (VehicleController_Player._pInstance._pVehicle.NegatesObstacle(_spedUpByNegatedObstacles[num]))
			{
				_completionSpeedMulti = 2f;
				break;
			}
		}
		if (_markerPoint != null)
		{
			_markerPoint._pRadius = _radius * _radiusVisualMultiply;
		}
		if ((bool)GetComponent<Pickupable>() && VehicleController_Player._pInstance._pVehicle._pPickerUpper != null)
		{
			_pickupableCooldownPeriod = GetComponent<Pickupable>()._dropPickupCooldown;
		}
	}

	private void Update()
	{
		if (!base._pEnabled || base._pCompleted)
		{
			return;
		}
		if (!_hasEntered && MathHelper.DistXZSqrd(VehicleController_Player._pInstance.transform.position, base.transform.position) < _radiusSqrd && (!_onlyActivateIfCanSee || MinigameController._pInstance._pCamera._pCamera.IsPointInFrustrum(base.transform.position, 0.05f)) && (_pickupableCooldownPeriod == 0f || VehicleController_Player._pInstance._pVehicle._pPickerUpper._pTimeSinceLastDrop > _pickupableCooldownPeriod))
		{
			OnEnter();
		}
		if (_hasEntered)
		{
			_timer = Mathf.Min(_timer + Time.deltaTime * _completionSpeedMulti, _completionDuration);
			Progress();
			if (_timer == _completionDuration)
			{
				OnReachedTimer();
			}
		}
	}

	public override void Reset(bool toInitialState)
	{
		base.Reset(toInitialState);
		_timer = 0f;
		_hasEntered = false;
	}

	private void OnEnter()
	{
		_hasEntered = true;
		base._pPositionOnEntry = base.transform.position;
		if (_doCollectPlayersPickupable)
		{
			_playersLifter = VehicleController_Player._pInstance.GetComponentInChildren<SpecialAbility_Lifter>();
			if ((bool)_playersLifter)
			{
				Invoke("CollectPlayersPickupable_Lifter", 0.5f);
			}
			Grapple componentInChildren = VehicleController_Player._pInstance.GetComponentInChildren<Grapple>();
			if ((bool)componentInChildren)
			{
				CollectPlayersPickupable_Grapple(componentInChildren);
			}
		}
		if (this._onEnter != null)
		{
			this._onEnter(this);
		}
		if (_completionSpeedMulti > 1f && MinigameObjective_Waypoint._onSpedUp != null)
		{
			MinigameObjective_Waypoint._onSpedUp(this);
		}
		if (_markerPoint != null)
		{
			_markerPoint._pDoShow = false;
		}
		EmoticonSystem.OnPlayerEnterWaypoint(this);
	}

	private void CollectPlayersPickupable_Lifter()
	{
		if ((bool)_playersLifter && (bool)_playersLifter._pCurrentObject)
		{
			Pickupable pCurrentObject = _playersLifter._pCurrentObject;
			_playersLifter.Drop();
			pCurrentObject.TweenTo((!(_collectPickupablePoint != null)) ? base.transform : _collectPickupablePoint, delegate(Pickupable t)
			{
				t.enabled = false;
				t.gameObject.layer = LayerMask.NameToLayer("CollateralDynamic2");
				t.gameObject.AddComponent<TimedDestroyer>().StartTimer(10f, true, false, true);
			}, Easing.EaseType.EaseInOut, _completionDuration - 1f, 0f, false, (float t) => Vector3.up * 4f * (1f - (t * 2f - 1f) * (t * 2f - 1f)));
		}
	}

	private void CollectPlayersPickupable_Grapple(Grapple grapple)
	{
		if ((bool)grapple && (bool)grapple._pCurrentObject)
		{
			grapple.DropDelayed(delegate(Pickupable p)
			{
				p.gameObject.AddComponent<TimedDestroyer>().StartTimer(10f, true, false, true);
			}, base.transform.position.y);
		}
	}

	private void OnReachedTimer()
	{
		Complete();
		EmoticonSystem.OnWaypointComplete(this);
	}

	private void OnDrawGizmos()
	{
		GizmosPlus.drawCircle(base.transform.position, _radius);
	}
}
