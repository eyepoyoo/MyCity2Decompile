using System;
using System.Collections.Generic;
using AmuzoEngine;
using UnityEngine;

public class ExposedResources : MonoBehaviour
{
	[Serializable]
	private class Resource
	{
		public string _name;

		public UnityEngine.Object _target;

		public string _targetName = string.Empty;

		public ExposedResources _targetResources;
	}

	public const string RESOURCE_NAME_NULL = "";

	[SerializeField]
	private List<Resource> _resources;

	private Dictionary<string, Resource> _dict;

	private void OnEnable()
	{
		if (!Singleton<ExposedResources>.Set(this))
		{
		}
	}

	private void OnDisable()
	{
		if (!Singleton<ExposedResources>.Unset(this))
		{
		}
	}

	private void Start()
	{
		Initialize(true);
	}

	public void OfflineInitialize()
	{
		if (!Application.isPlaying)
		{
			Initialize(false);
		}
	}

	public void OfflineDeinitialize()
	{
		if (!Application.isPlaying)
		{
			Deinitialize(false);
		}
	}

	public T FindResource<T>(string name) where T : class
	{
		if (_resources == null)
		{
			return (T)null;
		}
		string[] namePath = name.Split('.');
		if (namePath == null || namePath.Length == 0)
		{
			return (T)null;
		}
		Resource resource = null;
		ExposedResources exposedResources = this;
		int num = namePath.Length;
		for (int i = 0; i < num; i++)
		{
			Resource resource2 = ((exposedResources._dict != null) ? exposedResources._dict[namePath[i]] : exposedResources._resources.Find((Resource r) => r._name == namePath[i]));
			if (resource2 == null)
			{
				break;
			}
			if (i + 1 < num)
			{
				exposedResources = resource2._targetResources;
				if (exposedResources == null)
				{
					break;
				}
			}
			else
			{
				resource = resource2;
			}
		}
		return (resource == null) ? ((T)null) : (resource._target as T);
	}

	public string FindResourceName(string exposedName)
	{
		if (_resources == null)
		{
			return string.Empty;
		}
		string[] namePath = exposedName.Split('.');
		if (namePath == null || namePath.Length == 0)
		{
			return string.Empty;
		}
		Resource resource = null;
		ExposedResources exposedResources = this;
		int num = namePath.Length;
		for (int i = 0; i < num; i++)
		{
			Resource resource2;
			if (exposedResources._dict == null)
			{
				resource2 = exposedResources._resources.Find((Resource r) => r._name == namePath[i]);
			}
			else if (exposedResources._dict.ContainsKey(namePath[i]))
			{
				resource2 = exposedResources._dict[namePath[i]];
			}
			else
			{
				Debug.LogWarning("[ExposedResources] Failed to find resource '" + namePath[i] + "' in '" + exposedResources.name + "'", base.gameObject);
				resource2 = null;
			}
			if (resource2 == null)
			{
				break;
			}
			if (i + 1 < num)
			{
				exposedResources = resource2._targetResources;
				if (exposedResources == null)
				{
					break;
				}
			}
			else
			{
				resource = resource2;
			}
		}
		return (resource == null) ? string.Empty : ((!(resource._target != null)) ? resource._targetName : resource._target.name);
	}

	private void Initialize(bool isRuntime)
	{
		InitializeResources(isRuntime);
		RefreshDict(true);
	}

	private void Deinitialize(bool isRuntime)
	{
		ClearDict();
		DeinitializeResources(isRuntime);
	}

	private void InitializeResources(bool isRuntime)
	{
		if (_resources == null)
		{
			return;
		}
		for (int i = 0; i < _resources.Count; i++)
		{
			if (_resources[i] == null || _resources[i]._target == null)
			{
				continue;
			}
			GameObject gameObject = _resources[i]._target as GameObject;
			if (!(gameObject != null))
			{
				continue;
			}
			_resources[i]._targetResources = gameObject.GetComponent<ExposedResources>();
			if (_resources[i]._targetResources != null && isRuntime)
			{
				gameObject = UnityEngine.Object.Instantiate(_resources[i]._target) as GameObject;
				if (gameObject != null)
				{
					gameObject.transform.parent = base.gameObject.transform;
					_resources[i]._targetResources = gameObject.GetComponent<ExposedResources>();
				}
			}
		}
	}

	private void DeinitializeResources(bool isRuntime)
	{
		if (_resources == null)
		{
			return;
		}
		for (int i = 0; i < _resources.Count; i++)
		{
			if (_resources[i] == null || !(_resources[i]._targetResources != null))
			{
				continue;
			}
			if (isRuntime)
			{
				GameObject gameObject = _resources[i]._target as GameObject;
				GameObject gameObject2 = _resources[i]._targetResources.gameObject;
				if (gameObject2 == gameObject)
				{
					Debug.LogWarning("[ExposedResources] Unexpected result during deinitialize", base.gameObject);
					continue;
				}
				UnityEngine.Object.Destroy(gameObject2);
				_resources[i]._targetResources = null;
			}
			else
			{
				_resources[i]._targetResources = null;
			}
		}
	}

	private void RefreshDict(bool isFullRefresh)
	{
		if (_resources == null)
		{
			_dict = null;
			return;
		}
		if (isFullRefresh || _dict == null)
		{
			_dict = new Dictionary<string, Resource>(_resources.Count);
		}
		for (int i = 0; i < _resources.Count; i++)
		{
			_dict[_resources[i]._name] = _resources[i];
		}
	}

	private void ClearDict()
	{
		if (_dict != null)
		{
			_dict.Clear();
			_dict = null;
		}
	}
}
