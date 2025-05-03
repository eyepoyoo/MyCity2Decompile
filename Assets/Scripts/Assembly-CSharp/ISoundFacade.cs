using System;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundFacade
{
	bool _pMusicMuted { get; set; }

	void Initialise();

	void PlayMusic(string soundGroupID, float crossFadeTime, bool allowRetrigger);

	void FadeMusicOut(float fadeTime);

	AudioSource GetPlayingAudioSourceByTag(string tagName);

	void GetAllPlayingAudioSourcesByTag(string tagName, ref List<AudioSource> targetArray);

	AudioSource GetPlayingAudioSourceByClipName(string clipName);

	void GetAllPlayingAudioSourcesByClipName(string clipname, ref List<AudioSource> targetArray);

	void SetSFXChannelVolumeDB(string channelName, float db);

	void SetSFXChannelVolumeScalar(string channelName, float val);

	void SetSFXChannelMute(string channelName, bool val);

	float GetSFXChannelVolumeDB(string channelName);

	bool GetSFXChannelMute(string channelName);

	AudioSource PlayOneShotSFX(string soundGroupID, float delay, Action<SoundFacadeSoundEffectInfo> onCompletedAction, string tagName, string spawnBehaviour);

	AudioSource PlayOneShotSFX(string soundGroupID, Vector3 location, float delay, Action<SoundFacadeSoundEffectInfo> onCompletedAction, string tagName, string spawnBehaviour);

	AudioSource PlayOneShotSFX(string soundGroupID, Transform followTarget, float delay, Action<SoundFacadeSoundEffectInfo> onCompletedAction, string tagName, string spawnBehaviour);

	AudioSource PlayLoopingSFX(string soundGroupID, float delay, Action<SoundFacadeSoundEffectInfo> onCompletedAction, string tagName, string spawnBehaviour);

	AudioSource PlayLoopingSFX(string soundGroupID, Vector3 location, float delay, Action<SoundFacadeSoundEffectInfo> onCompletedAction, string tagName, string spawnBehaviour);

	AudioSource PlayLoopingSFX(string soundGroupID, Transform followTarget, float delay, Action<SoundFacadeSoundEffectInfo> onCompletedAction, string tagName, string spawnBehaviour);

	void StopAllLoopingAudioByGroupName(string soundGroupID);

	void StopAllLoopingAudioByTag(string tagName);

	void StopAllLoopingAudioByTrackedTransform(Transform followTarget);

	void StopLoopingAudioByAudioSource(AudioSource source);

	void StopAllLoopingAudio();

	void MakeAudioListenerTrackTransform(Transform followTarget);
}
