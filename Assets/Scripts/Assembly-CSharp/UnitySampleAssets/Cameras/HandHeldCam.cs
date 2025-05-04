using UnityEngine;

namespace UnitySampleAssets.Cameras
{
	public class HandHeldCam : LookatTarget
	{
		[SerializeField]
		private float swaySpeed = 0.5f;

		[SerializeField]
		private float baseSwayAmount = 0.5f;

		[SerializeField]
		private float trackingSwayAmount = 0.5f;

		[SerializeField]
		[Range(-1f, 1f)]
		private float trackingBias;

		protected override void FollowTarget(float deltaTime)
		{
			base.FollowTarget(deltaTime);
			float num = Mathf.PerlinNoise(0f, Time.time * swaySpeed) - 0.5f;
			float num2 = Mathf.PerlinNoise(0f, Time.time * swaySpeed + 100f) - 0.5f;
			num *= baseSwayAmount;
			num2 *= baseSwayAmount;
			float num3 = Mathf.PerlinNoise(0f, Time.time * swaySpeed) - 0.5f + trackingBias;
			float num4 = Mathf.PerlinNoise(0f, Time.time * swaySpeed + 100f) - 0.5f + trackingBias;
			num3 *= (0f - trackingSwayAmount) * followVelocity.x;
			num4 *= trackingSwayAmount * followVelocity.y;
			base.transform.Rotate(num + num3, num2 + num4, 0f);
		}
	}
}
