using UnityEngine;

public class JsonValue : JsonObject
{
	private string rawContent;

	public JsonValue(string content)
	{
		type = JSON_TYPES.JSON_VALUE;
		rawContent = content;
	}

	public string GetRaw()
	{
		return rawContent;
	}

	public bool GetAsInt(out int val)
	{
		return int.TryParse(rawContent, out val);
	}

	public bool GetAsFloat(out float val)
	{
		return float.TryParse(rawContent, out val);
	}

	public bool GetAsDouble(out double val)
	{
		return double.TryParse(rawContent, out val);
	}

	public bool GetAsBool(out bool val)
	{
		return bool.TryParse(rawContent, out val);
	}

	public override void Print()
	{
		Debug.Log(rawContent);
	}
}
