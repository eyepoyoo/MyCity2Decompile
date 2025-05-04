using System;
using System.IO;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class GooglePlayDownloader
{
	// Token: 0x06000002 RID: 2 RVA: 0x000020F4 File Offset: 0x000002F4
	static GooglePlayDownloader()
	{
		if (!GooglePlayDownloader.RunningOnAndroid())
		{
			return;
		}
		GooglePlayDownloader.Environment = new AndroidJavaClass("android.os.Environment");
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.plugin.downloader.UnityDownloaderService"))
		{
			androidJavaClass.SetStatic<string>("BASE64_PUBLIC_KEY", GooglePlayDownloader.PUBLIC_KEY_DEFAULT);
			androidJavaClass.SetStatic<byte[]>("SALT", new byte[]
			{
				1, 43, 244, byte.MaxValue, 54, 98, 156, 244, 43, 2,
				248, 252, 9, 5, 150, 148, 223, 45, byte.MaxValue, 84
			});
		}
	}

	// Token: 0x06000003 RID: 3 RVA: 0x0000218C File Offset: 0x0000038C
	public static bool RunningOnAndroid()
	{
		if (GooglePlayDownloader.detectAndroidJNI == null)
		{
			GooglePlayDownloader.detectAndroidJNI = new AndroidJavaClass("android.os.Build");
		}
		return GooglePlayDownloader.detectAndroidJNI.GetRawClass() != IntPtr.Zero;
	}

	// Token: 0x06000004 RID: 4 RVA: 0x000021BC File Offset: 0x000003BC
	public static string GetExpansionFilePath()
	{
		GooglePlayDownloader.populateOBBData();
		if (GooglePlayDownloader.Environment.CallStatic<string>("getExternalStorageState", new object[0]) != "mounted")
		{
			return null;
		}
		string text2;
		using (AndroidJavaObject androidJavaObject = GooglePlayDownloader.Environment.CallStatic<AndroidJavaObject>("getExternalStorageDirectory", new object[0]))
		{
			string text = androidJavaObject.Call<string>("getPath", new object[0]);
			text2 = string.Format("{0}/{1}/{2}", text, "Android/obb", GooglePlayDownloader.obb_package);
		}
		return text2;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002268 File Offset: 0x00000468
	public static string GetMainOBBPath(string expansionFilePath)
	{
		GooglePlayDownloader.populateOBBData();
		if (expansionFilePath == null)
		{
			return null;
		}
		string text = string.Format("{0}/main.{1}.{2}.obb", expansionFilePath, GooglePlayDownloader.obb_version, GooglePlayDownloader.obb_package);
		if (!File.Exists(text))
		{
			return null;
		}
		return text;
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000022AC File Offset: 0x000004AC
	public static string GetPatchOBBPath(string expansionFilePath)
	{
		GooglePlayDownloader.populateOBBData();
		if (expansionFilePath == null)
		{
			return null;
		}
		string text = string.Format("{0}/patch.{1}.{2}.obb", expansionFilePath, GooglePlayDownloader.obb_version, GooglePlayDownloader.obb_package);
		if (!File.Exists(text))
		{
			return null;
		}
		return text;
	}

	// Token: 0x06000007 RID: 7 RVA: 0x000022F0 File Offset: 0x000004F0
	public static void FetchOBB()
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.content.Intent", new object[]
			{
				@static,
				new AndroidJavaClass("com.unity3d.plugin.downloader.UnityDownloaderActivity")
			});
			int num = 65536;
			androidJavaObject.Call<AndroidJavaObject>("addFlags", new object[] { num });
			androidJavaObject.Call<AndroidJavaObject>("putExtra", new object[]
			{
				"unityplayer.Activity",
				@static.Call<AndroidJavaObject>("getClass", new object[0]).Call<string>("getName", new object[0])
			});
			@static.Call("startActivity", new object[] { androidJavaObject });
			if (AndroidJNI.ExceptionOccurred() != IntPtr.Zero)
			{
				Debug.LogError("Exception occurred while attempting to start DownloaderActivity - is the AndroidManifest.xml incorrect?");
				AndroidJNI.ExceptionDescribe();
				AndroidJNI.ExceptionClear();
			}
		}
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00002400 File Offset: 0x00000600
	private static void populateOBBData()
	{
		if (GooglePlayDownloader.obb_version != 0)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			GooglePlayDownloader.obb_package = @static.Call<string>("getPackageName", new object[0]);
			AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getPackageManager", new object[0]).Call<AndroidJavaObject>("getPackageInfo", new object[]
			{
				GooglePlayDownloader.obb_package,
				0
			});
			GooglePlayDownloader.obb_version = androidJavaObject.Get<int>("versionCode");
		}
	}

	// Token: 0x04000001 RID: 1
	private const string Environment_MEDIA_MOUNTED = "mounted";

	// Token: 0x04000002 RID: 2
	private static string PUBLIC_KEY_DEFAULT = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAqSZ+Ihi58nsFBjnF036/jI3PwFGUApi9BZUPNLJaOe4ErjmXBrQfhZV3621drRBVpbWBXYkwA4KVlGtaNd0hRF/dRs80rciiky0nE5IsJwBU8o9xVb/11Iw/ACQgZiG/Q0IxsnrNcqn62K7wqItDVklxv+4PdjtpcavSFAQC20CxTkX+GM0ZiyvWKo8nb6+m0zNmC/7UOAMW4vqXfR72+X0is0qG3d9mUTe0FgVFyipSOB5bQFFw968syoKY1JztMBW3a0+fB7lk6l7FP9Aemae1YHY8HNofaAkUKoXrewjz1ZJm5QlayK7I3BMLlhCXKKKG+U5xoWbUAhFotuT+vQIDAQAB";

	// Token: 0x04000003 RID: 3
	private static AndroidJavaClass detectAndroidJNI;

	// Token: 0x04000004 RID: 4
	private static AndroidJavaClass Environment;

	// Token: 0x04000005 RID: 5
	private static string obb_package;

	// Token: 0x04000006 RID: 6
	private static int obb_version;
}
