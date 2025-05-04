using System;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
	[NonSerialized]
	public ObjectPool _objectPool;

	public bool _isActive;

	[NonSerialized]
	public string _prefabName;

	public virtual void ReturnToPool()
	{
		_objectPool.ReturnObject(this);
	}

	public virtual void Activate()
	{
	}
}
