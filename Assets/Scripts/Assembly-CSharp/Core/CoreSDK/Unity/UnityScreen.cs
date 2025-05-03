using System;
using UnityEngine;

namespace LEGO.CoreSDK.Unity
{
	// Token: 0x0200000D RID: 13
	public static class UnityScreen
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000026A0 File Offset: 0x000008A0
		public static int ScaleFactor
		{
			get
			{
				if (Screen.dpi > 401f)
				{
					return 3;
				}
				if (Screen.dpi > 163f)
				{
					return 2;
				}
				return 1;
			}
		}
	}
}
