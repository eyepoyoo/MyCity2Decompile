using UnityEngine;
using VacuumShaders.CurvedWorld;

public class MinigameIcon : TrackableIcon
{
	public MinigameManager.EMINIGAME_TYPE minigameType;

	private Vector3 _rootPos;

	public Vector3 _rotationMask;

	public Vector3 _finalPosShift;

	public float bobRate = 1f;

	public float bobAmount;

	public MeshRenderer[] vehicleUnlockIcons;

	private Vector3 _baseScale;

	public override Vector3 _pRootPos
	{
		get
		{
			return _rootPos;
		}
	}

	public override string _pUniqueId
	{
		get
		{
			return region.ToString() + "_" + minigameType;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		_baseScale = base.transform.localScale;
		_rootPos = base.transform.position;
		MinigameManager.MinigameData minigameDataFromType = MinigameManager._pInstance.GetMinigameDataFromType(minigameType);
		if (vehicleUnlockIcons != null)
		{
			bool flag = minigameDataFromType._pNumTimesCompleted >= 0;
			bool flag2 = minigameDataFromType._pNumTimesCompleted >= 1;
			bool flag3 = minigameDataFromType._pNumTimesCompleted >= 2;
			vehicleUnlockIcons[0].material.SetFloat("_Alpha", (!flag) ? 0.25f : 1f);
			vehicleUnlockIcons[1].material.SetFloat("_Alpha", (!flag2) ? 0.25f : 1f);
			vehicleUnlockIcons[2].material.SetFloat("_Alpha", (!flag3) ? 0.25f : 1f);
		}
	}

	private void Update()
	{
		if (!CameraHUB._pExists)
		{
			return;
		}
		base.transform.forward = CameraHUB._pInstance._pCameraRef.transform.forward;
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		localEulerAngles.x *= _rotationMask.x;
		localEulerAngles.y *= _rotationMask.y;
		localEulerAngles.z *= _rotationMask.z;
		base.transform.localEulerAngles = localEulerAngles;
		if (!(CurvedWorld_Controller.get == null))
		{
			base.transform.position = CurvedWorld_Controller.get.TransformPoint(_rootPos);
			base.transform.position += Vector3.up * Mathf.Sin(Time.time * bobRate) * bobAmount;
			base.transform.position += _finalPosShift;
			if (CameraHUB._pInstance._pCurFocusType == CameraHUB.EFocusType.PARTIAL_PROGRESS || CameraHUB._pInstance._pCurFocusType == CameraHUB.EFocusType.SCENARIO_COMPLETE)
			{
				base.transform.localScale = Vector3.MoveTowards(base.transform.localScale, Vector3.zero, Time.deltaTime * 45f);
			}
			else if (base.transform.localScale != _baseScale)
			{
				base.transform.localScale = Vector3.MoveTowards(base.transform.localScale, _baseScale, Time.deltaTime * 45f);
			}
		}
	}
}
