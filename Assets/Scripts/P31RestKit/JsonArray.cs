using System;
using System.Collections.Generic;

namespace Prime31
{
	// Token: 0x02000018 RID: 24
	public class JsonArray : List<object>
	{
		// Token: 0x06000093 RID: 147 RVA: 0x000060E6 File Offset: 0x000042E6
		public JsonArray()
		{
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000060EE File Offset: 0x000042EE
		public JsonArray(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000060F7 File Offset: 0x000042F7
		public override string ToString()
		{
			return JsonFormatter.prettyPrint(SimpleJson.encode(this)) ?? string.Empty;
		}
	}
}
