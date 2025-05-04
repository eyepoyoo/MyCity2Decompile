using UnityEngine;

namespace UnitySampleAssets.ImageEffects
{
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Contrast Enhance (Unsharp Mask)")]
	[ExecuteInEditMode]
	internal class ContrastEnhance : PostEffectsBase
	{
		public float intensity = 0.5f;

		public float threshhold;

		private Material separableBlurMaterial;

		private Material contrastCompositeMaterial;

		public float blurSpread = 1f;

		public Shader separableBlurShader;

		public Shader contrastCompositeShader;

		protected override bool CheckResources()
		{
			CheckSupport(false);
			contrastCompositeMaterial = CheckShaderAndCreateMaterial(contrastCompositeShader, contrastCompositeMaterial);
			separableBlurMaterial = CheckShaderAndCreateMaterial(separableBlurShader, separableBlurMaterial);
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
			RenderTexture temporary = RenderTexture.GetTemporary((int)((float)source.width / 2f), (int)((float)source.height / 2f), 0);
			RenderTexture temporary2 = RenderTexture.GetTemporary((int)((float)source.width / 4f), (int)((float)source.height / 4f), 0);
			RenderTexture temporary3 = RenderTexture.GetTemporary((int)((float)source.width / 4f), (int)((float)source.height / 4f), 0);
			Graphics.Blit(source, temporary);
			Graphics.Blit(temporary, temporary2);
			separableBlurMaterial.SetVector("offsets", new Vector4(0f, (float)((double)blurSpread * 1.0 / (double)temporary2.height), 0f, 0f));
			Graphics.Blit(temporary2, temporary3, separableBlurMaterial);
			separableBlurMaterial.SetVector("offsets", new Vector4((float)((double)blurSpread * 1.0 / (double)temporary2.width), 0f, 0f, 0f));
			Graphics.Blit(temporary3, temporary2, separableBlurMaterial);
			contrastCompositeMaterial.SetTexture("_MainTexBlurred", temporary2);
			contrastCompositeMaterial.SetFloat("intensity", intensity);
			contrastCompositeMaterial.SetFloat("threshhold", threshhold);
			Graphics.Blit(source, destination, contrastCompositeMaterial);
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
			RenderTexture.ReleaseTemporary(temporary3);
		}
	}
}
