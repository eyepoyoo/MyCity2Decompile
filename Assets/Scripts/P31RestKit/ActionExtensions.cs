using System;
using UnityEngine;

namespace Prime31
{
	// Token: 0x02000007 RID: 7
	public static class ActionExtensions
	{
		// Token: 0x0600001C RID: 28 RVA: 0x00002CD0 File Offset: 0x00000ED0
		private static void invoke(Delegate listener, object[] args)
		{
			if (!listener.Method.IsStatic && (listener.Target == null || listener.Target.Equals(null)))
			{
				Debug.LogError("an event listener is still subscribed to an event with the method " + listener.Method.Name + " even though it is null. Be sure to balance your event subscriptions.");
			}
			else
			{
				listener.Method.Invoke(listener.Target, args);
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002D40 File Offset: 0x00000F40
		public static void fire(this Action handler)
		{
			if (handler == null)
			{
				return;
			}
			object[] array = new object[0];
			foreach (Delegate @delegate in handler.GetInvocationList())
			{
				ActionExtensions.invoke(@delegate, array);
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002D84 File Offset: 0x00000F84
		public static void fire<T>(this Action<T> handler, T param)
		{
			if (handler == null)
			{
				return;
			}
			object[] array = new object[] { param };
			foreach (Delegate @delegate in handler.GetInvocationList())
			{
				ActionExtensions.invoke(@delegate, array);
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002DD0 File Offset: 0x00000FD0
		public static void fire<T, U>(this Action<T, U> handler, T param1, U param2)
		{
			if (handler == null)
			{
				return;
			}
			object[] array = new object[] { param1, param2 };
			foreach (Delegate @delegate in handler.GetInvocationList())
			{
				ActionExtensions.invoke(@delegate, array);
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002E24 File Offset: 0x00001024
		public static void fire<T, U, V>(this Action<T, U, V> handler, T param1, U param2, V param3)
		{
			if (handler == null)
			{
				return;
			}
			object[] array = new object[] { param1, param2, param3 };
			foreach (Delegate @delegate in handler.GetInvocationList())
			{
				ActionExtensions.invoke(@delegate, array);
			}
		}
	}
}
