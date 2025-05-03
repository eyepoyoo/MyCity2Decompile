using UnityEngine;

[RequireComponent(typeof(VehicleController_RobberTyre))]
public class MinigameCutscene_Tyre : MinigameCutscene
{
	public SkyHook _skyHookPrefab;

	private VehicleController_RobberTyre _vehicleController;

	private void Start()
	{
		_vehicleController = GetComponent<VehicleController_RobberTyre>();
	}

	protected override void OnWaypointEnter(MinigameObjective_Waypoint wp)
	{
		base.OnWaypointEnter(wp);
		_vehicleController._pVehicle._isInSpotlight = true;
		SkyHook skyHook = FastPoolManager.GetPool(_skyHookPrefab).FastInstantiate<SkyHook>();
		skyHook.Hook(_vehicleController._pVehicle._pRigidbody, true);
	}

	protected override void OnWaypointComplete(MinigameObjective wp)
	{
		base.OnWaypointComplete(wp);
		base.gameObject.SetActive(false);
	}
}
