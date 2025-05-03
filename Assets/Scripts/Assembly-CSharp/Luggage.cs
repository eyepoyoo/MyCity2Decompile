using UnityEngine;

public class Luggage : MonoBehaviour
{
	public MinigameObjective_Waypoint _waypoint;

	private void Awake()
	{
		base.gameObject.SetActive(false);
		_waypoint._onEnabled += OnWaypointEnabled;
		_waypoint._onDisabled += OnWaypointDisabled;
	}

	private void OnWaypointEnabled(MinigameObjective obj)
	{
		base.gameObject.SetActive(true);
	}

	private void OnWaypointDisabled(MinigameObjective obj)
	{
		base.gameObject.SetActive(false);
	}
}
