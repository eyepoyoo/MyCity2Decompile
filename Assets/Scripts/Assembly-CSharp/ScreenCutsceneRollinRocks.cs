using UnityEngine;

public class ScreenCutsceneRollinRocks : ScreenBase
{
	private const float ANIM_DUR = 8.5f;

	private float _startTime;

	private AudioSource _src;

	protected override void OnShowScreen()
	{
		ScreenBase.LoadEmptyScene();
		_src = SoundFacade._pInstance.PlayOneShotSFX("CutsceneRollinRocks", 0f);
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
			if (_src != null && _src.isPlaying)
			{
				_src.Stop();
			}
		}
		float num = RealTime.time - _startTime;
		if (num > 8.5f || (num > 0.5f && flag))
		{
			MinigameManager.MinigameData currentMinigameData = MinigameManager._pInstance.GetCurrentMinigameData();
			ScreenLoading.LoadMinigame(currentMinigameData.minigameSceneName);
			Navigate("MinigameLoading");
		}
	}
}
