using UnityEngine;

namespace AmuzoEngine
{
	public interface IObjectInstantiator
	{
		bool _pCanReuseInstances { get; }

		Object CreateInstance();

		void DestroyInstance(Object inst);
	}
}
