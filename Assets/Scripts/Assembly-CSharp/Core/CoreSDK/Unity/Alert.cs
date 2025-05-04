using System;
using LEGO.CoreSDK.Extensions.Unity;
using UnityEngine;

namespace LEGO.CoreSDK.Unity
{
	// Token: 0x0200007E RID: 126
	internal class Alert : IAlert
	{
		// Token: 0x060001F6 RID: 502 RVA: 0x000087D4 File Offset: 0x000069D4
		public void Show(string title, string message)
		{
			string text = "Alert/Alert";
			global::UnityEngine.Object @object = Resources.Load(text);
			if (@object == null)
			{
				throw new UnityPrefabNotFoundException("Failed to find " + text + ".prefab");
			}
			this.gameObject = global::UnityEngine.Object.Instantiate(@object, Vector3.zero, Quaternion.identity) as GameObject;
			AlertBehaviour component = this.gameObject.GetComponent<AlertBehaviour>();
			component.Headline = title;
			component.Message = message;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00008848 File Offset: 0x00006A48
		public void Close()
		{
			this.gameObject.SafeDestroy();
		}

		// Token: 0x0400010D RID: 269
		private GameObject gameObject;
	}
}
