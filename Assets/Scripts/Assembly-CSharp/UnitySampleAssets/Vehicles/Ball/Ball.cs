using UnityEngine;

namespace UnitySampleAssets.Vehicles.Ball
{
	public class Ball : MonoBehaviour
	{
		private const float GroundRayLength = 1f;

		[SerializeField]
		private float movePower = 5f;

		[SerializeField]
		private bool useTorque = true;

		[SerializeField]
		private float maxAngularVelocity = 25f;

		[SerializeField]
		private float jumpPower = 2f;

		private void Start()
		{
			GetComponent<Rigidbody>().maxAngularVelocity = maxAngularVelocity;
		}

		public void Move(Vector3 moveDirection, bool jump)
		{
			if (useTorque)
			{
				GetComponent<Rigidbody>().AddTorque(new Vector3(moveDirection.z, 0f, 0f - moveDirection.x) * movePower);
			}
			else
			{
				GetComponent<Rigidbody>().AddForce(moveDirection * movePower);
			}
			if (Physics.Raycast(base.transform.position, -Vector3.up, 1f) && jump)
			{
				GetComponent<Rigidbody>().AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
			}
		}
	}
}
