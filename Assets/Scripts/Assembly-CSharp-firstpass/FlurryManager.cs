using System;
using Prime31;

// Token: 0x0200000A RID: 10
public class FlurryManager : AbstractManager
{
	// Token: 0x06000042 RID: 66 RVA: 0x00002CFC File Offset: 0x00000EFC
	static FlurryManager()
	{
		AbstractManager.initialize(typeof(FlurryManager));
	}

	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000043 RID: 67 RVA: 0x00002D10 File Offset: 0x00000F10
	// (remove) Token: 0x06000044 RID: 68 RVA: 0x00002D28 File Offset: 0x00000F28
	public static event Action<string> spaceDidDismissEvent;

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000045 RID: 69 RVA: 0x00002D40 File Offset: 0x00000F40
	// (remove) Token: 0x06000046 RID: 70 RVA: 0x00002D58 File Offset: 0x00000F58
	public static event Action<string> spaceWillLeaveApplicationEvent;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x06000047 RID: 71 RVA: 0x00002D70 File Offset: 0x00000F70
	// (remove) Token: 0x06000048 RID: 72 RVA: 0x00002D88 File Offset: 0x00000F88
	public static event Action<string> spaceDidFailToRenderEvent;

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x06000049 RID: 73 RVA: 0x00002DA0 File Offset: 0x00000FA0
	// (remove) Token: 0x0600004A RID: 74 RVA: 0x00002DB8 File Offset: 0x00000FB8
	public static event Action<string> spaceDidFailToReceiveAdEvent;

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x0600004B RID: 75 RVA: 0x00002DD0 File Offset: 0x00000FD0
	// (remove) Token: 0x0600004C RID: 76 RVA: 0x00002DE8 File Offset: 0x00000FE8
	public static event Action<string> spaceDidReceiveAdEvent;

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x0600004D RID: 77 RVA: 0x00002E00 File Offset: 0x00001000
	// (remove) Token: 0x0600004E RID: 78 RVA: 0x00002E18 File Offset: 0x00001018
	public static event Action<string> videoDidFinishEvent;

	// Token: 0x0600004F RID: 79 RVA: 0x00002E30 File Offset: 0x00001030
	private void spaceDidDismiss(string space)
	{
		if (FlurryManager.spaceDidDismissEvent != null)
		{
			FlurryManager.spaceDidDismissEvent(space);
		}
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00002E48 File Offset: 0x00001048
	private void spaceWillLeaveApplication(string space)
	{
		if (FlurryManager.spaceWillLeaveApplicationEvent != null)
		{
			FlurryManager.spaceWillLeaveApplicationEvent(space);
		}
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00002E60 File Offset: 0x00001060
	private void spaceDidFailToRender(string space)
	{
		if (FlurryManager.spaceDidFailToRenderEvent != null)
		{
			FlurryManager.spaceDidFailToRenderEvent(space);
		}
	}

	// Token: 0x06000052 RID: 82 RVA: 0x00002E78 File Offset: 0x00001078
	private void spaceDidFailToReceiveAd(string space)
	{
		if (FlurryManager.spaceDidFailToReceiveAdEvent != null)
		{
			FlurryManager.spaceDidFailToReceiveAdEvent(space);
		}
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00002E90 File Offset: 0x00001090
	private void spaceDidReceiveAd(string space)
	{
		if (FlurryManager.spaceDidReceiveAdEvent != null)
		{
			FlurryManager.spaceDidReceiveAdEvent(space);
		}
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00002EA8 File Offset: 0x000010A8
	private void videoDidFinish(string space)
	{
		if (FlurryManager.videoDidFinishEvent != null)
		{
			FlurryManager.videoDidFinishEvent(space);
		}
	}
}
