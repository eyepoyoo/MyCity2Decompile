using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ScreenHub : ScreenBase
{
	private const string XP_BAR_FLASH_SAVE_KEY = "FTUE_XPBarFlasher";

	private const bool USES_GARAGE = false;

	private const float ICON_NEAR_DIST_SQR = 56000f;

	private static ScreenHub _instance;

	private readonly Vector3 SCALE_ONE = Vector3.one;

	public UIButton[] buttonsToDisableOnScenarioComplete;

	public UIAtlas[] iconAtlases;

	public FloatingProgressInfo[] floatingProgressInfo;

	public Vector3 _centralLocationNoGarage;

	public UILabel xpLevel;

	public UILabel currentBricks;

	public UILabel currentScenarioLabel;

	public UISprite currentScenarioImage;

	public UILabel rewardsRemainingLabel;

	[SerializeField]
	private GameObject _xpBarFlasher;

	public UISprite wobblyChest;

	public UISprite miniChest;

	public UISprite hubTransitionFade;

	public UIWidget rewardNumberContainer;

	public UITexture brickIcon;

	public Transform expBarCurrent;

	public Transform expBarStart;

	public Transform expBarEnd;

	public UIWidget[] chestShinies;

	public GameObject rewardChestContainer;

	public UIWidget templateTrackerIcon;

	public UIWidget pointerIconRegion;

	public UIWidget scenarioProgressRegion;

	public UIWidget screenRegion;

	public bool usesCompleteIcons = true;

	public UIWidget templateCompleteIcon;

	public UIWidget completeIconRegion;

	public UIWidget questIcon;

	public UILabel questCount;

	public UIWidget questCountCircle;

	public UIWidget completedTick;

	public UIWidget questWrapper;

	public UIButton questButton;

	public UIWidget questNewIcon;

	public ScreenObjectTransitionWidget _expBarTransitionWidget;

	public ScreenObjectTransitionWidget _backButtonTransitionWidget;

	public ScreenObjectTransitionWidget _settingsButtonTransitionWidget;

	public ScreenObjectTransitionWidget _scenarioTransitionWidget;

	public ScreenObjectTransitionWidget _questIconTransitionWidget;

	public ScreenObjectTransitionWidget _rewardChestTransitionWidget;

	public ScreenObjectTransitionWidget _zoomInBack;

	public ScreenObjectTransitionWidget _zoomInfoBarWidget;

	public UILabel _zoomInfoText;

	public UILabel _zoomInfoCount;

	public UITexture _zoomInfoBrick;

	public UIWidget _zoomInfoCountBox;

	public Animator questIconAnimator;

	public PeriodicWiggle questIconPeriodicWiggle;

	public ScreenObjectTransitionWidget[] _ftueAnimBits;

	public ScreenObjectTransitionWidget _ftueMessageBox;

	public UILabel _ftueLabel;

	public UILabel _ftueTitle;

	public ScreenObjectTransitionWidget _ftueBackground;

	public BoxCollider _ftueBackgroundCollider;

	private List<UIWidget> _trackerIcons = new List<UIWidget>();

	private int _numTrackerIcons;

	private List<UIWidget> _completeIcons = new List<UIWidget>();

	private int _numCompleteIcons;

	private int _lastKnownEXP;

	private bool _hasFetchedLastKnownEXP;

	private int _tweenExpStart;

	private int _tweenExpEnd;

	private float _tweenExpStartTime;

	private bool _tweeningEXP;

	private float _previousRewardT;

	private bool _showingBrickBagReward;

	private bool _preparingScenarioComplete;

	private StringBuilder _dummySB;

	private bool _wantFlushLevelOnScreenOut;

	private bool _wantScenarioComplete;

	private static bool _wantDailyReward;

	private bool _trackerIconFadeDirty;

	private bool _isFTUEPromptUp;

	private bool _isFTUEBackgroundUp;

	private int _currentLevel;

	private int _lastKnownBricks;

	private float _brickTweenStartTime;

	private bool _tweeningBricks;

	private bool _tweeningRewardChestIn;

	private float _rewardChestTweenStartTime;

	private bool _tweeningQuestButtonIn;

	private float _questButtonTweenStartTime;

	private bool _doTourOfTheCity;

	private bool _canTweenInBrickbagReward = true;

	private Dictionary<GameObject, Vector3> _markerLookup = new Dictionary<GameObject, Vector3>();

	private Vector3 _FTUEMissionIconLocation;

	private Vector3 _FTUEGarageIconLocation;

	private Rect _dummyRect1;

	private Rect _dummyRect2;

	private Plane _dummyPlane;

	private bool _hasSetSceneLoadCallbacks;

	private bool _anyProgressFloaterMidUpdate;

	public static ScreenHub _pInstance
	{
		get
		{
			return _instance;
		}
	}

	public Vector3 _pGarageLocation
	{
		get
		{
			return _centralLocationNoGarage;
		}
	}

	public int _pLastKnownEXP
	{
		get
		{
			return _lastKnownEXP;
		}
	}

	public bool _pShowingBrickBagReward
	{
		get
		{
			return _showingBrickBagReward;
		}
		set
		{
			_showingBrickBagReward = value;
		}
	}

	public bool _pTrackerIconFadeDirty
	{
		get
		{
			return _trackerIconFadeDirty;
		}
		set
		{
			_trackerIconFadeDirty = value;
		}
	}

	private void Awake()
	{
		_dummyPlane = default(Plane);
		_dummyRect1 = default(Rect);
		_dummyRect2 = default(Rect);
		_dummySB = new StringBuilder();
		_instance = this;
		if (!_hasSetSceneLoadCallbacks)
		{
			_hasSetSceneLoadCallbacks = true;
			GlobalInGameData._OnHUBWillBeUnloaded = (Action)Delegate.Combine(GlobalInGameData._OnHUBWillBeUnloaded, new Action(ClearReferences));
		}
	}

	public static void LoadDefaultHUB()
	{
		LoadHUB("01HubCity");
	}

	public static void LoadHUB(string hubLevelName)
	{
		if (Facades<TrackingFacade>.Instance != null)
		{
			Facades<TrackingFacade>.Instance.LogMetric(hubLevelName, "HUB");
		}
		GlobalInGameData.OnLevelWillLoad("Empty", ScreenLoading._pCurrentLevelName);
		ScreenBase.LoadEmptyScene();
		GlobalInGameData.OnLevelWillLoad(hubLevelName, ScreenLoading._pCurrentLevelName);
		ScreenBase.ChangeScene(hubLevelName);
	}

	protected override void Update()
	{
		base.Update();
		if (_isFTUEPromptUp && !Facades<ScreenFacade>.Instance._pIsAnyScreenTweening && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
		{
			OnFTUEOK();
		}
		UpdateTrackerIcons();
		UpdateTweeningBricks();
		UpdateCompletedIcons();
		UpdateProgressFloaters();
		UpdateTweeningRewardChest();
		UpdateTweeningQuestButton();
		if (_wantScenarioComplete)
		{
			OnScenarioComplete();
		}
		if (_wantDailyReward)
		{
			OnNewDailyReward();
		}
		if (!_tweeningEXP || !(RealTime.time >= _tweenExpStartTime))
		{
			return;
		}
		float num = RealTime.time - _tweenExpStartTime;
		float num2 = 1.5f;
		int num3 = 0;
		if (num < num2)
		{
			float t = Easing.Ease(Easing.EaseType.EaseOutCircle, num, num2, 0f, 1f);
			int exp = (int)Mathf.Lerp(_tweenExpStart, _tweenExpEnd, t);
			float levelProgressFromEXP = RewardManager._pInstance.GetLevelProgressFromEXP(exp);
			expBarCurrent.position = Vector3.Lerp(expBarStart.position, expBarEnd.position, levelProgressFromEXP);
			num3 = RewardManager._pInstance.GetLevelFromEXP(exp);
			xpLevel.text = num3.ToString();
			if (levelProgressFromEXP < _previousRewardT)
			{
				SoundFacade._pInstance.PlayOneShotSFX("LevelUp", 0f);
			}
			_previousRewardT = levelProgressFromEXP;
		}
		else
		{
			float levelProgressFromEXP2 = RewardManager._pInstance.GetLevelProgressFromEXP(GlobalInGameData._pCurrentExp);
			expBarCurrent.position = Vector3.Lerp(expBarStart.position, expBarEnd.position, levelProgressFromEXP2);
			num3 = RewardManager._pInstance.GetLevelFromEXP(GlobalInGameData._pCurrentExp);
			xpLevel.text = num3.ToString();
			_tweeningEXP = false;
		}
		UpdateMiniChest(num3);
		if (Facades<TrackingFacade>.Instance != null && num3 != _currentLevel)
		{
			Facades<TrackingFacade>.Instance.LogParameterMetric("Player: Level", new Dictionary<string, string> { 
			{
				"Level",
				_currentLevel.ToString()
			} });
			Facades<TrackingFacade>.Instance.LogProgress("Level_" + _currentLevel);
		}
		_currentLevel = num3;
		RefreshRewardsLabel();
	}

	public void SetHubTransitionFade(float val)
	{
		hubTransitionFade.alpha = val;
	}

	public float GetHubTransitionFade()
	{
		return hubTransitionFade.alpha;
	}

	public void OnFTUEOK()
	{
		_isFTUEPromptUp = false;
		_ftueMessageBox.TweenOut();
		Debug.Log("ONFTUEOK: " + GlobalInGameData._pHasDoneFirstTimeVisit + " / " + GlobalInGameData._pHasSeenGarageTutorial + " /" + GlobalInGameData._pHasCompletedFTUE);
		if (GlobalInGameData._pHasDoneFirstTimeVisit && !GlobalInGameData._pHasSeenEXPTutorial)
		{
			_expBarTransitionWidget.TweenIn();
			RefreshEXP();
			_canTweenInBrickbagReward = true;
			if (!_tweeningEXP)
			{
				Debug.Log("Insta refresh");
				RefreshRewardsLabel();
			}
		}
		else if (!GlobalInGameData._pHasDoneFirstTimeVisit || !GlobalInGameData._pHasSeenGarageTutorial || !GlobalInGameData._pHasCompletedFTUE)
		{
			if (!GlobalInGameData._pHasDoneFirstTimeVisit && !GlobalInGameData._pHasSeenGarageTutorial && !GlobalInGameData._pHasCompletedFTUE)
			{
				Debug.Log("FTUE: First Minigame");
				CameraHUB._pInstance.FocusPoint(_FTUEMissionIconLocation, CameraHUB.EFocusType.PAN_ONLY, 44f);
			}
			else if (GlobalInGameData._pHasDoneFirstTimeVisit && !GlobalInGameData._pHasSeenGarageTutorial && !GlobalInGameData._pHasCompletedFTUE)
			{
				GlobalInGameData._pHasSeenGarageTutorial = true;
			}
			if (GlobalInGameData._pHasDoneFirstTimeVisit && GlobalInGameData._pHasSeenGarageTutorial)
			{
				Debug.Log("FTUE: Fully Complete");
				GlobalInGameData._pHasCompletedFTUE = true;
				CameraHUB._pInstance._pCameraControllable = true;
				if (base._pCurrentTweenType == ScreenTweenType.Idle)
				{
					Navigate("ScreenTourOfTheCity");
				}
				else
				{
					_doTourOfTheCity = true;
				}
				return;
			}
			_ftueBackground.TweenOut();
			_ftueBackgroundCollider.enabled = false;
			_isFTUEBackgroundUp = false;
			CameraHUB._pInstance._pCameraControllable = true;
		}
		_xpBarFlasher.SetActive(GlobalInGameData._pHasCompletedFTUE && !AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getBool("FTUE_XPBarFlasher"));
	}

	public void RequestFadeIn()
	{
		TryChangeWidgetSets(base.gameObject, "WantFadeIn");
	}

	public void RefreshEXP()
	{
		if (!_preparingScenarioComplete)
		{
			if (!_hasFetchedLastKnownEXP && GlobalInGameData._pHasSeenEXPTutorial)
			{
				_lastKnownEXP = AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getInt("CurrentXP");
				_hasFetchedLastKnownEXP = true;
			}
			else if (!GlobalInGameData._pHasSeenEXPTutorial)
			{
				_hasFetchedLastKnownEXP = true;
			}
			float levelProgressFromEXP = RewardManager._pInstance.GetLevelProgressFromEXP(_lastKnownEXP);
			expBarCurrent.position = Vector3.Lerp(expBarStart.position, expBarEnd.position, levelProgressFromEXP);
			if (_lastKnownEXP != GlobalInGameData._pCurrentExp)
			{
				Debug.Log("EXP CHANGE!");
				_tweenExpStart = _lastKnownEXP;
				_tweenExpEnd = GlobalInGameData._pCurrentExp;
				_tweenExpStartTime = RealTime.time + 1f;
				_tweeningEXP = true;
				int levelFromEXP = RewardManager._pInstance.GetLevelFromEXP(_lastKnownEXP);
				xpLevel.text = levelFromEXP.ToString();
				RefreshRewardsLabel();
				_lastKnownEXP = GlobalInGameData._pCurrentExp;
			}
			_xpBarFlasher.SetActive(GlobalInGameData._pHasCompletedFTUE && !AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getBool("FTUE_XPBarFlasher"));
		}
	}

	public void BeginTweeningBricks(float delay = 0f)
	{
		brickIcon.material = ScenarioManager._pInstance._pCurrentScenario.brickMaterial;
		_tweeningBricks = true;
		_brickTweenStartTime = Time.time + delay;
		Debug.Log("BeginTweeningBricks()");
	}

	public void TweenOutScreenForManualPan(CityManager.ECITYBUILDINGS buildingType)
	{
		if (GlobalInGameData._pHasCompletedFTUE || GlobalInGameData._pHasSeenEXPTutorial)
		{
			_expBarTransitionWidget.TweenOut();
		}
		_backButtonTransitionWidget.TweenOut();
		_scenarioTransitionWidget.TweenOut();
		_settingsButtonTransitionWidget.TweenOut();
		_rewardChestTransitionWidget.SetToTweenInStartPos();
		_rewardChestTransitionWidget.SetStateConsideredTweenedIn(true);
		_rewardChestTransitionWidget.TweenOut();
		_questIconTransitionWidget.SetToTweenInStartPos();
		_questIconTransitionWidget.SetStateConsideredTweenedIn(true);
		_questIconTransitionWidget.TweenOut();
		_zoomInBack.TweenIn();
		_zoomInfoBarWidget.TweenIn();
		Scenario scenarioFromBuildingID = ScenarioManager._pInstance.GetScenarioFromBuildingID(buildingType);
		_zoomInfoText.text = Localise(scenarioFromBuildingID.scenarioName);
		_zoomInfoBrick.material = scenarioFromBuildingID.brickMaterial;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(scenarioFromBuildingID._pCurrentBricks);
		stringBuilder.Append("/");
		stringBuilder.Append(scenarioFromBuildingID.totalBricksRequired);
		_zoomInfoCount.text = stringBuilder.ToString();
	}

	public void TweenInScreenFromManualPan()
	{
		if ((GlobalInGameData._pHasCompletedFTUE || GlobalInGameData._pHasSeenEXPTutorial) && !_expBarTransitionWidget._pConsideredTweenedIn)
		{
			_expBarTransitionWidget.TweenIn();
		}
		if (!_backButtonTransitionWidget._pConsideredTweenedIn)
		{
			_backButtonTransitionWidget.TweenIn();
		}
		if (!_scenarioTransitionWidget._pConsideredTweenedIn)
		{
			_scenarioTransitionWidget.TweenIn();
		}
		if (!_settingsButtonTransitionWidget._pConsideredTweenedIn)
		{
			_settingsButtonTransitionWidget.TweenIn();
		}
		if (!_rewardChestTransitionWidget._pConsideredTweenedIn)
		{
			_rewardChestTransitionWidget.TweenIn();
		}
		if (!_questIconTransitionWidget._pConsideredTweenedIn)
		{
			_questIconTransitionWidget.TweenIn();
		}
		if (_zoomInBack._pConsideredTweenedIn)
		{
			_zoomInBack.TweenOut();
		}
		if (_zoomInfoBarWidget._pConsideredTweenedIn)
		{
			_zoomInfoBarWidget.TweenOut();
		}
	}

	public void UpdateTweeningBricks()
	{
		if (!_tweeningBricks)
		{
			return;
		}
		float num = Time.time - _brickTweenStartTime;
		if (!(num < 0f))
		{
			if (num < 1f)
			{
				float num2 = num / 1f;
				int value = (int)Mathf.Lerp(_lastKnownBricks, ScenarioManager._pInstance._pCurrentScenario._pCurrentBricks, num2);
				_dummySB.Length = 0;
				_dummySB.Append(value);
				_dummySB.Append("/");
				_dummySB.Append(ScenarioManager._pInstance._pCurrentScenario.totalBricksRequired);
				currentBricks.text = _dummySB.ToString();
				float t = Frac(num2 * 16f);
				brickIcon.transform.localScale = Vector3.Lerp(new Vector3(1.3f, 1.3f, 1.3f), SCALE_ONE, t);
			}
			else
			{
				_dummySB.Length = 0;
				_dummySB.Append(ScenarioManager._pInstance._pCurrentScenario._pCurrentBricks);
				_dummySB.Append("/");
				_dummySB.Append(ScenarioManager._pInstance._pCurrentScenario.totalBricksRequired);
				currentBricks.text = _dummySB.ToString();
				_lastKnownBricks = ScenarioManager._pInstance._pCurrentScenario._pCurrentBricks;
				_tweeningBricks = false;
				brickIcon.transform.localScale = SCALE_ONE;
			}
		}
	}

	public void RefreshBricks()
	{
		_lastKnownBricks = ScenarioManager._pInstance._pCurrentScenario._pCurrentBricks;
		_tweeningBricks = false;
		_dummySB.Length = 0;
		_dummySB.Append(ScenarioManager._pInstance._pCurrentScenario._pCurrentBricks);
		_dummySB.Append("/");
		_dummySB.Append(ScenarioManager._pInstance._pCurrentScenario.totalBricksRequired);
		currentBricks.text = _dummySB.ToString();
		brickIcon.material = ScenarioManager._pInstance._pCurrentScenario.brickMaterial;
	}

	private void UpdateTweeningRewardChest()
	{
		if (!_tweeningRewardChestIn)
		{
			return;
		}
		float num = Time.time - _rewardChestTweenStartTime;
		float num2 = 0.25f;
		if (num < 0f)
		{
			return;
		}
		if (num < num2)
		{
			float num3 = Easing.Ease(Easing.EaseType.EaseInCircle, num, num2, 0f, 1f);
			wobblyChest.transform.localScale = Vector3.Lerp(new Vector3(4f, 4f, 4f), new Vector3(1f, 1f, 1f), num3);
			wobblyChest.alpha = num3;
			rewardNumberContainer.alpha = num3;
			for (int i = 0; i < chestShinies.Length; i++)
			{
				chestShinies[i].alpha = num3;
			}
			return;
		}
		Debug.Log("Set wobbly chest visible");
		wobblyChest.transform.localScale = Vector3.one;
		wobblyChest.alpha = 1f;
		rewardNumberContainer.alpha = 1f;
		for (int j = 0; j < chestShinies.Length; j++)
		{
			chestShinies[j].alpha = 1f;
		}
		_tweeningRewardChestIn = false;
	}

	private void UpdateTweeningQuestButton()
	{
		if (!_tweeningQuestButtonIn)
		{
			return;
		}
		float num = Time.time - _questButtonTweenStartTime;
		float num2 = 0.25f;
		if (!(num < 0f))
		{
			if (num < num2)
			{
				float num3 = Easing.Ease(Easing.EaseType.EaseInCircle, num, num2, 0f, 1f);
				questWrapper.transform.localScale = Vector3.Lerp(new Vector3(4f, 4f, 4f), new Vector3(1f, 1f, 1f), num3);
				questWrapper.alpha = num3;
			}
			else
			{
				Debug.Log("Done bringing quest in");
				questWrapper.transform.localScale = Vector3.one;
				questWrapper.alpha = 1f;
				questWrapper.enabled = true;
				_tweeningQuestButtonIn = false;
			}
		}
	}

	public void OnCityReady()
	{
		if (Facades<FlowFacade>.Instance != null && CameraHUB._pInstance != null && Facades<FlowFacade>.Instance.CurrentLocation == "MinigameInfo")
		{
			CameraHUB._pInstance._pCameraControllable = false;
		}
		if (!GlobalInGameData._pHasSeenEXPTutorial)
		{
			_canTweenInBrickbagReward = false;
		}
		int num = (_currentLevel = RewardManager._pInstance.GetLevelFromEXP(GlobalInGameData._pCurrentExp));
		xpLevel.text = num.ToString();
		currentScenarioLabel.text = Localise(ScenarioManager._pInstance._pCurrentScenario.scenarioName);
		RefreshRewardsLabel();
		SoundFacade._pInstance.PlayMusic("FrontEnd", 0f);
		if (!ScenarioManager._pInstance._pIsFreshSave)
		{
			ScreenLoading._pInstance.DoNavigate("Hub");
		}
		else
		{
			ScreenLoading._pInstance.DoNavigate("FTUEWelcome");
		}
		if (CityManager._pInstance != null)
		{
			CityManager._pInstance.minigameMarkerRoot.SetActive(true);
		}
	}

	protected override void OnExitScreen()
	{
		base.OnExitScreen();
		if (_isFTUEPromptUp)
		{
			_ftueMessageBox.TweenOut();
			int num = _ftueAnimBits.Length;
			for (int i = 0; i < num; i++)
			{
				_ftueAnimBits[i].TweenOut();
			}
		}
		if (_isFTUEBackgroundUp)
		{
			_ftueBackground.TweenOut();
		}
		if (GlobalInGameData._pHasDoneFirstTimeVisit)
		{
			_expBarTransitionWidget.TweenOut();
		}
	}

	public void ShowFinalTutorial()
	{
		if (!_isFTUEPromptUp)
		{
			CameraHUB._pInstance._pCameraControllable = false;
			CameraHUB._pInstance.FocusPoint(_pGarageLocation, CameraHUB.EFocusType.PAN_ONLY, 44f);
			int num = _ftueAnimBits.Length;
			for (int i = 0; i < num; i++)
			{
				_ftueAnimBits[i].SetStateConsideredTweenedIn(false);
				_ftueAnimBits[i].SetToTweenInStartScale();
				_ftueAnimBits[i].TweenIn();
			}
			_ftueMessageBox.SetStateConsideredTweenedIn(false);
			_ftueMessageBox.SetToTweenInStartPos();
			_ftueMessageBox.TweenIn();
			_isFTUEPromptUp = true;
			_ftueTitle.text = Localise("FTUE.FinalTutorialTitle");
			_ftueLabel.text = Localise("FTUE.FinalTutorial");
			if (Facades<TrackingFacade>.Instance != null)
			{
				Facades<TrackingFacade>.Instance.LogMetric("7.Continue Building", "FTUE");
				Facades<TrackingFacade>.Instance.LogProgress("FTUE_7_Building");
			}
			_expBarTransitionWidget.TweenIn();
		}
	}

	public void ShowGarageTutorial()
	{
		GlobalInGameData._pHasSeenGarageTutorial = true;
		TrackableIcon.RefreshAllVisibility();
		ShowFinalTutorial();
	}

	public void OnZoomBack()
	{
		CameraHUB._pInstance.RestoreNormalZoomBackButton();
	}

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		UpdateMiniChest(0);
		bool flag = false;
		bool flag2 = true;
		if (!usesCompleteIcons)
		{
			templateCompleteIcon.gameObject.SetActive(false);
		}
		_zoomInBack.SetStateConsideredTweenedIn(false);
		_zoomInBack.SetToTweenInStartPos();
		if (GlobalInGameData._pHasCompletedFTUE)
		{
			_ftueMessageBox.SetToTweenInStartPos();
			_expBarTransitionWidget.TweenIn();
		}
		else if (GlobalInGameData._pHasDoneFirstTimeVisit)
		{
			if (!GlobalInGameData._pHasSeenEXPTutorial)
			{
				Debug.Log("FTUE - EXP");
				int num = _ftueAnimBits.Length;
				for (int i = 0; i < num; i++)
				{
					_ftueAnimBits[i].SetStateConsideredTweenedIn(false);
					_ftueAnimBits[i].SetToTweenInStartScale();
					_ftueAnimBits[i].TweenIn();
				}
				_ftueMessageBox.SetStateConsideredTweenedIn(false);
				_ftueMessageBox.SetToTweenInStartPos();
				_ftueMessageBox.TweenIn();
				_ftueTitle.text = Localise("FTUE.ExpInfoTitle");
				_ftueLabel.text = Localise("FTUE.ExpInfo");
				if (Facades<TrackingFacade>.Instance != null)
				{
					Facades<TrackingFacade>.Instance.LogMetric("4.XP and Chests", "FTUE");
					Facades<TrackingFacade>.Instance.LogProgress("FTUE_4_XP_Chests");
				}
				_isFTUEPromptUp = true;
				flag = true;
				flag2 = false;
				_expBarTransitionWidget.SetToTweenInStartPos();
				_canTweenInBrickbagReward = false;
			}
			else
			{
				bool pHasSeenGarageTutorial = GlobalInGameData._pHasSeenGarageTutorial;
				Debug.Log("FTUE - FINAL");
				flag = true;
				ShowFinalTutorial();
			}
		}
		else
		{
			Debug.Log("FTUE - FIRST");
			int num2 = _ftueAnimBits.Length;
			for (int j = 0; j < num2; j++)
			{
				_ftueAnimBits[j].SetStateConsideredTweenedIn(false);
				_ftueAnimBits[j].SetToTweenInStartScale();
				_ftueAnimBits[j].TweenIn();
			}
			_ftueMessageBox.SetStateConsideredTweenedIn(false);
			_ftueMessageBox.SetToTweenInStartPos();
			_ftueMessageBox.TweenIn();
			_ftueTitle.text = Localise("FTUE.TimeToStartTitle");
			_ftueLabel.text = Localise("FTUE.TimeToStart");
			if (Facades<TrackingFacade>.Instance != null)
			{
				Facades<TrackingFacade>.Instance.LogMetric("3.Mini-Game Select", "FTUE");
				Facades<TrackingFacade>.Instance.LogProgress("FTUE_3_MiniGame_Select");
			}
			_isFTUEPromptUp = true;
			flag = true;
			flag2 = false;
			_expBarTransitionWidget.SetToTweenInStartPos();
			_canTweenInBrickbagReward = false;
		}
		RewardManager._pInstance.LoadHistory();
		int num3 = QuestHandler._pInstance.SeekNewQuests();
		int numActiveQuests = QuestSystem._pInstance.GetNumActiveQuests();
		int num4 = QuestHandler._pInstance.NumRewardsToClaim();
		bool flag3 = num3 > 0;
		if (flag3)
		{
			ScreenQuests._numNewQuests = Mathf.Max(num3, ScreenQuests._numNewQuests);
		}
		else
		{
			ScreenQuests._numNewQuests = Mathf.Max(0, ScreenQuests._numNewQuests);
		}
		if (numActiveQuests == 0 && num4 == 0)
		{
			questCount.enabled = false;
			questCountCircle.enabled = false;
			questIcon.enabled = false;
			questButton.isEnabled = false;
			completedTick.enabled = false;
			questNewIcon.enabled = false;
		}
		else
		{
			questCount.enabled = num4 == 0 && !flag3 && ScreenQuests._numNewQuests <= 0;
			questCountCircle.enabled = num4 == 0 && !flag3 && ScreenQuests._numNewQuests <= 0;
			questIcon.enabled = true;
			questButton.isEnabled = true;
			questCount.text = numActiveQuests.ToString();
			questNewIcon.enabled = (flag3 || ScreenQuests._numNewQuests > 0) && num4 == 0;
			completedTick.enabled = num4 > 0;
			if (num4 > 0)
			{
				if (questIconAnimator != null)
				{
					questIconAnimator.enabled = true;
				}
				if (questIconPeriodicWiggle != null)
				{
					questIconPeriodicWiggle.enabled = true;
				}
				questWrapper.alpha = 0.004f;
				_questButtonTweenStartTime = Time.time + 0.75f;
				_tweeningQuestButtonIn = true;
			}
			else
			{
				if (flag3 || ScreenQuests._numNewQuests > 0)
				{
					if (questIconAnimator != null)
					{
						questIconAnimator.enabled = true;
					}
					if (questIconPeriodicWiggle != null)
					{
						questIconPeriodicWiggle.enabled = true;
					}
				}
				else
				{
					if (questIconAnimator != null)
					{
						questIconAnimator.enabled = false;
					}
					if (questIconPeriodicWiggle != null)
					{
						questIconPeriodicWiggle.enabled = false;
					}
					questIcon.transform.localRotation = Quaternion.identity;
				}
				questWrapper.alpha = 1f;
			}
		}
		CameraHUB._pInstance._pCameraControllable = !flag;
		_ftueBackgroundCollider.enabled = flag;
		if (flag)
		{
			_isFTUEBackgroundUp = true;
			_ftueBackground.SetToTweenInStartColor();
			_ftueBackground.TweenIn();
		}
		else
		{
			_ftueBackground.SetToTweenInStartColor();
		}
		_zoomInfoBarWidget.SetStateConsideredTweenedIn(false);
		_zoomInfoBarWidget.SetToTweenInStartPos();
		currentScenarioLabel.text = Localise(ScenarioManager._pInstance._pCurrentScenario.scenarioName);
		brickIcon.material = ScenarioManager._pInstance._pCurrentScenario.brickMaterial;
		int num5 = floatingProgressInfo.Length;
		for (int k = 0; k < num5; k++)
		{
			floatingProgressInfo[k].SetAlpha(0f);
		}
		if (flag2)
		{
			RefreshEXP();
		}
		if (DailyRewardsManager.Instance.processDailyRewards(false))
		{
			_wantDailyReward = true;
		}
		SoundFacade._pInstance.PlayOneShotSFX("GUIBang", 0.1f);
		SoundFacade._pInstance.PlayOneShotSFX("GUIPop", 0.25f);
		SoundFacade._pInstance.PlayOneShotSFX("GUIPop", 0.5f);
		questWrapper.gameObject.SetActive(GlobalInGameData._pHasCompletedFTUE);
		RefreshRewardsLabel();
		UpdateProgressFloaters();
		UpdateCompletedIcons();
		UpdateTrackerIcons();
	}

	protected override void OnScreenShowComplete()
	{
		base.OnScreenShowComplete();
		TryChangeWidgetSets(base.gameObject, "Default");
		UpdateProgressFloaters();
		UpdateCompletedIcons();
		UpdateTrackerIcons();
		if (_doTourOfTheCity)
		{
			_doTourOfTheCity = false;
			Navigate("ScreenTourOfTheCity");
		}
	}

	protected override void OnScreenExitComplete()
	{
		base.OnScreenExitComplete();
		rewardChestContainer.SetActive(false);
		if (_wantFlushLevelOnScreenOut)
		{
			_wantFlushLevelOnScreenOut = false;
			GlobalInGameData.OnLevelWillLoad("Empty", ScreenLoading._pCurrentLevelName);
			ScreenBase.LoadEmptyScene();
			GlobalInGameData.OnLevelWillLoad("Empty", ScreenLoading._pCurrentLevelName);
			ScreenBase.LoadEmptyScene();
		}
	}

	public void RefreshAfterRewards()
	{
		_showingBrickBagReward = false;
		RefreshRewardsLabel();
		CityManager.ECITYBUILDINGS building = ScenarioManager._pInstance._pCurrentScenario.building;
		CityManager._pInstance.SetCurrentSelection(building);
		if (ScenarioManager._pInstance._pCurrentScenario._pCurrentBricks >= ScenarioManager._pInstance._pCurrentScenario.totalBricksRequired)
		{
			ScenarioComplete(building);
		}
		else
		{
			ScenarioUpdated(building);
		}
	}

	public void OnSideMarkerClicked(GameObject obj)
	{
		Debug.Log(string.Concat("Clicked: ", obj, " CameraState=", CameraHUB._pInstance._pCurFocusType));
		if (CameraHUB._pInstance._pCurFocusType == CameraHUB.EFocusType.NONE && !CameraHUB._pInstance._pZoomingCamera)
		{
			Vector3 point = _markerLookup[obj];
			CameraHUB._pInstance.FocusPoint(point, CameraHUB.EFocusType.PAN_ONLY, 44f);
		}
	}

	private void RefreshRewardsLabel()
	{
		if (!_canTweenInBrickbagReward)
		{
			wobblyChest.alpha = 0f;
			rewardChestContainer.SetActive(false);
			return;
		}
		bool activeInHierarchy = rewardChestContainer.activeInHierarchy;
		bool flag = (GlobalInGameData._pClaimedRewards < _currentLevel || GlobalInGameData._pUnclaimedDailyRewardChests > 0) && !_showingBrickBagReward;
		rewardChestContainer.SetActive(flag);
		int level = GlobalInGameData._pClaimedRewards;
		if (GlobalInGameData._pClaimedRewards >= _currentLevel)
		{
			level = 1;
		}
		if (activeInHierarchy != flag && flag)
		{
			wobblyChest.alpha = 0f;
			_rewardChestTweenStartTime = Time.time + 0.5f;
			_tweeningRewardChestIn = true;
			Debug.Log("Set reward chest tween in true");
			rewardNumberContainer.alpha = 0f;
			for (int i = 0; i < chestShinies.Length; i++)
			{
				chestShinies[i].alpha = 0f;
			}
		}
		UpdateWobblyChest(level);
		int num = _currentLevel - GlobalInGameData._pClaimedRewards + GlobalInGameData._pUnclaimedDailyRewardChests;
		rewardsRemainingLabel.text = num.ToString();
	}

	public void OnCompleteBasicCameraPan()
	{
	}

	public void OnBack()
	{
		if (!_isFTUEPromptUp && base._pCurrentTweenType == ScreenTweenType.Idle)
		{
			SoundFacade._pInstance.PlayOneShotSFX("GUIBack", 0f);
			_wantFlushLevelOnScreenOut = true;
			Navigate("Title");
		}
	}

	public void OnSigninRequest()
	{
		Navigate("Signin");
	}

	public void OnSocial()
	{
		Navigate("SocialSignin");
	}

	public void OnScenario()
	{
		if (!_isFTUEPromptUp)
		{
			SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
			ScreenSelectScenario.selectScenarioCanGoBack = true;
			Navigate("SelectScenario");
		}
	}

	public void PrepareScenarioComplete()
	{
		_preparingScenarioComplete = true;
		int num = buttonsToDisableOnScenarioComplete.Length;
		Debug.Log("PrepareScenarioComplete()");
		for (int i = 0; i < num; i++)
		{
			if (!(buttonsToDisableOnScenarioComplete[i] == null))
			{
				buttonsToDisableOnScenarioComplete[i].isEnabled = false;
			}
		}
	}

	public void UnlockScenarioButtons()
	{
		Debug.Log("UnlockScenarioButtons()");
		_preparingScenarioComplete = false;
		int num = buttonsToDisableOnScenarioComplete.Length;
		for (int i = 0; i < num; i++)
		{
			buttonsToDisableOnScenarioComplete[i].isEnabled = true;
		}
	}

	public void OnWantCompleteScenario()
	{
		Debug.Log("OnWantCompleteScenario()");
		_wantScenarioComplete = true;
	}

	public void OnNewDailyReward()
	{
		if (!Facades<ScreenFacade>.Instance._pIsAnyScreenTweening && !(Facades<FlowFacade>.Instance.CurrentLocation != "Hub"))
		{
			_wantDailyReward = false;
			Navigate("DailyRewards");
		}
	}

	public void OnScenarioComplete()
	{
		if (Facades<ScreenFacade>.Instance._pIsAnyScreenTweening || Facades<FlowFacade>.Instance.CurrentLocation != "Hub")
		{
			_wantScenarioComplete = true;
			return;
		}
		Debug.Log("Navigating - Scenario Completed");
		_wantScenarioComplete = false;
		Navigate("ScenarioCompleted");
	}

	public void OnSettings()
	{
		if (!_isFTUEPromptUp)
		{
			SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
			Navigate("Settings");
		}
	}

	public void OnBrickBagReward()
	{
		if (!Facades<ScreenFacade>.Instance._pIsAnyScreenTweening)
		{
			CameraHUB._pInstance._pCameraControllable = false;
			_showingBrickBagReward = true;
			GlobalInGameData._pHasSeenEXPTutorial = true;
			if (_isFTUEBackgroundUp)
			{
				_ftueBackground.TweenOut();
				_ftueBackgroundCollider.enabled = false;
				_isFTUEBackgroundUp = false;
			}
			rewardChestContainer.SetActive(GlobalInGameData._pClaimedRewards < _currentLevel && !_showingBrickBagReward);
			Navigate("BrickBagReward");
		}
	}

	public void OnProgressPage()
	{
		if (!_isFTUEPromptUp)
		{
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setBool("FTUE_XPBarFlasher", true);
			CameraHUB._pInstance._pCameraControllable = false;
			Navigate("ProgressInfo");
		}
	}

	public void OnQuestPage()
	{
		if (!_isFTUEPromptUp)
		{
			CameraHUB._pInstance._pCameraControllable = false;
			Navigate("QuestScreen");
		}
	}

	public void OnGarage()
	{
		MinigameManager._pInstance._pCurrentMinigameType = MinigameManager.EMINIGAME_TYPE.INVALID;
		GlobalInGameData._pWantFullCarousel = true;
		Navigate("Garage");
	}

	public void OnMinigame(GameObject obj)
	{
		GlobalInGameData._pWantFullCarousel = false;
		SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		MinigameIcon component = obj.GetComponent<MinigameIcon>();
		if (component == null)
		{
			Debug.LogError("Minigame object clicked on does not have a MinigameIcon script on it!");
			MinigameManager._pInstance._pCurrentMinigameType = MinigameManager.EMINIGAME_TYPE.CATCH_THE_CROOKS;
		}
		else
		{
			if (component.minigameType == MinigameManager.EMINIGAME_TYPE.INVALID)
			{
				return;
			}
			MinigameManager._pInstance._pCurrentMinigameType = component.minigameType;
		}
		ScreenBase screen = Facades<ScreenFacade>.Instance.GetScreen("ScreenMinigameInfoV2");
		if (screen != null)
		{
			((ScreenMinigameInfoV2)screen).SetTransitionForward();
		}
		Navigate("MinigameInfo");
	}

	private void ScenarioComplete(CityManager.ECITYBUILDINGS building)
	{
		PrepareScenarioComplete();
		BeginTweeningBricks(0f);
		if (CityManager._pInstance != null)
		{
			CityManager._pInstance.PanCameraToBuilding(building, CameraHUB.EFocusType.SCENARIO_COMPLETE);
		}
	}

	private void ScenarioUpdated(CityManager.ECITYBUILDINGS building)
	{
		float pProgress = ScenarioManager._pInstance._pCurrentScenario._pProgress;
		float pLastSeenProgression = ScenarioManager._pInstance._pCurrentScenario._pLastSeenProgression;
		if (CityManager._pInstance != null)
		{
			if (CityManager._pInstance.HasBuildingMadeBuildProgress(building, pProgress, pLastSeenProgression))
			{
				Debug.Log("Progress was made!");
				BeginTweeningBricks(1.75f);
				CityManager._pInstance.PanCameraToBuilding(building, CameraHUB.EFocusType.PARTIAL_PROGRESS);
				Debug.Log("Made progress!");
			}
			else
			{
				RefreshBricks();
				Debug.Log("Progress was not made");
				CityManager._pInstance.SetupBuildingWithProgress(building, pProgress);
				CityManager._pInstance.PanCameraToBuilding(building, CameraHUB.EFocusType.PAN_ONLY);
			}
		}
	}

	private void UpdateMiniChest(int level)
	{
		switch (RewardManager._pInstance.GetRewardChestForLevel(level))
		{
		case RewardManager.EBRICK_BAG_CATEGORY.LARGE_CHEST:
			miniChest.spriteName = "chestSmall_Gold";
			break;
		case RewardManager.EBRICK_BAG_CATEGORY.MEDIUM_CHEST:
			miniChest.spriteName = "chestSmall_Silver";
			break;
		case RewardManager.EBRICK_BAG_CATEGORY.SMALL_CHEST:
			miniChest.spriteName = "chestSmall_Bronze";
			break;
		}
	}

	private void UpdateWobblyChest(int level)
	{
		switch (RewardManager._pInstance.GetRewardChestForLevel(level))
		{
		case RewardManager.EBRICK_BAG_CATEGORY.LARGE_CHEST:
			wobblyChest.spriteName = "chestLarge_Gold";
			break;
		case RewardManager.EBRICK_BAG_CATEGORY.MEDIUM_CHEST:
			wobblyChest.spriteName = "chestLarge_Silver";
			break;
		case RewardManager.EBRICK_BAG_CATEGORY.SMALL_CHEST:
			wobblyChest.spriteName = "chestLarge_Bronze";
			break;
		}
	}

	private void UpdateCompletedIcons()
	{
		if (!usesCompleteIcons || !CameraHUB._pExists)
		{
			return;
		}
		int num = ScenarioManager._pInstance.allScenarios.Length;
		_dummyRect1.x = completeIconRegion.worldCorners[1].x;
		_dummyRect1.y = completeIconRegion.worldCorners[0].y;
		_dummyRect1.width = completeIconRegion.worldCorners[2].x - completeIconRegion.worldCorners[0].x;
		_dummyRect1.height = completeIconRegion.worldCorners[1].y - completeIconRegion.worldCorners[0].y;
		int num2 = 0;
		int num3 = 500;
		num3 += num * 2;
		if (_numCompleteIcons < num)
		{
			templateCompleteIcon.gameObject.SetActive(true);
			int num4 = num - _numCompleteIcons;
			for (num2 = 0; num2 < num4; num2++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(templateCompleteIcon.gameObject);
				gameObject.transform.parent = templateCompleteIcon.transform.parent;
				gameObject.transform.localScale = templateCompleteIcon.transform.localScale;
				gameObject.transform.position = templateCompleteIcon.transform.position;
				UIWidget component = gameObject.GetComponent<UIWidget>();
				component.depth = num3;
				num3++;
				_completeIcons.Add(component);
			}
			_numCompleteIcons = num;
		}
		_dummyPlane.SetNormalAndPosition(CameraHUB._pInstance._pCameraRef.transform.forward, CameraHUB._pInstance._pCameraRef.transform.position);
		for (num2 = 0; num2 < num; num2++)
		{
			_completeIcons[num2].enabled = ScenarioManager._pInstance.allScenarios[num2]._pProgress == 1f;
			if (_completeIcons[num2].enabled)
			{
				Vector3 buildingProgressPos = CityManager._pInstance.GetBuildingProgressPos(ScenarioManager._pInstance.allScenarios[num2].building);
				Vector3 position = CameraHUB._pInstance._pCameraRef.WorldToViewportPoint(buildingProgressPos);
				if (!_dummyPlane.GetSide(buildingProgressPos))
				{
					position.x = 0f - position.x;
					position.y = 0f - position.y;
				}
				Vector3 position2 = ScreenRoot._pInstance._pUiCam.ViewportToWorldPoint(position);
				position2.z = 0f;
				_completeIcons[num2].transform.position = position2;
				bool flag = _dummyRect1.Contains(_completeIcons[num2].transform.position);
				_completeIcons[num2].enabled = flag;
			}
		}
		templateCompleteIcon.gameObject.SetActive(false);
	}

	public void UpdateProgressFloaters()
	{
		if (!CameraHUB._pExists)
		{
			return;
		}
		int num = floatingProgressInfo.Length;
		_dummyRect1.x = scenarioProgressRegion.worldCorners[1].x;
		_dummyRect1.y = scenarioProgressRegion.worldCorners[0].y;
		_dummyRect1.width = scenarioProgressRegion.worldCorners[2].x - scenarioProgressRegion.worldCorners[0].x;
		_dummyRect1.height = scenarioProgressRegion.worldCorners[1].y - scenarioProgressRegion.worldCorners[0].y;
		_dummyPlane.SetNormalAndPosition(CameraHUB._pInstance._pCameraRef.transform.forward, CameraHUB._pInstance._pCameraRef.transform.position);
		_anyProgressFloaterMidUpdate = true;
		for (int i = 0; i < num; i++)
		{
			Scenario slottedScenario = ScenarioManager._pInstance.GetSlottedScenario(i);
			CityManager.ECITYBUILDINGS building = slottedScenario.building;
			Vector3 buildingProgressPos = CityManager._pInstance.GetBuildingProgressPos(building);
			Vector3 position = CameraHUB._pInstance._pCameraRef.WorldToViewportPoint(buildingProgressPos);
			if (!_dummyPlane.GetSide(buildingProgressPos))
			{
				position.x = 0f - position.x;
				position.y = 0f - position.y;
			}
			Vector3 normalized = (new Vector3(0.5f, 0.5f, 0f) - new Vector3(position.x, 1f - position.y, 0f)).normalized;
			Vector3 up = Vector3.up;
			float num2 = Mathf.Atan2(Vector3.Dot(Vector3.forward, Vector3.Cross(up, normalized)), Vector3.Dot(up, normalized)) * 57.29578f;
			Vector3 position2 = ScreenRoot._pInstance._pUiCam.ViewportToWorldPoint(position);
			position2.z = 0f;
			floatingProgressInfo[i].transform.position = position2;
			bool flag = _dummyRect1.Contains(floatingProgressInfo[i].transform.position);
			if (slottedScenario._pProgress == 1f)
			{
				flag = false;
			}
			floatingProgressInfo[i].SetActive(flag);
			if (flag)
			{
				Vector3 position3 = floatingProgressInfo[i].transform.position;
				if (position3.x < _dummyRect1.x)
				{
					position3.x = _dummyRect1.x;
				}
				if (position3.y < _dummyRect1.y)
				{
					position3.y = _dummyRect1.y;
				}
				float num3 = _dummyRect1.y + _dummyRect1.height;
				if (position3.y > num3)
				{
					position3.y = num3;
				}
				float num4 = _dummyRect1.x + _dummyRect1.width;
				if (position3.x > num4)
				{
					position3.x = num4;
				}
				floatingProgressInfo[i].transform.position = position3;
			}
			floatingProgressInfo[i].SetInfo(slottedScenario._pCurrentBricks, slottedScenario.totalBricksRequired, slottedScenario.brickMaterial);
			if (floatingProgressInfo[i].DoUpdate())
			{
				_anyProgressFloaterMidUpdate = true;
			}
		}
	}

	private void UpdateTrackerIcons()
	{
		if (!CameraHUB._pExists)
		{
			return;
		}
		_trackerIconFadeDirty = false;
		int pNumTrackables = TrackableIcon._pNumTrackables;
		_dummyRect1.x = pointerIconRegion.worldCorners[1].x;
		_dummyRect1.y = pointerIconRegion.worldCorners[0].y;
		_dummyRect1.width = pointerIconRegion.worldCorners[2].x - pointerIconRegion.worldCorners[0].x;
		_dummyRect1.height = pointerIconRegion.worldCorners[1].y - pointerIconRegion.worldCorners[0].y;
		_dummyRect2.x = screenRegion.worldCorners[1].x;
		_dummyRect2.y = screenRegion.worldCorners[0].y;
		_dummyRect2.width = screenRegion.worldCorners[2].x - screenRegion.worldCorners[0].x;
		_dummyRect2.height = screenRegion.worldCorners[1].y - screenRegion.worldCorners[0].y;
		int num = 0;
		int num2 = 500;
		num2 += pNumTrackables * 2;
		if (_numTrackerIcons < pNumTrackables)
		{
			templateTrackerIcon.gameObject.SetActive(true);
			int num3 = pNumTrackables - _numTrackerIcons;
			for (num = 0; num < num3; num++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(templateTrackerIcon.gameObject);
				gameObject.transform.parent = templateTrackerIcon.transform.parent;
				gameObject.transform.localScale = templateTrackerIcon.transform.localScale;
				gameObject.transform.position = templateTrackerIcon.transform.position;
				UIWidget component = gameObject.GetComponent<UIWidget>();
				component.depth = num2;
				num2++;
				UISprite componentInChildren = gameObject.transform.GetChild(0).GetComponentInChildren<UISprite>();
				componentInChildren.depth = num2;
				num2++;
				_trackerIcons.Add(component);
			}
			_numTrackerIcons = pNumTrackables;
		}
		_dummyPlane.SetNormalAndPosition(CameraHUB._pInstance._pCameraRef.transform.forward, CameraHUB._pInstance._pCameraRef.transform.position);
		CityManager.REGIONS pRegion = CameraHUB._pInstance._pRegion;
		float z = CameraHUB._pInstance._pCameraRef.nearClipPlane + 64f;
		Vector3 vector = CameraHUB._pInstance._pCameraRef.ViewportToWorldPoint(new Vector3(0f, 0f, z));
		Vector3 vector2 = CameraHUB._pInstance._pCameraRef.ViewportToWorldPoint(new Vector3(1f, 0f, z));
		Vector3 normalized = (vector - vector2).normalized;
		for (num = 0; num < pNumTrackables; num++)
		{
			if (TrackableIcon._pAllIcons[num].region != pRegion)
			{
				_trackerIcons[num].gameObject.SetActive(false);
				continue;
			}
			Vector3 position = TrackableIcon._pAllIcons[num].transform.position;
			float sqrMagnitude = (TrackableIcon._pAllIcons[num]._pRootPos - CameraHUB._pInstance._pCameraRef.transform.position).sqrMagnitude;
			bool flag = sqrMagnitude < 56000f;
			_markerLookup[_trackerIcons[num].gameObject] = TrackableIcon._pAllIcons[num]._pRootPos;
			if (TrackableIcon._pAllIcons[num] is MinigameIcon)
			{
				MinigameIcon minigameIcon = TrackableIcon._pAllIcons[num] as MinigameIcon;
				if (minigameIcon.minigameType == MinigameManager.EMINIGAME_TYPE.CATCH_THE_CROOKS)
				{
					_FTUEMissionIconLocation = TrackableIcon._pAllIcons[num]._pRootPos;
				}
			}
			else if (TrackableIcon._pAllIcons[num] is SpecialActionIcon)
			{
				SpecialActionIcon specialActionIcon = TrackableIcon._pAllIcons[num] as SpecialActionIcon;
				if (specialActionIcon.actionType == SpecialActionIcon.EACTION_TYPE.GARAGE)
				{
					_FTUEGarageIconLocation = TrackableIcon._pAllIcons[num]._pRootPos;
				}
			}
			_FTUEGarageIconLocation = _centralLocationNoGarage;
			Vector3 position2 = CameraHUB._pInstance._pCameraRef.WorldToViewportPoint(position);
			bool flag2 = !_dummyPlane.GetSide(position);
			if (flag2)
			{
				position2.x = 0f - position2.x;
				position2.y = 0f - position2.y;
			}
			Vector3 position3 = ScreenRoot._pInstance._pUiCam.ViewportToWorldPoint(position2);
			position3.z = 0f;
			_trackerIcons[num].transform.position = position3;
			bool flag3 = _dummyRect1.Contains(_trackerIcons[num].transform.position);
			if (flag)
			{
				if (flag2)
				{
					flag = false;
				}
				Vector3 position4 = _trackerIcons[num].transform.position;
				if (position4.y < _dummyRect2.y)
				{
					flag = false;
				}
				if (position4.x < _dummyRect2.x)
				{
					flag = false;
				}
				if (position4.x > _dummyRect2.x + _dummyRect2.width)
				{
					flag = false;
				}
			}
			_trackerIcons[num].gameObject.SetActive(!flag && !flag3);
			if (!flag3)
			{
				Vector3 position5 = _trackerIcons[num].transform.position;
				if (position5.y < _dummyRect1.y)
				{
					position5.y = _dummyRect1.y;
					Vector3 vector3 = vector - TrackableIcon._pAllIcons[num].transform.position;
					Vector3 position6 = vector - Vector3.Project(vector3, normalized);
					position2 = CameraHUB._pInstance._pCameraRef.WorldToViewportPoint(position6);
					position3 = ScreenRoot._pInstance._pUiCam.ViewportToWorldPoint(position2);
					position3.z = 0f;
					position5.x = position3.x;
				}
				if (position5.x < _dummyRect1.x)
				{
					position5.x = _dummyRect1.x;
				}
				float num4 = _dummyRect1.y + _dummyRect1.height;
				if (position5.y > num4)
				{
					position5.y = num4;
				}
				float num5 = _dummyRect1.x + _dummyRect1.width;
				if (position5.x > num5)
				{
					position5.x = num5;
				}
				Vector3 normalized2 = (new Vector3(0.5f, 0.5f, 0f) - new Vector3(position2.x, 1f - position2.y, 0f)).normalized;
				Vector3 up = Vector3.up;
				float num6 = Mathf.Atan2(Vector3.Dot(Vector3.forward, Vector3.Cross(up, normalized2)), Vector3.Dot(up, normalized2)) * 57.29578f;
				_trackerIcons[num].transform.position = position5;
				Vector3 localEulerAngles = _trackerIcons[num].transform.localEulerAngles;
				localEulerAngles.z = 0f - num6;
				_trackerIcons[num].transform.localEulerAngles = localEulerAngles;
				Transform child = _trackerIcons[num].transform.GetChild(0);
				child.up = Vector3.up;
				UISprite componentInChildren2 = child.GetComponentInChildren<UISprite>();
				if (componentInChildren2 != null)
				{
					componentInChildren2.atlas = FindAtlas(TrackableIcon._pAllIcons[num].trackableIconName);
					componentInChildren2.spriteName = TrackableIcon._pAllIcons[num].trackableIconName;
				}
				UISprite component2 = _trackerIcons[num].GetComponent<UISprite>();
				component2.spriteName = TrackableIcon._pAllIcons[num].trackableIconBacking;
				bool flag4 = CameraHUB._pInstance._pZoomingCamera || CameraHUB._pInstance._pCurFocusType == CameraHUB.EFocusType.MANUAL;
				if (flag4 && _trackerIcons[num].alpha != 0f)
				{
					_trackerIcons[num].alpha = Mathf.MoveTowards(_trackerIcons[num].alpha, 0f, Time.deltaTime * 15f);
					_trackerIconFadeDirty = true;
				}
				else if (!flag4 && _trackerIcons[num].alpha != 1f)
				{
					_trackerIcons[num].alpha = Mathf.MoveTowards(_trackerIcons[num].alpha, 1f, Time.deltaTime * 15f);
					_trackerIconFadeDirty = true;
				}
			}
		}
		for (; num < _numTrackerIcons; num++)
		{
			_trackerIcons[num].gameObject.SetActive(false);
		}
		templateTrackerIcon.gameObject.SetActive(false);
	}

	private void ClearReferences()
	{
		_markerLookup.Clear();
		_markerLookup = new Dictionary<GameObject, Vector3>();
	}

	private UIAtlas FindAtlas(string spriteName)
	{
		int num = iconAtlases.Length;
		for (int i = 0; i < num; i++)
		{
			UISpriteData sprite = iconAtlases[i].GetSprite(spriteName);
			if (sprite != null)
			{
				return iconAtlases[i];
			}
		}
		return null;
	}

	public void ResetAll()
	{
		_wantScenarioComplete = false;
		_wantDailyReward = false;
		_wantFlushLevelOnScreenOut = false;
		_lastKnownEXP = 0;
		_preparingScenarioComplete = false;
		_showingBrickBagReward = false;
		_currentLevel = 0;
		UpdateMiniChest(0);
		UpdateWobblyChest(0);
	}

	private float Frac(float v)
	{
		return v - Mathf.Floor(v);
	}
}
