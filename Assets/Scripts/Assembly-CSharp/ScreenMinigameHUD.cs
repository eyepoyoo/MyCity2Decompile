using System;
using System.Collections;
using System.Collections.Generic;
using GameDefines;
using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

public class ScreenMinigameHUD : ScreenBase
{
	private enum EGameOverSequenceState
	{
		None = 0,
		SlowingDown = 1,
		TweeningPanel = 2,
		Finished = 3
	}

	[Serializable]
	public class MinigameBespokeElements
	{
		public MinigameManager.EMINIGAME_TYPE minigameType;

		public UIWidget[] elements;

		public bool displayAsPercentageProgress;
	}

	private const float MIN_BAR_DECIMAL = 0.06f;

	private static readonly Dictionary<VehiclePart.EUNIQUE_ID, string> _vehiclePartIdToSpriteLookup = new Dictionary<VehiclePart.EUNIQUE_ID, string>
	{
		{
			VehiclePart.EUNIQUE_ID.ACCSESORY_BUGGY_SIREN,
			"PropATVSiren"
		},
		{
			VehiclePart.EUNIQUE_ID.ACCESSORY_HOVERCRAFT_PROPELLER,
			"PropHovercrProp"
		},
		{
			VehiclePart.EUNIQUE_ID.ACCSESORY_MEDIUM_ROTOR_BLADES,
			"PropPropeller"
		},
		{
			VehiclePart.EUNIQUE_ID.ACCESSORY_POLICE_SIREN,
			"PropSiren"
		},
		{
			VehiclePart.EUNIQUE_ID.ACCESSORY_WATER_JET,
			"PropWaterhose"
		},
		{
			VehiclePart.EUNIQUE_ID.ACCESSORY_LIFTER_ARM,
			"PropExplorerLifterArm"
		},
		{
			VehiclePart.EUNIQUE_ID.ACCESSORY_FIRE_ENGINE_LADDER,
			"PropFireEngineLadder"
		},
		{
			VehiclePart.EUNIQUE_ID.ACCESSORY_LARGE_HELICOPTER_ENGINE,
			"PropHelicopterEngine"
		},
		{
			VehiclePart.EUNIQUE_ID.ACCESSORY_LARGE_DIGGER_ARM,
			"PropLargeDiggerArm"
		},
		{
			VehiclePart.EUNIQUE_ID.ACCESSORY_LIFTER,
			"PropLifter"
		},
		{
			VehiclePart.EUNIQUE_ID.ACCESSORY_MINI_DIGGER_ARM,
			"PropMiniDiggerArm"
		},
		{
			VehiclePart.EUNIQUE_ID.ACCSESORY_EXPLORER_ARM,
			"PropMiniLifterArm"
		},
		{
			VehiclePart.EUNIQUE_ID.ACCESSORY_MINI_SIREN,
			"PropMiniSiren"
		},
		{
			VehiclePart.EUNIQUE_ID.ACCESSORY_PARASOL,
			"PropParasol"
		},
		{
			VehiclePart.EUNIQUE_ID.ACCESSORY_REAR_DIGGER,
			"PropRearDigger"
		},
		{
			VehiclePart.EUNIQUE_ID.ACCESSORY_SEARCH_LIGHTS,
			"PropSearchlight"
		},
		{
			VehiclePart.EUNIQUE_ID.ACCESSORY_SIREN_MAST,
			"PropSirenMast"
		},
		{
			VehiclePart.EUNIQUE_ID.ACCESSORY_SPOTLIGHT,
			"PropSpotlight"
		},
		{
			VehiclePart.EUNIQUE_ID.ACCESSORY_WATER_CANNON,
			"PropWaterCannon"
		},
		{
			VehiclePart.EUNIQUE_ID.ACCESSORY_WATER_CANNON_ARM,
			"PropWaterCannonArm"
		},
		{
			VehiclePart.EUNIQUE_ID.ACCESSORY_QUAD_SIREN,
			"QuadSiren"
		}
	};

	public Color specialButtonBackingColorNormal;

	public Color specialButtonBackingColorRecharging;

	public Color specialButtonRingColorNormal;

	public Color specialButtonRingColorRecharging;

	public Color specialButtonIconColorNormal;

	public Color specialButtonIconColorRecharging;

	public UIAtlas[] iconAtlasList;

	public EmoticonSystem emoticonSystem;

	[SerializeField]
	private TimerFX timerFX;

	[SerializeField]
	private Clock clockTimer;

	[SerializeField]
	private UILabel timeValue;

	[SerializeField]
	private UILabel studValue;

	[SerializeField]
	private UITweener studValueTween;

	[SerializeField]
	private UIWidget specialButton;

	[SerializeField]
	private UISprite specialIconSprite;

	[SerializeField]
	private UITexture specialButtonBacking;

	[SerializeField]
	private UITexture specialButtonRing;

	[SerializeField]
	private UITexture specialButtonBurst1;

	[SerializeField]
	private UITexture specialButtonBurst2;

	[SerializeField]
	private UISprite specialIconBurst1;

	[SerializeField]
	private UISprite specialIconBurst2;

	[SerializeField]
	private ParticleSystem secretAreaParticles;

	[SerializeField]
	private ObjectiveProgressBar objectiveProgressBar;

	[SerializeField]
	private UIWidget activeBoostButton;

	[SerializeField]
	private UIWidget inactiveBoostButton;

	[SerializeField]
	private UIWidget studSprite;

	[SerializeField]
	private InGameMessageNotification messagingSystem;

	[SerializeField]
	private UILabel resultLabel;

	[SerializeField]
	private UISprite resultAlphaBG;

	[SerializeField]
	private UILabel progressLabel;

	[SerializeField]
	private ParticleSystem winParticlesBase;

	[SerializeField]
	private UIWidget _progressIconBalloon;

	[SerializeField]
	private UIWidget _progressIconPlayer;

	[SerializeField]
	private UIWidget _barAnchorRight;

	[SerializeField]
	private UISprite _tickSprite;

	[SerializeField]
	private UISprite _barSprite;

	[SerializeField]
	private UITexture _steeringWheel;

	[SerializeField]
	private UISprite _leftButton;

	[SerializeField]
	private UISprite _rightButton;

	[SerializeField]
	private UITexture _brakePedal;

	[SerializeField]
	private BonusTime _bonusTime;

	[SerializeField]
	private UITexture _specialSpaceBar;

	public MinigameBespokeElements[] minigameBespokeElems;

	public Transform _resultLabelStartPosition;

	public Transform _resultLabelEndPosition;

	private bool _isControlDown_Left;

	private bool _isControlDown_Right;

	private float specialCharge;

	private Coroutine _coroutine_WaitToShowTutorial;

	private float _gameOverTimer;

	private ParticleSystem _winParticlesCopy;

	private bool _hasRegisterPlayerVehicleStuckCallbacks;

	private bool _doShowTimer;

	private bool _displayAsPercentageProgress;

	private MinigameObjective _currObjective;

	private MinigameObjective_CatchMe _balloonMinigameProgressMonitor;

	private float _rewardTickTweenStartTime;

	private bool _isTweeningRewardTick;

	private bool _doneTweenRewardTick;

	private bool _isSpecialButtonBurst;

	private float _specialButtonBurstTime;

	private bool _isSpecialIconBurst;

	private float _specialIconBurstTime;

	private float _normalisedSteeringValue;

	private float _initialSteeringValue;

	private bool _isSteering;

	private bool _usingWheel;

	private Vector2 _steeringWheelPixelPosition;

	private bool _usingLateral;

	private bool _isPromptingSpecial;

	private EGameOverSequenceState _winSequenceState;

	private EGameOverSequenceState _loseSequenceState;

	private bool _isGameOver;

	public static ScreenMinigameHUD _pInstance { get; private set; }

	private void Awake()
	{
		_pInstance = this;
		objectiveProgressBar._onChanged += delegate
		{
			UpdateProgressLabel();
		};
		AddDebugMenu();
	}

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		OnPromptSpecialChanged(false);
		ScreenRoot._pInstance.EnableMultiTouch(true);
		emoticonSystem.OnShowScreen();
		resultLabel.gameObject.SetActive(false);
		_gameOverTimer = 0f;
		_isPromptingSpecial = false;
		_isSpecialButtonBurst = false;
		_isSpecialIconBurst = false;
		ResetHUDValues();
		messagingSystem.OnShowScreen();
		_tickSprite.transform.localScale = Vector3.one;
		if (MinigameController._pInstance._pMinigame._pNormProgress >= 1f)
		{
			_tickSprite.alpha = 1f;
			_barSprite.spriteName = "progressFillGreen";
		}
		else
		{
			_tickSprite.alpha = 0f;
			_barSprite.spriteName = "progressFill";
		}
		_isTweeningRewardTick = false;
		_doneTweenRewardTick = false;
		studSprite.material = ScenarioManager._pInstance._pCurrentScenario.guiStudMaterial;
		MinigameManager.MinigameData currentMinigameData = MinigameManager._pInstance.GetCurrentMinigameData();
		SoundFacade._pInstance.PlayMusic(currentMinigameData.gameMusic, 0f);
		MinigameController._pInstance._onProgress += OnMinigameProgress;
		MinigameController._pInstance._onEnded += OnMinigameEnded;
		MinigameController._pInstance._onStudsAwarded += OnStudAwarded;
		MinigameController._pInstance._onStudsReduced += OnStudReduced;
		MinigameController._pInstance._onTimeAdded += OnTimeAdded;
		MinigameController._pInstance._onPlayerDestroyedCollateral += OnCollateralDestroyed;
		MinigameController._pInstance._onPrimaryObjectiveChanged += OnPrimaryObjectiveChanged;
		MinigameController._pInstance._onDoPromptSpecialChanged += OnPromptSpecialChanged;
		ResetOnScreenControls();
		RefreshMinigameBespokeElements();
		UpdateHUD();
		if (VehicleController_Player._pInstance._pVehicle._specialAbility != null)
		{
			VehiclePart.EUNIQUE_ID uniqueID = VehicleController_Player._pInstance._pVehicle._specialAbility.GetComponent<VehiclePart>().uniqueID;
			if (_vehiclePartIdToSpriteLookup.ContainsKey(uniqueID))
			{
				string text = _vehiclePartIdToSpriteLookup[uniqueID];
				bool flag = false;
				int num = iconAtlasList.Length;
				for (int i = 0; i < num; i++)
				{
					if (flag)
					{
						break;
					}
					UIAtlas uIAtlas = iconAtlasList[i];
					int count = uIAtlas.spriteList.Count;
					for (int j = 0; j < count; j++)
					{
						if (flag)
						{
							break;
						}
						if (uIAtlas.spriteList[j].name == text)
						{
							specialIconSprite.atlas = uIAtlas;
							specialIconBurst1.atlas = uIAtlas;
							specialIconBurst2.atlas = uIAtlas;
							flag = true;
							break;
						}
					}
				}
				specialIconSprite.spriteName = _vehiclePartIdToSpriteLookup[uniqueID];
				specialIconBurst1.spriteName = specialIconSprite.spriteName;
				specialIconBurst2.spriteName = specialIconSprite.spriteName;
				specialIconSprite.gameObject.SetActive(true);
				inactiveBoostButton.gameObject.SetActive(true);
			}
			else
			{
				Debug.LogError(string.Concat("Attachment [", uniqueID, "] is not in the sprite lookup!"));
				specialIconSprite.gameObject.SetActive(false);
				inactiveBoostButton.gameObject.SetActive(true);
			}
		}
		else
		{
			Debug.LogWarning("No vehicle attachment!");
			specialIconSprite.gameObject.SetActive(false);
			inactiveBoostButton.gameObject.SetActive(false);
		}
		objectiveProgressBar.ResetProgressBar();
		RegisterPlayerVehicleStuckCallbacks();
		_isGameOver = false;
		_winSequenceState = EGameOverSequenceState.None;
		_loseSequenceState = EGameOverSequenceState.None;
		MinigameController._pInstance.PlayIntroAndStartMinigames();
		_doShowTimer = !float.IsPositiveInfinity(MinigameController._pInstance._timeLimit);
		timerFX.gameObject.SetActive(_doShowTimer);
		_bonusTime.OnShowScreen();
		MinigameManager.EMINIGAME_TYPE pCurrentMinigameType = MinigameManager._pInstance._pCurrentMinigameType;
		if (pCurrentMinigameType == MinigameManager.EMINIGAME_TYPE.BALLOON_CHASE)
		{
			_balloonMinigameProgressMonitor = (MinigameObjective_CatchMe)MinigameController._pInstance._pMinigame._objectives[0];
			UpdateBalloonProgress();
		}
		if (_specialSpaceBar != null)
		{
			_specialSpaceBar.gameObject.SetActive(!GlobalDefines._isMobile);
		}
		if (_usingWheel)
		{
			_steeringWheel.gameObject.SetActive(true);
			_brakePedal.gameObject.SetActive(true);
			_leftButton.gameObject.SetActive(false);
			_rightButton.gameObject.SetActive(false);
			MinigameController._pInstance._pCamera.rotationDamping = 0.04f;
		}
		else
		{
			_steeringWheel.gameObject.SetActive(false);
			_brakePedal.gameObject.SetActive(false);
			_leftButton.gameObject.SetActive(true);
			_rightButton.gameObject.SetActive(true);
			MinigameController._pInstance._pCamera.rotationDamping = 0.2f;
		}
		_steeringWheelPixelPosition.x = (float)Screen.width * 0.8f;
		_steeringWheelPixelPosition.y = 40f;
	}

	public void OnHiddenArea()
	{
		if (secretAreaParticles != null)
		{
			secretAreaParticles.Play();
		}
	}

	protected override void OnExitScreen()
	{
		base.OnExitScreen();
		UnregisterPlayerVehicleStuckCallbacks();
		if (_coroutine_WaitToShowTutorial != null)
		{
			StopCoroutine(_coroutine_WaitToShowTutorial);
		}
	}

	protected override void OnScreenExitComplete()
	{
		base.OnScreenExitComplete();
		ResetOnScreenControls();
	}

	private void UpdateBurstFX()
	{
		if (_isPromptingSpecial)
		{
			specialButton.transform.localScale = Vector3.one * (1f + 0.3f * Mathf.Abs(Mathf.Sin((RealTime.time - _specialButtonBurstTime) * (float)Math.PI * 3f)));
		}
		if (_isSpecialButtonBurst)
		{
			float num = RealTime.time - _specialButtonBurstTime;
			float num2 = RealTime.time - (_specialButtonBurstTime + 0.25f);
			if (num < 0f)
			{
				specialButtonBurst1.enabled = false;
			}
			else if (num < 0.35f)
			{
				specialButtonBurst1.enabled = true;
				float num3 = Easing.Ease(Easing.EaseType.EaseInCircle, num, 0.35f, 0f, 1f);
				specialButtonBurst1.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 3f, num3);
				specialButtonBurst1.alpha = 1f - num3;
			}
			else
			{
				specialButtonBurst1.enabled = false;
			}
			if (num2 < 0f)
			{
				specialButtonBurst2.enabled = false;
			}
			else if (num2 < 0.35f)
			{
				specialButtonBurst2.enabled = true;
				float num4 = Easing.Ease(Easing.EaseType.EaseInCircle, num2, 0.35f, 0f, 1f);
				specialButtonBurst2.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 3f, num4);
				specialButtonBurst2.alpha = 1f - num4;
			}
			else
			{
				specialButtonBurst2.enabled = false;
				_isSpecialButtonBurst = false;
			}
		}
		if (_isSpecialIconBurst)
		{
			float num5 = RealTime.time - _specialIconBurstTime;
			float num6 = RealTime.time - (_specialIconBurstTime + 0.15f);
			float num7 = 0.4f;
			if (num5 < 0f)
			{
				specialIconBurst1.enabled = false;
			}
			else if (num5 < num7)
			{
				specialIconBurst1.enabled = true;
				float num8 = Easing.Ease(Easing.EaseType.EaseInCircle, num5, num7, 0f, 1f);
				specialIconBurst1.transform.localScale = Vector3.Lerp(Vector3.one * 4f, Vector3.one, num8);
				specialIconBurst1.alpha = num8;
			}
			else
			{
				specialIconBurst1.enabled = false;
			}
			if (num6 < 0f)
			{
				specialIconBurst2.enabled = false;
			}
			else if (num6 < num7)
			{
				specialIconBurst2.enabled = true;
				float num9 = Easing.Ease(Easing.EaseType.EaseInCircle, num6, num7, 0f, 1f);
				specialIconBurst2.transform.localScale = Vector3.Lerp(Vector3.one * 4f, Vector3.one, num9);
				specialIconBurst2.alpha = num9 * 0.5f;
			}
			else
			{
				specialIconBurst2.enabled = false;
				_isSpecialIconBurst = false;
			}
		}
	}

	protected override void Update()
	{
		base.Update();
		UpdateBurstFX();
		UpdateHUD();
		if (_usingWheel)
		{
			UpdateSteeringValue();
		}
		if (_isGameOver)
		{
			UpdateGameOver();
		}
	}

	private void ResetOnScreenControls()
	{
		_isControlDown_Left = false;
		_isControlDown_Right = false;
		CrossPlatformInputManager.SetAxisZero("Horizontal");
		CrossPlatformInputManager.SetAxisZero("Vertical");
		CrossPlatformInputManager.SetButtonUp("Special");
	}

	private void RefreshMinigameBespokeElements()
	{
		int num = 0;
		int num2 = minigameBespokeElems.Length;
		for (int i = 0; i < num2; i++)
		{
			MinigameBespokeElements minigameBespokeElements = minigameBespokeElems[i];
			int num3 = minigameBespokeElements.elements.Length;
			for (int j = 0; j < num3; j++)
			{
				minigameBespokeElements.elements[j].enabled = false;
			}
			if (minigameBespokeElements.minigameType == MinigameManager._pInstance._pCurrentMinigameType)
			{
				num = i;
				_displayAsPercentageProgress = minigameBespokeElements.displayAsPercentageProgress;
			}
		}
		MinigameBespokeElements minigameBespokeElements2 = minigameBespokeElems[num];
		int num4 = minigameBespokeElements2.elements.Length;
		for (int k = 0; k < num4; k++)
		{
			minigameBespokeElements2.elements[k].enabled = true;
		}
	}

	public void ResetHUDValues()
	{
		clockTimer.SetClockDecimal(0f);
		timerFX.SetTime(0f);
		timeValue.text = "0:00";
		studValue.text = "0";
		objectiveProgressBar.ResetProgressBar();
		specialButtonBacking.color = specialButtonBackingColorNormal;
		specialButtonRing.color = specialButtonRingColorNormal;
		specialIconSprite.color = specialButtonIconColorNormal;
		specialButtonRing.fillAmount = 1f;
		specialButtonBurst1.enabled = false;
		specialButtonBurst2.enabled = false;
		specialIconBurst1.enabled = false;
		specialIconBurst2.enabled = false;
		resultAlphaBG.gameObject.SetActive(false);
		resultAlphaBG.alpha = 0f;
		progressLabel.text = string.Empty;
	}

	public void ShowBonusTime(Vector3 worldPos, float time)
	{
		_bonusTime.ShowBonusTime(worldPos, time);
	}

	private void UpdateHUD()
	{
		if (_isTweeningRewardTick && !_doneTweenRewardTick)
		{
			float num = Time.unscaledTime - _rewardTickTweenStartTime;
			float num2 = 0.5f;
			if (num < num2)
			{
				float num3 = Easing.Ease(Easing.EaseType.EaseInCircle, num, num2, 0f, 1f);
				_tickSprite.alpha = num3;
				_tickSprite.transform.localScale = Vector3.Lerp(new Vector3(4f, 4f, 4f), Vector3.one, num3);
			}
			else
			{
				_tickSprite.transform.localScale = Vector3.one;
				_tickSprite.alpha = 1f;
				_barSprite.spriteName = "progressFillGreen";
				_doneTweenRewardTick = true;
			}
		}
		if (MinigameController._pInstance._pStage == MinigameController.EStage.Waiting)
		{
			clockTimer.SetClockDecimal(0f);
			timerFX.SetTime(0f);
			timeValue.text = "0:00";
			return;
		}
		if (_doShowTimer)
		{
			timerFX.SetTime(MinigameController._pInstance._pTimeRemaining);
			clockTimer.SetClockDecimal(1f - MinigameController._pInstance._pNormTimeRemaining);
			timeValue.text = TimeManager.FormatTime(Mathf.CeilToInt(MinigameController._pInstance._pTimeRemaining), false, true, false);
		}
		if ((bool)VehicleController_Player._pInstance._pVehicle._specialAbility)
		{
			specialCharge = VehicleController_Player._pInstance._pVehicle._specialAbility._pNormCharge;
			bool flag = !VehicleController_Player._pInstance._pVehicle._specialAbility._pIsInUse && specialCharge < 1f && specialCharge > 0f;
			if (specialButtonRing.fillAmount < 1f && specialCharge >= 1f)
			{
				_isSpecialIconBurst = true;
				_specialIconBurstTime = RealTime.time;
			}
			specialButtonBacking.color = ((!flag) ? specialButtonBackingColorNormal : specialButtonBackingColorRecharging);
			specialButtonRing.color = ((!flag) ? specialButtonRingColorNormal : specialButtonRingColorRecharging);
			specialIconSprite.color = ((!flag) ? specialButtonIconColorNormal : specialButtonIconColorRecharging);
			specialButtonRing.fillAmount = specialCharge;
		}
		if ((bool)_balloonMinigameProgressMonitor)
		{
			UpdateBalloonProgress();
		}
		if (_isSteering)
		{
			_steeringWheel.transform.rotation = Quaternion.Euler(0f, 0f, _normalisedSteeringValue * -30f);
		}
		else
		{
			_steeringWheel.transform.eulerAngles = new Vector3(0f, 0f, Mathf.LerpAngle(_steeringWheel.transform.eulerAngles.z, 0f, Time.deltaTime * 10f));
		}
	}

	private void UpdateBalloonProgress()
	{
		_progressIconPlayer.transform.localPosition = new Vector3(_barAnchorRight.transform.localPosition.x * _balloonMinigameProgressMonitor._pProgressPlayer, _progressIconPlayer.transform.position.y, _progressIconPlayer.transform.position.z);
		_progressIconBalloon.transform.localPosition = new Vector3(_barAnchorRight.transform.localPosition.x * _balloonMinigameProgressMonitor._pProgressThis, _progressIconBalloon.transform.position.y, _progressIconBalloon.transform.position.z);
	}

	private void UpdateProgressLabel()
	{
		progressLabel.text = ((!_displayAsPercentageProgress) ? MinigameController._pInstance._pMinigame._pNumObjectivesCompleted.ToString() : (Mathf.RoundToInt(MinigameController._pInstance._pMinigame._pNormProgress * 100f) + "%"));
	}

	private void UpdateGameOver()
	{
		_gameOverTimer += Time.unscaledDeltaTime;
		if (_gameOverTimer <= 1f)
		{
			Time.timeScale = Mathf.Max(1f - _gameOverTimer, 0f);
		}
		if (_loseSequenceState == EGameOverSequenceState.SlowingDown)
		{
			if (_gameOverTimer >= 0.5f)
			{
				_loseSequenceState = EGameOverSequenceState.TweeningPanel;
				resultLabel.text = LocalisationFacade.Instance.GetString("Results.OutOfTime");
				resultLabel.color = Color.red;
			}
		}
		else if (_loseSequenceState == EGameOverSequenceState.TweeningPanel)
		{
			UpdateResultsTransitionTween();
			if (_gameOverTimer >= 2f)
			{
				_loseSequenceState = EGameOverSequenceState.Finished;
				OnResults();
			}
		}
		if (_winSequenceState == EGameOverSequenceState.SlowingDown)
		{
			if (_gameOverTimer >= 0.5f)
			{
				_winSequenceState = EGameOverSequenceState.TweeningPanel;
				resultLabel.text = LocalisationFacade.Instance.GetString("Results.MissionComplete");
				resultLabel.color = Color.cyan;
				if ((bool)_winParticlesCopy)
				{
					_winParticlesCopy.gameObject.SetActive(true);
				}
			}
		}
		else
		{
			if (_winSequenceState != EGameOverSequenceState.TweeningPanel)
			{
				return;
			}
			_winParticlesCopy.Simulate(Time.unscaledDeltaTime, true, false);
			UpdateResultsTransitionTween();
			if (_gameOverTimer >= 2f)
			{
				_winSequenceState = EGameOverSequenceState.Finished;
				if ((bool)_winParticlesCopy)
				{
					UnityEngine.Object.Destroy(_winParticlesCopy.gameObject);
				}
				OnResults();
			}
		}
	}

	public void ShowTutorial(bool evenIfAlreadySeen = false)
	{
		bool flag = false;
		Debug.Log("Tutorial Flow LOC: " + MinigameManager._pInstance._pTutorialFlowLocation + " seen? " + flag);
		if (flag && !evenIfAlreadySeen)
		{
			Debug.Log("Cancel 1");
		}
		else if (_coroutine_WaitToShowTutorial != null)
		{
			Debug.Log("Cancel 2");
		}
		else
		{
			_coroutine_WaitToShowTutorial = _pInstance.StartCoroutine(_ShowTutorial());
		}
	}

	private IEnumerator _ShowTutorial()
	{
		while (Facades<ScreenFacade>.Instance._pIsAnyScreenTweening || base._pCurrentTweenType != ScreenTweenType.Idle)
		{
			yield return false;
		}
		_coroutine_WaitToShowTutorial = null;
		MinigameManager._pInstance._pSeenTutorial = true;
		Debug.Log("Tutorial Flow LOC: " + MinigameManager._pInstance._pTutorialFlowLocation + " CUR: " + Facades<FlowFacade>.Instance.CurrentLocation);
		Navigate(MinigameManager._pInstance._pTutorialFlowLocation);
	}

	private void UpdateStudsDisplay()
	{
		studValue.text = MinigameController._pInstance._pStudsCollected.ToString();
		studValueTween.ResetToBeginning();
		studValueTween.PlayForward();
	}

	private void UpdateSteeringValue()
	{
		if (_usingLateral)
		{
			float num = (float)Screen.width * 0.7f;
			float num2 = (float)Screen.width * 0.95f;
			if (Input.touchCount != 0 && Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(0).position.x > num && Input.GetTouch(0).position.x < num2)
			{
				_isSteering = true;
				_initialSteeringValue = (Input.GetTouch(0).position.x - (float)Screen.width * 0.825f) / ((float)Screen.width * 0.125f);
			}
			if (Input.touchCount != 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				_isSteering = false;
			}
			if (Input.touchCount != 0)
			{
				float num3 = (Input.GetTouch(0).position.x - (float)Screen.width * 0.825f) / ((float)Screen.width * 0.125f);
				float value = ((!(_initialSteeringValue < 0f)) ? (num3 - _initialSteeringValue) : (num3 - _initialSteeringValue));
				_normalisedSteeringValue = Mathf.Clamp(value, -1f, 1f);
			}
		}
		else
		{
			if (Input.touchCount != 0 && Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(0).position.y > 20f)
			{
				_isSteering = true;
				Vector2 vector = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				vector -= _steeringWheelPixelPosition;
				if (vector.magnitude < 300f)
				{
					float value2 = Vector2.Dot(Vector2.right, vector.normalized);
					_initialSteeringValue = Mathf.Clamp(value2, -1f, 1f);
				}
			}
			if (Input.touchCount != 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				_isSteering = false;
			}
			if (Input.touchCount != 0 && Input.GetTouch(0).position.y > 20f)
			{
				Vector2 vector2 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				vector2 -= _steeringWheelPixelPosition;
				if (vector2.magnitude < 300f)
				{
					float num4 = Vector2.Dot(Vector2.right, vector2.normalized);
					_normalisedSteeringValue = Mathf.Clamp(num4 - _initialSteeringValue, -1f, 1f);
				}
			}
		}
		if (!_isSteering)
		{
			_normalisedSteeringValue = 0f;
		}
		VehicleController_Player._pInstance.SetSteerValue(_normalisedSteeringValue);
	}

	private void AddDebugMenu()
	{
		AmuzoDebugMenu amuzoDebugMenu = new AmuzoDebugMenu("CONTROL SCHEME");
		amuzoDebugMenu.AddButton(new AmuzoDebugMenuButton("BUTTONS", delegate
		{
			ToggleControlScheme(0);
		}));
		amuzoDebugMenu.AddButton(new AmuzoDebugMenuButton("WHEEL LATERAL", delegate
		{
			ToggleControlScheme(1);
		}));
		amuzoDebugMenu.AddButton(new AmuzoDebugMenuButton("WHEEL CIRCULAR", delegate
		{
			ToggleControlScheme(2);
		}));
		AmuzoDebugMenuManager.RegisterRootDebugMenu(amuzoDebugMenu);
	}

	private void ToggleControlScheme(int type)
	{
		switch (type)
		{
		case 0:
			_steeringWheel.gameObject.SetActive(false);
			_brakePedal.gameObject.SetActive(false);
			_leftButton.gameObject.SetActive(true);
			_rightButton.gameObject.SetActive(true);
			MinigameController._pInstance._pCamera.rotationDamping = 0.2f;
			_usingWheel = false;
			break;
		case 1:
			_steeringWheel.gameObject.SetActive(true);
			_brakePedal.gameObject.SetActive(true);
			_leftButton.gameObject.SetActive(false);
			_rightButton.gameObject.SetActive(false);
			MinigameController._pInstance._pCamera.rotationDamping = 0.04f;
			_usingWheel = true;
			_usingLateral = true;
			break;
		case 2:
			_steeringWheel.gameObject.SetActive(true);
			_brakePedal.gameObject.SetActive(true);
			_leftButton.gameObject.SetActive(false);
			_rightButton.gameObject.SetActive(false);
			MinigameController._pInstance._pCamera.rotationDamping = 0.04f;
			_usingWheel = true;
			_usingLateral = false;
			break;
		default:
			_steeringWheel.gameObject.SetActive(false);
			_brakePedal.gameObject.SetActive(false);
			_leftButton.gameObject.SetActive(true);
			_rightButton.gameObject.SetActive(true);
			MinigameController._pInstance._pCamera.rotationDamping = 0.2f;
			break;
		}
	}

	private void OnResults()
	{
		resultLabel.gameObject.SetActive(false);
		ScreenRoot._pInstance.EnableMultiTouch(false);
		Navigate("MinigameResults");
	}

	public void OnPause()
	{
		if (!_isGameOver)
		{
			ScreenRoot._pInstance.EnableMultiTouch(false);
			SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
			Navigate("Pause");
		}
	}

	public void OnLeftPressed()
	{
		_isControlDown_Left = true;
		if (_isControlDown_Right)
		{
			CrossPlatformInputManager.SetAxisZero("Horizontal");
			CrossPlatformInputManager.SetAxisNegative("Vertical");
		}
		else
		{
			CrossPlatformInputManager.SetAxisNegative("Horizontal");
		}
	}

	public void OnLeftReleased()
	{
		_isControlDown_Left = false;
		if (_isControlDown_Right)
		{
			CrossPlatformInputManager.SetAxisPositive("Horizontal");
			CrossPlatformInputManager.SetAxisZero("Vertical");
		}
		else
		{
			CrossPlatformInputManager.SetAxisZero("Horizontal");
		}
	}

	public void OnRightPressed()
	{
		_isControlDown_Right = true;
		if (_isControlDown_Left)
		{
			CrossPlatformInputManager.SetAxisZero("Horizontal");
			CrossPlatformInputManager.SetAxisNegative("Vertical");
		}
		else
		{
			CrossPlatformInputManager.SetAxisPositive("Horizontal");
		}
	}

	public void OnRightReleased()
	{
		_isControlDown_Right = false;
		if (_isControlDown_Left)
		{
			CrossPlatformInputManager.SetAxisNegative("Horizontal");
			CrossPlatformInputManager.SetAxisZero("Vertical");
		}
		else
		{
			CrossPlatformInputManager.SetAxisZero("Horizontal");
		}
	}

	public void OnSpecialPressed()
	{
		if (!(VehicleController_Player._pInstance._pVehicle._specialAbility == null))
		{
			SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
			if (!VehicleController_Player._pInstance._pIsSpecialDown && VehicleController_Player._pInstance._pVehicle._specialAbility._pNormCharge >= 1f)
			{
				VehicleController_Player._pInstance.SetSpecialPressed(true);
				_specialButtonBurstTime = RealTime.time;
				_isSpecialButtonBurst = true;
			}
			else
			{
				VehicleController_Player._pInstance.SetSpecialPressed(false);
			}
		}
	}

	public void OnSpecialReleased()
	{
	}

	public void OnBrakePressed()
	{
		CrossPlatformInputManager.SetAxisNegative("Vertical");
	}

	public void OnBrakeReleased()
	{
		CrossPlatformInputManager.SetAxisZero("Vertical");
	}

	public void RequestMessageSystemRegister()
	{
		messagingSystem.Register();
	}

	private void OnMinigameProgress()
	{
		objectiveProgressBar.SetProgressBarDecimal(MinigameController._pInstance._pMinigame._pNormProgress);
		bool flag = false;
		if (objectiveProgressBar._pTargetDecimal >= 1f && !_isTweeningRewardTick && !_doneTweenRewardTick && !flag)
		{
			_isTweeningRewardTick = true;
			_rewardTickTweenStartTime = Time.unscaledTime;
		}
	}

	private void OnMinigameEnded(bool completed)
	{
		_isGameOver = true;
		if (completed)
		{
			_winSequenceState = EGameOverSequenceState.SlowingDown;
			_winParticlesCopy = UnityEngine.Object.Instantiate(winParticlesBase);
		}
		else
		{
			_loseSequenceState = EGameOverSequenceState.SlowingDown;
		}
		SoundFacade._pInstance.SetSFXChannelMute("Game", true);
		SoundFacade._pInstance.FadeMusicOut(0.5f);
		SoundFacade._pInstance.PlayOneShotSFX((!completed) ? "GUILose" : "GUIWin", 0f);
	}

	private void OnStudAwarded(int num)
	{
		UpdateStudsDisplay();
	}

	private void OnStudReduced(int num)
	{
		UpdateStudsDisplay();
	}

	private void OnTimeAdded(float time, Vector3 effectPos)
	{
		SoundFacade._pInstance.PlayOneShotSFX("TimeBonus", 0f);
		ShowBonusTime(effectPos, time);
	}

	private void OnCollateralDestroyed(Collateral collateral)
	{
	}

	private void OnPromptSpecialChanged(bool doPrompt)
	{
		_isPromptingSpecial = doPrompt;
		if (doPrompt)
		{
			_specialButtonBurstTime = RealTime.time;
			_isSpecialButtonBurst = true;
		}
		else
		{
			specialButton.transform.localScale = Vector3.one;
		}
	}

	private void OnPrimaryObjectiveChanged(MinigameObjective objective)
	{
		if ((bool)_currObjective)
		{
			_currObjective._onProgress -= OnPrimaryObjectiveProgress;
			_currObjective._onComplete -= OnPrimaryObjectiveComplete;
		}
		_currObjective = objective;
		if ((bool)objective)
		{
			objective._onProgress += OnPrimaryObjectiveProgress;
			objective._onComplete += OnPrimaryObjectiveComplete;
		}
	}

	private void OnPrimaryObjectiveProgress(MinigameObjective objective, float normProgress)
	{
	}

	private void OnPrimaryObjectiveComplete(MinigameObjective objective)
	{
	}

	private void UpdateResultsTransitionTween()
	{
		float time = Mathf.Clamp(_gameOverTimer - 1f, 0f, 0.8f);
		float t = Easing.Ease(Easing.EaseType.EaseOutBounce, time, 0.8f, 0f, 1f);
		resultLabel.transform.position = Vector3.Lerp(_resultLabelStartPosition.position, _resultLabelEndPosition.position, t);
	}

	private void OnPlayerVehicleStuck()
	{
		Debug.Log("[HUD] Player vehicle STUCK!!!");
		if (!ScreenTutorialVehicleReverse._pHasShown)
		{
			Navigate("MinigameTutorialVehicleReverse");
		}
	}

	private void OnPlayerVehicleUnstuck()
	{
		Debug.Log("[HUD] Player vehicle UNSTUCK!!!");
	}

	private void RegisterPlayerVehicleStuckCallbacks()
	{
		if (!_hasRegisterPlayerVehicleStuckCallbacks && VehicleController_Player._pInstance != null && VehicleController_Player._pInstance._pVehicle != null)
		{
			VehicleController_Player._pInstance._pVehicle._onStuck += OnPlayerVehicleStuck;
			VehicleController_Player._pInstance._pVehicle._onUnstuck += OnPlayerVehicleUnstuck;
			_hasRegisterPlayerVehicleStuckCallbacks = true;
		}
	}

	private void UnregisterPlayerVehicleStuckCallbacks()
	{
		if (_hasRegisterPlayerVehicleStuckCallbacks && VehicleController_Player._pInstance != null && VehicleController_Player._pInstance._pVehicle != null)
		{
			VehicleController_Player._pInstance._pVehicle._onStuck -= OnPlayerVehicleStuck;
			VehicleController_Player._pInstance._pVehicle._onUnstuck -= OnPlayerVehicleUnstuck;
			_hasRegisterPlayerVehicleStuckCallbacks = false;
		}
	}
}
