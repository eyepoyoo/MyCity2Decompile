using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PersonalLogs
{
	public static class Colin
	{
		[Conditional("DEBUG_MESSAGES")]
		public static void Log(string message, UnityEngine.Object context = null)
		{
			string prefix = "<b><color=#00FF00>[COLIN]</color></b> ";
			PersonalLogs.Log(USERNAME_COLIN, prefix, message, context);
		}
	}

	private static List<string> EXCEPTIONS = new List<string> { "Name Goes Here" };

	private static string USERNAME_BOB = "Bob";

	private static string USERNAME_KAY = "Kay";

	private static string USERNAME_COLIN = "Colin";

	private static float TIME_PER_CYCLE = 1.5f;

	private static Color[] colours = new Color[6]
	{
		Color.red,
		Color.Lerp(Color.red, Color.yellow, 0.5f),
		Color.yellow,
		Color.green,
		Color.blue,
		Color.Lerp(Color.blue, Color.red, 0.5f)
	};

	public static void FindUsername()
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		string[] array = folderPath.Split('\\');
		UnityEngine.Debug.LogError("Your username is [" + array[array.Length - 2] + "]. Determined from special folder path [" + folderPath + "]");
	}

	public static void BobLog(string message, UnityEngine.Object context = null)
	{
		float num = Mathf.Clamp01(Time.realtimeSinceStartup % TIME_PER_CYCLE / TIME_PER_CYCLE);
		float decimalProgress = Utils.Wrap(num + 0.2f, 0f, 1f);
		float decimalProgress2 = Utils.Wrap(num + 0.4f, 0f, 1f);
		float decimalProgress3 = Utils.Wrap(num + 0.6f, 0f, 1f);
		string prefix = "<b><color=#" + Utils.ColorToHex(ScreenBase.getColourFromArray(colours, num)) + ">B</color><color=#" + Utils.ColorToHex(ScreenBase.getColourFromArray(colours, decimalProgress)) + ">O</color><color=#" + Utils.ColorToHex(ScreenBase.getColourFromArray(colours, decimalProgress2)) + ">B</color><color=#" + Utils.ColorToHex(ScreenBase.getColourFromArray(colours, decimalProgress3)) + ">:</color></b> ";
		Log(USERNAME_BOB, prefix, message, context);
	}

	public static void KayLog(string message, UnityEngine.Object context = null)
	{
		string prefix = "<b><color=#FF00FF>[KAY GIBBS]</color></b> ";
		Log(USERNAME_KAY, prefix, message, context);
	}

	private static void Log(string username, string prefix, string message, UnityEngine.Object context)
	{
		if (!EXCEPTIONS.Contains(username))
		{
			if (!Application.isEditor)
			{
				return;
			}
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			string[] array = folderPath.Split('\\');
			string text = array[array.Length - 2];
			if (text != username)
			{
				return;
			}
		}
		UnityEngine.Debug.Log(prefix + message, context);
	}
}
