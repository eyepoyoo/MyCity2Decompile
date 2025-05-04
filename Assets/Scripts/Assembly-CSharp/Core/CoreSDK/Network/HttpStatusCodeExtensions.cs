using System;
using System.Linq;
using System.Net;

namespace LEGO.CoreSDK.Network
{
	// Token: 0x0200002A RID: 42
	internal static class HttpStatusCodeExtensions
	{
		// Token: 0x06000063 RID: 99 RVA: 0x000037DC File Offset: 0x000019DC
		internal static bool IsSuccess(this HttpStatusCode statusCode)
		{
			return Enumerable.Range(200, 100).Contains((int)statusCode);
		}
	}
}
