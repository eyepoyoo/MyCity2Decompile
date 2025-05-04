using UnityEngine;

namespace UnitySampleAssets.ImageEffects
{
	[AddComponentMenu("Image Effects/Sun Shafts")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	public class SunShafts : PostEffectsBase
	{
		public SunShaftsResolution resolution = SunShaftsResolution.Normal;

		public ShaftsScreenBlendMode screenBlendMode;

		public Transform sunTransform;

		public int radialBlurIterations = 2;

		public Color sunColor = Color.white;

		public float sunShaftBlurRadius = 2.5f;

		public float sunShaftIntensity = 1.15f;

		public float useSkyBoxAlpha = 0.75f;

		public float maxRadius = 0.75f;

		public bool useDepthTexture = true;

		public Shader sunShaftsShader;

		private Material sunShaftsMaterial;

		public Shader simpleClearShader;

		private Material simpleClearMaterial;

		protected override bool CheckResources()
		{
			CheckSupport(useDepthTexture);
			sunShaftsMaterial = CheckShaderAndCreateMaterial(sunShaftsShader, sunShaftsMaterial);
			simpleClearMaterial = CheckShaderAndCreateMaterial(simpleClearShader, simpleClearMaterial);
			if (!isSupported)
			{
				ReportAutoDisable();
			}
			return isSupported;
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (useDepthTexture)
			{
				GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
			}
			float num = 4f;
			if (resolution == SunShaftsResolution.Normal)
			{
				num = 2f;
			}
			else if (resolution == SunShaftsResolution.High)
			{
				num = 1f;
			}
			Vector3 vector = Vector3.one * 0.5f;
			vector = ((!sunTransform) ? new Vector3(0.5f, 0.5f, 0f) : GetComponent<Camera>().WorldToViewportPoint(sunTransform.position));
			RenderTexture temporary = RenderTexture.GetTemporary((int)((float)source.width / num), (int)((float)source.height / num), 0);
			RenderTexture temporary2 = RenderTexture.GetTemporary((int)((float)source.width / num), (int)((float)source.height / num), 0);
			sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(1f, 1f, 0f, 0f) * sunShaftBlurRadius);
			sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector.x, vector.y, vector.z, maxRadius));
			sunShaftsMaterial.SetFloat("_NoSkyBoxMask", 1f - useSkyBoxAlpha);
			if (!useDepthTexture)
			{
				RenderTexture renderTexture = (RenderTexture.active = RenderTexture.GetTemporary(source.width, source.height, 0));
				GL.ClearWithSkybox(false, GetComponent<Camera>());
				sunShaftsMaterial.SetTexture("_Skybox", renderTexture);
				Graphics.Blit(source, temporary2, sunShaftsMaterial, 3);
				RenderTexture.ReleaseTemporary(renderTexture);
			}
			else
			{
				Graphics.Blit(source, temporary2, sunShaftsMaterial, 2);
			}
			DrawBorder(temporary2, simpleClearMaterial);
			radialBlurIterations = ClampBlurIterationsToSomethingThatMakesSense(radialBlurIterations);
			float num2 = sunShaftBlurRadius * 0.0013020834f;
			sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num2, num2, 0f, 0f));
			sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector.x, vector.y, vector.z, maxRadius));
			for (int i = 0; i < radialBlurIterations; i++)
			{
				Graphics.Blit(temporary2, temporary, sunShaftsMaterial, 1);
				num2 = sunShaftBlurRadius * (((float)i * 2f + 1f) * 6f) / 768f;
				sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num2, num2, 0f, 0f));
				Graphics.Blit(temporary, temporary2, sunShaftsMaterial, 1);
				num2 = sunShaftBlurRadius * (((float)i * 2f + 2f) * 6f) / 768f;
				sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num2, num2, 0f, 0f));
			}
			if ((double)vector.z >= 0.0)
			{
				sunShaftsMaterial.SetVector("_SunColor", new Vector4(sunColor.r, sunColor.g, sunColor.b, sunColor.a) * sunShaftIntensity);
			}
			else
			{
				sunShaftsMaterial.SetVector("_SunColor", Vector4.zero);
			}
			sunShaftsMaterial.SetTexture("_ColorBuffer", temporary2);
			Graphics.Blit(source, destination, sunShaftsMaterial, (screenBlendMode != ShaftsScreenBlendMode.Screen) ? 4 : 0);
			RenderTexture.ReleaseTemporary(temporary2);
			RenderTexture.ReleaseTemporary(temporary);
		}

		private int ClampBlurIterationsToSomethingThatMakesSense(int its)
		{
			if (its < 1)
			{
				return 1;
			}
			if (its > 4)
			{
				return 4;
			}
			return its;
		}
	}
}
