using System;
using UnityEngine;

public class ScreenWelcome : ScreenBase
{
	[SerializeField]
	private GameObject _welcomeMessage;

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		_welcomeMessage.SetActive(false);
		CameraHUB.OnCinematicIntroAnimationComplete = (Action)Delegate.Combine(CameraHUB.OnCinematicIntroAnimationComplete, new Action(ShowWelcomeMessage));
		CameraHUB._pInstance.PlayCinematicIntroAnimation();
	}

	protected override void OnScreenExitComplete()
	{
		base.OnScreenExitComplete();
	}

	private void ShowWelcomeMessage()
	{
		CameraHUB._pInstance._pCameraControllable = false;
		CameraHUB.OnCinematicIntroAnimationComplete = (Action)Delegate.Remove(CameraHUB.OnCinematicIntroAnimationComplete, new Action(ShowWelcomeMessage));
		_welcomeMessage.SetActive(true);
		if (Facades<TrackingFacade>.Instance != null)
		{
			Facades<TrackingFacade>.Instance.LogMetric("1.Welcome", "FTUE");
			Facades<TrackingFacade>.Instance.LogProgress("FTUE_1_Welcome");
		}
	}

	public void OnOK()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		Navigate("OnOK");
	}

	protected override void Update()
	{
		base.Update();
		if (_welcomeMessage.activeInHierarchy && !Facades<ScreenFacade>.Instance._pIsAnyScreenTweening && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
		{
			OnOK();
		}
	}
}
