using System;

[Serializable]
public class VehiclePartPropertiesAltPart : VehiclePartPropertiesBase
{
	public static implicit operator VehiclePartProperties(VehiclePartPropertiesAltPart m)
	{
		VehiclePartProperties vehiclePartProperties = new VehiclePartProperties();
		vehiclePartProperties._resourceString = m._resourceString;
		vehiclePartProperties._absoluteString = m._resourceString;
		vehiclePartProperties.uniqueID = m.uniqueID;
		vehiclePartProperties.slotType = m.slotType;
		vehiclePartProperties.connectionType = m.connectionType;
		vehiclePartProperties.vehicleTypeCategories = m.vehicleTypeCategories;
		vehiclePartProperties.carouselHeightOffset = m.carouselHeightOffset;
		vehiclePartProperties.unlockWeight = m.unlockWeight;
		vehiclePartProperties.minimumLevelToUnlock = m.minimumLevelToUnlock;
		vehiclePartProperties.brickBagRewardScale = m.brickBagRewardScale;
		vehiclePartProperties.carouselNormalScale = m.carouselNormalScale;
		vehiclePartProperties.carouselSelectedScale = m.carouselSelectedScale;
		vehiclePartProperties.carouselPreviewScale = m.carouselPreviewScale;
		vehiclePartProperties.completedBuildScale = m.completedBuildScale;
		vehiclePartProperties._completedCameraZoomFactor = m._completedCameraZoomFactor;
		vehiclePartProperties.carouselInfoPanelCameraPos = m.carouselInfoPanelCameraPos;
		vehiclePartProperties.partIsAvailable = m.partIsAvailable;
		vehiclePartProperties.localisationKey = m.localisationKey;
		vehiclePartProperties.partsThatAppearInSecondCarouselPart = m.partsThatAppearInSecondCarouselPart;
		vehiclePartProperties.partsThatAppaerInThirdCarouselPart = m.partsThatAppaerInThirdCarouselPart;
		vehiclePartProperties._negatedObstacles = m._negatedObstacles;
		vehiclePartProperties.turbulenceToleranceOverride = m.turbulenceToleranceOverride;
		vehiclePartProperties.secondaryFlippedPart = m.secondaryFlippedPart;
		vehiclePartProperties._minigameCamDistScale = m._minigameCamDistScale;
		vehiclePartProperties._animateWhenAttached = m._animateWhenAttached;
		vehiclePartProperties._soundToPlayOnAttached = m._soundToPlayOnAttached;
		return vehiclePartProperties;
	}
}
