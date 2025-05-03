using GameDefines;
using UnityEngine;

public class ScreenTitle : ScreenBase
{
	[SerializeField]
	private ParallaxBackground _parallax;

	[SerializeField]
	private UIButton _legoIDButton;

	[SerializeField]
	private UIButton _leaderboardButton;

	private bool _hasPressedStart;

	private float _startTime;

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		SoundFacade._pInstance.PlayMusic("FrontEnd", 0f);
		GlobalInGameData.AssignCallbacks();
		int num = QuestHandler._pInstance.LoadSaveData();
		if (num > 0)
		{
			ScreenQuests._numNewQuests = Mathf.Max(num, ScreenQuests._numNewQuests);
		}
		UpdateLEGOIDButton();
		_parallax.SetDefaultOrientation();
		_startTime = Time.time;
		_hasPressedStart = false;
	}

	protected override void OnScreenShowComplete()
	{
		base.OnScreenShowComplete();
		if (!Application.isEditor && (GlobalDefines._killSwitchRaised || GlobalDefines._minVersionWrong))
		{
			OnKillswitch();
		}
	}

	protected override void Update()
	{
		base.Update();
		UpdateLEGOIDButton();
	}

	private void UpdateLEGOIDButton()
	{
		if (_leaderboardButton.gameObject.activeInHierarchy)
		{
			Debug.Log("Disabled Leaderboard button because we are offline.");
			_leaderboardButton.gameObject.SetActive(false);
		}
		if (_legoIDButton.gameObject.activeInHierarchy)
		{
			Debug.Log("Disabled LEGOID button because we are offline.");
			_legoIDButton.gameObject.SetActive(false);
		}
	}

	public void OnPressStart()
	{
		if (!(Time.time - _startTime < 0.5f) && base._pCurrentTweenType == ScreenTweenType.Idle && !_hasPressedStart)
		{
			_hasPressedStart = true;
			SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
			CityManager.DID_COME_FROM_TITLE = true;
			ScreenHub.LoadDefaultHUB();
			Navigate("LoadHub");
		}
	}

	public void OnTrySignIn()
	{
		LEGOID._pInstance.Login(OnLogin);
	}

	public void OnLogin(LEGOID.ELoginStatus status)
	{
		if (Facades<TrackingFacade>.Instance != null && status == LEGOID.ELoginStatus.LOGIN_SUCCESS)
		{
			Facades<TrackingFacade>.Instance.LogMetric("Title", "LEGOID");
			Facades<TrackingFacade>.Instance.LogEvent("Login_From_Title");
		}
	}

	public void OnLeaderboardPressed()
	{
		Navigate("TitleLeaderboard");
	}

	public void OnKillswitch()
	{
		Navigate("Killswitch");
	}
}
