using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class TextureAtlaser : MonoBehaviour
{
	public class ShaderType
	{
		public Material generatedMaterial;

		public Material generatedLargeMaterial;

		public string shaderName;

		public Dictionary<string, Texture2D> textureList = new Dictionary<string, Texture2D>();

		public Dictionary<string, Texture2D> largeTextureList = new Dictionary<string, Texture2D>();

		public Texture2D largeTexture;

		public Texture2D packedTexture;

		public Dictionary<string, Rect> UVRectLookup = new Dictionary<string, Rect>();

		public Rect[] packedUVs;

		public Rect[] largePackedUVs;

		public ShaderType(string name)
		{
			shaderName = name;
		}

		public void AddTexture(Texture2D texture, int largeTextureThreshold, bool seperateLargeDiffuseTextures, string shaderName, bool debug = false)
		{
			if (texture == null)
			{
				Debug.LogError("A null texture was passed into AddTexture()");
				return;
			}
			if (debug)
			{
				Debug.Log("Adding Texture: " + texture.name);
				texture.GetPixel(0, 0);
			}
			if (seperateLargeDiffuseTextures && texture.width >= largeTextureThreshold && shaderName == "Diffuse")
			{
				if (!largeTextureList.ContainsKey(texture.name))
				{
					largeTextureList[texture.name] = texture;
				}
			}
			else if (!textureList.ContainsKey(texture.name))
			{
				textureList[texture.name] = texture;
			}
		}

		private Texture2D FilterTexturePadding(Texture2D source, int padding, Rect[] uvs)
		{
			Texture2D texture2D = new Texture2D(source.width, source.height, TextureFormat.ARGB32, true);
			texture2D.SetPixels(source.GetPixels());
			for (int i = 0; i < uvs.Length; i++)
			{
				Rect rect = uvs[i];
				Vector2 vector = new Vector2(rect.x * (float)texture2D.width, rect.y * (float)texture2D.height);
				Vector2 vector2 = new Vector2((rect.x + rect.width) * (float)texture2D.width, (rect.y + rect.height) * (float)texture2D.height);
				Vector2 vector3 = vector2 - vector;
				Color[] pixels = texture2D.GetPixels((int)vector.x, (int)vector.y, (int)vector3.x, 1);
				int num = (int)vector.y - Mathf.Clamp((int)vector.y - padding, 0, texture2D.height);
				Color[] array = new Color[num * pixels.Length];
				for (int j = 0; j < num; j++)
				{
					Array.Copy(pixels, 0, array, j * pixels.Length, pixels.Length);
				}
				texture2D.SetPixels((int)vector.x, (int)vector.y - num, (int)vector3.x, num, array);
				pixels = texture2D.GetPixels((int)vector.x, (int)vector2.y - 1, (int)vector3.x, 1);
				num = Mathf.Clamp((int)vector2.y + padding, 0, texture2D.height) - (int)vector2.y;
				array = new Color[num * pixels.Length];
				for (int k = 0; k < num; k++)
				{
					Array.Copy(pixels, 0, array, k * pixels.Length, pixels.Length);
				}
				texture2D.SetPixels((int)vector.x, (int)vector2.y, (int)vector3.x, num, array);
				pixels = texture2D.GetPixels((int)vector.x, (int)vector.y, 1, (int)vector3.y);
				num = (int)vector.x - Mathf.Clamp((int)vector.x - padding, 0, texture2D.width);
				array = new Color[num * pixels.Length];
				for (int l = 0; l < array.Length; l++)
				{
					array[l] = pixels[l / num];
				}
				texture2D.SetPixels((int)vector.x - num, (int)vector.y, num, (int)vector3.y, array);
				pixels = texture2D.GetPixels((int)vector2.x - 1, (int)vector.y, 1, (int)vector3.y);
				num = Mathf.Clamp((int)vector2.x + padding, 0, texture2D.width) - (int)vector2.x;
				array = new Color[num * pixels.Length];
				for (int m = 0; m < array.Length; m++)
				{
					array[m] = pixels[m / num];
				}
				texture2D.SetPixels((int)vector2.x, (int)vector.y, num, (int)vector3.y, array);
			}
			texture2D.Apply();
			return texture2D;
		}

		public Texture2D GenerateAtlases(int textureSize, int padding, bool debug = false)
		{
			packedTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, true);
			packedTexture.wrapMode = TextureWrapMode.Clamp;
			List<Texture2D> list = new List<Texture2D>();
			foreach (string key in textureList.Keys)
			{
				list.Add(textureList[key]);
			}
			if (debug)
			{
				foreach (string key2 in textureList.Keys)
				{
					Debug.Log("Packing: " + key2);
				}
			}
			if (list.Count != 0)
			{
				packedUVs = packedTexture.PackTextures(list.ToArray(), padding, textureSize, false);
				packedTexture = FilterTexturePadding(packedTexture, Mathf.FloorToInt((float)padding * 0.5f), packedUVs);
				int num = 0;
				foreach (string key3 in textureList.Keys)
				{
					UVRectLookup[key3] = packedUVs[num];
					num++;
				}
				if (largeTextureList.Count > 0)
				{
					largeTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, true);
					largeTexture.wrapMode = TextureWrapMode.Clamp;
					List<Texture2D> list2 = new List<Texture2D>();
					foreach (string key4 in largeTextureList.Keys)
					{
						list2.Add(largeTextureList[key4]);
					}
					largePackedUVs = largeTexture.PackTextures(list2.ToArray(), padding, textureSize, false);
					largeTexture = FilterTexturePadding(largeTexture, Mathf.FloorToInt((float)padding * 0.5f), largePackedUVs);
					num = 0;
					foreach (string key5 in largeTextureList.Keys)
					{
						UVRectLookup[key5] = largePackedUVs[num];
						num++;
					}
				}
			}
			else
			{
				Debug.LogWarning("No textures were packed for shader: " + shaderName);
			}
			return packedTexture;
		}
	}

	private const string DIFFUSE_NAME = "Diffuse";

	private const string REPLACEMENT_DIFFUSE_NAME = "Custom/TextureAtlas/Diffuse";

	private const string ALPHA_NAME = "Transparent/Diffuse";

	private const string REPLACEMENT_ALPHA_NAME = "Custom/TextureAtlas/Transparent Diffuse";

	private const string REPLACEMENT_ALPHA_NAME_NOTILE = "Custom/TextureAtlas/Transparent Diffuse No Tiling";

	private const string MOB_ADDITIVE_NAME = "Mobile/Particles/Additive";

	private const string REPLACEMENT_ADDITIVE_NAME_NOTILE = "Mobile/Particles/Additive";

	private const string REPLACEMENT_ADDITIVE_NAME = "Custom/TextureAtlas/Additive";

	private const string CUSTOM_UNLIT_NOBACKFACE_NAME = "Custom/UnlitNoBackfaceCull";

	private const string REPLACEMENT_CUSTOM_UNLIT_NOBACKFACE_NAME = "Custom/UnlitNoBackfaceCull";

	public GameObject[] objectsToAtlas;

	public int maxTextureSize = 1024;

	public int padding = 4;

	public bool seperateLargeDiffuseTextures;

	public int largeTextureThreshold = 512;

	public bool dontTileTransparentTextures = true;

	public bool removeMeshColliders;

	public bool debugOutputTextureNames;

	public bool saveAtlasTextures;

	public bool saveMaterialsAndPrefabs;

	public string assetSavePath = string.Empty;

	private Dictionary<string, ShaderType> shaderTypes = new Dictionary<string, ShaderType>();

	private float _percentComplete;

	private bool _isDirty = true;

	private int _uid;

	public Dictionary<string, ShaderType> ShaderTypes
	{
		get
		{
			return shaderTypes;
		}
	}

	public bool Dirty
	{
		get
		{
			bool isDirty = _isDirty;
			_isDirty = false;
			return isDirty;
		}
	}

	public float GetPercentComplete()
	{
		return _percentComplete;
	}

	private void UpdatePercentComplete(float percent)
	{
		_percentComplete = percent;
		_isDirty = true;
	}

	private void Awake()
	{
		StartCoroutine(GenerateMeshes());
	}

	public string MeshToString(MeshFilter mf)
	{
		Mesh mesh = mf.mesh;
		Material[] sharedMaterials = mf.GetComponent<Renderer>().sharedMaterials;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("g ").Append(mf.name).Append("\n");
		Vector3[] vertices = mesh.vertices;
		for (int i = 0; i < vertices.Length; i++)
		{
			Vector3 vector = vertices[i];
			stringBuilder.Append(string.Format("v {0} {1} {2}\n", vector.x, vector.y, vector.z));
		}
		stringBuilder.Append("\n");
		Vector3[] normals = mesh.normals;
		for (int j = 0; j < normals.Length; j++)
		{
			Vector3 vector2 = normals[j];
			stringBuilder.Append(string.Format("vn {0} {1} {2}\n", vector2.x, vector2.y, vector2.z));
		}
		stringBuilder.Append("\n");
		Vector2[] uv = mesh.uv;
		for (int k = 0; k < uv.Length; k++)
		{
			Vector3 vector3 = uv[k];
			stringBuilder.Append(string.Format("vt {0} {1}\n", vector3.x, vector3.y));
		}
		for (int l = 0; l < mesh.subMeshCount; l++)
		{
			stringBuilder.Append("\n");
			stringBuilder.Append("usemtl ").Append(sharedMaterials[l].name).Append("\n");
			stringBuilder.Append("usemap ").Append(sharedMaterials[l].name).Append("\n");
			int[] triangles = mesh.GetTriangles(l);
			for (int m = 0; m < triangles.Length; m += 3)
			{
				stringBuilder.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n", triangles[m] + 1, triangles[m + 1] + 1, triangles[m + 2] + 1));
			}
		}
		return stringBuilder.ToString();
	}

	public void MeshToFile(MeshFilter mf, string filename)
	{
		using (StreamWriter streamWriter = new StreamWriter(filename))
		{
			streamWriter.Write(MeshToString(mf));
		}
	}

	private void CombineMeshes(string objectName, string shaderName, Transform sourceTransform, CombineInstance[] combineInstances, bool isLargeTex = false)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(sourceTransform.gameObject);
		MeshCollider component = gameObject.GetComponent<MeshCollider>();
		if (removeMeshColliders && component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
		gameObject.layer = sourceTransform.gameObject.layer;
		MeshFilter component2 = gameObject.GetComponent<MeshFilter>();
		MeshRenderer component3 = gameObject.GetComponent<MeshRenderer>();
		gameObject.transform.parent = sourceTransform.parent;
		gameObject.transform.position = sourceTransform.position;
		gameObject.transform.localScale = sourceTransform.localScale;
		gameObject.transform.rotation = sourceTransform.rotation;
		component2.mesh = new Mesh();
		component2.mesh.CombineMeshes(combineInstances, true, false);
		var o_310_2_638819107724689969 = component2.mesh;
		component2.mesh.name = "generated_" + sourceTransform.name;
		Material[] array = new Material[1] { shaderTypes[shaderName].generatedMaterial };
		if (isLargeTex)
		{
			array[0] = shaderTypes[shaderName].generatedLargeMaterial;
		}
		if (saveMaterialsAndPrefabs)
		{
		}
		component3.sharedMaterials = array;
		gameObject.transform.parent = base.transform;
	}

	private IEnumerator GenerateMeshes()
	{
		base.transform.position = Vector3.zero;
		GameObject[] array = objectsToAtlas;
		foreach (GameObject obj in array)
		{
			MeshRenderer[] rendererList = obj.GetComponentsInChildren<MeshRenderer>();
			MeshRenderer[] array2 = rendererList;
			foreach (MeshRenderer mr in array2)
			{
				Material[] sharedMaterials = mr.sharedMaterials;
				foreach (Material material in sharedMaterials)
				{
					if ((material.shader.name == "Diffuse" || material.shader.name == "Transparent/Diffuse" || material.shader.name == "Mobile/Particles/Additive" || material.shader.name == "Custom/UnlitNoBackfaceCull") && !(material.mainTexture == null))
					{
						if (!shaderTypes.ContainsKey(material.shader.name))
						{
							shaderTypes[material.shader.name] = new ShaderType(material.shader.name);
						}
						shaderTypes[material.shader.name].AddTexture(material.mainTexture as Texture2D, largeTextureThreshold, seperateLargeDiffuseTextures, material.shader.name, debugOutputTextureNames);
					}
				}
			}
		}
		yield return new WaitForSeconds(0f);
		foreach (string key in shaderTypes.Keys)
		{
			Texture2D result = shaderTypes[key].GenerateAtlases(maxTextureSize, padding, debugOutputTextureNames);
			string shaderName = "Custom/TextureAtlas/Diffuse";
			switch (key)
			{
			case "Custom/UnlitNoBackfaceCull":
				shaderName = "Custom/UnlitNoBackfaceCull";
				break;
			case "Mobile/Particles/Additive":
				shaderName = ((!dontTileTransparentTextures) ? "Custom/TextureAtlas/Additive" : "Mobile/Particles/Additive");
				break;
			case "Transparent/Diffuse":
				shaderName = ((!dontTileTransparentTextures) ? "Custom/TextureAtlas/Transparent Diffuse" : "Custom/TextureAtlas/Transparent Diffuse No Tiling");
				break;
			}
			shaderTypes[key].generatedMaterial = new Material(Shader.Find(shaderName));
			shaderTypes[key].generatedMaterial.name = "generated_" + key;
			shaderTypes[key].generatedMaterial.mainTexture = result;
			if (shaderTypes[key].textureList.Count == 1)
			{
				string firstKey = string.Empty;
				foreach (string textureKey in shaderTypes[key].textureList.Keys)
				{
					firstKey = textureKey;
				}
				shaderTypes[key].generatedMaterial.mainTexture = shaderTypes[key].textureList[firstKey];
			}
			if (seperateLargeDiffuseTextures)
			{
				shaderTypes[key].generatedLargeMaterial = new Material(Shader.Find(shaderName));
				shaderTypes[key].generatedLargeMaterial.name = "largegenerated_" + key;
				shaderTypes[key].generatedLargeMaterial.mainTexture = shaderTypes[key].largeTexture;
			}
		}
		yield return new WaitForSeconds(0f);
		float numObjects = objectsToAtlas.Length;
		float curObject = 0f;
		GameObject[] array3 = objectsToAtlas;
		foreach (GameObject obj2 in array3)
		{
			curObject += 1f;
			yield return new WaitForSeconds(0f);
			MeshFilter[] meshFilters = obj2.GetComponentsInChildren<MeshFilter>();
			float mfPercentTrack = 0f;
			float mfPercentMax = meshFilters.Length;
			MeshFilter[] array4 = meshFilters;
			foreach (MeshFilter mf in array4)
			{
				mfPercentTrack += 1f;
				float mfPercentComplete = mfPercentTrack / mfPercentMax * 100f / (curObject / numObjects);
				UpdatePercentComplete(mfPercentComplete);
				yield return new WaitForSeconds(0f);
				MeshRenderer mr2 = mf.GetComponent<MeshRenderer>();
				Mesh mesh = mf.mesh;
				bool leaveAlone = false;
				if (mr2.sharedMaterials.Length != mesh.subMeshCount)
				{
					Debug.LogError("Object: " + mr2.name + " has a different number of materials(" + mr2.sharedMaterials.Length + ") to submeshes (" + mesh.subMeshCount + ") ignoring");
					continue;
				}
				Dictionary<string, List<CombineInstance>> combineInstances = new Dictionary<string, List<CombineInstance>>();
				for (int submeshIndex = 0; submeshIndex < mesh.subMeshCount; submeshIndex++)
				{
					int[] subTriangles = mesh.GetTriangles(submeshIndex);
					string shaderName2 = mr2.sharedMaterials[submeshIndex].shader.name;
					if (!shaderTypes.ContainsKey(shaderName2))
					{
						Debug.Log("Unknown Shader Name: " + shaderName2);
						leaveAlone = true;
					}
					else
					{
						if (mr2.sharedMaterials[submeshIndex].mainTexture == null)
						{
							continue;
						}
						Mesh newMesh = new Mesh();
						List<Vector3> verts = new List<Vector3>();
						List<int> triangles = new List<int>();
						List<Vector2> uvs = new List<Vector2>();
						List<Vector2> uv2 = new List<Vector2>();
						List<Vector3> normals = new List<Vector3>();
						List<Vector4> tangents = new List<Vector4>();
						List<Color> colors = new List<Color>();
						string textureName = mr2.sharedMaterials[submeshIndex].mainTexture.name;
						Rect textureUVRect = shaderTypes[shaderName2].UVRectLookup[textureName];
						for (int n = 0; n < subTriangles.Length; n++)
						{
							int triangleIndex = subTriangles[n];
							Vector2 uv3 = mesh.uv[triangleIndex];
							verts.Add(mesh.vertices[triangleIndex]);
							normals.Add(mesh.normals[triangleIndex]);
							tangents.Add(mesh.tangents[triangleIndex]);
							uv2.Add(mesh.uv2[triangleIndex]);
							Vector2 newUV = uv3;
							if (shaderTypes[shaderName2].textureList.Count != 1)
							{
								if (shaderName2 == "Diffuse" || !dontTileTransparentTextures)
								{
									colors.Add(new Color(textureUVRect.width, textureUVRect.x, textureUVRect.y, textureUVRect.height));
								}
								else
								{
									newUV = new Vector2(uv3.x * textureUVRect.width + textureUVRect.x, uv3.y * textureUVRect.height + textureUVRect.y);
								}
							}
							uvs.Add(newUV);
							triangles.Add(n);
						}
						newMesh.vertices = verts.ToArray();
						newMesh.normals = normals.ToArray();
						newMesh.tangents = tangents.ToArray();
						newMesh.uv = uvs.ToArray();
						newMesh.uv2 = uv2.ToArray();
						newMesh.triangles = triangles.ToArray();
						newMesh.colors = colors.ToArray();
						newMesh.RecalculateBounds();
						CombineInstance combineInstance = new CombineInstance
						{
							mesh = newMesh,
							transform = mf.transform.localToWorldMatrix
						};
						if (shaderName2 == "Diffuse" && seperateLargeDiffuseTextures && shaderTypes[shaderName2].largeTextureList.ContainsKey(textureName))
						{
							string ciKey = shaderName2 + "LARGE";
							if (!combineInstances.ContainsKey(ciKey))
							{
								combineInstances[ciKey] = new List<CombineInstance>();
							}
							combineInstances[ciKey].Add(combineInstance);
						}
						else
						{
							string ciKey2 = shaderName2;
							if (!combineInstances.ContainsKey(ciKey2))
							{
								combineInstances[ciKey2] = new List<CombineInstance>();
							}
							combineInstances[ciKey2].Add(combineInstance);
						}
					}
				}
				Transform sourceTransform = mr2.transform;
				if (!leaveAlone)
				{
					foreach (string ciKey3 in combineInstances.Keys)
					{
						if (ciKey3 == "DiffuseLARGE")
						{
							CombineMeshes("diffuseLarge", "Diffuse", sourceTransform, combineInstances[ciKey3].ToArray(), true);
						}
						else
						{
							CombineMeshes(ciKey3, ciKey3, sourceTransform, combineInstances[ciKey3].ToArray());
						}
					}
					mf.gameObject.GetComponent<Renderer>().enabled = false;
				}
				else
				{
					Debug.LogWarning(mf.name + " was not disabled -- source mesh has unknown shader type");
				}
			}
		}
		yield return new WaitForSeconds(0f);
		StaticBatchingUtility.Combine(base.gameObject);
		yield return new WaitForSeconds(1f);
		Debug.Log("Texture Atlaser has completed atlasing the scene");
		_isDirty = true;
	}
}
