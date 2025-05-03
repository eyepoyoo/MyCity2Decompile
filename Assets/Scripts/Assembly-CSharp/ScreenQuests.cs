using System.Text;
using UnityEngine;

public class ScreenQuests : ScreenBase
{
	public static int _numNewQuests;

	private readonly Vector3 SCALE_ONE = Vector3.one;

	public Transform[] panels;

	public Transform[] onScreen;

	public Transform[] stageRight;

	public Transform[] stageLeft;

	public UISprite[] infoSprites;

	public GameObject[] rewardButtons;

	public UIWidget[] rewardButtonContainers;

	public UILabel currentScenario;

	public UILabel currentScenarioBrickProgress;

	public UITexture scenarioBrick;

	public UITexture[] brickTextures;

	public UILabel[] descriptions;

	public UILabel[] rewardLabels;

	private float[] _startTime;

	private bool[] _isTweening;

	private Transform[] _tweenStart;

	private Transform[] _tweenEnd;

	private bool[] _isTweeningCompleteElements;

	private bool[] _isTweeningNewElements;

	private bool _tweeningCurrentBricks;

	private float _currentBrickTweenStartTime;

	private int _lastKnownBricks;

	private Achievement[] _achiRefs;

	private int _numActiveElements;

	private int[] _yOffset;

	private bool[] _disableOnTweenComplete;

	private StringBuilder _dummySB = new StringBuilder();

	private void Awake()
	{
		_startTime = new float[3];
		_isTweening = new bool[3];
		_tweenStart = new Transform[3];
		_tweenEnd = new Transform[3];
		_achiRefs = new Achievement[3];
		_yOffset = new int[3];
		_isTweeningCompleteElements = new bool[3];
		_isTweeningNewElements = new bool[3];
		_disableOnTweenComplete = new bool[3];
	}

	public void OnBack()
	{
		CameraHUB._pInstance._pCameraControllable = true;
		Navigate("Back");
	}

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		currentScenario.text = Localise(ScenarioManager._pInstance._pCurrentScenario.scenarioName);
		scenarioBrick.material = ScenarioManager._pInstance._pCurrentScenario.brickMaterial;
		scenarioBrick.transform.localScale = SCALE_ONE;
		for (int i = 0; i < brickTextures.Length; i++)
		{
			brickTextures[i].material = ScenarioManager._pInstance._pCurrentScenario.brickMaterial;
		}
		_dummySB.Length = 0;
		_dummySB.Append(ScenarioManager._pInstance._pCurrentScenario._pCurrentBricks);
		_dummySB.Append("/");
		_dummySB.Append(ScenarioManager._pInstance._pCurrentScenario.totalBricksRequired);
		currentScenarioBrickProgress.text = _dummySB.ToString();
		int numActiveQuests = QuestSystem._pInstance.GetNumActiveQuests();
		int num = QuestHandler._pInstance.NumRewardsToClaim();
		_numActiveElements = numActiveQuests + num;
		Debug.Log("Screen Quests: " + numActiveQuests + " / " + num);
		int num2 = 0;
		int num3 = 0;
		for (int j = 0; j < 3; j++)
		{
			_yOffset[j] = 0;
			if (j < _numActiveElements)
			{
				panels[j].gameObject.SetActive(true);
				panels[j].transform.position = stageRight[j].transform.position;
				_isTweening[j] = true;
				_tweenStart[j] = stageRight[j].transform;
				_tweenEnd[j] = onScreen[j].transform;
				_startTime[j] = Time.time + (float)j * 0.5f;
				_disableOnTweenComplete[j] = false;
				infoSprites[j].enabled = false;
				_isTweeningNewElements[j] = _numActiveElements - j <= _numNewQuests;
				Debug.Log("Should Display New Elem? " + (_numActiveElements - j) + " <= " + _numNewQuests);
				infoSprites[j].alpha = 1f;
				infoSprites[j].spriteName = "NewQuestAlertIcon";
				infoSprites[j].enabled = _isTweeningNewElements[j];
				if (num3 < num)
				{
					string questName = QuestHandler._pInstance.GetQuestName(num3);
					descriptions[j].text = Localise("Quest." + questName);
					Debug.Log("Looking up achi: " + questName + " reward = " + num3);
					Achievement achievement = AchievementSystem._pInstance.GetAchievement(questName);
					string metaString = achievement.GetMetaString("REWARD");
					rewardLabels[j].text = metaString;
					_achiRefs[j] = achievement;
					rewardButtons[j].SetActive(true);
					UIWidget uIWidget = rewardButtonContainers[j];
					uIWidget.alpha = 0.0039f;
					infoSprites[j].enabled = true;
					infoSprites[j].alpha = 0f;
					infoSprites[j].spriteName = "questCompleteIcon";
					_isTweeningCompleteElements[j] = true;
					num3++;
				}
				else if (num2 < numActiveQuests)
				{
					string questName2 = QuestSystem._pInstance.GetQuestName(num2);
					descriptions[j].text = Localise("Quest." + questName2);
					Achievement achievement2 = AchievementSystem._pInstance.GetAchievement(questName2);
					string metaString2 = achievement2.GetMetaString("REWARD");
					rewardLabels[j].text = metaString2;
					_achiRefs[j] = achievement2;
					rewardButtons[j].SetActive(false);
					num2++;
				}
			}
			else
			{
				panels[j].gameObject.SetActive(false);
				_isTweening[j] = false;
			}
		}
		_numNewQuests = 0;
	}

	protected override void Update()
	{
		base.Update();
		if (_tweeningCurrentBricks)
		{
			float num = Time.time - _currentBrickTweenStartTime;
			float num2 = num / 1f;
			if (num2 < 1f)
			{
				int value = (int)Mathf.Lerp(_lastKnownBricks, ScenarioManager._pInstance._pCurrentScenario._pCurrentBricks, num2);
				_dummySB.Length = 0;
				_dummySB.Append(value);
				_dummySB.Append("/");
				_dummySB.Append(ScenarioManager._pInstance._pCurrentScenario.totalBricksRequired);
				currentScenarioBrickProgress.text = _dummySB.ToString();
				float t = Frac(num2 * 16f);
				scenarioBrick.transform.localScale = Vector3.Lerp(new Vector3(1.3f, 1.3f, 1.3f), SCALE_ONE, t);
			}
			else
			{
				_tweeningCurrentBricks = false;
				_dummySB.Length = 0;
				_dummySB.Append(ScenarioManager._pInstance._pCurrentScenario._pCurrentBricks);
				_dummySB.Append("/");
				_dummySB.Append(ScenarioManager._pInstance._pCurrentScenario.totalBricksRequired);
				currentScenarioBrickProgress.text = _dummySB.ToString();
				ScreenHub._pInstance.RefreshBricks();
				if (ScenarioManager._pInstance._pCurrentScenario._pCurrentBricks == ScenarioManager._pInstance._pCurrentScenario.totalBricksRequired)
				{
					Navigate("Back");
					CityManager._pInstance.RefreshLocalCity();
				}
			}
		}
		else
		{
			scenarioBrick.transform.localScale = SCALE_ONE;
		}
		bool flag = false;
		for (int i = 0; i < 3; i++)
		{
			if (_isTweening[i])
			{
				flag = true;
				float num3 = Time.time - _startTime[i];
				float num4 = 0.5f;
				if (num3 > 0f)
				{
					if (num3 < num4)
					{
						float t2 = Easing.Ease(Easing.EaseType.EaseOutCircle, num3, num4, 0f, 1f);
						panels[i].transform.position = Vector3.Lerp(_tweenStart[i].position, _tweenEnd[i].position, t2);
					}
					else
					{
						panels[i].transform.position = _tweenEnd[i].position;
						_isTweening[i] = false;
						if (_disableOnTweenComplete[i])
						{
							panels[i].gameObject.SetActive(false);
						}
					}
				}
			}
			if (_isTweeningNewElements[i])
			{
				flag = true;
				float num5 = Time.time - _startTime[i];
				float num6 = 1f;
				float num7 = num5 - 1.7f;
				if (!(num7 < 0f))
				{
					if (num7 < num6)
					{
						float num8 = Easing.Ease(Easing.EaseType.EaseOutCircle, num7, num6, 0f, 1f);
						infoSprites[i].alpha = Mathf.Clamp(1f - num8, 0.0039f, 1f);
					}
					else if (num7 >= num6)
					{
						infoSprites[i].alpha = 0.0039f;
						_isTweeningNewElements[i] = false;
					}
				}
			}
			if (!_isTweeningCompleteElements[i])
			{
				continue;
			}
			flag = true;
			float num9 = Time.time - _startTime[i];
			float num10 = 0.3f;
			float num11 = num9 - 0.7f;
			float num12 = num9 - 0.9f;
			if (!(num11 < 0f))
			{
				if (num11 < num10)
				{
					float num13 = Easing.Ease(Easing.EaseType.EaseInCircle, num11, num10, 0f, 1f);
					rewardButtons[i].transform.localScale = Vector3.Lerp(new Vector3(3f, 3f, 3f), Vector3.one, num13);
					UIWidget uIWidget = rewardButtonContainers[i];
					uIWidget.alpha = Mathf.Clamp(num13, 0.0039f, 1f);
				}
				else if (num11 >= num10)
				{
					rewardButtons[i].transform.localScale = Vector3.one;
					UIWidget uIWidget2 = rewardButtonContainers[i];
					uIWidget2.alpha = 1f;
				}
			}
			if (!(num12 < 0f))
			{
				if (num12 < num10)
				{
					float num14 = Easing.Ease(Easing.EaseType.EaseInCircle, num12, num10, 0f, 1f);
					infoSprites[i].transform.localScale = Vector3.Lerp(new Vector3(3f, 3f, 3f), Vector3.one, num14);
					infoSprites[i].alpha = num14;
				}
				else if (num12 >= num10)
				{
					infoSprites[i].transform.localScale = Vector3.one;
					infoSprites[i].alpha = 1f;
					_isTweeningCompleteElements[i] = false;
				}
			}
		}
		if (flag)
		{
			return;
		}
		int num15 = panels.Length;
		bool flag2 = true;
		for (int j = 0; j < num15; j++)
		{
			if (panels[j].gameObject.activeInHierarchy)
			{
				flag2 = false;
			}
		}
		if (flag2)
		{
			CameraHUB._pInstance._pCameraControllable = true;
			Navigate("Back");
		}
	}

	private bool CanClickReward()
	{
		if (_tweeningCurrentBricks)
		{
			return false;
		}
		int num = _isTweening.Length;
		for (int i = 0; i < num; i++)
		{
			if (_isTweening[i])
			{
				return false;
			}
		}
		num = _isTweeningCompleteElements.Length;
		for (int j = 0; j < num; j++)
		{
			if (_isTweeningCompleteElements[j])
			{
				return false;
			}
		}
		num = _isTweeningNewElements.Length;
		for (int k = 0; k < num; k++)
		{
			if (_isTweeningNewElements[k])
			{
				return false;
			}
		}
		return true;
	}

	public void OnPanelAReward()
	{
		if (CanClickReward())
		{
			int num = int.Parse(_achiRefs[0].GetMetaString("REWARD"));
			ScenarioManager._pInstance.AddBrickReward(num);
			GlobalInGameData._pCumulativeStuds += num * 10;
			_isTweening[0] = true;
			_tweenStart[0] = onScreen[0].transform;
			_tweenEnd[0] = stageLeft[0].transform;
			_startTime[0] = Time.time;
			_disableOnTweenComplete[0] = true;
			if (panels[1].gameObject.activeInHierarchy)
			{
				_isTweening[1] = true;
				_tweenStart[1] = onScreen[1 - _yOffset[1]].transform;
				_tweenEnd[1] = onScreen[1 - _yOffset[1] - 1].transform;
				_startTime[1] = Time.time;
				_yOffset[1]++;
			}
			if (panels[2].gameObject.activeInHierarchy)
			{
				_isTweening[2] = true;
				_tweenStart[2] = onScreen[2 - _yOffset[2]].transform;
				_tweenEnd[2] = onScreen[2 - _yOffset[2] - 1].transform;
				_startTime[2] = Time.time;
				_yOffset[2]++;
			}
			_tweeningCurrentBricks = true;
			_currentBrickTweenStartTime = Time.time;
			QuestHandler._pInstance.OnQuestRewarded(_achiRefs[0].achievementName);
			QuestHandler._pInstance.SaveAll();
		}
	}

	public void OnPanelBReward()
	{
		if (CanClickReward())
		{
			int num = int.Parse(_achiRefs[1].GetMetaString("REWARD"));
			ScenarioManager._pInstance.AddBrickReward(num);
			GlobalInGameData._pCumulativeStuds += num * 10;
			_isTweening[1] = true;
			_tweenStart[1] = onScreen[1 - _yOffset[1]].transform;
			_tweenEnd[1] = stageLeft[1 - _yOffset[1]].transform;
			_startTime[1] = Time.time;
			rewardButtons[1].gameObject.SetActive(false);
			_disableOnTweenComplete[1] = true;
			if (panels[2].gameObject.activeInHierarchy)
			{
				_isTweening[2] = true;
				_tweenStart[2] = onScreen[2 - _yOffset[2]].transform;
				_tweenEnd[2] = onScreen[2 - _yOffset[2] - 1].transform;
				_startTime[2] = Time.time;
				_yOffset[2]++;
			}
			_tweeningCurrentBricks = true;
			_currentBrickTweenStartTime = Time.time;
			QuestHandler._pInstance.OnQuestRewarded(_achiRefs[1].achievementName);
			QuestHandler._pInstance.SaveAll();
		}
	}

	public void OnPanelCReward()
	{
		if (CanClickReward())
		{
			int num = int.Parse(_achiRefs[2].GetMetaString("REWARD"));
			_lastKnownBricks = ScenarioManager._pInstance._pCurrentScenario._pCurrentBricks;
			ScenarioManager._pInstance.AddBrickReward(num);
			GlobalInGameData._pCumulativeStuds += num * 10;
			rewardButtons[2].gameObject.SetActive(false);
			_disableOnTweenComplete[2] = true;
			_isTweening[2] = true;
			_tweenStart[2] = onScreen[2 - _yOffset[2]].transform;
			_tweenEnd[2] = stageLeft[2 - _yOffset[2]].transform;
			_startTime[2] = Time.time;
			_tweeningCurrentBricks = true;
			_currentBrickTweenStartTime = Time.time;
			QuestHandler._pInstance.OnQuestRewarded(_achiRefs[2].achievementName);
			QuestHandler._pInstance.SaveAll();
		}
	}

	private float Frac(float v)
	{
		return v - Mathf.Floor(v);
	}
}
