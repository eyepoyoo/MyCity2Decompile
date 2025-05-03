using AmuzoEngine;
using UnityEngine;

public class PrefabPoolManagerInstantiator : MonoBehaviour, IObjectInstantiator
{
	private const string LOG_TAG = "[PrefabPoolManagerInstantiator] ";

	public PrefabPoolManager _poolManager;

	public string _poolName;

	bool IObjectInstantiator._pCanReuseInstances
	{
		get
		{
			return _poolManager != null && _poolManager.CanReuseInstances(_poolName);
		}
	}

	Object IObjectInstantiator.CreateInstance()
	{
		if (_poolManager == null)
		{
			return null;
		}
		return _poolManager.Instantiate(_poolName);
	}

	void IObjectInstantiator.DestroyInstance(Object inst)
	{
		if (!(_poolManager == null))
		{
			_poolManager.Destroy(_poolName, (GameObject)inst);
		}
	}
}
