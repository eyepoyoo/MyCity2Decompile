using System;
using System.Collections.Generic;
using GameDefines;
using UnityEngine;

namespace AmuzoEngine
{
	public class ObjectSpawnFacade : InitialisationObject, ObjectSpawner.IOwner
	{
		[Serializable]
		public class ObjectTypeInfo
		{
			public ESpawnObjectType _type;

			public ObjectSpawner _spawner;
		}

		private const string LOG_TAG = "[ObjectSpawnFacade] ";

		public ObjectTypeInfo[] _objectTypes;

		private Dictionary<int, ObjectTypeInfo> _objectTypeDict;

		private bool _isInitialized;

		void ObjectSpawner.IOwner.OnSpawned(GameObject obj)
		{
		}

		void ObjectSpawner.IOwner.OnDespawn(GameObject obj)
		{
		}

		bool ObjectSpawner.IOwner.IsWantDespawn(GameObject obj)
		{
			return false;
		}

		protected override void Awake()
		{
			base.Awake();
			Facades<ObjectSpawnFacade>.Register(this);
		}

		protected override void OnDestroy()
		{
			Facades<ObjectSpawnFacade>.Register(null);
			base.OnDestroy();
		}

		public override void startInitialising()
		{
			_currentState = InitialisationState.INITIALISING;
			Initialize();
			_currentState = InitialisationState.FINISHED;
		}

		public GameObject Spawn(ESpawnObjectType objectType, Vector3 spawnPos)
		{
			if (!_isInitialized)
			{
				Debug.LogWarning("[ObjectSpawnFacade] Not initialized", base.gameObject);
				return null;
			}
			ObjectTypeInfo objectTypeInfo = GetObjectTypeInfo(objectType);
			if (objectTypeInfo == null)
			{
				Debug.LogWarning("[ObjectSpawnFacade] No info for object type: " + objectType, base.gameObject);
				return null;
			}
			if (objectTypeInfo._spawner == null)
			{
				Debug.LogWarning("[ObjectSpawnFacade] No spawner for object type: " + objectType, base.gameObject);
				return null;
			}
			return objectTypeInfo._spawner.SpawnObject(spawnPos);
		}

		public void Spawn(ESpawnObjectType objectType, int objectCount, Vector3 spawnPos, Action<GameObject> onSpawned = null)
		{
			if (!_isInitialized)
			{
				Debug.LogWarning("[ObjectSpawnFacade] Not initialized", base.gameObject);
				return;
			}
			ObjectTypeInfo objectTypeInfo = GetObjectTypeInfo(objectType);
			if (objectTypeInfo == null)
			{
				Debug.LogWarning("[ObjectSpawnFacade] No info for object type: " + objectType, base.gameObject);
			}
			else if (objectTypeInfo._spawner == null)
			{
				Debug.LogWarning("[ObjectSpawnFacade] No spawner for object type: " + objectType, base.gameObject);
			}
			else
			{
				objectTypeInfo._spawner.SpawnObjects(objectCount, spawnPos, onSpawned);
			}
		}

		public void Spawn(ESpawnObjectType objectType, int objectCount, ObjectSpawner.ISpawnPoints spawnPoints, Action<GameObject> onSpawned = null)
		{
			if (!_isInitialized)
			{
				Debug.LogWarning("[ObjectSpawnFacade] Not initialized", base.gameObject);
				return;
			}
			ObjectTypeInfo objectTypeInfo = GetObjectTypeInfo(objectType);
			if (objectTypeInfo == null)
			{
				Debug.LogWarning("[ObjectSpawnFacade] No info for object type: " + objectType, base.gameObject);
			}
			else if (objectTypeInfo._spawner == null)
			{
				Debug.LogWarning("[ObjectSpawnFacade] No spawner for object type: " + objectType, base.gameObject);
			}
			else
			{
				objectTypeInfo._spawner.SpawnObjects(objectCount, spawnPoints, onSpawned);
			}
		}

		public void DespawnAll(ESpawnObjectType objectType)
		{
			if (!_isInitialized)
			{
				Debug.LogWarning("[ObjectSpawnFacade] Not initialized", base.gameObject);
				return;
			}
			ObjectTypeInfo objectTypeInfo = GetObjectTypeInfo(objectType);
			if (objectTypeInfo == null)
			{
				Debug.LogWarning("[ObjectSpawnFacade] No info for object type: " + objectType, base.gameObject);
			}
			else if (objectTypeInfo._spawner == null)
			{
				Debug.LogWarning("[ObjectSpawnFacade] No spawner for object type: " + objectType, base.gameObject);
			}
			else
			{
				objectTypeInfo._spawner.DespawnAll();
			}
		}

		public void DespawnAll()
		{
			if (_objectTypes == null)
			{
				return;
			}
			for (int i = 0; i < _objectTypes.Length; i++)
			{
				if (_objectTypes[i] != null && !(_objectTypes[i]._spawner == null))
				{
					_objectTypes[i]._spawner.DespawnAll();
				}
			}
		}

		private void Initialize()
		{
			InitializeSpawners();
			_isInitialized = true;
		}

		private void InitializeSpawners()
		{
			_objectTypeDict = new Dictionary<int, ObjectTypeInfo>();
			if (_objectTypes == null)
			{
				return;
			}
			for (int i = 0; i < _objectTypes.Length; i++)
			{
				int type = (int)_objectTypes[i]._type;
				if (_objectTypeDict.ContainsKey(type))
				{
					Debug.LogError("[ObjectSpawnFacade] Object type " + i + " is duplicate", base.gameObject);
				}
				else if (_objectTypes[i]._spawner == null)
				{
					Debug.LogError("[ObjectSpawnFacade] Object type " + i + " has no spawner", base.gameObject);
				}
				else
				{
					_objectTypeDict.Add(type, _objectTypes[i]);
				}
			}
		}

		private ObjectTypeInfo GetObjectTypeInfo(ESpawnObjectType objectType)
		{
			ObjectTypeInfo result = null;
			if (_objectTypeDict != null)
			{
				int key = (int)objectType;
				if (_objectTypeDict.ContainsKey(key))
				{
					result = _objectTypeDict[key];
				}
			}
			else if (_objectTypes != null)
			{
				result = Array.Find(_objectTypes, (ObjectTypeInfo s) => s._type == objectType);
			}
			return result;
		}
	}
}
