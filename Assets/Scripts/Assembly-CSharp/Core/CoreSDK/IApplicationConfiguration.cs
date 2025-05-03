using System;
using System.Collections.Generic;

namespace LEGO.CoreSDK
{
	// Token: 0x02000078 RID: 120
	public interface IApplicationConfiguration
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001D9 RID: 473
		string Name { get; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001DA RID: 474
		string ContentLanguage { get; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001DB RID: 475
		Version Version { get; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001DC RID: 476
		string Experience { get; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001DD RID: 477
		bool Tracking { get; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001DE RID: 478
		IDictionary<string, string> CustomValues { get; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001DF RID: 479
		IDictionary<string, string> Endpoints { get; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001E0 RID: 480
		IDictionary<string, string> Assets { get; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001E1 RID: 481
		JSONObject RawJson { get; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001E2 RID: 482
		Version MinimumVersion { get; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001E3 RID: 483
		bool KillSwitch { get; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001E4 RID: 484
		string KillSwitchTitle { get; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001E5 RID: 485
		string KillSwitchMessage { get; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001E6 RID: 486
		string MinimumVersionTitle { get; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001E7 RID: 487
		string MinimumVersionMessage { get; }
	}
}
