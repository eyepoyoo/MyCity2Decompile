using UnityEngine;

public class TestSF : MonoBehaviour
{
	public bool crossfade1;

	public bool crossfade2;

	public bool fadeOut;

	public bool muteMusic;

	public bool testSound;

	public bool testSound2;

	public bool testSoundRetriggerPitch;

	public bool testSoundRetrigger;

	public bool testGameMute;

	public bool testLoop;

	public float gameChannelVolumeDB;

	private bool _isLoop;

	private void Update()
	{
		SoundFacade._pInstance.SetSFXChannelVolumeDB("Game", gameChannelVolumeDB);
		if (crossfade1)
		{
			crossfade1 = false;
			SoundFacade._pInstance.PlayMusic("Battle", 2f);
		}
		if (crossfade2)
		{
			crossfade2 = false;
			SoundFacade._pInstance.PlayMusic("Title", 2f);
		}
		if (fadeOut)
		{
			fadeOut = false;
			SoundFacade._pInstance.FadeMusicOut(2f);
		}
		if (muteMusic)
		{
			muteMusic = false;
			SoundFacade._pInstance._pMusicMuted = !SoundFacade._pInstance._pMusicMuted;
		}
		if (testSound)
		{
			testSound = false;
			SoundFacade._pInstance.PlayOneShotSFX("Test", 0f);
		}
		if (testSound2)
		{
			testSound2 = false;
			SoundFacade._pInstance.PlayOneShotSFX("Test2", 0f);
		}
		if (testLoop)
		{
			testLoop = false;
			if (_isLoop)
			{
				SoundFacade._pInstance.StopAllLoopingAudioByClipName("force_field");
			}
			else
			{
				SoundFacade._pInstance.PlayLoopingSFX("LoopTest", 0f);
			}
			_isLoop = !_isLoop;
		}
		if (testSoundRetrigger)
		{
			testSoundRetrigger = false;
			SoundFacade._pInstance.PlayOneShotSFX("Test", 0f, null, null, "SoundSpawnBehaviourRetrigger");
		}
		if (testSoundRetriggerPitch)
		{
			testSoundRetriggerPitch = false;
			SoundFacade._pInstance.PlayOneShotSFX("Test", 0f, null, null, "SoundSpawnBehaviourIncreasePitch");
		}
		if (testGameMute)
		{
			testGameMute = false;
			SoundFacade._pInstance.SetSFXChannelMute("Game", !SoundFacade._pInstance.GetSFXChannelMute("Game"));
		}
	}
}
