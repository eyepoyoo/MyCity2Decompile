using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class SoundFacadeChannel
{
	public AudioMixerGroup mixer;

	public string mixerVolumeControlParameterName;

	private bool _muted;

	public bool _pMuted
	{
		get
		{
			return _muted;
		}
		set
		{
			_muted = value;
		}
	}

	public void SetVolumeDb(float db)
	{
		mixer.audioMixer.SetFloat(mixerVolumeControlParameterName, db);
	}

	public void SetVolumeScalar(float val)
	{
		mixer.audioMixer.SetFloat(mixerVolumeControlParameterName, Mathf.Lerp(-80f, 0f, Mathf.Clamp01(val)));
	}

	public float GetVolumeDb()
	{
		float value = 0f;
		mixer.audioMixer.GetFloat(mixerVolumeControlParameterName, out value);
		return value;
	}
}
