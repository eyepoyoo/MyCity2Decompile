using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ScreenScenarioComplete : ScreenBase
{
	private enum ESTATE
	{
		AWAITING_BUILD = 0,
		BUILDING = 1,
		AWAITING_FTUE_CLEAR = 2,
		COMPLETED_BUILD = 3
	}

	private const string BUILDING_NAME = "[BUILDING_NAME]";

	private const string BUILDING_LOCALISATION_TEMPLATE = "BuildingNames.{0}";

	private const int COMPLETE_MESSAGE_COUNT = 5;

	public UILabel scenarioTitle;

	public UILabel scenarioDescription;

	public UILabel brickCount;

	public UITexture brickIcon;

	public UITexture scenarioTexture;

	public UITexture scenarioTextureWide;

	public UITexture scenarioFigurine;

	public ParticleSystem confettiParticles;

	public Animator animator;

	public ScreenObjectTransitionWidget panelTransitionWidget;

	public ScreenObjectTransitionWidget _ftueMessageBox;

	public UILabel _ftueLabel;

	public UILabel _ftueTitle;

	public ScreenObjectTransitionWidget _ftueBackground;

	public BoxCollider _ftueBackgroundCollider;

	private StringBuilder _dummySB = new StringBuilder();

	private ESTATE _state;

	private float _timer;

	protected override void OnShowScreen()
	{
		RefreshBricks();
		scenarioTitle.text = Localise(ScenarioManager._pInstance._pCurrentScenario.scenarioName);
		scenarioDescription.text = Localise(ScenarioManager._pInstance._pCurrentScenario.scenarioDescription);
		Scenario pCurrentScenario = ScenarioManager._pInstance._pCurrentScenario;
		_state = ESTATE.AWAITING_BUILD;
		scenarioFigurine.mainTexture = pCurrentScenario.scenarioMinifigTexture;
		_ftueMessageBox.SetStateConsideredTweenedIn(false);
		_ftueMessageBox.SetToTweenInStartPos();
		int num = Random.Range(0, 5);
		string text = Localise("FTUE.ScenarioComplete" + num);
		if (text.IndexOf("[BUILDING_NAME]") != -1)
		{
			string text2 = Localise(string.Format("BuildingNames.{0}", ScenarioManager._pInstance._pCurrentScenario.building.ToString()));
			text = text.Replace("[BUILDING_NAME]", text2);
			if (Facades<TrackingFacade>.Instance != null)
			{
				Facades<TrackingFacade>.Instance.LogParameterMetric("Building", new Dictionary<string, string> { { "Building", text2 } });
			}
		}
		_ftueTitle.text = Localise("FTUE.ScenarioCompleteTitle");
		_ftueLabel.text = text;
		_ftueBackgroundCollider.enabled = false;
		_ftueBackground.SetToTweenInStartColor();
		scenarioTexture.mainTexture = pCurrentScenario.scenarioTexture;
		scenarioTextureWide.mainTexture = pCurrentScenario.scenarioTexture;
		scenarioTexture.enabled = !pCurrentScenario.scenarioTextureWide;
		scenarioTextureWide.enabled = pCurrentScenario.scenarioTextureWide;
		brickIcon.material = ScenarioManager._pInstance._pCurrentScenario.brickMaterial;
		animator.enabled = false;
		animator.gameObject.SetActive(false);
	}

	public void OnContinue()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		if (_state == ESTATE.AWAITING_BUILD)
		{
			CameraHUB._pInstance.ResolveBuild();
			animator.gameObject.SetActive(true);
            animator.enabled = true;
            animator.Play("ScenarioComplete", 0, 0f);
			_state = ESTATE.BUILDING;
			panelTransitionWidget.TweenOut();
			_timer = 3.5f;
		}
	}

	protected override void Update()
	{
		base.Update();
		if (_state == ESTATE.AWAITING_FTUE_CLEAR && !Facades<ScreenFacade>.Instance._pIsAnyScreenTweening && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
		{
			OnFTUEOK();
		}
		if (_state == ESTATE.BUILDING)
		{
			_timer -= Time.deltaTime;
			if (_timer <= 0f)
			{
				Debug.Log("Completed scenario complete screen");
				ScenarioManager._pInstance.FindAvailableScenarios();
				ScreenHub._pInstance.UnlockScenarioButtons();
				ScreenHub._pInstance.RefreshBricks();
				_ftueBackground.SetToTweenInStartColor();
				_ftueBackground.TweenIn();
				_ftueMessageBox.SetStateConsideredTweenedIn(false);
				_ftueMessageBox.SetToTweenInStartPos();
				_ftueMessageBox.TweenIn();
				_state = ESTATE.AWAITING_FTUE_CLEAR;
			}
		}
	}

	public void OnFTUEOK()
	{
		CameraHUB._pInstance.RestoreNormalZoom();
		ScreenSelectScenario.selectScenarioCanGoBack = false;
		Navigate("Continue");
		_state = ESTATE.COMPLETED_BUILD;
	}

	public void RefreshBricks()
	{
		_dummySB.Length = 0;
		_dummySB.Append(ScenarioManager._pInstance._pCurrentScenario._pCurrentBricks);
		_dummySB.Append("/");
		_dummySB.Append(ScenarioManager._pInstance._pCurrentScenario.totalBricksRequired);
		brickCount.text = _dummySB.ToString();
	}
}
