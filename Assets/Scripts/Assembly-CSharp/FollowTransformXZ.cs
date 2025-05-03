using UnityEngine;

public class FollowTransformXZ : MonoBehaviour
{
	public Transform _other;

	private void LateUpdate()
	{
		if (!(_other == null))
		{
			Vector3 position = _other.transform.position;
			position.y = base.transform.position.y;
			base.transform.position = position;
		}
	}
}
