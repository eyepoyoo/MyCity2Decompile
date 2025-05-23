using UnityEngine;

namespace VacuumShaders.CurvedWorld.Water
{
	[RequireComponent(typeof(WaterBase))]
	[ExecuteInEditMode]
	[AddComponentMenu("VacuumShaders/Curved World/Water/Displace")]
	public class Displace : MonoBehaviour
	{
		public void Awake()
		{
			if (base.enabled)
			{
				OnEnable();
			}
			else
			{
				OnDisable();
			}
		}

		public void OnEnable()
		{
			Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
			Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
		}

		public void OnDisable()
		{
			Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
			Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
		}
	}
}
