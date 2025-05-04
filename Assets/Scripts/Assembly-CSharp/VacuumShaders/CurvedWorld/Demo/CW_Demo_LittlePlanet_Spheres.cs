using UnityEngine;

namespace VacuumShaders.CurvedWorld.Demo
{
	[RequireComponent(typeof(Rigidbody))]
	[AddComponentMenu("VacuumShaders/Curved World/Demo/Little Planet/Spheres")]
	public class CW_Demo_LittlePlanet_Spheres : MonoBehaviour
	{
		private Vector3 origPosition;

		private void Start()
		{
			origPosition = base.transform.position;
			origPosition.y = 5f;
		}

		private void FixedUpdate()
		{
			if (base.transform.position.y < -5f)
			{
				base.transform.position = origPosition;
			}
		}
	}
}
