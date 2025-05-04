using UnityEngine;

namespace AmuzoEngine
{
	public static class ObjectInstantiator
	{
		public static IObjectInstantiator GetObjectInstantiator(this GameObject obj)
		{
			if (obj == null)
			{
				return null;
			}
			return obj.GetComponent(typeof(IObjectInstantiator)) as IObjectInstantiator;
		}
	}
}
