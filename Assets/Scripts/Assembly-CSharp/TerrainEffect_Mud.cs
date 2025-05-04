using UnityEngine;

[DisallowMultipleComponent]
public class TerrainEffect_Mud : TerrainEffect
{
	private const float DRAG = 4f;

	private float _initDrag;

	protected override float _pRemoveDuration
	{
		get
		{
			return 0f;
		}
	}

	protected override string _pSoundId
	{
		get
		{
			return "DirtStay";
		}
	}

	protected override void Awake()
	{
		base.Awake();
		Rigidbody component = GetComponent<Rigidbody>();
		_initDrag = component.drag;
		WheelCollider componentInChildren = GetComponentInChildren<WheelCollider>();
		float num = ((!componentInChildren) ? 1f : Mathf.Min(1f, 0.42f / componentInChildren.radius));
		GetComponent<Rigidbody>().drag = 4f * component.mass * num;
		if (VehicleController_Player.IsPlayer(base.transform))
		{
			MinigameController._pInstance._pCamera.AddSpeedScaleRequest(this, 1f - 0.2f * num);
		}
	}

	public override void RemoveInstant()
	{
		GetComponent<Rigidbody>().drag = _initDrag;
		if (VehicleController_Player.IsPlayer(base.transform))
		{
			MinigameController._pInstance._pCamera.RemoveSpeedScaleRequest(this);
		}
		base.RemoveInstant();
	}
}
