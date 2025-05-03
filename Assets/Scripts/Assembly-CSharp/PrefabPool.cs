using System;
using System.Collections.Generic;
using AmuzoEngine;
using GameDefines;
using UnityEngine;

public class PrefabPool : MonoBehaviour, IObjectInstantiator
{
	public enum EInstantiateFailedPolicy
	{
		FAIL = 0,
		USE_OLDEST = 1
	}

	[Serializable]
	public class PrefabInfo
	{
		public UnityEngine.Object _prefab;

		public int _count;
	}

	private const string LOG_TAG = "[PrefabPool] ";

	public EInitializeType _initType;

	public PrefabInfo[] _prefabs;

	public string _instanceName;

	public bool _isShuffleInstances;

	public bool _isCreateOffline;

	public EInstantiateFailedPolicy _onInstantiateFailed;

	public GameObject[] _instances;

	private bool _isInitialized;

	private ObjectPool<GameObject> _instancePool;

	private string _pLogTag
	{
		get
		{
			return "[PrefabPool:" + base.gameObject.name + "] ";
		}
	}

	public bool _pCanReuseInstances
	{
		get
		{
			return _onInstantiateFailed == EInstantiateFailedPolicy.USE_OLDEST;
		}
	}

	UnityEngine.Object IObjectInstantiator.CreateInstance()
	{
		return AllocateInstance();
	}

	void IObjectInstantiator.DestroyInstance(UnityEngine.Object inst)
	{
		FreeInstance((GameObject)inst);
	}

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

	public bool OfflineCreateInstances()
	{
		if (Application.isPlaying)
		{
			Debug.LogError(_pLogTag + "Offline create not allowed when playing", base.gameObject);
			return false;
		}
		if (!_isCreateOffline)
		{
			Debug.LogError(_pLogTag + "Offline create not allowed by facade", base.gameObject);
			return false;
		}
		CreateInstances();
		return true;
	}

	public void Initialize()
	{
		if (!_isInitialized)
		{
			if (!_isCreateOffline)
			{
				CreateInstances();
			}
			PoolInstances();
			_isInitialized = true;
		}
	}

	public GameObject Instantiate()
	{
		GameObject gameObject = AllocateInstance();
		if (gameObject == null)
		{
			EInstantiateFailedPolicy onInstantiateFailed = _onInstantiateFailed;
			if (onInstantiateFailed == EInstantiateFailedPolicy.USE_OLDEST)
			{
				gameObject = ReallocateOldestInstance();
			}
		}
		return gameObject;
	}

	public GameObject Instantiate(Vector3 position, Quaternion rotation)
	{
		GameObject gameObject = AllocateInstance();
		if (gameObject == null)
		{
			EInstantiateFailedPolicy onInstantiateFailed = _onInstantiateFailed;
			if (onInstantiateFailed == EInstantiateFailedPolicy.USE_OLDEST)
			{
				gameObject = ReallocateOldestInstance();
			}
		}
		if (gameObject != null)
		{
			gameObject.transform.position = position;
			gameObject.transform.rotation = rotation;
		}
		return gameObject;
	}

	public void Destroy(GameObject instance)
	{
		FreeInstance(instance);
	}

	private void CreateInstances()
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < _prefabs.Length; i++)
		{
			if (_prefabs[i] == null)
			{
				continue;
			}
			for (int j = 0; j < _prefabs[i]._count; j++)
			{
				if (_prefabs[i]._prefab == null)
				{
					continue;
				}
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(_prefabs[i]._prefab);
				if (gameObject == null)
				{
					Debug.LogWarning(_pLogTag + "Failed to instantiate instance " + i + " from prefab '" + _prefabs[i]._prefab.name + "'", base.gameObject);
				}
				else
				{
					gameObject.transform.parent = base.gameObject.transform;
					if (_instanceName != null && _instanceName.Length > 0)
					{
						gameObject.name = _instanceName;
					}
					gameObject.SetActive(false);
					list.Add(gameObject);
				}
			}
		}
		if (_isShuffleInstances)
		{
			Utils.Shuffle(list);
		}
		_instances = list.ToArray();
	}

	private void PoolInstances()
	{
		if (_instances != null)
		{
			int nextInstance = 0;
			_instancePool = new ObjectPool<GameObject>(_instances.Length, () => _instances[nextInstance++], null);
		}
	}

	private GameObject AllocateInstance()
	{
		if (!_isInitialized)
		{
			Debug.LogError(_pLogTag + "Not initialized", base.gameObject);
			return null;
		}
		if (_instancePool == null)
		{
			return null;
		}
		GameObject gameObject = _instancePool.Allocate();
		if (gameObject == null)
		{
			return null;
		}
		gameObject.SetActive(true);
		return gameObject;
	}

	private void FreeInstance(GameObject instance)
	{
		instance.SetActive(false);
		if (_instancePool != null)
		{
			_instancePool.Free(instance);
		}
	}

	private GameObject ReallocateOldestInstance()
	{
		if (_instancePool == null)
		{
			return null;
		}
		_instancePool.FreeOldest();
		GameObject gameObject = _instancePool.Allocate();
		if (gameObject == null)
		{
			return null;
		}
		gameObject.SetActive(true);
		return gameObject;
	}
}
