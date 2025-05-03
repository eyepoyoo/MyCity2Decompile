using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using LEGO.CoreSDK.Extensions.Unity;
using LEGO.CoreSDK.Network.Caching;
using UnityEngine;
using UnityEngine.Networking;

namespace LEGO.CoreSDK.Network.Unity
{
	// Token: 0x0200001C RID: 28
	[ExecuteInEditMode]
	internal class NetworkLayerComponent : MonoBehaviour, INetworkRequest
	{
		// Token: 0x06000047 RID: 71 RVA: 0x000030BC File Offset: 0x000012BC
		public void Cancel()
		{
			base.gameObject.SafeDestroy();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000030CC File Offset: 0x000012CC
		internal void Download(Uri URL, HttpMethod method, Dictionary<string, string> parameters, Dictionary<string, string> headers, bool useCache, IHTTPCache cache, Action<HttpStatusCode, string, Dictionary<string, string>> success, Action<HttpStatusCode, string> failure, Action @finally)
		{
			base.StartCoroutine(this.DoDownload(URL, method, parameters, headers, useCache, cache, success, failure, @finally));
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000030F8 File Offset: 0x000012F8
		private IEnumerator DoDownload(Uri URL, HttpMethod method, Dictionary<string, string> parameters, Dictionary<string, string> headers, bool useCache, IHTTPCache cache, Action<HttpStatusCode, string, Dictionary<string, string>> success, Action<HttpStatusCode, string> failure, Action @finally)
		{
			using (UnityWebRequest request = new UnityWebRequest(URL.AbsoluteUri, method.ToUnity()))
			{
				if (parameters != null)
				{
					foreach (KeyValuePair<string, string> kvp in parameters)
					{
						request.SetRequestHeader(kvp.Key, kvp.Value);
					}
				}
				if (headers != null)
				{
					foreach (KeyValuePair<string, string> kvp2 in headers)
					{
						request.SetRequestHeader(kvp2.Key, kvp2.Value);
					}
				}
				request.downloadHandler = new DownloadHandlerBuffer();
				yield return request.Send();
				HttpStatusCode statusCode = (HttpStatusCode)request.responseCode;
				if (request.isNetworkError)
				{
					if (failure != null)
					{
						failure(statusCode, request.error);
					}
				}
				else if (!statusCode.IsSuccess())
				{
					if (failure != null)
					{
						failure(statusCode, request.error);
					}
				}
				else
				{
					if (useCache && statusCode == HttpStatusCode.OK)
					{
						string cacheControl = request.GetResponseHeader("CACHE-CONTROL");
						if (!string.IsNullOrEmpty(cacheControl))
						{
							cache.CacheResponse(URL, request.downloadHandler.text, cacheControl);
						}
					}
					success(statusCode, request.downloadHandler.text, request.GetResponseHeaders());
				}
				if (@finally != null)
				{
					@finally();
				}
				base.gameObject.SafeDestroy();
			}
			yield break;
		}
	}
}
