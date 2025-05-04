using System;
using System.Collections.Generic;
using Prime31;
using UnityEngine;

// Token: 0x0200000C RID: 12
public class FlurryAndroid
{
	// Token: 0x06000056 RID: 86 RVA: 0x00002EC8 File Offset: 0x000010C8
	static FlurryAndroid()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FlurryAndroid._flurryAgent = new AndroidJavaClass("com.flurry.android.FlurryAgent");
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.prime31.FlurryPlugin"))
		{
			FlurryAndroid._plugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
		}
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00002F40 File Offset: 0x00001140
	public static string getAndroidId()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return string.Empty;
		}
		return FlurryAndroid._plugin.Call<string>("getAndroidId", new object[0]);
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00002F6C File Offset: 0x0000116C
	public static void onStartSession(string apiKey, bool initializeAds, bool enableTestAdsAndLogging)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FlurryAndroid._plugin.Call("onStartSession", new object[] { apiKey, initializeAds, enableTestAdsAndLogging });
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00002FB4 File Offset: 0x000011B4
	public static void onEndSession()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FlurryAndroid._plugin.Call("onEndSession", new object[0]);
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00002FE4 File Offset: 0x000011E4
	public static void addUserCookie(string key, string value)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FlurryAndroid._plugin.Call("addUserCookie", new object[] { key, value });
	}

	// Token: 0x0600005B RID: 91 RVA: 0x0000301C File Offset: 0x0000121C
	public static void clearUserCookies()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FlurryAndroid._plugin.Call("clearUserCookies", new object[0]);
	}

	// Token: 0x0600005C RID: 92 RVA: 0x0000304C File Offset: 0x0000124C
	public static void setContinueSessionMillis(long milliseconds)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FlurryAndroid._flurryAgent.CallStatic("setContinueSessionMillis", new object[] { milliseconds });
	}

	// Token: 0x0600005D RID: 93 RVA: 0x0000307C File Offset: 0x0000127C
	public static void logEvent(string eventName)
	{
		FlurryAndroid.logEvent(eventName, false);
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00003088 File Offset: 0x00001288
	public static void logEvent(string eventName, bool isTimed)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (isTimed)
		{
			FlurryAndroid._plugin.Call("logTimedEvent", new object[] { eventName });
		}
		else
		{
			FlurryAndroid._plugin.Call("logEvent", new object[] { eventName });
		}
	}

	// Token: 0x0600005F RID: 95 RVA: 0x000030E0 File Offset: 0x000012E0
	public static void logEvent(string eventName, Dictionary<string, string> parameters)
	{
		FlurryAndroid.logEvent(eventName, parameters, false);
	}

	// Token: 0x06000060 RID: 96 RVA: 0x000030EC File Offset: 0x000012EC
	public static void logEvent(string eventName, Dictionary<string, string> parameters, bool isTimed)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (parameters == null)
		{
			Debug.LogError("attempting to call logEvent with null parameters");
			return;
		}
		if (isTimed)
		{
			FlurryAndroid._plugin.Call("logTimedEventWithParams", new object[]
			{
				eventName,
				parameters.toJson()
			});
		}
		else
		{
			FlurryAndroid._plugin.Call("logEventWithParams", new object[]
			{
				eventName,
				parameters.toJson()
			});
		}
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00003168 File Offset: 0x00001368
	public static void endTimedEvent(string eventName)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FlurryAndroid._plugin.Call("endTimedEvent", new object[] { eventName });
	}

	// Token: 0x06000062 RID: 98 RVA: 0x0000319C File Offset: 0x0000139C
	public static void endTimedEvent(string eventName, Dictionary<string, string> parameters)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (parameters == null)
		{
			Debug.LogError("attempting to call endTimedEvent with null parameters");
			return;
		}
		FlurryAndroid._plugin.Call("endTimedEventWithParams", new object[]
		{
			eventName,
			parameters.toJson()
		});
	}

	// Token: 0x06000063 RID: 99 RVA: 0x000031EC File Offset: 0x000013EC
	public static void onPageView()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FlurryAndroid._flurryAgent.CallStatic("onPageView", new object[0]);
	}

	// Token: 0x06000064 RID: 100 RVA: 0x0000321C File Offset: 0x0000141C
	public static void onError(string errorId, string message, string errorClass)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FlurryAndroid._flurryAgent.CallStatic("onError", new object[] { errorId, message, errorClass });
	}

	// Token: 0x06000065 RID: 101 RVA: 0x00003258 File Offset: 0x00001458
	public static void setUserID(string userId)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FlurryAndroid._flurryAgent.CallStatic("setUserId", new object[] { userId });
	}

	// Token: 0x06000066 RID: 102 RVA: 0x0000328C File Offset: 0x0000148C
	public static void setAge(int age)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FlurryAndroid._flurryAgent.CallStatic("setAge", new object[] { age });
	}

	// Token: 0x06000067 RID: 103 RVA: 0x000032BC File Offset: 0x000014BC
	public static void setLogEnabled(bool enable)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FlurryAndroid._flurryAgent.CallStatic("setLogEnabled", new object[] { enable });
	}

	// Token: 0x06000068 RID: 104 RVA: 0x000032EC File Offset: 0x000014EC
	public static void fetchAdsForSpace(string adSpace, FlurryAdPlacement adSize)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FlurryAndroid._plugin.Call("fetchAdsForSpace", new object[]
		{
			adSpace,
			(int)adSize
		});
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00003320 File Offset: 0x00001520
	public static void displayAd(string adSpace, FlurryAdPlacement adSize, long timeout)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FlurryAndroid._plugin.Call("displayAd", new object[]
		{
			adSpace,
			(int)adSize,
			timeout
		});
	}

	// Token: 0x0600006A RID: 106 RVA: 0x00003368 File Offset: 0x00001568
	public static void removeAd(string adSpace)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FlurryAndroid._plugin.Call("removeAd", new object[] { adSpace });
	}

	// Token: 0x0600006B RID: 107 RVA: 0x0000339C File Offset: 0x0000159C
	public static void checkIfAdIsAvailable(string adSpace, FlurryAdPlacement adSize, long timeout)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		FlurryAndroid._plugin.Call("isAdAvailable", new object[]
		{
			adSpace,
			(int)adSize,
			timeout
		});
	}

	// Token: 0x0400001B RID: 27
	private static AndroidJavaClass _flurryAgent;

	// Token: 0x0400001C RID: 28
	private static AndroidJavaObject _plugin;
}
