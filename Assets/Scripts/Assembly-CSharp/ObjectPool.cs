using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
	private readonly Dictionary<string, List<PoolableObject>> _pools = new Dictionary<string, List<PoolableObject>>();

	public readonly List<PoolableObject> _activeObjects = new List<PoolableObject>();

	private void Awake()
	{
		base.transform.position = Vector3.down * 999f;
	}

	public PoolableObject GetObject(string prefabName, bool doActivate = true)
	{
		if (!_pools.ContainsKey(prefabName) || _pools[prefabName].Count == 0)
		{
			CreateAndPoolObject(prefabName);
		}
		PoolableObject poolableObject = _pools[prefabName][0];
		_pools[prefabName].RemoveAt(0);
		_activeObjects.Add(poolableObject);
		poolableObject._isActive = true;
		if (doActivate)
		{
			poolableObject.Activate();
		}
		return poolableObject;
	}

	public void CreateAndPoolObject(string prefabName, int num)
	{
		for (int i = 0; i < num; i++)
		{
			CreateAndPoolObject(prefabName);
		}
	}

	public PoolableObject CreateAndPoolObject(string prefabName)
	{
		return PoolObject((GameObject)Object.Instantiate(Resources.Load(prefabName)), prefabName);
	}

	public PoolableObject PoolObject(GameObject obj, string prefabName)
	{
		if (!_pools.ContainsKey(prefabName))
		{
			_pools.Add(prefabName, new List<PoolableObject>());
		}
		PoolableObject poolableObject = obj.GetComponent<PoolableObject>();
		if (!poolableObject)
		{
			poolableObject = obj.AddComponent<PoolableObject>();
		}
		poolableObject._objectPool = this;
		poolableObject._prefabName = prefabName;
		ReturnObject(poolableObject);
		return poolableObject;
	}

	public void ReturnObject(PoolableObject obj)
	{
		obj._isActive = false;
		obj.transform.parent = base.transform;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.identity;
		_pools[obj._prefabName].Add(obj);
		_activeObjects.Remove(obj);
	}

	private void OnDestroy()
	{
		foreach (List<PoolableObject> value in _pools.Values)
		{
			value.Clear();
		}
		_pools.Clear();
	}
}
