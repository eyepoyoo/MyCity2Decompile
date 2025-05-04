using UnityEngine;

namespace UnitySampleAssets.Vehicles.Aeroplane
{
	public class LandingGear : MonoBehaviour
	{
		private enum GearState
		{
			Raised = -1,
			Lowered = 1
		}

		public float raiseAtAltitude = 40f;

		public float lowerAtAltitude = 40f;

		private GearState state = GearState.Lowered;

		private Animator animator;

		private AeroplaneController plane;

		private void Start()
		{
			plane = GetComponent<AeroplaneController>();
			animator = GetComponent<Animator>();
		}

		private void Update()
		{
			if (state == GearState.Lowered && plane.Altitude > raiseAtAltitude && GetComponent<Rigidbody>().velocity.y > 0f)
			{
				state = GearState.Raised;
			}
			if (state == GearState.Raised && plane.Altitude < lowerAtAltitude && GetComponent<Rigidbody>().velocity.y < 0f)
			{
				state = GearState.Lowered;
			}
			animator.SetInteger("GearState", (int)state);
		}
	}
}
