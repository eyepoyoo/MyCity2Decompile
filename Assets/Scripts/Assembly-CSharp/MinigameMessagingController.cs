using System;
using UnityEngine;

public class MinigameMessagingController : MonoBehaviour
{
	private const float MUD_OVER_DIST = 10f;

	private const float MUD_OVER_DIST_SQRD = 100f;

	private const float AIR_DISTANCE_THRESHOLD = 10f;

	private const int MIN_STUD_COMBO = 2;

	private const float STUD_COMBO_TIMEOUT = 1f;

	private Vehicle _player;

	private float _airDistance;

	private Vector3 _prevUserPos;

	private bool _wasGrounded;

	private Vector3 _leftGroundPos;

	private bool _waitingForRampJet;

	private bool _hasHoseHitPrimaryObjective;

	private bool _hasSpotlightHitVehicle;

	private float _lastStudCollectTime = float.NegativeInfinity;

	private int _currStudCombo;

	private bool _waitingForMudOver;

	private Vector2 _mudOverStartPos;

	private bool _doCheckForRamp;

	private bool _doCheckForMudThrough;

	private bool _doCheckForBridgeUnder;

	private bool _doCheckForBridgeOver;

	private bool _doCheckForBoxes;

	private bool _doCheckForTraffic;

	private bool _doCheckForHose;

	private bool _doCheckForSpotlight;

	private bool _doCheckForMudOver;

	private bool _doCheckForBoxesOver;

	private bool _doCheckForTurbulence;

	private bool _doCheckForLavaThrough;

	private float _lastHoseHitTime = float.NegativeInfinity;

	public static MinigameMessagingController _pInstance { get; private set; }

	public event Action<int> _onStudComboStarted;

	public event Action<int> _onStudComboContinued;

	public event Action<int> _onStudComboEnded;

	public event Action<float> _onAirBonus;

	public event Action<PlayerTrigger> _onHiddenAreaDiscovered;

	public event Action<VehiclePart.EObstacleToNegate, VehiclePart> _onOvercameObstacle;

	private void Awake()
	{
		_pInstance = this;
		if (!VehicleController_Player._pInstance)
		{
			Debug.LogError("Couldn't find player!");
			return;
		}
		_player = VehicleController_Player._pInstance._pVehicle;
		_prevUserPos = _player.transform.position;
		_doCheckForMudThrough = _player.NegatesObstacle(VehiclePart.EObstacleToNegate.MudThrough);
		_doCheckForRamp = _player.NegatesObstacle(VehiclePart.EObstacleToNegate.Ramp) && _player._specialAbility is SpecialAbility_Jet;
		_doCheckForBridgeUnder = _player.NegatesObstacle(VehiclePart.EObstacleToNegate.Under);
		_doCheckForBridgeOver = _player.NegatesObstacle(VehiclePart.EObstacleToNegate.Over);
		_doCheckForMudOver = _player.NegatesObstacle(VehiclePart.EObstacleToNegate.Over);
		_doCheckForBoxesOver = _player.NegatesObstacle(VehiclePart.EObstacleToNegate.Over);
		_doCheckForBoxes = _player.NegatesObstacle(VehiclePart.EObstacleToNegate.Boxes);
		_doCheckForTraffic = _player.NegatesObstacle(VehiclePart.EObstacleToNegate.Traffic);
		_doCheckForHose = _player.NegatesObstacle(VehiclePart.EObstacleToNegate.Hose);
		_doCheckForSpotlight = _player.NegatesObstacle(VehiclePart.EObstacleToNegate.Spotlight);
		_doCheckForTurbulence = _player.NegatesObstacle(VehiclePart.EObstacleToNegate.Turbulence);
		_doCheckForLavaThrough = _player.NegatesObstacle(VehiclePart.EObstacleToNegate.LavaThrough);
		Pickup._onPickupCollected += OnPickupCollected;
		PlayerTrigger._onDiscover += OnPlayerTriggerAreaDiscover;
		PlayerTrigger._onDiscover += EmoticonSystem.OnPlayerTriggerAreaDiscover;
		PlayerTrigger._onEnter += OnPlayerTriggerEnter;
		PlayerTrigger._onExit += OnPlayerTriggerExit;
		MinigameObjective_Waypoint._onSpedUp += OnWaypointSpedUp;
		if ((bool)MinigameController._pInstance._npcController)
		{
			MinigameController._pInstance._npcController._onNpcAvoidedSiren += OnNpcAvoidedSiren;
		}
		if (_doCheckForHose)
		{
			_player.GetComponentInChildren<SpecialAbility_Hose>()._onHit += OnHoseHitCollider;
		}
		if (_doCheckForSpotlight)
		{
			_player.GetComponentInChildren<SpecialAbility_Spotlight>()._onHitVehicle += OnSpotlightHitVehicle;
		}
		if (ScreenMinigameHUD._pInstance != null)
		{
			ScreenMinigameHUD._pInstance.RequestMessageSystemRegister();
		}
	}

	private void Update()
	{
		if (_player._pIsGrounded)
		{
			if (!_wasGrounded && _airDistance >= 10f)
			{
				if (_player is Vehicle_Boat)
				{
					Vehicle_Boat vehicle_Boat = _player as Vehicle_Boat;
					vehicle_Boat.DisplaySplash();
				}
				OnAirBonus(_airDistance);
			}
			_airDistance = 0f;
		}
		else
		{
			_airDistance += MathHelper.DistXZ(_prevUserPos, _player.transform.position);
		}
		if (_waitingForRampJet && _player._specialAbility._pIsInUse)
		{
			OnOvercameObstacle(VehiclePart.EObstacleToNegate.Ramp);
			_waitingForRampJet = false;
		}
		if (_waitingForMudOver && !_player._pIsGrounded && MathHelper.DistXZSqrd(_mudOverStartPos, _player.transform.position) > 100f)
		{
			_waitingForMudOver = false;
			OnOvercameObstacle(VehiclePart.EObstacleToNegate.Over);
		}
		if (_currStudCombo >= 2 && Time.time > _lastStudCollectTime + 1f)
		{
			OnStudComboEnded();
			_currStudCombo = 0;
		}
		_prevUserPos = _player.transform.position;
		_wasGrounded = _player._pIsGrounded;
	}

	private void OnPickupCollected(Pickup pickup)
	{
		if (pickup is Pickup_Stud)
		{
			OnStudCollecteded((Pickup_Stud)pickup);
		}
	}

	private void OnPlayerTriggerAreaDiscover(PlayerTrigger area)
	{
		switch (area._type)
		{
		case PlayerTrigger.EType.Hidden:
			OnHiddenAreaDiscovered(area);
			break;
		case PlayerTrigger.EType.BoxPileThrough:
			if (_doCheckForBoxes)
			{
				OnOvercameObstacle(VehiclePart.EObstacleToNegate.Boxes);
			}
			break;
		case PlayerTrigger.EType.BoxPileOver:
			if (_doCheckForBoxesOver && !_player._pIsGrounded)
			{
				OnOvercameObstacle(VehiclePart.EObstacleToNegate.Over);
			}
			break;
		}
	}

	private void OnPlayerTriggerEnter(PlayerTrigger area)
	{
		switch (area._type)
		{
		case PlayerTrigger.EType.MudThrough:
			if (_doCheckForMudThrough)
			{
				OnOvercameObstacle(VehiclePart.EObstacleToNegate.MudThrough);
			}
			break;
		case PlayerTrigger.EType.Ramp:
			if (_doCheckForRamp)
			{
				_waitingForRampJet = true;
			}
			break;
		case PlayerTrigger.EType.MudOver:
			if (_doCheckForMudOver && !_player._pIsGrounded)
			{
				_waitingForMudOver = true;
				_mudOverStartPos = _player.transform.position;
			}
			break;
		case PlayerTrigger.EType.TurbulenceThrough:
			if (_doCheckForTurbulence)
			{
				OnOvercameObstacle(VehiclePart.EObstacleToNegate.Turbulence);
			}
			break;
		case PlayerTrigger.EType.LavaThrough:
			if (_doCheckForLavaThrough)
			{
				OnOvercameObstacle(VehiclePart.EObstacleToNegate.LavaThrough);
			}
			break;
		case PlayerTrigger.EType.BridgeUnder:
		case PlayerTrigger.EType.BridgeOver:
		case PlayerTrigger.EType.BoxPileThrough:
		case PlayerTrigger.EType.BoxPileOver:
			break;
		}
	}

	private void OnPlayerTriggerExit(PlayerTrigger area)
	{
		switch (area._type)
		{
		case PlayerTrigger.EType.Ramp:
			if (_waitingForRampJet)
			{
				_waitingForRampJet = false;
			}
			break;
		case PlayerTrigger.EType.MudOver:
			if (_waitingForMudOver)
			{
				_waitingForMudOver = false;
			}
			break;
		case PlayerTrigger.EType.BridgeOver:
			if (_doCheckForBridgeOver)
			{
				OnOvercameObstacle(VehiclePart.EObstacleToNegate.Over);
			}
			break;
		case PlayerTrigger.EType.BridgeUnder:
			if (_doCheckForBridgeUnder)
			{
				OnOvercameObstacle(VehiclePart.EObstacleToNegate.Under);
			}
			break;
		case PlayerTrigger.EType.BoxPileThrough:
			break;
		}
	}

	private void OnWaypointSpedUp(MinigameObjective_Waypoint wp)
	{
		if (wp._spedUpByNegatedObstacles != null && wp._spedUpByNegatedObstacles.Length != 0)
		{
			OnOvercameObstacle(wp._spedUpByNegatedObstacles[0]);
		}
	}

	private void OnNpcAvoidedSiren()
	{
		OnOvercameObstacle(VehiclePart.EObstacleToNegate.Traffic);
	}

	private void OnHoseHitCollider(Collider collider)
	{
		if ((bool)MinigameController._pInstance._pMinigame._pPrimaryObjective && collider.transform.IsChildOf(MinigameController._pInstance._pMinigame._pPrimaryObjective.transform) && Time.time - _lastHoseHitTime > 1f)
		{
			_hasHoseHitPrimaryObjective = true;
			OnOvercameObstacle(VehiclePart.EObstacleToNegate.Hose);
			MinigameObjective_Destroyable_Car componentInParent = collider.GetComponentInParent<MinigameObjective_Destroyable_Car>();
			if ((bool)componentInParent)
			{
				EmoticonSystem.OnHoseHitCrook(componentInParent.facePos);
				componentInParent.ResetTauntTimer();
			}
		}
		_lastHoseHitTime = Time.time;
	}

	private void OnSpotlightHitVehicle(Vehicle vehicle)
	{
		if (_hasSpotlightHitVehicle)
		{
		}
		_hasSpotlightHitVehicle = true;
		OnOvercameObstacle(VehiclePart.EObstacleToNegate.Spotlight);
	}

	private void OnAirBonus(float distance)
	{
		if (this._onAirBonus != null)
		{
			this._onAirBonus(distance);
		}
		EmoticonSystem.OnPlayerAirTime();
	}

	private void OnStudCollecteded(Pickup_Stud stud)
	{
		_currStudCombo++;
		_lastStudCollectTime = Time.time;
		if (_currStudCombo == 2)
		{
			OnStudComboStarted();
		}
		if (_currStudCombo > 2)
		{
			OnStudComboContinued();
		}
	}

	private void OnStudComboStarted()
	{
		if (this._onStudComboStarted != null)
		{
			this._onStudComboStarted(2);
		}
	}

	private void OnStudComboContinued()
	{
		if (this._onStudComboContinued != null)
		{
			this._onStudComboContinued(_currStudCombo);
		}
	}

	private void OnStudComboEnded()
	{
		if (this._onStudComboEnded != null)
		{
			this._onStudComboEnded(_currStudCombo);
		}
	}

	private void OnHiddenAreaDiscovered(PlayerTrigger area)
	{
		if (this._onHiddenAreaDiscovered != null)
		{
			this._onHiddenAreaDiscovered(area);
		}
	}

	private void OnOvercameObstacle(VehiclePart.EObstacleToNegate obstacle)
	{
		if (this._onOvercameObstacle != null)
		{
			this._onOvercameObstacle(obstacle, VehicleController_Player._pInstance._pVehicle.GetVehiclePartThatNegatesObstacle(obstacle));
		}
	}

	private void OnDestroy()
	{
		Pickup._onPickupCollected -= OnPickupCollected;
		PlayerTrigger._onDiscover -= OnPlayerTriggerAreaDiscover;
		PlayerTrigger._onDiscover -= EmoticonSystem.OnPlayerTriggerAreaDiscover;
		PlayerTrigger._onEnter -= OnPlayerTriggerEnter;
		PlayerTrigger._onExit -= OnPlayerTriggerExit;
		MinigameObjective_Waypoint._onSpedUp -= OnWaypointSpedUp;
		_pInstance = null;
	}
}
