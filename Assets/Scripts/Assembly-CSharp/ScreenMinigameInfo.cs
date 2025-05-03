public class ScreenMinigameInfo : ScreenBase
{
	public UIPanel backingPanel;

	public UILabel highScore;

	public UILabel description;

	public UILabel minigameName;

	public UISprite vehicleTypeIcon;

	public UISprite[] backingPanels;

	public UISprite[] padlocks;

	public UITexture[] vehicleRenders;

	private bool _screenFlowIsForward = true;

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish", 0f);
		if (_screenFlowIsForward)
		{
			TryChangeWidgetSets(base.gameObject, "Default");
		}
		else
		{
			TryChangeWidgetSets(base.gameObject, "Reverse");
		}
		CameraHUB._pInstance._pCameraControllable = false;
		MinigameManager.MinigameData currentMinigameData = MinigameManager._pInstance.GetCurrentMinigameData();
		highScore.text = currentMinigameData._pPersonalHighScore.ToString();
		description.text = Localise(currentMinigameData.minigameDescription);
		minigameName.text = Localise(currentMinigameData.minigameName);
		switch (currentMinigameData.minigameVehicle)
		{
		case MinigameManager.EVEHICLE_TYPE.LAND:
			vehicleTypeIcon.spriteName = "GroundMissionIcon";
			break;
		case MinigameManager.EVEHICLE_TYPE.AIR:
			vehicleTypeIcon.spriteName = "SkyMissionIcon";
			break;
		case MinigameManager.EVEHICLE_TYPE.WATER:
			vehicleTypeIcon.spriteName = "WaterMissionIcon";
			break;
		}
		bool flag = currentMinigameData._pNumTimesCompleted > 0;
		bool flag2 = currentMinigameData._pNumTimesCompleted > 1;
		backingPanels[1].spriteName = ((!flag) ? "mg_vehicleLocked" : "mg_vehicleUnlocked");
		backingPanels[2].spriteName = ((!flag2) ? "mg_vehicleLocked" : "mg_vehicleUnlocked");
		padlocks[1].enabled = !flag;
		padlocks[2].enabled = !flag2;
		vehicleRenders[1].enabled = flag;
		vehicleRenders[2].enabled = flag2;
		for (int i = 0; i < 3; i++)
		{
			vehicleRenders[i].mainTexture = currentMinigameData.vehicleTemplates[i].templateRender;
		}
		SoundFacade._pInstance.PlayOneShotSFX("GUIPop", 0.75f);
		SoundFacade._pInstance.PlayOneShotSFX("GUIPop", 1f);
		SoundFacade._pInstance.PlayOneShotSFX("GUIPop", 1.25f);
	}

	protected override void Update()
	{
		base.Update();
		if (Facades<ScreenFacade>.Instance._pIsAnyScreenTweening)
		{
			backingPanel.SetDirty();
		}
	}

	public void SetTransitionReverse()
	{
		_screenFlowIsForward = false;
	}

	public void SetTransitionForward()
	{
		_screenFlowIsForward = true;
	}

	public void OnBack()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUIBack", 0f);
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish", 0.1f);
		SetTransitionReverse();
		TryChangeWidgetSets(base.gameObject, "Reverse");
		Navigate("Hub");
	}

	public void OnStartMinigame()
	{
		SetTransitionForward();
		TryChangeWidgetSets(base.gameObject, "Default");
		SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish", 0.1f);
		ScreenBase screen = Facades<ScreenFacade>.Instance.GetScreen("ScreenMinigameSelectVehicle");
		if (screen != null)
		{
			((ScreenMinigameSelectVehicle)screen).SetTransitionForward();
		}
		Navigate("OnStartMinigame");
	}
}
