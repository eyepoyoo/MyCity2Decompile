using System;

namespace LEGO.CoreSDK.Network.Unity
{
	// Token: 0x0200001E RID: 30
	internal static class HttpMethodExtensions
	{
		// Token: 0x0600004C RID: 76 RVA: 0x0000326C File Offset: 0x0000146C
		internal static string ToUnity(this HttpMethod method)
		{
			switch (method)
			{
			case HttpMethod.Get:
				return "GET";
			case HttpMethod.Post:
				return "POST";
			case HttpMethod.Delete:
				return "DELETE";
			case HttpMethod.Create:
				return "CREATE";
			case HttpMethod.Head:
				return "HEAD";
			case HttpMethod.Put:
				return "PUT";
			default:
				throw new NotImplementedException("There is currently no implementation for: '" + method + "'");
			}
		}
	}
}
