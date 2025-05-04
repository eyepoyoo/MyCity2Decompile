public class ScreenDailyRewards : ScreenBase
{
	public RewardDisplay[] rewardDisplays;

	public UIPanel backingPanel;

	public UILabel description;

	public UILabel minigameName;

	private bool _screenFlowIsForward = true;

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		int i = 0;
		for (int num = rewardDisplays.Length; i < num; i++)
		{
			if (DailyRewardsManager.Instance._dailyRewards.Length <= i)
			{
				rewardDisplays[i].gameObject.SetActive(false);
				continue;
			}
			rewardDisplays[i].gameObject.SetActive(true);
			rewardDisplays[i].showReward(DailyRewardsManager.Instance._dailyRewards[i], (i >= DailyRewardsManager.Instance.LastRewardIndex) ? ((i == DailyRewardsManager.Instance.LastRewardIndex) ? RewardDisplay.RewardState.FOCUSED : RewardDisplay.RewardState.LOCKED) : RewardDisplay.RewardState.UNLOCKED);
		}
	}

	protected override void Update()
	{
		base.Update();
		if (Facades<ScreenFacade>.Instance._pIsAnyScreenTweening)
		{
			backingPanel.SetDirty();
		}
	}

	public void SetTransitionReverse()
	{
		_screenFlowIsForward = false;
	}

	public void SetTransitionForward()
	{
		_screenFlowIsForward = true;
	}

	public void OnBack()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		DailyRewardsManager.Instance.LastReward.rewardAchieved();
		ScreenHub._pInstance.RefreshAfterRewards();
		SetTransitionReverse();
		TryChangeWidgetSets(base.gameObject, "Reverse");
		Navigate("Hub");
	}
}
