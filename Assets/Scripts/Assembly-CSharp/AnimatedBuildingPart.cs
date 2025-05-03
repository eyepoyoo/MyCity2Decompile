using UnityEngine;

public class AnimatedBuildingPart : MonoBehaviour
{
	[HideInInspector]
	public GameObject buildingPiece;

	public float inAnimationDelay;

	public float outAnimationDelay;

	private void Awake()
	{
		buildingPiece = base.gameObject;
	}
}
