using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

namespace UnitySampleAssets.Utility
{
	public class SimpleMouseRotator : MonoBehaviour
	{
		public Vector2 rotationRange = new Vector3(70f, 70f);

		public float rotationSpeed = 10f;

		public float dampingTime = 0.2f;

		public bool autoZeroVerticalOnMobile = true;

		public bool autoZeroHorizontalOnMobile;

		public bool relative = true;

		private Vector3 targetAngles;

		private Vector3 followAngles;

		private Vector3 followVelocity;

		private Quaternion originalRotation;

		private void Start()
		{
			originalRotation = base.transform.localRotation;
		}

		private void Update()
		{
			base.transform.localRotation = originalRotation;
			float num = 0f;
			float num2 = 0f;
			if (relative)
			{
				num = CrossPlatformInputManager.GetAxis("Mouse X");
				num2 = CrossPlatformInputManager.GetAxis("Mouse Y");
				if (targetAngles.y > 180f)
				{
					targetAngles.y -= 360f;
					followAngles.y -= 360f;
				}
				if (targetAngles.x > 180f)
				{
					targetAngles.x -= 360f;
					followAngles.x -= 360f;
				}
				if (targetAngles.y < -180f)
				{
					targetAngles.y += 360f;
					followAngles.y += 360f;
				}
				if (targetAngles.x < -180f)
				{
					targetAngles.x += 360f;
					followAngles.x += 360f;
				}
				if (autoZeroHorizontalOnMobile)
				{
					targetAngles.y = Mathf.Lerp((0f - rotationRange.y) * 0.5f, rotationRange.y * 0.5f, num * 0.5f + 0.5f);
				}
				else
				{
					targetAngles.y += num * rotationSpeed;
				}
				if (autoZeroVerticalOnMobile)
				{
					targetAngles.x = Mathf.Lerp((0f - rotationRange.x) * 0.5f, rotationRange.x * 0.5f, num2 * 0.5f + 0.5f);
				}
				else
				{
					targetAngles.x += num2 * rotationSpeed;
				}
				targetAngles.y = Mathf.Clamp(targetAngles.y, (0f - rotationRange.y) * 0.5f, rotationRange.y * 0.5f);
				targetAngles.x = Mathf.Clamp(targetAngles.x, (0f - rotationRange.x) * 0.5f, rotationRange.x * 0.5f);
			}
			else
			{
				num = Input.mousePosition.x;
				num2 = Input.mousePosition.y;
				targetAngles.y = Mathf.Lerp((0f - rotationRange.y) * 0.5f, rotationRange.y * 0.5f, num / (float)Screen.width);
				targetAngles.x = Mathf.Lerp((0f - rotationRange.x) * 0.5f, rotationRange.x * 0.5f, num2 / (float)Screen.height);
			}
			followAngles = Vector3.SmoothDamp(followAngles, targetAngles, ref followVelocity, dampingTime);
			base.transform.localRotation = originalRotation * Quaternion.Euler(0f - followAngles.x, followAngles.y, 0f);
		}
	}
}
