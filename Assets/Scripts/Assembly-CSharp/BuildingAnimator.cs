using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAnimator : MonoBehaviour
{
	[Serializable]
	public class BuildStage
	{
		public float progress;

		public AnimatedBuildingPart partsToShow;

		public void SetVisibilityOnProgress(float curProgress)
		{
			MeshRenderer[] componentsInChildren = partsToShow.GetComponentsInChildren<MeshRenderer>();
			bool enabled = curProgress >= progress;
			int num = componentsInChildren.Length;
			for (int i = 0; i < num; i++)
			{
				componentsInChildren[i].enabled = enabled;
			}
			ParticleSystem[] componentsInChildren2 = partsToShow.GetComponentsInChildren<ParticleSystem>();
			if (componentsInChildren2 != null)
			{
				int num2 = componentsInChildren2.Length;
				for (int j = 0; j < num2; j++)
				{
					ParticleSystem.EmissionModule emission = componentsInChildren2[j].emission;
					emission.enabled = enabled;
				}
			}
		}
	}

	private const float SINK_DUR = 0.5f;

	private const float SINK_OFFSET = 10f;

	private AnimatedBuildingPart[] buildingParts;

	public AnimationCurve bounceAnimCurve_IN;

	public AnimationCurve bounceAnimCurve_OUT;

	public float offsetHeight = 16f;

	public float animationDuration = 1f;

	public bool animateIn = true;

	public bool doAnimate;

	public ParticleSystem OnCompleteParticleSystem;

	public BuildStage[] buildStages;

	private MeshRenderer[] _allRenderers;

	private ParticleSystem[] _allParticles;

	private int numPiecesComplete;

	private int numBuildingPieces;

	private Vector3[] originalPiecePositions;

	private Vector3[] offsetPiecePositions;

	private bool hasPlayedFinishedSound;

	private Dictionary<AnimatedBuildingPart, int> _partIndexLookup = new Dictionary<AnimatedBuildingPart, int>();

	private Action _onCompletedAction;

	private bool _wantSink;

	private float _sinkStartTime;

	private Vector3 _rootPos;

	private bool _consideredVisible;

	public bool _pIsVisible
	{
		get
		{
			if (_wantSink)
			{
				return false;
			}
			return _consideredVisible;
		}
	}

	private void Awake()
	{
		_rootPos = base.transform.position;
		numPiecesComplete = 0;
		buildingParts = base.gameObject.GetComponentsInChildren<AnimatedBuildingPart>();
		numBuildingPieces = buildingParts.Length;
		GetOriginalPiecePos();
		_wantSink = false;
		_allRenderers = GetComponentsInChildren<MeshRenderer>();
		_allParticles = GetComponentsInChildren<ParticleSystem>();
		if (animateIn)
		{
			MoveToInitialOffsets();
		}
	}

	public void Sink()
	{
		_wantSink = true;
		_sinkStartTime = Time.time;
	}

	public void SetRenderVisibilityOnBuildStage(float progress)
	{
		ResetSink();
		_consideredVisible = progress > 0f;
		int num = buildStages.Length;
		for (int i = 0; i < num; i++)
		{
			buildStages[i].SetVisibilityOnProgress(progress);
		}
	}

	public bool HasMadeProgress(float currentProgress, float lastKnownProgress)
	{
		int num = buildStages.Length;
		for (int i = 0; i < num; i++)
		{
			if (currentProgress >= buildStages[i].progress && lastKnownProgress < buildStages[i].progress)
			{
				return true;
			}
		}
		return false;
	}

	public void SetRendererVisibilty(bool visible)
	{
		if (visible)
		{
			ResetSink();
		}
		int num = _allRenderers.Length;
		for (int i = 0; i < num; i++)
		{
			_allRenderers[i].enabled = visible;
		}
		int num2 = _allParticles.Length;
		for (int j = 0; j < num2; j++)
		{
			ParticleSystem.EmissionModule emission = _allParticles[j].emission;
			emission.enabled = visible;
		}
		_consideredVisible = visible;
	}

	public void SetToOutPosition()
	{
		MoveToInitialOffsets();
	}

	public void KickoffPartialBuildProcess(float prevProgression, float curProgression, Action onComplete = null)
	{
		ResetSink();
		_onCompletedAction = onComplete;
		int num = buildStages.Length;
		float num2 = float.MaxValue;
		_consideredVisible = curProgression > 0f;
		for (int i = 0; i < num; i++)
		{
			if (buildStages[i].progress <= curProgression && buildStages[i].progress > prevProgression)
			{
				int num3 = _partIndexLookup[buildStages[i].partsToShow];
				if (buildingParts[num3].inAnimationDelay < num2)
				{
					num2 = buildingParts[num3].inAnimationDelay;
				}
			}
		}
		for (int j = 0; j < num; j++)
		{
			buildStages[j].SetVisibilityOnProgress(curProgression);
			if (buildStages[j].progress <= curProgression && buildStages[j].progress > prevProgression)
			{
				int num4 = _partIndexLookup[buildStages[j].partsToShow];
				Debug.Log("Tweening piece: " + buildStages[j].partsToShow.name);
				Vector3 position = buildingParts[num4].transform.position;
				position.y += offsetHeight;
				buildingParts[num4].transform.position = position;
				LeanTween.moveY(buildingParts[num4].buildingPiece, originalPiecePositions[num4].y, animationDuration).setDelay(buildingParts[num4].inAnimationDelay - num2).setEase(bounceAnimCurve_IN)
					.setIgnoreTimeScale(true)
					.setOnComplete(OnCompleteFX);
			}
		}
	}

	public void KickoffBuildProcess(Action onComplete = null)
	{
		_consideredVisible = true;
		ResetSink();
		_onCompletedAction = onComplete;
		numPiecesComplete = 0;
		if (animateIn)
		{
			AnimateIntoPosition();
		}
		else
		{
			AnimateOutOfPosition();
		}
	}

	private void GetOriginalPiecePos()
	{
		originalPiecePositions = new Vector3[numBuildingPieces];
		for (int i = 0; i < numBuildingPieces; i++)
		{
			_partIndexLookup[buildingParts[i]] = i;
			originalPiecePositions[i] = buildingParts[i].transform.position;
		}
	}

	private void MoveToInitialOffsets()
	{
		offsetPiecePositions = new Vector3[numBuildingPieces];
		for (int i = 0; i < numBuildingPieces; i++)
		{
			Vector3 position = buildingParts[i].transform.position;
			position.y += offsetHeight;
			buildingParts[i].transform.position = position;
			offsetPiecePositions[i] = buildingParts[i].transform.position;
		}
	}

	private void AnimateOutOfPosition()
	{
		for (int num = numBuildingPieces - 1; num >= 0; num--)
		{
			LeanTween.moveY(buildingParts[num].buildingPiece, originalPiecePositions[num].y + offsetHeight, animationDuration).setDelay(buildingParts[num].outAnimationDelay).setEase(bounceAnimCurve_OUT)
				.setIgnoreTimeScale(true)
				.setOnComplete(OnCompleteFX);
		}
	}

	private void AnimateIntoPosition()
	{
		for (int i = 0; i < numBuildingPieces; i++)
		{
			LeanTween.moveY(buildingParts[i].buildingPiece, originalPiecePositions[i].y, animationDuration).setDelay(buildingParts[i].inAnimationDelay).setEase(bounceAnimCurve_IN)
				.setIgnoreTimeScale(true)
				.setOnComplete(OnCompleteFX);
		}
	}

	private void OnCompleteFX()
	{
		if (!hasPlayedFinishedSound)
		{
			hasPlayedFinishedSound = true;
		}
		if (_onCompletedAction != null)
		{
			Debug.Log("Calling completed!");
			_onCompletedAction();
			_onCompletedAction = null;
		}
		if ((bool)OnCompleteParticleSystem && animateIn)
		{
			if (numPiecesComplete == numBuildingPieces - 1)
			{
				OnCompleteParticleSystem.Play();
			}
			else
			{
				numPiecesComplete++;
			}
		}
	}

	private void ResetSink()
	{
		base.transform.position = _rootPos;
		_wantSink = false;
	}

	private void Update()
	{
		if (_wantSink)
		{
			float num = Time.time - _sinkStartTime;
			if (num < 0.5f)
			{
				num = Easing.Ease(Easing.EaseType.EaseInCircle, num, 0.5f, 0f, 1f);
				Vector3 position = Vector3.Lerp(_rootPos, _rootPos + Vector3.down * 10f, num);
				base.transform.position = position;
			}
			else
			{
				base.transform.position = _rootPos + Vector3.down * 10f;
			}
		}
		else if (doAnimate)
		{
			KickoffBuildProcess();
			doAnimate = false;
		}
	}
}
