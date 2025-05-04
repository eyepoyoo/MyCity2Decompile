using System.Collections;
using UnityEngine;

namespace UnitySampleAssets.Utility
{
	public class DragRigidbody : MonoBehaviour
	{
		private float spring = 50f;

		private float damper = 5f;

		private float drag = 10f;

		private float angularDrag = 5f;

		private float distance = 0.2f;

		private bool attachToCenterOfMass;

		private SpringJoint springJoint;

		private void Update()
		{
			if (!Input.GetMouseButtonDown(0))
			{
				return;
			}
			Camera camera = FindCamera();
			RaycastHit hitInfo = default(RaycastHit);
			if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition).origin, camera.ScreenPointToRay(Input.mousePosition).direction, out hitInfo, 100f, -5) && (bool)hitInfo.rigidbody && !hitInfo.rigidbody.isKinematic)
			{
				if (!springJoint)
				{
					GameObject gameObject = new GameObject("Rigidbody dragger");
					Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
					springJoint = gameObject.AddComponent<SpringJoint>();
					rigidbody.isKinematic = true;
				}
				springJoint.transform.position = hitInfo.point;
				if (attachToCenterOfMass)
				{
					Vector3 position = base.transform.TransformDirection(hitInfo.rigidbody.centerOfMass) + hitInfo.rigidbody.transform.position;
					position = springJoint.transform.InverseTransformPoint(position);
					springJoint.anchor = position;
				}
				else
				{
					springJoint.anchor = Vector3.zero;
				}
				springJoint.spring = spring;
				springJoint.damper = damper;
				springJoint.maxDistance = distance;
				springJoint.connectedBody = hitInfo.rigidbody;
				StartCoroutine("DragObject", hitInfo.distance);
			}
		}

		private IEnumerator DragObject(float distance)
		{
			float oldDrag = springJoint.connectedBody.drag;
			float oldAngularDrag = springJoint.connectedBody.angularDrag;
			springJoint.connectedBody.drag = drag;
			springJoint.connectedBody.angularDrag = angularDrag;
			Camera mainCamera = FindCamera();
			while (Input.GetMouseButton(0))
			{
				Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
				springJoint.transform.position = ray.GetPoint(distance);
				yield return null;
			}
			if ((bool)springJoint.connectedBody)
			{
				springJoint.connectedBody.drag = oldDrag;
				springJoint.connectedBody.angularDrag = oldAngularDrag;
				springJoint.connectedBody = null;
			}
		}

		private Camera FindCamera()
		{
			if ((bool)GetComponent<Camera>())
			{
				return GetComponent<Camera>();
			}
			return Camera.main;
		}
	}
}
