using UnityEngine;

namespace AmuzoEngine
{
	public class GameObjectState
	{
		private const string LOG_TAG = "[GameObjectState] ";

		private bool _isActive;

		private Transform _parent;

		private Vector3 _position;

		private Quaternion _rotation;

		public Vector3 _pPosition
		{
			get
			{
				return _position;
			}
		}

		public Quaternion _pRotation
		{
			get
			{
				return _rotation;
			}
		}

		public static GameObjectState GetState(GameObject obj)
		{
			if (obj == null)
			{
				return null;
			}
			GameObjectState gameObjectState = new GameObjectState();
			gameObjectState.Store(obj);
			return gameObjectState;
		}

		public static void SetState(GameObject obj, GameObjectState state)
		{
			if (!(obj == null) && state != null)
			{
				state.Restore(obj);
			}
		}

		public void Store(GameObject obj)
		{
			OnStore(obj);
		}

		public void Restore(GameObject obj)
		{
			OnRestore(obj);
		}

		protected virtual void OnStore(GameObject obj)
		{
			if (!(obj == null))
			{
				_isActive = obj.activeSelf;
				_parent = obj.transform.parent;
				_position = obj.transform.position;
				_rotation = obj.transform.rotation;
			}
		}

		protected virtual void OnRestore(GameObject obj)
		{
			if (!(obj == null))
			{
				obj.transform.parent = _parent;
				obj.transform.position = _position;
				obj.transform.rotation = _rotation;
				obj.SetActive(_isActive);
			}
		}
	}
}
