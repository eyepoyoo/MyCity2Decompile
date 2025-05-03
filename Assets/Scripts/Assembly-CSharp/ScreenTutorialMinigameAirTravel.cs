using UnityEngine;

public class ScreenTutorialMinigameAirTravel : ScreenBase
{
	public UIPanel backingPanel;

	private bool _goingToControlsTutorial;

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		Time.timeScale = 0f;
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish", 0f);
		SoundFacade._pInstance.PlayOneShotSFX("GUIPop", 0f);
		Debug.Log("ScreenTutorialCrookRoundup");
	}

	protected override void OnScreenExitComplete()
	{
		base.OnScreenExitComplete();
		if (!_goingToControlsTutorial)
		{
			Time.timeScale = 1f;
		}
	}

	public void OnOK()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish", 0.1f);
		SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		Navigate("OnOK");
	}

	protected override void Update()
	{
		base.Update();
		if (!Facades<ScreenFacade>.Instance._pIsAnyScreenTweening && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
		{
			OnOK();
		}
		backingPanel.SetDirty();
	}
}
