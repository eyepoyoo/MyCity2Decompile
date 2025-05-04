using UnityEngine;

namespace UnitySampleAssets.Utility
{
	public class TimedObjectDestructor : MonoBehaviour
	{
		public float timeOut = 1f;

		public bool detachChildren;

		private void Awake()
		{
			Invoke("DestroyNow", timeOut);
		}

		private void DestroyNow()
		{
			if (detachChildren)
			{
				base.transform.DetachChildren();
			}
			Object.DestroyObject(base.gameObject);
		}
	}
}
