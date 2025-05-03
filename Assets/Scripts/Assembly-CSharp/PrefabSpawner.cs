using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PrefabSpawner
{
	private const string UNTAGGED = "Untagged";

	private static Dictionary<string, GameObject> resourcesPrefabs;

	private static Dictionary<string, TagKeeper> objectsWithTags;

	private static GameObject prefabSpawnerInstance;

	private static PrefabSpawnerInstance prefabSpawnerScript;

	public static int navMeshAgentWalkableMask = int.MaxValue;

	private static PrefabSpawnerInstance Instance
	{
		get
		{
			if (prefabSpawnerInstance == null && Application.isPlaying)
			{
				GameObject original = (GameObject)Resources.Load("PrefabSpawner");
				prefabSpawnerInstance = UnityEngine.Object.Instantiate(original);
				prefabSpawnerScript = prefabSpawnerInstance.GetComponent<PrefabSpawnerInstance>();
				prefabSpawnerScript.initialise();
			}
			return prefabSpawnerScript;
		}
	}

	public static int numObjectsCached
	{
		get
		{
			int num = 0;
			for (int num2 = Instance.caches.Count - 1; num2 >= 0; num2--)
			{
				num += Instance.caches[num2].cacheSize;
			}
			return num;
		}
	}

	public static GameObject Spawn(string resourcePrefabName, Vector3 position, Quaternion rotation)
	{
		if (resourcesPrefabs == null)
		{
			resourcesPrefabs = new Dictionary<string, GameObject>();
		}
		if (resourcesPrefabs.ContainsKey(resourcePrefabName))
		{
			return Spawn(resourcesPrefabs[resourcePrefabName], position, rotation);
		}
		GameObject gameObject = Resources.Load(resourcePrefabName) as GameObject;
		if (gameObject != null)
		{
			resourcesPrefabs.Add(resourcePrefabName, gameObject);
			return Spawn(gameObject, position, rotation);
		}
		return null;
	}

	public static GameObject Spawn(GameObject prefab, [Optional] Vector3 position, [Optional] Quaternion rotation)
	{
		if (prefab == null)
		{
			return null;
		}
		GameObject gameObject = Instance.Spawn(prefab, position, rotation);
		AddObjectToTaglist(gameObject);
		return gameObject;
	}

	public static IEnumerator PrePopulateCache(GameObject prefab, int minQuantity)
	{
		if (prefab != null)
		{
			IEnumerator e = Instance.PrePopulateCache(prefab, minQuantity);
			while (e.MoveNext())
			{
				yield return e.Current;
			}
		}
	}

	public static IEnumerator PrePopulateCache(string resourcesPrefabName, int minQuantity)
	{
		if (resourcesPrefabs == null)
		{
			resourcesPrefabs = new Dictionary<string, GameObject>();
		}
		GameObject resourcePrefab = null;
		if (resourcesPrefabs.ContainsKey(resourcesPrefabName))
		{
			resourcePrefab = resourcesPrefabs[resourcesPrefabName];
		}
		else
		{
			resourcePrefab = Resources.Load(resourcesPrefabName) as GameObject;
			if (resourcePrefab != null)
			{
				resourcesPrefabs.Add(resourcesPrefabName, resourcePrefab);
			}
		}
		if (resourcePrefab != null)
		{
			IEnumerator e = Instance.PrePopulateCache(resourcePrefab, minQuantity);
			while (e.MoveNext())
			{
				yield return e.Current;
			}
		}
	}

	public static void Destroy(GameObject objectToDestroy)
	{
		removeObjectFromTaglist(objectToDestroy);
		Instance.Destroy(objectToDestroy);
	}

	public static void EmptyCache(Action onComplete)
	{
		Instance.EmptyCache(onComplete);
	}

	public static IEnumerator EmptyCacheCoroutine(Action onComplete)
	{
		IEnumerator e = Instance.EmptyCacheCoroutine(onComplete);
		while (e.MoveNext())
		{
			yield return e.Current;
		}
	}

	public static void EmptyObjectFromCache(GameObject objectToRemove)
	{
		Instance.EmptyObjectFromCache(objectToRemove);
	}

	private static void AddObjectToTaglist(GameObject objectToAdd)
	{
		if (!(objectToAdd.tag == "Untagged"))
		{
			if (objectsWithTags == null)
			{
				objectsWithTags = new Dictionary<string, TagKeeper>();
			}
			if (!objectsWithTags.ContainsKey(objectToAdd.tag))
			{
				objectsWithTags.Add(objectToAdd.tag, new TagKeeper());
			}
			TagKeeper tagKeeper = objectsWithTags[objectToAdd.tag];
			if (!tagKeeper.objectsWithTag.Contains(objectToAdd))
			{
				tagKeeper.objectsWithTag.Add(objectToAdd);
			}
		}
	}

	public static void changeTagOnObject(GameObject objectInExistingTaglist, string newTag)
	{
		if (objectInExistingTaglist.tag == "Untagged")
		{
			return;
		}
		if (objectsWithTags == null)
		{
			objectsWithTags = new Dictionary<string, TagKeeper>();
		}
		if (objectsWithTags.ContainsKey(objectInExistingTaglist.tag))
		{
			TagKeeper tagKeeper = objectsWithTags[objectInExistingTaglist.tag];
			if (tagKeeper.objectsWithTag.Contains(objectInExistingTaglist))
			{
				tagKeeper.objectsWithTag.Remove(objectInExistingTaglist);
			}
		}
		if (!objectsWithTags.ContainsKey(newTag))
		{
			objectsWithTags.Add(newTag, new TagKeeper());
		}
		objectInExistingTaglist.tag = newTag;
		TagKeeper tagKeeper2 = objectsWithTags[objectInExistingTaglist.tag];
		if (!tagKeeper2.objectsWithTag.Contains(objectInExistingTaglist))
		{
			tagKeeper2.objectsWithTag.Add(objectInExistingTaglist);
		}
	}

	public static void removeObjectFromTaglist(GameObject objectToRemove)
	{
		if (!(objectToRemove == null) && !(objectToRemove.tag == "Untagged") && objectsWithTags != null && objectsWithTags.ContainsKey(objectToRemove.tag))
		{
			TagKeeper tagKeeper = objectsWithTags[objectToRemove.tag];
			if (tagKeeper.objectsWithTag.Contains(objectToRemove))
			{
				tagKeeper.objectsWithTag.Remove(objectToRemove);
			}
		}
	}

	public static GameObject[] FindGameObjectsWithTag(string[] tags)
	{
		List<GameObject> list = new List<GameObject>();
		for (int num = tags.Length - 1; num >= 0; num--)
		{
			list.AddRange(FindGameObjectsWithTag(tags[num]));
		}
		return list.ToArray();
	}

	public static GameObject[] FindGameObjectsWithTag(string tag)
	{
		if (objectsWithTags == null)
		{
			objectsWithTags = new Dictionary<string, TagKeeper>();
		}
		if (!objectsWithTags.ContainsKey(tag))
		{
			objectsWithTags.Add(tag, new TagKeeper());
		}
		TagKeeper tagKeeper = objectsWithTags[tag];
		tagKeeper.objectsWithTag.RemoveAll((GameObject item) => item == null || !item.activeSelf);
		return tagKeeper.objectsWithTag.ToArray();
	}

	public static void FindGameObjectsWithTags(string[] tags, ref List<GameObject> result)
	{
		if (objectsWithTags == null)
		{
			objectsWithTags = new Dictionary<string, TagKeeper>();
		}
		for (int num = tags.Length - 1; num >= 0; num--)
		{
			if (!objectsWithTags.ContainsKey(tags[num]))
			{
				objectsWithTags.Add(tags[num], new TagKeeper());
			}
			TagKeeper tagKeeper = objectsWithTags[tags[num]];
			tagKeeper.objectsWithTag.RemoveAll((GameObject item) => item == null || !item.activeSelf);
			result.AddRange(tagKeeper.objectsWithTag);
		}
	}

	public static void FindGameObjectsWithTag(string tag, ref List<GameObject> result)
	{
		if (objectsWithTags == null)
		{
			objectsWithTags = new Dictionary<string, TagKeeper>();
		}
		if (!objectsWithTags.ContainsKey(tag))
		{
			objectsWithTags.Add(tag, new TagKeeper());
		}
		TagKeeper tagKeeper = objectsWithTags[tag];
		tagKeeper.objectsWithTag.RemoveAll((GameObject item) => item == null || !item.activeSelf);
		result = tagKeeper.objectsWithTag;
	}
}
