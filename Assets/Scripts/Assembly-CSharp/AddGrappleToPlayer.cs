using UnityEngine;

public class AddGrappleToPlayer : MonoBehaviour
{
	public Grapple _grapplePrefab;

	private void OnPlayerSpawned()
	{
		Grapple grapple = Object.Instantiate(_grapplePrefab);
		grapple.AssignTo(VehicleController_Player._pInstance._pVehicle);
	}
}
