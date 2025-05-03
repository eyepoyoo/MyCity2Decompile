using System.Collections.Generic;
using UnityEngine;

public class ScreenProgressInfo : ScreenBase
{
	public UILabel currentLevelLarge;

	public UILabel currentLevelSmall;

	public UILabel nextLevel;

	public Material[] brickRewardMaterials;

	public Transform expBarCurrent;

	public Transform expBarStart;

	public Transform expBarEnd;

	public Color ringNormalColor;

	public Color ringUnavailableColor;

	public Color textNormalColor;

	public Color textUnavailableColor;

	public Color numberNormalColor;

	public Color numberUnavailableColor;

	public Color shadowNormalColor;

	public Color shadowUnavailableColor;

	public UIWidget draggableElement;

	public ProgressionInfoElement templateElement;

	[SerializeField]
	private UIButton _legoIDButton;

	[SerializeField]
	private UIButton _leaderboardButton;

	private List<ProgressionInfoElement> _currentElements = new List<ProgressionInfoElement>();

	private bool _forceReset;

	public void OnBack()
	{
		CameraHUB._pInstance._pCameraControllable = true;
		Navigate("Back");
	}

	public void OnTrySignIn()
	{
		LEGOID._pInstance.Login(OnLogin);
	}

	public void OnLogin(LEGOID.ELoginStatus status)
	{
		if (Facades<TrackingFacade>.Instance != null && status == LEGOID.ELoginStatus.LOGIN_SUCCESS)
		{
			Facades<TrackingFacade>.Instance.LogMetric("Profile", "LEGOID");
			Facades<TrackingFacade>.Instance.LogEvent("Login_From_Profile");
		}
	}

	public void OnLeaderboardPressed()
	{
		Navigate("ProgressInfoLeaderboard");
	}

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		int levelFromEXP = RewardManager._pInstance.GetLevelFromEXP(GlobalInGameData._pCurrentExp);
		float levelProgressFromEXP = RewardManager._pInstance.GetLevelProgressFromEXP(GlobalInGameData._pCurrentExp);
		expBarCurrent.position = Vector3.Lerp(expBarStart.position, expBarEnd.position, levelProgressFromEXP);
		currentLevelLarge.text = levelFromEXP.ToString();
		currentLevelSmall.text = levelFromEXP.ToString();
		nextLevel.text = (levelFromEXP + 1).ToString();
		SetupInfo();
		UpdateLEGOIDButton();
		SoundFacade._pInstance.PlayOneShotSFX("GUIPop", 0.25f);
	}

	public void ResetAll()
	{
		_forceReset = true;
	}

	private void SetupInfo()
	{
		RewardManager._pInstance.LoadHistory();
		int levelFromEXP = RewardManager._pInstance.GetLevelFromEXP(GlobalInGameData._pCurrentExp);
		int count = RewardManager._pInstance._pLevelUpRewardedHistory.Count;
		int num = Mathf.Max(count, levelFromEXP);
		int count2 = _currentElements.Count;
		Debug.Log("<color=blue>Rewards Issued: " + count + "</color>");
		Debug.Log("<color=blue>" + count2 + "< " + (num + 10) + " || " + _forceReset + "</color>");
		if (_currentElements.Count < num + 10 || _forceReset)
		{
			Clean();
			templateElement.gameObject.SetActive(true);
			for (int i = 0; i < num + 10; i++)
			{
				GameObject gameObject = Object.Instantiate(templateElement.gameObject);
				ProgressionInfoElement component = gameObject.GetComponent<ProgressionInfoElement>();
				gameObject.transform.parent = templateElement.transform.parent;
				gameObject.transform.localPosition = templateElement.transform.localPosition + Vector3.down * i * 110f;
				gameObject.transform.localScale = templateElement.transform.localScale;
				gameObject.name = "item" + (i + 1);
				_currentElements.Add(component);
				if (i >= count)
				{
					RewardManager.EBRICK_BAG_CATEGORY rewardChestForLevel = RewardManager._pInstance.GetRewardChestForLevel(i);
					switch (rewardChestForLevel)
					{
					case RewardManager.EBRICK_BAG_CATEGORY.LARGE_CHEST:
						component.itemDesc.text = Localise("ProgressInfo.LargeChest");
						break;
					case RewardManager.EBRICK_BAG_CATEGORY.MEDIUM_CHEST:
						component.itemDesc.text = Localise("ProgressInfo.MediumChest");
						break;
					case RewardManager.EBRICK_BAG_CATEGORY.SMALL_CHEST:
						component.itemDesc.text = Localise("ProgressInfo.SmallChest");
						break;
					}
					component.chestSprite.enabled = true;
					component.studSprite.enabled = false;
					if (i >= levelFromEXP)
					{
						component.chestSprite.spriteName = "chestHidden";
						component.chestSprite.alpha = 1f;
					}
					else
					{
						switch (rewardChestForLevel)
						{
						case RewardManager.EBRICK_BAG_CATEGORY.LARGE_CHEST:
							component.chestSprite.spriteName = "chestLarge_Gold";
							break;
						case RewardManager.EBRICK_BAG_CATEGORY.MEDIUM_CHEST:
							component.chestSprite.spriteName = "chestLarge_Silver";
							break;
						case RewardManager.EBRICK_BAG_CATEGORY.SMALL_CHEST:
							component.chestSprite.spriteName = "chestLarge_Bronze";
							break;
						}
						component.chestSprite.alpha = 0.5f;
					}
					component.itemDesc.color = textUnavailableColor;
					component.levelNumber.color = ((i < levelFromEXP) ? numberNormalColor : numberUnavailableColor);
					component.levelNumber.effectColor = ((i < levelFromEXP) ? shadowNormalColor : shadowUnavailableColor);
					if (component.dropShadows != null && component.dropShadows.Length > 0)
					{
						for (int j = 0; j < component.dropShadows.Length; j++)
						{
							component.dropShadows[j].color = shadowUnavailableColor;
						}
					}
				}
				else
				{
					RewardManager.BrickBagRewardData brickBagRewardData = RewardManager._pInstance._pLevelUpRewardedHistory[i];
					if (brickBagRewardData.brickCount > 0)
					{
						component.itemDesc.text = brickBagRewardData.brickCount + Localise("ProgressInfo.Bricks");
					}
					else
					{
						component.itemDesc.text = Localise(brickBagRewardData.rewardedPart.localisationKey);
					}
					component.chestSprite.enabled = true;
					component.studSprite.enabled = false;
					RewardManager.EBRICK_BAG_CATEGORY rewardChestForLevel2 = RewardManager._pInstance.GetRewardChestForLevel(i);
					switch (rewardChestForLevel2)
					{
					case RewardManager.EBRICK_BAG_CATEGORY.LARGE_CHEST:
						component.chestSprite.spriteName = "chestLarge_Gold";
						break;
					case RewardManager.EBRICK_BAG_CATEGORY.MEDIUM_CHEST:
						component.chestSprite.spriteName = "chestLarge_Silver";
						break;
					case RewardManager.EBRICK_BAG_CATEGORY.SMALL_CHEST:
						component.chestSprite.spriteName = "chestLarge_Bronze";
						break;
					}
					component.chestSprite.alpha = 1f;
					Debug.Log("Category for " + i + " is " + rewardChestForLevel2);
					component.itemDesc.color = textNormalColor;
					component.levelNumber.color = numberNormalColor;
					component.levelNumber.effectColor = shadowNormalColor;
					if (component.dropShadows != null && component.dropShadows.Length > 0)
					{
						for (int k = 0; k < component.dropShadows.Length; k++)
						{
							component.dropShadows[k].color = shadowNormalColor;
						}
					}
				}
				component.levelNumber.text = (i + 1).ToString();
			}
			templateElement.gameObject.SetActive(false);
		}
		draggableElement.height = (num + 11) * 110;
		draggableElement.UpdateAnchors();
	}

	public void OnCodeEntry()
	{
		Navigate("CodeEntry");
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
			Debug.Log("Changed Leaderboard button active state [false] because we are offline.");
			_leaderboardButton.gameObject.SetActive(false);
		}
		if (_legoIDButton.gameObject.activeInHierarchy)
		{
			Debug.Log("Changed LEGOID button active state [false] because we are offline.");
			_legoIDButton.gameObject.SetActive(false);
		}
	}

	private Material GetBrickRewardMat(int colorID)
	{
		return brickRewardMaterials[colorID];
	}

	private void Clean()
	{
		int count = _currentElements.Count;
		for (int i = 0; i < count; i++)
		{
			Object.Destroy(_currentElements[i].gameObject);
		}
		_currentElements.Clear();
	}
}
