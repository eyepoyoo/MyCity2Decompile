using System;
using System.Collections.Generic;

// Token: 0x02000051 RID: 81
public static class JSONObjectExtension
{
	// Token: 0x060000E2 RID: 226 RVA: 0x00005690 File Offset: 0x00003890
	public static string StringForKey(this JSONObject obj, string key, bool repairString = true)
	{
		JSONObject jsonobject = obj;
		string[] array = key.Split(new char[] { '.' });
		for (int i = 0; i < array.Length - 1; i++)
		{
			string text = array[i];
			jsonobject = jsonobject.GetField(text);
			if (jsonobject == null)
			{
				return null;
			}
		}
		string text2 = null;
		jsonobject.GetField(ref text2, array[array.Length - 1], null);
		if (!string.IsNullOrEmpty(text2) && repairString)
		{
			text2 = text2.Replace("\\\"", "\"");
			text2 = text2.Replace("\\n", "\n");
		}
		return text2;
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x0000572C File Offset: 0x0000392C
	public static Dictionary<string, string> DictionaryForKey(this JSONObject obj, string key)
	{
		JSONObject jsonobject = obj;
		string[] array = key.Split(new char[] { '.' });
		for (int i = 0; i < array.Length - 1; i++)
		{
			string text = array[i];
			jsonobject = jsonobject.GetField(text);
			if (jsonobject == null)
			{
				return null;
			}
		}
		JSONObject field = jsonobject.GetField(array[array.Length - 1]);
		if (field == null || field.type == JSONObject.Type.NULL)
		{
			return null;
		}
		return field.ToDictionary();
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x000057A4 File Offset: 0x000039A4
	public static Version VersionForKey(this JSONObject obj, string key)
	{
		string text = obj.StringForKey(key, true);
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		Version version;
		try
		{
			if (text.Split(new char[] { '.' }).Length == 1)
			{
				text += ".0.0";
			}
			else if (text.Split(new char[] { '.' }).Length == 2)
			{
				text += ".0";
			}
			version = new Version(text);
		}
		catch (Exception ex)
		{
			throw ex;
		}
		return version;
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x0000584C File Offset: 0x00003A4C
	private static JSONObject fieldOrNull(this JSONObject obj, string key)
	{
		JSONObject field = obj.GetField(key);
		if (field == null || field.type == JSONObject.Type.NULL)
		{
			return null;
		}
		return field;
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x00005878 File Offset: 0x00003A78
	public static JSONObject ObjectForKey(this JSONObject obj, string key)
	{
		JSONObject jsonobject = obj;
		foreach (string text in key.Split(new char[] { '.' }))
		{
			jsonobject = jsonobject.GetField(text);
			if (jsonobject == null || jsonobject.type == JSONObject.Type.NULL)
			{
				return null;
			}
		}
		return jsonobject;
	}
}
