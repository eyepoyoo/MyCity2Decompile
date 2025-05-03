using UnityEngine;

namespace VacuumShaders.CurvedWorld.Demo
{
	[AddComponentMenu("VacuumShaders/Curved World/Demo/Classic Runner/Translate")]
	public class CW_Demo_ClassicRunner_Translate : MonoBehaviour
	{
		public Vector3 direction;

		private void Start()
		{
		}

		private void Update()
		{
			base.transform.Translate(direction * Time.deltaTime);
		}

		private void FixedUpdate()
		{
			if (base.transform.position.z < -20f && CW_Demo_ClassicRunner_ChunkController.get != null)
			{
				Vector3 lastChunkPosition = CW_Demo_ClassicRunner_ChunkController.get.GetLastChunkPosition();
				lastChunkPosition.z += 10f;
				base.transform.position = lastChunkPosition;
				CW_Demo_ClassicRunner_ChunkController.get.lastChunk = base.transform;
			}
		}
	}
}
