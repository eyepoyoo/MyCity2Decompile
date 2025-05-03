using System;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class LeanTest
{
	// Token: 0x060000A2 RID: 162 RVA: 0x00003CD0 File Offset: 0x00001ED0
	public static void debug(string name, bool didPass, string failExplaination = null)
	{
		LeanTest.expect(didPass, name, failExplaination);
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00003CDC File Offset: 0x00001EDC
	public static void expect(bool didPass, string definition, string failExplaination = null)
	{
		float num = LeanTest.printOutLength(definition);
		int num2 = 40 - (int)(num * 1.05f);
		string text = string.Empty.PadRight(num2, "_"[0]);
		string text2 = string.Concat(new string[]
		{
			LeanTest.formatB(definition),
			" ",
			text,
			" [ ",
			(!didPass) ? LeanTest.formatC("fail", "red") : LeanTest.formatC("pass", "green"),
			" ]"
		});
		if (!didPass && failExplaination != null)
		{
			text2 = text2 + " - " + failExplaination;
		}
		Debug.Log(text2);
		if (didPass)
		{
			LeanTest.passes++;
		}
		LeanTest.tests++;
		if (LeanTest.tests == LeanTest.expected && !LeanTest.testsFinished)
		{
			LeanTest.overview();
		}
		else if (LeanTest.tests > LeanTest.expected)
		{
			Debug.Log(LeanTest.formatB("Too many tests for a final report!") + " set LeanTest.expected = " + LeanTest.tests);
		}
		if (!LeanTest.timeoutStarted)
		{
			LeanTest.timeoutStarted = true;
			GameObject gameObject = new GameObject();
			gameObject.name = "~LeanTest";
			LeanTester leanTester = gameObject.AddComponent(typeof(LeanTester)) as LeanTester;
			leanTester.timeout = LeanTest.timeout;
			gameObject.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x00003E54 File Offset: 0x00002054
	public static string padRight(int len)
	{
		string text = string.Empty;
		for (int i = 0; i < len; i++)
		{
			text += "_";
		}
		return text;
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x00003E88 File Offset: 0x00002088
	public static float printOutLength(string str)
	{
		float num = 0f;
		for (int i = 0; i < str.Length; i++)
		{
			if (str[i] == "I"[0])
			{
				num += 0.5f;
			}
			else if (str[i] == "J"[0])
			{
				num += 0.85f;
			}
			else
			{
				num += 1f;
			}
		}
		return num;
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00003F04 File Offset: 0x00002104
	public static string formatBC(string str, string color)
	{
		return LeanTest.formatC(LeanTest.formatB(str), color);
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00003F14 File Offset: 0x00002114
	public static string formatB(string str)
	{
		return "<b>" + str + "</b>";
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00003F28 File Offset: 0x00002128
	public static string formatC(string str, string color)
	{
		return string.Concat(new string[] { "<color=", color, ">", str, "</color>" });
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00003F58 File Offset: 0x00002158
	public static void overview()
	{
		LeanTest.testsFinished = true;
		int num = LeanTest.expected - LeanTest.passes;
		string text = ((num <= 0) ? (string.Empty + num) : LeanTest.formatBC(string.Empty + num, "red"));
		Debug.Log(string.Concat(new string[]
		{
			LeanTest.formatB("Final Report:"),
			" _____________________ PASSED: ",
			LeanTest.formatBC(string.Empty + LeanTest.passes, "green"),
			" FAILED: ",
			text,
			" "
		}));
	}

	// Token: 0x04000034 RID: 52
	public static int expected;

	// Token: 0x04000035 RID: 53
	private static int tests;

	// Token: 0x04000036 RID: 54
	private static int passes;

	// Token: 0x04000037 RID: 55
	public static float timeout = 15f;

	// Token: 0x04000038 RID: 56
	public static bool timeoutStarted;

	// Token: 0x04000039 RID: 57
	public static bool testsFinished;
}
