using System;

namespace LEGO.CoreSDK.Network
{
	// Token: 0x02000027 RID: 39
	public static class NetworkConstantMethodExtensions
	{
		// Token: 0x0600005E RID: 94 RVA: 0x0000374C File Offset: 0x0000194C
		public static string Identifier(this HTTPHeaderRequestField requestField)
		{
			switch (requestField)
			{
			case HTTPHeaderRequestField.IfModifiedSince:
				return "If-Modified-Since";
			case HTTPHeaderRequestField.ContentType:
				return "Content-Type";
			case HTTPHeaderRequestField.Accept:
				return "Accept";
			default:
				return null;
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003788 File Offset: 0x00001988
		public static string Identifier(this HTTPHeaderAcceptedContent requestField)
		{
			if (requestField != HTTPHeaderAcceptedContent.JSON)
			{
				return null;
			}
			return "application/json";
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000037AC File Offset: 0x000019AC
		public static string Identifier(this HTTPHeaderResponseField requestField)
		{
			if (requestField == HTTPHeaderResponseField.ETag)
			{
				return "ETag";
			}
			if (requestField != HTTPHeaderResponseField.LastModified)
			{
				return null;
			}
			return "LAST-MODIFIED";
		}
	}
}
