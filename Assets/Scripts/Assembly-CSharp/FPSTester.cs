using UnityEngine;

public class FPSTester : MonoBehaviour
{
	public GameObject prefab;

	public Transform topRightBackSpawnPos;

	public Transform bottomLeftFrontSpawnPos;

	public float spawnInterval = 0.5f;

	public int numSpawns;

	private int lastSpawn;

	private void Update()
	{
		if (FrameRateMonitor.Instance.CurrentFrameRate >= 8)
		{
			int num = Mathf.FloorToInt(Time.realtimeSinceStartup / spawnInterval);
			if (num > lastSpawn)
			{
				numSpawns++;
				lastSpawn = num;
				Vector3 zero = Vector3.zero;
				zero.x = bottomLeftFrontSpawnPos.position.x + Random.value * (topRightBackSpawnPos.position.x - bottomLeftFrontSpawnPos.position.x);
				zero.y = bottomLeftFrontSpawnPos.position.y + Random.value * (topRightBackSpawnPos.position.y - bottomLeftFrontSpawnPos.position.y);
				zero.z = bottomLeftFrontSpawnPos.position.z + Random.value * (topRightBackSpawnPos.position.z - bottomLeftFrontSpawnPos.position.z);
				GameObject gameObject = (GameObject)Object.Instantiate(prefab, zero, Quaternion.identity);
				gameObject.transform.parent = base.transform;
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (!(topRightBackSpawnPos == null) && !(bottomLeftFrontSpawnPos == null))
		{
			Vector3 position = bottomLeftFrontSpawnPos.position;
			Vector3 position2 = topRightBackSpawnPos.position;
			Vector3 vector = new Vector3(position.x, position.y, position2.z);
			Vector3 vector2 = new Vector3(position2.x, position.y, position2.z);
			Vector3 vector3 = new Vector3(position2.x, position.y, position.z);
			Gizmos.DrawLine(vector, position);
			Gizmos.DrawLine(position, vector3);
			Gizmos.DrawLine(vector3, vector2);
			Gizmos.DrawLine(vector2, vector);
			Vector3 vector4 = new Vector3(position.x, position2.y, position2.z);
			Vector3 vector5 = new Vector3(position.x, position2.y, position.z);
			Vector3 vector6 = new Vector3(position2.x, position2.y, position.z);
			Gizmos.DrawLine(vector4, vector5);
			Gizmos.DrawLine(vector5, vector6);
			Gizmos.DrawLine(vector6, position2);
			Gizmos.DrawLine(position2, vector4);
			Gizmos.DrawLine(vector5, position);
			Gizmos.DrawLine(vector6, vector3);
			Gizmos.DrawLine(position2, vector2);
			Gizmos.DrawLine(vector4, vector);
		}
	}
}
