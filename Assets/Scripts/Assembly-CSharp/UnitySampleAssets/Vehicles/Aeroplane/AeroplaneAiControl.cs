using System;
using UnityEngine;

namespace UnitySampleAssets.Vehicles.Aeroplane
{
	[RequireComponent(typeof(AeroplaneController))]
	public class AeroplaneAiControl : MonoBehaviour
	{
		[SerializeField]
		private float rollSensitivity = 0.2f;

		[SerializeField]
		private float pitchSensitivity = 0.5f;

		[SerializeField]
		private float lateralWanderDistance = 5f;

		[SerializeField]
		private float lateralWanderSpeed = 0.11f;

		[SerializeField]
		private float maxClimbAngle = 45f;

		[SerializeField]
		private float maxRollAngle = 45f;

		[SerializeField]
		private float speedEffect = 0.01f;

		[SerializeField]
		private float takeoffHeight = 20f;

		[SerializeField]
		private Transform target;

		private AeroplaneController aeroplaneController;

		private float randomPerlin;

		private bool takenOff;

		private void Awake()
		{
			aeroplaneController = GetComponent<AeroplaneController>();
			randomPerlin = UnityEngine.Random.Range(0f, 100f);
		}

		public void Reset()
		{
			takenOff = false;
		}

		private void FixedUpdate()
		{
			if (target != null)
			{
				Vector3 position = target.position + base.transform.right * (Mathf.PerlinNoise(Time.time * lateralWanderSpeed, randomPerlin) * 2f - 1f) * lateralWanderDistance;
				Vector3 vector = base.transform.InverseTransformPoint(position);
				float num = Mathf.Atan2(vector.x, vector.z);
				float value = 0f - Mathf.Atan2(vector.y, vector.z);
				value = Mathf.Clamp(value, (0f - maxClimbAngle) * ((float)Math.PI / 180f), maxClimbAngle * ((float)Math.PI / 180f));
				float num2 = value - aeroplaneController.PitchAngle;
				float num3 = num2 * pitchSensitivity;
				float num4 = Mathf.Clamp(num, (0f - maxRollAngle) * ((float)Math.PI / 180f), maxRollAngle * ((float)Math.PI / 180f));
				float num5 = 0f;
				float num6 = 0f;
				if (!takenOff)
				{
					if (aeroplaneController.Altitude > takeoffHeight)
					{
						takenOff = true;
					}
				}
				else
				{
					num5 = num;
					num6 = (0f - (aeroplaneController.RollAngle - num4)) * rollSensitivity;
				}
				float num7 = 1f + aeroplaneController.ForwardSpeed * speedEffect;
				num6 *= num7;
				num3 *= num7;
				num5 *= num7;
				aeroplaneController.Move(num6, num3, num5, 0.5f, false);
			}
			else
			{
				aeroplaneController.Move(0f, 0f, 0f, 0f, false);
			}
		}

		public void SetTarget(Transform target)
		{
			this.target = target;
		}
	}
}
