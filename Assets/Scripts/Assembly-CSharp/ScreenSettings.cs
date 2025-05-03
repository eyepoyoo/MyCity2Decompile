using GameDefines;
using LEGO;
using LEGO.CoreSDK.Legal;
using UnityEngine;

public class ScreenSettings : ScreenBase
{
	private const string CHANNEL_GAME = "Game";

	private const string CHANNEL_GUI = "GUI";

	public Transform backButton;

	public Transform soundButton;

	public Transform fullScreenButton;

	public Transform deleteSaveButton;

	public UISprite fullScreenIcon;

	public UISprite soundIcon;

	public Transform posDeleteSaveMobile;

	public Transform posSoundMobile;

	public Transform posBackMobile;

	public UILabel versionNumber;

	private UIButton _deleteButton;

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		if (_deleteButton == null)
		{
			_deleteButton = deleteSaveButton.GetComponent<UIButton>();
		}
		if (Facades<FlowFacade>.Instance.CurrentLocation == "HubSettings")
		{
			CameraHUB._pInstance._pCameraControllable = false;
		}
		versionNumber.text = "v" + AnvilBuildInfo._pBambooVersion;
		backButton.transform.position = posBackMobile.position;
		soundButton.transform.position = posSoundMobile.position;
		deleteSaveButton.transform.position = posDeleteSaveMobile.position;
		fullScreenButton.gameObject.SetActive(false);
		if (_deleteButton != null)
		{
			_deleteButton.isEnabled = GlobalInGameData._pHasCompletedFTUE;
		}
		RefreshIcons();
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish", 0.25f);
		SoundFacade._pInstance.PlayOneShotSFX("GUIPop", 0f);
		SoundFacade._pInstance.PlayOneShotSFX("GUIPop", 0.25f);
		SoundFacade._pInstance.PlayOneShotSFX("GUIPop", 0.5f);
	}

	public void OnExitSettings()
	{
		if (Facades<FlowFacade>.Instance.CurrentLocation == "HubSettings")
		{
			CameraHUB._pInstance._pCameraControllable = true;
		}
		SoundFacade._pInstance.PlayOneShotSFX("GUIBack", 0f);
		Navigate("ExitSettings");
	}

	public void OnAudioMute()
	{
		SoundFacade._pInstance._pMusicMuted = !SoundFacade._pInstance._pMusicMuted;
		SoundFacade._pInstance.SetSFXChannelMute("Game", SoundFacade._pInstance._pMusicMuted);
		SoundFacade._pInstance.SetSFXChannelMute("GUI", SoundFacade._pInstance._pMusicMuted);
		RefreshIcons();
	}

	public void OnFullScreen()
	{
		Screen.fullScreen = !Screen.fullScreen;
		RefreshIcons();
	}

	public void OnDeleteSave()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		Navigate("DeleteSave");
	}

	public void OnTerms()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		if (GlobalDefines._pUseLegoSDK)
		{
			LEGOSDK.Policies.Show(LEGOSDKAmuzo.OnPoliciesShown, PolicyType.Terms);
		}
		else
		{
			Navigate("Terms");
		}
	}

	public void OnCookies()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		if (GlobalDefines._pUseLegoSDK)
		{
			LEGOSDK.Policies.Show(LEGOSDKAmuzo.OnPoliciesShown, PolicyType.Cookie);
			return;
		}
		string url = string.Empty;
		if (GlobalDefines._pUrlCookies != null && GlobalDefines._pUrlCookies != string.Empty)
		{
			url = GlobalDefines._pUrlCookies;
		}
		Application.OpenURL(url);
	}

	public void OnPrivacy()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		if (GlobalDefines._pUseLegoSDK)
		{
			LEGOSDK.Policies.Show(LEGOSDKAmuzo.OnPoliciesShown, PolicyType.Privacy);
			return;
		}
		string url = string.Empty;
		if (GlobalDefines._pUrlPrivacy != null && GlobalDefines._pUrlPrivacy != string.Empty)
		{
			url = GlobalDefines._pUrlPrivacy;
		}
		Application.OpenURL(url);
	}

	private void RefreshIcons()
	{
		Color color = soundIcon.color;
		color.r = ((!SoundFacade._pInstance._pMusicMuted) ? 1 : 0);
		soundIcon.color = color;
		Color color2 = fullScreenIcon.color;
		color2.r = ((!Screen.fullScreen) ? 1 : 0);
		fullScreenIcon.color = color2;
	}
}
