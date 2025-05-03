using System;
using System.Collections.Generic;
using System.Net;

namespace LEGO.CoreSDK.Network
{
	// Token: 0x02000023 RID: 35
	internal class OfflineNetwork : INetwork
	{
		// Token: 0x0600005D RID: 93 RVA: 0x00003738 File Offset: 0x00001938
		public INetworkRequest GetJSONRequest(Uri endPoint, Dictionary<string, string> parameters, Dictionary<string, string> headers, Action<HttpStatusCode, string, Dictionary<string, string>> success, Action<HttpStatusCode, string> failure, Action @finally, bool useCache = true)
		{
			if (@finally != null)
			{
				@finally();
			}
			return null;
		}
	}
}
