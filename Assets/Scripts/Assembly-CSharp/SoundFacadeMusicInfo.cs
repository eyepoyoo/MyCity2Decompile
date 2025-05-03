using System;
using UnityEngine;

[Serializable]
public class SoundFacadeMusicInfo
{
	public string musicGroupName;

	public AudioClip[] audioClips;

	public string[] resourceLoadedAudioClips;

	[Range(0f, 1f)]
	public float volume = 1f;

	[Range(-1f, 1f)]
	public float pan;

	public AudioClip GetClip()
	{
		if (audioClips == null && resourceLoadedAudioClips == null)
		{
			return null;
		}
		int num = 0;
		int num2 = 0;
		if (audioClips != null)
		{
			num = audioClips.Length;
		}
		if (resourceLoadedAudioClips != null)
		{
			num2 = resourceLoadedAudioClips.Length;
		}
		if (num == 0 && num2 == 0)
		{
			return null;
		}
		int num3 = 0;
		if (num > 0)
		{
			num3 = UnityEngine.Random.Range(0, num);
			return audioClips[num3];
		}
		num3 = UnityEngine.Random.Range(0, num2);
		return Resources.Load(resourceLoadedAudioClips[num3]) as AudioClip;
	}
}
