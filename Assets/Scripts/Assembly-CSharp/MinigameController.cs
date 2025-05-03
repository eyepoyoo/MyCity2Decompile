using System;
using UnityEngine;

[RequireComponent(typeof(Minigame))]
public class MinigameController : MonoBehaviour
{
	public enum EStage
	{
		Waiting = 0,
		InProgress = 1,
		Complete = 2,
		Failed = 3
	}

	public const int STUD_AWARD_HIDDEN_AREA = 50;

	public MinigameManager.EMINIGAME_TYPE _type;

	public float _timeLimit = float.PositiveInfinity;

	public float _timeBonusReduction;

	public float _minTimeBonus;

	public bool _renderArrowOnTop = true;

	public int _missionCompleteBonus;

	public PlayerSpawnPoint _playerSpawnPoint;

	public PlayerSpawnPoint _playerSpawnPointTest;

	public NpcController _npcController;

	public FastPoolManager _pools;

	public SpecialAbility _defaultSpecial;

	public GameObject[] _objectsToEnable;

	public bool _distanceCullVehicles = true;

	private DirectorArrow _directorArrow;

	private float _timeRemaining;

	private float _timeBonusAdjustment;

	private float _additionalTimeBonus;

	private bool _wasOnZeroTime;

	private readonly object _requestKey_makePlayerCoast = new object();

	private bool _hasSlowedTimeForSpecialPrompt;

	private MinigameMetrics _metrics;

	public static MinigameController _pInstance { get; private set; }

	public SmoothFollow _pCamera { get; private set; }

	public Minigame _pMinigame { get; private set; }

	public int _pStudsCollected { get; private set; }

	public EStage _pStage { get; private set; }

	public FastPool _pPool_FloatingStuds { get; private set; }

	public FastPool _pPool_DynamicStuds { get; private set; }

	public BooleanStateRequestsSimple _pDoPromptSpecial { get; private set; }

	public float _pTimeRemaining
	{
		get
		{
			return _timeRemaining;
		}
	}

	public float _pNormTimeRemaining
	{
		get
		{
			return (_timeLimit != float.PositiveInfinity) ? (_timeRemaining / _timeLimit) : float.PositiveInfinity;
		}
	}

	public int _pMissionCompleteBonus
	{
		get
		{
			return _pMinigame._pHasBeenCompleted ? _missionCompleteBonus : 0;
		}
	}

	public int _pTimeBonus
	{
		get
		{
			return (_timeLimit != float.PositiveInfinity) ? ((int)_pTimeRemaining + (int)_additionalTimeBonus) : 0;
		}
	}

	public int _pDestructionBonus
	{
		get
		{
			return 0;
		}
	}

	public int _pTotalScore
	{
		get
		{
			return _pMissionCompleteBonus + _pTimeBonus + _pDestructionBonus + _pStudsCollected;
		}
	}

	public event Action _onProgress;

	public event Action<bool> _onEnded;

	public event Action _onFailed;

	public event Action<int> _onStudsAwarded;

	public event Action<int> _onStudsReduced;

	public event Action<float, Vector3> _onTimeAdded;

	public event Action<Collateral> _onPlayerDestroyedCollateral;

	public event Action<MinigameObjective> _onPrimaryObjectiveChanged;

	public event Action<bool> _onDoPromptSpecialChanged;

	public void Init(bool wasRunFromMinigameScene)
	{
		base.gameObject.SetActive(true);
		_pInstance = this;
		_pStage = EStage.Waiting;
		_timeRemaining = ((_timeLimit != 0f) ? _timeLimit : float.PositiveInfinity);
		for (int num = _objectsToEnable.Length - 1; num >= 0; num--)
		{
			_objectsToEnable[num].SetActive(true);
		}
		_pCamera = UnityEngine.Object.Instantiate(MinigamesController._pInstance._cameraPrefab);
		_pCamera.transform.parent = base.transform;
		_pCamera.gameObject.AddComponent<MakeAudioListenerFollowThisOnAwake>();
		_pCamera.GetComponent<LayerBasedCuller>()._cullVehicles = _distanceCullVehicles;
		_pMinigame = GetComponent<Minigame>();
		AddMinigameListeners();
		Collateral._onCollateralDestroyed += OnCollateralDestroyed;
		((!Application.isEditor || !wasRunFromMinigameScene) ? _playerSpawnPoint : (_playerSpawnPointTest ?? _playerSpawnPoint)).SpawnPlayer();
		VehicleController_Player._pInstance._enabled = false;
		base.gameObject.SendMessage("OnPlayerSpawned", SendMessageOptions.DontRequireReceiver);
		VehicleController_Player._pInstance._pVehicle._onHitVehicle += EmoticonSystem.OnPlayerHitVehicle;
		if ((bool)_defaultSpecial && !VehicleController_Player._pInstance._pVehicle.GetComponentInChildren(_defaultSpecial.GetType()))
		{
			SpecialAbility specialAbility = UnityEngine.Object.Instantiate(_defaultSpecial);
			specialAbility._isAutomated = true;
			specialAbility.GetComponent<VehiclePart>().AttachToVehicle(VehicleController_Player._pInstance._pVehicle);
			SpecialAbility specialAbility2 = VehicleController_Player._pInstance._pVehicle._specialAbility;
			specialAbility.AssignToVehicle(VehicleController_Player._pInstance._pVehicle);
			VehicleController_Player._pInstance._pVehicle._specialAbility = specialAbility2;
		}
		GameObject gameObject = new GameObject("Messaging Controller");
		MinigameMessagingController minigameMessagingController = gameObject.AddComponent<MinigameMessagingController>();
		minigameMessagingController.transform.parent = base.transform;
		minigameMessagingController._onStudComboEnded += OnStudComboEnded;
		minigameMessagingController._onAirBonus += OnAirBonus;
		minigameMessagingController._onHiddenAreaDiscovered += OnHiddenAreaDiscovered;
		_pPool_FloatingStuds = _pools.Pools[0];
		_pPool_DynamicStuds = _pools.Pools[1];
		_directorArrow = UnityEngine.Object.Instantiate(MinigamesController._pInstance._directorArrowPrefab);
		_directorArrow.transform.parent = base.transform;
		_directorArrow.transform.position = Vector3.down * 100f;
		_directorArrow._pDoRenderOnTop = _renderArrowOnTop;
		Pickup.SetUpGrid();
		if ((bool)FidelityFacade.Instance && FidelityFacade.Instance.fidelity == Fidelity.Low)
		{
			RenderSettings.fog = false;
		}
		_pDoPromptSpecial = new BooleanStateRequestsSimple(false, OnPromptSpecialChanged);
		_metrics = new MinigameMetrics();
	}

	private void Start()
	{
		if (!ScreenMinigameHUD._pInstance)
		{
			PlayIntroAndStartMinigames();
		}
	}

	public void PlayIntroAndStartMinigames()
	{
		IntroAnim component = GetComponent<IntroAnim>();
		if ((bool)component && !Application.isEditor)
		{
			component.enabled = true;
		}
		if ((bool)component && component.enabled)
		{
			component.StartIntro(delegate
			{
				StartMinigames(true);
			});
		}
		else
		{
			StartMinigames(false);
		}
		if ((bool)_npcController)
		{
			_npcController.Initialise();
		}
	}

	public void StartMinigames(bool doEaseCam)
	{
		if ((bool)ScreenMinigameHUD._pInstance)
		{
			ScreenMinigameHUD._pInstance.ShowTutorial();
		}
		VehicleController_Player._pInstance._enabled = true;
		_pCamera.target = VehicleController_Player._pInstance.transform;
		if (doEaseCam)
		{
			_pCamera.EaseToDefault();
		}
		else
		{
			_pCamera.Snap();
		}
		_pStage = EStage.InProgress;
		_pMinigame.StartMinigame();
	}

	private void Update()
	{
		switch (_pStage)
		{
		case EStage.Waiting:
			break;
		case EStage.InProgress:
			Update_InProgress();
			break;
		case EStage.Failed:
			break;
		case EStage.Complete:
			break;
		}
	}

	private void Update_InProgress()
	{
		_timeRemaining -= Time.deltaTime;
		if (_timeRemaining <= 0f)
		{
			_timeRemaining = 0f;
			if (!_wasOnZeroTime)
			{
				_wasOnZeroTime = true;
				OnTimeUp();
			}
			if (!_pMinigame._pPrimaryObjective._pIsLockingGame && VehicleController_Player._pInstance._pVehicle._pRigidbody.velocity.SqrMagnitudeXZ() < 1f)
			{
				VehicleController_Player._pInstance._pVehicle.Brake(1f);
				End(_pMinigame._pHasBeenCompleted);
			}
		}
		_pMinigame.UpdateMinigame();
		_metrics.Update(Time.deltaTime);
	}

	private void AwardTime(float time, Vector3 effectPos)
	{
		_timeRemaining += time;
		VehicleController_Player._pInstance._pVehicle._pShouldCoast.RemoveContraryStateRequest(_requestKey_makePlayerCoast);
		if (this._onTimeAdded != null)
		{
			this._onTimeAdded(time, effectPos);
		}
		_wasOnZeroTime = false;
	}

	private void End(bool completed)
	{
		if (_pStage != EStage.InProgress)
		{
			return;
		}
		_pStage = ((!completed) ? EStage.Failed : EStage.Complete);
		VehicleController_Player._pInstance._enabled = false;
		StopMinigame();
		_metrics.LogAnalytics();
		if (completed && (bool)MinigameManager._pInstance)
		{
			MinigameManager.MinigameData currentMinigameData = MinigameManager._pInstance.GetCurrentMinigameData();
			if (currentMinigameData != null)
			{
				currentMinigameData._pPersonalHighScore = _pTotalScore;
			}
		}
		if (this._onEnded != null)
		{
			this._onEnded(completed);
		}
	}

	private void OnTimeUp()
	{
		VehicleController_Player._pInstance._pVehicle._pShouldCoast.AddContraryStateRequest(_requestKey_makePlayerCoast);
	}

	private void OnDestroy()
	{
		Collateral._onCollateralDestroyed -= OnCollateralDestroyed;
		Pickup.ClearGrid();
		_metrics.Destroy();
		_pInstance = null;
	}

	private void AwardStuds(int num)
	{
		_pStudsCollected += num;
		if (this._onStudsAwarded != null)
		{
			this._onStudsAwarded(num);
		}
	}

	public void DropStuds(int num = 5, float radius = 5f)
	{
		int num2 = Mathf.Min(num, _pStudsCollected);
		if (num2 != 0)
		{
			SpawnStudsRing(VehicleController_Player._pInstance.transform.position, num2, radius);
			_pStudsCollected -= num2;
			if (this._onStudsReduced != null)
			{
				this._onStudsReduced(num2);
			}
		}
	}

	public void AwardFloatingStud(Vector3 fromPos, Quaternion fromRot, Action onReachedHud = null)
	{
		Stud_FloatToHud floater = _pPool_FloatingStuds.FastInstantiate<Stud_FloatToHud>();
		Action<Stud_FloatToHud> onComplete = delegate
		{
			if (onReachedHud != null)
			{
				onReachedHud();
			}
			_pPool_FloatingStuds.FastDestroy(floater);
			AwardStuds(1);
		};
		floater.FloatToHud(fromPos, fromRot, onComplete);
	}

	public void SpawnStudsRing(Vector3 pos, int num, float radius, Transform parent = null)
	{
		for (int i = 0; i < num; i++)
		{
			float f = (float)Math.PI * 2f * ((float)i / (float)num);
			Pickup_Stud stud = _pPool_DynamicStuds.FastInstantiate<Pickup_Stud>();
			stud.enabled = false;
			stud._pool = _pPool_DynamicStuds;
			stud.transform.parent = base.transform;
			stud.transform.position = pos;
			Pickup.RefreshInGrid(stud);
			stud.transform.TweenToPos(pos + new Vector3(Mathf.Sin(f) * radius, 1f, Mathf.Cos(f) * radius), 0.1f, delegate
			{
				stud.enabled = true;
			}, Easing.EaseType.EaseOut, true, false, 0f);
		}
	}

	public void AddHideDirectorArrowRequest(object target)
	{
		_directorArrow._pHideRequests.AddContraryStateRequest(target);
	}

	public void RemoveHideDirectorArrowRequest(object target)
	{
		_directorArrow._pHideRequests.RemoveContraryStateRequest(target);
	}

	private void AddMinigameListeners()
	{
		_pMinigame._onProgress += OnMinigameProgress;
		_pMinigame._onCompleted += OnMinigameCompleted;
		_pMinigame._onFailed += OnMinigameFailed;
		_pMinigame._onPrimaryObjectiveChanged += OnPrimaryObjectiveChanged;
		_pMinigame._onObjectiveComplete += OnObjectiveComplete;
		_pMinigame._onShouldShowDirectorArrowChanged += OnShouldShowDirectorArrowChanged;
	}

	private void StopMinigame()
	{
		_pMinigame.StopMinigame();
		_pMinigame._onProgress -= OnMinigameProgress;
		_pMinigame._onCompleted -= OnMinigameCompleted;
		_pMinigame._onFailed -= OnMinigameFailed;
		_pMinigame._onPrimaryObjectiveChanged -= OnPrimaryObjectiveChanged;
		_pMinigame._onObjectiveComplete -= OnObjectiveComplete;
		_pMinigame._onShouldShowDirectorArrowChanged -= OnShouldShowDirectorArrowChanged;
	}

	private void OnMinigameProgress(Minigame minigame)
	{
		if (this._onProgress != null)
		{
			this._onProgress();
		}
	}

	private void OnMinigameCompleted(Minigame minigame)
	{
		End(true);
	}

	private void OnMinigameFailed(Minigame minigame)
	{
		End(false);
	}

	private void OnPrimaryObjectiveChanged(Minigame minigame)
	{
		_directorArrow._pTarget = ((!minigame._pPrimaryObjective) ? null : minigame._pPrimaryObjective.transform);
		if (this._onPrimaryObjectiveChanged != null)
		{
			this._onPrimaryObjectiveChanged(minigame._pPrimaryObjective);
		}
	}

	private void OnObjectiveComplete(MinigameObjective target)
	{
		if (SoundFacade._pInstance != null && target._playsSoundOnComplete)
		{
			SoundFacade._pInstance.PlayOneShotSFX("CollectObject", 0f);
		}
		if (target._studBonus != 0 && !target._awardStudsProgressively)
		{
			AwardStuds(target._studBonus);
		}
		if (target._timeBonus != 0f)
		{
			Vector3 position = target.transform.position;
			AwardTime(effectPos: (!(target._spawnStudsFrom != null)) ? target._pPositionOnEntry : target._spawnStudsFrom.position, time: Mathf.Max(_minTimeBonus, target._timeBonus + _timeBonusAdjustment));
			_timeBonusAdjustment -= _timeBonusReduction;
			_additionalTimeBonus += target._timeBonus;
		}
		if (this._onProgress != null)
		{
			this._onProgress();
		}
	}

	private void OnShouldShowDirectorArrowChanged(Minigame minigame, bool shouldShow)
	{
		if (shouldShow)
		{
			_directorArrow._pHideRequests.RemoveContraryStateRequest(this);
		}
		else
		{
			_directorArrow._pHideRequests.AddContraryStateRequest(this);
		}
	}

	private void OnCollateralDestroyed(Collateral collateral, Collider collider)
	{
		if (VehicleController_Player.IsPlayer(collider) && this._onPlayerDestroyedCollateral != null)
		{
			this._onPlayerDestroyedCollateral(collateral);
		}
	}

	private void OnStudComboEnded(int numStuds)
	{
		AwardStuds((numStuds - 1) * numStuds);
	}

	private void OnAirBonus(float distance)
	{
		AwardStuds((int)distance);
	}

	private void OnHiddenAreaDiscovered(PlayerTrigger area)
	{
		AwardStuds(50);
	}

	private void OnPromptSpecialChanged(bool doPrompt)
	{
		if (doPrompt)
		{
			if (!_hasSlowedTimeForSpecialPrompt)
			{
				Time.timeScale = 0.1f;
				_hasSlowedTimeForSpecialPrompt = true;
			}
		}
		else
		{
			Time.timeScale = 1f;
		}
		if (this._onDoPromptSpecialChanged != null)
		{
			this._onDoPromptSpecialChanged(doPrompt);
		}
	}
}
