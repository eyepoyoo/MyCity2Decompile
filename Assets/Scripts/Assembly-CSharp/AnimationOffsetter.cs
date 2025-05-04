using UnityEngine;

public class AnimationOffsetter : MonoBehaviour
{
	public float offset;

	public float timescale = 1f;

	private void Start()
	{
		if (GetComponent<Animation>() == null)
		{
			return;
		}
		foreach (AnimationState item in GetComponent<Animation>())
		{
			item.normalizedTime = offset;
			item.speed = timescale;
		}
	}
}
