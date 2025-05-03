using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FallingHazard : MonoBehaviour, IFastPoolItem
{
	private const float DESTROY_TIMEOUT = 5f;

	public MarkerPoint _dangerTelegraphPrefab;

	public Transform _shockwavePrefab;

	public float _radius = 10f;

	public float _maxForce = 10f;

	public int _maxStudsToTake = 10;

	public ParticleSystem _particleSystem;

	public Renderer _shellL;

	public Renderer _shellR;

	public static Action<FallingHazard> _onHitPlayer;

	public static Action<FallingHazard> _onMadePlayerDropPickup;

	private Rigidbody _rigidbody;

	private Collider _collider;

	private FastPool _pool;

	private FastPool _shockwavePool;

	private FastPool _telegraphPool;

	private float _destroyTimeoutTimer;

	private MarkerPoint _dangerTelegraph;

	private float _minDistFromPlayer;

	private float _maxDistFromPlayer;

	private float _maxAngleInFrontOfPlayer;

	private void Update()
	{
		if ((_destroyTimeoutTimer += Time.deltaTime) > 5f)
		{
			_pool.FastDestroy(this);
		}
	}

	public void Launch(float minDistFromPlayer = 0f, float maxDistFromPlayer = 0f, float maxAngleInFrontOfPlayer = 360f)
	{
		_minDistFromPlayer = minDistFromPlayer;
		_maxDistFromPlayer = maxDistFromPlayer;
		_maxAngleInFrontOfPlayer = maxAngleInFrontOfPlayer;
		_rigidbody.useGravity = false;
		_collider.enabled = false;
		base.transform.TweenToPos(base.transform.position + Vector3.up * 50f, 1f, OnReachedApex, Easing.EaseType.Linear, true, false, 0f);
	}

	private void OnReachedApex(Tween t)
	{
		_rigidbody.useGravity = true;
		_rigidbody.velocity = Vector3.down * 10f;
		_collider.enabled = true;
		bool flag = VehicleController_Player._pInstance._pVehicle is Vehicle_Air;
		float num = ((!flag) ? 50 : 100);
		base.transform.position = VehicleController_Player._pInstance.transform.position + Vector3.up * num + MathHelper.GetRandomPointOnSector(MinigameController._pInstance._pCamera.transform.eulerAngles.y, _maxAngleInFrontOfPlayer, _maxDistFromPlayer, _minDistFromPlayer);
		base.transform.position += MathHelper.ClipVector3(VehicleController_Player._pInstance._pVehicle._pRigidbody.velocity, Vector3.up) * num * ((!flag) ? 0.035f : 0.03f);
		if (_dangerTelegraphPrefab != null)
		{
			_dangerTelegraph = _telegraphPool.FastInstantiate<MarkerPoint>();
			_dangerTelegraph.transform.position = base.transform.position;
			_dangerTelegraph.HideInstant();
			_dangerTelegraph._pDoShow = true;
			_dangerTelegraph._pRadius = _radius;
			_dangerTelegraph._pIsDanger = true;
			RaycastHit hitInfo;
			if (Physics.Raycast(base.transform.position, Vector3.down, out hitInfo, num + 50f, LayerMask.GetMask("Geometry")))
			{
				_dangerTelegraph.transform.position = hitInfo.point;
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		Vector3 point = collision.contacts[0].point;
		float magnitude = (point - VehicleController_Player._pInstance.transform.position).magnitude;
		float num = 1f - Mathf.Min(1f, magnitude / _radius);
		_collider.enabled = false;
		if (num > 0f)
		{
			HitPlayer((!VehicleController_Player.IsPlayer(collision.rigidbody)) ? num : 1f, point);
		}
		if (_dangerTelegraph != null)
		{
			_dangerTelegraph._pDoShow = false;
			_telegraphPool.FastDestroy(_dangerTelegraph);
			_dangerTelegraph = null;
		}
		if ((bool)SoundFacade._pInstance)
		{
			SoundFacade._pInstance.PlayOneShotSFX("LavaRockImpact", base.transform.position, 0f);
		}
		ExplodeAndDestroy();
	}

	private void HitPlayer(float impactScale, Vector3 contactPoint)
	{
		if ((bool)MinigameCutscene._pIsAnyActive)
		{
			return;
		}
		if (impactScale == 1f)
		{
			Invoke("DirectHitPlayer", (!(VehicleController_Player._pInstance._pVehicle is Vehicle_Air)) ? 0.1f : 0f);
		}
		else
		{
			Vector3 force = (contactPoint - VehicleController_Player._pInstance.transform.position).normalized * _maxForce;
			force.y = Mathf.Max(10f, force.y);
			force *= impactScale;
			VehicleController_Player._pInstance._pVehicle._pRigidbody.AddForceAtPosition(force, contactPoint, ForceMode.Impulse);
		}
		MinigameController._pInstance.DropStuds(Mathf.CeilToInt((float)_maxStudsToTake * impactScale), 5f);
		if (impactScale > 0.3f)
		{
			if (VehicleController_Player._pInstance._pVehicle.DropPickupable(true) && _onMadePlayerDropPickup != null)
			{
				_onMadePlayerDropPickup(this);
			}
			if (_onHitPlayer != null)
			{
				_onHitPlayer(this);
			}
		}
	}

	private void DirectHitPlayer()
	{
		Vector3 torque;
		Vector3 force;
		Vector3 position;
		if (VehicleController_Player._pInstance._pVehicle is Vehicle_Air)
		{
			torque = Vector3.zero;
			force = Vector3.down * _maxForce * 2f;
			position = base.transform.position;
		}
		else
		{
			torque = UnityEngine.Random.onUnitSphere * _maxForce * 5f;
			force = Vector3.up * _maxForce * 1.5f;
			position = VehicleController_Player._pInstance._pVehicle._pRigidbody.worldCenterOfMass;
		}
		VehicleController_Player._pInstance._pVehicle._pRigidbody.AddForceAtPosition(force, position, ForceMode.Impulse);
		VehicleController_Player._pInstance._pVehicle._pRigidbody.AddTorque(torque, ForceMode.Impulse);
		EmoticonSystem.OnLavaBombHitPlayer();
	}

	private void ExplodeAndDestroy()
	{
		_shellL.enabled = false;
		_shellR.enabled = false;
		_particleSystem.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
		_particleSystem.Play();
		Transform transform = _shockwavePool.FastInstantiate<Transform>();
		transform.localScale = new Vector3(_radius, 1f, _radius);
		transform.position = base.transform.position;
		Invoke("DestroyDelayed", 2f);
	}

	private void DestroyDelayed()
	{
		_pool.FastDestroy(this);
	}

	public void OnFastInstantiate(FastPool pool)
	{
		_destroyTimeoutTimer = 0f;
		_rigidbody.velocity = Vector3.zero;
		_rigidbody.angularVelocity = UnityEngine.Random.onUnitSphere * 5f;
		_particleSystem.Simulate(0f, true, true);
		_particleSystem.Clear();
		_shellL.enabled = true;
		_shellR.enabled = true;
	}

	public void OnFastDestroy()
	{
		if (_dangerTelegraph != null)
		{
			_telegraphPool.FastDestroy(_dangerTelegraph);
			_dangerTelegraph = null;
		}
	}

	public void OnCloned(FastPool pool)
	{
		_rigidbody = GetComponent<Rigidbody>();
		_collider = GetComponent<Collider>();
		_pool = pool;
		_shockwavePool = FastPoolManager.GetPool(_shockwavePrefab);
		_shockwavePool.NotificationType = PoolItemNotificationType.Interface;
		_shockwavePool.ParentOnCache = true;
		_telegraphPool = FastPoolManager.GetPool(_dangerTelegraphPrefab);
		_telegraphPool.NotificationType = PoolItemNotificationType.None;
		_telegraphPool.ParentOnCache = true;
	}
}
