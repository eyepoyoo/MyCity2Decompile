using System;
using System.Collections.Generic;

namespace Prime31
{
	// Token: 0x02000019 RID: 25
	public class JsonObject : Dictionary<string, object>
	{
		// Token: 0x06000097 RID: 151 RVA: 0x00006118 File Offset: 0x00004318
		public override string ToString()
		{
			return JsonFormatter.prettyPrint(SimpleJson.encode(this)) ?? string.Empty;
		}
	}
}
