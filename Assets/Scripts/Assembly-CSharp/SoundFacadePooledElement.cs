using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class SoundFacadePooledElement : MonoBehaviour
{
	private AudioSource _audioSource;

	private SoundFacadeSoundEffectInfo _soundEffectInfo;

	private Action<SoundFacadePooledElement> _onCompleted;

	private Action<SoundFacadeSoundEffectInfo> _onCompletedBespoke;

	private string _tag;

	private float _startTime;

	private bool _startedPlaying;

	private Transform _trackableTransform;

	private SoundSpawnBehaviourBase _spawnBehaviour;

	public AudioSource _pAudioSource
	{
		get
		{
			return _audioSource;
		}
		set
		{
			_audioSource = value;
		}
	}

	public string _pTag
	{
		get
		{
			return _tag;
		}
		set
		{
			_tag = value;
		}
	}

	public int _pPriority
	{
		get
		{
			return _soundEffectInfo.poolPriority;
		}
	}

	public SoundFacadeSoundEffectInfo _pSoundEffectInfo
	{
		get
		{
			return _soundEffectInfo;
		}
	}

	public Transform _pTrackableTransform
	{
		get
		{
			return _trackableTransform;
		}
	}

	public void OnHijack()
	{
		_audioSource.Stop();
		_startedPlaying = false;
		_onCompletedBespoke = null;
	}

	public void UpdateElement()
	{
		float pRealTime = SoundFacade._pInstance._pRealTime;
		if (pRealTime < _startTime)
		{
			return;
		}
		if (!_startedPlaying)
		{
			_audioSource.volume = _soundEffectInfo.volume;
			_audioSource.panStereo = _soundEffectInfo.pan;
			_audioSource.spatialBlend = _soundEffectInfo.space;
			_audioSource.dopplerLevel = _soundEffectInfo.dopplerLevel;
			_audioSource.reverbZoneMix = _soundEffectInfo.reverbZoneMix;
			_audioSource.spread = _soundEffectInfo.spread;
			_audioSource.rolloffMode = _soundEffectInfo.rolloffMode;
			_audioSource.pitch = _soundEffectInfo.pitch;
			_audioSource.minDistance = _soundEffectInfo.minDistance;
			_audioSource.maxDistance = _soundEffectInfo.maxDistance;
			_audioSource.mute = SoundFacade._pInstance.GetSFXChannelMute(_audioSource.outputAudioMixerGroup);
			_startedPlaying = true;
			_audioSource.Play();
			_spawnBehaviour.OnSoundPlay(_audioSource);
		}
		else if (!_audioSource.isPlaying)
		{
			if (_onCompletedBespoke != null)
			{
				_onCompletedBespoke(_soundEffectInfo);
			}
			_onCompleted(this);
		}
	}

	public void Stop()
	{
		_audioSource.Stop();
	}

	public void LateUpdateElement()
	{
		if (_trackableTransform != null)
		{
			_audioSource.transform.position = _trackableTransform.position;
		}
	}

	public AudioSource LoadAudio(SoundFacadeSoundEffectInfo src, float delay, Action<SoundFacadePooledElement> onCompleted, Action<SoundFacadeSoundEffectInfo> onCompletedBespoke, string tagName, SoundSpawnBehaviourBase soundSpawnBehaviour, [Optional] Vector3 position, Transform tf = null, bool isModified = false, bool isLoop = false)
	{
		_audioSource.clip = src.GetClip();
		_audioSource.outputAudioMixerGroup = src.mixer;
		_audioSource.loop = isLoop;
		_spawnBehaviour = soundSpawnBehaviour;
		_tag = tagName;
		_onCompletedBespoke = onCompletedBespoke;
		_onCompleted = onCompleted;
		_soundEffectInfo = src;
		_audioSource.transform.position = position;
		_trackableTransform = tf;
		if (_trackableTransform != null)
		{
			_audioSource.transform.position = _trackableTransform.position;
		}
		if (!isModified || soundSpawnBehaviour._pRetrigger)
		{
			_startTime = SoundFacade._pInstance._pRealTime + delay;
			_startedPlaying = false;
		}
		if (delay == 0f)
		{
			if (!isModified || soundSpawnBehaviour._pRetrigger)
			{
				_startedPlaying = true;
				_audioSource.volume = src.volume;
				_audioSource.panStereo = src.pan;
				_audioSource.spatialBlend = src.space;
				_audioSource.dopplerLevel = src.dopplerLevel;
				_audioSource.spread = src.spread;
				_audioSource.reverbZoneMix = src.reverbZoneMix;
				_audioSource.rolloffMode = src.rolloffMode;
				_audioSource.minDistance = src.minDistance;
				_audioSource.maxDistance = src.maxDistance;
				_audioSource.pitch = src.pitch;
				_audioSource.mute = SoundFacade._pInstance.GetSFXChannelMute(_audioSource.outputAudioMixerGroup);
				_audioSource.Play();
				soundSpawnBehaviour.OnSoundPlay(_audioSource);
			}
			else
			{
				soundSpawnBehaviour.OnSoundContinue(_audioSource);
			}
		}
		else if (isModified && !soundSpawnBehaviour._pRetrigger)
		{
			soundSpawnBehaviour.OnSoundContinue(_audioSource);
		}
		return _audioSource;
	}
}
