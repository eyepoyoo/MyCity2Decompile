using UnityEngine;

namespace UnitySampleAssets.Cameras
{
	[ExecuteInEditMode]
	public class AutoCam : PivotBasedCameraRig
	{
		[SerializeField]
		private float moveSpeed = 3f;

		[SerializeField]
		private float turnSpeed = 1f;

		[SerializeField]
		private float rollSpeed = 0.2f;

		[SerializeField]
		private bool followVelocity;

		[SerializeField]
		private bool followTilt = true;

		[SerializeField]
		private float spinTurnLimit = 90f;

		[SerializeField]
		private float targetVelocityLowerLimit = 4f;

		[SerializeField]
		private float smoothTurnTime = 0.2f;

		private float lastFlatAngle;

		private float currentTurnAmount;

		private float turnSpeedVelocityChange;

		private Vector3 rollUp = Vector3.up;

		protected override void FollowTarget(float deltaTime)
		{
			if (!(deltaTime > 0f) || target == null)
			{
				return;
			}
			Vector3 forward = target.forward;
			Vector3 up = target.up;
			if (followVelocity && Application.isPlaying)
			{
				if (target.GetComponent<Rigidbody>().velocity.magnitude > targetVelocityLowerLimit)
				{
					forward = target.GetComponent<Rigidbody>().velocity.normalized;
					up = Vector3.up;
				}
				else
				{
					up = Vector3.up;
				}
				currentTurnAmount = Mathf.SmoothDamp(currentTurnAmount, 1f, ref turnSpeedVelocityChange, smoothTurnTime);
			}
			else
			{
				float num = Mathf.Atan2(forward.x, forward.z) * 57.29578f;
				if (spinTurnLimit > 0f)
				{
					float value = Mathf.Abs(Mathf.DeltaAngle(lastFlatAngle, num)) / deltaTime;
					float num2 = Mathf.InverseLerp(spinTurnLimit, spinTurnLimit * 0.75f, value);
					float smoothTime = ((!(currentTurnAmount > num2)) ? 1f : 0.1f);
					if (Application.isPlaying)
					{
						currentTurnAmount = Mathf.SmoothDamp(currentTurnAmount, num2, ref turnSpeedVelocityChange, smoothTime);
					}
					else
					{
						currentTurnAmount = num2;
					}
				}
				else
				{
					currentTurnAmount = 1f;
				}
				lastFlatAngle = num;
			}
			base.transform.position = Vector3.Lerp(base.transform.position, target.position, deltaTime * moveSpeed);
			if (!followTilt)
			{
				forward.y = 0f;
				if (forward.sqrMagnitude < float.Epsilon)
				{
					forward = base.transform.forward;
				}
			}
			Quaternion b = Quaternion.LookRotation(forward, rollUp);
			rollUp = ((!(rollSpeed > 0f)) ? Vector3.up : Vector3.Slerp(rollUp, up, rollSpeed * deltaTime));
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b, turnSpeed * currentTurnAmount * deltaTime);
		}
	}
}
