using UnityEngine;

namespace VacuumShaders.CurvedWorld.Demo
{
	[AddComponentMenu("VacuumShaders/Curved World/Demo/Follow")]
	[ExecuteInEditMode]
	public class CW_Demo_Follow : MonoBehaviour
	{
		public Transform parent;

		public bool followX;

		public bool followY;

		public bool followZ;

		private void Start()
		{
			if (parent == null)
			{
				parent = base.transform.parent;
			}
		}

		private void Update()
		{
			UpdatePosition();
		}

		private void LateUpdate()
		{
			UpdatePosition();
		}

		private void UpdatePosition()
		{
			if (parent != null && CurvedWorld_Controller.get != null)
			{
				Vector3 position = CurvedWorld_Controller.get.TransformPoint(parent.position);
				if (!followX)
				{
					position.x = base.transform.position.x;
				}
				if (!followY)
				{
					position.y = base.transform.position.y;
				}
				if (!followZ)
				{
					position.z = base.transform.position.z;
				}
				base.transform.position = position;
			}
		}
	}
}
