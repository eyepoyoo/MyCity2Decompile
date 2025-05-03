using System;
using System.Collections.Generic;
using System.Linq;

namespace LEGO.CoreSDK.Legal
{
	// Token: 0x0200004C RID: 76
	public struct LegalPolicies
	{
		// Token: 0x060000DA RID: 218 RVA: 0x000054D8 File Offset: 0x000036D8
		public LegalPolicies(string text)
		{
			JSONObject jsonobject = new JSONObject(text, -2, false, false);
			this.Title = jsonobject.StringForKey("title", true);
			this.Policies = jsonobject.GetField("policies").list.Select((JSONObject x) => new LegalPolicies.Policy(x));
		}

		// Token: 0x040000A1 RID: 161
		public string Title;

		// Token: 0x040000A2 RID: 162
		public IEnumerable<LegalPolicies.Policy> Policies;

		// Token: 0x0200004D RID: 77
		public struct Policy
		{
			// Token: 0x060000DC RID: 220 RVA: 0x00005544 File Offset: 0x00003744
			public Policy(JSONObject json)
			{
				this.Title = json.StringForKey("title", true);
				this.LastChanged = json.StringForKey("lastChanged", true);
				this.Sections = json["sections"].list.Select((JSONObject x) => new LegalPolicies.Policy.Section(x));
				string text = json.StringForKey("type", true);
				switch (text)
				{
				case "termsOfUse":
					this.Type = PolicyType.Terms;
					return;
				case "privacy":
					this.Type = PolicyType.Privacy;
					return;
				case "cookies":
					this.Type = PolicyType.Cookie;
					return;
				}
				this.Type = PolicyType.Unknown;
			}

			// Token: 0x040000A4 RID: 164
			public string Title;

			// Token: 0x040000A5 RID: 165
			public string LastChanged;

			// Token: 0x040000A6 RID: 166
			public PolicyType Type;

			// Token: 0x040000A7 RID: 167
			public IEnumerable<LegalPolicies.Policy.Section> Sections;

			// Token: 0x0200004E RID: 78
			public struct Section
			{
				// Token: 0x060000DE RID: 222 RVA: 0x0000565C File Offset: 0x0000385C
				public Section(JSONObject json)
				{
					this.Title = json.StringForKey("title", true);
					this.Text = json.StringForKey("text", true);
				}

				// Token: 0x040000AA RID: 170
				public string Title;

				// Token: 0x040000AB RID: 171
				public string Text;
			}
		}
	}
}
