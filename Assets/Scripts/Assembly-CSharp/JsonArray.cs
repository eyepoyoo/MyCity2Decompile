using System.Collections.Generic;
using UnityEngine;

public class JsonArray : JsonObject
{
	private List<JsonObject> entries;

	public JsonObject this[int index]
	{
		get
		{
			IEnumerator<JsonObject> enumerator = entries.GetEnumerator();
			while (index >= 0)
			{
				if (!enumerator.MoveNext())
				{
					return null;
				}
				index--;
			}
			return enumerator.Current;
		}
	}

	public int Length
	{
		get
		{
			return entries.Count;
		}
	}

	public JsonArray()
	{
		entries = new List<JsonObject>();
		type = JSON_TYPES.JSON_ARRAY;
	}

	public void AddObject(JsonObject obj)
	{
		entries.Add(obj);
	}

	public override void Print()
	{
		IEnumerator<JsonObject> enumerator = entries.GetEnumerator();
		int num = 0;
		while (enumerator.MoveNext())
		{
			Debug.Log("Object at index " + num + " has the following content :");
			enumerator.Current.Print();
			num++;
		}
	}
}
