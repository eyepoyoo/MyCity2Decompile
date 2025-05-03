using UnityEngine;

public class ScreenTestVehicle : ScreenBase
{
	public static void LoadTestScene()
	{
		GlobalInGameData.OnLevelWillLoad("VehicleTester", ScreenLoading._pCurrentLevelName);
		Application.LoadLevel("VehicleTester");
	}

	public void OnBack()
	{
		Navigate("CustomiseVehicle");
	}
}
