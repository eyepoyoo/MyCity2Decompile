using UnityEngine;

namespace UnitySampleAssets.Vehicles.Car
{
	public class BrakeLight : MonoBehaviour
	{
		public Vehicle_Car car;

		private void Update()
		{
			GetComponent<Renderer>().enabled = car.BrakeInput > 0f;
		}
	}
}
