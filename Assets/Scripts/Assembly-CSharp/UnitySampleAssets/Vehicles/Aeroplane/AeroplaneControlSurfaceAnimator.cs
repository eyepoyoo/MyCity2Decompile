using System;
using UnityEngine;

namespace UnitySampleAssets.Vehicles.Aeroplane
{
	public class AeroplaneControlSurfaceAnimator : MonoBehaviour
	{
		[Serializable]
		public class ControlSurface
		{
			public enum Type
			{
				Aileron = 0,
				Elevator = 1,
				Rudder = 2,
				RuddervatorNegative = 3,
				RuddervatorPositive = 4
			}

			public Transform transform;

			public float amount;

			public Type type;

			[HideInInspector]
			public Quaternion originalLocalRotation;
		}

		[SerializeField]
		private float smoothing = 5f;

		[SerializeField]
		private ControlSurface[] controlSurfaces;

		private AeroplaneController plane;

		private void Start()
		{
			plane = GetComponent<AeroplaneController>();
			ControlSurface[] array = controlSurfaces;
			foreach (ControlSurface controlSurface in array)
			{
				controlSurface.originalLocalRotation = controlSurface.transform.localRotation;
			}
		}

		private void Update()
		{
			ControlSurface[] array = controlSurfaces;
			foreach (ControlSurface controlSurface in array)
			{
				switch (controlSurface.type)
				{
				case ControlSurface.Type.Aileron:
				{
					Quaternion rotation5 = Quaternion.Euler(controlSurface.amount * plane.RollInput, 0f, 0f);
					RotateSurface(controlSurface, rotation5);
					break;
				}
				case ControlSurface.Type.Elevator:
				{
					Quaternion rotation4 = Quaternion.Euler(controlSurface.amount * (0f - plane.PitchInput), 0f, 0f);
					RotateSurface(controlSurface, rotation4);
					break;
				}
				case ControlSurface.Type.Rudder:
				{
					Quaternion rotation3 = Quaternion.Euler(0f, controlSurface.amount * plane.YawInput, 0f);
					RotateSurface(controlSurface, rotation3);
					break;
				}
				case ControlSurface.Type.RuddervatorPositive:
				{
					float num2 = plane.YawInput + plane.PitchInput;
					Quaternion rotation2 = Quaternion.Euler(0f, 0f, controlSurface.amount * num2);
					RotateSurface(controlSurface, rotation2);
					break;
				}
				case ControlSurface.Type.RuddervatorNegative:
				{
					float num = plane.YawInput - plane.PitchInput;
					Quaternion rotation = Quaternion.Euler(0f, 0f, controlSurface.amount * num);
					RotateSurface(controlSurface, rotation);
					break;
				}
				}
			}
		}

		private void RotateSurface(ControlSurface surface, Quaternion rotation)
		{
			Quaternion b = surface.originalLocalRotation * rotation;
			surface.transform.localRotation = Quaternion.Slerp(surface.transform.localRotation, b, smoothing * Time.deltaTime);
		}
	}
}
