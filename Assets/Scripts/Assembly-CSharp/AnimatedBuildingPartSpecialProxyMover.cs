using UnityEngine;

public class AnimatedBuildingPartSpecialProxyMover : MonoBehaviour
{
	public AnimatedBuildingPart _basePart;

	public Transform _referenceTransform;

	private void LateUpdate()
	{
		Vector3 position = _referenceTransform.transform.position;
		position.y += _basePart.transform.position.y;
		base.transform.position = position;
		base.transform.rotation = _referenceTransform.rotation;
	}
}
