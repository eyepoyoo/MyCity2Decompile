using UnityEngine;

namespace UnitySampleAssets.Cameras
{
	public abstract class PivotBasedCameraRig : AbstractTargetFollower
	{
		protected Transform cam;

		protected Transform pivot;

		protected Vector3 lastTargetPosition;

		protected virtual void Awake()
		{
			cam = GetComponentInChildren<Camera>().transform;
			pivot = cam.parent;
		}
	}
}
