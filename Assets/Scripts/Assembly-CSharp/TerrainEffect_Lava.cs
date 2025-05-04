using UnityEngine;

[DisallowMultipleComponent]
public class TerrainEffect_Lava : TerrainEffect
{
	private const float DRAG = 4f;

	private float _initDrag;

	private readonly Transform[] _steams = new Transform[4];

	private bool _doAddDragToVehicle;

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
		for (int i = 0; i < 4; i++)
		{
			_steams[i] = Object.Instantiate(Resources.Load<Transform>("LavaSteam"));
		}
		_doAddDragToVehicle = !_vehicle.NegatesObstacle(VehiclePart.EObstacleToNegate.LavaThrough);
		if (_doAddDragToVehicle)
		{
			Rigidbody component = GetComponent<Rigidbody>();
			_initDrag = component.drag;
			WheelCollider componentInChildren = GetComponentInChildren<WheelCollider>();
			float num = 1f;
			GetComponent<Rigidbody>().drag = 4f * component.mass * num;
			if (VehicleController_Player.IsPlayer(base.transform))
			{
				MinigameController._pInstance._pCamera.AddSpeedScaleRequest(this, 1f - 0.2f * num);
			}
		}
	}

	protected override void Update()
	{
		base.Update();
		_steams[0].position = _vehicle._pConnectors._wheelFL.transform.position - base.transform.right * 0.5f;
		_steams[1].position = _vehicle._pConnectors._wheelFR.transform.position + base.transform.right * 0.5f;
		_steams[2].position = _vehicle._pConnectors._wheelBL.transform.position - base.transform.right * 0.5f;
		_steams[3].position = _vehicle._pConnectors._wheelBR.transform.position + base.transform.right * 0.5f;
	}

	public override void RemoveInstant()
	{
		if (_doAddDragToVehicle)
		{
			GetComponent<Rigidbody>().drag = _initDrag;
			if (VehicleController_Player.IsPlayer(base.transform))
			{
				MinigameController._pInstance._pCamera.RemoveSpeedScaleRequest(this);
			}
		}
		for (int i = 0; i < 4; i++)
		{
			ParticleSystem.EmissionModule emission = _steams[i].GetComponentInChildren<ParticleSystem>().emission;
			emission.enabled = false;
			Object.Destroy(_steams[i].gameObject, 1f);
		}
		base.RemoveInstant();
	}
}
