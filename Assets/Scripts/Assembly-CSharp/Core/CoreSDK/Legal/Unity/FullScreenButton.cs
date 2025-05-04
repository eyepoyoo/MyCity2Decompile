using System;
using LEGO.CoreSDK.Extensions.Unity;
using LEGO.CoreSDK.Unity.UI;
using UnityEngine;

namespace LEGO.CoreSDK.Legal.Unity
{
	// Token: 0x02000048 RID: 72
	public class FullScreenButton : UnityGUIComponent
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x000052C0 File Offset: 0x000034C0
		public void Setup(Action didSelect)
		{
			this.DidSelect = didSelect;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000052CC File Offset: 0x000034CC
		protected override void OnGUI()
		{
			GUI.depth = -2;
			if (GUI.Button(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), string.Empty, GUIStyle.none))
			{
				this.DidSelect();
				this.SafeDestroy();
			}
		}

		// Token: 0x04000096 RID: 150
		private Action DidSelect;
	}
}
