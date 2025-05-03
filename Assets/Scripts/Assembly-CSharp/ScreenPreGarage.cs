using System.Text;

public class ScreenPreGarage : ScreenBase
{
	public UIPanel backingPanel;

	public UILabel bodiesCount;

	public UILabel wheelsCount;

	public UILabel attachmentsCount;

	public UILabel accessoriesCount;

	public UIWidget bodiesNewIcon;

	public UIWidget wheelsNewIcon;

	public UIWidget attachmentsNewIcon;

	public UIWidget accessoriesNewIcon;

	private bool _tweenOut;

	private StringBuilder _dummySB = new StringBuilder();

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		GlobalInGameData._pHasSeenGarageTutorial = true;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		int num9 = 0;
		int num10 = 0;
		int num11 = 0;
		int num12 = 0;
		int num13 = VehiclePartManager._pInstance.allVehiclePartProperties.Length;
		for (int i = 0; i < num13; i++)
		{
			bool partIsAvailable = VehiclePartManager._pInstance.allVehiclePartProperties[i].partIsAvailable;
			bool flag = VehiclePartManager._pInstance.allVehiclePartProperties[i]._pHasBeenUnlocked && partIsAvailable;
			bool flag2 = !GlobalInGameData.IsPartNew(VehiclePartManager._pInstance.allVehiclePartProperties[i].uniqueID);
			if (!partIsAvailable)
			{
				continue;
			}
			switch (VehiclePartManager._pInstance.allVehiclePartProperties[i].slotType)
			{
			case VehiclePart.EPART_SLOT_TYPE.ACCESSORY:
				num10++;
				if (flag)
				{
					num11++;
				}
				if (flag && !flag2)
				{
					num12++;
				}
				break;
			case VehiclePart.EPART_SLOT_TYPE.ATTACHMENT:
				num7++;
				if (flag)
				{
					num8++;
				}
				if (flag && !flag2)
				{
					num9++;
				}
				break;
			case VehiclePart.EPART_SLOT_TYPE.BODY:
				num4++;
				if (flag)
				{
					num5++;
				}
				if (flag && !flag2)
				{
					num6++;
				}
				break;
			case VehiclePart.EPART_SLOT_TYPE.WHEEL:
				num++;
				if (flag)
				{
					num2++;
				}
				if (flag && !flag2)
				{
					num3++;
				}
				break;
			}
		}
		_dummySB.Length = 0;
		_dummySB.Append(num2);
		_dummySB.Append("/");
		_dummySB.Append(num);
		wheelsCount.text = _dummySB.ToString();
		wheelsNewIcon.gameObject.SetActive(num3 > 0);
		_dummySB.Length = 0;
		_dummySB.Append(num5);
		_dummySB.Append("/");
		_dummySB.Append(num4);
		bodiesCount.text = _dummySB.ToString();
		bodiesNewIcon.gameObject.SetActive(num6 > 0);
		_dummySB.Length = 0;
		_dummySB.Append(num8);
		_dummySB.Append("/");
		_dummySB.Append(num7);
		attachmentsCount.text = _dummySB.ToString();
		attachmentsNewIcon.gameObject.SetActive(num9 > 0);
		_dummySB.Length = 0;
		_dummySB.Append(num11);
		_dummySB.Append("/");
		_dummySB.Append(num10);
		accessoriesCount.text = _dummySB.ToString();
		accessoriesNewIcon.gameObject.SetActive(num12 > 0);
		_tweenOut = false;
		if (CameraHUB._pExists && CameraHUB._pInstance._pCameraControllable)
		{
			CameraHUB._pInstance._pCameraControllable = false;
		}
	}

	public void OnBodies()
	{
		GlobalInGameData._pGaragePartType = VehiclePart.EPART_SLOT_TYPE.BODY;
		Navigate("Garage");
	}

	public void OnWheels()
	{
		GlobalInGameData._pGaragePartType = VehiclePart.EPART_SLOT_TYPE.WHEEL;
		Navigate("Garage");
	}

	public void OnAttachments()
	{
		GlobalInGameData._pGaragePartType = VehiclePart.EPART_SLOT_TYPE.ATTACHMENT;
		Navigate("Garage");
	}

	public void OnAccessories()
	{
		GlobalInGameData._pGaragePartType = VehiclePart.EPART_SLOT_TYPE.ACCESSORY;
		Navigate("Garage");
	}

	public void OnBack()
	{
		_tweenOut = true;
		CameraHUB._pInstance._pCameraControllable = true;
		GlobalInGameData._pGaragePartType = VehiclePart.EPART_SLOT_TYPE.INVALID;
		TrackableIcon.RefreshAllVisibility();
		Navigate("Back");
	}

	protected override void OnScreenExitComplete()
	{
		base.OnScreenExitComplete();
	}

	protected override void Update()
	{
		base.Update();
		backingPanel.SetDirty();
		if (CameraHUB._pExists && CameraHUB._pInstance._pCameraControllable && !_tweenOut)
		{
			CameraHUB._pInstance._pCameraControllable = false;
		}
	}
}
