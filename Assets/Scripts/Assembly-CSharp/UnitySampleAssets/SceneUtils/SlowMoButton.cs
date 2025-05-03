using UnityEngine;
using UnityEngine.UI;

namespace UnitySampleAssets.SceneUtils
{
	public class SlowMoButton : MonoBehaviour
	{
		public Sprite FullSpeedTex;

		public Sprite SlowSpeedTex;

		public float fullSpeed = 1f;

		public float slowSpeed = 0.3f;

		public Button button;

		public bool alsoScalePhysicsTimestep = true;

		private bool slowMo;

		private float targetTime;

		private float lastRealTime;

		private float fixedTimeRatio;

		private void Start()
		{
			targetTime = fullSpeed;
			lastRealTime = Time.realtimeSinceStartup;
			fixedTimeRatio = Time.fixedDeltaTime / Time.timeScale;
		}

		private void Update()
		{
			float num = Time.realtimeSinceStartup - lastRealTime;
			if (Time.timeScale != targetTime)
			{
				Time.timeScale = Mathf.Lerp(Time.timeScale, targetTime, num * 2f);
				if (alsoScalePhysicsTimestep)
				{
					Time.fixedDeltaTime = fixedTimeRatio * Time.timeScale;
				}
				if (Mathf.Abs(Time.timeScale - targetTime) < 0.01f)
				{
					Time.timeScale = targetTime;
				}
			}
			lastRealTime = Time.realtimeSinceStartup;
		}

		public void ChangeSpeed()
		{
			slowMo = !slowMo;
			Image image = button.targetGraphic as Image;
			if (image != null)
			{
				image.sprite = ((!slowMo) ? FullSpeedTex : SlowSpeedTex);
			}
			button.targetGraphic = image;
			targetTime = ((!slowMo) ? fullSpeed : slowSpeed);
		}
	}
}
