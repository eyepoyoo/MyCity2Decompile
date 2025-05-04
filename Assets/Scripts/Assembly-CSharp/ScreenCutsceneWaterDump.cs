using UnityEngine;

public class ScreenCutsceneWaterDump : ScreenBase
{
	private const float ANIM_DUR = 8.25f;

	private float _startTime;

	protected override void OnShowScreen()
	{
		ScreenBase.LoadEmptyScene();
		SoundFacade._pInstance.PlayOneShotSFX("CutsceneWaterDump", 0f);
		base.OnShowScreen();
		_startTime = RealTime.time;
	}

	protected override void Update()
	{
		base.Update();
		bool flag = false;
		if (Input.anyKeyDown || Input.touchCount > 0)
		{
			flag = true;
		}
		float num = RealTime.time - _startTime;
		if (num > 8.25f || (num > 0.5f && flag))
		{
			MinigameManager.MinigameData currentMinigameData = MinigameManager._pInstance.GetCurrentMinigameData();
			ScreenLoading.LoadMinigame(currentMinigameData.minigameSceneName);
			Navigate("MinigameLoading");
		}
	}
}
