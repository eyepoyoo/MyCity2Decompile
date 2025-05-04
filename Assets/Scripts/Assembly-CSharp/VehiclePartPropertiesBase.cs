using System;
using UnityEngine;

[Serializable]
public class VehiclePartPropertiesBase
{
	public string _resourceString;

	public string _absoluteString;

	public VehiclePart.EUNIQUE_ID uniqueID;

	public VehiclePart.EPART_SLOT_TYPE slotType;

	public Connectors.ECONNECTOR_TYPE connectionType;

	public MinigameManager.EVEHICLE_TYPE[] vehicleTypeCategories;

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

	public VehiclePart.EUNIQUE_ID[] partsThatAppearInSecondCarouselPart;

	public VehiclePart.EUNIQUE_ID[] partsThatAppaerInThirdCarouselPart;

	public VehiclePart.EObstacleToNegate[] _negatedObstacles;

	public float turbulenceToleranceOverride = -1f;

	public GameObject secondaryFlippedPart;

	public float _minigameCamDistScale = 1f;

	public bool _animateWhenAttached;

	public string _soundToPlayOnAttached;
}
