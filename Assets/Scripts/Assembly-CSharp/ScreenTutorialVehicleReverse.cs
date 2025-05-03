using UnityEngine;

public class ScreenTutorialVehicleReverse : ScreenBase
{
	public UIPanel backingPanel;

	public GameObject mobileComponents;

	public GameObject webComponents;

	public static bool _pHasShown { get; set; }

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		Time.timeScale = 0f;
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish", 0f);
		SoundFacade._pInstance.PlayOneShotSFX("GUIPop", 0f);
		Debug.Log("ScreenTutorialVehicleReverse");
		mobileComponents.SetActive(true);
		webComponents.SetActive(false);
		_pHasShown = true;
	}

	protected override void OnScreenExitComplete()
	{
		base.OnScreenExitComplete();
		Time.timeScale = 1f;
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
