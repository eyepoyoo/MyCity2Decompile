using System;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAbility_Hose : SpecialAbility
{
	private struct NozzleSnapshot
	{
		public float _timestamp;

		public Vector3 _position;

		public Vector3 _forward;

		public Vector3 _velocity;

		public NozzleSnapshot(float timestamp, Vector3 position, Vector3 forward, Vector3 velocity)
		{
			_timestamp = timestamp;
			_position = position;
			_forward = forward;
			_velocity = velocity;
		}

		public void SetTo(float timestamp, Vector3 position, Vector3 forward, Vector3 velocity)
		{
			_timestamp = timestamp;
			_position = position;
			_forward = forward;
			_velocity = velocity;
		}
	}

	public delegate void DOnRaycastHit(RaycastHit hit, Vector3 velocity);

	public const float MAX_SPLASH_PARTICLES_PER_SECOND = 50f;

	public const float MAX_SPLASH_PARTICLES_PER_SECOND_INV = 0.02f;

	public const int NUM_LINE_RENDERERS_TO_INITIALLY_CACHE = 5;

	public const int TRAJ_STEPS_MAX = 100;

	public const float TRAJ_TIME_MAX = 1f;

	public const float TRAJ_TIMESTEP = 0.02f;

	public const int TRAJ_NUM_RAYCAST_POINTS = 5;

	private const float EASE_TO_TARGET_HALFLIFE = 0.1f;

	private const float UPDATE_CLOSEST_TARGET_INTERVAL = 0.2f;

	private const float VARIATION_FREQ_X = (float)Math.PI;

	private const float VARIATION_FREQ_Y = 4.3982296f;

	private const float RAYCASTS_INTERVAL = 0.1f;

	public static readonly Quaternion DEFAULT_ROTATION = Quaternion.identity;

	public Transform _pivot;

	public Transform _nozzle;

	public float _waterSpeed;

	public float _waterMass;

	public ParticleSystem _splashParticles;

	public Material _lineRendererMat;

	public float _maxAimDistance;

	public float _widthStart = 0.2f;

	public float _widthEnd = 2f;

	public float _variationAmount;

	public bool _enableHeadingRotation;

	[NonSerialized]
	public Transform _lookAt;

	public readonly int _trajectoryNumSteps = Math.Min(100, 50);

	public readonly float _trajectoryTime = Math.Min(1f, 2f);

	private readonly NozzleSnapshot[] _nozzleSnapshots = new NozzleSnapshot[1000];

	private int _currNozzleSnapshotIndex;

	private HoseStream _currStream;

	private readonly List<HoseStream> _streams = new List<HoseStream>();

	private readonly List<LineRenderer> _lineRenderers = new List<LineRenderer>();

	private float _lastSplashTime = float.NegativeInfinity;

	private Quaternion _rotation = DEFAULT_ROTATION;

	private readonly RepeatedAction _repeatedAction_UpdateClosestTarget = new RepeatedAction(0.2f);

	private readonly RepeatedAction _repeatedAction_RaycastsTimer = new RepeatedAction(0.1f);

	private float _maxAimDistanceSqrd;

	private Transform _closestTarget;

	private Vector3 _pVariationOffset
	{
		get
		{
			return new Vector3(Mathf.Sin((float)Math.PI * Time.time) * _variationAmount, Mathf.Cos(4.3982296f * Time.time) * _variationAmount);
		}
	}

	public event Action<Collider> _onHit;

	protected override void Awake()
	{
		base.Awake();
		for (int i = 0; i < 5; i++)
		{
			CreateLineRenderer();
		}
		_maxAimDistanceSqrd = _maxAimDistance * _maxAimDistance;
	}

	protected override void Update()
	{
		base.Update();
		_currNozzleSnapshotIndex = (_currNozzleSnapshotIndex + 1) % _nozzleSnapshots.Length;
		_nozzleSnapshots[_currNozzleSnapshotIndex].SetTo(Time.time, _nozzle.position, _nozzle.forward, base._pVehicle._pRigidbody.velocity);
		if (Time.timeScale != 0f)
		{
			for (int num = _streams.Count - 1; num >= 0; num--)
			{
				_streams[num].UpdateTrajectory();
			}
		}
		UpdateRotation();
		if (_repeatedAction_RaycastsTimer.Update())
		{
			DoRaycasts();
		}
	}

	private void DoRaycasts()
	{
		for (int num = _streams.Count - 1; num >= 0; num--)
		{
			_streams[num].DoRaycast(HandleRaycastHit);
		}
	}

	private void UpdateRotation()
	{
		if (_repeatedAction_UpdateClosestTarget.Update())
		{
			UpdateClosestTarget();
		}
		Transform transform = _lookAt ?? _closestTarget;
		float launchAngle = 0f - base._pVehicle.transform.eulerAngles.x;
		float y = base._pVehicle.transform.eulerAngles.y;
		if ((bool)transform)
		{
			Vector3 targ = transform.position - _pivot.position;
			Vector3 position = transform.position;
			if (_variationAmount != 0f)
			{
				float f = Mathf.Atan2(targ.x, targ.z);
				Vector3 normalized = targ.normalized;
				Vector3 vector = new Vector3(Mathf.Cos(f), 0f, 0f - Mathf.Sin(f));
				Vector3 vector2 = Vector3.Cross(normalized, vector);
				Vector3 pVariationOffset = _pVariationOffset;
				position += pVariationOffset.x * vector + pVariationOffset.y * vector2 + pVariationOffset.z * normalized;
			}
			MathHelper.GetElevation(_pivot.position.y, position.y, targ.MagnitudeXZ(), _waterSpeed, out launchAngle);
			if (_enableHeadingRotation)
			{
				y = Mathf.Atan2(position.x - _pivot.position.x, position.z - _pivot.position.z) * 57.29578f;
			}
		}
		_pivot.transform.rotation = MathHelper.EaseTowardsRotation(_pivot.transform.rotation, Quaternion.Euler(0f - launchAngle, y, base._pVehicle.transform.eulerAngles.z), 0.1f, Time.deltaTime);
	}

	private void UpdateClosestTarget()
	{
		MinigameObjective closestObjective = MinigameController._pInstance._pMinigame.GetClosestObjective(IsValidTarget);
		_closestTarget = ((!closestObjective) ? null : closestObjective.transform);
	}

	private bool IsValidTarget(MinigameObjective target)
	{
		return MathHelper.DistXZSqrd(base.transform.position, target.transform.position) < _maxAimDistanceSqrd && Vector3.Dot(base.transform.forward, target.transform.position - base.transform.position) > 0f && (bool)target.GetComponent<Vehicle>();
	}

	protected override void OnStarted()
	{
		base.OnStarted();
		StartStream();
		if ((bool)SoundFacade._pInstance)
		{
			SoundFacade._pInstance.PlayLoopingSFX("AttachmentHose", base.transform, 0f);
		}
	}

	protected override void OnEnded()
	{
		base.OnEnded();
		_currStream.End();
		if ((bool)SoundFacade._pInstance)
		{
			SoundFacade._pInstance.StopAllLoopingAudioByGroupName("AttachmentHose");
		}
	}

	private void HandleRaycastHit(RaycastHit hit, Vector3 velocity)
	{
		HitObject(hit, velocity, 0.1f);
		if (Time.time - _lastSplashTime > 0.02f)
		{
			Splash(hit.point, hit.normal);
		}
		if (this._onHit != null)
		{
			this._onHit(hit.collider);
		}
	}

	private void HitObject(RaycastHit hit, Vector3 velocity, float dt)
	{
		Vehicle_Car component = hit.transform.GetComponent<Vehicle_Car>();
		if ((bool)component)
		{
			component.MultiplyWheelsSidewaysFriction(0.1f, 0.25f);
			component.MultiplyWheelsForwardFriction(0.1f, 0.25f);
		}
		Rigidbody rigidbody = hit.rigidbody;
		float num = 1f;
		ReplaceOnCollide component2 = hit.transform.GetComponent<ReplaceOnCollide>();
		if ((bool)component2)
		{
			Transform transform = component2.Replace();
			if ((bool)transform)
			{
				rigidbody = transform.GetComponent<Rigidbody>() ?? rigidbody;
				num = 5f;
			}
		}
		if ((bool)rigidbody)
		{
			Vector3 point = hit.point;
			point.y = rigidbody.worldCenterOfMass.y;
			rigidbody.AddForceAtPosition(velocity * _waterMass * num * dt, point, ForceMode.VelocityChange);
		}
	}

	private void Splash(Vector3 pos, Vector3 vel)
	{
		_splashParticles.transform.position = pos;
		_splashParticles.transform.forward = vel;
		_splashParticles.Emit(1);
		_lastSplashTime = Time.time;
	}

	private LineRenderer CreateLineRenderer()
	{
		GameObject gameObject = new GameObject("Line Renderer");
		gameObject.layer = LayerMask.NameToLayer("WaterVehicleSpecialRender2");
		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = _lineRendererMat;
		lineRenderer.transform.parent = base.transform;
		ReturnLineRenderer(lineRenderer);
		return lineRenderer;
	}

	public LineRenderer GetLineRenderer()
	{
		LineRenderer lineRenderer = ((_lineRenderers.Count <= 0) ? CreateLineRenderer() : _lineRenderers[0]);
		_lineRenderers.Remove(lineRenderer);
		lineRenderer.enabled = true;
		return lineRenderer;
	}

	public void ReturnLineRenderer(LineRenderer lr)
	{
		_lineRenderers.Add(lr);
		lr.enabled = false;
	}

	private void StartStream()
	{
		_currStream = new HoseStream(this);
		_streams.Add(_currStream);
	}

	public void DestroyStream(HoseStream stream)
	{
		if (_currStream == stream)
		{
			_currStream = null;
		}
		_streams.Remove(stream);
		stream.Destroy();
	}

	public void GetTrajectory(float timeStart, float timeEnd, out Vector3[] points, out Vector3[] velocities)
	{
		if (timeEnd == timeStart)
		{
			points = new Vector3[0];
			velocities = new Vector3[0];
			return;
		}
		int num = 1 + Mathf.FloorToInt((timeEnd - timeStart) / 0.02f);
		if ((timeEnd - timeStart) % 0.02f == 0f)
		{
			num--;
		}
		num = Mathf.Min(num, _trajectoryNumSteps - 1);
		points = new Vector3[num + 1];
		velocities = new Vector3[num + 1];
		int returnedIndexL = _currNozzleSnapshotIndex;
		float num2 = timeStart;
		NozzleSnapshot nozzleSnapshotInterpolated;
		for (int i = 0; i < num; i++)
		{
			nozzleSnapshotInterpolated = GetNozzleSnapshotInterpolated(Time.time - num2, returnedIndexL, out returnedIndexL);
			points[i] = GetTrajectoryPoint(nozzleSnapshotInterpolated._position, nozzleSnapshotInterpolated._forward * _waterSpeed + nozzleSnapshotInterpolated._velocity, num2);
			velocities[i] = GetTrajectoryVelocity(nozzleSnapshotInterpolated._position, nozzleSnapshotInterpolated._forward * _waterSpeed + nozzleSnapshotInterpolated._velocity, num2);
			num2 += 0.02f;
		}
		nozzleSnapshotInterpolated = GetNozzleSnapshotInterpolated(Time.time - timeEnd, returnedIndexL, out returnedIndexL);
		points[points.Length - 1] = GetTrajectoryPoint(nozzleSnapshotInterpolated._position, nozzleSnapshotInterpolated._forward * _waterSpeed + nozzleSnapshotInterpolated._velocity, timeEnd);
		velocities[velocities.Length - 1] = GetTrajectoryVelocity(nozzleSnapshotInterpolated._position, nozzleSnapshotInterpolated._forward * _waterSpeed + nozzleSnapshotInterpolated._velocity, timeEnd);
	}

	private Vector3 GetTrajectoryPoint(Vector3 initPos, Vector3 initVel, float time)
	{
		return initPos + new Vector3(initVel.x, initVel.y + 0.5f * Physics.gravity.y * time, initVel.z) * time;
	}

	private Vector3 GetTrajectoryVelocity(Vector3 initPos, Vector3 initVel, float time)
	{
		return new Vector3(initVel.x, initVel.y + Physics.gravity.y * time, initVel.z);
	}

	private NozzleSnapshot GetFirstNozzleSnapshotBeforeTime(float time, int fromIndex, out int returnedIndex)
	{
		for (int num = 0; num > -_nozzleSnapshots.Length; num--)
		{
			int num2 = MathHelper.Wrap(fromIndex + num, _nozzleSnapshots.Length);
			NozzleSnapshot result = _nozzleSnapshots[num2];
			if (result._timestamp <= time)
			{
				returnedIndex = num2;
				return result;
			}
		}
		Debug.LogError("Couldn't find a snapshot before or on time " + time);
		returnedIndex = -1;
		return default(NozzleSnapshot);
	}

	private NozzleSnapshot GetNozzleSnapshotInterpolated(float time, int fromIndexL, out int returnedIndexL)
	{
		NozzleSnapshot firstNozzleSnapshotBeforeTime = GetFirstNozzleSnapshotBeforeTime(time, fromIndexL, out returnedIndexL);
		if (returnedIndexL == -1)
		{
			Debug.LogError("Couldn't get an interpolated snapshot at time " + time);
			return default(NozzleSnapshot);
		}
		NozzleSnapshot nozzleSnapshot = _nozzleSnapshots[(returnedIndexL + 1) % _nozzleSnapshots.Length];
		float t = Mathf.InverseLerp(firstNozzleSnapshotBeforeTime._timestamp, nozzleSnapshot._timestamp, time);
		return new NozzleSnapshot(time, Vector3.Lerp(firstNozzleSnapshotBeforeTime._position, nozzleSnapshot._position, t), Vector3.Slerp(firstNozzleSnapshotBeforeTime._forward, nozzleSnapshot._forward, t), Vector3.Lerp(firstNozzleSnapshotBeforeTime._velocity, nozzleSnapshot._velocity, t));
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		foreach (HoseStream stream in _streams)
		{
			for (int i = 0; i < stream._trajectoryPoints.Length - 1; i++)
			{
				Vector3 vector = stream._trajectoryPoints[i];
				Vector3 vector2 = stream._trajectoryPoints[i + 1];
				if (!(vector == Vector3.zero) && !(vector2 == Vector3.zero))
				{
					Gizmos.DrawLine(vector, vector2);
					Gizmos.DrawSphere(vector, 0.1f);
					Gizmos.DrawSphere(vector2, 0.1f);
				}
			}
		}
	}
}
