using System;

public class ScreenTourOfTheCity : ScreenBase
{
	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		CameraHUB.OnTourOfTheCityAnimationComplete = (Action)Delegate.Combine(CameraHUB.OnTourOfTheCityAnimationComplete, new Action(OnFinish));
		CameraHUB._pInstance.PlayTourOfTheCityAnimation();
	}

	private void OnFinish()
	{
		CameraHUB._pInstance.StopAnimating();
		CameraHUB.OnTourOfTheCityAnimationComplete = (Action)Delegate.Remove(CameraHUB.OnTourOfTheCityAnimationComplete, new Action(OnFinish));
		Navigate("Next");
	}

	protected override void Update()
	{
		base.Update();
	}
}
