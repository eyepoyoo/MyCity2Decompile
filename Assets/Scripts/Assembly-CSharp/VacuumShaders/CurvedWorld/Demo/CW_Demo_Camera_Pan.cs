using UnityEngine;

namespace VacuumShaders.CurvedWorld.Demo
{
	[AddComponentMenu("VacuumShaders/Curved World/Demo/Camera Pan")]
	public class CW_Demo_Camera_Pan : MonoBehaviour
	{
		public float moveSpeed = 1f;

		private void Start()
		{
		}

		private void Update()
		{
			if (Input.GetMouseButton(0))
			{
				base.transform.Translate(Vector3.right * (0f - Input.GetAxis("Mouse X")) * moveSpeed);
				base.transform.Translate(base.transform.up * (0f - Input.GetAxis("Mouse Y")) * moveSpeed, Space.World);
			}
			else if (Input.touchSupported && Input.touchCount == 1 && Input.touches[0].phase != TouchPhase.Moved)
			{
			}
		}
	}
}
