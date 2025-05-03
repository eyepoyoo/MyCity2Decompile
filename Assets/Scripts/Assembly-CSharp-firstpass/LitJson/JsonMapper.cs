using System;

namespace LitJson
{
	// Token: 0x02000024 RID: 36
	public class JsonMapper
	{
		// Token: 0x0600025F RID: 607 RVA: 0x0000E8F4 File Offset: 0x0000CAF4
		public static JsonData ToObject(JsonReader reader)
		{
			return JsonMapperLite.ToObject(reader);
		}
	}
}
