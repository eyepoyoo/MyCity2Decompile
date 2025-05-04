using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Audio;

public class SoundFacade : InitialisationObject, ISoundFacade
{
	private enum EMUSIC_STATE
	{
		NONE = 0,
		TRACK1 = 1,
		TRACK2 = 2
	}

	public const float MIN_DB = -80f;

	public const float MAX_DB = 0f;

	private const string MIXER_PARAM_MUSIC1_VOL = "Music1Volume";

	private const string MIXER_PARAM_MUSIC2_VOL = "Music2Volume";

	private const string MIXER_PARAM_MUSIC_MASTER_VOL = "MusicMasterVolume";

	private static SoundFacade _instance;

	[Header("Mixer Settings")]
	public AudioMixerGroup mixerMusicMaster;

	public AudioMixerGroup mixerMusicTrack1;

	public AudioMixerGroup mixerMusicTrack2;

	public SoundFacadeChannel[] sfxMixerChannels;

	[Header("Pool Settings")]
	public int oneShotPoolSize = 32;

	public int loopingPoolSize = 16;

	[Header("Sound Settings")]
	public SoundFacadeMusicInfo[] music;

	public SoundFacadeSoundEffectInfo[] soundEffectGroups;

	public bool debugReportPoolStatus;

	private Dictionary<string, SoundFacadeMusicInfo> _musicLookup = new Dictionary<string, SoundFacadeMusicInfo>();

	private Dictionary<string, SoundFacadeSoundEffectInfo> _sfxLookup = new Dictionary<string, SoundFacadeSoundEffectInfo>();

	private Dictionary<string, SoundFacadeChannel> _sfxChannels = new Dictionary<string, SoundFacadeChannel>();

	private Dictionary<AudioMixerGroup, SoundFacadeChannel> _sfxChannelsByGroup = new Dictionary<AudioMixerGroup, SoundFacadeChannel>();

	private EMUSIC_STATE _currentMusicState;

	private AudioSource _asMusic1;

	private AudioSource _asMusic2;

	private SoundFacadeMusicInfo _currentMusicGroup;

	private AudioListener _listener;

	private bool _isMusicFading;

	private float _musicFadeStartTime;

	private float _musicFadeDuration;

	private float _music1FadeStartDB;

	private float _music2FadeStartDB;

	private bool _initialised;

	private SoundFacadePool _oneShotPool;

	private SoundFacadePool _loopingPool;

	private Transform _audioListenerFollowTransform;

	public static SoundFacade _pInstance
	{
		get
		{
			return _instance;
		}
	}

	public bool _pMusicMuted
	{
		get
		{
			return _asMusic1.mute || _asMusic2.mute;
		}
		set
		{
			AudioSource asMusic = _asMusic1;
			bool mute = value;
			_asMusic2.mute = mute;
			asMusic.mute = mute;
		}
	}

	public float _pRealTime
	{
		get
		{
			return RealTime.time;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Update()
	{
		if (_isMusicFading)
		{
			UpdateMusicFade();
		}
		if (_oneShotPool != null)
		{
			_oneShotPool.UpdateUsedElements();
		}
		if (_loopingPool != null)
		{
			_loopingPool.UpdateUsedElements();
		}
		if (debugReportPoolStatus)
		{
			Debug.Log(_oneShotPool.ReportPools());
			debugReportPoolStatus = false;
		}
	}

	private void LateUpdate()
	{
		if (_oneShotPool != null)
		{
			_oneShotPool.LateUpdateUsedElements();
		}
		if (_loopingPool != null)
		{
			_loopingPool.LateUpdateUsedElements();
		}
		if (_audioListenerFollowTransform != null)
		{
			_listener.transform.position = _audioListenerFollowTransform.position;
			_listener.transform.rotation = _audioListenerFollowTransform.rotation;
		}
	}

	public override void startInitialising()
	{
		if (_currentState != InitialisationState.INITIALISING)
		{
			_currentState = InitialisationState.INITIALISING;
			Initialise();
			_currentState = InitialisationState.FINISHED;
		}
	}

	public void Initialise()
	{
		Debug.Log("Initialising Sound Facade");
		_instance = this;
		SoundSpawnBehaviourRegistrar.Register();
		_oneShotPool = new SoundFacadePool(oneShotPoolSize, "OneShot");
		_loopingPool = new SoundFacadePool(loopingPoolSize, "Looping");
		GameObject gameObject = new GameObject("Music1");
		GameObject gameObject2 = new GameObject("Music2");
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		AudioSource audioSource2 = gameObject2.AddComponent<AudioSource>();
		gameObject.transform.parent = base.transform;
		gameObject2.transform.parent = base.transform;
		audioSource.spatialBlend = 0f;
		audioSource2.spatialBlend = 0f;
		audioSource.loop = true;
		audioSource.outputAudioMixerGroup = mixerMusicTrack1;
		audioSource2.outputAudioMixerGroup = mixerMusicTrack2;
		audioSource2.loop = true;
		_asMusic1 = audioSource;
		_asMusic2 = audioSource2;
		Transform transform = base.transform.Find("Listener");
		GameObject gameObject3;
		if (transform != null)
		{
			gameObject3 = transform.gameObject;
		}
		else
		{
			gameObject3 = new GameObject("Listener");
			gameObject3.transform.parent = base.transform;
		}
		_listener = gameObject3.EnsureComponent<AudioListener>();
		if (mixerMusicTrack1 == null)
		{
			Debug.LogError("mixerMusicTrack1 has not been set on SoundFacade");
		}
		if (mixerMusicTrack2 == null)
		{
			Debug.LogError("mixerMusicTrack2 has not been set on SoundFacade");
		}
		if (mixerMusicMaster == null)
		{
			Debug.LogError("mixerMusicMaster has not been set on SoundFacade");
		}
		int num = music.Length;
		for (int i = 0; i < num; i++)
		{
			_musicLookup[music[i].musicGroupName] = music[i];
		}
		int num2 = soundEffectGroups.Length;
		for (int j = 0; j < num2; j++)
		{
			_sfxLookup[soundEffectGroups[j].soundGroupName] = soundEffectGroups[j];
		}
		int num3 = sfxMixerChannels.Length;
		for (int k = 0; k < num3; k++)
		{
			_sfxChannels[sfxMixerChannels[k].mixer.name] = sfxMixerChannels[k];
			_sfxChannelsByGroup[sfxMixerChannels[k].mixer] = sfxMixerChannels[k];
		}
		mixerMusicTrack1.audioMixer.SetFloat("Music1Volume", -80f);
		mixerMusicTrack2.audioMixer.SetFloat("Music2Volume", -80f);
		_initialised = true;
	}

	public void PlayMusic(string soundGroupID, float crossFadeTime = 0f, bool allowRetrigger = false)
	{
		SoundFacadeMusicInfo soundFacadeMusicInfo = _musicLookup[soundGroupID];
		if (soundFacadeMusicInfo == _currentMusicGroup && !allowRetrigger)
		{
			return;
		}
		_currentMusicGroup = soundFacadeMusicInfo;
		if (crossFadeTime == 0f)
		{
			if (_currentMusicState == EMUSIC_STATE.NONE || _currentMusicState == EMUSIC_STATE.TRACK2)
			{
				_asMusic1.clip = soundFacadeMusicInfo.GetClip();
				_asMusic1.volume = soundFacadeMusicInfo.volume;
				mixerMusicTrack1.audioMixer.SetFloat("Music1Volume", 0f);
				mixerMusicTrack2.audioMixer.SetFloat("Music2Volume", -80f);
				_asMusic2.volume = 0f;
				_asMusic2.Stop();
				_asMusic1.panStereo = soundFacadeMusicInfo.pan;
				_asMusic1.Play();
				_currentMusicState = EMUSIC_STATE.TRACK1;
			}
			else
			{
				_asMusic2.clip = soundFacadeMusicInfo.GetClip();
				_asMusic2.volume = soundFacadeMusicInfo.volume;
				_asMusic1.volume = 0f;
				_asMusic1.Stop();
				mixerMusicTrack1.audioMixer.SetFloat("Music1Volume", -80f);
				mixerMusicTrack2.audioMixer.SetFloat("Music2Volume", 0f);
				_asMusic2.panStereo = soundFacadeMusicInfo.pan;
				_asMusic2.Play();
				_currentMusicState = EMUSIC_STATE.TRACK2;
			}
		}
		else
		{
			mixerMusicTrack1.audioMixer.GetFloat("Music1Volume", out _music1FadeStartDB);
			mixerMusicTrack2.audioMixer.GetFloat("Music2Volume", out _music2FadeStartDB);
			_musicFadeStartTime = _pRealTime;
			_musicFadeDuration = crossFadeTime;
			if (_currentMusicState == EMUSIC_STATE.TRACK1)
			{
				_currentMusicState = EMUSIC_STATE.TRACK2;
				_asMusic2.clip = soundFacadeMusicInfo.GetClip();
				_asMusic2.panStereo = soundFacadeMusicInfo.pan;
				_asMusic2.volume = soundFacadeMusicInfo.volume;
				_asMusic2.Play();
			}
			else
			{
				_currentMusicState = EMUSIC_STATE.TRACK1;
				_asMusic1.clip = soundFacadeMusicInfo.GetClip();
				_asMusic1.volume = soundFacadeMusicInfo.volume;
				_asMusic1.panStereo = soundFacadeMusicInfo.pan;
				_asMusic1.Play();
			}
			_isMusicFading = true;
		}
	}

	public void FadeMusicOut(float fadeTime)
	{
		if (fadeTime == 0f)
		{
			_asMusic1.volume = 0f;
			_asMusic1.Stop();
			_asMusic2.volume = 0f;
			_asMusic2.Stop();
			mixerMusicTrack1.audioMixer.SetFloat("Music1Volume", -80f);
			mixerMusicTrack2.audioMixer.SetFloat("Music2Volume", -80f);
			_currentMusicGroup = null;
			_currentMusicState = EMUSIC_STATE.NONE;
		}
		else
		{
			_isMusicFading = true;
			_currentMusicState = EMUSIC_STATE.NONE;
			_musicFadeStartTime = _pRealTime;
			_musicFadeDuration = fadeTime;
			mixerMusicTrack1.audioMixer.GetFloat("Music1Volume", out _music1FadeStartDB);
			mixerMusicTrack2.audioMixer.GetFloat("Music2Volume", out _music2FadeStartDB);
		}
	}

	public float GetSFXChannelVolumeDB(string channelName)
	{
		if (!_initialised)
		{
			return 0f;
		}
		return _sfxChannels[channelName].GetVolumeDb();
	}

	public void SetSFXChannelVolumeDB(string channelName, float db)
	{
		if (_initialised)
		{
			_sfxChannels[channelName].SetVolumeDb(db);
		}
	}

	public void SetSFXChannelVolumeScalar(string channelName, float val)
	{
		if (_initialised)
		{
			_sfxChannels[channelName].SetVolumeScalar(val);
		}
	}

	public void SetSFXChannelMute(string channelName, bool val)
	{
		if (_initialised)
		{
			_sfxChannels[channelName]._pMuted = val;
			_oneShotPool.UpdateMuteStates(_sfxChannels[channelName].mixer, val);
			_loopingPool.UpdateMuteStates(_sfxChannels[channelName].mixer, val);
		}
	}

	public bool GetSFXChannelMute(string channelName)
	{
		if (!_initialised)
		{
			return false;
		}
		return _sfxChannels[channelName]._pMuted;
	}

	public bool GetSFXChannelMute(AudioMixerGroup group)
	{
		if (!_initialised)
		{
			return false;
		}
		return _sfxChannelsByGroup[group]._pMuted;
	}

	public AudioSource GetPlayingAudioSourceByTag(string tagName)
	{
		AudioSource audioSource = _oneShotPool.FindActiveAudioSourceByTag(tagName);
		if (audioSource == null)
		{
			audioSource = _loopingPool.FindActiveAudioSourceByTag(tagName);
		}
		return audioSource;
	}

	public void GetAllPlayingAudioSourcesByTag(string clipname, ref List<AudioSource> targetArray)
	{
		_oneShotPool.FindActiveAudioSourcesByTag(clipname, ref targetArray);
		_loopingPool.FindActiveAudioSourcesByTag(clipname, ref targetArray, false);
	}

	public AudioSource GetPlayingAudioSourceByClipName(string clipName)
	{
		AudioSource audioSource = _oneShotPool.FindActiveAudioSource(clipName);
		if (audioSource == null)
		{
			audioSource = _loopingPool.FindActiveAudioSource(clipName);
		}
		return audioSource;
	}

	public void GetAllPlayingAudioSourcesByClipName(string clipname, ref List<AudioSource> targetArray)
	{
		_oneShotPool.FindActiveAudioSources(clipname, ref targetArray);
		_loopingPool.FindActiveAudioSources(clipname, ref targetArray);
	}

	public AudioSource PlayOneShotSFX(string soundGroupID, float delay = 0f, Action<SoundFacadeSoundEffectInfo> onCompletedAction = null, string tagName = null, string spawnBehaviour = "SoundSpawnBehaviourBase")
	{
		return HandleOneShotCreation(soundGroupID, null, null, delay, onCompletedAction, tagName, spawnBehaviour);
	}

	public AudioSource PlayOneShotSFX(string soundGroupID, Vector3 location, float delay = 0f, Action<SoundFacadeSoundEffectInfo> onCompletedAction = null, string tagName = null, string spawnBehaviour = "SoundSpawnBehaviourBase")
	{
		return HandleOneShotCreation(soundGroupID, null, location, delay, onCompletedAction, tagName, spawnBehaviour, true);
	}

	public AudioSource PlayOneShotSFX(string soundGroupID, Transform followTarget, float delay = 0f, Action<SoundFacadeSoundEffectInfo> onCompletedAction = null, string tagName = null, string spawnBehaviour = "SoundSpawnBehaviourBase")
	{
		return HandleOneShotCreation(soundGroupID, followTarget, null, delay, onCompletedAction, tagName, spawnBehaviour);
	}

	public AudioSource PlayLoopingSFX(string soundGroupID, float delay = 0f, Action<SoundFacadeSoundEffectInfo> onCompletedAction = null, string tagName = null, string spawnBehaviour = "SoundSpawnBehaviourBase")
	{
		return HandleLoopingCreation(soundGroupID, null, null, delay, onCompletedAction, tagName, spawnBehaviour);
	}

	public AudioSource PlayLoopingSFX(string soundGroupID, Vector3 location, float delay = 0f, Action<SoundFacadeSoundEffectInfo> onCompletedAction = null, string tagName = null, string spawnBehaviour = "SoundSpawnBehaviourBase")
	{
		return HandleLoopingCreation(soundGroupID, null, location, delay, onCompletedAction, tagName, spawnBehaviour, true);
	}

	public AudioSource PlayLoopingSFX(string soundGroupID, Transform followTarget, float delay = 0f, Action<SoundFacadeSoundEffectInfo> onCompletedAction = null, string tagName = null, string spawnBehaviour = "SoundSpawnBehaviourBase")
	{
		return HandleLoopingCreation(soundGroupID, followTarget, null, delay, onCompletedAction, tagName, spawnBehaviour);
	}

	public void StopAllLoopingAudio()
	{
		_loopingPool.StopAll();
	}

	public void StopAllLoopingAudioByGroupName(string soundGroupID)
	{
		_loopingPool.StopAllByGroupName(soundGroupID);
	}

	public void StopAllLoopingAudioByClipName(string clipName)
	{
		_loopingPool.StopAllByClipName(clipName);
	}

	public void StopAllLoopingAudioByTag(string tagName)
	{
		_loopingPool.StopAllByTag(tagName);
	}

	public void StopAllLoopingAudioByTrackedTransform(Transform followTarget)
	{
		_loopingPool.StopAllByTransform(followTarget);
	}

	public void StopLoopingAudioByAudioSource(AudioSource source)
	{
		if ((bool)source)
		{
			source.Stop();
		}
	}

	public void MakeAudioListenerTrackTransform(Transform followTarget)
	{
		_audioListenerFollowTransform = followTarget;
	}

	private void UpdateMusicFade()
	{
		float num = _pRealTime - _musicFadeStartTime;
		float num2 = num / _musicFadeDuration;
		if (num2 >= 1f)
		{
			if (_currentMusicState == EMUSIC_STATE.TRACK1)
			{
				mixerMusicTrack1.audioMixer.SetFloat("Music2Volume", -80f);
				mixerMusicTrack2.audioMixer.SetFloat("Music1Volume", 0f);
				_asMusic2.Stop();
			}
			else if (_currentMusicState == EMUSIC_STATE.TRACK2)
			{
				mixerMusicTrack1.audioMixer.SetFloat("Music1Volume", -80f);
				mixerMusicTrack2.audioMixer.SetFloat("Music2Volume", 0f);
				_asMusic1.Stop();
			}
			else
			{
				mixerMusicTrack1.audioMixer.SetFloat("Music1Volume", -80f);
				mixerMusicTrack2.audioMixer.SetFloat("Music2Volume", -80f);
				_asMusic1.Stop();
				_asMusic2.Stop();
				_currentMusicGroup = null;
			}
			_isMusicFading = false;
		}
		else
		{
			float t = Easing.Ease(Easing.EaseType.EaseInCircle, num, _musicFadeDuration, 0f, 1f);
			float t2 = Easing.Ease(Easing.EaseType.EaseOutCircle, num, _musicFadeDuration, 0f, 1f);
			if (_currentMusicState == EMUSIC_STATE.TRACK1)
			{
				mixerMusicTrack1.audioMixer.SetFloat("Music1Volume", Mathf.Lerp(_music1FadeStartDB, 0f, t2));
				mixerMusicTrack2.audioMixer.SetFloat("Music2Volume", Mathf.Lerp(_music2FadeStartDB, -80f, t));
			}
			else if (_currentMusicState == EMUSIC_STATE.TRACK2)
			{
				mixerMusicTrack1.audioMixer.SetFloat("Music1Volume", Mathf.Lerp(_music1FadeStartDB, -80f, t));
				mixerMusicTrack2.audioMixer.SetFloat("Music2Volume", Mathf.Lerp(_music2FadeStartDB, 0f, t2));
			}
			else
			{
				mixerMusicTrack1.audioMixer.SetFloat("Music1Volume", Mathf.Lerp(_music1FadeStartDB, -80f, t));
				mixerMusicTrack2.audioMixer.SetFloat("Music2Volume", Mathf.Lerp(_music2FadeStartDB, -80f, t));
			}
		}
	}

	private void OnOneShotSFXCompleted(SoundFacadePooledElement elem)
	{
		_oneShotPool.ReturnToPool(elem);
	}

	private void OnLoopingSFXCompleted(SoundFacadePooledElement elem)
	{
		_loopingPool.ReturnToPool(elem);
	}

	private AudioSource HandleOneShotCreation(string soundGroupID, Transform followTarget, [Optional] Vector3? location, float delay = 0f, Action<SoundFacadeSoundEffectInfo> onCompletedAction = null, string tagName = null, string spawnBehaviour = "SoundSpawnBehaviourBase", bool setsPosition = false)
	{
		SoundSpawnBehaviourBase.CREATION_BEHAVIOUR pCreationBehaviour = SoundSpawnBehaviourBase._allBehaviours[spawnBehaviour]._pCreationBehaviour;
		SoundFacadePooledElement soundFacadePooledElement = null;
		switch (pCreationBehaviour)
		{
		case SoundSpawnBehaviourBase.CREATION_BEHAVIOUR.CREATE_NEW:
			soundFacadePooledElement = _oneShotPool.GetFreeElement(_sfxLookup[soundGroupID].poolPriority);
			if (soundFacadePooledElement != null)
			{
				return soundFacadePooledElement.LoadAudio(_sfxLookup[soundGroupID], delay, OnOneShotSFXCompleted, onCompletedAction, tagName, SoundSpawnBehaviourBase._allBehaviours[spawnBehaviour], (Vector3)location, followTarget);
			}
			break;
		case SoundSpawnBehaviourBase.CREATION_BEHAVIOUR.MODIFY_THEN_CREATE_IF_NONE_EXIST:
		{
			bool isModified = false;
			soundFacadePooledElement = _oneShotPool.GetExistingElementByGroupName(soundGroupID);
			if (soundFacadePooledElement == null)
			{
				soundFacadePooledElement = _oneShotPool.GetFreeElement(_sfxLookup[soundGroupID].poolPriority);
			}
			else
			{
				isModified = true;
			}
			if (soundFacadePooledElement != null)
			{
				return soundFacadePooledElement.LoadAudio(_sfxLookup[soundGroupID], delay, OnOneShotSFXCompleted, onCompletedAction, tagName, SoundSpawnBehaviourBase._allBehaviours[spawnBehaviour], (Vector3)location, followTarget, isModified);
			}
			break;
		}
		case SoundSpawnBehaviourBase.CREATION_BEHAVIOUR.ALWAYS_MODIFY:
			soundFacadePooledElement = _oneShotPool.GetExistingElementByGroupName(soundGroupID);
			if (soundFacadePooledElement != null)
			{
				return soundFacadePooledElement.LoadAudio(_sfxLookup[soundGroupID], delay, OnOneShotSFXCompleted, onCompletedAction, tagName, SoundSpawnBehaviourBase._allBehaviours[spawnBehaviour], (Vector3)location, followTarget, true);
			}
			break;
		}
		return null;
	}

	private AudioSource HandleLoopingCreation(string soundGroupID, Transform followTarget, [Optional] Vector3? location, float delay = 0f, Action<SoundFacadeSoundEffectInfo> onCompletedAction = null, string tagName = null, string spawnBehaviour = "SoundSpawnBehaviourBase", bool setsPosition = false)
	{
		SoundSpawnBehaviourBase.CREATION_BEHAVIOUR pCreationBehaviour = SoundSpawnBehaviourBase._allBehaviours[spawnBehaviour]._pCreationBehaviour;
		SoundFacadePooledElement soundFacadePooledElement = null;
		switch (pCreationBehaviour)
		{
		case SoundSpawnBehaviourBase.CREATION_BEHAVIOUR.CREATE_NEW:
			soundFacadePooledElement = _loopingPool.GetFreeElement(_sfxLookup[soundGroupID].poolPriority);
			if (soundFacadePooledElement != null)
			{
				return soundFacadePooledElement.LoadAudio(_sfxLookup[soundGroupID], delay, OnLoopingSFXCompleted, onCompletedAction, tagName, SoundSpawnBehaviourBase._allBehaviours[spawnBehaviour], (Vector3)location, followTarget, false, true);
			}
			Debug.LogError("No free elements!");
			break;
		case SoundSpawnBehaviourBase.CREATION_BEHAVIOUR.MODIFY_THEN_CREATE_IF_NONE_EXIST:
		{
			bool isModified = false;
			soundFacadePooledElement = _oneShotPool.GetExistingElementByGroupName(soundGroupID);
			if (soundFacadePooledElement == null)
			{
				soundFacadePooledElement = _loopingPool.GetFreeElement(_sfxLookup[soundGroupID].poolPriority);
			}
			else
			{
				isModified = true;
			}
			if (soundFacadePooledElement != null)
			{
				return soundFacadePooledElement.LoadAudio(_sfxLookup[soundGroupID], delay, OnLoopingSFXCompleted, onCompletedAction, tagName, SoundSpawnBehaviourBase._allBehaviours[spawnBehaviour], (Vector3)location, followTarget, isModified, true);
			}
			Debug.LogError("No free elements!");
			break;
		}
		case SoundSpawnBehaviourBase.CREATION_BEHAVIOUR.ALWAYS_MODIFY:
			soundFacadePooledElement = _loopingPool.GetExistingElementByGroupName(soundGroupID);
			if (soundFacadePooledElement != null)
			{
				return soundFacadePooledElement.LoadAudio(_sfxLookup[soundGroupID], delay, OnLoopingSFXCompleted, onCompletedAction, tagName, SoundSpawnBehaviourBase._allBehaviours[spawnBehaviour], (Vector3)location, followTarget, true, true);
			}
			Debug.LogError("No free elements!");
			break;
		}
		return null;
	}
}

// Token: 0x02000030 RID: 48
public class Easing
{
    // Token: 0x060002D5 RID: 725 RVA: 0x00011904 File Offset: 0x0000FB04
    public static float Ease(Easing.EaseType easing, float time, float duration, float start, float end)
    {
        float num = Easing.CalculateTime(easing, time, duration);
        float num2 = end - start;
        return start + num2 * num;
    }

    // Token: 0x060002D6 RID: 726 RVA: 0x00011924 File Offset: 0x0000FB24
    public static Vector2 Ease(Easing.EaseType easing, float time, float duration, Vector2 start, Vector2 end)
    {
        float num = Easing.CalculateTime(easing, time, duration);
        Vector2 vector = end - start;
        return start + vector * num;
    }

    // Token: 0x060002D7 RID: 727 RVA: 0x00011950 File Offset: 0x0000FB50
    public static Vector3 Ease(Easing.EaseType easing, float time, float duration, Vector3 start, Vector3 end)
    {
        float num = Easing.CalculateTime(easing, time, duration);
        Vector3 vector = end - start;
        return start + vector * num;
    }

    // Token: 0x060002D8 RID: 728 RVA: 0x0001197C File Offset: 0x0000FB7C
    public static Vector4 Ease(Easing.EaseType easing, float time, float duration, Vector4 start, Vector4 end)
    {
        float num = Easing.CalculateTime(easing, time, duration);
        Vector4 vector = end - start;
        return start + vector * num;
    }

    // Token: 0x060002D9 RID: 729 RVA: 0x000119A8 File Offset: 0x0000FBA8
    public static Color Ease(Easing.EaseType easing, float time, float duration, Color start, Color end)
    {
        return Color.Lerp(start, end, Easing.CalculateTime(easing, time, duration));
    }

    // Token: 0x060002DA RID: 730 RVA: 0x000119BC File Offset: 0x0000FBBC
    public static float Wave(Easing.EaseType easing, float time, float duration, float start, float max, int cycles = 1, Easing.FadeType fade = Easing.FadeType.None)
    {
        Easing.WaveTransformTime(ref time, ref duration, cycles, fade);
        return Easing.Ease(easing, time, duration, start, max);
    }

    // Token: 0x060002DB RID: 731 RVA: 0x000119D8 File Offset: 0x0000FBD8
    public static Vector2 Wave(Easing.EaseType easing, float time, float duration, Vector2 start, Vector2 max, int cycles = 1, Easing.FadeType fade = Easing.FadeType.None)
    {
        Easing.WaveTransformTime(ref time, ref duration, cycles, fade);
        return Easing.Ease(easing, time, duration, start, max);
    }

    // Token: 0x060002DC RID: 732 RVA: 0x000119F4 File Offset: 0x0000FBF4
    public static Vector3 Wave(Easing.EaseType easing, float time, float duration, Vector3 start, Vector3 max, int cycles = 1, Easing.FadeType fade = Easing.FadeType.None)
    {
        Easing.WaveTransformTime(ref time, ref duration, cycles, fade);
        return Easing.Ease(easing, time, duration, start, max);
    }

    // Token: 0x060002DD RID: 733 RVA: 0x00011A10 File Offset: 0x0000FC10
    public static Vector4 Wave(Easing.EaseType easing, float time, float duration, Vector4 start, Vector4 max, int cycles = 1, Easing.FadeType fade = Easing.FadeType.None)
    {
        Easing.WaveTransformTime(ref time, ref duration, cycles, fade);
        return Easing.Ease(easing, time, duration, start, max);
    }

    // Token: 0x060002DE RID: 734 RVA: 0x00011A2C File Offset: 0x0000FC2C
    public static Color Wave(Easing.EaseType easing, float time, float duration, Color start, Color max, int cycles = 1, Easing.FadeType fade = Easing.FadeType.None)
    {
        Easing.WaveTransformTime(ref time, ref duration, cycles, fade);
        return Easing.Ease(easing, time, duration, start, max);
    }

    // Token: 0x060002DF RID: 735 RVA: 0x00011A48 File Offset: 0x0000FC48
    public static float FadeMultiplier(Easing.FadeType fade, float time, float duration)
    {
        switch (fade)
        {
            default:
                return 1f;
            case Easing.FadeType.In:
                return time / duration;
            case Easing.FadeType.Out:
                return 1f - time / duration;
            case Easing.FadeType.InOut:
                time *= 2f;
                if (time < duration)
                {
                    return time / duration;
                }
                return 2f - time / duration;
        }
    }

    // Token: 0x060002E0 RID: 736 RVA: 0x00011AA4 File Offset: 0x0000FCA4
    private static void WaveTransformTime(ref float time, ref float duration, int cycles, Easing.FadeType fade)
    {
        float num = Easing.FadeMultiplier(fade, time, duration);
        duration /= (float)cycles;
        float num2 = duration / 2f;
        while (time >= duration)
        {
            time -= duration;
        }
        if (time > num2)
        {
            time = duration - time;
        }
        duration = num2;
        time *= num;
    }

    // Token: 0x060002E1 RID: 737 RVA: 0x00011AFC File Offset: 0x0000FCFC
    private static float CalculateTime(Easing.EaseType easing, float time, float duration)
    {
        switch (easing)
        {
            default:
                return time / duration;
            case Easing.EaseType.EaseIn:
                time /= duration;
                return time * time;
            case Easing.EaseType.EaseOut:
                time /= duration;
                return -1f * time * (time - 2f);
            case Easing.EaseType.EaseInOut:
                time /= duration / 2f;
                if (time < 1f)
                {
                    return 0.5f * time * time;
                }
                return -0.5f * ((time - 1f) * (time - 3f) - 1f);
            case Easing.EaseType.EaseInSine:
                return 1f - Mathf.Cos(time / duration * 1.5707964f);
            case Easing.EaseType.EaseOutSine:
                return Mathf.Sin(time / duration * 1.5707964f);
            case Easing.EaseType.EaseInOutSine:
                return -0.5f * (Mathf.Cos(3.1415927f * (time / duration)) - 1f);
            case Easing.EaseType.EaseInExpo:
                if (Mathf.Approximately(0f, time))
                {
                    return 0f;
                }
                return Mathf.Pow(2f, 10f * (time / duration - 1f));
            case Easing.EaseType.EaseOutExpo:
                if (Mathf.Approximately(duration, time))
                {
                    return 1f;
                }
                return 1f - Mathf.Pow(2f, -10f * (time / duration));
            case Easing.EaseType.EaseInOutExpo:
                if (Mathf.Approximately(0f, time))
                {
                    return 0f;
                }
                if (Mathf.Approximately(duration, time))
                {
                    return 1f;
                }
                time /= duration / 2f;
                if (time < 1f)
                {
                    return 0.5f * Mathf.Pow(2f, 10f * (time - 1f));
                }
                return 0.5f * (2f - Mathf.Pow(2f, -10f * (time - 1f)));
            case Easing.EaseType.EaseInCircle:
                time /= duration;
                return -(Mathf.Sqrt(1f - time * time) - 1f);
            case Easing.EaseType.EaseOutCircle:
                time /= duration;
                time -= 1f;
                return Mathf.Sqrt(1f - time * time);
            case Easing.EaseType.EaseInOutCircle:
                time /= duration / 2f;
                if (time < 1f)
                {
                    return -0.5f * (Mathf.Sqrt(1f - time * time) - 1f);
                }
                time -= 2f;
                return 0.5f * (Mathf.Sqrt(1f - time * time) + 1f);
            case Easing.EaseType.EaseInElastic:
                {
                    if (Mathf.Approximately(time, 0f))
                    {
                        return 0f;
                    }
                    time /= duration;
                    if (Mathf.Approximately(time, 1f))
                    {
                        return 1f;
                    }
                    float num = duration * 0.3f;
                    float num2 = num / 4f;
                    time -= 1f;
                    return -(Mathf.Pow(2f, 10f * time) * Mathf.Sin((time * duration - num2) * 6.2831855f / num));
                }
            case Easing.EaseType.EaseOutElastic:
                {
                    if (Mathf.Approximately(time, 0f))
                    {
                        return 0f;
                    }
                    time /= duration;
                    if (Mathf.Approximately(time, 1f))
                    {
                        return 1f;
                    }
                    float num = duration * 0.3f;
                    float num2 = num / 4f;
                    return Mathf.Pow(2f, -10f * time) * Mathf.Sin((time * duration - num2) * 6.2831855f / num) + 1f;
                }
            case Easing.EaseType.EaseInOutElastic:
                {
                    if (Mathf.Approximately(time, 0f))
                    {
                        return 0f;
                    }
                    time /= duration / 2f;
                    if (Mathf.Approximately(time, 2f))
                    {
                        return 1f;
                    }
                    float num = duration * 0.45000002f;
                    float num2 = num / 4f;
                    time -= 1f;
                    if (time < 0f)
                    {
                        return -0.5f * (Mathf.Pow(2f, 10f * time) * Mathf.Sin((time * duration - num2) * 6.2831855f / num));
                    }
                    return Mathf.Pow(2f, -10f * time) * Mathf.Sin((time * duration - num2) * 6.2831855f / num) * 0.5f + 1f;
                }
            case Easing.EaseType.EaseInBack:
                {
                    float num2 = 1.70158f;
                    time /= duration;
                    return time * time * ((num2 + 1f) * time - num2);
                }
            case Easing.EaseType.EaseOutBack:
                {
                    float num2 = 1.70158f;
                    time /= duration;
                    time -= 1f;
                    return time * time * ((num2 + 1f) * time + num2) + 1f;
                }
            case Easing.EaseType.EaseInOutBack:
                {
                    float num2 = 2.5949094f;
                    time /= duration / 2f;
                    if (time < 1f)
                    {
                        return 0.5f * (time * time * ((num2 + 1f) * time - num2));
                    }
                    time -= 2f;
                    return 0.5f * (time * time * ((num2 + 1f) * time + num2) + 2f);
                }
            case Easing.EaseType.EaseInBounce:
                return Easing.EaseInBounce(time, duration);
            case Easing.EaseType.EaseOutBounce:
                return Easing.EaseOutBounce(time, duration);
            case Easing.EaseType.EaseInOutBounce:
                if (time < duration / 2f)
                {
                    return Easing.EaseInBounce(time * 2f, duration) * 0.5f;
                }
                return Easing.EaseOutBounce(time * 2f - duration, duration) * 0.5f + 0.5f;
        }
    }

    // Token: 0x060002E2 RID: 738 RVA: 0x00011FF0 File Offset: 0x000101F0
    private static float EaseInBounce(float time, float duration)
    {
        return 1f - Easing.EaseOutBounce(duration - time, duration);
    }

    // Token: 0x060002E3 RID: 739 RVA: 0x00012004 File Offset: 0x00010204
    private static float EaseOutBounce(float time, float duration)
    {
        time /= duration;
        if (time < 0.36363637f)
        {
            return 7.5625f * time * time;
        }
        if (time < 0.72727275f)
        {
            time -= 0.54545456f;
            return 7.5625f * time * time + 0.75f;
        }
        if (time < 0.90909094f)
        {
            time -= 0.8181818f;
            return 7.5625f * time * time + 0.9375f;
        }
        time -= 0.95454544f;
        return 7.5625f * time * time + 0.984375f;
    }

    // Token: 0x02000031 RID: 49
    public enum EaseType
    {
        // Token: 0x0400019A RID: 410
        Linear,
        // Token: 0x0400019B RID: 411
        EaseIn,
        // Token: 0x0400019C RID: 412
        EaseOut,
        // Token: 0x0400019D RID: 413
        EaseInOut,
        // Token: 0x0400019E RID: 414
        EaseInSine,
        // Token: 0x0400019F RID: 415
        EaseOutSine,
        // Token: 0x040001A0 RID: 416
        EaseInOutSine,
        // Token: 0x040001A1 RID: 417
        EaseInExpo,
        // Token: 0x040001A2 RID: 418
        EaseOutExpo,
        // Token: 0x040001A3 RID: 419
        EaseInOutExpo,
        // Token: 0x040001A4 RID: 420
        EaseInCircle,
        // Token: 0x040001A5 RID: 421
        EaseOutCircle,
        // Token: 0x040001A6 RID: 422
        EaseInOutCircle,
        // Token: 0x040001A7 RID: 423
        EaseInElastic,
        // Token: 0x040001A8 RID: 424
        EaseOutElastic,
        // Token: 0x040001A9 RID: 425
        EaseInOutElastic,
        // Token: 0x040001AA RID: 426
        EaseInBack,
        // Token: 0x040001AB RID: 427
        EaseOutBack,
        // Token: 0x040001AC RID: 428
        EaseInOutBack,
        // Token: 0x040001AD RID: 429
        EaseInBounce,
        // Token: 0x040001AE RID: 430
        EaseOutBounce,
        // Token: 0x040001AF RID: 431
        EaseInOutBounce
    }

    // Token: 0x02000032 RID: 50
    public enum FadeType
    {
        // Token: 0x040001B1 RID: 433
        None,
        // Token: 0x040001B2 RID: 434
        In,
        // Token: 0x040001B3 RID: 435
        Out,
        // Token: 0x040001B4 RID: 436
        InOut
    }
}
