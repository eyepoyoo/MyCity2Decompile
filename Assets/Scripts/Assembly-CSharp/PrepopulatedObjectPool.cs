using System.Collections.Generic;
using UnityEngine;

public class PrepopulatedObjectPool : MonoBehaviour
{
	public static PrepopulatedObjectPool Instance;

	public PrepopulatedObjectPoolEntry[] objectPools;

	private void OnDestroy()
	{
		Instance = null;
	}

	private void Awake()
	{
		Instance = this;
	}

	public bool hasAvalibleObject(string[] objectNames)
	{
		if (objectNames == null || objectNames.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < objectNames.Length; i++)
		{
			if (!string.IsNullOrEmpty(objectNames[i]) && hasAvalibleObject(objectNames[i]))
			{
				return true;
			}
		}
		return false;
	}

	public bool hasAvalibleObject(string objectName)
	{
		if (objectPools == null || objectPools.Length <= 0)
		{
			return false;
		}
		for (int i = 0; i < objectPools.Length; i++)
		{
			if (checkObjectPool(objectPools[i], objectName))
			{
				return true;
			}
		}
		return false;
	}

	public GameObject getObject(string[] objectNames)
	{
		if (objectPools == null || objectPools.Length <= 0)
		{
			Debug.LogError("ObjectPool: No objects defined! Cannot return object");
			return null;
		}
		List<int> list = new List<int>();
		for (int i = 0; i < objectNames.Length; i++)
		{
			for (int j = 0; j < objectPools.Length; j++)
			{
				if (checkObjectPool(objectPools[j], objectNames[i]))
				{
					list.Add(j);
					break;
				}
			}
		}
		if (list.Count <= 0)
		{
			return null;
		}
		return objectPools[list[Random.Range(0, list.Count)]].removeObjectFromPool();
	}

	public GameObject getObject(string objectName)
	{
		if (objectPools == null || objectPools.Length <= 0)
		{
			Debug.LogError("ObjectPool: No objects defined! Cannot return object");
			return null;
		}
		for (int i = 0; i < objectPools.Length; i++)
		{
			if (namecheckObjectPool(objectPools[i], objectName))
			{
				return objectPools[i].removeObjectFromPool();
			}
		}
		Debug.LogWarning("ObjectPool: Unable to find object pool for [" + objectName + "]");
		return null;
	}

	public void disposeOfObject(GameObject objectToDispose)
	{
		if (objectToDispose == null || objectPools == null || objectPools.Length <= 0)
		{
			Debug.LogError("ObjectPool: Object or object pools were null or empty. Cannot return object to pool");
			Utils.DestroyGameObject(objectToDispose);
			return;
		}
		for (int i = 0; i < objectPools.Length; i++)
		{
			if (!(objectPools[i].name != objectToDispose.name))
			{
				objectPools[i].addObjectToPool(objectToDispose);
				return;
			}
		}
		Utils.DestroyGameObject(objectToDispose);
	}

	private bool namecheckObjectPool(PrepopulatedObjectPoolEntry entry, string name)
	{
		if (entry == null || entry.name != name)
		{
			return false;
		}
		return true;
	}

	private bool checkObjectPool(PrepopulatedObjectPoolEntry entry, string name)
	{
		if (entry == null || entry.name != name)
		{
			return false;
		}
		if (entry.instancePool == null || (entry.instancePool.Length <= 0 && !entry.doAllowOverflow))
		{
			return false;
		}
		return true;
	}
}
