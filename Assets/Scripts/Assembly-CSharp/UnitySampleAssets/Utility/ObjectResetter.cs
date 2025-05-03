using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitySampleAssets.Utility
{
	public class ObjectResetter : MonoBehaviour
	{
		private Vector3 originalPosition;

		private Quaternion originalRotation;

		private List<Transform> originalStructure;

		private void Start()
		{
			originalStructure = new List<Transform>(GetComponentsInChildren<Transform>());
			originalPosition = base.transform.position;
			originalRotation = base.transform.rotation;
		}

		public void DelayedReset(float delay)
		{
			StartCoroutine(ResetCoroutine(delay));
		}

		public IEnumerator ResetCoroutine(float delay)
		{
			yield return new WaitForSeconds(delay);
			Transform[] componentsInChildren = GetComponentsInChildren<Transform>();
			foreach (Transform t in componentsInChildren)
			{
				if (!originalStructure.Contains(t))
				{
					t.parent = null;
				}
			}
			base.transform.position = originalPosition;
			base.transform.rotation = originalRotation;
			if ((bool)GetComponent<Rigidbody>())
			{
				GetComponent<Rigidbody>().velocity = Vector3.zero;
				GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			}
			SendMessage("Reset");
		}
	}
}
