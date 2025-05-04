using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BouncePlayerOffOnCollide : MonoBehaviour
{
	public bool _spin90Degrees;

	public int _numStudsToDrop;

	public float _studsRadius = 5f;

	private void OnCollisionEnter(Collision collision)
	{
		if (base.enabled && VehicleController_Player.IsPlayer(collision.collider))
		{
			VehicleController_Player._pInstance._pVehicle.BounceAwayFromImpact(-collision.contacts[0].normal, _spin90Degrees);
			MinigameController._pInstance.DropStuds(_numStudsToDrop, _studsRadius);
		}
	}
}
