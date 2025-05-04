using UnityEngine;

public class ScreenGarage : ScreenCustomiseVehicle
{
	private const string PARTS_COLLECTED_LOCAL_KEY = "VehicleBuilder.PartsCollected";

	private static ScreenGarage _instance;

	[SerializeField]
	private UILabel _partsCollectedLabel;

	public new static ScreenGarage _pInstance
	{
		get
		{
			return _instance;
		}
	}

	public UILabel _pPartsCollectedLabel
	{
		get
		{
			return _partsCollectedLabel;
		}
		set
		{
			_partsCollectedLabel = value;
		}
	}

	protected override bool _pRevealPlayButtonOnComplete
	{
		get
		{
			return true;
		}
	}

	protected override void Awake()
	{
		_instance = this;
	}

	protected override void OnDestroy()
	{
		_instance = null;
	}

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		SoundFacade._pInstance.PlayMusic("Collection", 0f);
		minigameType.gameObject.SetActive(false);
		_partsCollectedLabel.text = string.Empty;
	}

	public override void OnBuilderReady()
	{
		base.OnBuilderReady();
		_partsCollectedLabel.text = VehicleBuilder._pInstance._carouselBodies.GetNumUnlocked() + "/" + VehicleBuilder._pInstance._carouselBodies._pCarouselLength + " " + Localise("VehicleBuilder.PartsCollected");
	}

	public override void OnBuildStageChange(int buildStage)
	{
		base.OnBuildStageChange(buildStage);
	}

	protected override void OnExitScreen()
	{
		base.OnExitScreen();
		GlobalInGameData.SetCategoryViewed(GlobalInGameData._pGaragePartType);
	}

	public override void OnBack()
	{
		if (base._pCurrentTweenType == ScreenTweenType.Idle)
		{
			CityManager.DID_COME_FROM_GARAGE = true;
			GlobalInGameData.OnLevelWillLoad("Empty", ScreenLoading._pCurrentLevelName);
			Application.LoadLevel("Empty");
			ScreenHub.LoadDefaultHUB();
			Navigate("Hub");
			SoundFacade._pInstance.PlayOneShotSFX("GUIBack", 0f);
		}
	}

	public void OnSettings()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		Navigate("Settings");
	}

	public void OnDoOverPressed()
	{
		ScreenLoading.CoverNewSceneLoading();
		GlobalInGameData.OnLevelWillLoad("VehicleBuilder", ScreenLoading._pCurrentLevelName);
		Application.LoadLevel("VehicleBuilder");
	}

	public void TrackLockedPart(VehiclePart lockedPart)
	{
		if (!(lockedPart == null))
		{
		}
	}

	protected override void SetAccessoryInfoButton()
	{
		VehiclePart component = VehicleBuilder._pInstance._carouselBodies._pCurrItem.GetComponent<VehiclePart>();
		if (component.slotType == VehiclePart.EPART_SLOT_TYPE.ACCESSORY && _lastBuildStage == 0)
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
}
