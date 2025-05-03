using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using LitJson;
using UnityEngine;

public static class ConfigData
{
	public interface ISchemas
	{
		JsonData _pMainSchema { get; }

		JsonData GetSubSchema(string schemaId);
	}

	public interface ITarget
	{
		void SetValue(IPath path, JsonData valueData);

		JsonData GetValue(IPath path);

		void SetArrayLength(IPath path, int length);

		int GetArrayLength(IPath path);

		void BeginRead(IPath path);

		void EndRead(IPath path);

		ITarget GetTarget(IPath path);
	}

	public interface IPath
	{
		IList<string> _pElements { get; }

		IList<int?> _pIndices { get; }

		string _pFullPath { get; }

		IPath Subpath(int startIndex, int length = -1);
	}

	private class Path : IPath
	{
		private List<string> _elements;

		private List<int?> _indices;

		private StringBuilder _fullPath;

		IList<string> IPath._pElements
		{
			get
			{
				return _elements;
			}
		}

		IList<int?> IPath._pIndices
		{
			get
			{
				return _indices;
			}
		}

		string IPath._pFullPath
		{
			get
			{
				return _pFullPath;
			}
		}

		public bool _pIsEmpty
		{
			get
			{
				return _elements == null || _elements.Count == 0;
			}
		}

		public string _pFullPath
		{
			get
			{
				return _fullPath.ToString();
			}
		}

		public Path()
		{
			_elements = new List<string>();
			_indices = new List<int?>();
			_fullPath = new StringBuilder(string.Empty);
		}

		IPath IPath.Subpath(int startIndex, int length)
		{
			Path path = new Path();
			int num = ((length >= 0) ? (startIndex + length) : _elements.Count);
			if (num > _elements.Count)
			{
				num = _elements.Count;
			}
			for (int i = startIndex; i < num; i++)
			{
				path.Add(_elements[i], _indices[i]);
			}
			return path;
		}

		private void Add(string elem, int? index)
		{
			_elements.Add(elem);
			_indices.Add(index);
			if (index.HasValue)
			{
				_fullPath.Append("[" + index.Value + "]");
			}
			else if (_fullPath.Length > 0)
			{
				_fullPath.Append("." + elem);
			}
			else
			{
				_fullPath.Append(elem);
			}
		}

		public void Add(string elem)
		{
			int result;
			if (int.TryParse(elem, out result))
			{
				Add(elem, result);
			}
			else
			{
				Add(elem, null);
			}
		}

		public void Add(int index)
		{
			Add(index.ToString(), index);
		}

		public void RemoveLast()
		{
			if (_elements.Count > 0)
			{
				_elements.RemoveAt(_elements.Count - 1);
			}
			if (_indices.Count > 0)
			{
				_indices.RemoveAt(_indices.Count - 1);
			}
			if (_fullPath.Length <= 0)
			{
				return;
			}
			int num = 1;
			if (_fullPath[_fullPath.Length - 1] == ']')
			{
				while (num < _fullPath.Length)
				{
					num++;
					if (_fullPath[_fullPath.Length - num] == '[')
					{
						break;
					}
				}
			}
			else
			{
				while (num < _fullPath.Length)
				{
					num++;
					if (_fullPath[_fullPath.Length - num] == '.')
					{
						break;
					}
				}
			}
			_fullPath.Remove(_fullPath.Length - num, num);
		}
	}

	private const string LOG_TAG = "[ConfigData] ";

	private static Dictionary<string, JsonType> _typeDict;

	static ConfigData()
	{
		_typeDict = new Dictionary<string, JsonType>();
		_typeDict.Add("number", JsonType.Double);
		_typeDict.Add("integer", JsonType.Int);
		_typeDict.Add("boolean", JsonType.Boolean);
		_typeDict.Add("string", JsonType.String);
		_typeDict.Add("array", JsonType.Array);
		_typeDict.Add("object", JsonType.Object);
	}

	private static string GetLogTag(Path path)
	{
		return "[ConfigData:" + path._pFullPath + "] ";
	}

	private static JsonType GetJsonType(string typeName)
	{
		return (_typeDict != null && typeName != null && typeName.Length > 0 && _typeDict.ContainsKey(typeName)) ? _typeDict[typeName] : JsonType.None;
	}

	public static void ReadConfig(ITarget configObject, JsonData configData)
	{
		ReadConfigData(configObject, new Path(), configData);
	}

	public static JsonData WriteConfig(ITarget configObject, ISchemas configSchemas)
	{
		return WriteConfigData(configObject, new Path(), configSchemas._pMainSchema, configSchemas);
	}

	public static void ReadFloat(JsonData valueData, Action<float> onSuccess, Action<string> onFail = null)
	{
		Action<string> action = delegate(string msg)
		{
			if (onFail != null)
			{
				onFail(msg);
			}
		};
		if (valueData == null)
		{
			action("null valueData");
		}
		else
		{
			if (onSuccess == null)
			{
				return;
			}
			if (valueData.IsDouble)
			{
				onSuccess((float)valueData);
			}
			else if (valueData.IsInt)
			{
				onSuccess((int)valueData);
			}
			else if (valueData.IsString)
			{
				float result;
				if (float.TryParse((string)valueData, out result))
				{
					onSuccess(result);
				}
				else
				{
					action("string cannot be parsed");
				}
			}
			else
			{
				action("unexpected type: " + valueData.GetJsonType());
			}
		}
	}

	public static void ReadInt(JsonData valueData, Action<int> onSuccess, Action<string> onFail = null)
	{
		Action<string> action = delegate(string msg)
		{
			if (onFail != null)
			{
				onFail(msg);
			}
		};
		if (valueData == null)
		{
			action("null valueData");
		}
		else
		{
			if (onSuccess == null)
			{
				return;
			}
			if (valueData.IsInt)
			{
				onSuccess((int)valueData);
			}
			else if (valueData.IsString)
			{
				int result;
				if (int.TryParse((string)valueData, out result))
				{
					onSuccess(result);
				}
				else
				{
					action("string cannot be parsed");
				}
			}
			else
			{
				action("unexpected type: " + valueData.GetJsonType());
			}
		}
	}

	public static void ReadBool(JsonData valueData, Action<bool> onSuccess, Action<string> onFail = null)
	{
		Action<string> action = delegate(string msg)
		{
			if (onFail != null)
			{
				onFail(msg);
			}
		};
		if (valueData == null)
		{
			action("null valueData");
		}
		else
		{
			if (onSuccess == null)
			{
				return;
			}
			if (valueData.IsBoolean)
			{
				onSuccess((bool)valueData);
			}
			else if (valueData.IsString)
			{
				bool result;
				if (bool.TryParse((string)valueData, out result))
				{
					onSuccess(result);
				}
				else
				{
					action("string cannot be parsed");
				}
			}
			else
			{
				action("unexpected type: " + valueData.GetJsonType());
			}
		}
	}

	public static void ReadString(JsonData valueData, Action<string> onSuccess, Action<string> onFail = null)
	{
		Action<string> action = delegate(string msg)
		{
			if (onFail != null)
			{
				onFail(msg);
			}
		};
		if (valueData == null)
		{
			action("null valueData");
		}
		else if (onSuccess != null)
		{
			if (valueData.IsString)
			{
				onSuccess((string)valueData);
			}
			else
			{
				action("unexpected type: " + valueData.GetJsonType());
			}
		}
	}

	public static void ReadEnum<T>(JsonData valueData, Action<T> onSuccess, Action<string> onFail = null) where T : struct, IConvertible
	{
		if (!typeof(T).IsEnum)
		{
			Debug.LogError("[ConfigData] Bad call, expected T to be enum");
			return;
		}
		Action<string> action = delegate(string msg)
		{
			if (onFail != null)
			{
				onFail(msg);
			}
		};
		if (valueData == null)
		{
			action("null valueData");
		}
		else
		{
			if (onSuccess == null)
			{
				return;
			}
			int? intVal = null;
			string enumName = null;
			if (valueData.IsInt)
			{
				intVal = (int)valueData;
			}
			else if (valueData.IsString)
			{
				enumName = (string)valueData;
				int result;
				if (int.TryParse(enumName, out result))
				{
					intVal = result;
					enumName = null;
				}
			}
			if (intVal.HasValue)
			{
				T[] array = (T[])Enum.GetValues(typeof(T));
				int num = Array.FindIndex(array, (T enumVal) => enumVal.ToInt32(CultureInfo.CurrentCulture) == intVal.Value);
				if (num < 0)
				{
					action("invalid " + typeof(T).Name + " enum value: " + intVal.Value);
				}
				else
				{
					onSuccess(array[num]);
				}
			}
			else if (enumName != null)
			{
				string[] names = Enum.GetNames(typeof(T));
				int num2 = Array.FindIndex(names, (string text) => text == enumName);
				if (num2 < 0)
				{
					action("invalid " + typeof(T).Name + " enum name: " + enumName);
					return;
				}
				T[] array2 = (T[])Enum.GetValues(typeof(T));
				onSuccess(array2[num2]);
			}
			else
			{
				action("unexpected type: " + valueData.GetJsonType());
			}
		}
	}

	private static void ReadConfigData(ITarget configObject, Path currPath, JsonData configData)
	{
		if (configObject == null || currPath == null || configData == null)
		{
			return;
		}
		configObject.BeginRead(currPath);
		if (!currPath._pIsEmpty)
		{
			ITarget target = configObject.GetTarget(currPath);
			if (target == null)
			{
				configObject.EndRead(currPath);
				return;
			}
			if (target != configObject)
			{
				ReadConfigData(target, new Path(), configData);
				configObject.EndRead(currPath);
				return;
			}
		}
		switch (configData.GetJsonType())
		{
		case JsonType.None:
			configObject.EndRead(currPath);
			return;
		case JsonType.Array:
		{
			configObject.SetArrayLength(currPath, configData.Count);
			for (int i = 0; i < configData.Count; i++)
			{
				currPath.Add(i.ToString());
				ReadConfigData(configObject, currPath, configData[i]);
				currPath.RemoveLast();
			}
			break;
		}
		case JsonType.Object:
			foreach (DictionaryEntry item in (IOrderedDictionary)configData)
			{
				currPath.Add((string)item.Key);
				ReadConfigData(configObject, currPath, (JsonData)item.Value);
				currPath.RemoveLast();
			}
			break;
		default:
			configObject.SetValue(currPath, configData);
			break;
		}
		configObject.EndRead(currPath);
	}

	private static JsonData WriteConfigData(ITarget configObject, Path currPath, JsonData configSchema, ISchemas configSchemas)
	{
		if (configObject == null || currPath == null || configSchema == null)
		{
			return null;
		}
		if (!currPath._pIsEmpty)
		{
			ITarget target = configObject.GetTarget(currPath);
			if (target == null)
			{
				return null;
			}
			if (target != configObject)
			{
				return WriteConfigData(target, new Path(), configSchema, configSchemas);
			}
		}
		if (!configSchema.IsObject)
		{
			Debug.LogWarning(GetLogTag(currPath) + "Bad schema");
			return null;
		}
		JsonData jsonData = configSchema.TryGet("$ref", JsonType.String);
		if (jsonData != null)
		{
			if (configSchemas == null)
			{
				Debug.LogWarning(GetLogTag(currPath) + "No sub-schemas");
				return null;
			}
			configSchema = configSchemas.GetSubSchema((string)jsonData);
			if (configSchema == null)
			{
				Debug.LogWarning(GetLogTag(currPath) + "Failed to find sub-schema: " + (string)jsonData);
				return null;
			}
			return WriteConfigData(configObject, currPath, configSchema, configSchemas);
		}
		JsonData jsonData2 = configSchema.TryGet("allOf", JsonType.Array);
		if (jsonData2 == null)
		{
			jsonData2 = configSchema.TryGet("anyOf", JsonType.Array);
			if (jsonData2 == null)
			{
				jsonData2 = configSchema.TryGet("oneOf", JsonType.Array);
			}
		}
		if (jsonData2 != null)
		{
			JsonData jsonData3 = null;
			int count = jsonData2.Count;
			for (int i = 0; i < count; i++)
			{
				JsonData jsonData4 = WriteConfigData(configObject, currPath, jsonData2[i], configSchemas);
				if (jsonData4 == null)
				{
					continue;
				}
				if (jsonData4.GetJsonType() == JsonType.Object)
				{
					jsonData3 = Extensions.MergeJsonObjects(jsonData3, jsonData4);
					continue;
				}
				if (jsonData3 == null)
				{
					jsonData3 = jsonData4;
					break;
				}
				Debug.LogWarning(GetLogTag(currPath) + "Can't merge data");
			}
			return jsonData3;
		}
		JsonData jsonData5 = configSchema.TryGet("type", JsonType.String);
		if (jsonData5 == null)
		{
			Debug.LogWarning(GetLogTag(currPath) + "No type in schema");
			return null;
		}
		string text = (string)jsonData5;
		JsonType jsonType = GetJsonType(text);
		if (jsonType == JsonType.None)
		{
			Debug.LogWarning(GetLogTag(currPath) + "Bad type '" + text + "' in schema");
			return null;
		}
		JsonData jsonData6 = configSchema.TryGet("default");
		if (jsonData6 != null && jsonData6.GetJsonType() != jsonType)
		{
			Debug.LogWarning(GetLogTag(currPath) + "Bad default type '" + jsonData6.GetJsonType().ToString() + "' in schema");
			return null;
		}
		JsonData jsonData7;
		switch (jsonType)
		{
		case JsonType.Array:
		{
			jsonData7 = new JsonData();
			jsonData7.SetJsonType(JsonType.Array);
			JsonData jsonData10 = configSchema.TryGet("items", JsonType.Object);
			if (jsonData10 == null)
			{
				break;
			}
			int arrayLength = configObject.GetArrayLength(currPath);
			for (int j = 0; j < arrayLength; j++)
			{
				currPath.Add(j.ToString());
				JsonData jsonData11 = WriteConfigData(configObject, currPath, jsonData10, configSchemas);
				if (jsonData11 != null)
				{
					jsonData7.Add(jsonData11);
				}
				currPath.RemoveLast();
			}
			break;
		}
		case JsonType.Object:
		{
			jsonData7 = new JsonData();
			jsonData7.SetJsonType(JsonType.Object);
			JsonData jsonData8 = configSchema.TryGet("properties", JsonType.Object);
			if (jsonData8 == null)
			{
				break;
			}
			foreach (DictionaryEntry item in (IOrderedDictionary)jsonData8)
			{
				string text2 = (string)item.Key;
				JsonData configSchema2 = (JsonData)item.Value;
				currPath.Add(text2);
				JsonData jsonData9 = WriteConfigData(configObject, currPath, configSchema2, configSchemas);
				if (jsonData9 != null)
				{
					jsonData7[text2] = jsonData9;
				}
				currPath.RemoveLast();
			}
			break;
		}
		default:
			jsonData7 = configObject.GetValue(currPath);
			break;
		}
		return jsonData7;
	}

	public static JsonData ConcatSchema(ISchemas schema)
	{
		JsonData jsonData = new JsonData();
		jsonData["schema"] = schema._pMainSchema;
		jsonData["refs"] = FindSchemaRefs(schema._pMainSchema, schema);
		return jsonData;
	}

	private static JsonData FindSchemaRefs(JsonData schema, ISchemas schemas)
	{
		JsonData jsonData = new JsonData();
		jsonData.SetJsonType(JsonType.Object);
		FindSchemaRefs(schema, schemas, jsonData);
		return jsonData;
	}

	private static void FindSchemaRefs(JsonData schema, ISchemas schemas, JsonData refs)
	{
		EnumerateJson(schema, delegate(string key, JsonData val)
		{
			if (key == "$ref" && val.GetJsonType() == JsonType.String)
			{
				string text = (string)val;
				JsonData subSchema = schemas.GetSubSchema(text);
				if (!refs.Contains(text))
				{
					refs[text] = subSchema;
					FindSchemaRefs(subSchema, schemas, refs);
				}
			}
		});
	}

	public static void EnumerateJson(JsonData data, Action<string, JsonData> onElement)
	{
		EnumerateJson(null, data, onElement);
	}

	private static void EnumerateJson(string key, JsonData data, Action<string, JsonData> onElement)
	{
		switch (data.GetJsonType())
		{
		case JsonType.None:
			break;
		case JsonType.Array:
		{
			int count = data.Count;
			for (int i = 0; i < count; i++)
			{
				EnumerateJson(i.ToString(), data[i], onElement);
			}
			break;
		}
		case JsonType.Object:
		{
			foreach (DictionaryEntry item in (IOrderedDictionary)data)
			{
				EnumerateJson((string)item.Key, (JsonData)item.Value, onElement);
			}
			break;
		}
		default:
			onElement(key, data);
			break;
		}
	}
}
