using System;
using System.Collections.Generic;

namespace LEGO.CoreSDK.Network.Caching
{
	// Token: 0x02000021 RID: 33
	internal class HTTPCache : IHTTPCache
	{
		// Token: 0x06000053 RID: 83 RVA: 0x00003384 File Offset: 0x00001584
		public string GetCacheResponse(Uri endPoint)
		{
			if (endPoint == null)
			{
				return null;
			}
			string text;
			lock (this)
			{
				if (this.cache.ContainsKey(endPoint.AbsoluteUri))
				{
					HTTPCacheItem httpcacheItem = this.cache[endPoint.AbsoluteUri];
					if (httpcacheItem.IsValid())
					{
						return httpcacheItem.ResponseBody;
					}
					this.cache.Remove(endPoint.AbsoluteUri);
				}
				text = null;
			}
			return text;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003428 File Offset: 0x00001628
		public void CacheResponse(Uri endPoint, string body, string cacheControl)
		{
			lock (this)
			{
				HTTPCacheItem httpcacheItem = new HTTPCacheItem(body, cacheControl);
				if (this.cache.ContainsKey(endPoint.AbsoluteUri))
				{
					this.cache[endPoint.AbsoluteUri] = httpcacheItem;
				}
				else
				{
					this.cache.Add(endPoint.AbsoluteUri, httpcacheItem);
				}
			}
		}

		// Token: 0x0400003B RID: 59
		private Dictionary<string, HTTPCacheItem> cache = new Dictionary<string, HTTPCacheItem>();
	}
}
