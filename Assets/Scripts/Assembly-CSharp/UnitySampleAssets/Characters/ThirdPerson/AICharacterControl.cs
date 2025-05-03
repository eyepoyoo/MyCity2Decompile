using UnityEngine;

namespace UnitySampleAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
	[RequireComponent(typeof(ThirdPersonCharacter))]
	public class AICharacterControl : MonoBehaviour
	{
		public Transform target;

		public float targetChangeTolerance = 1f;

		private Vector3 targetPos;

		public UnityEngine.AI.NavMeshAgent agent { get; private set; }

		public ThirdPersonCharacter character { get; private set; }

		private void Start()
		{
			agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
			character = GetComponent<ThirdPersonCharacter>();
		}

		private void Update()
		{
			if (target != null)
			{
				if ((target.position - targetPos).magnitude > targetChangeTolerance)
				{
					targetPos = target.position;
					agent.SetDestination(targetPos);
				}
				agent.transform.position = base.transform.position;
				character.Move(agent.desiredVelocity, false, false, targetPos);
			}
			else
			{
				character.Move(Vector3.zero, false, false, base.transform.position + base.transform.forward * 100f);
			}
		}

		public void SetTarget(Transform target)
		{
			this.target = target;
		}
	}
}
