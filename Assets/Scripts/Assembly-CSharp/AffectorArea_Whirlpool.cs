using UnityEngine;

public class AffectorArea_Whirlpool : AffectorArea
{
	public float _force;

	public float _radius;

	public float _inwardForceScale = 3f;

	public float _torqueScale = 2f;

	protected override void ApplyForces(Rigidbody rb)
	{
		if (VehicleController_Player.IsPlayer(rb) && (!VehicleController_Player._pInstance._enabled || (bool)VehicleController_Player._pInstance._pVehicle._pShouldCoast))
		{
			return;
		}
		Vector3 vector = new Vector3(rb.transform.position.x - base.transform.position.x, 0f, rb.transform.position.z - base.transform.position.z);
		float f = Mathf.Atan2(vector.x, vector.z);
		float num = 1f - Mathf.Min(1f, vector.magnitude / _radius);
		float num2 = _force * num / rb.mass;
		Vehicle component = rb.GetComponent<Vehicle>();
		if ((bool)component)
		{
			num2 = _force * num;
			switch (component._size)
			{
			case Vehicle.ESize.Small:
				num2 *= 1f;
				break;
			case Vehicle.ESize.Medium:
				num2 *= 0.8f;
				break;
			case Vehicle.ESize.Large:
				num2 *= 0.2f;
				break;
			}
		}
		rb.AddForce(new Vector3(Mathf.Cos(f), 0f, 0f - Mathf.Sin(f)) * num2, ForceMode.Acceleration);
		rb.AddTorque(Vector3.up * _torqueScale * num2, ForceMode.Acceleration);
		rb.AddForce(-vector * _inwardForceScale * num2, ForceMode.Acceleration);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		GizmosPlus.drawCircle(base.transform.position, _radius);
	}

	protected override void OnBodyEnter(Rigidbody rb)
	{
		base.OnBodyEnter(rb);
		Vehicle component = rb.GetComponent<Vehicle>();
		if ((bool)component)
		{
			EmoticonSystem.OnVehicleEnteredWhirlpool(component);
		}
	}
}
