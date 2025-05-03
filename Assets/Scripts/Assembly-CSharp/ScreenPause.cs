using GameDefines;
using LEGO;
using LEGO.CoreSDK.Legal;
using UnityEngine;

public class ScreenPause : ScreenBase
{
	private const string CHANNEL_GAME = "Game";

	private const string CHANNEL_GUI = "GUI";

	public Transform backButton;

	public Transform restartButton;

	public Transform soundButton;

	public Transform fullScreenButton;

	public Transform playButton;

	public UISprite fullScreenIcon;

	public UISprite soundIcon;

	public Transform posSoundMobile;

	public Transform posBackMobile;

	public Transform posPlayMobile;

	public Transform posRestartMobile;

	protected override void OnShowScreen()
	{
		Time.timeScale = 0f;
		SoundFacade._pInstance.SetSFXChannelMute("Game", true);
		backButton.transform.position = posBackMobile.position;
		soundButton.transform.position = posSoundMobile.position;
		restartButton.transform.position = posRestartMobile.position;
		playButton.transform.position = posPlayMobile.position;
		fullScreenButton.gameObject.SetActive(false);
		RefreshIcons();
	}

	public void OnAudioMute()
	{
		SoundFacade._pInstance._pMusicMuted = !SoundFacade._pInstance._pMusicMuted;
		SoundFacade._pInstance.SetSFXChannelMute("GUI", SoundFacade._pInstance._pMusicMuted);
		RefreshIcons();
	}

	public void OnFullScreen()
	{
		Screen.fullScreen = !Screen.fullScreen;
		RefreshIcons();
	}

	public void OnReturnToGame()
	{
		ScreenRoot._pInstance.EnableMultiTouch(true);
		SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		Time.timeScale = 1f;
		if (!SoundFacade._pInstance._pMusicMuted)
		{
			SoundFacade._pInstance.SetSFXChannelMute("Game", false);
		}
		Navigate("ReturnToGame");
	}

	public void OnRestart()
	{
		ScreenRoot._pInstance.EnableMultiTouch(true);
		SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		Navigate("Restart");
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

	public void OnHub()
	{
		Navigate("OnExit");
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
