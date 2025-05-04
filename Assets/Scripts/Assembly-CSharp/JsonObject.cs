using System.Collections.Generic;
using UnityEngine;

public class JsonObject
{
	public enum JSON_TYPES
	{
		JSON_OBJECT = 0,
		JSON_VALUE = 1,
		JSON_ARRAY = 2,
		TYPES = 3
	}

	private Dictionary<string, JsonObject> fields;

	private IEnumerator<KeyValuePair<string, JsonObject>> enumerator;

	protected JSON_TYPES type;

	public JsonObject this[string key]
	{
		get
		{
			return fields[key];
		}
		set
		{
			fields[key] = value;
		}
	}

	public KeyValuePair<string, JsonObject> Current
	{
		get
		{
			return enumerator.Current;
		}
	}

	public JsonObject()
	{
		fields = new Dictionary<string, JsonObject>();
		type = JSON_TYPES.JSON_OBJECT;
	}

	public JSON_TYPES GetJsonType()
	{
		return type;
	}

	public void StartEnumeration()
	{
		enumerator = fields.GetEnumerator();
	}

	public bool MoveNext()
	{
		return enumerator.MoveNext();
	}

	public virtual void Print()
	{
		foreach (KeyValuePair<string, JsonObject> field in fields)
		{
			Debug.Log("Object name : " + field.Key + " with fields : ");
			field.Value.Print();
		}
	}
}
