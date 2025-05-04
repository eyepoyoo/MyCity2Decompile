using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(ReplaceOnCollide))]
public class Collateral : MonoBehaviour
{
	public float _impactForceMulti = 1f;

	public int _numStudsToSpawn = 5;

	public int _studsRadius = 2;

	public ReplaceOnCollide _replaceOnCollide;

	public static event Action<Collateral, Collider> _onCollateralDestroyed;

	private void Awake()
	{
		_replaceOnCollide._onReplace += OnCollide;
	}

	private void OnCollide(Collider other)
	{
		if ((bool)other)
		{
			Vehicle componentInParent = other.GetComponentInParent<Vehicle>();
			if ((bool)componentInParent)
			{
				BumpVehicle(componentInParent);
			}
		}
		if ((bool)SoundFacade._pInstance)
		{
			SoundFacade._pInstance.PlayOneShotSFX("CollateralDestroyed", base.transform.position, 0f);
		}
		MinigameController._pInstance.SpawnStudsRing(base.transform.position, _numStudsToSpawn, _studsRadius, base.transform.parent);
		if (Collateral._onCollateralDestroyed != null)
		{
			Collateral._onCollateralDestroyed(this, other);
		}
	}

	private void BumpVehicle(Vehicle vehicle)
	{
		Vector3 vector = new Vector3(vehicle.transform.position.x - base.transform.position.x, 0f, vehicle.transform.position.z - base.transform.position.z);
		Vector3 normalized = vector.normalized;
		float num = Mathf.InverseLerp(1f, -1f, Vector3.Dot(normalized, vehicle._pRigidbody.velocity.normalized)) * vehicle._pRigidbody.velocity.magnitude;
		vehicle._pRigidbody.AddForce(normalized * num * _impactForceMulti, ForceMode.Impulse);
	}
}
