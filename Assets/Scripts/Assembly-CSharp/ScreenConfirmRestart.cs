using UnityEngine;

public class ScreenConfirmRestart : ScreenBase
{
	public void OnConfirm()
	{
		if (!Facades<ScreenFacade>.Instance._pIsAnyScreenTweening)
		{
			SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
			Navigate("Confirm");
			QuestHandler._pInstance.ResetMinigameSessionStats();
			ScreenMinigameResults.StopGameSounds();
			if (!SoundFacade._pInstance._pMusicMuted)
			{
				SoundFacade._pInstance.SetSFXChannelMute("Game", false);
			}
			Time.timeScale = 1f;
			ScreenLoading.Reload();
		}
	}

	public void OnCancel()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUIBack", 0f);
		Navigate("Cancel");
	}
}
