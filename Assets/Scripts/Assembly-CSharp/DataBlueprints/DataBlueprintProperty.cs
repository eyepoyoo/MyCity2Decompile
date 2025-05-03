using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace DataBlueprints
{
	[Serializable]
	public class DataBlueprintProperty
	{
		public enum ValueType
		{
			String = 3,
			Int = 4,
			Float = 6,
			Bool = 7,
			Prefab = 8,
			Texture = 9,
			AudioClip = 10
		}

		private const string KEY_NAME = "name";

		private const string KEY_DESCRIPTION = "description";

		private const string KEY_MEMBER_NAME = "objectMemberName";

		private const string KEY_VALUE = "value";

		private const string KEY_TYPE = "type";

		private Dictionary<ValueType, Type> _valueTypeToSystemType = new Dictionary<ValueType, Type>
		{
			{
				ValueType.String,
				typeof(string)
			},
			{
				ValueType.Int,
				typeof(int)
			},
			{
				ValueType.Float,
				typeof(float)
			},
			{
				ValueType.Bool,
				typeof(bool)
			},
			{
				ValueType.Prefab,
				typeof(GameObject)
			},
			{
				ValueType.Texture,
				typeof(Texture2D)
			},
			{
				ValueType.AudioClip,
				typeof(AudioClip)
			}
		};

		public string _name;

		public string _description;

		public string _objectMemberName;

		public ValueType _valueType = ValueType.String;

		private string _valueString = string.Empty;

		private float _valueFloat;

		private int _valueInt;

		private bool _valueBool;

		private GameObject _valuePrefab;

		private Texture2D _valueTexture;

		private AudioClip _valueAudioClip;

		public Type _pSystemType
		{
			get
			{
				return _valueTypeToSystemType[_valueType];
			}
		}

		public object _pValue
		{
			get
			{
				switch (_valueType)
				{
				case ValueType.Bool:
					return _valueBool;
				case ValueType.Float:
					return _valueFloat;
				case ValueType.Int:
					return _valueInt;
				case ValueType.String:
					return _valueString;
				case ValueType.Prefab:
					return _valuePrefab;
				case ValueType.Texture:
					return _valueTexture;
				case ValueType.AudioClip:
					return _valueAudioClip;
				default:
					Debug.LogError(string.Format("Unhandled ValueType \"{0}\"", _valueType));
					return null;
				}
			}
			set
			{
				if (value == null)
				{
					switch (_valueType)
					{
					case ValueType.Prefab:
						_valuePrefab = null;
						break;
					case ValueType.Texture:
						_valueTexture = null;
						break;
					case ValueType.AudioClip:
						_valueAudioClip = null;
						break;
					}
				}
				else if (value is string)
				{
					_valueType = ValueType.String;
					_valueString = (string)value;
				}
				else if (value is float)
				{
					_valueType = ValueType.Float;
					_valueFloat = (float)value;
				}
				else if (value is int)
				{
					_valueType = ValueType.Int;
					_valueInt = (int)value;
				}
				else if (value is bool)
				{
					_valueType = ValueType.Bool;
					_valueBool = (bool)value;
				}
				else if (value is GameObject)
				{
					_valueType = ValueType.Prefab;
					_valuePrefab = (GameObject)value;
				}
				else if (value is Texture2D)
				{
					_valueType = ValueType.Texture;
					_valueTexture = (Texture2D)value;
				}
				else
				{
					if (!(value is AudioClip))
					{
						throw new InvalidCastException(string.Format("Tried to set value to unsupported type \"{0}\"", value.GetType()));
					}
					_valueType = ValueType.AudioClip;
					_valueAudioClip = (AudioClip)value;
				}
			}
		}

		public DataBlueprintProperty(JsonData source)
			: this((string)source["name"], (string)source["description"], (string)source["objectMemberName"], source["value"].GetValue(true), (ValueType)(int)Enum.Parse(typeof(ValueType), (string)source["type"]))
		{
		}

		public DataBlueprintProperty(string name, string description, string objectMemberName = "", object value = null, ValueType type = ValueType.String)
		{
			_name = name;
			_description = description;
			_objectMemberName = objectMemberName;
			_valueType = type;
			if (_pSystemType.Is(typeof(UnityEngine.Object)))
			{
				value = Resources.Load((string)value);
			}
			_pValue = value;
		}

		public DataBlueprintProperty Clone()
		{
			return new DataBlueprintProperty(_name, _description, _objectMemberName, _pValue);
		}
	}
}
