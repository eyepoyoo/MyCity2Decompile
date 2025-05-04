using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ScreenScenarioInfo : ScreenBase
{
	[Serializable]
	public class ScenarioImage
	{
		public Texture2D scenarioTexture;

		public bool wideImage;

		public CityManager.ECITYBUILDINGS buildingType;
	}

	public UILabel scenarioTitle;

	public UILabel scenarioDescription;

	public UILabel brickCount;

	public UITexture brickIcon;

	public UIPanel backingPanel;

	public UITexture scenarioFigurine;

	public UITexture scenarioTexture;

	public UITexture scenarioTextureWide;

	public ScenarioImage[] scenarioRenders;

	public UISprite hubTransitionFade;

	public static int _selectedScenarioIndex;

	private StringBuilder _dummySB = new StringBuilder();

	protected override void OnShowScreen()
	{
		RefreshBricks();
		Scenario scenarioInfo = ScenarioManager._pInstance.GetSlottedScenario(_selectedScenarioIndex);
		scenarioTitle.text = Localise(scenarioInfo.scenarioName);
		scenarioDescription.text = Localise(scenarioInfo.scenarioDescription);
		scenarioFigurine.mainTexture = scenarioInfo.scenarioMinifigTexture;
		ScenarioImage scenarioImage = Array.Find(scenarioRenders, (ScenarioImage s) => s.buildingType == scenarioInfo.building);
		if (scenarioImage != null)
		{
			if (scenarioImage.wideImage)
			{
				scenarioTexture.gameObject.SetActive(false);
				scenarioTextureWide.gameObject.SetActive(true);
				scenarioTextureWide.mainTexture = scenarioImage.scenarioTexture;
			}
			else
			{
				if (scenarioInfo.building == CityManager.ECITYBUILDINGS.LIGHTHOUSE)
				{
					scenarioTexture.transform.localPosition = new Vector3(306f, -85f, 0f);
				}
				else if (scenarioInfo.building == CityManager.ECITYBUILDINGS.POLICE_ISLAND_HQ)
				{
					scenarioTexture.transform.localPosition = new Vector3(287f, -85f, 0f);
				}
				else
				{
					scenarioTexture.transform.localPosition = new Vector3(231f, -85f, 0f);
				}
				scenarioTexture.gameObject.SetActive(true);
				scenarioTextureWide.gameObject.SetActive(false);
				scenarioTexture.mainTexture = scenarioImage.scenarioTexture;
			}
		}
		CityManager._pInstance.PanCameraToBuilding(scenarioInfo.building, CameraHUB.EFocusType.PAN_ONLY);
		brickIcon.material = scenarioInfo.brickMaterial;
		hubTransitionFade.alpha = ScreenHub._pInstance.hubTransitionFade.alpha;
		SoundFacade._pInstance.PlayOneShotSFX("GUISwish", 0f);
	}

	public void OnBack()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUIBack", 0f);
		Navigate("SelectScenario");
	}

	public void OnContinue()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		ScenarioManager._pInstance.SelectScenario(_selectedScenarioIndex);
		if (Facades<TrackingFacade>.Instance != null)
		{
			string scenarioName = ScenarioManager._pInstance._pCurrentScenario.scenarioName;
			Facades<TrackingFacade>.Instance.LogParameterMetric("Scenario", new Dictionary<string, string> { { "Scenario", scenarioName } });
			Facades<TrackingFacade>.Instance.LogEvent("Scenario_" + ScenarioManager._pInstance._pCurrentScenario.building);
		}
		if (ScenarioManager._pInstance._pCurrentScenario._pProgress == 1f)
		{
			ScenarioManager._pInstance._pCurrentScenario.ResetScenario();
		}
		CameraHUB._pInstance._pCameraControllable = true;
		ScreenHub._pInstance.RefreshBricks();
		CityManager.ECITYBUILDINGS building = ScenarioManager._pInstance._pCurrentScenario.building;
		CityManager._pInstance.SetCurrentSelection(building);
		ScenarioManager._pInstance._pIsFreshSave = false;
		Navigate("Hub");
	}

	protected override void Update()
	{
		base.Update();
		hubTransitionFade.alpha = ScreenHub._pInstance.hubTransitionFade.alpha;
		if (Facades<ScreenFacade>.Instance._pIsAnyScreenTweening)
		{
			backingPanel.SetDirty();
		}
	}

	public void RefreshBricks()
	{
		Scenario slottedScenario = ScenarioManager._pInstance.GetSlottedScenario(_selectedScenarioIndex);
		_dummySB.Length = 0;
		_dummySB.Append(slottedScenario._pCurrentBricks);
		_dummySB.Append("/");
		_dummySB.Append(slottedScenario.totalBricksRequired);
		brickCount.text = _dummySB.ToString();
	}
}
