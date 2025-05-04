using UnityEngine;

public class TurbulenceZone : PlayerTrigger
{
	protected override void OnTriggerEnter(Collider other)
	{
		Vehicle_Air component = other.GetComponent<Vehicle_Air>();
		bool flag = VehicleController_Player.IsPlayer(other.transform);
		if (!(component == null) && flag)
		{
			if (component._pTurbulenceTolerance >= 1f)
			{
				_rumbleCamera = false;
			}
			base.OnTriggerEnter(other);
			if (component != null)
			{
				component._pTurbulence = true;
			}
		}
	}

	protected override void OnTriggerExit(Collider other)
	{
		Vehicle_Air component = other.GetComponent<Vehicle_Air>();
		bool flag = VehicleController_Player.IsPlayer(other.transform);
		if (!(component == null) && flag)
		{
			base.OnTriggerExit(other);
			component._pTurbulence = false;
		}
	}
}
