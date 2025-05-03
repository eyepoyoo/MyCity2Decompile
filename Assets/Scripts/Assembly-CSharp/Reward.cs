using System;

[Serializable]
public class Reward
{
	public enum RewardType
	{
		CHEST = 0,
		VEHICLE_PIECE = 1,
		BRICKS = 2
	}

	public RewardType _rewardType;

	public int _brickReward;

	public void givePlayerReward()
	{
		if (_rewardType == RewardType.BRICKS || !isRewardAvalible())
		{
			ScenarioManager._pInstance.AddBrickReward(_brickReward);
			GlobalInGameData._pCumulativeStuds += _brickReward * 10;
		}
		else if (_rewardType == RewardType.CHEST)
		{
			GlobalInGameData._pUnclaimedDailyRewardChests++;
		}
	}

	public bool isRewardAvalible()
	{
		if (_rewardType == RewardType.BRICKS || _rewardType == RewardType.CHEST)
		{
			return true;
		}
		return true;
	}
}
