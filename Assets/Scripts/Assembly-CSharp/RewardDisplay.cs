using UnityEngine;

public class RewardDisplay : MonoBehaviour
{
	public enum RewardState
	{
		UNLOCKED = 0,
		FOCUSED = 1,
		LOCKED = 2
	}

	[SerializeField]
	private Color _panelNotFocusedColour;

	[SerializeField]
	private Color _panelFocusedColour;

	[SerializeField]
	private Color _circleNotFocusedColour;

	[SerializeField]
	private Color _circleFocusedColour;

	[SerializeField]
	private UIWidget _displayContainer;

	[SerializeField]
	private UILabel _dayLabel;

	[SerializeField]
	private UIWidget _circleContainer;

	[SerializeField]
	private UISprite _chestSprite;

	[SerializeField]
	private UITexture _brickTexture;

	[SerializeField]
	private UILabel _brickCountLabel;

	[SerializeField]
	private UISprite _tickSprite;

	[SerializeField]
	private UISprite _padlockSprite;

	[SerializeField]
	private UIWidget _acceptButton;

	public void showReward(DailyRewardsManager.DailyReward rewardToShow, RewardState displayState = RewardState.UNLOCKED)
	{
		Reward reward = rewardToShow._rewards[0];
		_chestSprite.gameObject.SetActive(reward._rewardType == Reward.RewardType.CHEST);
		_brickTexture.gameObject.SetActive(reward._rewardType != Reward.RewardType.CHEST);
		_brickTexture.material = ScenarioManager._pInstance._pCurrentScenario.brickMaterial;
		_brickCountLabel.text = ((reward._rewardType != Reward.RewardType.BRICKS) ? 1 : reward._brickReward).ToString();
		_displayContainer.color = ((displayState != RewardState.FOCUSED) ? _panelNotFocusedColour : _panelFocusedColour);
		_circleContainer.color = ((displayState != RewardState.FOCUSED) ? _circleNotFocusedColour : _circleFocusedColour);
		_tickSprite.gameObject.SetActive(displayState == RewardState.UNLOCKED);
		_padlockSprite.gameObject.SetActive(displayState == RewardState.LOCKED);
		_acceptButton.gameObject.SetActive(displayState == RewardState.FOCUSED);
	}
}
