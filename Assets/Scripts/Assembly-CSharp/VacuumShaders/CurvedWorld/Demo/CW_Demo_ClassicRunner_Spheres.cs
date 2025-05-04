using UnityEngine;

namespace VacuumShaders.CurvedWorld.Demo
{
	[RequireComponent(typeof(Rigidbody))]
	[AddComponentMenu("VacuumShaders/Curved World/Demo/Classic Runner/Sphere")]
	public class CW_Demo_ClassicRunner_Spheres : MonoBehaviour
	{
		public Vector3 direction;

		private Rigidbody rigidBody;

		private void Start()
		{
			rigidBody = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			rigidBody.MovePosition(base.transform.position + direction * Time.deltaTime);
			if (base.transform.position.z < -20f || base.transform.position.y < -10f)
			{
				base.transform.position = new Vector3(Random.Range(-2f, 2f), 10f, 75f);
			}
		}
	}
}
