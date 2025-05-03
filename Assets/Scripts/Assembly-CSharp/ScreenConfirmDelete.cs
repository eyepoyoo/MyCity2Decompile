using UnityEngine;

public class ScreenConfirmDelete : ScreenBase
{
	public UIWidget _deletionBackground;

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		_deletionBackground.enabled = false;
	}

	public void OnConfirm()
	{
		if (!Facades<ScreenFacade>.Instance._pIsAnyScreenTweening)
		{
			_deletionBackground.enabled = true;
			SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance.DeleteAll();
			ScenarioManager._pInstance.ResetAll();
			GlobalInGameData.ResetAll();
			MinigameManager._pInstance.ResetAll();
			RewardManager._pInstance.ResetAll();
			ScreenHub._pInstance.ResetAll();
			QuestSystem._pInstance.ResetAll();
			ScreenBase screen = Facades<ScreenFacade>.Instance.GetScreen("ScreenProgressInfo");
			if (screen != null)
			{
				ScreenProgressInfo screenProgressInfo = (ScreenProgressInfo)screen;
				screenProgressInfo.ResetAll();
			}
			ScreenBase screen2 = Facades<ScreenFacade>.Instance.GetScreen("ScreenSelectScenario");
			if (screen2 != null)
			{
				ScreenSelectScenario screenSelectScenario = (ScreenSelectScenario)screen2;
				screenSelectScenario.ResetAll();
			}
			TryChangeWidgetSets(base.gameObject, "OnDelete");
			Navigate("Confirm");
			if (Facades<TrackingFacade>.Instance != null)
			{
				Facades<TrackingFacade>.Instance.LogMetric("City", "Delete");
				Facades<TrackingFacade>.Instance.LogEvent("DeleteCity");
			}
			GlobalInGameData.OnLevelWillLoad("Empty", ScreenLoading._pCurrentLevelName);
			Application.LoadLevel("Empty");
		}
	}

	public void OnCancel()
	{
		TryChangeWidgetSets(base.gameObject, "Default");
		SoundFacade._pInstance.PlayOneShotSFX("GUIBack", 0f);
		Navigate("Cancel");
	}
}
