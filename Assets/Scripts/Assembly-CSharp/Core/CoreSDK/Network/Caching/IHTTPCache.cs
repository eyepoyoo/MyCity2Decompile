using System;

namespace LEGO.CoreSDK.Network.Caching
{
	// Token: 0x0200001F RID: 31
	internal interface IHTTPCache
	{
		// Token: 0x0600004D RID: 77
		string GetCacheResponse(Uri endPoint);

		// Token: 0x0600004E RID: 78
		void CacheResponse(Uri endPoint, string body, string cacheControl);
	}
}
