using UnityEngine;

namespace UnitySampleAssets.ImageEffects
{
	[AddComponentMenu("Image Effects/Screen Overlay")]
	[RequireComponent(typeof(Camera))]
	[ExecuteInEditMode]
	internal class ScreenOverlay : PostEffectsBase
	{
		internal enum OverlayBlendMode
		{
			Additive = 0,
			ScreenBlend = 1,
			Multiply = 2,
			Overlay = 3,
			AlphaBlend = 4
		}

		public OverlayBlendMode blendMode = OverlayBlendMode.Overlay;

		public float intensity = 1f;

		public Texture2D texture;

		public Shader overlayShader;

		private Material overlayMaterial;

		protected override bool CheckResources()
		{
			CheckSupport(false);
			overlayMaterial = CheckShaderAndCreateMaterial(overlayShader, overlayMaterial);
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
			overlayMaterial.SetFloat("_Intensity", intensity);
			overlayMaterial.SetTexture("_Overlay", texture);
			Graphics.Blit(source, destination, overlayMaterial, (int)blendMode);
		}
	}
}
