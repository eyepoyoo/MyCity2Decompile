using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AffectorArea_LavaJet : AffectorArea
{
	private const float DROP_STUDS_INTERVAL = 5f;

	private const float FORCE_OFFSET_FREQ_X = (float)Math.PI * 3f;

	private const float FORCE_OFFSET_FREQ_Z = 16.13522f;

	private const float ENABLE_COLLIDERS_OFFSET = 1.5f;

	private const float MARKER_PREENABLE_TIME = 1f;

	public float _timeEnabled = 5f;

	public float _timeDisabled = 5f;

	public int _numStudsToDrop;

	public MarkerPoint _markerPrefab;

	public Color32 _markerColor;

	public static Action<AffectorArea_LavaJet> _onMadePlayerDropPickup;

	private bool _enabled = true;

	private ParticleSystem _particleSystem;

	private float _enabledTimer;

	private float _cycleTime;

	private float _lastDropStudsTime = float.NegativeInfinity;

	private MarkerPoint _marker;

	private bool _pEnabled
	{
		get
		{
			return _enabled;
		}
		set
		{
			if (value != _enabled)
			{
				_enabled = value;
				ParticleSystem.EmissionModule emission = _particleSystem.emission;
				emission.enabled = _enabled;
			}
		}
	}

	private Vector3 _pForcePointOffset
	{
		get
		{
			return new Vector3(Mathf.Sin(Time.time * ((float)Math.PI * 3f)), 0f, Mathf.Cos(Time.time * 16.13522f)) * 8f;
		}
	}

	private void Awake()
	{
		_particleSystem = GetComponentInChildren<ParticleSystem>();
		_cycleTime = _timeEnabled + _timeDisabled;
		_enabledTimer = UnityEngine.Random.value * _cycleTime;
		_marker = UnityEngine.Object.Instantiate(_markerPrefab);
		_marker.transform.parent = base.transform;
		_marker.transform.position = base.transform.position + Vector3.up;
		_marker._pRadius = 5f;
		_marker._doFlicker = true;
		_marker._pIsDanger = true;
		_marker._tintColor = _markerColor;
		_enabled = true;
		_pEnabled = false;
	}

	protected override void FixedUpdate()
	{
		_enabledTimer += Time.fixedDeltaTime;
		_marker._pDoShow = (_enabledTimer + 1f) % _cycleTime > _cycleTime - _timeEnabled && !_pEnabled;
		_pEnabled = _enabledTimer % _cycleTime > _cycleTime - _timeEnabled;
		if ((_enabledTimer - 1.5f) % _cycleTime > _cycleTime - _timeEnabled)
		{
			ApplyForcesToBodies();
		}
	}

	protected override void ApplyForces(Rigidbody rb)
	{
		Vehicle component = rb.GetComponent<Vehicle>();
		if ((bool)component)
		{
			float num = ((!(component is Vehicle_Air)) ? 2 : 5);
			rb.AddForceAtPosition(Vector3.up * 6f * num, MathHelper.ClipVector3(base.transform.position, Vector3.up) + _pForcePointOffset + Vector3.up * rb.transform.position.y, ForceMode.Force);
			if (_numStudsToDrop > 0 && VehicleController_Player.IsPlayer(rb) && Time.time > _lastDropStudsTime + 5f)
			{
				_lastDropStudsTime = Time.time;
				MinigameController._pInstance.DropStuds(_numStudsToDrop, 5f);
			}
			if (!MinigameCutscene._pIsAnyActive && component.DropPickupable(true) && _onMadePlayerDropPickup != null)
			{
				_onMadePlayerDropPickup(this);
			}
		}
	}

	protected override void OnBodyEnter(Rigidbody rb)
	{
		base.OnBodyEnter(rb);
		Vehicle component = rb.GetComponent<Vehicle>();
		if ((bool)component)
		{
			EmoticonSystem.OnVehicleEnteredLavaJet(component);
		}
	}
}
