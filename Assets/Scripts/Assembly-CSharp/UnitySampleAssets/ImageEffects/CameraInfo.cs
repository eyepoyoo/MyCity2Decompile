using UnityEngine;

namespace UnitySampleAssets.ImageEffects
{
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Camera Info")]
	[ExecuteInEditMode]
	public class CameraInfo : MonoBehaviour
	{
		public DepthTextureMode currentDepthMode;

		public RenderingPath currentRenderPath;

		public int recognizedPostFxCount;
	}
}
