using UnityEngine;

public class AnimateOnEnable : MonoBehaviour
{
	public AnimationClip[] onEnableAnims;

	public void Awake()
	{
		OnEnable();
	}

	public void OnEnable()
	{
		if (onEnableAnims != null && onEnableAnims.Length != 0)
		{
			if (GetComponent<Animation>() == null)
			{
				base.gameObject.AddComponent<Animation>();
			}
			AnimationClip animationClip = onEnableAnims[Random.Range(0, onEnableAnims.Length)];
			if (GetComponent<Animation>().GetClip(animationClip.name) == null)
			{
				GetComponent<Animation>().AddClip(animationClip, animationClip.name);
			}
			AnimationState animationState = GetComponent<Animation>()[animationClip.name];
			animationState.time = 0f;
			GetComponent<Animation>().Stop();
			GetComponent<Animation>().clip = animationState.clip;
			GetComponent<Animation>().Play();
		}
	}
}
