using System.Collections;
using UnityEngine;
using UnitySampleAssets.Utility;

namespace UnitySampleAssets.Effects
{
	public class Explosive : MonoBehaviour
	{
		public Transform explosionPrefab;

		private bool exploded;

		public float detonationImpactVelocity = 10f;

		public float sizeMultiplier = 1f;

		public bool reset = true;

		public float resetTimeDelay = 10f;

		private void Start()
		{
		}

		private IEnumerator OnCollisionEnter(Collision col)
		{
			if (base.enabled && col.contacts.Length > 0)
			{
				float velocityAlongCollisionNormal = Vector3.Project(col.relativeVelocity, col.contacts[0].normal).magnitude;
				if ((velocityAlongCollisionNormal > detonationImpactVelocity || exploded) && !exploded)
				{
					Object.Instantiate(explosionPrefab, col.contacts[0].point, Quaternion.LookRotation(col.contacts[0].normal));
					exploded = true;
					SendMessage("Immobilize");
					if (reset)
					{
						GetComponent<ObjectResetter>().DelayedReset(resetTimeDelay);
					}
				}
			}
			yield return null;
		}

		public void Reset()
		{
			exploded = false;
		}
	}
}
