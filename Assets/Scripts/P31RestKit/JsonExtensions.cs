using System;
using System.Collections;
using System.Collections.Generic;

namespace Prime31
{
	// Token: 0x02000015 RID: 21
	public static class JsonExtensions
	{
		// Token: 0x0600008A RID: 138 RVA: 0x00005CE4 File Offset: 0x00003EE4
		public static string toJson(this IList obj)
		{
			return Json.encode(obj);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00005CEC File Offset: 0x00003EEC
		public static string toJson(this IDictionary obj)
		{
			return Json.encode(obj);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00005CF4 File Offset: 0x00003EF4
		public static List<object> listFromJson(this string json)
		{
			return Json.decode(json) as List<object>;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00005D01 File Offset: 0x00003F01
		public static Dictionary<string, object> dictionaryFromJson(this string json)
		{
			return Json.decode(json) as Dictionary<string, object>;
		}
	}
}
