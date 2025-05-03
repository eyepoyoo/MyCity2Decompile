using UnityEngine;

namespace UnitySampleAssets.Effects
{
	public class WaterHoseParticles : MonoBehaviour
	{
		private ParticleCollisionEvent[] collisionEvents = new ParticleCollisionEvent[16];

		public static float lastSoundTime;

		public float force = 1f;

		private void OnParticleCollision(GameObject other)
		{
			int safeCollisionEventSize = GetComponent<ParticleSystem>().GetSafeCollisionEventSize();
			if (collisionEvents.Length < safeCollisionEventSize)
			{
				collisionEvents = new ParticleCollisionEvent[safeCollisionEventSize];
			}
			int num = GetComponent<ParticleSystem>().GetCollisionEvents(other, collisionEvents);
			for (int i = 0; i < num; i++)
			{
				if (Time.time > lastSoundTime + 0.2f)
				{
					lastSoundTime = Time.time;
				}
				Collider collider = (Collider)collisionEvents[i].colliderComponent;
				if (collider.attachedRigidbody != null)
				{
					Vector3 velocity = collisionEvents[i].velocity;
					collider.attachedRigidbody.AddForce(velocity * force, ForceMode.Impulse);
				}
				other.BroadcastMessage("Extinguish", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
