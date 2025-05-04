using System;
using System.Collections;
using UnityEngine;

namespace UnitySampleAssets.Utility
{
	[Serializable]
	public class LerpControlledBob
	{
		public float BobDuration;

		public float BobAmount;

		private float offset;

		public float Offset()
		{
			return offset;
		}

		public IEnumerator DoBobCycle()
		{
			float t = 0f;
			while (t < BobDuration)
			{
				offset = Mathf.Lerp(0f, BobAmount, t / BobDuration);
				t += Time.deltaTime;
				yield return new WaitForFixedUpdate();
			}
			t = 0f;
			while (t < BobDuration)
			{
				offset = Mathf.Lerp(BobAmount, 0f, t / BobDuration);
				t += Time.deltaTime;
				yield return new WaitForFixedUpdate();
			}
			offset = 0f;
		}
	}
}
