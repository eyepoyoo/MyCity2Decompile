using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Prime31
{
	// Token: 0x0200000A RID: 10
	public class OAuthManager
	{
		// Token: 0x06000024 RID: 36 RVA: 0x00002F7C File Offset: 0x0000117C
		public OAuthManager()
		{
			this._random = new Random();
			this._params = new SortedDictionary<string, string>();
			this._params["consumer_key"] = "";
			this._params["consumer_secret"] = "";
			this._params["timestamp"] = this.generateTimeStamp();
			this._params["nonce"] = this.generateNonce();
			this._params["signature_method"] = "HMAC-SHA1";
			this._params["signature"] = "";
			this._params["token"] = "";
			this._params["token_secret"] = "";
			this._params["version"] = "1.0";
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00003064 File Offset: 0x00001264
		public OAuthManager(string consumerKey, string consumerSecret, string token, string tokenSecret)
			: this()
		{
			this._params["consumer_key"] = consumerKey;
			this._params["consumer_secret"] = consumerSecret;
			this._params["token"] = token;
			this._params["token_secret"] = tokenSecret;
		}

		// Token: 0x17000004 RID: 4
		public string this[string ix]
		{
			get
			{
				if (this._params.ContainsKey(ix))
				{
					return this._params[ix];
				}
				throw new ArgumentException(ix);
			}
			set
			{
				if (!this._params.ContainsKey(ix))
				{
					throw new ArgumentException(ix);
				}
				this._params[ix] = value;
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000310C File Offset: 0x0000130C
		private string generateTimeStamp()
		{
			return Convert.ToInt64((DateTime.UtcNow - OAuthManager._epoch).TotalSeconds).ToString();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00003143 File Offset: 0x00001343
		private void prepareNewRequest()
		{
			this._params["nonce"] = this.generateNonce();
			this._params["timestamp"] = this.generateTimeStamp();
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003174 File Offset: 0x00001374
		private string generateNonce()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 8; i++)
			{
				int num = this._random.Next(3);
				if (num != 0)
				{
					stringBuilder.Append((char)(this._random.Next(10) + 48), 1);
				}
				else
				{
					stringBuilder.Append((char)(this._random.Next(26) + 97), 1);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000031F4 File Offset: 0x000013F4
		private SortedDictionary<string, string> extractQueryParameters(string queryString)
		{
			if (queryString.StartsWith("?"))
			{
				queryString = queryString.Remove(0, 1);
			}
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
			if (string.IsNullOrEmpty(queryString))
			{
				return sortedDictionary;
			}
			foreach (string text in queryString.Split(new char[] { '&' }))
			{
				if (!string.IsNullOrEmpty(text) && !text.StartsWith("oauth_"))
				{
					if (text.IndexOf('=') > -1)
					{
						string[] array2 = text.Split(new char[] { '=' });
						sortedDictionary.Add(array2[0], array2[1]);
					}
					else
					{
						sortedDictionary.Add(text, string.Empty);
					}
				}
			}
			return sortedDictionary;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000032B4 File Offset: 0x000014B4
		public static string urlEncode(string value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in value)
			{
				if (OAuthManager.unreservedChars.IndexOf(c) != -1)
				{
					stringBuilder.Append(c);
				}
				else
				{
					stringBuilder.Append('%' + string.Format("{0:X2}", (int)c));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003330 File Offset: 0x00001530
		private static SortedDictionary<string, string> mergePostParamsWithOauthParams(SortedDictionary<string, string> postParams, SortedDictionary<string, string> oAuthParams)
		{
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
			foreach (KeyValuePair<string, string> keyValuePair in postParams)
			{
				sortedDictionary.Add(keyValuePair.Key, keyValuePair.Value);
			}
			foreach (KeyValuePair<string, string> keyValuePair2 in oAuthParams)
			{
				if (!string.IsNullOrEmpty(keyValuePair2.Value) && !keyValuePair2.Key.EndsWith("secret"))
				{
					sortedDictionary.Add("oauth_" + keyValuePair2.Key, keyValuePair2.Value);
				}
			}
			return sortedDictionary;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003420 File Offset: 0x00001620
		private static string encodeRequestParameters(SortedDictionary<string, string> p)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, string> keyValuePair in p)
			{
				if (!string.IsNullOrEmpty(keyValuePair.Value) && !keyValuePair.Key.EndsWith("secret"))
				{
					stringBuilder.AppendFormat("oauth_{0}=\"{1}\", ", keyValuePair.Key, OAuthManager.urlEncode(keyValuePair.Value));
				}
			}
			return stringBuilder.ToString().TrimEnd(new char[] { ' ' }).TrimEnd(new char[] { ',' });
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000034E4 File Offset: 0x000016E4
		public static byte[] encodePostParameters(SortedDictionary<string, string> p)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, string> keyValuePair in p)
			{
				if (!string.IsNullOrEmpty(keyValuePair.Value))
				{
					stringBuilder.AppendFormat("{0}={1}, ", OAuthManager.urlEncode(keyValuePair.Key), OAuthManager.urlEncode(keyValuePair.Value));
				}
			}
			return Encoding.UTF8.GetBytes(stringBuilder.ToString().TrimEnd(new char[] { ' ' }).TrimEnd(new char[] { ',' }));
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000035A0 File Offset: 0x000017A0
		public OAuthResponse acquireRequestToken(string uri, string method)
		{
			this.prepareNewRequest();
			string authorizationHeader = this.getAuthorizationHeader(uri, method);
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
			httpWebRequest.Headers.Add("Authorization", authorizationHeader);
			httpWebRequest.Method = method;
			OAuthResponse oauthResponse2;
			using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
			{
				using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
				{
					OAuthResponse oauthResponse = new OAuthResponse(streamReader.ReadToEnd());
					this["token"] = oauthResponse["oauth_token"];
					try
					{
						if (oauthResponse["oauth_token_secret"] != null)
						{
							this["token_secret"] = oauthResponse["oauth_token_secret"];
						}
					}
					catch
					{
					}
					oauthResponse2 = oauthResponse;
				}
			}
			return oauthResponse2;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000369C File Offset: 0x0000189C
		public OAuthResponse acquireAccessToken(string uri, string method, string verifier)
		{
			this.prepareNewRequest();
			this._params["verifier"] = verifier;
			string authorizationHeader = this.getAuthorizationHeader(uri, method);
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
			httpWebRequest.Headers.Add("Authorization", authorizationHeader);
			httpWebRequest.Method = method;
			OAuthResponse oauthResponse2;
			using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
			{
				using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
				{
					OAuthResponse oauthResponse = new OAuthResponse(streamReader.ReadToEnd());
					this["token"] = oauthResponse["oauth_token"];
					this["token_secret"] = oauthResponse["oauth_token_secret"];
					oauthResponse2 = oauthResponse;
				}
			}
			return oauthResponse2;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003784 File Offset: 0x00001984
		public string generateCredsHeader(string uri, string method, string realm)
		{
			this.prepareNewRequest();
			return this.getAuthorizationHeader(uri, method, realm);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003795 File Offset: 0x00001995
		public string generateAuthzHeader(string uri, string method)
		{
			this.prepareNewRequest();
			return this.getAuthorizationHeader(uri, method, null);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000037A6 File Offset: 0x000019A6
		private string getAuthorizationHeader(string uri, string method)
		{
			return this.getAuthorizationHeader(uri, method, null);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000037B4 File Offset: 0x000019B4
		private string getAuthorizationHeader(string uri, string method, string realm)
		{
			if (string.IsNullOrEmpty(this._params["consumer_key"]))
			{
				throw new ArgumentNullException("consumer_key");
			}
			if (string.IsNullOrEmpty(this._params["signature_method"]))
			{
				throw new ArgumentNullException("signature_method");
			}
			this.sign(uri, method);
			string text = OAuthManager.encodeRequestParameters(this._params);
			return (!string.IsNullOrEmpty(realm)) ? (string.Format("OAuth realm=\"{0}\", ", realm) + text) : ("OAuth " + text);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000384C File Offset: 0x00001A4C
		private void sign(string uri, string method)
		{
			string signatureBase = this.getSignatureBase(uri, method);
			HashAlgorithm hash = this.getHash();
			byte[] bytes = Encoding.ASCII.GetBytes(signatureBase);
			byte[] array = hash.ComputeHash(bytes);
			this["signature"] = Convert.ToBase64String(array);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003890 File Offset: 0x00001A90
		private string getSignatureBase(string url, string method)
		{
			Uri uri = new Uri(url);
			string text = string.Format("{0}://{1}", uri.Scheme, uri.Host);
			if ((!(uri.Scheme == "http") || uri.Port != 80) && (!(uri.Scheme == "https") || uri.Port != 443))
			{
				text = text + ":" + uri.Port;
			}
			text += uri.AbsolutePath;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(method).Append('&').Append(OAuthManager.urlEncode(text))
				.Append('&');
			SortedDictionary<string, string> sortedDictionary = this.extractQueryParameters(uri.Query);
			foreach (KeyValuePair<string, string> keyValuePair in this._params)
			{
				if (!string.IsNullOrEmpty(this._params[keyValuePair.Key]) && !keyValuePair.Key.EndsWith("_secret") && !keyValuePair.Key.EndsWith("signature"))
				{
					sortedDictionary.Add("oauth_" + keyValuePair.Key, keyValuePair.Value);
				}
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			foreach (KeyValuePair<string, string> keyValuePair2 in sortedDictionary)
			{
				stringBuilder2.AppendFormat("{0}={1}&", keyValuePair2.Key, keyValuePair2.Value);
			}
			stringBuilder.Append(OAuthManager.urlEncode(stringBuilder2.ToString().TrimEnd(new char[] { '&' })));
			return stringBuilder.ToString();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003A9C File Offset: 0x00001C9C
		private HashAlgorithm getHash()
		{
			if (this["signature_method"] != "HMAC-SHA1")
			{
				throw new NotImplementedException();
			}
			string text = string.Format("{0}&{1}", OAuthManager.urlEncode(this["consumer_secret"]), OAuthManager.urlEncode(this["token_secret"]));
			return new HMACSHA1
			{
				Key = Encoding.ASCII.GetBytes(text)
			};
		}

		// Token: 0x0400000F RID: 15
		private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);

		// Token: 0x04000010 RID: 16
		private SortedDictionary<string, string> _params;

		// Token: 0x04000011 RID: 17
		private Random _random;

		// Token: 0x04000012 RID: 18
		private static string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
	}
}
