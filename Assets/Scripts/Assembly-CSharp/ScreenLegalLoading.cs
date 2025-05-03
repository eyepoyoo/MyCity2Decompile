using GameDefines;
using UnityEngine;

public class ScreenLegalLoading : ScreenBase
{
	public UIWidget _background;

	private float _screenShownTime;

	private float _minWaitTime = 3f;

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		_screenShownTime = RealTime.time;
	}

	protected override void Update()
	{
		base.Update();
		_background.panel.ResetAnchors();
		if (RealTime.time > _screenShownTime + _minWaitTime && Facades<FlowFacade>.Instance != null && InitialisationFacade.Instance != null && InitialisationFacade.Instance._pHasFinished)
		{
			if (!Application.isEditor && (GlobalDefines._killSwitchRaised || GlobalDefines._minVersionWrong))
			{
				Debug.Log("Navgating to Killswitch.");
				Navigate("Killswitch");
			}
			else
			{
				Debug.Log("Navgating to Title.");
				Navigate("Title");
			}
		}
	}
}
