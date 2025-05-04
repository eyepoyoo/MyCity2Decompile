using System;
using LitJson;
using UnityEngine;

// Token: 0x02000025 RID: 37
public class JsonMapperLite
{
	// Token: 0x06000262 RID: 610 RVA: 0x0000E908 File Offset: 0x0000CB08
	public static JsonData ToObject(JsonReader reader)
	{
		JsonData jsonData = null;
		JsonMapperLite._loadedAnything = false;
		while (reader.Read())
		{
			if (reader.Token == JsonToken.ObjectStart || reader.Token == JsonToken.ArrayStart)
			{
				if (JsonMapperLite.DO_DEBUG)
				{
					Debug.Log("JsonMapperLite: Json checked and started with a " + ((reader.Token != JsonToken.ObjectStart) ? "Array" : "Object") + ".");
				}
				if (reader.Token == JsonToken.ArrayStart)
				{
					JsonMapperLite.parseArray(reader, ref jsonData);
				}
				else
				{
					jsonData = JsonMapperLite.ParseObject(reader, true);
				}
			}
			else if (reader.Token != JsonToken.ObjectEnd)
			{
				Debug.LogError("Unexpected Token when reading JSONData");
				return null;
			}
		}
		if (!JsonMapperLite._loadedAnything)
		{
			return null;
		}
		return jsonData;
	}

	// Token: 0x06000263 RID: 611 RVA: 0x0000E9CC File Offset: 0x0000CBCC
	private static JsonData ParseObject(JsonReader reader, bool isRoot = false)
	{
		string text = null;
		JsonData jsonData = new JsonData();
		while (reader.Read())
		{
			if (reader.Token == JsonToken.PropertyName)
			{
				text = (string)reader.Value;
				if (JsonMapperLite.DO_DEBUG)
				{
					Debug.Log("JsonMapperLite: Property name found [" + text + "]");
				}
			}
			else
			{
				if (reader.Token == JsonToken.ObjectEnd)
				{
					if (JsonMapperLite.DO_DEBUG)
					{
						Debug.Log("JsonMapperLite: Object Ended");
					}
					return jsonData;
				}
				if (reader.Token == JsonToken.ObjectStart)
				{
					if (JsonMapperLite.DO_DEBUG)
					{
						Debug.Log("JsonMapperLite: Object Started");
					}
					JsonData jsonData2 = JsonMapperLite.ParseObject(reader, false);
					jsonData[text] = jsonData2;
					JsonMapperLite._loadedAnything = true;
				}
				else if (reader.Token == JsonToken.String)
				{
					jsonData[text] = (string)reader.Value;
					if (JsonMapperLite.DO_DEBUG)
					{
						Debug.Log("JsonMapperLite: String found [" + (string)reader.Value + "]");
					}
					JsonMapperLite._loadedAnything = true;
				}
				else if (reader.Token == JsonToken.Boolean)
				{
					jsonData[text] = (bool)reader.Value;
					if (JsonMapperLite.DO_DEBUG)
					{
						Debug.Log("JsonMapperLite: Boolean found [" + (bool)reader.Value + "]");
					}
					JsonMapperLite._loadedAnything = true;
				}
				else if (reader.Token == JsonToken.Int)
				{
					jsonData[text] = (int)reader.Value;
					if (JsonMapperLite.DO_DEBUG)
					{
						Debug.Log("JsonMapperLite: Int found [" + (int)reader.Value + "]");
					}
					JsonMapperLite._loadedAnything = true;
				}
				else if (reader.Token == JsonToken.Long)
				{
					jsonData[text] = (long)reader.Value;
					if (JsonMapperLite.DO_DEBUG)
					{
						Debug.Log("JsonMapperLite: Long found [" + (long)reader.Value + "]");
					}
					JsonMapperLite._loadedAnything = true;
				}
				else if (reader.Token == JsonToken.Double)
				{
					jsonData[text] = (double)reader.Value;
					if (JsonMapperLite.DO_DEBUG)
					{
						Debug.Log("JsonMapperLite: Double found [" + (double)reader.Value + "]");
					}
					JsonMapperLite._loadedAnything = true;
				}
				else if (reader.Token == JsonToken.ArrayStart)
				{
					JsonData jsonData3 = new JsonData();
					JsonMapperLite._loadedAnything = true;
					JsonMapperLite.parseArray(reader, ref jsonData3);
					jsonData[text] = jsonData3;
				}
				else if (reader.Token == JsonToken.Null)
				{
					jsonData[text] = "Null";
					if (JsonMapperLite.DO_DEBUG)
					{
						Debug.Log("JsonMapperLite: String found [Null]");
					}
					JsonMapperLite._loadedAnything = true;
				}
				else
				{
					Debug.LogWarning("Unhandled Token: " + reader.Token);
				}
			}
		}
		return jsonData;
	}

	// Token: 0x06000264 RID: 612 RVA: 0x0000ECD8 File Offset: 0x0000CED8
	private static void parseArray(JsonReader reader, ref JsonData arrayRoot)
	{
		if (arrayRoot == null)
		{
			arrayRoot = new JsonData();
		}
		arrayRoot.Add(new JsonData(false));
		arrayRoot.Clear();
		if (JsonMapperLite.DO_DEBUG)
		{
			Debug.Log("JsonMapperLite: Array Started");
		}
		while (reader.Read())
		{
			if (reader.Token == JsonToken.ArrayEnd)
			{
				if (JsonMapperLite.DO_DEBUG)
				{
					Debug.Log("JsonMapperLite: Array Ended");
				}
				break;
			}
			if (reader.Token == JsonToken.ObjectStart)
			{
				if (JsonMapperLite.DO_DEBUG)
				{
					Debug.Log("JsonMapperLite: Array Object Started");
				}
				JsonData jsonData = JsonMapperLite.ParseObject(reader, false);
				arrayRoot.Add(jsonData);
			}
			else if (reader.Token == JsonToken.String)
			{
				if (JsonMapperLite.DO_DEBUG)
				{
					Debug.Log("JsonMapperLite: Array string found [" + (string)reader.Value + "]");
				}
				arrayRoot.Add(new JsonData((string)reader.Value));
			}
			else if (reader.Token == JsonToken.Boolean)
			{
				if (JsonMapperLite.DO_DEBUG)
				{
					Debug.Log("JsonMapperLite: Array bool found [" + (bool)reader.Value + "]");
				}
				arrayRoot.Add(new JsonData((bool)reader.Value));
			}
			else if (reader.Token == JsonToken.Int)
			{
				if (JsonMapperLite.DO_DEBUG)
				{
					Debug.Log("JsonMapperLite: Array int found [" + (int)reader.Value + "]");
				}
				arrayRoot.Add(new JsonData((int)reader.Value));
			}
			else if (reader.Token == JsonToken.Double)
			{
				if (JsonMapperLite.DO_DEBUG)
				{
					Debug.Log("JsonMapperLite: Array double found [" + (double)reader.Value + "]");
				}
				arrayRoot.Add(new JsonData((double)reader.Value));
			}
			else if (reader.Token == JsonToken.Long)
			{
				if (JsonMapperLite.DO_DEBUG)
				{
					Debug.Log("JsonMapperLite: Array long found [" + (long)reader.Value + "]");
				}
				arrayRoot.Add(new JsonData((long)reader.Value));
			}
			else if (reader.Token == JsonToken.ArrayStart)
			{
				JsonData jsonData2 = new JsonData();
				JsonMapperLite.parseArray(reader, ref jsonData2);
				arrayRoot.Add(jsonData2);
			}
		}
	}

	// Token: 0x0400013E RID: 318
	private static bool DO_DEBUG;

	// Token: 0x0400013F RID: 319
	private static bool _loadedAnything;
}
