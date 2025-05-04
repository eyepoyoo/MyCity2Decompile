using UnityEngine;

[RequireComponent(typeof(MinigameObjective_Waypoint))]
public class MinigameCutscene : MonoBehaviour
{
	public bool _freezePlayer = true;

	public Camera _camera;

	public static readonly BooleanStateRequestsSimple _pIsAnyActive = new BooleanStateRequestsSimple(false);

	protected bool _camEnabled = true;

	protected MinigameObjective_Waypoint _waypoint { get; private set; }

	protected virtual void Awake()
	{
		_waypoint = GetComponent<MinigameObjective_Waypoint>();
		_waypoint._onEnabled += OnWaypointEnabled;
		_waypoint._onDisabled += OnWaypointDisabled;
		_waypoint._onEnter += OnWaypointEnter;
		_waypoint._onProgress += OnWaypointProgress;
		_waypoint._onComplete += OnWaypointComplete;
		_waypoint._onReset += OnWaypointReset;
		if ((bool)_camera)
		{
			_camera.enabled = false;
		}
	}

	protected virtual void OnWaypointEnabled(MinigameObjective objective)
	{
	}

	protected virtual void OnWaypointDisabled(MinigameObjective objective)
	{
	}

	protected virtual void OnWaypointReset(MinigameObjective objective, bool toInitialState)
	{
	}

	protected virtual void OnWaypointEnter(MinigameObjective_Waypoint wp)
	{
		if (_freezePlayer)
		{
			FreezePlayer();
		}
		if ((bool)_camera && _camEnabled)
		{
			MinigameController._pInstance._pCamera._pOverride = _camera;
		}
		MinigameController._pInstance.AddHideDirectorArrowRequest(this);
		_pIsAnyActive.AddContraryStateRequest(this);
	}

	protected virtual void OnWaypointProgress(MinigameObjective objective, float progress)
	{
	}

	protected virtual void OnWaypointComplete(MinigameObjective objective)
	{
		if (_freezePlayer)
		{
			UnfreezePlayer();
		}
		if (MinigameController._pInstance._pCamera._pOverride == _camera)
		{
			MinigameController._pInstance._pCamera._pOverride = null;
		}
		MinigameController._pInstance.RemoveHideDirectorArrowRequest(this);
		_pIsAnyActive.RemoveContraryStateRequest(this);
	}

	private void FreezePlayer()
	{
		VehicleController_Player._pInstance._enabled = false;
		VehicleController_Player._pInstance._pVehicle.Brake(999f);
		if (VehicleController_Player._pInstance._pVehicle is Vehicle_Car)
		{
			Vehicle_Car vehicle_Car = (Vehicle_Car)VehicleController_Player._pInstance._pVehicle;
			vehicle_Car.RestoreWheelsForwardFriction();
			vehicle_Car.RestoreWheelsSidewaysFriction();
		}
	}

	private void UnfreezePlayer()
	{
		VehicleController_Player._pInstance._enabled = true;
		VehicleController_Player._pInstance._pVehicle.UnBrake();
	}

	private void OnDestroy()
	{
		_pIsAnyActive.RemoveContraryStateRequest(this);
	}
}
