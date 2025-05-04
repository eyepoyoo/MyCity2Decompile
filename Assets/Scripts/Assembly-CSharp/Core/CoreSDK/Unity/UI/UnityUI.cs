using System;
using UnityEngine;

namespace LEGO.CoreSDK.Unity.UI
{
	// Token: 0x0200000C RID: 12
	internal static class UnityUI
	{
		// Token: 0x06000017 RID: 23 RVA: 0x00002610 File Offset: 0x00000810
		public static Rect Label(int x, int y, LabelStyle style, string text, int endMargin = 16)
		{
			Vector2 vector = style.Style.CalcSize(new GUIContent(text));
			if ((float)x + vector.x > (float)Screen.width)
			{
				vector.y = style.Style.CalcHeight(new GUIContent(text), (float)(Screen.width - endMargin));
				vector.x = (float)(Screen.width - x - endMargin);
			}
			Rect rect = new Rect((float)x, (float)y, vector.x, vector.y);
			GUI.Label(rect, text, style);
			return rect;
		}
	}
}
