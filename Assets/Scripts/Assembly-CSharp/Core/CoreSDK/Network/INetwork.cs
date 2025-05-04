using System;
using System.Collections.Generic;
using System.Net;

namespace LEGO.CoreSDK.Network
{
	// Token: 0x02000029 RID: 41
	public interface INetwork
	{
		// Token: 0x06000062 RID: 98
		INetworkRequest GetJSONRequest(Uri endPoint, Dictionary<string, string> parameters, Dictionary<string, string> headers, Action<HttpStatusCode, string, Dictionary<string, string>> success, Action<HttpStatusCode, string> failure, Action @finally, bool useCache = true);
	}
}
