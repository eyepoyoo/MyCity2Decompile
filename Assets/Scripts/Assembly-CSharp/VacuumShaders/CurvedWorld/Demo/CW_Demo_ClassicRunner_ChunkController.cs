using UnityEngine;

namespace VacuumShaders.CurvedWorld.Demo
{
	[AddComponentMenu("VacuumShaders/Curved World/Demo/Classic Runner/Chunk Controller")]
	public class CW_Demo_ClassicRunner_ChunkController : MonoBehaviour
	{
		public Transform lastChunk;

		public static CW_Demo_ClassicRunner_ChunkController get;

		private void Start()
		{
			get = this;
		}

		private void Update()
		{
			get = this;
		}

		public Vector3 GetLastChunkPosition()
		{
			return lastChunk.position;
		}
	}
}
