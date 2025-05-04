using System;
using Prime31;

// Token: 0x0200000D RID: 13
public class FlurryAndroidManager : AbstractManager
{
	// Token: 0x0600006D RID: 109 RVA: 0x000033EC File Offset: 0x000015EC
	static FlurryAndroidManager()
	{
		AbstractManager.initialize(typeof(FlurryAndroidManager));
	}

	// Token: 0x14000007 RID: 7
	// (add) Token: 0x0600006E RID: 110 RVA: 0x00003400 File Offset: 0x00001600
	// (remove) Token: 0x0600006F RID: 111 RVA: 0x00003418 File Offset: 0x00001618
	public static event Action<string> adAvailableForSpaceEvent;

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06000070 RID: 112 RVA: 0x00003430 File Offset: 0x00001630
	// (remove) Token: 0x06000071 RID: 113 RVA: 0x00003448 File Offset: 0x00001648
	public static event Action<string> adNotAvailableForSpaceEvent;

	// Token: 0x14000009 RID: 9
	// (add) Token: 0x06000072 RID: 114 RVA: 0x00003460 File Offset: 0x00001660
	// (remove) Token: 0x06000073 RID: 115 RVA: 0x00003478 File Offset: 0x00001678
	public static event Action<string> onAdClosedEvent;

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06000074 RID: 116 RVA: 0x00003490 File Offset: 0x00001690
	// (remove) Token: 0x06000075 RID: 117 RVA: 0x000034A8 File Offset: 0x000016A8
	public static event Action<string> onApplicationExitEvent;

	// Token: 0x1400000B RID: 11
	// (add) Token: 0x06000076 RID: 118 RVA: 0x000034C0 File Offset: 0x000016C0
	// (remove) Token: 0x06000077 RID: 119 RVA: 0x000034D8 File Offset: 0x000016D8
	public static event Action<string> onRenderFailedEvent;

	// Token: 0x1400000C RID: 12
	// (add) Token: 0x06000078 RID: 120 RVA: 0x000034F0 File Offset: 0x000016F0
	// (remove) Token: 0x06000079 RID: 121 RVA: 0x00003508 File Offset: 0x00001708
	public static event Action<string> spaceDidFailToReceiveAdEvent;

	// Token: 0x1400000D RID: 13
	// (add) Token: 0x0600007A RID: 122 RVA: 0x00003520 File Offset: 0x00001720
	// (remove) Token: 0x0600007B RID: 123 RVA: 0x00003538 File Offset: 0x00001738
	public static event Action<string> spaceDidReceiveAdEvent;

	// Token: 0x1400000E RID: 14
	// (add) Token: 0x0600007C RID: 124 RVA: 0x00003550 File Offset: 0x00001750
	// (remove) Token: 0x0600007D RID: 125 RVA: 0x00003568 File Offset: 0x00001768
	public static event Action<string> onAdClickedEvent;

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x0600007E RID: 126 RVA: 0x00003580 File Offset: 0x00001780
	// (remove) Token: 0x0600007F RID: 127 RVA: 0x00003598 File Offset: 0x00001798
	public static event Action<string> onAdOpenedEvent;

	// Token: 0x14000010 RID: 16
	// (add) Token: 0x06000080 RID: 128 RVA: 0x000035B0 File Offset: 0x000017B0
	// (remove) Token: 0x06000081 RID: 129 RVA: 0x000035C8 File Offset: 0x000017C8
	public static event Action<string> onVideoCompletedEvent;

	// Token: 0x06000082 RID: 130 RVA: 0x000035E0 File Offset: 0x000017E0
	public void adAvailableForSpace(string adSpace)
	{
		if (FlurryAndroidManager.adAvailableForSpaceEvent != null)
		{
			FlurryAndroidManager.adAvailableForSpaceEvent(adSpace);
		}
	}

	// Token: 0x06000083 RID: 131 RVA: 0x000035F8 File Offset: 0x000017F8
	public void adNotAvailableForSpace(string adSpace)
	{
		if (FlurryAndroidManager.adNotAvailableForSpaceEvent != null)
		{
			FlurryAndroidManager.adNotAvailableForSpaceEvent(adSpace);
		}
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00003610 File Offset: 0x00001810
	public void onAdClosed(string adSpace)
	{
		if (FlurryAndroidManager.onAdClosedEvent != null)
		{
			FlurryAndroidManager.onAdClosedEvent(adSpace);
		}
	}

	// Token: 0x06000085 RID: 133 RVA: 0x00003628 File Offset: 0x00001828
	public void onApplicationExit(string adSpace)
	{
		if (FlurryAndroidManager.onApplicationExitEvent != null)
		{
			FlurryAndroidManager.onApplicationExitEvent(adSpace);
		}
	}

	// Token: 0x06000086 RID: 134 RVA: 0x00003640 File Offset: 0x00001840
	public void onRenderFailed(string adSpace)
	{
		if (FlurryAndroidManager.onRenderFailedEvent != null)
		{
			FlurryAndroidManager.onRenderFailedEvent(adSpace);
		}
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00003658 File Offset: 0x00001858
	public void spaceDidFailToReceiveAd(string adSpace)
	{
		if (FlurryAndroidManager.spaceDidFailToReceiveAdEvent != null)
		{
			FlurryAndroidManager.spaceDidFailToReceiveAdEvent(adSpace);
		}
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00003670 File Offset: 0x00001870
	public void spaceDidReceiveAd(string adSpace)
	{
		if (FlurryAndroidManager.spaceDidReceiveAdEvent != null)
		{
			FlurryAndroidManager.spaceDidReceiveAdEvent(adSpace);
		}
	}

	// Token: 0x06000089 RID: 137 RVA: 0x00003688 File Offset: 0x00001888
	public void onAdClicked(string adSpace)
	{
		FlurryAndroidManager.onAdClickedEvent.fire(adSpace);
	}

	// Token: 0x0600008A RID: 138 RVA: 0x00003698 File Offset: 0x00001898
	public void onAdOpened(string adSpace)
	{
		FlurryAndroidManager.onAdOpenedEvent.fire(adSpace);
	}

	// Token: 0x0600008B RID: 139 RVA: 0x000036A8 File Offset: 0x000018A8
	public void onVideoCompleted(string adSpace)
	{
		FlurryAndroidManager.onVideoCompletedEvent.fire(adSpace);
	}
}
