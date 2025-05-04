using UnityEngine;

public class Ring : MonoBehaviour
{
	public MinigameObjective_Waypoint _waypoint;

	private void Awake()
	{
		_waypoint._onEnter += OnWaypointEnter;
	}

	private void OnWaypointEnter(MinigameObjective obj)
	{
		base.gameObject.SetActive(false);
	}
}
