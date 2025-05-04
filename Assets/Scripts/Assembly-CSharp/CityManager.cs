using System;
using System.Collections.Generic;
using UnityEngine;
using VacuumShaders.CurvedWorld;

public class CityManager : MonoBehaviour
{
	public enum ECITYBUILDINGS
	{
		INVALID = 0,
		FIRE_STATION = 1,
		LIGHTHOUSE = 2,
		FIRE_CONTAINERS = 3,
		POLICE_ISLAND_HQ = 4,
		POLICE_BOATS = 5,
		JETSKI_PATROL = 6,
		HELICOPTER_REQUESTED = 7,
		LAVA_LIFTERS_NEEDED = 8,
		LAVA_LAB_PERIL = 9,
		AIRPORT_1 = 10,
		AIRPORT_2 = 11,
		NUM_BUILDINGS = 12
	}

	public enum REGIONS
	{
		INVALID = 0,
		CITY = 1,
		VOLCANO = 2,
		AIRPORT = 3
	}

	[Serializable]
	public class CityElement
	{
		public REGIONS region;

		public ECITYBUILDINGS buildingID;

		public Transform cameraFocalPoint;

		public Transform cameraFocalPointManualClick;

		public BuildingAnimator buildingAnimator;

		public float zoomDistance = 44f;

		public GameObject progressFXRoot;

		public Transform floatyUIPositions;

		public BuildingAnimator buildSiteAnimator;

		public MeshRenderer buildSiteTile;

		public GameObject currentSelection;

		private Vector3 _progressFXRootOriginalPos;

		public Vector3 _pProgressFXRootOriginalPos
		{
			get
			{
				return _progressFXRootOriginalPos;
			}
			set
			{
				_progressFXRootOriginalPos = value;
			}
		}

		public void MoveProgressFXToCurvedWorldSpace()
		{
			progressFXRoot.transform.position = CurvedWorld_Controller.get.TransformPoint(_progressFXRootOriginalPos);
		}

		public void TriggerProgressFX()
		{
			ParticleSystem[] componentsInChildren = progressFXRoot.GetComponentsInChildren<ParticleSystem>();
			int num = componentsInChildren.Length;
			for (int i = 0; i < num; i++)
			{
				componentsInChildren[i].Play();
			}
		}
	}

	public static bool DID_COME_FROM_TITLE;

	public static bool DID_COME_FROM_GARAGE;

	public CityElement[] cityElements;

	public GameObject minigameMarkerRoot;

	private Action _onOtherCityLoadedAction;

	private Vector3 _lastKnownTouchPos;

	private bool _wantTouchUpCheck;

	private bool _isPressNotOverGUI;

	private static CityManager _instance;

	public static CityManager _pInstance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		_instance = this;
		for (int i = 0; i < cityElements.Length; i++)
		{
			cityElements[i]._pProgressFXRootOriginalPos = cityElements[i].progressFXRoot.transform.position;
		}
		_isPressNotOverGUI = false;
	}

	private void Start()
	{
		bool flag = RefreshLocalCity(true);
		SetCurrentSelection(ScenarioManager._pInstance._pCurrentScenario.building);
		if (DID_COME_FROM_TITLE && !flag)
		{
			PanCameraToBuilding(ECITYBUILDINGS.FIRE_STATION, CameraHUB.EFocusType.PAN_ONLY, true);
		}
		DID_COME_FROM_TITLE = false;
	}

	public Vector3 GetBuildingProgressPos(ECITYBUILDINGS buildingID)
	{
		int num = cityElements.Length;
		for (int i = 0; i < num; i++)
		{
			if (cityElements[i].buildingID == buildingID)
			{
				if (cityElements[i].floatyUIPositions != null)
				{
					return cityElements[i].floatyUIPositions.position;
				}
				return cityElements[i].cameraFocalPointManualClick.position + Vector3.up * 5f;
			}
		}
		return Vector3.zero;
	}

	public bool RefreshLocalCity(bool fromCityLoad = false)
	{
		bool result = false;
		float pProgress = ScenarioManager._pInstance._pCurrentScenario._pProgress;
		float pLastSeenProgression = ScenarioManager._pInstance._pCurrentScenario._pLastSeenProgression;
		ECITYBUILDINGS building = ScenarioManager._pInstance._pCurrentScenario.building;
		int num = ScenarioManager._pInstance.allScenarios.Length;
		for (int i = 0; i < num; i++)
		{
			SetupBuildingWithProgress(ScenarioManager._pInstance.allScenarios[i].building, ScenarioManager._pInstance.allScenarios[i]._pProgress);
		}
		if (pProgress < 1f)
		{
			if (HasBuildingMadeBuildProgress(building, pProgress, pLastSeenProgression))
			{
				ScreenHub._pInstance.BeginTweeningBricks(1.75f);
				PanCameraToBuilding(building, CameraHUB.EFocusType.PARTIAL_PROGRESS, fromCityLoad);
			}
			else if (DID_COME_FROM_GARAGE && ScreenHub._pInstance != null)
			{
				DID_COME_FROM_GARAGE = false;
				ScreenHub._pInstance.RefreshBricks();
				SetupBuildingWithProgress(building, pProgress);
				CameraHUB._pInstance.FocusPoint(ScreenHub._pInstance._pGarageLocation, CameraHUB.EFocusType.PAN_ONLY, 44f);
			}
			else
			{
				ScreenHub._pInstance.RefreshBricks();
				SetupBuildingWithProgress(building, pProgress);
				PanCameraToBuilding(building, CameraHUB.EFocusType.PAN_ONLY, fromCityLoad);
			}
		}
		else
		{
			ScreenHub._pInstance.BeginTweeningBricks(0f);
			ScreenHub._pInstance.PrepareScenarioComplete();
			PanCameraToBuilding(building, CameraHUB.EFocusType.SCENARIO_COMPLETE, fromCityLoad);
			result = true;
		}
		ScreenHub._pInstance.OnCityReady();
		return result;
	}

	public void RequestSetupCityFromOtherPlayer(string playerID, Action onOtherCityLoaded)
	{
		_onOtherCityLoadedAction = onOtherCityLoaded;
		ScenarioManager._pInstance.GetOtherPlayerScenarioData(playerID, PerformSetupCityFromOtherPlayer);
	}

	private void PerformSetupCityFromOtherPlayer(Scenario[] scenarioData)
	{
		int num = scenarioData.Length;
		int num2 = -1;
		ECITYBUILDINGS buildingID = ECITYBUILDINGS.INVALID;
		for (int i = 0; i < num; i++)
		{
			SetupBuildingWithProgress(scenarioData[i].building, scenarioData[i]._pProgress);
			if (scenarioData[i]._pCurrentBricks > num2)
			{
				buildingID = scenarioData[i].building;
				num2 = scenarioData[i]._pCurrentBricks;
			}
		}
		PanCameraToBuilding(buildingID, CameraHUB.EFocusType.PAN_ONLY);
		if (_onOtherCityLoadedAction != null)
		{
			_onOtherCityLoadedAction();
			_onOtherCityLoadedAction = null;
		}
	}

	public void SetCurrentSelection(ECITYBUILDINGS buildingID)
	{
		int num = cityElements.Length;
		for (int i = 0; i < num; i++)
		{
			bool flag = cityElements[i].buildingID == buildingID;
			if (flag && cityElements[i].buildSiteAnimator != null && !cityElements[i].buildSiteAnimator._pIsVisible)
			{
				cityElements[i].buildSiteTile.enabled = false;
				cityElements[i].buildSiteAnimator.SetToOutPosition();
				cityElements[i].buildSiteAnimator.animateIn = true;
				cityElements[i].buildSiteAnimator.SetRendererVisibilty(true);
				cityElements[i].buildSiteAnimator.KickoffBuildProcess();
			}
			if (cityElements[i].currentSelection != null)
			{
				cityElements[i].currentSelection.SetActive(flag);
			}
		}
	}

	public void SetupBuildingWithProgress(ECITYBUILDINGS buildingID, float progress)
	{
		int num = cityElements.Length;
		for (int i = 0; i < num; i++)
		{
			if (cityElements[i].buildingID != buildingID)
			{
				continue;
			}
			bool flag = progress == 0f;
			bool flag2 = ScenarioManager._pInstance.IsSlottedScenarioCity(buildingID);
			if (cityElements[i].buildSiteTile != null)
			{
				cityElements[i].buildSiteTile.enabled = flag && !flag2;
			}
			if (cityElements[i].buildSiteAnimator != null)
			{
				if (!flag2)
				{
					cityElements[i].buildSiteAnimator.SetRenderVisibilityOnBuildStage(0f);
				}
				else if (progress == 1f)
				{
					cityElements[i].buildSiteAnimator.SetRenderVisibilityOnBuildStage(0f);
				}
				else
				{
					cityElements[i].buildSiteAnimator.SetRenderVisibilityOnBuildStage(1f);
				}
			}
			cityElements[i].buildingAnimator.SetRenderVisibilityOnBuildStage(progress);
			break;
		}
	}

	public bool HasBuildingMadeBuildProgress(ECITYBUILDINGS buildingID, float progress, float lastKnownProgress)
	{
		int num = cityElements.Length;
		for (int i = 0; i < num; i++)
		{
			if (cityElements[i].buildingID == buildingID)
			{
				return cityElements[i].buildingAnimator.HasMadeProgress(progress, lastKnownProgress);
			}
		}
		return false;
	}

	public void SetLastSeenBuildStageToCurrent(ECITYBUILDINGS buildingID)
	{
		int num = cityElements.Length;
		for (int i = 0; i < num; i++)
		{
			if (cityElements[i].buildingID == buildingID)
			{
				Scenario scenarioFromBuildingID = ScenarioManager._pInstance.GetScenarioFromBuildingID(buildingID);
				if (scenarioFromBuildingID != null)
				{
					scenarioFromBuildingID._pLastSeenProgression = scenarioFromBuildingID._pProgress;
				}
				break;
			}
		}
	}

	public void ResetBuilding(ECITYBUILDINGS buildingID)
	{
		int num = cityElements.Length;
		for (int i = 0; i < num; i++)
		{
			if (cityElements[i].buildingID == buildingID)
			{
				cityElements[i].buildingAnimator.animateIn = false;
				cityElements[i].buildingAnimator.SetRenderVisibilityOnBuildStage(0f);
				break;
			}
		}
	}

	public void PlayProgressFXParticleSystem(ECITYBUILDINGS buildingID)
	{
		int num = cityElements.Length;
		for (int i = 0; i < num; i++)
		{
			if (cityElements[i].buildingID == buildingID)
			{
				cityElements[i].MoveProgressFXToCurvedWorldSpace();
				cityElements[i].TriggerProgressFX();
				break;
			}
		}
	}

	public void BuildEntireBuilding(ECITYBUILDINGS buildingID)
	{
		int num = cityElements.Length;
		for (int i = 0; i < num; i++)
		{
			if (cityElements[i].buildingID == buildingID)
			{
				if (cityElements[i].buildSiteAnimator != null)
				{
					cityElements[i].buildSiteAnimator.Sink();
				}
				cityElements[i].buildingAnimator.animateIn = true;
				cityElements[i].buildingAnimator.SetRendererVisibilty(true);
				cityElements[i].buildingAnimator.KickoffBuildProcess(OnBuildFullyComplete);
				SoundFacade._pInstance.PlayOneShotSFX("BuildingComplete", 0.6f);
				SoundFacade._pInstance.PlayOneShotSFX("Fanfare", 1.2f);
				SoundFacade._pInstance.PlayOneShotSFX("Fireworks", 1.2f);
				SoundFacade._pInstance.PlayOneShotSFX("GUIBrickTally", 0.6f);
				SoundFacade._pInstance.PlayOneShotSFX("GUIBrickTally", 0.85f);
				SoundFacade._pInstance.PlayOneShotSFX("GUIBrickTally", 1.1f);
				break;
			}
		}
	}

	public Vector3 GetCameraCenterFocusPos(ECITYBUILDINGS buildingID)
	{
		int num = cityElements.Length;
		for (int i = 0; i < num; i++)
		{
			if (cityElements[i].buildingID == buildingID)
			{
				return cityElements[i].cameraFocalPointManualClick.position;
			}
		}
		return Vector3.zero;
	}

	public void BuildPartialBuilding(ECITYBUILDINGS buildingID)
	{
		PlayProgressFXParticleSystem(buildingID);
		Debug.Log("BuildPartial");
		int num = cityElements.Length;
		for (int i = 0; i < num; i++)
		{
			if (cityElements[i].buildingID == buildingID)
			{
				cityElements[i].buildingAnimator.animateIn = true;
				cityElements[i].buildingAnimator.SetRendererVisibilty(true);
				cityElements[i].buildingAnimator.KickoffPartialBuildProcess(ScenarioManager._pInstance._pCurrentScenario._pLastSeenProgression, ScenarioManager._pInstance._pCurrentScenario._pProgress, OnBuildPartialComplete);
				SoundFacade._pInstance.PlayOneShotSFX("BuildingNewPiece", 0.25f);
				SoundFacade._pInstance.PlayOneShotSFX("GUIBrickTally", 0.6f);
				break;
			}
		}
	}

	public void PanCameraToBuilding(ECITYBUILDINGS buildingID, CameraHUB.EFocusType focusType, bool fromCityLoad = false)
	{
		int num = cityElements.Length;
		for (int i = 0; i < num; i++)
		{
			if (cityElements[i].buildingID == buildingID)
			{
				CameraHUB._pInstance.SetRegion(cityElements[i].region, fromCityLoad);
				if (focusType == CameraHUB.EFocusType.SCENARIO_COMPLETE)
				{
					cityElements[i].buildingAnimator.SetRendererVisibilty(false);
					cityElements[i].buildingAnimator.SetToOutPosition();
				}
				Vector3 point = ((focusType != CameraHUB.EFocusType.MANUAL && focusType != CameraHUB.EFocusType.PARTIAL_PROGRESS) ? cityElements[i].cameraFocalPoint.position : cityElements[i].cameraFocalPointManualClick.position);
				CameraHUB._pInstance.FocusPoint(point, focusType, cityElements[i].zoomDistance);
				break;
			}
		}
	}

	private void OnBuildFullyComplete()
	{
		if (!(Facades<ScreenFacade>.Instance == null))
		{
			if (SelectedBuildingHighlight._pLastSelected != null)
			{
				SelectedBuildingHighlight._pLastSelected.TweenOut();
			}
			ScreenScenarioComplete screenScenarioComplete = Facades<ScreenFacade>.Instance.GetScreen("ScreenScenarioComplete") as ScreenScenarioComplete;
			if (screenScenarioComplete != null && screenScenarioComplete.confettiParticles != null)
			{
				screenScenarioComplete.confettiParticles.Play();
			}
		}
	}

	private void OnBuildPartialComplete()
	{
		SetLastSeenBuildStageToCurrent(ScenarioManager._pInstance._pCurrentScenario.building);
		CameraHUB._pInstance.OnBuildPartialComplete();
	}

	public void Process3DIconClick()
	{
		if (Facades<FlowFacade>.Instance.CurrentLocation != "Hub")
		{
			return;
		}
		bool flag = false;
		flag = Input.touchCount > 0;
		Vector3 zero = Vector3.zero;
		if (flag)
		{
			zero = Input.GetTouch(0).position;
			_lastKnownTouchPos = zero;
			_wantTouchUpCheck = true;
		}
		if (flag && CameraHUB._pInstance._pDragDist < 50f && Time.time - CameraHUB._pInstance._pDragStartTime < 0.2f)
		{
			Debug.Log("Is Press Not Over? " + _isPressNotOverGUI);
			_wantTouchUpCheck = false;
			Ray ray = CameraHUB._pInstance._pCameraRef.ScreenPointToRay(_lastKnownTouchPos);
			RaycastHit hitInfo;
			if (!Physics.Raycast(ray, out hitInfo, 500f))
			{
				return;
			}
			MinigameIcon component = hitInfo.collider.GetComponent<MinigameIcon>();
			if (component != null)
			{
				component._pNumInteractions++;
				ScreenHub._pInstance.OnMinigame(hitInfo.collider.gameObject);
			}
			SpecialActionIcon component2 = hitInfo.collider.GetComponent<SpecialActionIcon>();
			if (component2 != null)
			{
				component2._pNumInteractions++;
				if (Facades<TrackingFacade>.Instance != null)
				{
					Facades<TrackingFacade>.Instance.LogEvent("Region_" + component2.actionType);
					Facades<TrackingFacade>.Instance.LogParameterMetric("Region", new Dictionary<string, string> { 
					{
						"Region",
						component2.actionType.ToString()
					} });
				}
				switch (component2.actionType)
				{
				case SpecialActionIcon.EACTION_TYPE.GARAGE:
					ScreenHub._pInstance.OnGarage();
					break;
				case SpecialActionIcon.EACTION_TYPE.SOCIAL:
					ScreenHub._pInstance.OnSocial();
					break;
				case SpecialActionIcon.EACTION_TYPE.TO_CITY:
					PanToRegion(REGIONS.CITY);
					break;
				case SpecialActionIcon.EACTION_TYPE.TO_VOLCANO:
					PanToRegion(REGIONS.VOLCANO);
					break;
				case SpecialActionIcon.EACTION_TYPE.TO_AIRPORT:
					PanToRegion(REGIONS.AIRPORT);
					break;
				}
			}
			BuildingClickableRegion component3 = hitInfo.collider.GetComponent<BuildingClickableRegion>();
			if (component3 != null)
			{
				Debug.Log("Player clicked building: " + component3.buildingType);
				if (CameraHUB._pInstance._pPlayerCanControl)
				{
					PanCameraToBuilding(component3.buildingType, CameraHUB.EFocusType.MANUAL);
					ScreenHub._pInstance.TweenOutScreenForManualPan(component3.buildingType);
				}
			}
		}
		else if (!flag && _wantTouchUpCheck)
		{
			_wantTouchUpCheck = false;
		}
	}

	private void Update()
	{
		UpdateCheats();
	}

	private void UpdateCheats()
	{
		if (Input.GetKeyDown(KeyCode.F6) && !Input.GetKey(KeyCode.LeftControl))
		{
			PanCameraToBuilding(ScenarioManager._pInstance._pCurrentScenario.building, CameraHUB.EFocusType.PAN_ONLY);
		}
		else if (Input.GetKeyDown(KeyCode.F6) && Input.GetKey(KeyCode.LeftControl))
		{
			VehiclePartManager._pInstance.UnlockAll();
		}
		if (Input.GetKeyDown(KeyCode.F7))
		{
			ScenarioManager._pInstance.AddBrickReward(100);
			GlobalInGameData._pCumulativeStuds += 1000;
			ScreenHub._pInstance.RefreshAfterRewards();
		}
		if (Input.GetKeyDown(KeyCode.F8))
		{
			GlobalInGameData._pCurrentExp += 100;
			ScreenHub._pInstance.RefreshEXP();
		}
	}

	public void OnPressNotOverGUI(bool isOver)
	{
		Process3DIconClick();
	}

	private void PanToRegion(REGIONS region)
	{
		CameraHUB._pInstance.PanToRegion(region);
	}
}
