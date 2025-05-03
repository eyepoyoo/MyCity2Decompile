using UnityEngine;

public class PrefabLoader : MonoBehaviour
{
	public GameObject prefab;

	public string spawnPoint;

	private void OnSignal()
	{
		Transform transform = null;
		if (spawnPoint != null && spawnPoint != string.Empty)
		{
			GameObject gameObject = GameObject.Find(spawnPoint);
			if (gameObject != null)
			{
				transform = gameObject.transform;
			}
		}
		if (transform == null)
		{
			transform = base.transform;
		}
		Object.Instantiate(prefab, transform.position, transform.rotation);
	}
}
