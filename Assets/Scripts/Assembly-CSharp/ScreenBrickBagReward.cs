using System;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBrickBagReward : ScreenBase
{
	[Serializable]
	public class ShaderReplacementPairs
	{
		public Shader sourceShader;

		public Shader targetShader;
	}

	public Animator animRef;

	private float _startTime;

	private float _animSyncError;

	public ShaderReplacementPairs[] shaderReplacementPairs;

	public Transform[] initScale0;

	public UIWidget[] initAlpha1;

	public MeshRenderer[] chestRenderers;

	public Material largeMaterial;

	public Material mediumMaterial;

	public Material smallMaterial;

	public Transform partSpawnLoc;

	public Transform chestLoc;

	public UITexture brickIcon;

	public SpecialBrickBagRewardCam specialChestCam;

	public Transform chestTransform;

	private GameObject _spawnedItem;

	public UILabel rewardLabel;

	private bool _claimedReward;

	private float _objAnimStartTime;

	private Vector3 _spawnCenterPos;

	private Vector3 _targetItemScale;

	private GameObject _dummySpawnLoc;

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		rewardLabel.text = string.Empty;
		specialChestCam.referenceInterploate = 0f;
		specialChestCam.Update();
		UpdateChestMaterial();
		int num = initScale0.Length;
		chestTransform.localScale = Vector3.zero;
		for (int i = 0; i < num; i++)
		{
			initScale0[i].localScale = Vector3.zero;
		}
		int num2 = initAlpha1.Length;
		float alpha = 0.003921569f;
		for (int j = 0; j < num2; j++)
		{
			initAlpha1[j].alpha = alpha;
		}
		brickIcon.material = ScenarioManager._pInstance._pCurrentScenario.brickMaterialHighRes;
		_startTime = RealTime.time;
		_animSyncError = 0f;
		animRef.Play("Default");
		SoundFacade._pInstance.PlayOneShotSFX("ChestCreak", 1.5f);
		SoundFacade._pInstance.PlayOneShotSFX("ChestOpen", 1.6f);
		UpdateChestMaterial();
		_claimedReward = false;
		int levelFromEXP = RewardManager._pInstance.GetLevelFromEXP(GlobalInGameData._pCurrentExp);
		if (GlobalInGameData._pClaimedRewards < levelFromEXP)
		{
			RewardManager._pInstance.IssueLevelUpRewardFromLevel(GlobalInGameData._pClaimedRewards, OnIssuedReward);
			Debug.Log("Issuing Level up Chest");
			GlobalInGameData._pClaimedRewards++;
		}
		else if (GlobalInGameData._pUnclaimedDailyRewardChests > 0)
		{
			Debug.Log("Issuing Daily Reward Chest");
			RewardManager.BrickBagRewardData rewardData = RewardManager._pInstance.IssueReward(RewardManager.EBRICK_BAG_CATEGORY.LARGE_CHEST, levelFromEXP);
			OnIssuedReward(rewardData);
			GlobalInGameData._pUnclaimedDailyRewardChests--;
		}
	}

	protected override void OnScreenExitComplete()
	{
		base.OnScreenExitComplete();
		chestTransform.localScale = Vector3.zero;
	}

	protected override void Update()
	{
		base.Update();
		float num = RealTime.time - _startTime;
		ScreenHub._pInstance.UpdateTweeningBricks();
		if (num >= 6.5f && !_claimedReward)
		{
			OnContinue();
		}
		if (_spawnedItem != null)
		{
			float num2 = RealTime.time - _objAnimStartTime;
			float num3 = 1f;
			float normalizedTime = animRef.GetCurrentAnimatorStateInfo(0).normalizedTime;
			float num4 = num2 - _animSyncError;
			if (num4 < 0f)
			{
				partSpawnLoc.localScale = Vector3.zero;
			}
			else if (normalizedTime < 0.3f)
			{
				_animSyncError += RealTime.deltaTime;
			}
			else if (num4 < num3)
			{
				float t = Easing.Ease(Easing.EaseType.EaseOutCircle, num4, num3, 0f, 1f);
				float t2 = Easing.Ease(Easing.EaseType.Linear, num4, num3, 0f, 1f);
				Vector3 localScale = Vector3.Lerp(Vector3.zero, _targetItemScale, t);
				partSpawnLoc.transform.localScale = localScale;
				Vector3 vector = partSpawnLoc.InverseTransformPoint(partSpawnLoc.TransformPoint(_spawnCenterPos) + Vector3.up * 0.5f);
				Vector3 localPosition = Vector3.Lerp(Vector3.Lerp(_spawnCenterPos, vector, t2), Vector3.Lerp(vector, _spawnCenterPos, t2), t2);
				_spawnedItem.transform.localPosition = localPosition;
			}
			else
			{
				partSpawnLoc.transform.localScale = _targetItemScale;
				_spawnedItem.transform.localPosition = _spawnCenterPos;
				Vector3 localEulerAngles = partSpawnLoc.localEulerAngles;
				localEulerAngles.y += 90f * RealTime.deltaTime;
				partSpawnLoc.localEulerAngles = localEulerAngles;
			}
		}
	}

	private void UpdateChestMaterial()
	{
		int level = GlobalInGameData._pClaimedRewards;
		int levelFromEXP = RewardManager._pInstance.GetLevelFromEXP(GlobalInGameData._pCurrentExp);
		if (GlobalInGameData._pClaimedRewards >= levelFromEXP)
		{
			level = 1;
		}
		RewardManager.EBRICK_BAG_CATEGORY rewardChestForLevel = RewardManager._pInstance.GetRewardChestForLevel(level);
		Material material = smallMaterial;
		switch (rewardChestForLevel)
		{
		case RewardManager.EBRICK_BAG_CATEGORY.LARGE_CHEST:
			material = largeMaterial;
			break;
		case RewardManager.EBRICK_BAG_CATEGORY.MEDIUM_CHEST:
			material = mediumMaterial;
			break;
		case RewardManager.EBRICK_BAG_CATEGORY.SMALL_CHEST:
			material = smallMaterial;
			break;
		}
		for (int i = 0; i < chestRenderers.Length; i++)
		{
			chestRenderers[i].material = material;
		}
	}

	private void OnIssuedReward(RewardManager.BrickBagRewardData rewardData)
	{
		Debug.Log("OnIssuedReward: " + rewardData.brickCount + " / " + (rewardData.rewardedPart == null));
		if (rewardData.brickCount != 0)
		{
			rewardLabel.text = "+" + rewardData.brickCount;
			brickIcon.enabled = true;
			ScreenHub._pInstance.BeginTweeningBricks(3.5f);
			return;
		}
		partSpawnLoc.localRotation = Quaternion.identity;
		if (Facades<TrackingFacade>.Instance != null)
		{
			Facades<TrackingFacade>.Instance.LogParameterMetric("Unlocked", new Dictionary<string, string> { 
			{
				"Parts",
				rewardData.rewardedPart.localisationKey
			} });
			Facades<TrackingFacade>.Instance.LogProgress("Unlock_" + rewardData.rewardedPart.localisationKey);
		}
		Debug.Log("Rewarded Part: " + rewardData.rewardedPart.localisationKey);
		rewardLabel.text = Localise(rewardData.rewardedPart.localisationKey);
		brickIcon.enabled = false;
		_objAnimStartTime = RealTime.time + 2f;
		GameObject gameObject = rewardData.rewardedPart.Spawn();
		VehiclePart component = gameObject.GetComponent<VehiclePart>();
		CarouselBehaviour carouselBehaviour = gameObject.AddComponent<CarouselBehaviour>();
		carouselBehaviour._pIsCarouselMode = true;
		partSpawnLoc.localScale = Vector3.one;
		gameObject.transform.parent = partSpawnLoc;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		Vector3 vector = partSpawnLoc.InverseTransformPoint(component._pCentralPoint);
		gameObject.transform.localPosition = -vector;
		_targetItemScale = component.brickBagRewardScale;
		MonoBehaviour[] componentsInChildren = gameObject.GetComponentsInChildren<MonoBehaviour>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
		Collider[] componentsInChildren2 = gameObject.GetComponentsInChildren<Collider>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].enabled = false;
		}
		ParticleSystem[] componentsInChildren3 = gameObject.GetComponentsInChildren<ParticleSystem>();
		for (int k = 0; k < componentsInChildren3.Length; k++)
		{
			componentsInChildren3[k].gameObject.layer = LayerMask.NameToLayer("Geometry");
		}
		SkinnedMeshRenderer[] componentsInChildren4 = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		for (int l = 0; l < componentsInChildren4.Length; l++)
		{
			componentsInChildren4[l].gameObject.layer = LayerMask.NameToLayer("Geometry");
			Material[] materials = componentsInChildren4[l].materials;
			int num = materials.Length;
			for (int m = 0; m < num; m++)
			{
				componentsInChildren4[l].materials[m] = new Material(componentsInChildren4[l].materials[m]);
				componentsInChildren4[l].materials[m].shader = FindReplacementShader(componentsInChildren4[l].materials[m].shader);
			}
		}
		MeshRenderer[] componentsInChildren5 = gameObject.GetComponentsInChildren<MeshRenderer>();
		for (int n = 0; n < componentsInChildren5.Length; n++)
		{
			componentsInChildren5[n].gameObject.layer = LayerMask.NameToLayer("Geometry");
			Material[] materials2 = componentsInChildren5[n].materials;
			int num2 = materials2.Length;
			for (int num3 = 0; num3 < num2; num3++)
			{
				componentsInChildren5[n].materials[num3] = new Material(componentsInChildren5[n].materials[num3]);
				componentsInChildren5[n].materials[num3].shader = FindReplacementShader(componentsInChildren5[n].materials[num3].shader);
			}
		}
		ScreenBase screen = Facades<ScreenFacade>.Instance.GetScreen("ScreenProgressInfo");
		if (screen != null)
		{
			ScreenProgressInfo screenProgressInfo = (ScreenProgressInfo)screen;
			screenProgressInfo.ResetAll();
		}
		_spawnedItem = gameObject;
		partSpawnLoc.localScale = Vector3.zero;
		_spawnCenterPos = _spawnedItem.transform.localPosition;
	}

	private Shader FindReplacementShader(Shader source)
	{
		Shader result = source;
		for (int i = 0; i < shaderReplacementPairs.Length; i++)
		{
			if (shaderReplacementPairs[i].sourceShader == source)
			{
				result = shaderReplacementPairs[i].targetShader;
				break;
			}
		}
		return result;
	}

	public void OnContinue()
	{
		_claimedReward = true;
		CameraHUB._pInstance._pCameraControllable = true;
		ScreenHub._pInstance.RefreshAfterRewards();
		if (_spawnedItem != null)
		{
			UnityEngine.Object.Destroy(_spawnedItem);
		}
		if (!GlobalInGameData._pHasSeenGarageTutorial)
		{
			ScreenHub._pInstance.ShowGarageTutorial();
		}
		Navigate("Hub");
	}
}
