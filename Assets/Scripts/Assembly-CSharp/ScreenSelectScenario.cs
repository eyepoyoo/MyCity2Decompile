using System.Text;
using UnityEngine;

public class ScreenSelectScenario : ScreenBase
{
	public static bool selectScenarioCanGoBack = true;

	public UILabel[] brickCountLabels;

	public UILabel[] scenarioNameLabels;

	public UITexture[] brickIcons;

	public UITexture[] scenarioRenderHolders;

	public UITexture[] scenarioRenderHoldersWide;

	[SerializeField]
	private GameObject[] _buttonFlashers;

	[SerializeField]
	private float _buttonFlashInterval = 0.5f;

	public ScreenObjectTransitionWidget[] ftueWidgets;

	public Collider ftueBackgroundCollider;

	public UIButton backButton;

	private bool _seenFTUE;

	private bool _isFTUEPromptUp;

	private StringBuilder _dummySB = new StringBuilder();

	protected override void Update()
	{
		base.Update();
		if (_isFTUEPromptUp && !Facades<ScreenFacade>.Instance._pIsAnyScreenTweening && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
		{
			OnFTUEOK();
		}
	}

	public void ResetAll()
	{
		_seenFTUE = false;
	}

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		bool flag = ScenarioManager._pInstance._pIsFreshSave && !_seenFTUE;
		ftueBackgroundCollider.enabled = flag;
		if (flag)
		{
			_isFTUEPromptUp = true;
			_seenFTUE = true;
			if (Facades<TrackingFacade>.Instance != null)
			{
				Facades<TrackingFacade>.Instance.LogMetric("2.Scenario Select", "FTUE");
				Facades<TrackingFacade>.Instance.LogProgress("FTUE_2_Scenario_Select");
			}
			int num = ftueWidgets.Length;
			for (int i = 0; i < num; i++)
			{
				ftueWidgets[i].SetStateConsideredTweenedIn(false);
				ftueWidgets[i].SetToTweenInStartColor();
				ftueWidgets[i].SetToTweenInStartPos();
				ftueWidgets[i].SetToTweenInStartScale();
				ftueWidgets[i].TweenIn();
			}
		}
		else
		{
			int num2 = ftueWidgets.Length;
			for (int j = 0; j < num2; j++)
			{
				ftueWidgets[j].SetToTweenInStartColor();
				ftueWidgets[j].SetToTweenInStartPos();
				ftueWidgets[j].SetToTweenInStartScale();
			}
		}
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish", 0f);
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish2", 0.25f);
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish3", 0.5f);
		int num3 = brickCountLabels.Length;
		CameraHUB._pInstance._pCameraControllable = false;
		for (int k = 0; k < num3; k++)
		{
			Scenario slottedScenario = ScenarioManager._pInstance.GetSlottedScenario(k);
			scenarioNameLabels[k].text = Localise(slottedScenario.scenarioName);
			_dummySB.Length = 0;
			_dummySB.Append(slottedScenario._pCurrentBricks);
			_dummySB.Append("/");
			_dummySB.Append(slottedScenario.totalBricksRequired);
			brickCountLabels[k].text = _dummySB.ToString();
			brickIcons[k].material = slottedScenario.brickMaterial;
			Debug.Log(k + ": " + slottedScenario.building);
			scenarioRenderHolders[k].mainTexture = slottedScenario.scenarioTexture;
			scenarioRenderHoldersWide[k].mainTexture = slottedScenario.scenarioTexture;
			scenarioRenderHolders[k].enabled = !slottedScenario.scenarioTextureWide;
			scenarioRenderHoldersWide[k].enabled = slottedScenario.scenarioTextureWide;
		}
		backButton.isEnabled = selectScenarioCanGoBack && !ScenarioManager._pInstance._pIsFreshSave;
	}

	public void OnFTUEOK()
	{
		if (_isFTUEPromptUp && !Facades<ScreenFacade>.Instance._pIsAnyScreenTweening)
		{
			_isFTUEPromptUp = false;
			int num = ftueWidgets.Length;
			for (int i = 0; i < num; i++)
			{
				ftueWidgets[i].TweenOut();
			}
			ftueBackgroundCollider.enabled = false;
			FlashButtons();
		}
	}

	protected override void OnScreenShowComplete()
	{
		base.OnScreenShowComplete();
		backButton.isEnabled = selectScenarioCanGoBack && !ScenarioManager._pInstance._pIsFreshSave;
	}

	public void OnScenarioA()
	{
		if (!Facades<ScreenFacade>.Instance._pIsAnyScreenTweening)
		{
			ScreenScenarioInfo._selectedScenarioIndex = 0;
			SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
			SoundFacade._pInstance.PlayOneShotSFX("GUISwish", 0.1f);
			SoundFacade._pInstance.PlayOneShotSFX("GUISwish2", 0.35f);
			SoundFacade._pInstance.PlayOneShotSFX("GUISwish3", 0.6f);
			HideAllFlashes();
			Navigate("ScenarioInfo");
		}
	}

	public void OnScenarioB()
	{
		if (!Facades<ScreenFacade>.Instance._pIsAnyScreenTweening)
		{
			ScreenScenarioInfo._selectedScenarioIndex = 1;
			SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
			SoundFacade._pInstance.PlayOneShotSFX("GUISwish", 0.1f);
			SoundFacade._pInstance.PlayOneShotSFX("GUISwish2", 0.35f);
			SoundFacade._pInstance.PlayOneShotSFX("GUISwish3", 0.6f);
			HideAllFlashes();
			Navigate("ScenarioInfo");
		}
	}

	public void OnScenarioC()
	{
		if (!Facades<ScreenFacade>.Instance._pIsAnyScreenTweening)
		{
			ScreenScenarioInfo._selectedScenarioIndex = 2;
			SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
			SoundFacade._pInstance.PlayOneShotSFX("GUISwish", 0.1f);
			SoundFacade._pInstance.PlayOneShotSFX("GUISwish2", 0.35f);
			SoundFacade._pInstance.PlayOneShotSFX("GUISwish3", 0.6f);
			HideAllFlashes();
			Navigate("ScenarioInfo");
		}
	}

	public void OnBack()
	{
		HideAllFlashes();
		Navigate("Hub");
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish", 0.1f);
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish2", 0.35f);
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish3", 0.6f);
		SoundFacade._pInstance.PlayOneShotSFX("GUIBack", 0f);
		CameraHUB._pInstance._pCameraControllable = true;
	}

	public void FlashButtons()
	{
		if (_isVisible && !Facades<ScreenFacade>.Instance._pIsAnyScreenTweening && _buttonFlashers != null && _buttonFlashers.Length != 0)
		{
			Invoke("FlashButtonOne", _buttonFlashInterval);
			Invoke("FlashButtonTwo", _buttonFlashInterval * 2f);
			Invoke("FlashButtonThree", _buttonFlashInterval * 3f);
			Invoke("FlashButtons", _buttonFlashInterval * 4f);
		}
	}

	private void HideAllFlashes()
	{
		for (int i = 0; i < _buttonFlashers.Length; i++)
		{
			if (!(_buttonFlashers[i] == null))
			{
				_buttonFlashers[i].SetActive(false);
			}
		}
	}

	private void FlashButtonOne()
	{
		if (_isVisible && !Facades<ScreenFacade>.Instance._pIsAnyScreenTweening && _buttonFlashers != null && _buttonFlashers.Length != 0)
		{
			_buttonFlashers[0].SetActive(true);
		}
	}

	private void FlashButtonTwo()
	{
		if (_isVisible && !Facades<ScreenFacade>.Instance._pIsAnyScreenTweening && _buttonFlashers != null && _buttonFlashers.Length >= 2)
		{
			_buttonFlashers[1].SetActive(true);
		}
	}

	private void FlashButtonThree()
	{
		if (_isVisible && !Facades<ScreenFacade>.Instance._pIsAnyScreenTweening && _buttonFlashers != null && _buttonFlashers.Length >= 3)
		{
			_buttonFlashers[2].SetActive(true);
		}
	}
}
