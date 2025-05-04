using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class SoundFacadeSoundEffectInfo
{
	[Header("Sound Info")]
	public string soundGroupName;

	public AudioClip[] audioClips;

	public AudioMixerGroup mixer;

	public int poolPriority;

	[Range(0f, 1f)]
	[Header("General Settings")]
	public float volume = 1f;

	public float pitch = 1f;

	[Range(-1f, 1f)]
	public float pan;

	[Range(0f, 1f)]
	public float space;

	[Range(0f, 1.1f)]
	[Header("3D Sound Settings")]
	public float reverbZoneMix = 1f;

	[Range(0f, 5f)]
	public float dopplerLevel;

	public AudioRolloffMode rolloffMode;

	public float minDistance = 1f;

	[Range(0f, 360f)]
	public float spread;

	public float maxDistance = 500f;

	public AudioClip GetClip()
	{
		if (audioClips == null)
		{
			return null;
		}
		int num = audioClips.Length;
		if (num == 0)
		{
			return null;
		}
		int num2 = UnityEngine.Random.Range(0, num);
		return audioClips[num2];
	}
}
