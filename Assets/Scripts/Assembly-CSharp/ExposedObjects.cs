using System;
using System.Collections.Generic;
using AmuzoEngine;
using UnityEngine;

public class ExposedObjects : MonoBehaviour
{
	public interface IObjectList
	{
		string _pName { get; }

		int _pCount { get; }

		GameObject this[int index] { get; }
	}

	[Serializable]
	private class SingleObject : IObjectList
	{
		public string _name;

		public GameObject _target;

		string IObjectList._pName
		{
			get
			{
				return _pName;
			}
		}

		int IObjectList._pCount
		{
			get
			{
				return 1;
			}
		}

		GameObject IObjectList.this[int index]
		{
			get
			{
				return (index != 0) ? null : _target;
			}
		}

		public string _pName
		{
			get
			{
				return _name.ToUpper();
			}
		}
	}

	[Serializable]
	private class ObjectGroup : IObjectList
	{
		public string _name;

		public List<GameObject> _targets;

		string IObjectList._pName
		{
			get
			{
				return _pName;
			}
		}

		int IObjectList._pCount
		{
			get
			{
				return _targets.Count;
			}
		}

		GameObject IObjectList.this[int index]
		{
			get
			{
				return _targets[index];
			}
		}

		public string _pName
		{
			get
			{
				return _name.ToUpper();
			}
		}
	}

	private const string LOG_TAG = "[ExposedObjects] ";

	[SerializeField]
	private List<SingleObject> _singleObjects;

	[SerializeField]
	private List<ObjectGroup> _objectGroups;

	private Dictionary<string, IObjectList> _dict;

	private void OnEnable()
	{
		Singleton<ExposedObjects>.Set(this);
	}

	private void OnDisable()
	{
		Singleton<ExposedObjects>.Unset(this);
	}

	private void Start()
	{
		RefreshDict(true);
	}

	public IObjectList FindObjects(string name)
	{
		if (name == null)
		{
			Debug.LogError("[ExposedObjects] null name");
			return null;
		}
		name = name.ToUpper();
		IObjectList objectList = null;
		if (_dict != null)
		{
			if (_dict.ContainsKey(name))
			{
				objectList = _dict[name];
			}
		}
		else
		{
			objectList = FindInList(_singleObjects, name);
			if (objectList == null)
			{
				objectList = FindInList(_objectGroups, name);
			}
		}
		return objectList;
	}

	private void RefreshDict(bool isFullRefresh)
	{
		if (isFullRefresh || _dict == null)
		{
			_dict = new Dictionary<string, IObjectList>();
		}
		for (int i = 0; i < _singleObjects.Count; i++)
		{
			_dict[_singleObjects[i]._pName] = _singleObjects[i];
		}
		for (int j = 0; j < _objectGroups.Count; j++)
		{
			_dict[_objectGroups[j]._pName] = _objectGroups[j];
		}
	}

	private T FindInList<T>(List<T> list, string name) where T : class, IObjectList
	{
		if (list == null)
		{
			return (T)null;
		}
		return list.Find((T e) => e._pName == name);
	}
}
