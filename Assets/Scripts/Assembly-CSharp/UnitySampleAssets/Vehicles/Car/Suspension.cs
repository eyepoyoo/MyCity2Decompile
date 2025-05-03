using UnityEngine;

namespace UnitySampleAssets.Vehicles.Car
{
	public class Suspension : MonoBehaviour
	{
		public Wheel wheel;

		private Vector3 originalPosition;

		private void Start()
		{
			originalPosition = base.transform.localPosition;
		}

		private void Update()
		{
			base.transform.localPosition = originalPosition + wheel.suspensionSpringPos * base.transform.up;
		}
	}
}
