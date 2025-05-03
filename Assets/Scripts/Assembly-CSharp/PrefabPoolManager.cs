using System;
using System.Collections.Generic;
using GameDefines;
using UnityEngine;

public class PrefabPoolManager : MonoBehaviour
{
	[Serializable]
	public class PrefabPoolInfo
	{
		public string _name;

		public PrefabPool.PrefabInfo[] _prefabs;

		public PrefabPool.EInstantiateFailedPolicy _onInstantiateFailed;

		public PrefabPool _pool;
	}

	private const string LOG_TAG = "[PrefabPoolManager] ";

	private const string POOL_NAME_PREFIX = "PrefabPool_";

	public EInitializeType _initType;

	public PrefabPoolInfo[] _pools;

	public bool _isCreateOffline;

	private bool _isInitialized;

	private Dictionary<int, PrefabPool> _prefabPoolDict;

	private Dictionary<int, PrefabPool> _instanceSourceDict;

	private void Awake()
	{
		if (_initType == EInitializeType.AWAKE)
		{
			Initialize();
		}
	}

	private void Start()
	{
		if (_initType == EInitializeType.START)
		{
			Initialize();
		}
	}

	public bool OfflineCreatePools()
	{
		if (Application.isPlaying)
		{
			Debug.LogError("[PrefabPoolManager] Offline create not allowed when playing", base.gameObject);
			return false;
		}
		if (!_isCreateOffline)
		{
			Debug.LogError("[PrefabPoolManager] Offline create not allowed by manager", base.gameObject);
			return false;
		}
		CreatePools(true);
		if (_pools != null)
		{
			for (int i = 0; i < _pools.Length; i++)
			{
				if (!(_pools[i]._pool == null))
				{
					_pools[i]._pool.OfflineCreateInstances();
				}
			}
		}
		return true;
	}

	public void Initialize()
	{
		if (_isInitialized)
		{
			return;
		}
		if (!_isCreateOffline)
		{
			CreatePools(false);
		}
		if (_pools != null)
		{
			for (int i = 0; i < _pools.Length; i++)
			{
				if (!(_pools[i]._pool == null))
				{
					_pools[i]._pool.Initialize();
				}
			}
		}
		RefreshDict();
		_isInitialized = true;
	}

	public GameObject Instantiate(string poolName)
	{
		if (poolName == null || poolName.Length == 0)
		{
			return null;
		}
		PrefabPool prefabPool = GetPrefabPool(poolName.GetHashCode());
		if (prefabPool == null)
		{
			Debug.LogError("[PrefabPoolManager] Failed to find pool: " + poolName, base.gameObject);
			return null;
		}
		GameObject gameObject = prefabPool.Instantiate();
		if (gameObject == null)
		{
			return null;
		}
		gameObject.name = poolName;
		return gameObject;
	}

	public GameObject Instantiate(string poolName, Vector3 position, Quaternion rotation)
	{
		if (poolName == null || poolName.Length == 0)
		{
			return null;
		}
		PrefabPool prefabPool = GetPrefabPool(poolName.GetHashCode());
		if (prefabPool == null)
		{
			Debug.LogError("[PrefabPoolManager] Failed to find pool: " + poolName, base.gameObject);
			return null;
		}
		GameObject gameObject = prefabPool.Instantiate(position, rotation);
		if (gameObject == null)
		{
			return null;
		}
		gameObject.name = poolName;
		return gameObject;
	}

	public void Destroy(string poolName, GameObject instance)
	{
		if (instance == null)
		{
			return;
		}
		if (poolName == null || poolName.Length == 0)
		{
			Debug.LogError("[PrefabPoolManager] Missing pool name", base.gameObject);
			return;
		}
		PrefabPool prefabPool = GetPrefabPool(poolName.GetHashCode());
		if (prefabPool == null)
		{
			Debug.LogError("[PrefabPoolManager] Failed to find pool: " + poolName, base.gameObject);
		}
		else
		{
			prefabPool.Destroy(instance);
		}
	}

	public void Destroy(GameObject instance)
	{
		if (instance == null)
		{
			return;
		}
		string text = instance.name;
		if (text == null || text.Length == 0)
		{
			Debug.LogError("[PrefabPoolManager] Instance unnamed", instance);
			return;
		}
		PrefabPool prefabPool = GetPrefabPool(text.GetHashCode());
		if (prefabPool == null)
		{
			Debug.LogError("[PrefabPoolManager] Failed to find pool: " + text, base.gameObject);
		}
		else
		{
			prefabPool.Destroy(instance);
		}
	}

	public bool CanReuseInstances(string poolName)
	{
		if (poolName == null || poolName.Length == 0)
		{
			return false;
		}
		PrefabPool prefabPool = GetPrefabPool(poolName.GetHashCode());
		if (prefabPool == null)
		{
			Debug.LogError("[PrefabPoolManager] Failed to find pool: " + poolName, base.gameObject);
			return false;
		}
		return prefabPool._pCanReuseInstances;
	}

	private void CreatePools(bool isOffline)
	{
		if (_pools == null)
		{
			return;
		}
		DestroyPools(isOffline);
		for (int i = 0; i < _pools.Length; i++)
		{
			if (_pools[i] != null)
			{
				if (_pools[i]._name == null || _pools[i]._name.Length == 0)
				{
					Debug.LogError("[PrefabPoolManager] Pool " + i + " unnamed", base.gameObject);
					continue;
				}
				GameObject gameObject = new GameObject("PrefabPool_" + _pools[i]._name);
				gameObject.transform.parent = base.gameObject.transform;
				_pools[i]._pool = gameObject.AddComponent<PrefabPool>();
				_pools[i]._pool._initType = EInitializeType.MANUAL;
				_pools[i]._pool._prefabs = _pools[i]._prefabs;
				_pools[i]._pool._isShuffleInstances = _pools[i]._prefabs.Length > 1;
				_pools[i]._pool._instanceName = _pools[i]._name;
				_pools[i]._pool._isCreateOffline = _isCreateOffline;
				_pools[i]._pool._onInstantiateFailed = _pools[i]._onInstantiateFailed;
			}
		}
	}

	private void DestroyPools(bool isImmediate)
	{
		if (_pools == null)
		{
			return;
		}
		Action<UnityEngine.Object> action = delegate(UnityEngine.Object o)
		{
			UnityEngine.Object.Destroy(o);
		};
		if (isImmediate)
		{
			action = delegate(UnityEngine.Object o)
			{
				UnityEngine.Object.DestroyImmediate(o);
			};
		}
		for (int num = 0; num < _pools.Length; num++)
		{
			if (!(_pools[num]._pool == null))
			{
				action(_pools[num]._pool.gameObject);
			}
		}
	}

	private void RefreshDict()
	{
		Utils.DictionaryAdd(ref _prefabPoolDict, true, _pools.Length, (int i) => _pools[i]._name.GetHashCode(), (int i) => _pools[i]._pool, (PrefabPool p) => p != null, delegate(string error)
		{
			Debug.LogError("[PrefabPoolManager] RefreshDict: " + error);
		});
	}

	private PrefabPool GetPrefabPool(int poolNameHash)
	{
		PrefabPool result = null;
		if (_prefabPoolDict != null)
		{
			if (_prefabPoolDict.ContainsKey(poolNameHash))
			{
				result = _prefabPoolDict[poolNameHash];
			}
		}
		else if (_pools != null)
		{
			PrefabPoolInfo prefabPoolInfo = Array.Find(_pools, (PrefabPoolInfo i) => i._name.GetHashCode() == poolNameHash);
			if (prefabPoolInfo != null)
			{
				result = prefabPoolInfo._pool;
			}
		}
		return result;
	}
}
