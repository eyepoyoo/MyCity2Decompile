using System;
using System.Collections.Generic;

namespace AmuzoEngine
{
	public static class Singleton<T> where T : class
	{
		private static T _instance;

		private static int _nextOnCreatedActionKey = 0;

		private static Dictionary<int, Action> _onCreated = new Dictionary<int, Action>();

		public static T _pGet
		{
			get
			{
				return _instance;
			}
		}

		public static bool _pExists
		{
			get
			{
				return _instance != null;
			}
		}

		private static int _pNewOnCreatedActionKey
		{
			get
			{
				return _nextOnCreatedActionKey++;
			}
		}

		public static bool Set(T thisInst)
		{
			if (_instance != null)
			{
				return false;
			}
			_instance = thisInst;
			if (_onCreated != null)
			{
				foreach (KeyValuePair<int, Action> item in _onCreated)
				{
					if (item.Value != null)
					{
						item.Value();
					}
				}
				_onCreated = null;
			}
			return true;
		}

		public static bool Unset(T thisInst)
		{
			if (_instance != thisInst)
			{
				return false;
			}
			_instance = (T)null;
			return true;
		}

		public static int? WhenExists(Action action, int? actionKey = null)
		{
			if (action == null)
			{
				return actionKey;
			}
			if (_instance != null)
			{
				action();
			}
			else
			{
				if (_onCreated == null)
				{
					_onCreated = new Dictionary<int, Action>();
				}
				if (!actionKey.HasValue)
				{
					actionKey = _pNewOnCreatedActionKey;
				}
				if (_onCreated.ContainsKey(actionKey.Value))
				{
					Dictionary<int, Action> onCreated;
					Dictionary<int, Action> dictionary = (onCreated = _onCreated);
					int value;
					int key = (value = actionKey.Value);
					Action a = onCreated[value];
					dictionary[key] = (Action)Delegate.Combine(a, action);
				}
				else
				{
					_onCreated.Add(actionKey.Value, action);
				}
			}
			return actionKey;
		}

		public static void CancelWhenExistsActions(int? actionKey)
		{
			if (actionKey.HasValue && _onCreated != null && _onCreated.ContainsKey(actionKey.Value))
			{
				_onCreated.Remove(actionKey.Value);
			}
		}
	}
}
