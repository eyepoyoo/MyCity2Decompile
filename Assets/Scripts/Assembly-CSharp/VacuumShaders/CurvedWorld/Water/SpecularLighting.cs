using UnityEngine;

namespace VacuumShaders.CurvedWorld.Water
{
	[AddComponentMenu("VacuumShaders/Curved World/Water/SpecularLighting")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(WaterBase))]
	public class SpecularLighting : MonoBehaviour
	{
		public Transform specularLight;

		private WaterBase m_WaterBase;

		public void Start()
		{
			m_WaterBase = (WaterBase)base.gameObject.GetComponent(typeof(WaterBase));
		}

		public void Update()
		{
			if (!m_WaterBase)
			{
				m_WaterBase = (WaterBase)base.gameObject.GetComponent(typeof(WaterBase));
			}
			if ((bool)specularLight && (bool)m_WaterBase.sharedMaterial)
			{
				m_WaterBase.sharedMaterial.SetVector("_WorldLightDir", specularLight.transform.forward);
			}
		}
	}
}
