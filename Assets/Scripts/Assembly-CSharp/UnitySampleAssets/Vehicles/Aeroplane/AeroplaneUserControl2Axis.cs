using System;
using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

namespace UnitySampleAssets.Vehicles.Aeroplane
{
	[RequireComponent(typeof(AeroplaneController))]
	public class AeroplaneUserControl2Axis : MonoBehaviour
	{
		public float maxRollAngle = 80f;

		public float maxPitchAngle = 80f;

		private AeroplaneController aeroplane;

		private void Awake()
		{
			aeroplane = GetComponent<AeroplaneController>();
		}

		private void FixedUpdate()
		{
			float roll = CrossPlatformInputManager.GetAxis("Horizontal");
			float pitch = CrossPlatformInputManager.GetAxis("Vertical");
			bool button = CrossPlatformInputManager.GetButton("Fire1");
			float throttle = ((!button) ? 1 : (-1));
			AdjustInputForMobileControls(ref roll, ref pitch, ref throttle);
			aeroplane.Move(roll, pitch, 0f, throttle, button);
		}

		private void AdjustInputForMobileControls(ref float roll, ref float pitch, ref float throttle)
		{
			float num = roll * maxRollAngle * ((float)Math.PI / 180f);
			float num2 = pitch * maxPitchAngle * ((float)Math.PI / 180f);
			roll = Mathf.Clamp(num - aeroplane.RollAngle, -1f, 1f);
			pitch = Mathf.Clamp(num2 - aeroplane.PitchAngle, -1f, 1f);
			float num3 = throttle * 0.5f + 0.5f;
			throttle = Mathf.Clamp(num3 - aeroplane.Throttle, -1f, 1f);
		}
	}
}
