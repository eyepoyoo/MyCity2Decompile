using UnityEngine;

public class SpecialAbility_Siren : SpecialAbility
{
	private const float MAX_SPEED = 30f;

	public Transform _lightPrefab;

	public Transform _lightHolderL;

	public Transform _lightHolderR;

	private ParticleSystem[] particleSystems;

	protected override void Awake()
	{
		base.Awake();
		if (!(VehicleBuilder._pInstance != null))
		{
			CreateLights();
			ChangeObjectActivation(false);
		}
	}

	private void CreateLights()
	{
		if (_lightPrefab == null)
		{
			return;
		}
		Transform transform = Object.Instantiate(_lightPrefab);
		transform.parent = _lightHolderL;
		transform.transform.localPosition = Vector3.zero;
		transform.transform.localRotation = Quaternion.identity;
		if ((bool)_lightHolderR)
		{
			Transform transform2 = Object.Instantiate(_lightPrefab);
			transform2.parent = _lightHolderR;
			transform2.transform.localPosition = Vector3.zero;
			transform2.transform.localRotation = Quaternion.identity;
			ParticleSystem[] componentsInChildren = transform2.GetComponentsInChildren<ParticleSystem>();
			foreach (ParticleSystem particleSystem in componentsInChildren)
			{
				particleSystem.startDelay = particleSystem.startLifetime / 2f;
			}
		}
		particleSystems = GetComponentsInChildren<ParticleSystem>();
	}

	protected override void OnStarted()
	{
		base.OnStarted();
		ChangeObjectActivation(true);
		if (base._pVehicle is Vehicle_Car)
		{
			((Vehicle_Car)base._pVehicle).MultiplyWheelsForwardFriction(0f, 0f);
		}
		if (VehicleController_Player.IsPlayer(base._pVehicle))
		{
			MinigameController._pInstance._pCamera.AddSpeedScaleRequest(this, 1.15f);
		}
		if ((bool)SoundFacade._pInstance)
		{
			AudioSource audioSource = SoundFacade._pInstance.PlayLoopingSFX("AttachmentSiren", base.transform, 0f);
			if (GetComponent<VehiclePart>().uniqueID == VehiclePart.EUNIQUE_ID.ACCSESORY_BUGGY_SIREN)
			{
				audioSource.pitch *= 1.5f;
			}
		}
	}

	protected override void OnEnded()
	{
		ChangeObjectActivation(false);
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
			SoundFacade._pInstance.StopAllLoopingAudioByGroupName("AttachmentSiren");
		}
	}

	private void ChangeObjectActivation(bool doActivate)
	{
		if (particleSystems == null)
		{
			return;
		}
		for (int num = particleSystems.Length - 1; num >= 0; num--)
		{
			ParticleSystem particleSystem = particleSystems[num];
			ParticleSystem.EmissionModule emission = particleSystem.emission;
			emission.enabled = doActivate;
			if (doActivate)
			{
				particleSystem.time = particleSystem.startDelay;
			}
			else
			{
				particleSystem.Clear();
			}
		}
	}

	private void FixedUpdate()
	{
		if (base._pIsInUse)
		{
			float num = 30f - base._pVehicle._pRigidbody.velocity.magnitude;
			if (num > 0f)
			{
				base._pVehicle._pRigidbody.AddForceAtPosition(base._pVehicle.transform.forward * num * (base._pVehicle._pRigidbody.drag + 1f), base._pVehicle._pRigidbody.worldCenterOfMass, ForceMode.Force);
			}
		}
	}
}
