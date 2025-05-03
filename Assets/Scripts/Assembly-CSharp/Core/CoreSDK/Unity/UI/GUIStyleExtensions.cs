using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LEGO.CoreSDK.Unity.UI
{
	// Token: 0x02000014 RID: 20
	public static class GUIStyleExtensions
	{
		// Token: 0x0600002F RID: 47 RVA: 0x00002BF0 File Offset: 0x00000DF0
		public static Vector2 CalcLargestSize(this GUIStyle style, IEnumerable<string> content)
		{
			IEnumerable<GUIContent> enumerable = content.Select((string x) => new GUIContent(x));
			Vector2 zero = Vector2.zero;
			foreach (GUIContent guicontent in enumerable)
			{
				Vector2 vector = style.CalcSize(guicontent);
				if (vector.x > zero.x)
				{
					zero.x = vector.x;
				}
				if (vector.y > zero.y)
				{
					zero.y = vector.y;
				}
			}
			return zero;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002CC0 File Offset: 0x00000EC0
		public static int HorizontalPositionForNextElement(this Rect rect, int margin)
		{
			return (int)(rect.x + rect.width + (float)margin);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002CD8 File Offset: 0x00000ED8
		public static int VerticalPositionForNextElement(this Rect rect, int margin)
		{
			return (int)(rect.y + rect.height + (float)margin);
		}
	}
}
