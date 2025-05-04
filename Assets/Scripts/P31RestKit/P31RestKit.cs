using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Prime31
{
	// Token: 0x02000003 RID: 3
	public class P31RestKit
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x00002060 File Offset: 0x00000260
		// (set) Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		protected virtual GameObject surrogateGameObject
		{
			get
			{
				if (this._surrogateGameObject == null)
				{
					this._surrogateGameObject = GameObject.Find("P31CoroutineSurrogate");
					if (this._surrogateGameObject == null)
					{
						this._surrogateGameObject = new GameObject("P31CoroutineSurrogate");
						global::UnityEngine.Object.DontDestroyOnLoad(this._surrogateGameObject);
					}
				}
				return this._surrogateGameObject;
			}
			set
			{
				this._surrogateGameObject = value;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020C9 File Offset: 0x000002C9
		// (set) Token: 0x06000005 RID: 5 RVA: 0x000020F3 File Offset: 0x000002F3
		protected MonoBehaviour surrogateMonobehaviour
		{
			get
			{
				if (this._surrogateMonobehaviour == null)
				{
					this._surrogateMonobehaviour = this.surrogateGameObject.AddComponent<MonoBehaviour>();
				}
				return this._surrogateMonobehaviour;
			}
			set
			{
				this._surrogateMonobehaviour = value;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020FC File Offset: 0x000002FC
		protected virtual IEnumerator send(string path, HTTPVerb httpVerb, Dictionary<string, object> parameters, Action<string, object> onComplete)
		{
			if (path.StartsWith("/"))
			{
				path = path.Substring(1);
			}
			WWW www = this.processRequest(path, httpVerb, parameters);
			yield return www;
			if (this.debugRequests)
			{
				Debug.Log("response error: " + www.error);
				Debug.Log("response text: " + www.text);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("Response Headers:\n");
				foreach (KeyValuePair<string, string> keyValuePair in www.responseHeaders)
				{
					stringBuilder.AppendFormat("{0}: {1}\n", keyValuePair.Key, keyValuePair.Value);
				}
				Debug.Log(stringBuilder.ToString());
			}
			if (onComplete != null)
			{
				this.processResponse(www, onComplete);
			}
			www.Dispose();
			yield break;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002134 File Offset: 0x00000334
		protected virtual WWW processRequest(string path, HTTPVerb httpVerb, Dictionary<string, object> parameters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!path.StartsWith("http"))
			{
				stringBuilder.Append(this._baseUrl).Append(path);
			}
			else
			{
				stringBuilder.Append(path);
			}
			bool flag = httpVerb != HTTPVerb.GET;
			WWWForm wwwform = ((!flag) ? null : new WWWForm());
			if (parameters != null && parameters.Count > 0)
			{
				if (flag)
				{
					foreach (KeyValuePair<string, object> keyValuePair in parameters)
					{
						if (keyValuePair.Value is string)
						{
							wwwform.AddField(keyValuePair.Key, keyValuePair.Value as string);
						}
						else if (keyValuePair.Value is byte[])
						{
							wwwform.AddBinaryData(keyValuePair.Key, keyValuePair.Value as byte[]);
						}
					}
				}
				else
				{
					bool flag2 = true;
					if (path.Contains("?"))
					{
						flag2 = false;
					}
					foreach (KeyValuePair<string, object> keyValuePair2 in parameters)
					{
						if (keyValuePair2.Value is string)
						{
							stringBuilder.AppendFormat("{0}{1}={2}", (!flag2) ? "&" : "?", WWW.EscapeURL(keyValuePair2.Key), WWW.EscapeURL(keyValuePair2.Value as string));
							flag2 = false;
						}
					}
				}
			}
			if (this.debugRequests)
			{
				Debug.Log("url: " + stringBuilder.ToString());
			}
			return (!flag) ? new WWW(stringBuilder.ToString()) : new WWW(stringBuilder.ToString(), wwwform);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000233C File Offset: 0x0000053C
		protected virtual Hashtable headersForRequest(HTTPVerb httpVerb, Dictionary<string, object> parameters)
		{
			if (httpVerb == HTTPVerb.PUT)
			{
				return new Hashtable { { "X-HTTP-Method-Override", "PUT" } };
			}
			return null;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000236C File Offset: 0x0000056C
		protected virtual void processResponse(WWW www, Action<string, object> onComplete)
		{
			if (!string.IsNullOrEmpty(www.error))
			{
				onComplete(www.error, null);
			}
			else if (this.isResponseJson(www))
			{
				object obj = Json.decode(www.text);
				if (obj == null)
				{
					obj = www.text;
				}
				onComplete(null, obj);
			}
			else
			{
				onComplete(null, www.text);
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000023DC File Offset: 0x000005DC
		protected bool isResponseJson(WWW www)
		{
			bool flag = false;
			if (this.forceJsonResponse)
			{
				flag = true;
			}
			if (!flag)
			{
				foreach (KeyValuePair<string, string> keyValuePair in www.responseHeaders)
				{
					if (keyValuePair.Key.ToLower() == "content-type" && (keyValuePair.Value.ToLower().Contains("/json") || keyValuePair.Value.ToLower().Contains("/javascript")))
					{
						flag = true;
					}
				}
			}
			return (!flag || www.text.StartsWith("[") || www.text.StartsWith("{")) && flag;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000024CC File Offset: 0x000006CC
		public void get(string path, Action<string, object> completionHandler)
		{
			this.get(path, null, completionHandler);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000024D7 File Offset: 0x000006D7
		public void get(string path, Dictionary<string, object> parameters, Action<string, object> completionHandler)
		{
			this.surrogateMonobehaviour.StartCoroutine(this.send(path, HTTPVerb.GET, parameters, completionHandler));
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000024EF File Offset: 0x000006EF
		public void post(string path, Action<string, object> completionHandler)
		{
			this.post(path, null, completionHandler);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000024FA File Offset: 0x000006FA
		public void post(string path, Dictionary<string, object> parameters, Action<string, object> completionHandler)
		{
			this.surrogateMonobehaviour.StartCoroutine(this.send(path, HTTPVerb.POST, parameters, completionHandler));
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002512 File Offset: 0x00000712
		public void put(string path, Action<string, object> completionHandler)
		{
			this.put(path, null, completionHandler);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000251D File Offset: 0x0000071D
		public void put(string path, Dictionary<string, object> parameters, Action<string, object> completionHandler)
		{
			this.surrogateMonobehaviour.StartCoroutine(this.send(path, HTTPVerb.PUT, parameters, completionHandler));
		}

		// Token: 0x04000006 RID: 6
		protected string _baseUrl;

		// Token: 0x04000007 RID: 7
		public bool debugRequests = false;

		// Token: 0x04000008 RID: 8
		protected bool forceJsonResponse;

		// Token: 0x04000009 RID: 9
		private GameObject _surrogateGameObject;

		// Token: 0x0400000A RID: 10
		private MonoBehaviour _surrogateMonobehaviour;
	}
}
