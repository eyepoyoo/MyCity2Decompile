using GameDefines;
using UnityEngine;

public class AmuzoDebugMenu_SystemInfo : AmuzoDebugMenu_TwoInfoColumns
{
	public AmuzoDebugMenu_SystemInfo(string menuName)
		: base(menuName)
	{
		AddInfoTextFunction(GetLeftDebugText, GetRightDebugText, false);
	}

	private string GetLeftDebugText()
	{
		return string.Concat("DeviceModel: ", SystemInfo.deviceModel, AmuzoDebugMenu.NEW_LINE, "DeviceName: ", SystemInfo.deviceName, AmuzoDebugMenu.NEW_LINE, "DeviceType: ", SystemInfo.deviceType, AmuzoDebugMenu.NEW_LINE, "AmuzoDeviceUniqueIdentifier: ", GlobalDefines._pAmuzoDeviceUniqueId, AmuzoDebugMenu.NEW_LINE, "MaxTextureSize: ", SystemInfo.maxTextureSize, AmuzoDebugMenu.NEW_LINE, "NpotSupport: ", SystemInfo.npotSupport, AmuzoDebugMenu.NEW_LINE, "OperatingSystem: ", SystemInfo.operatingSystem, AmuzoDebugMenu.NEW_LINE, "ProcessorCount: ", SystemInfo.processorCount, AmuzoDebugMenu.NEW_LINE, "ProcessorType: ", SystemInfo.processorType, AmuzoDebugMenu.NEW_LINE, "MaxTextureSize: ", SystemInfo.maxTextureSize, AmuzoDebugMenu.NEW_LINE, "SystemMemorySize: ", SystemInfo.systemMemorySize, AmuzoDebugMenu.NEW_LINE, "SupportedRenderTargetCount: ", SystemInfo.supportedRenderTargetCount, AmuzoDebugMenu.NEW_LINE, "Supports3DTextures: ", SystemInfo.supports3DTextures, AmuzoDebugMenu.NEW_LINE, "SupportsAccelerometer: ", SystemInfo.supportsAccelerometer, AmuzoDebugMenu.NEW_LINE, "SupportsComputeShaders: ", SystemInfo.supportsComputeShaders, AmuzoDebugMenu.NEW_LINE, "SupportsGyroscope: ", SystemInfo.supportsGyroscope, AmuzoDebugMenu.NEW_LINE);
	}

	private string GetRightDebugText()
	{
		return string.Concat("SupportsImageEffects: ", SystemInfo.supportsImageEffects, AmuzoDebugMenu.NEW_LINE, "SupportsLocationService: ", SystemInfo.supportsLocationService, AmuzoDebugMenu.NEW_LINE, "SupportsRenderTextures: ", SystemInfo.supportsRenderTextures, AmuzoDebugMenu.NEW_LINE, "SupportsRenderToCubemap: ", SystemInfo.supportsRenderToCubemap, AmuzoDebugMenu.NEW_LINE, "SupportsShadows: ", SystemInfo.supportsShadows, AmuzoDebugMenu.NEW_LINE, "SupportsSparseTextures: ", SystemInfo.supportsSparseTextures, AmuzoDebugMenu.NEW_LINE, "SupportsVibration: ", SystemInfo.supportsVibration, AmuzoDebugMenu.NEW_LINE, "GraphicsDeviceID: ", SystemInfo.graphicsDeviceID, AmuzoDebugMenu.NEW_LINE, "GraphicsDeviceName: ", SystemInfo.graphicsDeviceName, AmuzoDebugMenu.NEW_LINE, "GraphicsDeviceType: ", SystemInfo.graphicsDeviceType, AmuzoDebugMenu.NEW_LINE, "GraphicsDeviceVendor: ", SystemInfo.graphicsDeviceVendor, AmuzoDebugMenu.NEW_LINE, "GraphicsDeviceVendorID: ", SystemInfo.graphicsDeviceVendorID, AmuzoDebugMenu.NEW_LINE, "GraphicsDeviceVersion: ", SystemInfo.graphicsDeviceVersion, AmuzoDebugMenu.NEW_LINE, "GraphicsMemorySize: ", SystemInfo.graphicsMemorySize, AmuzoDebugMenu.NEW_LINE, "GraphicsMultiThreaded: ", SystemInfo.graphicsMultiThreaded, AmuzoDebugMenu.NEW_LINE, "GraphicsShaderLevel: ", SystemInfo.graphicsShaderLevel, AmuzoDebugMenu.NEW_LINE);
	}
}
