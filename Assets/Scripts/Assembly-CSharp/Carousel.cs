using System.Collections.Generic;
using UnityEngine;

public class Carousel : MonoBehaviour
{
	public const float TRANSITION_RATE = 5f;

	private VehiclePartProperties[] _itemProperties;

	public Material blackoutMaterial;

	public float itemSpacing = 25f;

	public Vector3 displayAngle;

	public Vector3 normalScale;

	public Vector3 selectedScale;

	public VehiclePart.EPART_SLOT_TYPE slotType;

	private CarouselBehaviour[] _itemInstances;

	private int _currIndex;

	private int _previousIndex;

	private bool _previouslyOnTarget = true;

	private bool _changedOnTarget;

	private bool _hasReturnedToCenter;

	private bool _wasEasedIndexManualOverrideSet;

	private bool _isEasedIndexManualOverrideSet;

	private float _easedIndexManualOverride = -1f;

	private float _easedIndex;

	private float _restStopTime;

	private bool _isCarouselActive;

	private bool _tweeningItemsIn;

	private float _tweenStartTime;

	private int _defaultIndex;

	private int _lastReportedIndexForDirtyFlag = -1;

	private List<VehiclePartProperties> _dummyList = new List<VehiclePartProperties>();

	private Vector3 _dummyVector3;

	private bool _hasItems;

	public bool _pHasItems
	{
		get
		{
			return _hasItems && _itemInstances != null && _itemInstances.Length > 0;
		}
	}

	public int _pCarouselLength
	{
		get
		{
			return _itemProperties.Length;
		}
	}

	public bool _pIsApproximatelyOnTarget
	{
		get
		{
			return Mathf.Abs((float)_currIndex - _easedIndex) < 0.1f;
		}
	}

	public float _pCurrEasedIndex
	{
		get
		{
			return _easedIndex;
		}
	}

	public int _pCurrIndex
	{
		get
		{
			return _currIndex;
		}
		set
		{
			_currIndex = Mathf.Clamp(value, 0, _itemProperties.Length - 1);
			if (slotType == VehiclePart.EPART_SLOT_TYPE.BODY && VehicleBuilder._pInstance != null && VehicleBuilder._pInstance._pOnSelectedVehicleAction != null)
			{
				VehiclePartProperties vehiclePartProperties = _itemProperties[_currIndex];
				if (vehiclePartProperties.vehicleTypeCategories != null)
				{
					VehicleBuilder._pInstance._pOnSelectedVehicleAction(vehiclePartProperties.vehicleTypeCategories[0]);
				}
			}
		}
	}

	public CarouselBehaviour _pCurrItem
	{
		get
		{
			if (_itemInstances == null)
			{
				Debug.LogError("Carousel : " + base.name + " has null instances", base.gameObject);
				return null;
			}
			if (_currIndex >= _itemInstances.Length)
			{
				Debug.LogError("Carousel: " + base.name + " is out of range (asking for " + _currIndex + " from " + _itemInstances.Length + " )", base.gameObject);
				return null;
			}
			return _itemInstances[_currIndex];
		}
	}

	public string _pCurItemLocalisation
	{
		get
		{
			VehiclePart component = _itemInstances[_currIndex].GetComponent<VehiclePart>();
			if (component != null)
			{
				return component.localisationKey;
			}
			return string.Empty;
		}
	}

	public Vector3 _pInfoPanelCameraPos
	{
		get
		{
			VehiclePart component = _itemInstances[_currIndex].GetComponent<VehiclePart>();
			if (component != null)
			{
				return component.carouselInfoPanelCameraPos;
			}
			return Vector3.zero;
		}
	}

	public void Awake()
	{
		if (slotType == VehiclePart.EPART_SLOT_TYPE.BODY)
		{
			if (GlobalInGameData._pGaragePartType != VehiclePart.EPART_SLOT_TYPE.INVALID)
			{
				Repopulate(null, true, GlobalInGameData._pGaragePartType);
			}
			else
			{
				Repopulate(null, true);
			}
			return;
		}
		VehiclePart.EUNIQUE_ID[] validPartTypes = new VehiclePart.EUNIQUE_ID[0];
		if (GlobalInGameData._pGaragePartType != VehiclePart.EPART_SLOT_TYPE.INVALID)
		{
			Repopulate(validPartTypes);
		}
		else
		{
			Repopulate(null, true);
		}
	}

	public void Repopulate(VehiclePart.EUNIQUE_ID[] validPartTypes, bool populateAllIfNullList = false, VehiclePart.EPART_SLOT_TYPE forcedSlot = VehiclePart.EPART_SLOT_TYPE.INVALID)
	{
		MinigameManager.EVEHICLE_TYPE eVEHICLE_TYPE = MinigameManager._pInstance._pCurrentVehicleTypeForMinigame;
		bool flag = false;
		bool flag2 = false;
		if (eVEHICLE_TYPE == MinigameManager.EVEHICLE_TYPE.INVALID)
		{
			if (VehicleBuilder._pInstance != null && VehicleBuilder._pInstance._carouselBodies != null && VehicleBuilder._pInstance._carouselBodies._pCurrItem != null)
			{
				VehiclePart component = VehicleBuilder._pInstance._carouselBodies._pCurrItem.GetComponent<VehiclePart>();
				if (VehicleBuilder._pInstance._targetCarouselIndex == 1)
				{
					component = VehicleBuilder._pInstance._carouselWheels._pCurrItem.GetComponent<VehiclePart>();
				}
				if (component != null)
				{
					if (component.vehicleTypeCategories.Length <= 1)
					{
						eVEHICLE_TYPE = component.vehicleTypeCategories[0];
						flag = false;
					}
					else
					{
						eVEHICLE_TYPE = component.vehicleTypeCategories[0];
						flag = true;
					}
				}
			}
		}
		else
		{
			MinigameManager.MinigameData currentMinigameData = MinigameManager._pInstance.GetCurrentMinigameData();
			flag2 = MinigameManager._pInstance._pSelectedVehicleTemplate != 2;
		}
		switch (eVEHICLE_TYPE)
		{
		case MinigameManager.EVEHICLE_TYPE.WATER:
			if (slotType == VehiclePart.EPART_SLOT_TYPE.WHEEL)
			{
				slotType = VehiclePart.EPART_SLOT_TYPE.ATTACHMENT;
			}
			break;
		case MinigameManager.EVEHICLE_TYPE.AIR:
			if (slotType == VehiclePart.EPART_SLOT_TYPE.WHEEL)
			{
				slotType = VehiclePart.EPART_SLOT_TYPE.ATTACHMENT;
			}
			break;
		case MinigameManager.EVEHICLE_TYPE.LAND:
			if (slotType == VehiclePart.EPART_SLOT_TYPE.ATTACHMENT)
			{
				slotType = VehiclePart.EPART_SLOT_TYPE.WHEEL;
			}
			break;
		}
		if (forcedSlot != VehiclePart.EPART_SLOT_TYPE.INVALID)
		{
			slotType = forcedSlot;
		}
		if ((validPartTypes == null || validPartTypes.Length == 0) && populateAllIfNullList)
		{
			Debug.Log("Carousel " + base.name + " Repop 1");
			VehiclePartProperties[] array = null;
			if (MinigameManager._pInstance._pCurrentVehicleTypeForMinigame != MinigameManager.EVEHICLE_TYPE.INVALID)
			{
				array = VehiclePartManager._pInstance.GetAllPartPropertiesForSlotAndTerrainType(MinigameManager._pInstance._pCurrentVehicleTypeForMinigame, slotType).ToArray();
			}
			else
			{
				array = VehiclePartManager._pInstance.GetAllPartPropertiesForSlotType(slotType).ToArray();
				Debug.Log("Carousel " + base.name + " Found: " + array.Length + " parts");
			}
			MinigameManager.VehicleTemplate currentVehicleTemplate = MinigameManager._pInstance.GetCurrentVehicleTemplate();
			_dummyList.Clear();
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				VehiclePartProperties vehiclePartProperties = array[i];
				if (vehiclePartProperties == null)
				{
					Debug.Log("Null VP");
				}
				else if (vehiclePartProperties.partIsAvailable)
				{
					bool flag3 = GlobalInGameData.HasPartBeenUnlocked(vehiclePartProperties.uniqueID);
					if (GlobalInGameData._pWantFullCarousel)
					{
						flag3 = true;
					}
					if (flag2)
					{
						flag3 = false;
					}
					if (flag3 || vehiclePartProperties.uniqueID == currentVehicleTemplate.bodyPart || vehiclePartProperties.uniqueID == currentVehicleTemplate.attachmentPart || vehiclePartProperties.uniqueID == currentVehicleTemplate.accessoryPart || vehiclePartProperties.uniqueID == currentVehicleTemplate.wheelPart)
					{
						_dummyList.Add(vehiclePartProperties);
					}
				}
			}
			_itemProperties = _dummyList.ToArray();
			_hasItems = true;
		}
		else
		{
			Debug.Log("Carousel " + base.name + " Repop 2");
			AddItemsToCarouselList(validPartTypes, VehiclePart.EPART_SLOT_TYPE.INVALID);
			if (flag)
			{
				Debug.Log("Multitype!");
				if (MinigameManager._pInstance._pCurrentVehicleTypeForMinigame == MinigameManager.EVEHICLE_TYPE.INVALID && VehicleBuilder._pInstance != null && VehicleBuilder._pInstance._carouselBodies != null && VehicleBuilder._pInstance._carouselBodies._pCurrItem != null)
				{
					VehiclePart component2 = VehicleBuilder._pInstance._carouselBodies._pCurrItem.GetComponent<VehiclePart>();
					int num2 = component2.vehicleTypeCategories.Length;
					for (int j = 1; j < num2; j++)
					{
						Debug.Log("Dealing with additional category: " + component2.vehicleTypeCategories[j]);
						VehiclePart.EPART_SLOT_TYPE ePART_SLOT_TYPE = slotType;
						if (component2.vehicleTypeCategories[j] == MinigameManager.EVEHICLE_TYPE.WATER)
						{
							if (ePART_SLOT_TYPE == VehiclePart.EPART_SLOT_TYPE.WHEEL)
							{
								ePART_SLOT_TYPE = VehiclePart.EPART_SLOT_TYPE.ATTACHMENT;
							}
						}
						else if (component2.vehicleTypeCategories[j] == MinigameManager.EVEHICLE_TYPE.AIR)
						{
							if (ePART_SLOT_TYPE == VehiclePart.EPART_SLOT_TYPE.WHEEL)
							{
								ePART_SLOT_TYPE = VehiclePart.EPART_SLOT_TYPE.ATTACHMENT;
							}
						}
						else if (component2.vehicleTypeCategories[j] == MinigameManager.EVEHICLE_TYPE.LAND && ePART_SLOT_TYPE == VehiclePart.EPART_SLOT_TYPE.ATTACHMENT)
						{
							ePART_SLOT_TYPE = VehiclePart.EPART_SLOT_TYPE.WHEEL;
						}
						if (forcedSlot != VehiclePart.EPART_SLOT_TYPE.INVALID)
						{
							ePART_SLOT_TYPE = forcedSlot;
						}
						AddItemsToCarouselList(validPartTypes, ePART_SLOT_TYPE);
					}
				}
			}
			_itemProperties = _dummyList.ToArray();
			if (_itemProperties == null || _itemProperties.Length == 0)
			{
				_hasItems = false;
			}
			else
			{
				_hasItems = true;
			}
		}
		if (_itemInstances != null)
		{
			int num3 = _itemInstances.Length;
			for (int k = 0; k < num3; k++)
			{
				if (_itemInstances[k] != null)
				{
					Object.Destroy(_itemInstances[k].gameObject);
				}
			}
		}
		_itemInstances = new CarouselBehaviour[_itemProperties.Length];
		for (int l = 0; l < _itemProperties.Length; l++)
		{
			VehiclePartProperties vehiclePartProperties2 = _itemProperties[l];
			if (MinigameManager._pInstance._pCurrentVehicleTypeForMinigame != MinigameManager.EVEHICLE_TYPE.INVALID)
			{
				switch (MinigameManager._pInstance._pCurrentVehicleTypeForMinigame)
				{
				case MinigameManager.EVEHICLE_TYPE.AIR:
					vehiclePartProperties2 = vehiclePartProperties2.GetAltPartOrThis(Vehicle.EType.Air);
					break;
				case MinigameManager.EVEHICLE_TYPE.LAND:
					vehiclePartProperties2 = vehiclePartProperties2.GetAltPartOrThis(Vehicle.EType.Land);
					break;
				case MinigameManager.EVEHICLE_TYPE.WATER:
					vehiclePartProperties2 = vehiclePartProperties2.GetAltPartOrThis(Vehicle.EType.Boat);
					break;
				}
			}
			_itemInstances[l] = CarouselBehaviour.GetCarouselReadyClone(vehiclePartProperties2.GetResource());
			_itemInstances[l]._isInCarousel = true;
			_itemInstances[l].transform.parent = base.transform;
			_itemInstances[l].transform.localScale = Vector3.zero;
			if (GlobalInGameData._pWantFullCarousel)
			{
				VehiclePart component3 = _itemInstances[l].GetComponent<VehiclePart>();
				if (component3 != null && !component3._pHasBeenUnlocked)
				{
					ScreenGarage._pInstance.TrackLockedPart(component3);
					SetItemSilhouetted(_itemInstances[l].gameObject, _itemInstances[l]);
				}
			}
			StopParticleSystems(_itemInstances[l].gameObject);
		}
		FindDefaultItem();
		_currIndex = _defaultIndex;
		_easedIndex = _currIndex;
		_easedIndexManualOverride = -1f;
		_isEasedIndexManualOverrideSet = false;
		UpdateItemPositions();
	}

	public int GetNumUnlocked()
	{
		int num = 0;
		int num2 = _itemInstances.Length;
		for (int i = 0; i < num2; i++)
		{
			VehiclePart component = _itemInstances[i].GetComponent<VehiclePart>();
			if (component._pHasBeenUnlocked)
			{
				num++;
			}
		}
		return num;
	}

	private void AddItemsToCarouselList(VehiclePart.EUNIQUE_ID[] validPartTypes, VehiclePart.EPART_SLOT_TYPE overrideSlotType)
	{
		VehiclePartProperties[] array = null;
		bool flag = false;
		VehiclePart.EPART_SLOT_TYPE ePART_SLOT_TYPE = slotType;
		if (overrideSlotType != VehiclePart.EPART_SLOT_TYPE.INVALID)
		{
			ePART_SLOT_TYPE = overrideSlotType;
		}
		if (MinigameManager._pInstance._pCurrentVehicleTypeForMinigame != MinigameManager.EVEHICLE_TYPE.INVALID)
		{
			MinigameManager.MinigameData currentMinigameData = MinigameManager._pInstance.GetCurrentMinigameData();
			flag = MinigameManager._pInstance._pSelectedVehicleTemplate != 2;
			array = VehiclePartManager._pInstance.GetAllPartPropertiesForSlotAndTerrainType(MinigameManager._pInstance._pCurrentVehicleTypeForMinigame, ePART_SLOT_TYPE).ToArray();
		}
		else if (VehicleBuilder._pInstance._targetCarouselIndex == 0)
		{
			array = VehiclePartManager._pInstance.GetAllPartPropertiesForSlotType(ePART_SLOT_TYPE).ToArray();
		}
		else if (VehicleBuilder._pInstance._targetCarouselIndex == 1)
		{
			MinigameManager.EVEHICLE_TYPE vehicleType = MinigameManager.EVEHICLE_TYPE.INVALID;
			VehiclePart component = VehicleBuilder._pInstance._carouselBodies._pCurrItem.GetComponent<VehiclePart>();
			if (component != null)
			{
				if (component.vehicleTypeCategories.Length <= 1)
				{
					vehicleType = component.vehicleTypeCategories[0];
				}
				else
				{
					component = VehicleBuilder._pInstance._carouselWheels._pCurrItem.GetComponent<VehiclePart>();
					if (component.vehicleTypeCategories.Length > 1)
					{
						Debug.LogWarning("Warning - unhandled situation - more than one category on a wheel/attachment part, carousel may misbehave");
					}
					vehicleType = component.vehicleTypeCategories[0];
				}
			}
			array = VehiclePartManager._pInstance.GetAllPartPropertiesForSlotAndTerrainType(vehicleType, ePART_SLOT_TYPE).ToArray();
		}
		int num = validPartTypes.Length;
		int num2 = array.Length;
		MinigameManager.VehicleTemplate currentVehicleTemplate = MinigameManager._pInstance.GetCurrentVehicleTemplate();
		VehiclePartProperties[] array2 = new VehiclePartProperties[num2];
		if (overrideSlotType == VehiclePart.EPART_SLOT_TYPE.INVALID)
		{
			_dummyList.Clear();
		}
		for (int i = 0; i < num2; i++)
		{
			array2[i] = array[i];
			for (int j = 0; j < num; j++)
			{
				if (array2[i].uniqueID == validPartTypes[j])
				{
					bool flag2 = GlobalInGameData.HasPartBeenUnlocked(array2[i].uniqueID);
					if (GlobalInGameData._pWantFullCarousel)
					{
						flag2 = true;
					}
					if (flag)
					{
						flag2 = false;
					}
					if (flag2 || array2[i].uniqueID == currentVehicleTemplate.bodyPart || array2[i].uniqueID == currentVehicleTemplate.attachmentPart || array2[i].uniqueID == currentVehicleTemplate.accessoryPart || array2[i].uniqueID == currentVehicleTemplate.wheelPart)
					{
						_dummyList.Add(array2[i]);
					}
				}
			}
		}
	}

	public void OnNextButton()
	{
		_pCurrIndex++;
	}

	public void OnPrevButton()
	{
		_pCurrIndex--;
	}

	public void SetCarouselActive(bool activeState)
	{
		if (activeState == _isCarouselActive)
		{
			return;
		}
		_isCarouselActive = activeState;
		if (!_isCarouselActive)
		{
			for (int num = _itemInstances.Length - 1; num >= 0; num--)
			{
				if (_itemInstances[num]._isInCarousel)
				{
					_itemInstances[num].transform.localScale = Vector3.zero;
					StopParticleSystems(_itemInstances[num].gameObject);
				}
			}
			return;
		}
		for (int num2 = _itemInstances.Length - 1; num2 >= 0; num2--)
		{
			if (_itemInstances[num2]._isInCarousel)
			{
				_itemInstances[num2].transform.localScale = Vector3.zero;
				StartParticleSystems(_itemInstances[num2].gameObject);
			}
		}
		_tweeningItemsIn = true;
		_tweenStartTime = RealTime.time;
	}

	public void Update()
	{
		if (!_isCarouselActive)
		{
			return;
		}
		if (_isEasedIndexManualOverrideSet)
		{
			_easedIndex = _easedIndexManualOverride;
			_isEasedIndexManualOverrideSet = false;
			_wasEasedIndexManualOverrideSet = true;
		}
		else
		{
			if (_wasEasedIndexManualOverrideSet)
			{
				_currIndex = Mathf.Clamp(Mathf.RoundToInt(_easedIndex), 0, _itemProperties.Length - 1);
			}
			_easedIndex = Mathf.MoveTowards(_easedIndex, _currIndex, Time.deltaTime * 5f);
		}
		if (_currIndex != _previousIndex)
		{
			SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		}
		if (!_pIsApproximatelyOnTarget && _pIsApproximatelyOnTarget != _previouslyOnTarget)
		{
			_changedOnTarget = true;
			_hasReturnedToCenter = false;
		}
		if (_easedIndex == (float)_currIndex)
		{
			_hasReturnedToCenter = true;
			_wasEasedIndexManualOverrideSet = false;
		}
		if (_changedOnTarget && Input.touchCount == 0 && _hasReturnedToCenter)
		{
			SoundFacade._pInstance.PlayOneShotSFX("GUIBang", 0f);
			_changedOnTarget = false;
		}
		_previousIndex = _currIndex;
		_previouslyOnTarget = _pIsApproximatelyOnTarget;
		UpdateItemPositions();
	}

	private void UpdateItemPositions()
	{
		int num = _itemInstances.Length;
		bool pIsApproximatelyOnTarget = _pIsApproximatelyOnTarget;
		for (int i = 0; i < num; i++)
		{
			if (!_itemInstances[i]._isInCarousel)
			{
				continue;
			}
			_itemInstances[i].UpdateOverride();
			VehiclePart component = _itemInstances[i].GetComponent<VehiclePart>();
			_itemInstances[i].transform.localPosition = Vector3.zero;
			Vector3 vector = base.transform.InverseTransformPoint(component._pCentralPoint);
			_itemInstances[i].transform.localPosition = -vector + Vector3.right * itemSpacing * ((float)i - _easedIndex) + Vector3.up * _itemInstances[i]._pOffset;
			_itemInstances[i].transform.localEulerAngles = displayAngle;
			Vector3 carouselNormalScale = component.carouselNormalScale;
			Vector3 carouselSelectedScale = component.carouselSelectedScale;
			if (!_isCarouselActive)
			{
				continue;
			}
			if (pIsApproximatelyOnTarget)
			{
				if (i == _currIndex)
				{
					float num2 = RealTime.time - _restStopTime;
					if (num2 < 0.5f)
					{
						float t = Easing.Ease(Easing.EaseType.EaseOutElastic, num2, 0.5f, 0f, 1f);
						Vector3 localScale = Vector3.Lerp(carouselNormalScale, carouselSelectedScale, t);
						_itemInstances[i].transform.localScale = localScale;
						if (_currIndex != _lastReportedIndexForDirtyFlag)
						{
							VehicleBuilder._pInstance.OnPositionDirty();
							_lastReportedIndexForDirtyFlag = _currIndex;
						}
					}
					else
					{
						_itemInstances[i].transform.localScale = carouselSelectedScale;
						if (_currIndex != _lastReportedIndexForDirtyFlag)
						{
							VehicleBuilder._pInstance.OnPositionDirty();
							_lastReportedIndexForDirtyFlag = _currIndex;
						}
					}
				}
				else
				{
					_itemInstances[i].transform.localScale = carouselNormalScale;
				}
			}
			else
			{
				_itemInstances[i].transform.localScale = carouselNormalScale;
				_restStopTime = RealTime.time;
			}
		}
	}

	public CarouselBehaviour WithdrawItem(CarouselBehaviour item = null)
	{
		item = item ?? _pCurrItem;
		item._isInCarousel = false;
		item.transform.parent = null;
		item.transform.localRotation = Quaternion.identity;
		return item;
	}

	public void ReturnItem(CarouselBehaviour item)
	{
		VehiclePart component = item.GetComponent<VehiclePart>();
		component.OnReturnItem();
		item._isInCarousel = true;
		item.transform.parent = base.transform;
		item.transform.localScale = component.carouselSelectedScale;
		item.gameObject.SetActive(true);
		UpdateItemPositions();
	}

	public void SetEasedIndexOveride(float easedIndexOverride)
	{
		_isEasedIndexManualOverrideSet = true;
		_easedIndexManualOverride = Mathf.Clamp(easedIndexOverride, 0f, _pCarouselLength - 1);
	}

	private void StartParticleSystems(GameObject obj)
	{
		SpecialAbility component = obj.GetComponent<SpecialAbility>();
		if (component is SpecialAbility_Siren)
		{
			ParticleSystem[] componentsInChildren = obj.GetComponentsInChildren<ParticleSystem>();
			int num = componentsInChildren.Length;
			for (int i = 0; i < num; i++)
			{
				ParticleSystem.EmissionModule emission = componentsInChildren[i].emission;
				emission.enabled = true;
			}
		}
	}

	private void StopParticleSystems(GameObject obj)
	{
		ParticleSystem[] componentsInChildren = obj.GetComponentsInChildren<ParticleSystem>();
		int num = componentsInChildren.Length;
		for (int i = 0; i < num; i++)
		{
			ParticleSystem.EmissionModule emission = componentsInChildren[i].emission;
			emission.enabled = false;
		}
	}

	private void FindDefaultItem()
	{
		VehiclePart.EUNIQUE_ID eUNIQUE_ID = VehiclePart.EUNIQUE_ID.INVALID;
		MinigameManager.VehicleTemplate currentVehicleTemplate = MinigameManager._pInstance.GetCurrentVehicleTemplate();
		switch (slotType)
		{
		case VehiclePart.EPART_SLOT_TYPE.BODY:
			eUNIQUE_ID = currentVehicleTemplate.bodyPart;
			break;
		case VehiclePart.EPART_SLOT_TYPE.ACCESSORY:
			eUNIQUE_ID = currentVehicleTemplate.accessoryPart;
			break;
		case VehiclePart.EPART_SLOT_TYPE.WHEEL:
			eUNIQUE_ID = currentVehicleTemplate.wheelPart;
			break;
		}
		_defaultIndex = 0;
		int num = _itemProperties.Length;
		for (int i = 0; i < num; i++)
		{
			VehiclePart component = _itemInstances[i].GetComponent<VehiclePart>();
			if (component.uniqueID == eUNIQUE_ID && _itemInstances[i].gameObject.activeInHierarchy)
			{
				_defaultIndex = i;
				return;
			}
		}
		for (int j = 0; j < num; j++)
		{
			if (_itemInstances[j].gameObject.activeInHierarchy)
			{
				_defaultIndex = j;
				break;
			}
		}
	}

	private void SetItemSilhouetted(GameObject go, CarouselBehaviour behaviour)
	{
		behaviour._pIsSilhouetted = true;
		MeshRenderer[] componentsInChildren = go.GetComponentsInChildren<MeshRenderer>();
		int num = componentsInChildren.Length;
		for (int i = 0; i < num; i++)
		{
			componentsInChildren[i].sharedMaterial = blackoutMaterial;
			int num2 = componentsInChildren[i].sharedMaterials.Length;
			if (num2 > 1)
			{
				Debug.Log(componentsInChildren[i].name + " has more than 1 mat");
				Material[] sharedMaterials = componentsInChildren[i].sharedMaterials;
				for (int j = 0; j < num2; j++)
				{
					sharedMaterials[j] = blackoutMaterial;
				}
				componentsInChildren[i].sharedMaterials = sharedMaterials;
			}
			if (componentsInChildren[i].CompareTag("DisableInCarousel"))
			{
				componentsInChildren[i].gameObject.SetActive(false);
			}
		}
		SkinnedMeshRenderer[] componentsInChildren2 = go.GetComponentsInChildren<SkinnedMeshRenderer>();
		int num3 = componentsInChildren2.Length;
		for (int k = 0; k < num3; k++)
		{
			componentsInChildren2[k].sharedMaterial = blackoutMaterial;
		}
		ParticleSystem[] componentsInChildren3 = go.GetComponentsInChildren<ParticleSystem>();
		int num4 = componentsInChildren3.Length;
		for (int l = 0; l < num4; l++)
		{
			componentsInChildren3[l].gameObject.SetActive(false);
		}
	}

	public void SetNewIcons(FloatingNewIconManager iconManager)
	{
		int num = _itemInstances.Length;
		for (int i = 0; i < num; i++)
		{
			if (GlobalInGameData.IsPartNew(_itemInstances[i]._pVehiclePartRef.uniqueID))
			{
				iconManager.MakeNewIconTrackTransform(_itemInstances[i]._pVehiclePartRef);
			}
		}
	}
}
