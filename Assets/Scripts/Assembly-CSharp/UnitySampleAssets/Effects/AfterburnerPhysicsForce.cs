using UnityEngine;

namespace UnitySampleAssets.Effects
{
	[RequireComponent(typeof(SphereCollider))]
	public class AfterburnerPhysicsForce : MonoBehaviour
	{
		public float effectAngle = 15f;

		public float effectWidth = 1f;

		public float effectDistance = 10f;

		public float force = 10f;

		private Collider[] cols;

		private float r;

		private SphereCollider sphere;

		private void Start()
		{
			sphere = GetComponent<Collider>() as SphereCollider;
		}

		private void FixedUpdate()
		{
			cols = Physics.OverlapSphere(base.transform.position + sphere.center, sphere.radius);
			for (int i = 0; i < cols.Length; i++)
			{
				if (cols[i].attachedRigidbody != null)
				{
					Vector3 current = base.transform.InverseTransformPoint(cols[i].transform.position);
					current = Vector3.MoveTowards(current, new Vector3(0f, 0f, current.z), effectWidth * 0.5f);
					float value = Mathf.Abs(Mathf.Atan2(current.x, current.z) * 57.29578f);
					float num = Mathf.InverseLerp(effectDistance, 0f, current.magnitude);
					num *= Mathf.InverseLerp(effectAngle, 0f, value);
					Vector3 vector = cols[i].transform.position - base.transform.position;
					cols[i].attachedRigidbody.AddForceAtPosition(vector.normalized * force * num, Vector3.Lerp(cols[i].transform.position, base.transform.TransformPoint(0f, 0f, current.z), 0.1f));
				}
			}
		}

		private void OnDrawGizmosSelected()
		{
			(GetComponent<Collider>() as SphereCollider).radius = effectDistance * 0.5f;
			(GetComponent<Collider>() as SphereCollider).center = new Vector3(0f, 0f, effectDistance * 0.5f);
			Vector3[] array = new Vector3[4]
			{
				Vector3.up,
				-Vector3.up,
				Vector3.right,
				-Vector3.right
			};
			Vector3[] array2 = new Vector3[4]
			{
				-Vector3.right,
				Vector3.right,
				Vector3.up,
				-Vector3.up
			};
			Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
			for (int i = 0; i < 4; i++)
			{
				Vector3 vector = base.transform.position + base.transform.rotation * array[i] * effectWidth * 0.5f;
				Vector3 vector2 = base.transform.TransformDirection(Quaternion.AngleAxis(effectAngle, array2[i]) * Vector3.forward);
				Gizmos.DrawLine(vector, vector + vector2 * (GetComponent<Collider>() as SphereCollider).radius * 2f);
			}
		}
	}
}
