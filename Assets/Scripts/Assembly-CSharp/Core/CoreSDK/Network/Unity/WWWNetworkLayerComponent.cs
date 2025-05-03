using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using LEGO.CoreSDK.Extensions.Unity;
using LEGO.CoreSDK.Network.Caching;
using UnityEngine;

namespace LEGO.CoreSDK.Network.Unity
{
	// Token: 0x0200001A RID: 26
	internal class WWWNetworkLayerComponent : MonoBehaviour, INetworkRequest
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00002E88 File Offset: 0x00001088
		public void Cancel()
		{
			base.gameObject.SafeDestroy();
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002E98 File Offset: 0x00001098
		internal void Download(Uri URL, HttpMethod method, Dictionary<string, string> parameters, Dictionary<string, string> headers, bool useCache, IHTTPCache cache, Action<HttpStatusCode, string, Dictionary<string, string>> success, Action<HttpStatusCode, string> failure, Action @finally)
		{
			base.StartCoroutine(this.DoDownload(URL, method, parameters, headers, useCache, cache, success, failure, @finally));
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002EC4 File Offset: 0x000010C4
		private IEnumerator DoDownload(Uri URL, HttpMethod method, Dictionary<string, string> parameters, Dictionary<string, string> headers, bool useCache, IHTTPCache cache, Action<HttpStatusCode, string, Dictionary<string, string>> success, Action<HttpStatusCode, string> failure, Action @finally)
		{
			if (parameters != null)
			{
				throw new NotImplementedException("Parameter support has not yet been implemented.");
			}
			using (WWW www = new WWW(URL.AbsoluteUri, null, headers))
			{
				yield return www;
				string status;
				HttpStatusCode statusCode;
				if (www.responseHeaders.TryGetValue("STATUS", out status))
				{
					statusCode = this.convertHttpStatusStringToStatusCode(status);
				}
				else
				{
					statusCode = HttpStatusCode.ExpectationFailed;
				}
				if (statusCode == HttpStatusCode.Found)
				{
					string urlString;
					if (!www.responseHeaders.TryGetValue("LOCATION", out urlString))
					{
						if (failure != null)
						{
							failure(HttpStatusCode.Found, "Got redirect response but no forwarding adress.");
						}
						if (@finally != null)
						{
							@finally();
						}
						yield break;
					}
					Uri uri;
					try
					{
						uri = new Uri(urlString);
					}
					catch (UriFormatException ex)
					{
						UriFormatException exception = ex;
						if (failure != null)
						{
							failure(HttpStatusCode.Found, exception.Message);
						}
						if (@finally != null)
						{
							@finally();
						}
						yield break;
					}
					yield return this.DoDownload(uri, method, parameters, headers, useCache, cache, success, failure, @finally);
					yield break;
				}
				else
				{
					if (!string.IsNullOrEmpty(www.error))
					{
						if (failure != null)
						{
							failure(statusCode, www.error);
						}
						if (@finally != null)
						{
							@finally();
						}
						yield break;
					}
					if (statusCode == HttpStatusCode.NotModified)
					{
						success(statusCode, null, www.responseHeaders);
					}
					else if (statusCode == HttpStatusCode.OK)
					{
						if (useCache)
						{
							string cacheControl = null;
							if (www.responseHeaders.ContainsKey("CACHE-CONTROL"))
							{
								cacheControl = www.responseHeaders["CACHE-CONTROL"];
							}
							cache.CacheResponse(URL, www.text, cacheControl);
						}
						success(statusCode, www.text, www.responseHeaders);
					}
					else if (failure != null)
					{
						failure(statusCode, www.error);
						if (@finally != null)
						{
							@finally();
						}
						yield break;
					}
					if (@finally != null)
					{
						@finally();
					}
					base.gameObject.SafeDestroy();
				}
			}
			yield break;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002F6C File Offset: 0x0000116C
		private HttpStatusCode convertHttpStatusStringToStatusCode(string status)
		{
			Match match = Regex.Match(status, ".*? (\\d*) .*");
			Group group = match.Groups[1];
			if (group != null)
			{
				string value = group.Value;
				int num;
				if (int.TryParse(value, out num))
				{
					return (HttpStatusCode)num;
				}
			}
			return HttpStatusCode.NotFound;
		}
	}
}
