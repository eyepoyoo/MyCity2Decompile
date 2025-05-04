using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectCache
{
	public GameObject prefab;

	private List<GameObject> objects;

	private int cacheIndex;

	public GameObject group;

	public int cacheSize
	{
		get
		{
			return objects.Count;
		}
	}

	public void Initialize(GameObject spawnerGroup, int cacheSize)
	{
		objects = new List<GameObject>();
		group = new GameObject(prefab.name + " cache");
		group.transform.parent = spawnerGroup.transform;
		for (int i = 0; i < cacheSize; i++)
		{
			objects.Add(UnityEngine.Object.Instantiate(prefab));
			objects[i].SetActive(false);
			objects[i].transform.parent = group.transform;
		}
	}

	public void incrementCacheSize()
	{
		objects.Add(UnityEngine.Object.Instantiate(prefab));
		objects[objects.Count - 1].SetActive(false);
		objects[objects.Count - 1].transform.parent = group.transform;
	}

	public void clearCache()
	{
		for (int num = objects.Count - 1; num >= 0; num--)
		{
			GameObject gameObject = objects[num];
			PrefabSpawner.removeObjectFromTaglist(gameObject);
			objects.RemoveAt(num);
			UnityEngine.Object.Destroy(gameObject);
		}
		cacheIndex = 0;
	}

	public GameObject GetNextObjectInCache()
	{
		GameObject gameObject = null;
		for (int i = 0; i < objects.Count; i++)
		{
			gameObject = objects[cacheIndex];
			if (gameObject == null)
			{
				objects.RemoveAt(cacheIndex);
				cacheIndex--;
			}
			else if (!gameObject.activeSelf)
			{
				break;
			}
			cacheIndex = ((objects.Count != 0) ? ((cacheIndex + 1) % objects.Count) : 0);
		}
		if (gameObject == null || gameObject.activeSelf)
		{
			Debug.LogWarning("PrefabSpawner ObjectCache: Cache size for [" + prefab.name + "] exceeded! Instantiating and increasing cache size to [" + (objects.Count + 1) + "]");
			objects.Add(UnityEngine.Object.Instantiate(prefab));
			objects[objects.Count - 1].SetActive(false);
			gameObject = objects[objects.Count - 1];
		}
		gameObject.transform.parent = null;
		cacheIndex = (cacheIndex + 1) % objects.Count;
		return gameObject;
	}
}
