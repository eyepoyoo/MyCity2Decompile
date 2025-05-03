using UnityEngine;

public class ScreenMinigameInfoV2 : ScreenBase
{
	public UIButton _backButton;

	public GameObject _demoBacking;

	public UISprite _cameraFade;

	public UIPanel backingPanel;

	public UITexture minigameTexture;

	public UILabel highScore;

	public UILabel description;

	public UILabel minigameName;

	public UISprite vehicleTypeIcon;

	public UISprite[] backingPanels;

	public UISprite[] padlocks;

	public UITexture[] vehicleRenders;

	public UISprite constructPanel;

	public UISprite[] arrows;

	public Color enabledColor;

	public Color disabledColor;

	public Color disabledVehicleColor;

	public Color constructPanelEnabled;

	public Color constructPanelDisabled;

	public float _bobRate;

	public float _bobAmount;

	private bool _screenFlowIsForward = true;

	private bool _isSecondVisible;

	private bool _isThirdVisible;

	private Vector3 _dummyVector;

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		_demoBacking.SetActive(false);
		_cameraFade.enabled = true;
		_cameraFade.alpha = 0f;
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish", 0f);
		if (_screenFlowIsForward)
		{
			TryChangeWidgetSets(base.gameObject, "Default");
		}
		else
		{
			TryChangeWidgetSets(base.gameObject, "Reverse");
		}
		if (CameraHUB._pExists)
		{
			CameraHUB._pInstance._pCameraControllable = false;
		}
		MinigameManager.MinigameData currentMinigameData = MinigameManager._pInstance.GetCurrentMinigameData();
		minigameTexture.mainTexture = currentMinigameData.menuBackingTexture;
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
		_isSecondVisible = currentMinigameData._pNumTimesCompleted > 0;
		_isThirdVisible = currentMinigameData._pNumTimesCompleted > 1;
		backingPanels[1].color = ((!_isSecondVisible) ? disabledColor : enabledColor);
		backingPanels[2].color = ((!_isThirdVisible) ? disabledColor : enabledColor);
		constructPanel.color = ((!_isThirdVisible) ? constructPanelDisabled : constructPanelEnabled);
		padlocks[1].enabled = !_isSecondVisible;
		padlocks[2].enabled = !_isThirdVisible;
		vehicleRenders[1].color = ((!_isSecondVisible) ? disabledVehicleColor : enabledColor);
		vehicleRenders[2].color = ((!_isThirdVisible) ? disabledVehicleColor : enabledColor);
		arrows[1].enabled = _isSecondVisible;
		arrows[2].enabled = _isThirdVisible;
		for (int i = 0; i < 3; i++)
		{
			vehicleRenders[i].mainTexture = currentMinigameData.vehicleTemplates[i].templateRender;
		}
		SoundFacade._pInstance.PlayOneShotSFX("GUIPop", 0.75f);
		SoundFacade._pInstance.PlayOneShotSFX("GUIPop", 1f);
		SoundFacade._pInstance.PlayOneShotSFX("GUIPop", 1.25f);
	}

	protected override void OnScreenShowComplete()
	{
		base.OnScreenShowComplete();
	}

	protected override void Update()
	{
		base.Update();
		if (_cameraFade.enabled && CameraHUB._pExists && ScreenHub._pInstance != null)
		{
			_cameraFade.alpha = ScreenHub._pInstance.GetHubTransitionFade();
		}
		backingPanel.SetDirty();
		float y = Mathf.Sin(RealTime.time * _bobRate) * _bobAmount;
		_dummyVector.y = y;
		arrows[0].transform.localPosition = _dummyVector;
		arrows[1].transform.localPosition = _dummyVector;
		arrows[2].transform.localPosition = _dummyVector;
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
		if (base._pCurrentTweenType == ScreenTweenType.Idle)
		{
			SoundFacade._pInstance.PlayOneShotSFX("GUIBack", 0f);
			SoundFacade._pInstance.PlayOneShotSFX("GUISwish", 0.1f);
			SetTransitionReverse();
			TryChangeWidgetSets(base.gameObject, "Reverse");
			Navigate("Hub");
		}
	}

	public void OnSelectVehicle1()
	{
		if (base._pCurrentTweenType == ScreenTweenType.Idle)
		{
			MinigameManager._pInstance._pSelectedVehicleTemplate = 0;
			OnStartMinigame();
		}
	}

	public void OnSelectVehicle2()
	{
		if (base._pCurrentTweenType == ScreenTweenType.Idle && _isSecondVisible)
		{
			MinigameManager._pInstance._pSelectedVehicleTemplate = 1;
			OnStartMinigame();
		}
	}

	public void OnSelectVehicle3()
	{
		if (base._pCurrentTweenType == ScreenTweenType.Idle && _isThirdVisible)
		{
			MinigameManager._pInstance._pSelectedVehicleTemplate = 2;
			OnStartMinigame();
		}
	}

	private void OnStartMinigame()
	{
		if (Facades<TrackingFacade>.Instance != null)
		{
			Facades<TrackingFacade>.Instance.LogMetric(MinigameManager._pInstance._pCurrentMinigameType.ToString(), "MiniGame Start", true);
			Facades<TrackingFacade>.Instance.LogProgress("Minigame_" + MinigameManager._pInstance._pCurrentMinigameType.ToString() + "_start");
		}
		SetTransitionForward();
		TryChangeWidgetSets(base.gameObject, "Default");
		SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish", 0.1f);
		Navigate("OnStartMinigame");
	}
}
