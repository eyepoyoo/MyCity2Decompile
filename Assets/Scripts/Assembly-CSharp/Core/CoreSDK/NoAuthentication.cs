using System;

namespace LEGO.CoreSDK
{
	// Token: 0x02000074 RID: 116
	internal class NoAuthentication : IAuthentication
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0000829C File Offset: 0x0000649C
		public bool LoggedIn
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001CB RID: 459 RVA: 0x000082A0 File Offset: 0x000064A0
		public string VPCUserTrackingId
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001CC RID: 460 RVA: 0x000082A4 File Offset: 0x000064A4
		public string Gender
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001CD RID: 461 RVA: 0x000082A8 File Offset: 0x000064A8
		public int Age
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001CE RID: 462 RVA: 0x000082AC File Offset: 0x000064AC
		public string UserLocation
		{
			get
			{
				return null;
			}
		}
	}
}
