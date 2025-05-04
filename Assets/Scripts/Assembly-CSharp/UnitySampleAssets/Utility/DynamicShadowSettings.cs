using UnityEngine;

namespace UnitySampleAssets.Utility
{
	public class DynamicShadowSettings : MonoBehaviour
	{
		public Light sunLight;

		public float minHeight = 10f;

		public float minShadowDistance = 80f;

		public float minShadowBias = 1f;

		public float maxHeight = 1000f;

		public float maxShadowDistance = 10000f;

		public float maxShadowBias = 0.1f;

		public float adaptTime = 1f;

		private float smoothHeight;

		private float changeSpeed;

		private float originalStrength = 1f;

		private void Start()
		{
			originalStrength = sunLight.shadowStrength;
		}

		private void Update()
		{
			Ray ray = new Ray(Camera.main.transform.position, -Vector3.up);
			float num = base.transform.position.y;
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo))
			{
				num = hitInfo.distance;
			}
			if (Mathf.Abs(num - smoothHeight) > 1f)
			{
				smoothHeight = Mathf.SmoothDamp(smoothHeight, num, ref changeSpeed, adaptTime);
			}
			float num2 = Mathf.InverseLerp(minHeight, maxHeight, smoothHeight);
			QualitySettings.shadowDistance = Mathf.Lerp(minShadowDistance, maxShadowDistance, num2);
			sunLight.shadowBias = Mathf.Lerp(minShadowBias, maxShadowBias, 1f - (1f - num2) * (1f - num2));
			sunLight.shadowStrength = Mathf.Lerp(originalStrength, 0f, num2);
		}
	}
}
