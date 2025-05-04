using System;
using System.Collections.Generic;
using UnityEngine;

public class MinigameMetrics
{
	private int _playerCollateralDestroyed;

	private int _numRampsUsed;

	private int _numFallingHazardsHitPlayer;

	private int _numPlayerCollisions;

	private int _numHiddenAreasFound;

	private BooleanStateRequestsSimple _isInLava = new BooleanStateRequestsSimple(false);

	private int _numItemsDroppedByHazard;

	private float _timeElapsed;

	private float _timeInLava;

	private float _lastVehicleCollisionTime = float.NegativeInfinity;

	public MinigameMetrics()
	{
		Collateral._onCollateralDestroyed += OnCollateralDestroyed;
		PlayerTrigger._onEnter += OnPlayerTriggerEnter;
		PlayerTrigger._onExit += OnPlayerTriggerExit;
		FallingHazard._onHitPlayer = (Action<FallingHazard>)Delegate.Combine(FallingHazard._onHitPlayer, new Action<FallingHazard>(OnFallingHazardHitPlayer));
		FallingHazard._onMadePlayerDropPickup = (Action<FallingHazard>)Delegate.Combine(FallingHazard._onMadePlayerDropPickup, new Action<FallingHazard>(OnItemDroppedByFallingHazard));
		AffectorArea_LavaJet._onMadePlayerDropPickup = (Action<AffectorArea_LavaJet>)Delegate.Combine(AffectorArea_LavaJet._onMadePlayerDropPickup, new Action<AffectorArea_LavaJet>(OnItemDroppedByLavaJet));
		VehicleController_Player._pInstance._pVehicle._onHitVehicle += OnPlayerHitVehicle;
		MinigameMessagingController._pInstance._onHiddenAreaDiscovered += OnHiddenAreaDiscovered;
	}

	private void OnCollateralDestroyed(Collateral collateral, Collider collider)
	{
		if (VehicleController_Player.IsPlayer(collider))
		{
			_playerCollateralDestroyed++;
		}
	}

	private void OnPlayerTriggerEnter(PlayerTrigger area)
	{
		switch (area._type)
		{
		case PlayerTrigger.EType.Ramp:
			_numRampsUsed++;
			break;
		case PlayerTrigger.EType.LavaThrough:
			_isInLava.AddContraryStateRequest(area);
			break;
		}
	}

	private void OnPlayerTriggerExit(PlayerTrigger area)
	{
		PlayerTrigger.EType type = area._type;
		if (type == PlayerTrigger.EType.LavaThrough)
		{
			_isInLava.RemoveContraryStateRequest(area);
		}
	}

	private void OnFallingHazardHitPlayer(FallingHazard fh)
	{
		_numFallingHazardsHitPlayer++;
	}

	private void OnItemDroppedByFallingHazard(FallingHazard fh)
	{
		_numItemsDroppedByHazard++;
	}

	private void OnItemDroppedByLavaJet(AffectorArea_LavaJet lj)
	{
		_numItemsDroppedByHazard++;
	}

	private void OnPlayerHitVehicle(Vehicle veh, Collision col)
	{
		if (!(Time.time < _lastVehicleCollisionTime + 1f))
		{
			_lastVehicleCollisionTime = Time.time;
			_numPlayerCollisions++;
		}
	}

	private void OnHiddenAreaDiscovered(PlayerTrigger area)
	{
		_numHiddenAreasFound++;
	}

	public void Update(float dt)
	{
		_timeElapsed += dt;
		if ((bool)_isInLava)
		{
			_timeInLava += dt;
		}
	}

	public void LogAnalytics()
	{
		if (!(Facades<TrackingFacade>.Instance == null))
		{
			string text = MinigameManager._pInstance._pCurrentMinigameType.ToString();
			Facades<TrackingFacade>.Instance.StopTimerMetric("MiniGame Start: " + text, string.Empty);
			Facades<TrackingFacade>.Instance.LogParameterMetric("MiniGame Completed: " + text, new Dictionary<string, string> { 
			{
				"Vehicle",
				MinigameManager._pInstance._pCurrentVehicleTypeForMinigame.ToString()
			} });
			Facades<TrackingFacade>.Instance.LogProgress("Minigame_" + MinigameManager._pInstance._pCurrentMinigameType.ToString() + "_end");
			Facades<TrackingFacade>.Instance.LogParameterMetric("MiniGame: " + text, GetMetricsDict());
			if (MinigameController._pInstance._pTimeBonus < 0)
			{
				Facades<TrackingFacade>.Instance.LogMetric(MinigameManager._pInstance._pCurrentMinigameType.ToString(), "MiniGame TimeOut");
			}
		}
	}

	private Dictionary<string, string> GetMetricsDict()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("Time To Complete", (Mathf.Ceil(_timeElapsed / 10f) * 10f).ToString());
		dictionary.Add("Studs", MinigameController._pInstance._pStudsCollected.ToString());
		dictionary.Add("Hidden Areas Found", _numHiddenAreasFound.ToString());
		dictionary.Add("Ramps Used", _numRampsUsed.ToString());
		dictionary.Add("Objects Smashed", _playerCollateralDestroyed.ToString());
		dictionary.Add("Hit By Falling Rocks", _numFallingHazardsHitPlayer.ToString());
		dictionary.Add("Boulders Dropped", _numItemsDroppedByHazard.ToString());
		dictionary.Add("Time In Lava", Mathf.Floor(_timeInLava).ToString());
		dictionary.Add("Collisions", _numPlayerCollisions.ToString());
		return dictionary;
	}

	public void Destroy()
	{
		Collateral._onCollateralDestroyed -= OnCollateralDestroyed;
		PlayerTrigger._onEnter -= OnPlayerTriggerEnter;
		PlayerTrigger._onExit -= OnPlayerTriggerExit;
		FallingHazard._onHitPlayer = (Action<FallingHazard>)Delegate.Remove(FallingHazard._onHitPlayer, new Action<FallingHazard>(OnFallingHazardHitPlayer));
		FallingHazard._onMadePlayerDropPickup = (Action<FallingHazard>)Delegate.Remove(FallingHazard._onMadePlayerDropPickup, new Action<FallingHazard>(OnItemDroppedByFallingHazard));
		AffectorArea_LavaJet._onMadePlayerDropPickup = (Action<AffectorArea_LavaJet>)Delegate.Remove(AffectorArea_LavaJet._onMadePlayerDropPickup, new Action<AffectorArea_LavaJet>(OnItemDroppedByLavaJet));
	}
}
