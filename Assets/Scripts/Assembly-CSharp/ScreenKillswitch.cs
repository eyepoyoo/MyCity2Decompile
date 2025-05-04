using GameDefines;
using UnityEngine;

public class ScreenKillswitch : ScreenBase
{
	public UILabel infoLabel;

	private float _showTime;

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		Debug.Log("Killswitch raised - VerWrong:" + GlobalDefines._minVersionWrong + " KS: " + GlobalDefines._killSwitchRaised);
		_showTime = RealTime.time;
		infoLabel.text = GlobalDefines._pExitAppMessage;
	}

	public void OnExit()
	{
		Application.Quit();
	}

	protected override void Update()
	{
		base.Update();
		if (RealTime.time - _showTime > 1f && (Input.GetMouseButtonUp(0) || Input.touchCount > 0))
		{
			Application.Quit();
		}
	}
}
