using UnityEngine;

public class Waypoint : MonoBehaviour
{
	public const float RADIUS = 10f;

	public static readonly Color COLOUR = Color.white;

	public bool _includeLaneOffset;

	private void OnDrawGizmos()
	{
		Gizmos.color = COLOUR;
		Gizmos.DrawSphere(base.transform.position + Vector3.up * 0.1f, 0.5f);
		GizmosPlus.DrawSquare(base.transform.position + Vector3.up * 0.1f, 20f, Vector3.forward, Vector3.right);
	}
}
