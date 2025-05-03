using System.Text;
using UnityEngine;

public class ScreenMinigameResults : ScreenBase
{
	private enum ERESULTS_STATE
	{
		INIT = 0,
		TWEEN_IN_FRAME_X = 1,
		TWEEN_IN_FRAME_Y = 2,
		PAUSE1 = 3,
		TALLY_R1 = 4,
		TALLY_R2 = 5,
		TALLY_R3 = 6,
		TALLY_R4 = 7,
		PAUSE2 = 8,
		FADE_PANEL = 9,
		MOVE_MAIN_STUD_DOWN = 10,
		MOVE_MAIN_STUD_ACROSS = 11,
		PAUSE3 = 12,
		TALLY_BRICKS = 13,
		MOVE_BRICK_ACROSS = 14,
		PAUSE4 = 15,
		WRAP_UP = 16
	}

	private const string MISSION_COMPLETE_TEXT_KEY = "Results.MissionComplete";

	private const string OUT_OF_TIME_TEXT_KEY = "Results.OutOfTime";

	public UISprite frame;

	public UILabel titleText;

	public UIWidget[] row1;

	public UIWidget[] row2;

	public UIWidget[] row3;

	public UIWidget[] row4;

	public UIWidget tallyAreaPanel;

	public UILabel mainStudCount;

	public UILabel studCount1;

	public UILabel studCount2;

	public UILabel studCount3;

	public UILabel studCount4;

	public Transform largeStudStartLoc;

	public Transform largeStudMidLoc;

	public Transform largeStudEndLoc;

	public Transform brickStartLoc;

	public Transform brickEndLoc;

	public Animator arrowAnim;

	public UIWidget largeStud;

	public UIWidget arrowWidget;

	public UIWidget brick;

	public UITexture[] studs;

	public UIButton retryButton;

	public UIWidget brickConverterPanel;

	public UILabel bricksGained;

	public ScreenObjectTransitionWidget[] widgetsAfterTally;

	private int _studsCollected;

	private int _missionCompleteBonus;

	private int _timeBonus;

	private int _destructionBonus;

	private ERESULTS_STATE _state;

	private float _stateStartTime;

	private StringBuilder _dummySB = new StringBuilder();

	private int _mainStudStart;

	private int _mainStudTarget;

	private int _brickTarget;

	private int _studSFXCounter;

	private int _brickSFXCounter;

	private bool _isStudSFXPlaying;

	private bool _isBrickSFXPlaying;

	protected override void OnShowScreen()
	{
		retryButton.isEnabled = GlobalInGameData._pHasSeenGarageTutorial;
		SoundFacade._pInstance.SetSFXChannelMute("Game", true);
		_missionCompleteBonus = MinigameController._pInstance._pMissionCompleteBonus;
		_timeBonus = MinigameController._pInstance._pTimeBonus;
		_destructionBonus = MinigameController._pInstance._pDestructionBonus;
		_studsCollected = MinigameController._pInstance._pStudsCollected;
		if (_timeBonus < 0)
		{
			_timeBonus = 0;
			Debug.LogError("Time bonus recieved was negative");
		}
		QuestHandler._pInstance.SessionStudsGained(_missionCompleteBonus + _timeBonus + _destructionBonus + _studsCollected);
		MinigameManager.MinigameData currentMinigameData = MinigameManager._pInstance.GetCurrentMinigameData();
		int num = _missionCompleteBonus + _timeBonus + _studsCollected;
		if (currentMinigameData._pPersonalHighScore < num)
		{
			currentMinigameData._pPersonalHighScore = num;
		}
		studCount1.text = "0";
		studCount2.text = "0";
		studCount3.text = "0";
		studCount4.text = "0";
		mainStudCount.text = "0";
		int num2 = studs.Length;
		for (int i = 0; i < num2; i++)
		{
			studs[i].material = ScenarioManager._pInstance._pCurrentScenario.guiStudMaterial;
		}
		titleText.text = LocalisationFacade.Instance.GetString((MinigameController._pInstance._pMissionCompleteBonus <= 0) ? "Results.OutOfTime" : "Results.MissionComplete");
		brick.material = ScenarioManager._pInstance._pCurrentScenario.brickMaterial;
		brick.transform.localPosition = brickStartLoc.localPosition;
		largeStud.transform.localPosition = largeStudStartLoc.localPosition;
		tallyAreaPanel.alpha = 0f;
		brickConverterPanel.alpha = 0f;
		bricksGained.text = "0";
		MinigameManager._pInstance.OnMinigameResults();
		for (int j = 0; j < widgetsAfterTally.Length; j++)
		{
			widgetsAfterTally[j].SetStateConsideredTweenedIn(false);
			widgetsAfterTally[j].SetToTweenInStartScale();
		}
		Time.timeScale = 0f;
		IssueRewards();
		ChangeState(ERESULTS_STATE.TWEEN_IN_FRAME_X);
		GlobalInGameData._pHasDoneFirstTimeVisit = true;
	}

	protected override void Update()
	{
		base.Update();
		switch (_state)
		{
		case ERESULTS_STATE.TWEEN_IN_FRAME_X:
			UpdateTweenInFrameX();
			break;
		case ERESULTS_STATE.TWEEN_IN_FRAME_Y:
			UpdateTweenInFrameY();
			break;
		case ERESULTS_STATE.PAUSE1:
			UpdatePause1();
			break;
		case ERESULTS_STATE.TALLY_R1:
			UpdateTally1();
			break;
		case ERESULTS_STATE.TALLY_R2:
			UpdateTally2();
			break;
		case ERESULTS_STATE.TALLY_R3:
			UpdateTally3();
			break;
		case ERESULTS_STATE.TALLY_R4:
			UpdateTally4();
			break;
		case ERESULTS_STATE.PAUSE2:
			UpdatePause2();
			break;
		case ERESULTS_STATE.FADE_PANEL:
			UpdateFadePanel();
			break;
		case ERESULTS_STATE.MOVE_MAIN_STUD_DOWN:
			UpdateMoveMainStudDown();
			break;
		case ERESULTS_STATE.MOVE_MAIN_STUD_ACROSS:
			UpdateMoveMainStudAcross();
			break;
		case ERESULTS_STATE.PAUSE3:
			UpdatePause3();
			break;
		case ERESULTS_STATE.TALLY_BRICKS:
			UpdateTallyBricks();
			break;
		case ERESULTS_STATE.PAUSE4:
			UpdatePause4();
			break;
		case ERESULTS_STATE.MOVE_BRICK_ACROSS:
			UpdateMoveBrickAcross();
			break;
		case ERESULTS_STATE.WRAP_UP:
			UpdateWrapUp();
			break;
		}
	}

	private void ChangeState(ERESULTS_STATE state)
	{
		_state = state;
		_stateStartTime = RealTime.time;
		switch (_state)
		{
		case ERESULTS_STATE.TWEEN_IN_FRAME_X:
			StartTweenInFrameX();
			break;
		case ERESULTS_STATE.TWEEN_IN_FRAME_Y:
			StartTweenInFrameY();
			break;
		case ERESULTS_STATE.PAUSE1:
			StartPause1();
			break;
		case ERESULTS_STATE.TALLY_R1:
			StartTally1();
			break;
		case ERESULTS_STATE.TALLY_R2:
			StartTally2();
			break;
		case ERESULTS_STATE.TALLY_R3:
			StartTally3();
			break;
		case ERESULTS_STATE.TALLY_R4:
			StartTally4();
			break;
		case ERESULTS_STATE.PAUSE2:
			StartPause2();
			break;
		case ERESULTS_STATE.FADE_PANEL:
			StartFadePanel();
			break;
		case ERESULTS_STATE.MOVE_MAIN_STUD_DOWN:
			StartMoveMainStudDown();
			break;
		case ERESULTS_STATE.MOVE_MAIN_STUD_ACROSS:
			StartMoveMainStudAcross();
			break;
		case ERESULTS_STATE.PAUSE3:
			StartPause3();
			break;
		case ERESULTS_STATE.TALLY_BRICKS:
			StartTallyBricks();
			break;
		case ERESULTS_STATE.PAUSE4:
			StartPause4();
			break;
		case ERESULTS_STATE.MOVE_BRICK_ACROSS:
			StartMoveBrickAcross();
			break;
		case ERESULTS_STATE.WRAP_UP:
			StartWrapUp();
			break;
		}
	}

	private void StartTweenInFrameX()
	{
		frame.width = 1;
		frame.height = 1;
		for (int i = 0; i < row1.Length; i++)
		{
			row1[i].alpha = 0f;
		}
		for (int j = 0; j < row2.Length; j++)
		{
			row2[j].alpha = 0f;
		}
		for (int k = 0; k < row3.Length; k++)
		{
			row3[k].alpha = 0f;
		}
		for (int l = 0; l < row4.Length; l++)
		{
			row4[l].alpha = 0f;
		}
	}

	private void UpdateTweenInFrameX()
	{
		float num = RealTime.time - _stateStartTime;
		if (num < 0.25f)
		{
			float t = Easing.Ease(Easing.EaseType.EaseOutCircle, num, 0.25f, 0f, 1f);
			frame.width = (int)Mathf.Lerp(1f, 1622f, t);
		}
		else
		{
			frame.width = 1622;
			ChangeState(ERESULTS_STATE.TWEEN_IN_FRAME_Y);
		}
	}

	private void StartTweenInFrameY()
	{
	}

	private void UpdateTweenInFrameY()
	{
		float num = RealTime.time - _stateStartTime;
		if (num < 0.25f)
		{
			float t = Easing.Ease(Easing.EaseType.EaseOutCircle, num, 0.25f, 0f, 1f);
			frame.height = (int)Mathf.Lerp(1f, 959f, t);
		}
		else
		{
			frame.height = 959;
			ChangeState(ERESULTS_STATE.PAUSE1);
		}
	}

	private void StartPause1()
	{
	}

	private void UpdatePause1()
	{
		float num = RealTime.time - _stateStartTime;
		if (num > 0.25f)
		{
			tallyAreaPanel.alpha = 1f;
			ChangeState(ERESULTS_STATE.TALLY_R1);
		}
		else
		{
			float alpha = num / 0.25f;
			tallyAreaPanel.alpha = alpha;
		}
	}

	private void StartTally1()
	{
		for (int i = 0; i < row1.Length; i++)
		{
			row1[i].alpha = 1f;
		}
		_mainStudStart = 0;
		_mainStudTarget = _studsCollected;
		if (_studsCollected != 0)
		{
			playStudSFX();
		}
	}

	private void UpdateTally1()
	{
		float num = RealTime.time - _stateStartTime;
		float num2 = 1f;
		if (num > num2)
		{
			stopStudsSFX();
			ChangeState(ERESULTS_STATE.TALLY_R2);
			studCount1.text = _studsCollected.ToString();
		}
		else
		{
			float t = num / num2;
			int num3 = (int)Mathf.Lerp(0f, _studsCollected, t);
			studCount1.text = num3.ToString();
			int num4 = (int)Mathf.Lerp(_mainStudStart, _mainStudTarget, t);
			mainStudCount.text = num4.ToString();
		}
	}

	private void StartTally2()
	{
		for (int i = 0; i < row2.Length; i++)
		{
			row2[i].alpha = 1f;
		}
		_mainStudStart = _studsCollected;
		_mainStudTarget = _studsCollected + _missionCompleteBonus;
		if (_missionCompleteBonus != 0)
		{
			playStudSFX();
		}
	}

	private void UpdateTally2()
	{
		float num = RealTime.time - _stateStartTime;
		float num2 = 1f;
		if (num > num2)
		{
			stopStudsSFX();
			ChangeState(ERESULTS_STATE.TALLY_R3);
			studCount2.text = _missionCompleteBonus.ToString();
		}
		else
		{
			float t = num / num2;
			int num3 = (int)Mathf.Lerp(0f, _missionCompleteBonus, t);
			studCount2.text = num3.ToString();
			int num4 = (int)Mathf.Lerp(_mainStudStart, _mainStudTarget, t);
			mainStudCount.text = num4.ToString();
		}
	}

	private void StartTally3()
	{
		for (int i = 0; i < row3.Length; i++)
		{
			row3[i].alpha = 1f;
		}
		_mainStudStart = _studsCollected + _missionCompleteBonus;
		_mainStudTarget = _studsCollected + _missionCompleteBonus + _timeBonus;
		if (_timeBonus != 0)
		{
			playStudSFX();
		}
	}

	private void UpdateTally3()
	{
		float num = RealTime.time - _stateStartTime;
		float num2 = 1f;
		if (num > num2)
		{
			stopStudsSFX();
			ChangeState(ERESULTS_STATE.PAUSE2);
			studCount3.text = _timeBonus.ToString();
		}
		else
		{
			float t = num / num2;
			int num3 = (int)Mathf.Lerp(0f, _timeBonus, t);
			studCount3.text = num3.ToString();
			int num4 = (int)Mathf.Lerp(_mainStudStart, _mainStudTarget, t);
			mainStudCount.text = num4.ToString();
		}
	}

	private void StartTally4()
	{
		for (int i = 0; i < row4.Length; i++)
		{
			row4[i].alpha = 1f;
		}
		_mainStudStart = _studsCollected + _missionCompleteBonus + _timeBonus;
		_mainStudTarget = _studsCollected + _missionCompleteBonus + _timeBonus + _destructionBonus;
		if (_destructionBonus != 0)
		{
			playStudSFX();
		}
	}

	private void UpdateTally4()
	{
		float num = RealTime.time - _stateStartTime;
		float num2 = 1f;
		if (num > num2)
		{
			stopStudsSFX();
			ChangeState(ERESULTS_STATE.PAUSE2);
			studCount4.text = _destructionBonus.ToString();
			mainStudCount.text = _mainStudTarget.ToString();
		}
		else
		{
			float t = num / num2;
			int num3 = (int)Mathf.Lerp(0f, _destructionBonus, t);
			studCount4.text = num3.ToString();
			int num4 = (int)Mathf.Lerp(_mainStudStart, _mainStudTarget, t);
			mainStudCount.text = num4.ToString();
		}
	}

	private void StartPause2()
	{
	}

	private void UpdatePause2()
	{
		float num = RealTime.time - _stateStartTime;
		if (num > 0.75f)
		{
			ChangeState(ERESULTS_STATE.FADE_PANEL);
		}
	}

	private void StartFadePanel()
	{
	}

	private void UpdateFadePanel()
	{
		float num = RealTime.time - _stateStartTime;
		float num2 = 0.25f;
		if (num > num2)
		{
			tallyAreaPanel.alpha = 0f;
			ChangeState(ERESULTS_STATE.MOVE_MAIN_STUD_DOWN);
		}
		else
		{
			float num3 = num / num2;
			tallyAreaPanel.alpha = 1f - num3;
		}
	}

	private void StartMoveMainStudDown()
	{
	}

	private void UpdateMoveMainStudDown()
	{
		float num = RealTime.time - _stateStartTime;
		float num2 = 0.25f;
		if (num > num2)
		{
			largeStud.transform.localPosition = largeStudMidLoc.localPosition;
			ChangeState(ERESULTS_STATE.MOVE_MAIN_STUD_ACROSS);
		}
		else
		{
			float t = Easing.Ease(Easing.EaseType.EaseOutCircle, num, num2, 0f, 1f);
			largeStud.transform.localPosition = Vector3.Lerp(largeStudStartLoc.localPosition, largeStudMidLoc.localPosition, t);
		}
	}

	private void StartMoveMainStudAcross()
	{
		arrowWidget.alpha = 1f;
		arrowAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
	}

	private void UpdateMoveMainStudAcross()
	{
		float num = RealTime.time - _stateStartTime;
		float num2 = 0.25f;
		if (num > num2)
		{
			largeStud.transform.localPosition = largeStudEndLoc.localPosition;
			ChangeState(ERESULTS_STATE.PAUSE3);
			frame.height = 522;
			brickConverterPanel.alpha = 1f;
		}
		else
		{
			float num3 = Easing.Ease(Easing.EaseType.EaseOutCircle, num, num2, 0f, 1f);
			brickConverterPanel.alpha = num3;
			largeStud.transform.localPosition = Vector3.Lerp(largeStudMidLoc.localPosition, largeStudEndLoc.localPosition, num3);
			frame.height = (int)Mathf.Lerp(959f, 522f, num3);
		}
	}

	private void StartPause3()
	{
	}

	private void UpdatePause3()
	{
		float num = RealTime.time - _stateStartTime;
		if (num > 0.75f)
		{
			ChangeState(ERESULTS_STATE.TALLY_BRICKS);
		}
	}

	private void StartTallyBricks()
	{
		_mainStudStart = _studsCollected + _missionCompleteBonus + _timeBonus + _destructionBonus;
		_mainStudTarget = 0;
		if (_mainStudStart > 0)
		{
			playBrickSFX();
		}
	}

	private void UpdateTallyBricks()
	{
		float num = RealTime.time - _stateStartTime;
		float num2 = 1f;
		if (num > num2)
		{
			ChangeState(ERESULTS_STATE.PAUSE4);
			bricksGained.text = _brickTarget.ToString();
			mainStudCount.text = "0";
			stopBrickSFX();
		}
		else
		{
			float t = num / num2;
			int num3 = (int)Mathf.Lerp(0f, _brickTarget, t);
			bricksGained.text = num3.ToString();
			int num4 = (int)Mathf.Lerp(_mainStudStart, _mainStudTarget, t);
			mainStudCount.text = num4.ToString();
		}
	}

	private void StartPause4()
	{
	}

	private void UpdatePause4()
	{
		float num = RealTime.time - _stateStartTime;
		if (num > 0.75f)
		{
			ChangeState(ERESULTS_STATE.MOVE_BRICK_ACROSS);
		}
		else if (num > 0.5f)
		{
			float num2 = (num - 0.5f) / 0.25f;
			largeStud.alpha = 1f - num2;
		}
	}

	private void StartMoveBrickAcross()
	{
		arrowWidget.alpha = 0f;
		largeStud.alpha = 0f;
	}

	private void UpdateMoveBrickAcross()
	{
		float num = RealTime.time - _stateStartTime;
		float num2 = 0.25f;
		if (num > num2)
		{
			brick.transform.localPosition = brickEndLoc.localPosition;
			ChangeState(ERESULTS_STATE.WRAP_UP);
			brickConverterPanel.alpha = 1f;
		}
		else
		{
			float t = Easing.Ease(Easing.EaseType.EaseOutCircle, num, num2, 0f, 1f);
			brick.transform.localPosition = Vector3.Lerp(brickStartLoc.localPosition, brickEndLoc.localPosition, t);
		}
	}

	private void StartWrapUp()
	{
		for (int i = 0; i < widgetsAfterTally.Length; i++)
		{
			widgetsAfterTally[i].TweenIn();
		}
		SoundFacade._pInstance.PlayOneShotSFX("GUIPop", 1f);
		SoundFacade._pInstance.PlayOneShotSFX("GUIPop", 1.25f);
		SoundFacade._pInstance.PlayMusic("FrontEnd", 1f);
	}

	private void UpdateWrapUp()
	{
	}

	private void IssueRewards()
	{
		int num = _studsCollected + _missionCompleteBonus + _timeBonus + _destructionBonus;
		_brickTarget = num / 10;
		ScenarioManager._pInstance.AddBrickReward(_brickTarget);
		GlobalInGameData._pCumulativeStuds += num;
		if (MinigameController._pInstance._pMinigame._pHasBeenCompleted)
		{
			Debug.Log("Won: Rewarding 20 EXP");
			GlobalInGameData._pCurrentExp += 20;
		}
		else
		{
			Debug.Log("Failed: Rewarding 10 EXP");
			GlobalInGameData._pCurrentExp += 10;
		}
	}

	public void OnContinue()
	{
		if (base._pCurrentTweenType == ScreenTweenType.Idle)
		{
			StopGameSounds();
			if (!SoundFacade._pInstance._pMusicMuted)
			{
				SoundFacade._pInstance.SetSFXChannelMute("Game", false);
			}
			SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
			Time.timeScale = 1f;
			VehicleBuilder.ClearTemplate();
			ScreenHub.LoadDefaultHUB();
			Navigate("LoadHub");
		}
	}

	public void OnReload()
	{
		if (base._pCurrentTweenType == ScreenTweenType.Idle)
		{
			QuestHandler._pInstance.ResetMinigameSessionStats();
			StopGameSounds();
			SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
			if (!SoundFacade._pInstance._pMusicMuted)
			{
				SoundFacade._pInstance.SetSFXChannelMute("Game", false);
			}
			Time.timeScale = 1f;
			Navigate("MinigameLoading");
			ScreenLoading.Reload();
		}
	}

	private void playStudSFX()
	{
		if (!_isStudSFXPlaying)
		{
			SoundFacade._pInstance.PlayLoopingSFX("GUIStudTally", 0f);
			_isStudSFXPlaying = true;
		}
	}

	private void playBrickSFX()
	{
		if (!_isBrickSFXPlaying)
		{
			SoundFacade._pInstance.PlayLoopingSFX("GUIBrickTally", 0f);
			_isBrickSFXPlaying = true;
		}
	}

	private void stopStudsSFX()
	{
		SoundFacade._pInstance.StopAllLoopingAudioByGroupName("GUIStudTally");
		_isStudSFXPlaying = false;
	}

	private void stopBrickSFX()
	{
		SoundFacade._pInstance.StopAllLoopingAudioByGroupName("GUIBrickTally");
		_isBrickSFXPlaying = false;
	}

	public static void StopGameSounds()
	{
		SoundFacade._pInstance.StopAllLoopingAudioByGroupName("CarEngineDrive");
		SoundFacade._pInstance.StopAllLoopingAudioByGroupName("CarEngineIdle");
		SoundFacade._pInstance.StopAllLoopingAudioByGroupName("CarSkid");
		SoundFacade._pInstance.StopAllLoopingAudioByGroupName("BoatEngineDrive");
		SoundFacade._pInstance.StopAllLoopingAudioByGroupName("BoatEngineIdle");
		SoundFacade._pInstance.StopAllLoopingAudioByGroupName("AttachmentHelicopter");
		SoundFacade._pInstance.StopAllLoopingAudioByGroupName("AttachmentHose");
		SoundFacade._pInstance.StopAllLoopingAudioByGroupName("AttachmentJet");
		SoundFacade._pInstance.StopAllLoopingAudioByGroupName("AttachmentSiren");
		SoundFacade._pInstance.StopAllLoopingAudioByGroupName("OilStay");
		SoundFacade._pInstance.StopAllLoopingAudioByGroupName("DirtStay");
	}
}
