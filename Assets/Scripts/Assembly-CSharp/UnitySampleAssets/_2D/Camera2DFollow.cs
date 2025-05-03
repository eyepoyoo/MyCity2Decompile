using UnityEngine;

namespace UnitySampleAssets._2D
{
	public class Camera2DFollow : MonoBehaviour
	{
		public Transform target;

		public float damping = 1f;

		public float lookAheadFactor = 3f;

		public float lookAheadReturnSpeed = 0.5f;

		public float lookAheadMoveThreshold = 0.1f;

		private float offsetZ;

		private Vector3 lastTargetPosition;

		private Vector3 currentVelocity;

		private Vector3 lookAheadPos;

		private void Start()
		{
			lastTargetPosition = target.position;
			offsetZ = (base.transform.position - target.position).z;
			base.transform.parent = null;
		}

		private void Update()
		{
			float x = (target.position - lastTargetPosition).x;
			if (Mathf.Abs(x) > lookAheadMoveThreshold)
			{
				lookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(x);
			}
			else
			{
				lookAheadPos = Vector3.MoveTowards(lookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
			}
			Vector3 vector = target.position + lookAheadPos + Vector3.forward * offsetZ;
			Vector3 position = Vector3.SmoothDamp(base.transform.position, vector, ref currentVelocity, damping);
			base.transform.position = position;
			lastTargetPosition = target.position;
		}
	}
}
