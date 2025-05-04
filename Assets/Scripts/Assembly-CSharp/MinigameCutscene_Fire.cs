using UnityEngine;

public class MinigameCutscene_Fire : MinigameCutscene
{
	private class HoseSettings
	{
		public bool _forceIsInUse;

		public float _variationAmount;

		public bool _enableHeadingRotation;

		public Transform _lookAt;

		public void Backup(SpecialAbility_Hose hose)
		{
			_forceIsInUse = hose._forceIsInUse;
			_variationAmount = hose._variationAmount;
			_enableHeadingRotation = hose._enableHeadingRotation;
			_lookAt = hose._lookAt;
		}

		public void Restore(SpecialAbility_Hose hose)
		{
			hose._forceIsInUse = _forceIsInUse;
			hose._variationAmount = _variationAmount;
			hose._enableHeadingRotation = _enableHeadingRotation;
			hose._lookAt = _lookAt;
		}
	}

	public Fire _fire;

	public bool _startWithFireLit;

	private SpecialAbility_Hose _hose;

	private readonly HoseSettings _hoseBackup = new HoseSettings();

	protected override void Awake()
	{
		base.Awake();
		if (!_startWithFireLit)
		{
			_fire._pNormIntensity = 0f;
		}
	}

	private void Start()
	{
		_hose = VehicleController_Player._pInstance._pVehicle.GetComponentInChildren<SpecialAbility_Hose>();
	}

	protected override void OnWaypointEnabled(MinigameObjective objective)
	{
		base.OnWaypointEnabled(objective);
		_fire._pNormIntensity = 1f;
	}

	protected override void OnWaypointDisabled(MinigameObjective objective)
	{
		base.OnWaypointDisabled(objective);
	}

	protected override void OnWaypointEnter(MinigameObjective_Waypoint wp)
	{
		base.OnWaypointEnter(wp);
		if ((bool)_hose)
		{
			_hoseBackup.Backup(_hose);
			_hose._forceIsInUse = true;
			_hose._variationAmount = 1f;
			_hose._enableHeadingRotation = true;
			_hose._lookAt = ((!(_fire._centre != null)) ? _fire.transform : _fire._centre);
		}
	}

	protected override void OnWaypointProgress(MinigameObjective objective, float progress)
	{
		base.OnWaypointProgress(objective, progress);
		_fire._pNormIntensity = 1f - progress;
	}

	protected override void OnWaypointComplete(MinigameObjective wp)
	{
		base.OnWaypointComplete(wp);
		_fire._pNormIntensity = 0f;
		if ((bool)_hose)
		{
			_hoseBackup.Restore(_hose);
		}
	}
}
