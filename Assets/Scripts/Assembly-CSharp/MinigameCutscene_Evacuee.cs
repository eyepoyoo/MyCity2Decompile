using UnityEngine;

public class MinigameCutscene_Evacuee : MinigameCutscene
{
	public bool _visibleOnAwake;

	public Transform _carModel;

	private Pickupable _pickupable;

	private readonly Vector3 _camPosRelativeToPlayer = new Vector3(-15.2f, 0.73f, -12.95f);

	private readonly Quaternion _camRotRelativeToPlayer = Quaternion.Euler(20f, 53f, 0f);

	private bool _makeCameraTrackPlayer;

	private Vector3 trackPlayerPos;

	private float trackPlayerAngle;

	private bool _pVisible
	{
		set
		{
			_carModel.gameObject.SetActive(value);
		}
	}

	protected override void Awake()
	{
		base.Awake();
		_pickupable = GetComponent<Pickupable>();
		_pickupable.GetComponent<Pickupable>()._pMinigameObjective = base._waypoint;
		_camera.transform.parent = null;
		_pVisible = _visibleOnAwake;
	}

	protected override void OnWaypointReset(MinigameObjective objective, bool toInitialState)
	{
		base.OnWaypointReset(objective, toInitialState);
		_makeCameraTrackPlayer = false;
		if (toInitialState)
		{
			_pVisible = _visibleOnAwake;
		}
	}

	protected override void OnWaypointEnabled(MinigameObjective objective)
	{
		base.OnWaypointEnabled(objective);
		_pVisible = true;
	}

	protected override void OnWaypointEnter(MinigameObjective_Waypoint wp)
	{
		Vehicle_Air vehicle_Air = (Vehicle_Air)VehicleController_Player._pInstance._pVehicle;
		vehicle_Air._yOverride = base.transform.position.y + 5f + 4f + vehicle_Air._pCentreOffsetFromBottom;
		Invoke("GrappleUs", 1f);
		_makeCameraTrackPlayer = true;
		trackPlayerPos = base.transform.position + Vector3.up * 10f;
		trackPlayerAngle = VehicleController_Player._pInstance.transform.eulerAngles.y;
		base.OnWaypointEnter(wp);
	}

	protected override void OnWaypointComplete(MinigameObjective objective)
	{
		base.OnWaypointComplete(objective);
		_makeCameraTrackPlayer = false;
	}

	private void Update()
	{
		if (base._waypoint._pEnabled && _makeCameraTrackPlayer)
		{
			Quaternion quaternion = Quaternion.Euler(0f, trackPlayerAngle, 0f);
			_camera.transform.position = quaternion * _camPosRelativeToPlayer + trackPlayerPos;
			_camera.transform.rotation = quaternion * _camRotRelativeToPlayer;
		}
	}

	private void GrappleUs()
	{
		Grapple componentInChildren = VehicleController_Player._pInstance.GetComponentInChildren<Grapple>();
		if (!componentInChildren)
		{
			Debug.LogError("Grapple not found on player!");
		}
		else
		{
			componentInChildren.PickUp(_pickupable);
		}
	}
}
