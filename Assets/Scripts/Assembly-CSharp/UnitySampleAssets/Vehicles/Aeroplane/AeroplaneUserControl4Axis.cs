using System;
using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

namespace UnitySampleAssets.Vehicles.Aeroplane
{
	[RequireComponent(typeof(AeroplaneController))]
	public class AeroplaneUserControl4Axis : MonoBehaviour
	{
		public float maxRollAngle = 80f;

		public float maxPitchAngle = 80f;

		private AeroplaneController aeroplane;

		private float throttle;

		private bool airBrakes;

		private float yaw;

		private void Awake()
		{
			aeroplane = GetComponent<AeroplaneController>();
		}

		private void FixedUpdate()
		{
			float roll = CrossPlatformInputManager.GetAxis("Mouse X");
			float pitch = CrossPlatformInputManager.GetAxis("Mouse Y");
			airBrakes = CrossPlatformInputManager.GetButton("Fire1");
			yaw = CrossPlatformInputManager.GetAxis("Horizontal");
			throttle = CrossPlatformInputManager.GetAxis("Vertical");
			AdjustInputForMobileControls(ref roll, ref pitch, ref throttle);
			aeroplane.Move(roll, pitch, yaw, throttle, airBrakes);
		}

		private void AdjustInputForMobileControls(ref float roll, ref float pitch, ref float throttle)
		{
			float num = roll * maxRollAngle * ((float)Math.PI / 180f);
			float num2 = pitch * maxPitchAngle * ((float)Math.PI / 180f);
			roll = Mathf.Clamp(num - aeroplane.RollAngle, -1f, 1f);
			pitch = Mathf.Clamp(num2 - aeroplane.PitchAngle, -1f, 1f);
		}
	}
}
