using System;
using System.Collections.Generic;
using UnityEngine;

namespace AmuzoEngine
{
	public class ObjectSpawner : MonoBehaviour
	{
		public interface IOwner
		{
			void OnSpawned(GameObject obj);

			void OnDespawn(GameObject obj);

			bool IsWantDespawn(GameObject obj);
		}

		public interface ISpawnObject
		{
			bool _pIsWantDespawn { get; }

			void OnSpawned();

			void OnDespawn();
		}

		public interface ISpawnPoints
		{
			bool ChooseSpawnPoint(out Vector3 position);
		}

		private struct ObjectData
		{
			public GameObject _gameObject;

			public ISpawnObject _spawnObject;

			public ObjectData(GameObject gameObject)
			{
				_gameObject = gameObject;
				_spawnObject = gameObject.GetComponent(typeof(ISpawnObject)) as ISpawnObject;
			}

			public void Clear()
			{
				_gameObject = null;
				_spawnObject = null;
			}
		}

		public class SimpleSpawnPoint : ISpawnPoints
		{
			private Vector3 _pos;

			private SimpleSpawnPoint()
			{
			}

			public SimpleSpawnPoint(Vector3 pos)
			{
				_pos = pos;
			}

			bool ISpawnPoints.ChooseSpawnPoint(out Vector3 position)
			{
				position = _pos;
				return true;
			}
		}

		private const string LOG_TAG = "[ObjectSpawner] ";

		public GameObject _owner;

		private IOwner _ownerInt;

		public GameObject _objectInstantiator;

		private IObjectInstantiator _objectInstantiatorInt;

		public GameObject _objectContainer;

		public GameObject _defaultSpawnPoints;

		private ISpawnPoints _defaultSpawnPointsInt;

		private List<ObjectData> _objects;

		public bool _isFailedSpawnError;

		private bool _isCheckRespawn;

		private bool _pIsValid
		{
			get
			{
				return _objectInstantiatorInt != null && _objects != null;
			}
		}

		protected virtual void Awake()
		{
			Initialize();
		}

		protected virtual void Update()
		{
			UpdateObjects();
		}

		public GameObject SpawnObject(Vector3 spawnPos)
		{
			if (!_pIsValid)
			{
				if (_isFailedSpawnError)
				{
					Debug.LogWarning("[ObjectSpawner] Cannot spawn objects", base.gameObject);
				}
				return null;
			}
			return SpawnObjectInt(spawnPos, null);
		}

		public void SpawnObjects(int count, Vector3 spawnPos, Action<GameObject> onSpawned = null)
		{
			if (!_pIsValid)
			{
				if (_isFailedSpawnError)
				{
					Debug.LogWarning("[ObjectSpawner] Cannot spawn objects", base.gameObject);
				}
			}
			else
			{
				while (count-- > 0)
				{
					SpawnObjectInt(spawnPos, onSpawned);
				}
			}
		}

		public void SpawnObjects(int count, ISpawnPoints spawnPoints = null, Action<GameObject> onSpawned = null)
		{
			if (!_pIsValid)
			{
				if (_isFailedSpawnError)
				{
					Debug.LogWarning("[ObjectSpawner] Cannot spawn objects", base.gameObject);
				}
				return;
			}
			if (spawnPoints == null)
			{
				spawnPoints = _defaultSpawnPointsInt;
			}
			while (count-- > 0)
			{
				Vector3 position = base.transform.position;
				if (spawnPoints != null && !spawnPoints.ChooseSpawnPoint(out position))
				{
					if (_isFailedSpawnError)
					{
						Debug.LogWarning("[ObjectSpawner] Failed to choose spawn point", base.gameObject);
					}
					break;
				}
				SpawnObjectInt(position, onSpawned);
			}
		}

		public void DespawnObject(GameObject obj)
		{
			if (_pIsValid)
			{
				int num = _objects.FindIndex((ObjectData d) => d._gameObject == obj);
				if (num < 0)
				{
					Debug.LogWarning("[ObjectSpawner] Cannot despawn object", obj);
				}
				DespawnObject(num, true);
			}
		}

		public void DespawnAll()
		{
			if (_pIsValid)
			{
				for (int i = 0; i < _objects.Count; i++)
				{
					DespawnObject(i, false);
				}
				_objects.Clear();
			}
		}

		private void Compose()
		{
			if (_owner != null)
			{
				_ownerInt = _owner.GetComponent(typeof(IOwner)) as IOwner;
				if (_ownerInt == null)
				{
					Debug.LogError("[ObjectSpawner] Bad owner", base.gameObject);
				}
			}
			if (_objectInstantiator != null)
			{
				_objectInstantiatorInt = _objectInstantiator.GetObjectInstantiator();
				if (_objectInstantiatorInt == null)
				{
					Debug.LogError("[ObjectSpawner] Bad object instantiator", base.gameObject);
				}
			}
			else
			{
				Debug.LogError("[ObjectSpawner] No object instantiator", base.gameObject);
			}
			if (_defaultSpawnPoints != null)
			{
				_defaultSpawnPointsInt = _defaultSpawnPoints.GetComponent(typeof(ISpawnPoints)) as ISpawnPoints;
				if (_defaultSpawnPointsInt == null)
				{
					Debug.LogError("[ObjectSpawner] Bad spawn points", base.gameObject);
				}
			}
			_objects = new List<ObjectData>();
		}

		private void Initialize()
		{
			Compose();
			if (_objectInstantiatorInt != null)
			{
				_isCheckRespawn = _objectInstantiatorInt._pCanReuseInstances;
			}
		}

		private GameObject SpawnObjectInt(Vector3 spawnPos, Action<GameObject> onSpawned)
		{
			GameObject obj = _objectInstantiatorInt.CreateInstance() as GameObject;
			if (obj == null)
			{
				if (_isFailedSpawnError)
				{
					Debug.LogWarning("[ObjectSpawner] Failed to instantiate object", base.gameObject);
				}
				return null;
			}
			int num = ((!_isCheckRespawn) ? (-1) : _objects.FindIndex((ObjectData d) => d._gameObject == obj));
			ObjectData objectData;
			if (num < 0)
			{
				objectData = new ObjectData(obj);
				_objects.Add(objectData);
			}
			else
			{
				objectData = _objects[num];
				NotifyDespawn(objectData);
			}
			if (_objectContainer != null)
			{
				obj.transform.parent = _objectContainer.transform;
			}
			obj.transform.position = spawnPos;
			if (_ownerInt != null)
			{
				_ownerInt.OnSpawned(obj);
			}
			if (objectData._spawnObject != null)
			{
				objectData._spawnObject.OnSpawned();
			}
			if (onSpawned != null)
			{
				onSpawned(obj);
			}
			return obj;
		}

		private void DespawnObject(int objIndex, bool isRemove)
		{
			ObjectData data = _objects[objIndex];
			GameObject gameObject = data._gameObject;
			NotifyDespawn(data);
			if (isRemove)
			{
				_objects.RemoveAt(objIndex);
			}
			else
			{
				_objects[objIndex].Clear();
			}
			if (gameObject != null)
			{
				_objectInstantiatorInt.DestroyInstance(gameObject);
			}
		}

		private void NotifyDespawn(ObjectData data)
		{
			GameObject gameObject = data._gameObject;
			if (gameObject != null)
			{
				if (data._spawnObject != null)
				{
					data._spawnObject.OnDespawn();
				}
				if (_ownerInt != null)
				{
					_ownerInt.OnDespawn(gameObject);
				}
			}
		}

		private void UpdateObjects()
		{
			for (int i = 0; i < _objects.Count; i++)
			{
				if (_objects[i]._gameObject == null)
				{
					_objects.RemoveAt(i);
					i--;
				}
				else if ((_objects[i]._spawnObject != null && _objects[i]._spawnObject._pIsWantDespawn) || (_ownerInt != null && _ownerInt.IsWantDespawn(_objects[i]._gameObject)))
				{
					DespawnObject(i, false);
					_objects.RemoveAt(i);
					i--;
				}
			}
		}
	}
}
