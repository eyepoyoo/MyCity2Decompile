using System;
using UnityEngine;

public class LightmapSampler : MonoBehaviour
{
	public Texture2D sampledMap;

	public int textureSize = 32;

	public LayerMask layersToTest;

	public Color defaultColor = Color.black;

	public Material[] materialsToAssignSampler;

	public bool showGizmos;

	private void Start()
	{
		GenerateTexture();
	}

	public void GenerateTexture()
	{
		bool flag = false;
		if (sampledMap == null)
		{
			sampledMap = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false);
			flag = true;
		}
		BoxCollider component = GetComponent<BoxCollider>();
		Vector3 vector = base.transform.TransformPoint(component.center - component.size / 2f);
		Vector3 vector2 = base.transform.TransformPoint(component.center + component.size / 2f);
		float num = Mathf.Abs(vector.x - vector2.x);
		float num2 = Mathf.Abs(vector.z - vector2.z);
		float num3 = num / (float)textureSize;
		float num4 = num2 / (float)textureSize;
		if (flag)
		{
			for (int i = 0; i < textureSize; i++)
			{
				for (int j = 0; j < textureSize; j++)
				{
					Vector3 origin = new Vector3(vector.x + num3 * (float)i, base.transform.position.y, vector.z + num4 * (float)j);
					RaycastHit hitInfo;
					if (Physics.Raycast(origin, -Vector3.up, out hitInfo, 1000f, layersToTest))
					{
						Vector2 lightmapCoord = hitInfo.lightmapCoord;
						Color color = defaultColor;
						if (hitInfo.collider.GetComponent<Renderer>() != null && hitInfo.collider.GetComponent<Renderer>().lightmapIndex >= 0)
						{
							try
							{
								Texture2D lightmapFar = LightmapSettings.lightmaps[hitInfo.collider.GetComponent<Renderer>().lightmapIndex].lightmapColor;
								Color pixelBilinear = lightmapFar.GetPixelBilinear(lightmapCoord.x, lightmapCoord.y);
								color = pixelBilinear;
							}
							catch (Exception ex)
							{
								Debug.LogError("Exception in Lightmap Sampler: " + ex.Message + " (Lightmap Index: " + hitInfo.collider.GetComponent<Renderer>().lightmapIndex + ")");
							}
						}
						sampledMap.SetPixel(i, j, color);
					}
					else
					{
						sampledMap.SetPixel(i, j, defaultColor);
					}
				}
				sampledMap.Apply();
			}
		}
		Vector4 vector3 = new Vector4(vector.x, vector.z, num, num2);
		for (int k = 0; k < materialsToAssignSampler.Length; k++)
		{
			materialsToAssignSampler[k].SetTexture("_SampledLightmap", sampledMap);
			materialsToAssignSampler[k].SetVector("_SamplerRect", vector3);
		}
	}

	private void Update()
	{
	}

	private void OnDrawGizmosSelected()
	{
		if (!showGizmos)
		{
			return;
		}
		BoxCollider component = GetComponent<BoxCollider>();
		Vector3 vector = base.transform.TransformPoint(component.center - component.size / 2f);
		Vector3 vector2 = base.transform.TransformPoint(component.center + component.size / 2f);
		float num = Mathf.Abs(vector2.x - vector.x);
		float num2 = Mathf.Abs(vector2.z - vector.z);
		float num3 = num / (float)textureSize;
		float num4 = num2 / (float)textureSize;
		for (int i = 0; i < textureSize; i++)
		{
			for (int j = 0; j < textureSize; j++)
			{
				Vector3 center = new Vector3(vector.x + num3 * (float)i, base.transform.position.y, vector.z + num4 * (float)j);
				Gizmos.DrawSphere(center, 0.1f);
			}
		}
	}
}
