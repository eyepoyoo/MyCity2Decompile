using UnityEngine;

public class JSonParser
{
	public static JsonObject Parse(string context)
	{
		JsonObject obj = new JsonObject();
		ParseObject(ref obj, ref context);
		return obj;
	}

	private static bool ParseObject(ref JsonObject obj, ref string context)
	{
		int num = context.IndexOf("{");
		if (num != -1)
		{
			context = context.Substring(num + 1).Trim();
			while (context.IndexOf("\"") == 0)
			{
				string fieldName;
				if (GetFieldName(ref context, out fieldName))
				{
					num = context.IndexOf(":");
					if (num != -1)
					{
						JsonObject jsonObject = FormObjectFromContent(ref context);
						if (jsonObject != null)
						{
							obj[fieldName] = jsonObject;
						}
						context = context.Trim();
						if (context.IndexOf("}") == 0)
						{
							context = context.Substring(1);
							return true;
						}
						if (context.IndexOf(",") == 0)
						{
							context = context.Substring(1).Trim();
							continue;
						}
						Debug.LogWarning("Incorrect separator token : expected either } ");
						return false;
					}
					Debug.LogWarning("Cannot find object delimiter \":\" .Json string so far:" + context);
					return false;
				}
				Debug.LogWarning("Cannot obtain field name. String so far : " + context);
				return false;
			}
			return true;
		}
		Debug.Log("Cannot locate start of jason object. JSon string left to parse :" + context);
		return false;
	}

	private static bool GetFieldName(ref string context, out string fieldName)
	{
		fieldName = null;
		context = context.Substring(1);
		int num = context.IndexOf("\"");
		if (num != -1)
		{
			fieldName = context.Substring(0, num);
			context = context.Substring(num + 1).Trim();
			return true;
		}
		Debug.Log("Cannot find end of field name. JSon string left to parse:" + context);
		return false;
	}

	private static JsonObject FormObjectFromContent(ref string context)
	{
		int num = -1;
		context = context.Substring(1).Trim();
		switch (context.ToCharArray()[0])
		{
		case '[':
		{
			JsonArray array = new JsonArray();
			if (ParseArray(ref context, ref array))
			{
				return array;
			}
			return null;
		}
		case '{':
		{
			JsonObject obj = new JsonObject();
			if (ParseObject(ref obj, ref context))
			{
				return obj;
			}
			return null;
		}
		default:
		{
			num = GetEndTokenIndex(context);
			string text = context.Substring(0, num).Trim();
			context = context.Substring(num).Trim();
			if (text.IndexOf("\"") == 0)
			{
				text = text.Substring(1);
			}
			if (text.LastIndexOf("\"") == text.Length - 1)
			{
				text = text.Substring(0, text.Length - 1);
			}
			return new JsonValue(text);
		}
		}
	}

	private static bool ParseArray(ref string context, ref JsonArray array)
	{
		if (ArrayEmpty(context.Substring(1).Trim()))
		{
			context = context.Substring(2).Trim();
			return true;
		}
		while (context.IndexOf("]") != 0)
		{
			JsonObject jsonObject = FormObjectFromContent(ref context);
			if (jsonObject != null)
			{
				array.AddObject(jsonObject);
				continue;
			}
			array = null;
			return false;
		}
		context = context.Substring(1).Trim();
		return true;
	}

	private static bool ArrayEmpty(string array)
	{
		return array.IndexOf("]") == 0;
	}

	private static int GetEndTokenIndex(string context)
	{
		int num = -1;
		int num2 = -1;
		num2 = context.IndexOf(",");
		num = context.IndexOf("}");
		if (num > 0 && num2 > 0)
		{
			return Mathf.Min(num, num2);
		}
		return Mathf.Max(num, num2);
	}
}
