using UnityEngine;

namespace VacuumShaders.CurvedWorld.Demo
{
	[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
	[AddComponentMenu("VacuumShaders/Curved World/Demo/Little Planet/Horse")]
	public class CW_Demo_LittlePlanet_Horse : MonoBehaviour
	{
		private UnityEngine.AI.NavMeshAgent agent;

		private void Start()
		{
			agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
			agent.speed *= Random.Range(1.2f, 0.8f);
			SetDestination();
		}

		private void FixedUpdate()
		{
			if (Vector3.Distance(base.transform.position, agent.destination) < 1f)
			{
				SetDestination();
			}
		}

		private void OnCollisionEnter(Collision collision)
		{
			Rigidbody component = collision.gameObject.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.AddForce((base.transform.forward + Vector3.up) * Random.Range(5f, 10f), ForceMode.Impulse);
			}
			if (collision.gameObject.name == "Border")
			{
				SetDestination();
			}
		}

		private void SetDestination()
		{
			Vector3 destination = Random.onUnitSphere * 40f;
			destination.y = 0f;
			agent.SetDestination(destination);
		}
	}
}
