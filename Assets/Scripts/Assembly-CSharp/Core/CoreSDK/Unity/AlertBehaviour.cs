using System;
using UnityEngine;

namespace LEGO.CoreSDK.Unity
{
	// Token: 0x0200007D RID: 125
	public class AlertBehaviour : MonoBehaviour
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x00008480 File Offset: 0x00006680
		private GUIStyle HeadlineStyle
		{
			get
			{
				if (this._HeadlineStyle == null)
				{
					this._HeadlineStyle = new GUIStyle();
					this._HeadlineStyle.normal.textColor = Color.black;
					this._HeadlineStyle.font = this.HeadlineFont;
					this._HeadlineStyle.fontSize = this.HeadlineFontSize * this.scaleFactor;
					this._HeadlineStyle.alignment = TextAnchor.MiddleCenter;
				}
				return this._HeadlineStyle;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x000084F4 File Offset: 0x000066F4
		private GUIStyle MessageStyle
		{
			get
			{
				if (this._MessageStyle == null)
				{
					this._MessageStyle = new GUIStyle();
					this._MessageStyle.normal.textColor = Color.black;
					this._MessageStyle.font = this.MessageFont;
					this._MessageStyle.fontSize = this.MessageFontSize * this.scaleFactor;
					this._MessageStyle.alignment = TextAnchor.MiddleCenter;
					this._MessageStyle.wordWrap = true;
				}
				return this._MessageStyle;
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00008574 File Offset: 0x00006774
		private void Start()
		{
			this.scaleFactor = UnityScreen.ScaleFactor;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00008584 File Offset: 0x00006784
		private void OnGUI()
		{
			GUI.depth = int.MinValue;
			int num = ((!this.OverrideScaleFactor) ? UnityScreen.ScaleFactor : this.OverridenScaleFactor);
			if (this.scaleFactor != num)
			{
				this.scaleFactor = num;
				this._HeadlineStyle = null;
				this._MessageStyle = null;
			}
			float num2 = (float)Screen.width;
			float num3 = (float)Screen.height;
			float num4 = (float)this.Image2x.width;
			float num5 = (float)this.Image2x.height;
			num4 /= 2f;
			num5 /= 2f;
			num4 *= (float)this.scaleFactor;
			num5 *= (float)this.scaleFactor;
			float num6 = this.HeadlineStyle.CalcHeight(new GUIContent(this.Headline), num2);
			float num7 = this.MessageStyle.CalcHeight(new GUIContent(this.Message), num2);
			float num8 = num6 + num7 + (float)(2 * this.Padding) + num5;
			float num9 = num5;
			if (num8 > num3)
			{
				float num10 = num8 - num3;
				num9 -= num10;
			}
			Vector2 vector = this.SizeForImage(num4, num5, num2, num9);
			num8 = num6 + num7 + (float)(2 * this.Padding) + vector.y;
			Rect rect = new Rect(0f, (num3 - num8) * 0.5f, num2, num6);
			Rect rect2 = new Rect((num2 - vector.x) * 0.5f, rect.y + rect.height + (float)this.Padding, vector.x, vector.y);
			Rect rect3 = new Rect(0f, rect2.y + rect2.height + (float)this.Padding, num2, num7);
			GUI.DrawTexture(new Rect(0f, 0f, num2, num3), Texture2D.whiteTexture);
			GUI.DrawTexture(rect2, this.Image2x);
			GUI.Label(rect, this.Headline, this.HeadlineStyle);
			GUI.Label(rect3, this.Message, this.MessageStyle);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00008778 File Offset: 0x00006978
		private Vector2 SizeForImage(float imageWidth, float imageHeight, float maxWidthOfImage, float maxHeightOfImage)
		{
			if (maxWidthOfImage < imageWidth)
			{
				float num = imageWidth - maxWidthOfImage;
				float num2 = num / imageWidth;
				imageWidth = maxWidthOfImage;
				imageHeight *= 1f - num2;
			}
			if (maxHeightOfImage < imageHeight)
			{
				float num3 = imageHeight - maxHeightOfImage;
				float num4 = num3 / imageHeight;
				imageHeight = maxHeightOfImage;
				imageWidth *= 1f - num4;
			}
			return new Vector2(imageWidth, imageHeight);
		}

		// Token: 0x04000100 RID: 256
		public Texture2D Image2x;

		// Token: 0x04000101 RID: 257
		public Font HeadlineFont;

		// Token: 0x04000102 RID: 258
		public Font MessageFont;

		// Token: 0x04000103 RID: 259
		public bool OverrideScaleFactor;

		// Token: 0x04000104 RID: 260
		public int OverridenScaleFactor = 2;

		// Token: 0x04000105 RID: 261
		public int HeadlineFontSize = 35;

		// Token: 0x04000106 RID: 262
		public int MessageFontSize = 20;

		// Token: 0x04000107 RID: 263
		public int Padding = 5;

		// Token: 0x04000108 RID: 264
		public string Headline = "Warning";

		// Token: 0x04000109 RID: 265
		public string Message = "This means that you should most likely just stop to use the app. Delete it, and/or throw it from a bridge for all I care.";

		// Token: 0x0400010A RID: 266
		private int scaleFactor;

		// Token: 0x0400010B RID: 267
		private GUIStyle _HeadlineStyle;

		// Token: 0x0400010C RID: 268
		private GUIStyle _MessageStyle;
	}
}
