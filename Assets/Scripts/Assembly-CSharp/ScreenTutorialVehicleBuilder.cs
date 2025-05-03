using System;
using UnityEngine;

public class ScreenTutorialVehicleBuilder : ScreenBase
{
	public UIPanel backingPanel;

	public static ScreenTutorialVehicleBuilder _pInstance { get; private set; }

	public event Action<ScreenTutorialVehicleBuilder> _onOk;

	private void Awake()
	{
		_pInstance = this;
	}

	protected override void OnShowScreen()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish", 0f);
		SoundFacade._pInstance.PlayOneShotSFX("GUIPop", 0.25f);
		base.OnShowScreen();
	}

	protected override void Update()
	{
		base.Update();
		if (!Facades<ScreenFacade>.Instance._pIsAnyScreenTweening && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
		{
			OnOK();
		}
		if (Facades<ScreenFacade>.Instance._pIsAnyScreenTweening)
		{
			backingPanel.SetDirty();
		}
	}

	public void OnOK()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish", 0.1f);
		SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		Navigate("OnOK");
		if (this._onOk != null)
		{
			this._onOk(this);
			this._onOk = null;
		}
	}
}
