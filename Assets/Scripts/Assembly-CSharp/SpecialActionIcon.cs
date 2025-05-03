using System;
using UnityEngine;
using VacuumShaders.CurvedWorld;

public class SpecialActionIcon : TrackableIcon
{
	public enum EACTION_TYPE
	{
		INVALID = 0,
		SOCIAL = 1,
		GARAGE = 2,
		TO_CITY = 3,
		TO_VOLCANO = 4,
		TO_AIRPORT = 5
	}

	public EACTION_TYPE actionType;

	[SerializeField]
	private GameObject _newIcon;

	private Vector3 _rootPos;

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
			return region.ToString() + "_" + actionType;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		_rootPos = base.transform.position;
		if (actionType == EACTION_TYPE.GARAGE && !(_newIcon == null))
		{
			GlobalInGameData._onPartUnlocked = (Action<VehiclePart.EUNIQUE_ID>)Delegate.Combine(GlobalInGameData._onPartUnlocked, new Action<VehiclePart.EUNIQUE_ID>(OnPartUnlocked));
		}
	}

	private void OnDestroy()
	{
		if (actionType == EACTION_TYPE.GARAGE && !(_newIcon == null))
		{
			GlobalInGameData._onPartUnlocked = (Action<VehiclePart.EUNIQUE_ID>)Delegate.Remove(GlobalInGameData._onPartUnlocked, new Action<VehiclePart.EUNIQUE_ID>(OnPartUnlocked));
		}
	}

	private void Update()
	{
		if (CameraHUB._pExists)
		{
			base.transform.forward = CameraHUB._pInstance._pCameraRef.transform.forward;
			if (!(CurvedWorld_Controller.get == null))
			{
				base.transform.position = CurvedWorld_Controller.get.TransformPoint(_rootPos);
			}
		}
	}

	private void OnPartUnlocked(VehiclePart.EUNIQUE_ID part)
	{
		RefreshFX();
	}

	protected override void RefreshFX()
	{
		base.RefreshFX();
		if (actionType == EACTION_TYPE.GARAGE && !(_newIcon == null))
		{
			_newIcon.SetActive(GlobalInGameData.IsAnyAvaliblePartNew());
		}
	}
}
