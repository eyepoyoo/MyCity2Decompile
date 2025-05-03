using UnityEngine;
using UnityEngine.UI;

namespace VacuumShaders.CurvedWorld.Demo
{
	[RequireComponent(typeof(Text))]
	[AddComponentMenu("VacuumShaders/Curved World/Demo/FPSCounter")]
	public class CW_Demo_FPSCounter : MonoBehaviour
	{
		private const float fpsMeasurePeriod = 0.5f;

		private const string display = "{0} FPS";

		private int m_FpsAccumulator;

		private float m_FpsNextPeriod;

		private int m_CurrentFps;

		private Text uiText;

		private void Start()
		{
			m_FpsNextPeriod = Time.realtimeSinceStartup + 0.5f;
			uiText = GetComponent<Text>();
		}

		private void Update()
		{
			m_FpsAccumulator++;
			if (Time.realtimeSinceStartup > m_FpsNextPeriod)
			{
				m_CurrentFps = (int)((float)m_FpsAccumulator / 0.5f);
				m_FpsAccumulator = 0;
				m_FpsNextPeriod += 0.5f;
				uiText.text = string.Format("{0} FPS", m_CurrentFps);
			}
		}
	}
}
