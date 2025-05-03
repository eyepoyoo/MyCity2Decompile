using System;
using System.Collections.Generic;
using System.Net;
using LEGO.CoreSDK.Extensions;
using LEGO.CoreSDK.Network.Caching;
using UnityEngine;

namespace LEGO.CoreSDK.Network.Unity
{
	// Token: 0x0200001B RID: 27
	internal class WWWNetwork : INetwork
	{
		// Token: 0x06000044 RID: 68 RVA: 0x00002FB4 File Offset: 0x000011B4
		public WWWNetwork(IHTTPCache cache)
		{
			this._cache = cache;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002FC4 File Offset: 0x000011C4
		public INetworkRequest GetJSONRequest(Uri endPoint, Dictionary<string, string> parameters, Dictionary<string, string> headers, Action<HttpStatusCode, string, Dictionary<string, string>> success, Action<HttpStatusCode, string> failure, Action @finally, bool useCache = true)
		{
			Asserts.ParameterNotNull(endPoint, "endPoint");
			Asserts.ParameterNotNull(success, "success");
			headers.Add(HTTPHeaderRequestField.Accept.Identifier(), HTTPHeaderAcceptedContent.JSON.Identifier());
			headers.Add(HTTPHeaderRequestField.ContentType.Identifier(), HTTPHeaderAcceptedContent.JSON.Identifier());
			endPoint = endPoint.AddOrUpdateValueForQueryParameter("format", "json");
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
				WWWNetworkLayerComponent wwwnetworkLayerComponent = gameObject.AddComponent<WWWNetworkLayerComponent>();
				wwwnetworkLayerComponent.Download(endPoint, HttpMethod.Get, parameters, headers, useCache, this._cache, success, failure, @finally);
				return wwwnetworkLayerComponent;
			}
			success(HttpStatusCode.OK, cacheResponse, null);
			if (@finally != null)
			{
				@finally();
			}
			return null;
		}

		// Token: 0x04000037 RID: 55
		private IHTTPCache _cache;
	}
}
