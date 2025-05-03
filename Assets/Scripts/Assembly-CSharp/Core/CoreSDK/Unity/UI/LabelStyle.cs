using System;
using UnityEngine;

namespace LEGO.CoreSDK.Unity.UI
{
	// Token: 0x02000010 RID: 16
	[Serializable]
	public class LabelStyle
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000027F8 File Offset: 0x000009F8
		public int ActualSize
		{
			get
			{
				return this.Size * this.Owner.ScaleFactor;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001F RID: 31 RVA: 0x0000280C File Offset: 0x00000A0C
		public GUIStyle Style
		{
			get
			{
				if (this._style == null)
				{
					this._style = new GUIStyle();
					this._style.font = this.Font;
					this._style.fontSize = this.ActualSize;
					this._style.wordWrap = true;
					this._style.richText = true;
				}
				return this._style;
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002870 File Offset: 0x00000A70
		public static implicit operator GUIStyle(LabelStyle style)
		{
			return style.Style;
		}

		// Token: 0x04000024 RID: 36
		public Font Font;

		// Token: 0x04000025 RID: 37
		public int Size;

		// Token: 0x04000026 RID: 38
		public UnityGUIComponent Owner;

		// Token: 0x04000027 RID: 39
		private GUIStyle _style;
	}
}
