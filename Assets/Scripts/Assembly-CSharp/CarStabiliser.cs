using UnityEngine;

[RequireComponent(typeof(Vehicle_Car))]
public class CarStabiliser : MonoBehaviour
{
	private const float HEIGHT_THRESHOLD_SOFT = 0.2f;

	private const float HEIGHT_THRESHOLD_HARD = 0.5f;

	private const float HEIGHT_THRESHOLD_RANGE = 0.3f;

	private const float FORCE_MAX = 100f;

	private Vehicle_Car _car;

	private void Awake()
	{
		_car = GetComponent<Vehicle_Car>();
	}

	private void FixedUpdate()
	{
		float num = Mathf.Max(0f, (_car._pDistOffGround - 0.2f) / 0.3f);
		if (num > 1f)
		{
			_car._pRigidbody.velocity = new Vector3(_car._pRigidbody.velocity.x, 0f, _car._pRigidbody.velocity.z);
			base.transform.position = new Vector3(base.transform.position.x, _car._pCentreOffsetFromBottom, base.transform.position.z);
		}
		else if (num > 0f)
		{
			_car._pRigidbody.AddForce(Vector3.down * num * 100f, ForceMode.Acceleration);
		}
	}
}
