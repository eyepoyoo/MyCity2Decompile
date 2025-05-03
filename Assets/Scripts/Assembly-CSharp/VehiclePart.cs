using System;
using UnityEngine;

public class VehiclePart : MonoBehaviour
{
	public enum EUNIQUE_ID
	{
		INVALID = 0,
		BODY_FIRE_ENGINE = 1,
		BODY_POLICE_BUGGY = 2,
		BODY_POLICE_CAR = 3,
		BODY_PORTALOO = 4,
		BODY_4X4 = 5,
		ACCESSORY_WATER_JET = 6,
		DEPRECATED__ACCESSORY_HOVERCRAFT_PROPELLER_SIMPLE = 7,
		ACCSESORY_MEDIUM_ROTOR_BLADES = 8,
		WHEEL_NORMAL = 9,
		WHEEL_SPEED = 10,
		WHEEL_LARGE = 11,
		ACCESSORY_POLICE_SIREN = 12,
		ACCSESORY_BUGGY_SIREN = 13,
		BODY_LARGE_POLICE_BOAT = 14,
		ATTACHMENT_LARGE_POLICE_BOAT_BRIDGE = 15,
		ACCESSORY_SPOTLIGHT = 16,
		BODY_FLOATING_TYRE = 17,
		BODY_POLICE_JETSKI = 18,
		ATTACHMENT_POLICE_JETSKI_ENGINE = 19,
		ACCESSORY_SIREN_MAST = 20,
		BODY_POLICE_HELICOPTER = 21,
		BODY_SMALL_POLICE_HELICOPTER = 22,
		BODY_HOT_AIR_BALLOON_BASKET = 23,
		ATTACHMENT_MEDIUM_ROTOR_BLADE = 24,
		ATTACHMENT_SMALL_ROTOR_BLADE = 25,
		ATTACHMENT_BALLOON = 26,
		ACCESSORY_SEARCH_LIGHTS = 27,
		BODY_FIRE_QUAD_BIKE = 28,
		BODY_HOT_DOG = 29,
		ACCESSORY_FIRE_ENGINE_LADDER = 30,
		ACCESSORY_QUAD_SIREN = 31,
		ACCESSORY_PARASOL = 32,
		WHEEL_SMALL = 33,
		BODY_LARGE_FIRE_BOAT = 34,
		BODY_SMALL_FIRE_HOVERCRAFT = 35,
		BODY_SMALL_RIB = 36,
		BODY_EXPLORER_HELICOPTER = 37,
		BODY_EXPLORER_CHINOOK = 38,
		BODY_EXPLORER_UAV = 39,
		BODY_EXPLORER_TRUCK = 40,
		BODY_EXPLORER_4X4 = 41,
		BODY_EXPLORER_LIFTER = 42,
		BODY_MINI_DIGGER = 43,
		BODY_MINI_TRACTOR = 44,
		BODY_LARGE_FIRE_HELICOPTER = 45,
		BODY_SMALL_FIRE_HELICOPTER = 46,
		ATTACHMENT_LARGE_ROTOR_BLADE = 47,
		ATTACHMENT_DOUBLE_LARGE_ROTOR_BLADE = 48,
		ATTACHMENT_SMALL_OUTBOARD_MOTOR = 49,
		ATTACHMENT_LARGE_FIRE_BOAT_BRIDGE = 50,
		ACCESSORY_HOVERCRAFT_PROPELLER = 51,
		ATTACHMENT_MINI_ROTOR_BLADE = 52,
		WHEEL_MINI_EXPLORER_TRACKS = 53,
		WHEEL_EXPLORER_TRACKS = 54,
		WHEEL_BIG_EXPLORER_TRACKS = 55,
		ACCSESORY_EXPLORER_ARM = 56,
		ACCESSORY_REAR_DIGGER = 57,
		ACCESSORY_LIFTER = 58,
		ACCESSORY_MINI_DIGGER_ARM = 59,
		ACCESSORY_LARGE_DIGGER_ARM = 60,
		ACCESSORY_LIFTER_ARM = 61,
		ACCESSORY_LARGE_HELICOPTER_ENGINE = 62,
		ACCESSORY_WATER_CANNON = 63,
		ACCESSORY_HELICOPTER_WINCH = 64,
		ACCESSORY_WATER_CANNON_ARM = 65,
		ACCESSORY_MINI_SIREN = 66,
		ACCESSORY_MINI_TAIL_ROTOR = 67,
		WHEEL_OFFROAD = 68,
		BODY_EXPLORER_BIG_DIGGER = 69,
		ATTACHMENT_LARGE_ROTOR_BLADE_EXPLORER = 70,
		BODY_PLANE = 71,
		ATTACHMENT_PLANE_WINGS = 72,
		ACCESSORY_PLANE_ENGINES = 73,
		BODY_SIGN_BUGGY = 74,
		WHEEL_MINI = 75,
		ACCESSORY_WARNING_LIGHT = 76,
		BODY_BIPLANE = 77,
		ATTACHMENT_BIPLANE_WINGS = 78,
		ACCSESSORY_STUNT_BAR = 79,
		BODY_JETPLANE = 80,
		ATTACHMENT_JET_WINGS = 81,
		ACCESSORY_JET_ENGINES = 82,
		BODY_LUGGAGE_CARRIER = 83,
		WHEEL_LUGGAGE_CARRIER = 84,
		BODY_STRETCH_LIMO = 85,
		BODY_SHARK = 86,
		ACCESSORY_HOSE_SIMPLE_SKY = 87,
		ACCESSORY_LIFTER_SIMPLE = 88,
		ACCESSORY_HOSE_SIMPLE_GROUND = 89,
		NUM_PARTS = 90
	}

	public enum EPART_SLOT_TYPE
	{
		INVALID = 0,
		BODY = 1,
		WHEEL = 2,
		ACCESSORY = 3,
		ATTACHMENT = 4
	}

	public enum EObstacleToNegate
	{
		MudThrough = 0,
		Under = 1,
		Over = 2,
		Boxes = 3,
		Ramp = 4,
		Traffic = 5,
		Hose = 6,
		Turbulence = 7,
		Spotlight = 8,
		LavaThrough = 9
	}

	[Serializable]
	public class AltPart
	{
		public Vehicle.EType _type;

		public VehiclePart _part;
	}

	public EUNIQUE_ID uniqueID;

	public EPART_SLOT_TYPE slotType;

	public Connectors.ECONNECTOR_TYPE connectionType;

	public MinigameManager.EVEHICLE_TYPE[] vehicleTypeCategories;

	public Transform centralPoint;

	public float carouselHeightOffset;

	public int unlockWeight = 1;

	public int minimumLevelToUnlock;

	public Vector3 brickBagRewardScale = Vector3.one;

	public Vector3 carouselNormalScale = Vector3.one;

	public Vector3 carouselSelectedScale = Vector3.one;

	public Vector3 carouselPreviewScale = Vector3.one;

	public Vector3 completedBuildScale = Vector3.one;

	public float _completedCameraZoomFactor = 1f;

	public Vector3 carouselInfoPanelCameraPos;

	public bool partIsAvailable = true;

	public string localisationKey;

	public EUNIQUE_ID[] partsThatAppearInSecondCarouselPart;

	public EUNIQUE_ID[] partsThatAppaerInThirdCarouselPart;

	public EObstacleToNegate[] _negatedObstacles;

	public float turbulenceToleranceOverride = -1f;

	public GameObject secondaryFlippedPart;

	public AltPart[] _altParts;

	public float _minigameCamDistScale = 1f;

	public bool _animateWhenAttached;

	public string _soundToPlayOnAttached;

	public Transform faceLocation;

	private Vector3 _secondaryFlippedPartRestingPos;

	public bool _pHasBeenUnlocked
	{
		get
		{
			return GlobalInGameData.HasPartBeenUnlocked(uniqueID);
		}
	}

	public Vector3 _pCentralPoint
	{
		get
		{
			if (centralPoint != null)
			{
				return centralPoint.position;
			}
			return base.transform.position;
		}
	}

	public Transform _pFaceTransform
	{
		get
		{
			return faceLocation ?? base.transform;
		}
	}

	private void Awake()
	{
		if (secondaryFlippedPart != null)
		{
			_secondaryFlippedPartRestingPos = secondaryFlippedPart.transform.localPosition;
			secondaryFlippedPart.SetActive(false);
		}
		Vehicle componentInParent = GetComponentInParent<Vehicle>();
		if ((bool)componentInParent && componentInParent.transform != base.transform)
		{
			AttachToVehicle(componentInParent);
		}
	}

	public void OnReturnItem()
	{
		if (secondaryFlippedPart != null)
		{
			secondaryFlippedPart.SetActive(false);
		}
	}

	public bool IsVehicleType(MinigameManager.EVEHICLE_TYPE vt)
	{
		if (vehicleTypeCategories == null)
		{
			return false;
		}
		int num = vehicleTypeCategories.Length;
		for (int i = 0; i < num; i++)
		{
			if (vehicleTypeCategories[i] == vt)
			{
				return true;
			}
		}
		return false;
	}

	public void AttachToVehicle(Vehicle vehicle)
	{
		vehicle._pConnectors.Attach(this);
		vehicle.OnPartAttached(this);
	}

	public void AttachToVehicle_Animate(Vehicle vehicle, float animationDelay = 0f, Action<VehiclePart> onAnimComplete = null)
	{
		vehicle._pConnectors.Attach(this);
		vehicle._pConnectors.Animate(this, animationDelay, delegate
		{
			vehicle.OnPartAttached(this);
			if (onAnimComplete != null)
			{
				onAnimComplete(this);
			}
		});
	}

	public VehiclePart GetAltPartOrThis(Vehicle.EType type)
	{
		if (_altParts != null)
		{
			for (int num = _altParts.Length - 1; num >= 0; num--)
			{
				if (_altParts[num]._type == type)
				{
					return _altParts[num]._part;
				}
			}
		}
		return this;
	}
}
