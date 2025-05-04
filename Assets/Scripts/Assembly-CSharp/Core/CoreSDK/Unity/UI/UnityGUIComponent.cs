using System;
using UnityEngine;

namespace LEGO.CoreSDK.Unity.UI
{
	// Token: 0x02000011 RID: 17
	public class UnityGUIComponent : MonoBehaviour
	{
		// Token: 0x06000022 RID: 34 RVA: 0x00002890 File Offset: 0x00000A90
		public virtual void Start()
		{
			this.ScaleFactor = UnityScreen.ScaleFactor;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000028A0 File Offset: 0x00000AA0
		protected virtual void OnGUI()
		{
			int num = ((!this.OverrideScaleFactor) ? UnityScreen.ScaleFactor : this.OverridenScaleFactor);
			if (this.ScaleFactor != num)
			{
				this.ScaleFactor = num;
				this.ScaleFactorDidChange(this.ScaleFactor);
			}
			if (Screen.width != this.oldWidth || Screen.height != this.oldHeight)
			{
				this.oldWidth = Screen.width;
				this.oldHeight = Screen.height;
				this.OrientationDidChange();
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002924 File Offset: 0x00000B24
		protected virtual void ScaleFactorDidChange(int newScaleFactor)
		{
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002928 File Offset: 0x00000B28
		protected virtual void OrientationDidChange()
		{
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000026 RID: 38 RVA: 0x0000292C File Offset: 0x00000B2C
		public GUIStyle ButtonStyle
		{
			get
			{
				if (this._buttonStyle == null)
				{
					this._buttonStyle = new GUIStyle();
					this._buttonStyle.border = this.ButtonBackground.border;
					this._buttonStyle.font = this.ButtonLabelStyle.Font;
					this._buttonStyle.fontSize = this.ButtonLabelStyle.ActualSize;
					this._buttonStyle.normal.background = this.ButtonBackground;
					this._buttonStyle.active.background = this.ButtonBackgroundSelected;
					this._buttonStyle.alignment = TextAnchor.MiddleCenter;
					this._buttonStyle.normal.textColor = new Color(0.22745098f, 0.38039216f, 0.62352943f);
					this._buttonStyle.padding = new RectOffset(this.DefaultMargin, this.DefaultMargin, 0, 0);
					this._buttonStyle.overflow = this.ButtonBackground.overflow;
					this._buttonStyle.imagePosition = ImagePosition.ImageLeft;
					this._buttonStyle.padding = new RectOffset(this.DefaultMargin, this.DefaultMargin, 8, 8);
				}
				return this._buttonStyle;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002A5C File Offset: 0x00000C5C
		public GUIStyle VerticalScrollbarStyle
		{
			get
			{
				if (this._verticalScrollbarStyle == null)
				{
					this._verticalScrollbarStyle = new GUIStyle();
					Texture2D texture2D = new Texture2D(1, 1);
					texture2D.SetPixel(0, 0, new Color(0.9372549f, 0.9372549f, 0.9372549f));
					texture2D.Apply(false, true);
					this._verticalScrollbarStyle.normal.background = texture2D;
					this._verticalScrollbarStyle.fixedWidth = 4f;
				}
				return this._verticalScrollbarStyle;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002AD4 File Offset: 0x00000CD4
		public GUIStyle VerticalScrollbarThumbStyle
		{
			get
			{
				if (this._verticalScrollbarThumbStyle == null)
				{
					this._verticalScrollbarThumbStyle = new GUIStyle();
					Texture2D texture2D = new Texture2D(1, 1);
					texture2D.SetPixel(0, 0, new Color(0.22745098f, 0.38039216f, 0.62352943f));
					texture2D.Apply(false, true);
					this._verticalScrollbarThumbStyle.normal.background = texture2D;
				}
				return this._verticalScrollbarThumbStyle;
			}
		}

		// Token: 0x04000028 RID: 40
		public bool OverrideScaleFactor;

		// Token: 0x04000029 RID: 41
		public int OverridenScaleFactor = 2;

		// Token: 0x0400002A RID: 42
		internal int ScaleFactor;

		// Token: 0x0400002B RID: 43
		public readonly int DefaultMargin = 16;

		// Token: 0x0400002C RID: 44
		private int oldWidth;

		// Token: 0x0400002D RID: 45
		private int oldHeight;

		// Token: 0x0400002E RID: 46
		public LabelStyle ButtonLabelStyle;

		// Token: 0x0400002F RID: 47
		public Image ButtonBackground;

		// Token: 0x04000030 RID: 48
		public Image ButtonBackgroundSelected;

		// Token: 0x04000031 RID: 49
		private GUIStyle _buttonStyle;

		// Token: 0x04000032 RID: 50
		private GUIStyle _verticalScrollbarStyle;

		// Token: 0x04000033 RID: 51
		private GUIStyle _verticalScrollbarThumbStyle;
	}
}
