using System.Collections;
using UnityEngine;

namespace UnitySampleAssets.Utility
{
	public class ParticleSystemDestroyer : MonoBehaviour
	{
		public float minDuration = 8f;

		public float maxDuration = 10f;

		private float maxLifetime;

		private bool earlyStop;

		private IEnumerator Start()
		{
			ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();
			ParticleSystem[] array = systems;
			foreach (ParticleSystem system in array)
			{
				maxLifetime = Mathf.Max(system.startLifetime, maxLifetime);
			}
			float stopTime = Time.time + Random.Range(minDuration, maxDuration);
			while (Time.time < stopTime || earlyStop)
			{
				yield return null;
			}
			Debug.Log("stopping " + base.name);
			ParticleSystem[] array2 = systems;
			foreach (ParticleSystem system2 in array2)
			{
				system2.enableEmission = false;
			}
			BroadcastMessage("Extinguish", SendMessageOptions.DontRequireReceiver);
			yield return new WaitForSeconds(maxLifetime);
			Object.Destroy(base.gameObject);
		}

		public void Stop()
		{
			earlyStop = true;
		}
	}
}
