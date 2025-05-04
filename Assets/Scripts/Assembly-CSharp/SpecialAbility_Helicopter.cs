using UnityEngine;

public class SpecialAbility_Helicopter : SpecialAbility
{
	private const float PROPELLER_SPEED_IDLE = 180f;

	private const float PROPELLER_SPEED_ACTIVE = 1080f;

	public Transform _propeller;

	public float _force;

	private float _propellerSpeedCurrent = 180f;

	protected override void Update()
	{
		base.Update();
		if ((bool)_propeller)
		{
			MathHelper.EaseTowards(ref _propellerSpeedCurrent, (!base._pIsInUse) ? 180f : 1080f, 0.2f, Time.deltaTime);
			_propeller.transform.localEulerAngles += Vector3.up * _propellerSpeedCurrent * Time.deltaTime;
		}
	}

	private void FixedUpdate()
	{
		if (base._pIsInUse)
		{
			base._pVehicle._pRigidbody.AddForceAtPosition(Vector3.up * _force, base._pVehicle._pRigidbody.worldCenterOfMass + base._pVehicle.transform.up, ForceMode.Force);
			base._pVehicle._pRigidbody.velocity = new Vector3(base._pVehicle._pRigidbody.velocity.x, base._pVehicle._pRigidbody.velocity.y * Mathf.InverseLerp(100f, 10f, base.transform.position.y), base._pVehicle._pRigidbody.velocity.z);
		}
	}

	protected override void OnStarted()
	{
		base.OnStarted();
		if (base._pVehicle._pIsGrounded)
		{
			if (base._pVehicle is Vehicle_Car)
			{
				((Vehicle_Car)base._pVehicle).ZeroWheelSuspensionSpringDampers(0.1f);
			}
			base._pVehicle._pRigidbody.AddForceAtPosition(Vector3.up * 6f, base._pVehicle._pRigidbody.worldCenterOfMass + base._pVehicle.transform.up, ForceMode.Impulse);
		}
		if ((bool)SoundFacade._pInstance)
		{
			SoundFacade._pInstance.PlayLoopingSFX("AttachmentHelicopter", base.transform, 0f);
		}
	}

	protected override void OnEnded()
	{
		base.OnEnded();
		if ((bool)SoundFacade._pInstance)
		{
			SoundFacade._pInstance.StopAllLoopingAudioByGroupName("AttachmentHelicopter");
		}
	}
}
