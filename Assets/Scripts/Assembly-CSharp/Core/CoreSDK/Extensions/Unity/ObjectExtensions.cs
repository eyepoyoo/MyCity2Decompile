using System;
using UnityEngine;

namespace LEGO.CoreSDK.Extensions.Unity
{
	// Token: 0x02000015 RID: 21
	public static class ObjectExtensions
	{
		// Token: 0x06000033 RID: 51 RVA: 0x00002CF8 File Offset: 0x00000EF8
		public static void SafeDestroy(this global::UnityEngine.Object obj)
		{
			global::UnityEngine.Object.Destroy(obj);
		}
	}
}
