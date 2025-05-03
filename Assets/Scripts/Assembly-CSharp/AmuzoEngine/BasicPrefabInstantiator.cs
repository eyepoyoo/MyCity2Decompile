using UnityEngine;

namespace AmuzoEngine
{
	public class BasicPrefabInstantiator : MonoBehaviour, IObjectInstantiator
	{
		private const string LOG_TAG = "[BasicPrefabInstantiator] ";

		public Object _prefab;

		bool IObjectInstantiator._pCanReuseInstances
		{
			get
			{
				return false;
			}
		}

		Object IObjectInstantiator.CreateInstance()
		{
			if (_prefab == null)
			{
				return null;
			}
			return Object.Instantiate(_prefab);
		}

		void IObjectInstantiator.DestroyInstance(Object inst)
		{
			if (!(inst == null))
			{
				Object.Destroy(inst);
			}
		}
	}
}
