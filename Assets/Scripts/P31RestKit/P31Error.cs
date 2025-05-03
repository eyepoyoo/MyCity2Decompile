using System;
using System.Collections.Generic;

namespace Prime31
{
	// Token: 0x02000025 RID: 37
	public sealed class P31Error
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00007B50 File Offset: 0x00005D50
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00007B58 File Offset: 0x00005D58
		public string message { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00007B61 File Offset: 0x00005D61
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00007B69 File Offset: 0x00005D69
		public string domain { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00007B72 File Offset: 0x00005D72
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x00007B7A File Offset: 0x00005D7A
		public int code { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00007B83 File Offset: 0x00005D83
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00007B8B File Offset: 0x00005D8B
		public Dictionary<string, object> userInfo { get; private set; }

		// Token: 0x060000EC RID: 236 RVA: 0x00007B94 File Offset: 0x00005D94
		public static P31Error errorFromJson(string json)
		{
			P31Error p31Error = new P31Error();
			if (!json.StartsWith("{"))
			{
				p31Error.message = json;
				p31Error._containsOnlyMessage = true;
				return p31Error;
			}
			Dictionary<string, object> dictionary = Json.decode(json) as Dictionary<string, object>;
			if (dictionary == null)
			{
				p31Error.message = "Unknown error";
			}
			else
			{
				p31Error.message = ((!dictionary.ContainsKey("message")) ? null : dictionary["message"].ToString());
				p31Error.domain = ((!dictionary.ContainsKey("domain")) ? null : dictionary["domain"].ToString());
				p31Error.code = ((!dictionary.ContainsKey("code")) ? (-1) : int.Parse(dictionary["code"].ToString()));
				p31Error.userInfo = ((!dictionary.ContainsKey("userInfo")) ? null : (dictionary["userInfo"] as Dictionary<string, object>));
			}
			return p31Error;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00007CA0 File Offset: 0x00005EA0
		public override string ToString()
		{
			if (this._containsOnlyMessage)
			{
				return string.Format("[P31Error]: {0}", this.message);
			}
			string text2;
			try
			{
				string text = Json.encode(this);
				text2 = string.Format("[P31Error]: {0}", JsonFormatter.prettyPrint(text));
			}
			catch (Exception)
			{
				text2 = string.Format("[P31Error]: {0}", this.message);
			}
			return text2;
		}

		// Token: 0x04000065 RID: 101
		private bool _containsOnlyMessage;
	}
}
