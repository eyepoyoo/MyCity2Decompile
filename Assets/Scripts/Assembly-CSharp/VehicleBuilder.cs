using System;
using UnityEngine;
using UnitySampleAssets.Vehicles.Car;

public class VehicleBuilder : MonoBehaviour
{
	public const int INDEX_BODY = 0;

	public const int INDEX_WHEELS = 1;

	public const int INDEX_SPECIAL = 2;

	public const int INDEX_END = 3;

	private const float CLOSE_UP_CAM_SCALE_TO_WORLD_UNITS = 2f;

	private const float CAROUSEL_SHIFT_SPEED = 100f;

	private const float MODEL_SPIN_SPEED = 30f;

	private const float DEFAULT_MODEL_ROTATION = 136f;

	private const float ANIMATE_PIECE_DELAY = 0.4f;

	private const float NO_CAROUSEL_ROTATION_OVERRIDE = 999f;

	public static VehiclePartProperties _templateBody;

	public static VehiclePartProperties _templateWheel;

	public static VehiclePartProperties _templateSpecialAbility;

	public static VehiclePartProperties _templateAttachment;

	private readonly Vector3 CAROUSEL_CENTRE = new Vector3(0f, 7.5f, 8.5f);

	private readonly Vector3 CAROUSEL_SPACING = Vector3.right * 40f;

	public int _targetCarouselIndex;

	public Carousel _carouselBodies;

	public Carousel _carouselWheels;

	public Carousel _carouselSpecials;

	private Carousel _carouselAttachments;

	public Transform _camPosZoomedOut;

	public Transform _camPosCloseup;

	public GameObject instructionArrows;

	private float _currCarouselIndex;

	private Carousel[] _carousels;

	private CarouselBehaviour _carouselItemBody;

	private CarouselBehaviour _carouselItemWheel;

	private CarouselBehaviour _carouselItemSpecial;

	private CarouselBehaviour _carouselItemAttachment;

	private VehiclePart _body;

	private VehiclePart[] _wheels;

	private VehiclePart _special;

	private VehiclePart _attachment;

	private float _carouselRotationOverride = 999f;

	private GameObject _carouselDisplayPivot;

	public static VehicleBuilder _pInstance { get; private set; }

	public Action<MinigameManager.EVEHICLE_TYPE> _pOnSelectedVehicleAction { get; set; }

	public float _pCurrCarouselIndex
	{
		get
		{
			return _currCarouselIndex;
		}
		private set
		{
			_currCarouselIndex = Mathf.Clamp(value, 0f, _carousels.Length);
			UpdateCarouselPositions();
		}
	}

	public int _pCurrCarouselIndexRounded
	{
		get
		{
			return Mathf.RoundToInt(_pCurrCarouselIndex);
		}
	}

	public Carousel _pCurrCarousel
	{
		get
		{
			return (_pCurrCarouselIndexRounded >= _carousels.Length) ? null : _carousels[_pCurrCarouselIndexRounded];
		}
	}

	public int _pNumCarousels
	{
		get
		{
			return _carousels.Length;
		}
	}

	private bool _pCustomiseVehicleActive
	{
		get
		{
			return ScreenCustomiseVehicle._pInstance.gameObject.activeInHierarchy;
		}
	}

	public void Awake()
	{
		_pInstance = this;
		_templateAttachment = null;
		_templateBody = null;
		_templateSpecialAbility = null;
		_templateWheel = null;
		_carouselDisplayPivot = new GameObject("CarouselDisplayPivot");
		_carouselDisplayPivot.transform.position = Vector3.up * 1.5f;
		_carouselAttachments = _carouselWheels;
	}

	private void OnDestroy()
	{
		_pInstance = null;
	}

	public void Start()
	{
		_carousels = new Carousel[3] { _carouselBodies, _carouselWheels, _carouselSpecials };
		instructionArrows.SetActive(true);
		_pCurrCarouselIndex = 0f;
		if (_pCustomiseVehicleActive)
		{
			ScreenCustomiseVehicle._pInstance.OnBuilderReady();
			ScreenCustomiseVehicle._pInstance.OnBuildStageChange(_pCurrCarouselIndexRounded);
			ScreenCustomiseVehicle._pInstance.OnPartChange(_pCurrCarousel);
			return;
		}
		ScreenGarage._pInstance.OnBuilderReady();
		ScreenGarage._pInstance.OnBuildStageChange(_pCurrCarouselIndexRounded);
		ScreenGarage._pInstance.OnPartChange(_pCurrCarousel);
		VehiclePart component = _pCurrCarousel._pCurrItem.GetComponent<VehiclePart>();
		if (!component._pHasBeenUnlocked)
		{
			instructionArrows.SetActive(false);
		}
	}

	private void Update()
	{
		if (_pCurrCarouselIndex > (float)_targetCarouselIndex)
		{
			_pCurrCarouselIndex = Mathf.Max(_pCurrCarouselIndex - 100f * Time.deltaTime / CAROUSEL_SPACING.magnitude, _targetCarouselIndex);
		}
		else if (_pCurrCarouselIndex < (float)_targetCarouselIndex)
		{
			_pCurrCarouselIndex = Mathf.Min(_pCurrCarouselIndex + 100f * Time.deltaTime / CAROUSEL_SPACING.magnitude, _targetCarouselIndex);
		}
		if ((bool)_body)
		{
			if (_carouselRotationOverride != 999f)
			{
				float value = MathHelper.Wrap(_carouselDisplayPivot.transform.eulerAngles.y, -180f, 180f);
				MathHelper.EaseTowardsAngle(ref value, _carouselRotationOverride, 0.1f, Time.deltaTime);
				_carouselDisplayPivot.transform.eulerAngles = Vector3.up * value;
			}
			else
			{
				_carouselDisplayPivot.transform.eulerAngles += Vector3.up * 30f * Time.deltaTime;
			}
		}
	}

	private void UpdateCarouselPositions()
	{
		for (int i = 0; i < _carousels.Length; i++)
		{
			_carousels[i].transform.position = CAROUSEL_CENTRE;
			_carousels[i].SetCarouselActive(i == _targetCarouselIndex);
		}
	}

	private void TweenCameraToCloseUp()
	{
		VehiclePart body = _body;
		Vector3 vector = (_camPosCloseup.forward * body._completedCameraZoomFactor - _camPosCloseup.forward) * 2f;
		Camera.main.transform.TweenTo(_camPosCloseup.position + vector, _camPosCloseup.rotation, 1f, null, Easing.EaseType.EaseInOut, true, false, 0f);
		AmuzoMonoSingleton<VehicleBuilderBackdrop>._pInstance.TweenSkylineUp();
	}

	public void PreserveVehicleForGameplay()
	{
	}

	public bool IsValidBuild()
	{
		if (_body == null)
		{
			return false;
		}
		bool flag = _body.uniqueID == VehiclePart.EUNIQUE_ID.BODY_EXPLORER_UAV || _body.uniqueID == VehiclePart.EUNIQUE_ID.BODY_FLOATING_TYRE;
		if (_wheels == null && _attachment == null && !flag)
		{
			return false;
		}
		return true;
	}

	public void OnNextButton()
	{
		_pCurrCarousel.OnNextButton();
		if (_pCustomiseVehicleActive)
		{
			ScreenCustomiseVehicle._pInstance.OnPartChange(_carousels[_targetCarouselIndex]);
		}
		else
		{
			ScreenGarage._pInstance.OnPartChange(_carousels[_targetCarouselIndex]);
		}
	}

	public void OnPrevButton()
	{
		Debug.Log("PREV");
		_pCurrCarousel.OnPrevButton();
		if (_pCustomiseVehicleActive)
		{
			ScreenCustomiseVehicle._pInstance.OnPartChange(_carousels[_targetCarouselIndex]);
		}
		else
		{
			ScreenGarage._pInstance.OnPartChange(_carousels[_targetCarouselIndex]);
		}
	}

	public void OnPositionDirty()
	{
		if (_pCustomiseVehicleActive)
		{
			ScreenCustomiseVehicle._pInstance.OnPartChange(_carousels[_targetCarouselIndex]);
			return;
		}
		VehiclePart component = _carousels[_targetCarouselIndex]._pCurrItem.GetComponent<VehiclePart>();
		instructionArrows.SetActive(component._pHasBeenUnlocked);
		ScreenGarage._pInstance.OnPartChange(_carousels[_targetCarouselIndex]);
	}

	public void SelectCurrentPiece()
	{
		if (!_pCurrCarousel || !_pCurrCarousel._pCurrItem._isInCarousel)
		{
			return;
		}
		if (GlobalInGameData._pWantFullCarousel)
		{
			VehiclePart component = _pCurrCarousel._pCurrItem.GetComponent<VehiclePart>();
			if (!component._pHasBeenUnlocked)
			{
				return;
			}
		}
		CarouselBehaviour carouselBehaviour = _pCurrCarousel.WithdrawItem();
		MinigameManager.EVEHICLE_TYPE pCurrentVehicleTypeForMinigame = MinigameManager._pInstance._pCurrentVehicleTypeForMinigame;
		bool flag = pCurrentVehicleTypeForMinigame == MinigameManager.EVEHICLE_TYPE.LAND;
		if (pCurrentVehicleTypeForMinigame == MinigameManager.EVEHICLE_TYPE.INVALID && _carouselBodies._pCurrItem != null)
		{
			VehiclePart component2 = _carouselBodies._pCurrItem.GetComponent<VehiclePart>();
			if (component2.vehicleTypeCategories.Length <= 1)
			{
				flag = component2.IsVehicleType(MinigameManager.EVEHICLE_TYPE.LAND);
			}
			else if (_targetCarouselIndex == 0)
			{
				flag = component2.IsVehicleType(MinigameManager.EVEHICLE_TYPE.LAND);
			}
			else if (_targetCarouselIndex == 1)
			{
				component2 = _carouselWheels._pCurrItem.GetComponent<VehiclePart>();
				flag = component2.IsVehicleType(MinigameManager.EVEHICLE_TYPE.LAND);
			}
		}
		if (_targetCarouselIndex == 1 && GlobalInGameData._pGaragePartType == VehiclePart.EPART_SLOT_TYPE.INVALID)
		{
			_carouselSpecials.Repopulate(_body.partsThatAppaerInThirdCarouselPart);
		}
		switch (_pCurrCarouselIndexRounded)
		{
		case 0:
			SetBody(carouselBehaviour);
			_targetCarouselIndex++;
			if (!_carousels[_targetCarouselIndex]._pHasItems)
			{
				Debug.Log("Skipping wheels, as there are no items in the carousel (from body)");
				_targetCarouselIndex++;
				if (!_carousels[_targetCarouselIndex]._pHasItems)
				{
					Debug.Log("Skipping accessories, as there are no items in the carousel (from body)");
					_targetCarouselIndex++;
					TweenCameraToCloseUp();
					instructionArrows.SetActive(false);
				}
			}
			break;
		case 1:
			if (flag)
			{
				SetWheel(carouselBehaviour);
			}
			else
			{
				SetAttachment(carouselBehaviour);
			}
			_targetCarouselIndex++;
			if (!_carousels[_targetCarouselIndex]._pHasItems)
			{
				Debug.Log("Skipping wheels, as there are no items in the carousel");
				_targetCarouselIndex++;
				TweenCameraToCloseUp();
				instructionArrows.SetActive(false);
			}
			break;
		case 2:
			SetSpecial(carouselBehaviour);
			_targetCarouselIndex++;
			instructionArrows.SetActive(false);
			break;
		}
		if (_pCustomiseVehicleActive)
		{
			ScreenCustomiseVehicle._pInstance.OnBuildStageChange(_targetCarouselIndex);
			if (_targetCarouselIndex >= _carousels.Length)
			{
				ScreenCustomiseVehicle._pInstance.vehicleNameLabel.text = string.Empty;
			}
			else
			{
				ScreenCustomiseVehicle._pInstance.OnPartChange(_carousels[_targetCarouselIndex]);
			}
			return;
		}
		ScreenGarage._pInstance.OnBuildStageChange(_targetCarouselIndex);
		if (_targetCarouselIndex >= _carousels.Length)
		{
			ScreenGarage._pInstance.vehicleNameLabel.text = string.Empty;
			ScreenGarage._pInstance._pPartsCollectedLabel.text = string.Empty;
		}
		else
		{
			ScreenGarage._pInstance.OnPartChange(_carousels[_targetCarouselIndex]);
		}
	}

	public void ReturnCurrentPiece()
	{
		if (_pCurrCarouselIndexRounded != 0)
		{
			SoundFacade._pInstance.PlayOneShotSFX("CollateralDestroyed", 0f);
		}
		MinigameManager.EVEHICLE_TYPE eVEHICLE_TYPE = MinigameManager._pInstance._pCurrentVehicleTypeForMinigame;
		if (eVEHICLE_TYPE == MinigameManager.EVEHICLE_TYPE.INVALID && _carouselBodies != null && _carouselBodies._pCurrItem != null)
		{
			VehiclePart component = _carouselBodies._pCurrItem.GetComponent<VehiclePart>();
			if (component != null)
			{
				if (component.vehicleTypeCategories.Length <= 1)
				{
					eVEHICLE_TYPE = component.vehicleTypeCategories[0];
				}
				else if (_targetCarouselIndex == 2)
				{
					component = _carouselWheels._pCurrItem.GetComponent<VehiclePart>();
					if (component.IsVehicleType(MinigameManager.EVEHICLE_TYPE.LAND))
					{
						eVEHICLE_TYPE = MinigameManager.EVEHICLE_TYPE.LAND;
					}
				}
			}
		}
		bool flag = eVEHICLE_TYPE == MinigameManager.EVEHICLE_TYPE.LAND;
		switch (_pCurrCarouselIndexRounded)
		{
		case 1:
			SetBody(null);
			_targetCarouselIndex--;
			break;
		case 2:
			if (flag)
			{
				SetWheel(null);
			}
			else
			{
				SetAttachment(null);
			}
			_targetCarouselIndex--;
			if (!_carousels[_targetCarouselIndex]._pHasItems)
			{
				SetBody(null);
				_targetCarouselIndex--;
			}
			break;
		case 3:
			SetSpecial(null);
			_targetCarouselIndex--;
			instructionArrows.SetActive(true);
			if (!_carousels[_targetCarouselIndex]._pHasItems)
			{
				if (flag)
				{
					SetWheel(null);
				}
				else
				{
					SetAttachment(null);
				}
				_targetCarouselIndex--;
				if (!_carousels[_targetCarouselIndex]._pHasItems)
				{
					SetBody(null);
					_targetCarouselIndex--;
				}
			}
			break;
		}
		if (_pCustomiseVehicleActive)
		{
			ScreenCustomiseVehicle._pInstance.OnBuildStageChange(_targetCarouselIndex);
			ScreenCustomiseVehicle._pInstance.OnPartChange(_carousels[_targetCarouselIndex]);
		}
		else
		{
			ScreenGarage._pInstance.OnBuildStageChange(_targetCarouselIndex);
			ScreenGarage._pInstance.OnPartChange(_carousels[_targetCarouselIndex]);
		}
	}

	public void SetBody(CarouselBehaviour carouselItem)
	{
		if ((bool)_carouselItemBody)
		{
			_carouselBodies.ReturnItem(_carouselItemBody);
		}
		_carouselItemBody = carouselItem;
		if ((bool)carouselItem)
		{
			_body = carouselItem.GetComponent<VehiclePart>();
			_body.transform.parent = _carouselDisplayPivot.transform;
			_body.transform.localRotation = Quaternion.identity;
			_body.transform.localPosition = Vector3.zero;
			_body.transform.localScale = _body.carouselPreviewScale;
			_templateBody = VehiclePartManager._pInstance.GetPartPropertiesFromPartID(_body.uniqueID);
			Vector3 vector = _carouselDisplayPivot.transform.InverseTransformPoint(_body._pCentralPoint);
			_body.transform.localPosition = -vector;
			_carouselDisplayPivot.transform.eulerAngles = Vector3.up * 136f;
			Debug.Log("Setting Body: " + carouselItem.name + " _body=" + _body);
			if (GlobalInGameData._pGaragePartType == VehiclePart.EPART_SLOT_TYPE.INVALID)
			{
				_carouselWheels.Repopulate(_body.partsThatAppearInSecondCarouselPart);
				_carouselSpecials.Repopulate(_body.partsThatAppaerInThirdCarouselPart);
			}
		}
		else
		{
			_templateBody = null;
			_body = null;
			_carouselRotationOverride = 999f;
		}
		UpdateCarouselPositions();
	}

	public void SetWheel(CarouselBehaviour carouselItem)
	{
		if ((bool)_carouselItemWheel)
		{
			_carouselWheels.ReturnItem(_carouselItemWheel);
			Array.ForEach(_wheels, delegate(VehiclePart w)
			{
				UnityEngine.Object.Destroy(w.gameObject);
			});
		}
		_carouselItemWheel = carouselItem;
		if ((bool)carouselItem && (bool)_body)
		{
			_carouselItemWheel.transform.localScale = _body.carouselPreviewScale;
			_wheels = new VehiclePart[4];
			VehiclePart componentInChildren = _carouselItemWheel.GetComponentInChildren<VehiclePart>();
			_carouselRotationOverride = GetCarouselRotationOverrideFromPiece(componentInChildren);
			_wheels[0] = UnityEngine.Object.Instantiate(componentInChildren);
			_wheels[0].connectionType = Connectors.ECONNECTOR_TYPE.WHEEL_FL;
			_wheels[0].AttachToVehicle_Animate(_body.GetComponent<Vehicle>(), 0.4f, OnAttachedToVehicle);
			_wheels[0].GetComponentInChildren<Wheel>().SetFrontLeft();
			_wheels[1] = UnityEngine.Object.Instantiate(componentInChildren);
			_wheels[1].connectionType = Connectors.ECONNECTOR_TYPE.WHEEL_FR;
			_wheels[1].AttachToVehicle_Animate(_body.GetComponent<Vehicle>(), 0.6f, OnAttachedToVehicle);
			_wheels[1].GetComponentInChildren<Wheel>().SetFrontRight();
			_wheels[2] = UnityEngine.Object.Instantiate(componentInChildren);
			_wheels[2].connectionType = Connectors.ECONNECTOR_TYPE.WHEEL_BR;
			_wheels[2].AttachToVehicle_Animate(_body.GetComponent<Vehicle>(), 0.8f, OnAttachedToVehicle);
			_wheels[2].GetComponentInChildren<Wheel>().SetBackRight();
			_wheels[3] = UnityEngine.Object.Instantiate(componentInChildren);
			_wheels[3].connectionType = Connectors.ECONNECTOR_TYPE.WHEEL_BL;
			_wheels[3].AttachToVehicle_Animate(_body.GetComponent<Vehicle>(), 1f, delegate
			{
				OnAttachedToVehicle(_wheels[3]);
				_carouselRotationOverride = 999f;
			});
			_wheels[3].GetComponentInChildren<Wheel>().SetBackLeft();
			_templateWheel = VehiclePartManager._pInstance.GetPartPropertiesFromPartID(componentInChildren.uniqueID);
			carouselItem.gameObject.SetActive(false);
		}
		else
		{
			_templateWheel = null;
			_wheels = null;
			_carouselRotationOverride = 999f;
		}
		UpdateCarouselPositions();
	}

	public void SetAttachment(CarouselBehaviour carouselItem)
	{
		if (_body == null)
		{
			Debug.LogError("Null Body for Attachment: " + carouselItem.name);
			return;
		}
		if ((bool)_carouselItemAttachment)
		{
			_carouselAttachments.ReturnItem(_carouselItemAttachment);
		}
		_carouselItemAttachment = carouselItem;
		if ((bool)carouselItem && (bool)_body)
		{
			_attachment = carouselItem.GetComponent<VehiclePart>();
			_attachment.transform.localScale = _body.carouselPreviewScale;
			_attachment.AttachToVehicle(_body.GetComponent<Vehicle>());
			OnAttachedToVehicle(_attachment);
			_templateAttachment = VehiclePartManager._pInstance.GetPartPropertiesFromPartID(_attachment.uniqueID);
		}
		else
		{
			_templateAttachment = null;
			_attachment = null;
			_carouselRotationOverride = 999f;
		}
		UpdateCarouselPositions();
	}

	public void SetSpecial(CarouselBehaviour carouselItem)
	{
		if ((bool)_carouselItemSpecial)
		{
			_carouselSpecials.ReturnItem(_carouselItemSpecial);
		}
		_carouselItemSpecial = carouselItem;
		if ((bool)carouselItem && (bool)_body)
		{
			_special = carouselItem.GetComponent<VehiclePart>();
			_special.transform.localScale = _body.carouselPreviewScale;
			_templateSpecialAbility = VehiclePartManager._pInstance.GetPartPropertiesFromPartID(_special.uniqueID);
			if (_special._animateWhenAttached)
			{
				_carouselRotationOverride = GetCarouselRotationOverrideFromPiece(_special);
				_special.AttachToVehicle_Animate(_body.GetComponent<Vehicle>(), 0.4f, delegate
				{
					OnAttachedToVehicle(_special);
					_carouselRotationOverride = 999f;
					TweenCameraToCloseUp();
				});
			}
			else
			{
				_special.AttachToVehicle(_body.GetComponent<Vehicle>());
				OnAttachedToVehicle(_special);
				TweenCameraToCloseUp();
			}
		}
		else
		{
			_templateSpecialAbility = null;
			_special = null;
			Camera.main.transform.TweenTo(_camPosZoomedOut.position, _camPosZoomedOut.rotation, 1f, null, Easing.EaseType.EaseInOut, true, false, 0f);
			AmuzoMonoSingleton<VehicleBuilderBackdrop>._pInstance.TweenSkylineNormal();
			_carouselRotationOverride = 999f;
		}
		UpdateCarouselPositions();
	}

	private void OnAttachedToVehicle(VehiclePart part)
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUILegoClick", 0f);
		if (!string.IsNullOrEmpty(part._soundToPlayOnAttached))
		{
			SoundFacade._pInstance.PlayOneShotSFX(part._soundToPlayOnAttached, 0.2f);
		}
	}

	private float GetCarouselRotationOverrideFromPiece(VehiclePart vehiclePart)
	{
		switch (vehiclePart.connectionType)
		{
		case Connectors.ECONNECTOR_TYPE.TOP_BACK:
		case Connectors.ECONNECTOR_TYPE.BACK:
			return 316f;
		case Connectors.ECONNECTOR_TYPE.FRONT_SIDES:
		case Connectors.ECONNECTOR_TYPE.BACK_SIDES:
			return -136f;
		default:
			return 136f;
		}
	}

	public static void ClearTemplate()
	{
		_templateAttachment = null;
		_templateBody = null;
		_templateSpecialAbility = null;
		_templateWheel = null;
	}
}
