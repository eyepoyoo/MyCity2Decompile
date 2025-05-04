using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawnerInstance : MonoBehaviour
{
	public List<ObjectCache> caches;

	public Dictionary<GameObject, CachedObjectData> activeCachedObjects;

	private GameObject spawnerGroup;

	public void initialise()
	{
		spawnerGroup = base.gameObject;
		if (caches == null)
		{
			caches = new List<ObjectCache>();
		}
		activeCachedObjects = new Dictionary<GameObject, CachedObjectData>();
	}

	public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		ObjectCache objectCache = null;
		if (prefab == null)
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("Cannot spawn null prefab");
			}
			return null;
		}
		for (int i = 0; i < caches.Count; i++)
		{
			if (caches[i].prefab == prefab)
			{
				objectCache = caches[i];
			}
		}
		if (objectCache == null)
		{
			objectCache = new ObjectCache();
			objectCache.prefab = prefab;
			objectCache.Initialize(spawnerGroup, 1);
			caches.Add(objectCache);
		}
		GameObject nextObjectInCache = objectCache.GetNextObjectInCache();
		nextObjectInCache.transform.position = position;
		nextObjectInCache.transform.rotation = rotation;
		nextObjectInCache.SetActive(true);
		if (activeCachedObjects.ContainsKey(nextObjectInCache))
		{
			activeCachedObjects[nextObjectInCache].isActive = true;
		}
		else
		{
			CachedObjectData cachedObjectData = new CachedObjectData();
			cachedObjectData.isActive = true;
			cachedObjectData.cacheObjectBelongsTo = objectCache;
			activeCachedObjects.Add(nextObjectInCache, cachedObjectData);
		}
		return nextObjectInCache;
	}

	public void EmptyCache(Action onComplete)
	{
		StartCoroutine(EmptyCacheCoroutine(onComplete));
	}

	public IEnumerator EmptyCacheCoroutine(Action onComplete)
	{
		for (int i = caches.Count - 1; i >= 0; i--)
		{
			ObjectCache cacheToDestroy = caches[i];
			cacheToDestroy.clearCache();
			cacheToDestroy.prefab = new GameObject();
			UnityEngine.Object.Destroy(cacheToDestroy.prefab);
			UnityEngine.Object.Destroy(cacheToDestroy.group);
			caches.RemoveAt(i);
			yield return new WaitForEndOfFrame();
		}
		if (onComplete != null)
		{
			onComplete();
		}
	}

	public void EmptyObjectFromCache(GameObject objectToRemove)
	{
		for (int num = caches.Count - 1; num >= 0; num--)
		{
			if (!(caches[num].prefab != objectToRemove))
			{
				Debug.Log("Removing object cache for [" + objectToRemove.name + "]");
				ObjectCache objectCache = caches[num];
				objectCache.clearCache();
				objectCache.prefab = new GameObject();
				UnityEngine.Object.Destroy(objectCache.prefab);
				UnityEngine.Object.Destroy(objectCache.group);
				caches.RemoveAt(num);
			}
		}
	}

	public IEnumerator PrePopulateCache(GameObject prefab, int minQuantity)
	{
		bool doesExist = false;
		for (int i = 0; i < caches.Count; i++)
		{
			if (!(caches[i].prefab == prefab))
			{
				continue;
			}
			if (caches[i].cacheSize < minQuantity)
			{
				IEnumerator e = prePopulateCache(caches[i], minQuantity);
				while (e.MoveNext())
				{
					yield return e.Current;
				}
			}
			doesExist = true;
			break;
		}
		if (!doesExist)
		{
			ObjectCache cache = new ObjectCache();
			cache.prefab = prefab;
			cache.Initialize(spawnerGroup, 0);
			caches.Add(cache);
			IEnumerator e2 = prePopulateCache(cache, minQuantity);
			while (e2.MoveNext())
			{
				yield return e2.Current;
			}
		}
	}

	private IEnumerator prePopulateCache(ObjectCache cacheToIncrease, int minQuantity)
	{
		if (Debug.isDebugBuild)
		{
			Debug.Log("PrefabSpawnerInstance: Increasing cache for [" + cacheToIncrease.prefab.name + "] from [" + cacheToIncrease.cacheSize + "] => [" + minQuantity + "]");
		}
		do
		{
			yield return new WaitForEndOfFrame();
			cacheToIncrease.incrementCacheSize();
		}
		while (cacheToIncrease.cacheSize < minQuantity);
	}

	public void Destroy(GameObject objectToDestroy)
	{
		if (objectToDestroy == null)
		{
			return;
		}
		if (activeCachedObjects.ContainsKey(objectToDestroy))
		{
			if (activeCachedObjects[objectToDestroy].cacheObjectBelongsTo.group != null)
			{
				objectToDestroy.transform.parent = activeCachedObjects[objectToDestroy].cacheObjectBelongsTo.group.transform;
			}
			objectToDestroy.SetActive(false);
			activeCachedObjects[objectToDestroy].isActive = false;
		}
		else
		{
			UnityEngine.Object.Destroy(objectToDestroy);
		}
	}
}
