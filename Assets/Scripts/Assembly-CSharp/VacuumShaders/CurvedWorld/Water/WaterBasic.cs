using UnityEngine;

namespace VacuumShaders.CurvedWorld.Water
{
	[AddComponentMenu("VacuumShaders/Curved World/Water/WaterBasic")]
	[ExecuteInEditMode]
	public class WaterBasic : MonoBehaviour
	{
		private void Update()
		{
			Renderer component = GetComponent<Renderer>();
			if ((bool)component)
			{
				Material sharedMaterial = component.sharedMaterial;
				if ((bool)sharedMaterial)
				{
					Vector4 vector = sharedMaterial.GetVector("WaveSpeed");
					float num = sharedMaterial.GetFloat("_WaveScale");
					float num2 = Time.time / 20f;
					Vector4 vector2 = vector * (num2 * num);
					Vector4 vector3 = new Vector4(Mathf.Repeat(vector2.x, 1f), Mathf.Repeat(vector2.y, 1f), Mathf.Repeat(vector2.z, 1f), Mathf.Repeat(vector2.w, 1f));
					sharedMaterial.SetVector("_WaveOffset", vector3);
				}
			}
		}
	}
}
