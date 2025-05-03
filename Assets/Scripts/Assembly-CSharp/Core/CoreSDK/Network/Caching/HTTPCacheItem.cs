using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace LEGO.CoreSDK.Network.Caching
{
	// Token: 0x02000020 RID: 32
	internal class HTTPCacheItem
	{
		// Token: 0x0600004F RID: 79 RVA: 0x000032DC File Offset: 0x000014DC
		internal HTTPCacheItem(string body, string cacheControl)
		{
			this.ResponseBody = body;
			this.TimeToLive = Time.time + (float)HTTPCacheItem.GetTimeToLiveFromCacheControl(cacheControl);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0000330C File Offset: 0x0000150C
		public bool IsValid()
		{
			return Time.time <= this.TimeToLive;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003320 File Offset: 0x00001520
		internal static int GetTimeToLiveFromCacheControl(string cacheControl)
		{
			if (string.IsNullOrEmpty(cacheControl))
			{
				return 0;
			}
			Match match = Regex.Match(cacheControl, ".*?max-age=(\\d*)");
			Group group = match.Groups[1];
			if (group != null)
			{
				string value = group.Value;
				int num;
				if (int.TryParse(value, out num))
				{
					return num;
				}
			}
			return 0;
		}

		// Token: 0x04000039 RID: 57
		public string ResponseBody;

		// Token: 0x0400003A RID: 58
		private float TimeToLive;
	}
}
