using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
	public VehicleConstructor _default;

	public bool _spawnOnAwake = true;

	public bool _pHasSpawned { get; private set; }

	private void Awake()
	{
		if (_spawnOnAwake)
		{
			SpawnPlayer();
		}
	}

	public void SpawnPlayer()
	{
		if (_pHasSpawned)
		{
			Debug.LogError("Already spawned!");
			return;
		}
		_pHasSpawned = true;
		if (!VehicleController_Player._pInstance)
		{
			if (!_default)
			{
				Debug.LogError("Existing player vehicle not found in scene and no default supplied!");
				return;
			}
			Vehicle vehicle = _default.ConstructVehicle(true);
			vehicle.gameObject.AddComponent<VehicleController_Player>();
		}
		VehicleController_Player._pInstance.gameObject.name = "Player";
		VehicleController_Player._pInstance._pVehicle.Reset();
		if (VehicleController_Player._pInstance._pVehicle is Vehicle_Car && VehicleController_Player._pInstance._pVehicle._specialAbility is SpecialAbility_Lifter)
		{
			VehicleController_Player._pInstance.transform.position += Vector3.up;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		GizmosPlus.drawSpawnPoint(base.transform, 1f);
	}
}
