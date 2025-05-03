using UnityEngine;

public class SpecialAbility_Jet : SpecialAbility
{
	private const float PROPELLER_SPEED_IDLE = 180f;

	private const float PROPELLER_SPEED_ACTIVE = 1080f;

	private const float PROPELLER_SPEED_HALF_LIFE_UP = 0.2f;

	private const float PROPELLER_SPEED_HALF_LIFE_DOWN = 0.5f;

	private const float MAX_SPEED = 40f;

	public Transform _propeller;

	private float _propellerSpeedCurrent = 180f;

	protected override void Update()
	{
		base.Update();
		if ((bool)_propeller)
		{
			MathHelper.EaseTowards(ref _propellerSpeedCurrent, (!base._pIsInUse) ? 180f : 1080f, (!base._pIsInUse) ? 0.5f : 0.2f, Time.deltaTime);
			_propeller.transform.localEulerAngles += Vector3.forward * _propellerSpeedCurrent * Time.deltaTime;
		}
	}

	private void FixedUpdate()
	{
		if (base._pIsInUse)
		{
			float num = 40f - base._pVehicle._pRigidbody.velocity.magnitude;
			if (num > 0f)
			{
				base._pVehicle._pRigidbody.AddForceAtPosition(base._pVehicle.transform.forward * num * (base._pVehicle._pRigidbody.drag + 1f), base._pVehicle._pRigidbody.worldCenterOfMass, ForceMode.Force);
			}
		}
	}

	protected override void OnStarted()
	{
		if (base._pVehicle is Vehicle_Car)
		{
			((Vehicle_Car)base._pVehicle).MultiplyWheelsForwardFriction(0f, 0f);
		}
		if (VehicleController_Player.IsPlayer(base._pVehicle))
		{
			MinigameController._pInstance._pCamera.AddSpeedScaleRequest(this, 1.3f);
		}
		if ((bool)SoundFacade._pInstance)
		{
			SoundFacade._pInstance.PlayLoopingSFX("AttachmentJet", base.transform, 0f);
		}
	}

	protected override void OnEnded()
	{
		if (base._pVehicle is Vehicle_Car)
		{
			((Vehicle_Car)base._pVehicle).RestoreWheelsForwardFriction();
		}
		if (VehicleController_Player.IsPlayer(base._pVehicle))
		{
			MinigameController._pInstance._pCamera.RemoveSpeedScaleRequest(this);
		}
		if ((bool)SoundFacade._pInstance)
		{
			SoundFacade._pInstance.StopAllLoopingAudioByGroupName("AttachmentJet");
		}
	}

	private void BoatSpecialRendererPartReplaced(BoatSpecialRender.ReplacedPart replacedPart)
	{
		if (replacedPart._original == _propeller)
		{
			Transform transform = new GameObject("propeller (boat special renderer container)").transform;
			transform.parent = _propeller.parent;
			transform.localPosition = _propeller.localPosition;
			transform.localRotation = _propeller.localRotation;
			replacedPart._upper.parent = transform;
			replacedPart._lower.parent = transform;
			_propeller = transform;
		}
	}
}
