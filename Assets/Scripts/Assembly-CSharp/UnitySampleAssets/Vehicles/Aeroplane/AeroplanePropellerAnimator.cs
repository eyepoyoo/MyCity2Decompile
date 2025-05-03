using UnityEngine;

namespace UnitySampleAssets.Vehicles.Aeroplane
{
	public class AeroplanePropellerAnimator : MonoBehaviour
	{
		private const float RpmToDps = 60f;

		[SerializeField]
		private Transform propellorModel;

		[SerializeField]
		private Transform propellorBlur;

		[SerializeField]
		private Texture2D[] propellorBlurTextures;

		[SerializeField]
		[Range(0f, 1f)]
		private float throttleBlurStart = 0.25f;

		[SerializeField]
		[Range(0f, 1f)]
		private float throttleBlurEnd = 0.5f;

		[SerializeField]
		private float maxRpm = 2000f;

		private AeroplaneController plane;

		private int propellorBlurState = -1;

		private void Awake()
		{
			plane = GetComponent<AeroplaneController>();
			propellorBlur.parent = propellorModel;
		}

		private void Update()
		{
			propellorModel.Rotate(0f, maxRpm * plane.Throttle * Time.deltaTime * 60f, 0f);
			int num = 0;
			if (plane.Throttle > throttleBlurStart)
			{
				float num2 = Mathf.InverseLerp(throttleBlurStart, throttleBlurEnd, plane.Throttle);
				num = Mathf.FloorToInt(num2 * (float)(propellorBlurTextures.Length - 1));
			}
			if (num != propellorBlurState)
			{
				propellorBlurState = num;
				if (propellorBlurState == 0)
				{
					propellorModel.GetComponent<Renderer>().enabled = true;
					propellorBlur.GetComponent<Renderer>().enabled = false;
				}
				else
				{
					propellorModel.GetComponent<Renderer>().enabled = false;
					propellorBlur.GetComponent<Renderer>().enabled = true;
					propellorBlur.GetComponent<Renderer>().material.mainTexture = propellorBlurTextures[propellorBlurState];
				}
			}
		}
	}
}
