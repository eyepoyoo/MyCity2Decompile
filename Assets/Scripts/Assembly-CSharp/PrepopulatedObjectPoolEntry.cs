using System;
using UnityEngine;

[Serializable]
public class PrepopulatedObjectPoolEntry
{
	public string name = string.Empty;

	public GameObject objectPrefab;

	public int poolSize = 1;

	public bool doAllowOverflow;

	[HideInInspector]
	public GameObject poolRoot;

	[HideInInspector]
	public GameObject[] instancePool;

	public GameObject removeObjectFromPool()
	{
		if (instancePool == null)
		{
			Debug.LogWarning("ObjectPool: Pool [" + name + "] was Null!");
			return null;
		}
		if (instancePool.Length <= 0)
		{
			if (!doAllowOverflow)
			{
				Debug.LogWarning("ObjectPool: Pool [" + name + "] was Empty! Consider increasing pool size.");
				return null;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(objectPrefab);
			gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);
			poolSize++;
			return gameObject;
		}
		GameObject gameObject2 = instancePool[0];
		GameObject[] array = new GameObject[instancePool.Length - 1];
		for (int i = 1; i < instancePool.Length; i++)
		{
			array[i - 1] = instancePool[i];
			instancePool[i] = null;
		}
		instancePool = null;
		instancePool = array;
		gameObject2.transform.parent = null;
		gameObject2.SetActive(true);
		return gameObject2;
	}

	public void addObjectToPool(GameObject poolObject)
	{
		if (instancePool == null || poolRoot == null || poolObject == null)
		{
			Debug.LogError("ObjectPool: Pool, pool root or pool object was null!");
			return;
		}
		GameObject[] array = new GameObject[instancePool.Length + 1];
		for (int i = 0; i < instancePool.Length; i++)
		{
			array[i] = instancePool[i];
			instancePool[i] = null;
		}
		poolObject.transform.position = poolRoot.transform.position;
		poolObject.transform.rotation = poolRoot.transform.rotation;
		poolObject.transform.parent = poolRoot.transform;
		poolObject.SetActive(false);
		array[array.Length - 1] = poolObject;
		instancePool = null;
		instancePool = array;
	}
}
