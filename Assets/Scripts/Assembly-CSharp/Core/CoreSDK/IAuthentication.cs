using System;

namespace LEGO.CoreSDK
{
	// Token: 0x02000076 RID: 118
	public interface IAuthentication
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001D1 RID: 465
		bool LoggedIn { get; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001D2 RID: 466
		string VPCUserTrackingId { get; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001D3 RID: 467
		string Gender { get; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001D4 RID: 468
		int Age { get; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001D5 RID: 469
		string UserLocation { get; }
	}
}
