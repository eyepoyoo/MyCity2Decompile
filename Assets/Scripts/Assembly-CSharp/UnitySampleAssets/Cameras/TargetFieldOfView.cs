using UnityEngine;

namespace UnitySampleAssets.Cameras
{
	public class TargetFieldOfView : AbstractTargetFollower
	{
		[SerializeField]
		private float fovAdjustTime = 1f;

		[SerializeField]
		private float zoomAmountMultiplier = 2f;

		[SerializeField]
		private bool includeEffectsInSize;

		private float boundSize;

		private float fovAdjustVelocity;

		private Camera cam;

		private Transform lastTarget;

		protected override void Start()
		{
			base.Start();
			boundSize = MaxBoundsExtent(target, includeEffectsInSize);
			cam = GetComponentInChildren<Camera>();
		}

		protected override void FollowTarget(float deltaTime)
		{
			float magnitude = (target.position - base.transform.position).magnitude;
			float num = Mathf.Atan2(boundSize, magnitude) * 57.29578f * zoomAmountMultiplier;
			cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, num, ref fovAdjustVelocity, fovAdjustTime);
		}

		public override void SetTarget(Transform newTransform)
		{
			base.SetTarget(newTransform);
			boundSize = MaxBoundsExtent(newTransform, includeEffectsInSize);
		}

		public static float MaxBoundsExtent(Transform obj, bool includeEffects)
		{
			Renderer[] componentsInChildren = obj.GetComponentsInChildren<Renderer>();
			Bounds bounds = default(Bounds);
			bool flag = false;
			Renderer[] array = componentsInChildren;
			foreach (Renderer renderer in array)
			{
				if (!(renderer is TrailRenderer) && !(renderer is ParticleRenderer) && !(renderer is ParticleSystemRenderer))
				{
					if (!flag)
					{
						flag = true;
						bounds = renderer.bounds;
					}
					else
					{
						bounds.Encapsulate(renderer.bounds);
					}
				}
			}
			return Mathf.Max(bounds.extents.x, bounds.extents.y, bounds.extents.z);
		}
	}
}
