using System;
using System.Collections.Generic;
using System.Linq;

namespace LEGO.CoreSDK.Extensions
{
	// Token: 0x02000022 RID: 34
	internal static class URIExtension
	{
		// Token: 0x06000055 RID: 85 RVA: 0x000034AC File Offset: 0x000016AC
		internal static bool IsValidNetworkURL(this Uri uri)
		{
			return !string.IsNullOrEmpty(uri.Host) && !uri.IsUnc;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000034DC File Offset: 0x000016DC
		internal static bool IsSecure(this Uri uri)
		{
			return uri.Scheme == Uri.UriSchemeHttps;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000034F0 File Offset: 0x000016F0
		internal static IEnumerable<KeyValuePair<string, string>> GetQueryParameters(this Uri uri)
		{
			if (uri.Query.Length == 0)
			{
				return new KeyValuePair<string, string>[0];
			}
			string text = uri.Query.Remove(0, 1);
			return (from x in text.Split(new char[] { '&' })
				where x.Split(new char[] { '=' }).Length == 2
				select x).Select(delegate(string x)
			{
				string[] array = x.Split(new char[] { '=' });
				string text2 = array[0];
				string text3 = array[1];
				return new KeyValuePair<string, string>(text2, text3);
			});
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003578 File Offset: 0x00001778
		internal static Uri AddOrUpdateValueForQueryParameter(this Uri uri, string key, string value)
		{
			KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>(key, value);
			if (string.IsNullOrEmpty(uri.Query))
			{
				return new Uri(string.Concat(new string[] { uri.AbsoluteUri, "?", key, "=", value }));
			}
			IEnumerable<KeyValuePair<string, string>> enumerable = uri.GetQueryParameters();
			if (enumerable.Any((KeyValuePair<string, string> x) => x.Key == key))
			{
				enumerable = enumerable.Where((KeyValuePair<string, string> x) => x.Key != key).Concat(new KeyValuePair<string, string>[] { keyValuePair });
			}
			else
			{
				enumerable = enumerable.Concat(new KeyValuePair<string, string>[] { keyValuePair });
			}
			string text = enumerable.Aggregate("?", (string next, KeyValuePair<string, string> curr) => string.Concat(new string[] { next, curr.Key, "=", curr.Value, "&" }));
			text = text.Remove(text.Length - 1);
			return new Uri(uri.AbsoluteUri.Split(new char[] { '?' }).First<string>() + text);
		}
	}
}
