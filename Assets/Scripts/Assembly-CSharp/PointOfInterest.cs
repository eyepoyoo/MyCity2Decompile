using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
	public float activeDistance = 20f;

	public float transitionTime = 2f;

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(base.transform.position, activeDistance);
	}
}
