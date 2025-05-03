using System;
using UnityEngine;

namespace AmuzoEngine
{
	public static class ManagedInitUtil
	{
		public static IManagedInitTarget[] FindTargets(GameObject gobj, bool findInChildren, bool includeInactive)
		{
			Component[] array = ((!findInChildren) ? gobj.GetComponents(typeof(IManagedInitTarget)) : gobj.GetComponentsInChildren(typeof(IManagedInitTarget), includeInactive));
			if (array == null)
			{
				return null;
			}
			int num = array.Length;
			IManagedInitTarget[] array2 = new IManagedInitTarget[num];
			for (int i = 0; i < num; i++)
			{
				array2[i] = array[i] as IManagedInitTarget;
			}
			return array2;
		}

		public static void SafeInititialize(IManagedInitTarget target, EManagedInitType initType)
		{
			if (target != null && (initType != EManagedInitType.FULL || !target._pIsInitialized))
			{
				target.Initialize(initType);
				if (initType == EManagedInitType.FULL && !target._pIsInitialized)
				{
					Debug.LogError(string.Concat("[ManagedInit.SafeInititialize] Target '", target, "' was not initialized!"));
				}
			}
		}

		public static void FindAndInitialize(EManagedInitType initType, GameObject gobj, bool findInChildren, bool includeInactive, Predicate<IManagedInitTarget> filter = null)
		{
			IManagedInitTarget[] array = FindTargets(gobj, findInChildren, includeInactive);
			if (array == null)
			{
				return;
			}
			if (filter != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (filter(array[i]))
					{
						SafeInititialize(array[i], initType);
					}
				}
			}
			else
			{
				for (int j = 0; j < array.Length; j++)
				{
					SafeInititialize(array[j], initType);
				}
			}
		}
	}
}
