using UnityEngine;

public class ScreenConfirmExitGame : ScreenBase
{
	public void OnConfirm()
	{
		if (!Facades<ScreenFacade>.Instance._pIsAnyScreenTweening)
		{
			SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
			VehicleBuilder.ClearTemplate();
			Navigate("Confirm");
			ScreenMinigameResults.StopGameSounds();
			if (!SoundFacade._pInstance._pMusicMuted)
			{
				SoundFacade._pInstance.SetSFXChannelMute("Game", false);
			}
			Time.timeScale = 1f;
			ScreenHub.LoadDefaultHUB();
			ScreenLoading.CoverNewSceneLoading();
		}
	}

	public void OnCancel()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUIBack", 0f);
		Navigate("Cancel");
	}
}
