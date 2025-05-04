using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

namespace UnitySampleAssets.Cameras
{
	public class FreeLookCam : PivotBasedCameraRig
	{
		private const float LookDistance = 100f;

		[SerializeField]
		private float moveSpeed = 1f;

		[SerializeField]
		[Range(0f, 10f)]
		private float turnSpeed = 1.5f;

		[SerializeField]
		private float turnSmoothing = 0.1f;

		[SerializeField]
		private float tiltMax = 75f;

		[SerializeField]
		private float tiltMin = 45f;

		[SerializeField]
		private bool lockCursor;

		[SerializeField]
		private bool verticalAutoReturn;

		private float lookAngle;

		private float tiltAngle;

		private float smoothX;

		private float smoothY;

		private float smoothXvelocity;

		private float smoothYvelocity;

		protected override void Awake()
		{
			base.Awake();
			Screen.lockCursor = lockCursor;
		}

		protected void Update()
		{
			HandleRotationMovement();
			if (lockCursor && Input.GetMouseButtonUp(0))
			{
				Screen.lockCursor = lockCursor;
			}
		}

		private void OnDisable()
		{
			Screen.lockCursor = false;
		}

		protected override void FollowTarget(float deltaTime)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, target.position, deltaTime * moveSpeed);
		}

		private void HandleRotationMovement()
		{
			float axis = CrossPlatformInputManager.GetAxis("Mouse X");
			float axis2 = CrossPlatformInputManager.GetAxis("Mouse Y");
			if (turnSmoothing > 0f)
			{
				smoothX = Mathf.SmoothDamp(smoothX, axis, ref smoothXvelocity, turnSmoothing);
				smoothY = Mathf.SmoothDamp(smoothY, axis2, ref smoothYvelocity, turnSmoothing);
			}
			else
			{
				smoothX = axis;
				smoothY = axis2;
			}
			lookAngle += smoothX * turnSpeed;
			base.transform.rotation = Quaternion.Euler(0f, lookAngle, 0f);
			if (verticalAutoReturn)
			{
				tiltAngle = ((!(axis2 > 0f)) ? Mathf.Lerp(0f, tiltMax, 0f - smoothY) : Mathf.Lerp(0f, 0f - tiltMin, smoothY));
			}
			else
			{
				tiltAngle -= smoothY * turnSpeed;
				tiltAngle = Mathf.Clamp(tiltAngle, 0f - tiltMin, tiltMax);
			}
			pivot.localRotation = Quaternion.Euler(tiltAngle, 0f, 0f);
		}
	}
}
