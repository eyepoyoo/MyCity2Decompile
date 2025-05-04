using UnityEngine;

namespace UnitySampleAssets.Effects
{
	public class FireLight : MonoBehaviour
	{
		private float rnd;

		private bool burning = true;

		private void Start()
		{
			rnd = Random.value * 100f;
		}

		private void Update()
		{
			if (burning)
			{
				GetComponent<Light>().intensity = 2f * Mathf.PerlinNoise(rnd + Time.time, rnd + 1f + Time.time * 1f);
				float x = Mathf.PerlinNoise(rnd + Time.time * 2f, rnd + 1f + Time.time * 2f) - 0.5f;
				float y = Mathf.PerlinNoise(rnd + 2f + Time.time * 2f, rnd + 3f + Time.time * 2f) - 0.5f;
				float z = Mathf.PerlinNoise(rnd + 4f + Time.time * 2f, rnd + 5f + Time.time * 2f) - 0.5f;
				base.transform.localPosition = Vector3.up + new Vector3(x, y, z) * 1f;
			}
		}

		public void Extinguish()
		{
			burning = false;
			GetComponent<Light>().enabled = false;
		}
	}
}
