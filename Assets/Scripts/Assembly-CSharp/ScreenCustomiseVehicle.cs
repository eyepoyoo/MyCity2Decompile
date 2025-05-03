using UnityEngine;

public class ScreenCustomiseVehicle : ScreenBase
{
	private static ScreenCustomiseVehicle _instance;

	public GameObject playButtonObj;

	public UISprite temporaryBackdrop;

	public UIButton accessoryInfoButton;

	public UISprite accessoryInfoIcon;

	public UIWidget infoWidget;

	public UILabel infoText;

	public UILabel vehicleNameLabel;

	public UISprite minigameType;

	public UIButton[] carouselButtons;

	public UILabel partInfoNameLabel;

	public UILabel partInfoDescLabel;

	public ScreenObjectTransitionWidget partInfoTransition;

	public ScreenObjectTransitionWidget partInfoTransitionButton;

	public ScreenObjectTransitionWidget partInfoTransitionBacking;

	public Collider partInfoBackingCollider;

	public Camera previewButtonRenderCam;

	public Camera previewPanelRenderCam;

	public TweenPosition _bottomSpeechAreaTweenOffscreen;

	public TweenAlpha _readyTextAlphaTween;

	public TweenAlpha _collectMoreAlphaTween;

	public UIPanel _playButtonShine;

	[SerializeField]
	private FloatingNewIconManager _newIconManager;

	private string _currentPartKey;

	private bool _openedTutorial;

	public bool _isInTutorial;

	public bool _isInInfoPanel;

	protected int _lastBuildStage;

	public static ScreenCustomiseVehicle _pInstance
	{
		get
		{
			return _instance;
		}
	}

	protected virtual bool _pRevealPlayButtonOnComplete
	{
		get
		{
			return true;
		}
	}

	protected virtual void Awake()
	{
		_instance = this;
	}

	protected virtual void OnDestroy()
	{
		_instance = null;
	}

	protected override void OnShowScreen()
	{
		if (temporaryBackdrop != null)
		{
			temporaryBackdrop.enabled = true;
		}
		_openedTutorial = false;
		playButtonObj.SetActive(false);
		if (_playButtonShine != null && _playButtonShine.gameObject != null)
		{
			_playButtonShine.gameObject.SetActive(false);
		}
		GlobalInGameData.OnLevelWillLoad("VehicleBuilder", ScreenLoading._pCurrentLevelName);
		Application.LoadLevel("VehicleBuilder");
		accessoryInfoButton.isEnabled = false;
		accessoryInfoIcon.enabled = false;
		if (VehicleBuilder._pInstance == null)
		{
			vehicleNameLabel.text = string.Empty;
		}
		_isInInfoPanel = false;
		partInfoTransition.SetToTweenInStartPos();
		partInfoTransition.SetStateConsideredTweenedIn(false);
		partInfoTransitionButton.SetToTweenInStartScale();
		partInfoTransitionButton.SetStateConsideredTweenedIn(false);
		partInfoTransitionBacking.SetToTweenInStartColor();
		partInfoTransitionBacking.SetStateConsideredTweenedIn(false);
		partInfoBackingCollider.enabled = false;
		previewButtonRenderCam.enabled = false;
		previewPanelRenderCam.enabled = false;
		_isInTutorial = false;
	}

	public virtual void OnBuilderReady()
	{
		VehicleBuilder._pInstance._pOnSelectedVehicleAction = OnHighlightedBodyPart;
		VehiclePart component = VehicleBuilder._pInstance._pCurrCarousel._pCurrItem.GetComponent<VehiclePart>();
		OnHighlightedBodyPart(component.vehicleTypeCategories[0]);
		AmuzoMonoSingleton<VehicleBuilderBackdrop>._pInstance._magnifyBox.SetActive(false);
		bool isGarage = this is ScreenGarage;
		AmuzoMonoSingleton<VehicleBuilderBackdrop>._pInstance.Prepare(isGarage);
		ShowNewIcons();
	}

	protected override void Update()
	{
		base.Update();
		if (AmuzoMonoSingleton<VehicleBuilderBackdrop>._pExists && temporaryBackdrop != null)
		{
			temporaryBackdrop.enabled = false;
		}
		if (!GlobalInGameData._pHasSeenTutorialVehicleBuilder)
		{
			if (!_openedTutorial && !Facades<ScreenFacade>.Instance._pIsAnyScreenTweening)
			{
				_openedTutorial = true;
				_isInTutorial = true;
				ScreenTutorialVehicleBuilder._pInstance._onOk += delegate
				{
					_isInTutorial = false;
				};
				GlobalInGameData._pHasSeenTutorialVehicleBuilder = true;
				Navigate("CustomiseVehicleTutorial");
			}
			return;
		}
		if (VehicleBuilder._pInstance != null)
		{
			bool flag = VehicleBuilder._pInstance._pCurrCarouselIndex == 3f;
			bool flag2 = this is ScreenGarage;
			if (VehicleBuilder._pInstance.IsValidBuild())
			{
				playButtonObj.SetActive(flag && _pRevealPlayButtonOnComplete && !flag2);
				if (_playButtonShine != null)
				{
					_playButtonShine.gameObject.SetActive(playButtonObj.activeInHierarchy);
				}
				if (_collectMoreAlphaTween != null && _collectMoreAlphaTween.value != 0f)
				{
					_collectMoreAlphaTween.PlayReverse();
				}
			}
			else
			{
				playButtonObj.SetActive(false);
				if (_playButtonShine != null)
				{
					_playButtonShine.gameObject.SetActive(false);
				}
				if (_readyTextAlphaTween != null && _readyTextAlphaTween.value != 0f)
				{
					_readyTextAlphaTween.PlayReverse();
				}
			}
		}
		for (int num = 0; num < carouselButtons.Length; num++)
		{
			if (VehicleBuilder._pInstance._pCurrCarousel != null && VehicleBuilder._pInstance._pCurrCarousel._pCarouselLength <= 1)
			{
				carouselButtons[num].gameObject.SetActive(false);
			}
			else if (VehicleBuilder._pInstance._pCurrCarouselIndexRounded == 3)
			{
				carouselButtons[num].gameObject.SetActive(false);
			}
			else
			{
				carouselButtons[num].gameObject.SetActive(true);
			}
		}
	}

	public void OnShowPartInfo()
	{
		Debug.Log("OnShowPartInfo()");
		if (!_isInInfoPanel)
		{
			partInfoNameLabel.text = Localise(_currentPartKey + "_NAME");
			partInfoDescLabel.text = Localise(_currentPartKey + "_DESC");
			_isInInfoPanel = true;
			partInfoTransition.TweenIn();
			partInfoTransitionButton.TweenIn();
			partInfoTransitionBacking.TweenIn();
			previewPanelRenderCam.enabled = true;
			partInfoBackingCollider.enabled = true;
			SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		}
	}

	public void OnDismissPartInfo()
	{
		_isInInfoPanel = false;
		if (partInfoTransition._pTweenType == ScreenObjectTransitionWidget.TweenType.IDLE)
		{
			partInfoTransition.TweenOut();
			partInfoTransitionButton.TweenOut();
			partInfoTransitionBacking.TweenOut();
			previewPanelRenderCam.enabled = false;
			partInfoBackingCollider.enabled = false;
		}
		SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
	}

	public virtual void OnBuildStageChange(int buildStage)
	{
		Debug.Log("OnBuildStageChange( " + buildStage + ")");
		MinigameManager.EVEHICLE_TYPE pCurrentVehicleTypeForMinigame = MinigameManager._pInstance._pCurrentVehicleTypeForMinigame;
		switch (buildStage)
		{
		case 0:
			if (infoText != null)
			{
				infoText.text = Localise(string.Concat("VehicleBuilder.", pCurrentVehicleTypeForMinigame, ".InfoBody")).ToUpper();
			}
			if (_bottomSpeechAreaTweenOffscreen != null)
			{
				_bottomSpeechAreaTweenOffscreen.PlayForward();
				_bottomSpeechAreaTweenOffscreen.ResetToBeginning();
				_bottomSpeechAreaTweenOffscreen.enabled = false;
			}
			if ((bool)_readyTextAlphaTween)
			{
				_readyTextAlphaTween.PlayForward();
				_readyTextAlphaTween.ResetToBeginning();
				_readyTextAlphaTween.enabled = false;
			}
			if ((bool)_collectMoreAlphaTween)
			{
				_collectMoreAlphaTween.PlayForward();
				_collectMoreAlphaTween.ResetToBeginning();
				_collectMoreAlphaTween.enabled = false;
			}
			AmuzoMonoSingleton<VehicleBuilderBackdrop>._pInstance.ShowStrip();
			ShowNewIcons(VehicleBuilder._pInstance._carouselBodies);
			break;
		case 3:
			if (_bottomSpeechAreaTweenOffscreen != null)
			{
				_bottomSpeechAreaTweenOffscreen.enabled = true;
				_bottomSpeechAreaTweenOffscreen.PlayForward();
				_bottomSpeechAreaTweenOffscreen.ResetToBeginning();
			}
			if ((bool)_readyTextAlphaTween && VehicleBuilder._pInstance.IsValidBuild())
			{
				_readyTextAlphaTween.enabled = true;
				_readyTextAlphaTween.PlayForward();
				_readyTextAlphaTween.ResetToBeginning();
			}
			if ((bool)_collectMoreAlphaTween && !VehicleBuilder._pInstance.IsValidBuild())
			{
				_collectMoreAlphaTween.enabled = true;
				_collectMoreAlphaTween.PlayForward();
				_collectMoreAlphaTween.ResetToBeginning();
			}
			AmuzoMonoSingleton<VehicleBuilderBackdrop>._pInstance.HideStrip(false);
			break;
		case 2:
			if (infoText != null)
			{
				infoText.text = Localise(string.Concat("VehicleBuilder.", pCurrentVehicleTypeForMinigame, ".InfoAccessory")).ToUpper();
			}
			if (_lastBuildStage == 3)
			{
				if (_readyTextAlphaTween != null)
				{
					_readyTextAlphaTween.PlayReverse();
				}
				if (_collectMoreAlphaTween != null)
				{
					_collectMoreAlphaTween.PlayReverse();
				}
				if (_bottomSpeechAreaTweenOffscreen != null)
				{
					_bottomSpeechAreaTweenOffscreen.PlayReverse();
				}
				AmuzoMonoSingleton<VehicleBuilderBackdrop>._pInstance.ShowStrip(false);
			}
			ShowNewIcons(VehicleBuilder._pInstance._carouselSpecials);
			break;
		case 1:
			if (infoText != null)
			{
				infoText.text = Localise(string.Concat("VehicleBuilder.", pCurrentVehicleTypeForMinigame, ".InfoWheels")).ToUpper();
			}
			ShowNewIcons(VehicleBuilder._pInstance._carouselWheels);
			break;
		}
		_lastBuildStage = buildStage;
		Debug.Log("Build Stage Change: " + buildStage);
		SetAccessoryInfoButton();
	}

	public void OnHighlightedBodyPart(MinigameManager.EVEHICLE_TYPE partType)
	{
		Debug.Log("PART: " + partType);
		if (minigameType != null)
		{
			switch (partType)
			{
			case MinigameManager.EVEHICLE_TYPE.LAND:
				minigameType.spriteName = "GroundMissionIcon";
				break;
			case MinigameManager.EVEHICLE_TYPE.WATER:
				minigameType.spriteName = "WaterMissionIcon";
				break;
			case MinigameManager.EVEHICLE_TYPE.AIR:
				minigameType.spriteName = "SkyMissionIcon";
				break;
			}
		}
	}

	public void OnPartChange(Carousel carousel)
	{
		string pCurItemLocalisation = carousel._pCurItemLocalisation;
		Vector3 pInfoPanelCameraPos = carousel._pInfoPanelCameraPos;
		_currentPartKey = pCurItemLocalisation;
		vehicleNameLabel.text = Localise(pCurItemLocalisation);
		if (carousel != null && GlobalInGameData.IsPartNew(carousel._pCurrItem._pVehiclePartRef.uniqueID))
		{
			GlobalInGameData.SetPartViewed(carousel._pCurrItem._pVehiclePartRef.uniqueID);
			ShowNewIcons(carousel);
		}
		SpecialBrickBagRewardCam component = previewButtonRenderCam.GetComponent<SpecialBrickBagRewardCam>();
		SpecialBrickBagRewardCam component2 = previewPanelRenderCam.GetComponent<SpecialBrickBagRewardCam>();
		component.camPos1 = pInfoPanelCameraPos;
		component2.camPos1 = pInfoPanelCameraPos;
		SetAccessoryInfoButton();
	}

	public void OnNextButton()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		VehicleBuilder._pInstance.OnNextButton();
	}

	public void OnPrevButton()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		VehicleBuilder._pInstance.OnPrevButton();
	}

	public virtual void OnBack()
	{
		if (base._pCurrentTweenType == ScreenTweenType.Idle && GlobalInGameData._pHasSeenTutorialVehicleBuilder)
		{
			GlobalInGameData.OnLevelWillLoad("Empty", ScreenLoading._pCurrentLevelName);
			Application.LoadLevel("Empty");
			ScreenHub.LoadDefaultHUB();
			Navigate("MinigameSelectVehicle");
			SoundFacade._pInstance.PlayOneShotSFX("GUIBack", 0f);
		}
	}

	public void OnPlay()
	{
		VehicleBuilder._pInstance.PreserveVehicleForGameplay();
		MinigameManager.MinigameData currentMinigameData = MinigameManager._pInstance.GetCurrentMinigameData();
		Debug.Log("OnPlay");
		bool flag = false;
		if (currentMinigameData == null)
		{
			Debug.LogError("Unable to find current minigame. Launching default minigame instead.");
			ScreenLoading.LoadMinigame("01PoliceLand");
		}
		else if (currentMinigameData.cutsceneFlowLocation == null || currentMinigameData.cutsceneFlowLocation == string.Empty)
		{
			Debug.Log("Loading minigame: " + currentMinigameData.minigameSceneName);
			flag = true;
			ScreenLoading.LoadMinigame(currentMinigameData.minigameSceneName);
		}
		VehiclePart component = VehicleBuilder._pInstance._carouselBodies._pCurrItem.GetComponent<VehiclePart>();
		VehiclePart part = ((!(VehicleBuilder._pInstance._carouselWheels._pCurrItem == null)) ? VehicleBuilder._pInstance._carouselWheels._pCurrItem.GetComponent<VehiclePart>() : null);
		VehiclePart part2 = ((!(VehicleBuilder._pInstance._carouselSpecials._pCurrItem == null)) ? VehicleBuilder._pInstance._carouselSpecials._pCurrItem.GetComponent<VehiclePart>() : null);
		QuestHandler._pInstance.OnStartMission(currentMinigameData, component, part, part2);
		SoundFacade._pInstance.FadeMusicOut(0.5f);
		SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		if (flag)
		{
			Navigate("MinigameLoading");
		}
		else
		{
			Navigate(currentMinigameData.cutsceneFlowLocation);
		}
	}

	public void OnTest()
	{
		VehicleBuilder._pInstance.PreserveVehicleForGameplay();
		ScreenTestVehicle.LoadTestScene();
		Navigate("TestVehicle");
	}

	protected virtual void SetAccessoryInfoButton()
	{
		if (_lastBuildStage == 2)
		{
			accessoryInfoButton.isEnabled = true;
			AmuzoMonoSingleton<VehicleBuilderBackdrop>._pInstance._magnifyBox.SetActive(true);
			accessoryInfoIcon.enabled = true;
			previewButtonRenderCam.enabled = true;
		}
		else
		{
			previewButtonRenderCam.enabled = false;
			accessoryInfoButton.isEnabled = false;
			AmuzoMonoSingleton<VehicleBuilderBackdrop>._pInstance._magnifyBox.SetActive(false);
			accessoryInfoIcon.enabled = false;
		}
	}

	protected void ShowNewIcons(Carousel carousel = null)
	{
		if (!(_newIconManager == null))
		{
			_newIconManager.HideAllNewIcons();
			if (carousel == null)
			{
				carousel = VehicleBuilder._pInstance._pCurrCarousel;
			}
			if (!(carousel == null))
			{
				Debug.Log("Refreshing new icons. Curr Item [" + carousel._pCurItemLocalisation + "]");
				carousel.SetNewIcons(_newIconManager);
			}
		}
	}
}
