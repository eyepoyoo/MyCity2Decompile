using System;
using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

namespace UnitySampleAssets.Characters.FirstPerson
{
	[Serializable]
	public class MouseLook
	{
		public float XSensitivity = 15f;

		public float YSensitivity = 15f;

		public float MinimumX = -360f;

		public float MaximumX = 360f;

		public float MinimumY = -90f;

		public float MaximumY = 90f;

		private float xvel;

		private float yvel;

		public bool smooth;

		public float smoothtime;

		public Vector2 UnClamped(float x, float y)
		{
			Vector2 result = default(Vector2);
			result.x = y + CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
			result.y = x + CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;
			if (smooth)
			{
				result.x = Mathf.SmoothDamp(y, result.x, ref xvel, smoothtime);
				result.y = Mathf.SmoothDamp(x, result.y, ref yvel, smoothtime);
			}
			return result;
		}

		public Vector2 Clamped(float x, float y)
		{
			Vector2 result = default(Vector2);
			result.x = y + CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
			result.y = x + CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;
			result.x = Mathf.Clamp(result.x, MinimumX, MaximumX);
			result.y = Mathf.Clamp(result.y, MinimumY, MaximumY);
			if (smooth)
			{
				result.x = Mathf.SmoothDamp(y, result.x, ref xvel, smoothtime);
				result.y = Mathf.SmoothDamp(x, result.y, ref yvel, smoothtime);
			}
			return result;
		}
	}
}
