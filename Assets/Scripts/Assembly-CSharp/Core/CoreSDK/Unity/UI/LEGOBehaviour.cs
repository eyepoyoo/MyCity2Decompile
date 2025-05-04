using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LEGO.CoreSDK.Unity.UI
{
	// Token: 0x02000013 RID: 19
	public abstract class LEGOBehaviour : MonoBehaviour
	{
		// Token: 0x0600002D RID: 45 RVA: 0x00002B7C File Offset: 0x00000D7C
		protected virtual void Start()
		{
			if (EventSystem.current == null)
			{
				GameObject gameObject = Resources.Load("EventSystem") as GameObject;
				global::UnityEngine.Object.Instantiate<GameObject>(gameObject);
			}
			base.GetComponentsInChildren<CanvasScaler>().ToList<CanvasScaler>().ForEach(delegate(CanvasScaler x)
			{
				x.scaleFactor = (float)UnityScreen.ScaleFactor;
			});
		}
	}
}
