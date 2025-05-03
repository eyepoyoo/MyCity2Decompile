using UnityEngine;

public class ScreenMinigameSelectVehicle : ScreenBase
{
	public UILabel hintText;

	private bool _screenFlowIsForward = true;

	public Color enabledTitleTextColor;

	public Color disabledTitleTextColor;

	public Color enabledTopBorderColor;

	public Color disabledTopBorderColor;

	public Color enabledVehicleRenderColor;

	public Color disabledVehiclerenderColor;

	public UILabel[] templateNames;

	public UITexture[] vehicleRenders;

	public UIWidget[] darkenAreas;

	public UILabel[] titleText;

	public UISprite[] topBorders;

	public UISprite[] padlocks;

	public UIButton[] playButtons;

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		if (_screenFlowIsForward)
		{
			TryChangeWidgetSets(base.gameObject, "Default");
		}
		else
		{
			TryChangeWidgetSets(base.gameObject, "Reverse");
		}
		MinigameManager.MinigameData currentMinigameData = MinigameManager._pInstance.GetCurrentMinigameData();
		int num = currentMinigameData.vehicleTemplates.Length;
		for (int i = 0; i < num; i++)
		{
			templateNames[i].text = Localise(currentMinigameData.vehicleTemplates[i].templateLocalisationID);
		}
		bool flag = currentMinigameData._pNumTimesCompleted > 0;
		bool flag2 = currentMinigameData._pNumTimesCompleted > 1;
		titleText[1].color = ((!flag) ? disabledTitleTextColor : enabledTitleTextColor);
		titleText[2].color = ((!flag2) ? disabledTitleTextColor : enabledTitleTextColor);
		vehicleRenders[1].color = ((!flag) ? disabledVehiclerenderColor : enabledVehicleRenderColor);
		vehicleRenders[2].color = ((!flag2) ? disabledVehiclerenderColor : enabledVehicleRenderColor);
		darkenAreas[1].alpha = ((!flag) ? 1f : 0.004f);
		darkenAreas[2].alpha = ((!flag2) ? 1f : 0.004f);
		topBorders[1].color = ((!flag) ? disabledTopBorderColor : enabledTopBorderColor);
		topBorders[2].color = ((!flag2) ? disabledTopBorderColor : enabledTopBorderColor);
		playButtons[1].isEnabled = flag;
		playButtons[2].isEnabled = flag2;
		padlocks[1].enabled = !flag;
		padlocks[2].enabled = !flag2;
		for (int j = 0; j < 3; j++)
		{
			vehicleRenders[j].mainTexture = currentMinigameData.vehicleTemplates[j].templateRender;
		}
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish", 0f);
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish2", 0.25f);
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish3", 0.5f);
		SoundFacade._pInstance.PlayOneShotSFX("GUIPop", 0.25f);
		SoundFacade._pInstance.PlayOneShotSFX("GUIPop", 0.75f);
		if (flag)
		{
			SoundFacade._pInstance.PlayOneShotSFX("GUIPop", 1f);
		}
		if (flag2)
		{
			SoundFacade._pInstance.PlayOneShotSFX("GUIPop", 1.25f);
		}
		hintText.text = Localise(currentMinigameData.vehicleSelectHint);
	}

	public void OnBack()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUIBack", 0f);
		SetTransitionReverse();
		TryChangeWidgetSets(base.gameObject, "Reverse");
		ScreenBase screen = Facades<ScreenFacade>.Instance.GetScreen("ScreenMinigameInfo");
		if (screen != null)
		{
			((ScreenMinigameInfo)screen).SetTransitionReverse();
		}
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish", 0f);
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish2", 0.25f);
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish3", 0.5f);
		Navigate("MinigameInfo");
	}

	public void SetTransitionReverse()
	{
		_screenFlowIsForward = false;
	}

	public void SetTransitionForward()
	{
		_screenFlowIsForward = true;
	}

	public void OnFirstVehicleSelected()
	{
		if (base._pCurrentTweenType == ScreenTweenType.Idle)
		{
			SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
			SetTransitionForward();
			TryChangeWidgetSets(base.gameObject, "Default");
			MinigameManager._pInstance._pSelectedVehicleTemplate = 0;
			SoundFacade._pInstance.PlayOneShotSFX("GUISwish", 0f);
			SoundFacade._pInstance.PlayOneShotSFX("GUISwish2", 0.25f);
			SoundFacade._pInstance.PlayOneShotSFX("GUISwish3", 0.5f);
			Navigate("CustomiseVehicle");
		}
	}

	public void OnSecondVehicleSelected()
	{
		if (base._pCurrentTweenType == ScreenTweenType.Idle)
		{
			SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
			SetTransitionForward();
			TryChangeWidgetSets(base.gameObject, "Default");
			MinigameManager._pInstance._pSelectedVehicleTemplate = 1;
			SoundFacade._pInstance.PlayOneShotSFX("GUISwish", 0f);
			SoundFacade._pInstance.PlayOneShotSFX("GUISwish2", 0.25f);
			SoundFacade._pInstance.PlayOneShotSFX("GUISwish3", 0.5f);
			Navigate("CustomiseVehicle");
		}
	}

	public void OnThirdVehicleSelected()
	{
		if (base._pCurrentTweenType == ScreenTweenType.Idle)
		{
			SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
			SetTransitionForward();
			TryChangeWidgetSets(base.gameObject, "Default");
			MinigameManager._pInstance._pSelectedVehicleTemplate = 2;
			SoundFacade._pInstance.PlayOneShotSFX("GUISwish", 0f);
			SoundFacade._pInstance.PlayOneShotSFX("GUISwish2", 0.25f);
			SoundFacade._pInstance.PlayOneShotSFX("GUISwish3", 0.5f);
			Navigate("CustomiseVehicle");
		}
	}
}
