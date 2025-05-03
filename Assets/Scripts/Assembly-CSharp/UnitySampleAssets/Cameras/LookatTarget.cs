using UnityEngine;

namespace UnitySampleAssets.Cameras
{
	public class LookatTarget : AbstractTargetFollower
	{
		[SerializeField]
		private Vector2 rotationRange;

		[SerializeField]
		private float followSpeed = 1f;

		private Vector3 followAngles;

		protected Vector3 followVelocity;

		private Quaternion originalRotation;

		protected override void Start()
		{
			base.Start();
			originalRotation = base.transform.localRotation;
		}

		protected override void FollowTarget(float deltaTime)
		{
			base.transform.localRotation = originalRotation;
			Vector3 vector = base.transform.InverseTransformPoint(target.position);
			float value = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
			value = Mathf.Clamp(value, (0f - rotationRange.y) * 0.5f, rotationRange.y * 0.5f);
			base.transform.localRotation = originalRotation * Quaternion.Euler(0f, value, 0f);
			vector = base.transform.InverseTransformPoint(target.position);
			float value2 = Mathf.Atan2(vector.y, vector.z) * 57.29578f;
			value2 = Mathf.Clamp(value2, (0f - rotationRange.x) * 0.5f, rotationRange.x * 0.5f);
			followAngles = Vector3.SmoothDamp(target: new Vector3(followAngles.x + Mathf.DeltaAngle(followAngles.x, value2), followAngles.y + Mathf.DeltaAngle(followAngles.y, value)), current: followAngles, currentVelocity: ref followVelocity, smoothTime: followSpeed);
			base.transform.localRotation = originalRotation * Quaternion.Euler(0f - followAngles.x, followAngles.y, 0f);
		}
	}
}
