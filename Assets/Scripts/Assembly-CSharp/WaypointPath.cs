using System;
using UnityEngine;

public class WaypointPath : MonoBehaviour
{
	[Serializable]
	public class Node
	{
		public Waypoint _waypoint;

		public Waypoint[] _nextWaypoints;
	}

	public bool _alwaysDrawNetwork;

	public Node[] _nodes;

	public Waypoint GetNextWaypoint(Waypoint waypoint, Waypoint exclude = null)
	{
		Node node = Array.Find(_nodes, (Node n) => n._waypoint == waypoint);
		if (node == null)
		{
			Debug.LogError(string.Concat("Waypoint ", waypoint, " is not a node in this path!"), waypoint);
			return null;
		}
		if (node._nextWaypoints.Length == 1 && node._nextWaypoints[0] == exclude)
		{
			Debug.LogWarning("The only next waypoint option for this node is the same as 'exclude'! selecting anyway", exclude);
			return exclude;
		}
		Waypoint random;
		do
		{
			random = node._nextWaypoints.GetRandom();
		}
		while (random == exclude && exclude != null);
		return random;
	}

	private void OnDrawGizmosSelected()
	{
		if (!_alwaysDrawNetwork)
		{
			DrawNetwork();
		}
	}

	private void OnDrawGizmos()
	{
		if (_alwaysDrawNetwork)
		{
			DrawNetwork();
		}
	}

	private void DrawNetwork()
	{
		if (_nodes == null || _nodes.Length == 0)
		{
			return;
		}
		Gizmos.color = Waypoint.COLOUR;
		for (int i = 0; i < _nodes.Length; i++)
		{
			for (int num = _nodes[i]._nextWaypoints.Length - 1; num >= 0; num--)
			{
				if (!(_nodes[i]._nextWaypoints[num] == null))
				{
					GizmosPlus.DrawArrowThin(Vector3.up * 0.1f + _nodes[i]._waypoint.transform.position, Vector3.up * 0.1f + _nodes[i]._nextWaypoints[num].transform.position, 5f, 1f, false);
				}
			}
		}
	}

	public Waypoint GetClosestWaypoint(Vector3 pos)
	{
		float num = float.PositiveInfinity;
		Waypoint result = null;
		for (int num2 = _nodes.Length - 1; num2 >= 0; num2--)
		{
			float sqrMagnitude = (pos - _nodes[num2]._waypoint.transform.position).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				num = sqrMagnitude;
				result = _nodes[num2]._waypoint;
			}
		}
		return result;
	}
}
