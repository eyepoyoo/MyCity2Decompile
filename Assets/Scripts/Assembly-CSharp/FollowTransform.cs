using UnityEngine;

public class FollowTransform : MonoBehaviour
{
	public Vector3 offset;

	public Transform followTarget;

	private void LateUpdate()
	{
		if (!(followTarget == null))
		{
			base.transform.position = followTarget.position + offset;
		}
	}
}
