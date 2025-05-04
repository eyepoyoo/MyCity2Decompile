using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BumpAwayFromVehicleCollision : MonoBehaviour
{
	private Rigidbody _rigidbody;

	private Vehicle_Car _car;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_car = GetComponent<Vehicle_Car>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		OnCollisionStay(collision);
	}

	private void OnCollisionStay(Collision collision)
	{
		BumpAway(collision, 1f, 0.5f, ForceMode.VelocityChange);
	}

	private void BumpAway(Collision collision, float force, float torque, ForceMode forceMode)
	{
		Vehicle component = collision.transform.GetComponent<Vehicle>();
		if ((bool)component)
		{
			if ((bool)_car)
			{
				_car.MultiplyWheelsSidewaysFriction(0f, 0.25f);
				_car.MultiplyWheelsForwardFriction(0f, 0.25f);
			}
			int num = ((Vector3.Dot(component.transform.right, base.transform.position - component.transform.position) > 0f) ? 1 : (-1));
			_rigidbody.AddTorque(Vector3.up * num * torque, forceMode);
			_rigidbody.AddForceAtPosition((component.transform.forward + component.transform.right * num) * force, _rigidbody.worldCenterOfMass, forceMode);
		}
	}
}
