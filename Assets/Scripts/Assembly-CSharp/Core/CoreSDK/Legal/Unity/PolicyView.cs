using System;
using System.Collections.Generic;
using LEGO.CoreSDK.Extensions.Unity;
using LEGO.CoreSDK.Unity;
using UnityEngine;

namespace LEGO.CoreSDK.Legal.Unity
{
	// Token: 0x0200003F RID: 63
	public class PolicyView : IPolicyView
	{
		// Token: 0x060000AC RID: 172 RVA: 0x00003E24 File Offset: 0x00002024
		public void Show(Func<Locale, LegalPolicies> getPolicies, Locale currentLocale, IEnumerable<Locale> supportedLocales, PolicyType? policyType, Action completionHandler)
		{
			global::UnityEngine.Object @object = Resources.Load("Legal/Prefabs/PolicyView");
			if (@object == null)
			{
				throw new UnityPrefabNotFoundException("Failed to find the default PolicyView prefab");
			}
			this.viewGameObject = global::UnityEngine.Object.Instantiate(@object) as GameObject;
			this.viewGameObject.GetComponent<PolicyBehaviour>().Setup(getPolicies, currentLocale, policyType, completionHandler);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00003E7C File Offset: 0x0000207C
		public void Close()
		{
			this.viewGameObject.SafeDestroy();
		}

		// Token: 0x04000065 RID: 101
		private GameObject viewGameObject;
	}
}
