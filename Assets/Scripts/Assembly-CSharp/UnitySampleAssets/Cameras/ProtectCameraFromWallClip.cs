using System;
using System.Collections;
using UnityEngine;

namespace UnitySampleAssets.Cameras
{
	public class ProtectCameraFromWallClip : MonoBehaviour
	{
		public class RayHitComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				return ((RaycastHit)x).distance.CompareTo(((RaycastHit)y).distance);
			}
		}

		public float clipMoveTime = 0.05f;

		public float returnTime = 0.4f;

		public float sphereCastRadius = 0.1f;

		public bool visualiseInEditor;

		public float closestDistance = 0.5f;

		public string dontClipTag = "Player";

		private Transform cam;

		private Transform pivot;

		private float originalDist;

		private float moveVelocity;

		private float currentDist;

		private Ray ray;

		private RaycastHit[] hits;

		private RayHitComparer rayHitComparer;

		public bool protecting { get; private set; }

		private void Start()
		{
			cam = GetComponentInChildren<Camera>().transform;
			pivot = cam.parent;
			originalDist = cam.localPosition.magnitude;
			currentDist = originalDist;
			rayHitComparer = new RayHitComparer();
		}

		private void LateUpdate()
		{
			float num = originalDist;
			ray.origin = pivot.position + pivot.forward * sphereCastRadius;
			ray.direction = -pivot.forward;
			Collider[] array = Physics.OverlapSphere(ray.origin, sphereCastRadius);
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].isTrigger && (!(array[i].attachedRigidbody != null) || !array[i].attachedRigidbody.CompareTag(dontClipTag)))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				ray.origin += pivot.forward * sphereCastRadius;
				hits = Physics.RaycastAll(ray, originalDist - sphereCastRadius);
			}
			else
			{
				hits = Physics.SphereCastAll(ray, sphereCastRadius, originalDist + sphereCastRadius);
			}
			Array.Sort(hits, rayHitComparer);
			float num2 = float.PositiveInfinity;
			for (int j = 0; j < hits.Length; j++)
			{
				if (hits[j].distance < num2 && !hits[j].collider.isTrigger && (!(hits[j].collider.attachedRigidbody != null) || !hits[j].collider.attachedRigidbody.CompareTag(dontClipTag)))
				{
					num2 = hits[j].distance;
					num = 0f - pivot.InverseTransformPoint(hits[j].point).z;
					flag2 = true;
				}
			}
			if (flag2)
			{
				Debug.DrawRay(ray.origin, -pivot.forward * (num + sphereCastRadius), Color.red);
			}
			protecting = flag2;
			currentDist = Mathf.SmoothDamp(currentDist, num, ref moveVelocity, (!(currentDist > num)) ? returnTime : clipMoveTime);
			currentDist = Mathf.Clamp(currentDist, closestDistance, originalDist);
			cam.localPosition = -Vector3.forward * currentDist;
		}
	}
}
