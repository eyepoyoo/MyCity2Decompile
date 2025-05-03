using UnityEngine;

public class ScreenSocialHub : ScreenBase
{
	public ScreenObjectTransitionWidget fadeWidget;

	public bool _returningToHub;

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		_returningToHub = false;
		fadeWidget.SetStateConsideredTweenedIn(false);
		fadeWidget.SetToTweenInStartColor();
		CityManager._pInstance.RequestSetupCityFromOtherPlayer(GlobalInGameData._pCurrentSocialCity, OnCityLoadComplete);
		CityManager._pInstance.minigameMarkerRoot.SetActive(false);
	}

	protected override void OnScreenExitComplete()
	{
		base.OnScreenExitComplete();
		if (_returningToHub)
		{
			CityManager._pInstance.RefreshLocalCity();
		}
	}

	private void OnCityLoadComplete()
	{
		fadeWidget.TweenIn();
		CameraHUB._pInstance._pCameraControllable = true;
	}

	public void OnBack()
	{
		_returningToHub = true;
		ScreenHub._pInstance.RequestFadeIn();
		Navigate("Hub");
		fadeWidget.TweenOut();
	}

	public void OnSettings()
	{
		Navigate("Settings");
	}

	public void OnSocialMinigame(GameObject obj)
	{
		string text = obj.name;
		Debug.Log("Going to Social Minigame:" + text);
		Navigate("MinigameInfo");
	}
}
