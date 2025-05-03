using System;
using System.Collections.Generic;
using System.Net;
using LEGO.CoreSDK.Extensions;
using LEGO.CoreSDK.Network.Caching;
using UnityEngine;

namespace LEGO.CoreSDK.Network.Unity
{
	// Token: 0x0200001D RID: 29
	internal class Network : INetwork
	{
		// Token: 0x0600004A RID: 74 RVA: 0x000031A0 File Offset: 0x000013A0
		public Network(IHTTPCache httpCache)
		{
			this._cache = httpCache;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000031B0 File Offset: 0x000013B0
		public INetworkRequest GetJSONRequest(Uri endPoint, Dictionary<string, string> parameters, Dictionary<string, string> headers, Action<HttpStatusCode, string, Dictionary<string, string>> success, Action<HttpStatusCode, string> failure, Action @finally, bool useCache = true)
		{
			Asserts.ParameterNotNull(endPoint, "endPoint");
			Asserts.ParameterNotNull(success, "success");
			if (!endPoint.IsValidNetworkURL() || !endPoint.IsSecure())
			{
				if (failure != null)
				{
					failure(HttpStatusCode.ExpectationFailed, "The endpoint must be an external HTTPS based URL.");
				}
				if (@finally != null)
				{
					@finally();
				}
				return null;
			}
			string cacheResponse = this._cache.GetCacheResponse(endPoint);
			if (cacheResponse == null)
			{
				GameObject gameObject = new GameObject();
				NetworkLayerComponent networkLayerComponent = gameObject.AddComponent<NetworkLayerComponent>();
				networkLayerComponent.Download(endPoint, HttpMethod.Get, parameters, headers, useCache, this._cache, success, failure, @finally);
				return networkLayerComponent;
			}
			success(HttpStatusCode.OK, cacheResponse, null);
			if (@finally != null)
			{
				@finally();
			}
			return null;
		}

		// Token: 0x04000038 RID: 56
		private IHTTPCache _cache;
	}
}
