using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Prime31
{
	// Token: 0x02000010 RID: 16
	public class Json
	{
		// Token: 0x06000069 RID: 105 RVA: 0x00004AE8 File Offset: 0x00002CE8
		public static object decode(string json)
		{
			if (Json.useSimpleJson)
			{
				return SimpleJson.decode(json);
			}
			object obj = Json.Deserializer.deserialize(json);
			if (obj == null)
			{
				Utils.logObject("Something went wrong deserializing the json. We got a null return. Here is the json we tried to deserialize: " + json);
			}
			return obj;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00004B24 File Offset: 0x00002D24
		public static T decode<T>(string json, string rootElement = null) where T : new()
		{
			if (Json.useSimpleJson)
			{
				return SimpleJson.decode<T>(json, rootElement);
			}
			return (T)((object)Json.ObjectDecoder.decode<T>(json, rootElement));
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00004B44 File Offset: 0x00002D44
		public static T decodeObject<T>(object jsonObject, string rootElement = null) where T : new()
		{
			return SimpleJson.decodeObject<T>(jsonObject, rootElement);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004B50 File Offset: 0x00002D50
		public static string encode(object obj)
		{
			string text = ((!Json.useSimpleJson) ? Json.Serializer.serialize(obj) : SimpleJson.encode(obj));
			if (text == null)
			{
				Utils.logObject("Something went wrong serializing the object. We got a null return. Here is the object we tried to deserialize: ");
				Utils.logObject(obj);
			}
			return text;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00004B90 File Offset: 0x00002D90
		public static object jsonDecode(string json)
		{
			return Json.decode(json);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004B98 File Offset: 0x00002D98
		public static string jsonEncode(object obj)
		{
			string text = Json.Serializer.serialize(obj);
			if (text == null)
			{
				Utils.logObject("Something went wrong serializing the object. We got a null return. Here is the object we tried to deserialize: ");
				Utils.logObject(obj);
			}
			return text;
		}

		// Token: 0x0400002B RID: 43
		public static bool useSimpleJson = true;

		// Token: 0x02000011 RID: 17
		internal class ObjectDecoder
		{
			// Token: 0x06000071 RID: 113 RVA: 0x00004BD4 File Offset: 0x00002DD4
			public static object decode<T>(string json, string rootElement = null) where T : new()
			{
				object obj = Json.decode(json);
				if (obj == null)
				{
					return null;
				}
				return new Json.ObjectDecoder().decode<T>(obj, rootElement);
			}

			// Token: 0x06000072 RID: 114 RVA: 0x00004BFC File Offset: 0x00002DFC
			private object decode<T>(object decodedJsonObject, string rootElement = null) where T : new()
			{
				if (rootElement != null)
				{
					IDictionary dictionary = decodedJsonObject as IDictionary;
					if (dictionary == null)
					{
						Utils.logObject(string.Concat(new object[] { "A rootElement was requested (", rootElement, ") but the json did not decode to a Dictionary. It decoded to: ", decodedJsonObject }));
						return null;
					}
					if (!dictionary.Contains(rootElement))
					{
						Utils.logObject("A rootElement was requested (" + rootElement + ") but does not exist in the decoded Dictionary");
						return null;
					}
					decodedJsonObject = dictionary[rootElement];
				}
				Type type = typeof(T);
				IList list = null;
				if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(List<>))
				{
					return this.createAndPopulateObjectFromDictionary(type, decodedJsonObject as Dictionary<string, object>);
				}
				list = new T() as IList;
				type = list.GetType().GetGenericArguments()[0];
				if (!(decodedJsonObject is IList) || !decodedJsonObject.GetType().IsGenericType)
				{
					Utils.logObject("A List was required but the json did not decode to a List. It decoded to: " + decodedJsonObject);
					return null;
				}
				IEnumerator enumerator = ((IList)decodedJsonObject).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Dictionary<string, object> dictionary2 = (Dictionary<string, object>)obj;
						if (dictionary2 == null)
						{
							Utils.logObject("Aborted populating List because the json did not decode to a List of Dictionaries");
							return list;
						}
						list.Add(this.createAndPopulateObjectFromDictionary(type, dictionary2));
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = enumerator as IDisposable) != null)
					{
						disposable.Dispose();
					}
				}
				return list;
			}

			// Token: 0x06000073 RID: 115 RVA: 0x00004D7C File Offset: 0x00002F7C
			private Dictionary<string, Action<object, object>> getMemberInfoForObject(object obj)
			{
				if (this._memberInfo == null)
				{
					this._memberInfo = Json.ObjectDecoder.getMembersWithSetters(obj);
				}
				return this._memberInfo;
			}

			// Token: 0x06000074 RID: 116 RVA: 0x00004D9C File Offset: 0x00002F9C
			private static Dictionary<string, Action<object, object>> getMembersWithSetters(object obj)
			{
				Dictionary<string, Action<object, object>> dictionary = new Dictionary<string, Action<object, object>>();
				FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				for (int i = 0; i < fields.Length; i++)
				{
					FieldInfo fieldInfo = fields[i];
					if (fieldInfo.FieldType.Namespace.StartsWith("System"))
					{
						FieldInfo theInfo2 = fieldInfo;
						Type theFieldType = fieldInfo.FieldType;
						dictionary[fieldInfo.Name] = delegate(object ownerObject, object val)
						{
							theInfo2.SetValue(ownerObject, Convert.ChangeType(val, theFieldType));
						};
					}
				}
				foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					if (propertyInfo.PropertyType.Namespace.StartsWith("System"))
					{
						if (propertyInfo.CanWrite && propertyInfo.GetSetMethod(true) != null)
						{
							PropertyInfo theInfo = propertyInfo;
							Type thePropertyType = propertyInfo.PropertyType;
							dictionary[propertyInfo.Name] = delegate(object ownerObject, object val)
							{
								theInfo.SetValue(ownerObject, Convert.ChangeType(val, thePropertyType), null);
							};
						}
					}
				}
				return dictionary;
			}

			// Token: 0x06000075 RID: 117 RVA: 0x00004ECC File Offset: 0x000030CC
			public object createAndPopulateObjectFromDictionary(Type objectType, Dictionary<string, object> dict)
			{
				object obj = Activator.CreateInstance(objectType);
				Dictionary<string, Action<object, object>> memberInfoForObject = this.getMemberInfoForObject(obj);
				Dictionary<string, object>.KeyCollection keys = dict.Keys;
				foreach (string text in keys)
				{
					if (memberInfoForObject.ContainsKey(text))
					{
						try
						{
							memberInfoForObject[text](obj, dict[text]);
						}
						catch (Exception ex)
						{
							Utils.logObject(ex);
						}
					}
				}
				return obj;
			}

			// Token: 0x0400002C RID: 44
			private Dictionary<string, Action<object, object>> _memberInfo;
		}

		// Token: 0x02000012 RID: 18
		internal class Deserializer
		{
			// Token: 0x06000076 RID: 118 RVA: 0x00004FB9 File Offset: 0x000031B9
			private Deserializer(string json)
			{
				this.charArray = json.ToCharArray();
			}

			// Token: 0x06000077 RID: 119 RVA: 0x00004FD0 File Offset: 0x000031D0
			public static object deserialize(string json)
			{
				if (json != null)
				{
					Json.Deserializer deserializer = new Json.Deserializer(json);
					return deserializer.deserialize();
				}
				return null;
			}

			// Token: 0x06000078 RID: 120 RVA: 0x00004FF4 File Offset: 0x000031F4
			private object deserialize()
			{
				int num = 0;
				return this.parseValue(this.charArray, ref num);
			}

			// Token: 0x06000079 RID: 121 RVA: 0x00005014 File Offset: 0x00003214
			protected object parseValue(char[] json, ref int index)
			{
				switch (this.lookAhead(json, index))
				{
				case Json.Deserializer.JsonToken.CurlyOpen:
					return this.parseObject(json, ref index);
				case Json.Deserializer.JsonToken.SquaredOpen:
					return this.parseArray(json, ref index);
				case Json.Deserializer.JsonToken.String:
					return this.parseString(json, ref index);
				case Json.Deserializer.JsonToken.Number:
					return this.parseNumber(json, ref index);
				case Json.Deserializer.JsonToken.True:
					this.nextToken(json, ref index);
					return bool.Parse("TRUE");
				case Json.Deserializer.JsonToken.False:
					this.nextToken(json, ref index);
					return bool.Parse("FALSE");
				case Json.Deserializer.JsonToken.Null:
					this.nextToken(json, ref index);
					return null;
				}
				return null;
			}

			// Token: 0x0600007A RID: 122 RVA: 0x000050D0 File Offset: 0x000032D0
			private IDictionary parseObject(char[] json, ref int index)
			{
				IDictionary dictionary = new Dictionary<string, object>();
				this.nextToken(json, ref index);
				bool flag = false;
				while (!flag)
				{
					Json.Deserializer.JsonToken jsonToken = this.lookAhead(json, index);
					if (jsonToken == Json.Deserializer.JsonToken.None)
					{
						return null;
					}
					if (jsonToken == Json.Deserializer.JsonToken.Comma)
					{
						this.nextToken(json, ref index);
					}
					else
					{
						if (jsonToken == Json.Deserializer.JsonToken.CurlyClose)
						{
							this.nextToken(json, ref index);
							return dictionary;
						}
						string text = this.parseString(json, ref index);
						if (text == null)
						{
							return null;
						}
						jsonToken = this.nextToken(json, ref index);
						if (jsonToken != Json.Deserializer.JsonToken.Colon)
						{
							return null;
						}
						object obj = this.parseValue(json, ref index);
						dictionary[text] = obj;
					}
				}
				return dictionary;
			}

			// Token: 0x0600007B RID: 123 RVA: 0x0000516C File Offset: 0x0000336C
			private IList parseArray(char[] json, ref int index)
			{
				List<object> list = new List<object>();
				this.nextToken(json, ref index);
				bool flag = false;
				while (!flag)
				{
					Json.Deserializer.JsonToken jsonToken = this.lookAhead(json, index);
					if (jsonToken == Json.Deserializer.JsonToken.None)
					{
						return null;
					}
					if (jsonToken == Json.Deserializer.JsonToken.Comma)
					{
						this.nextToken(json, ref index);
					}
					else
					{
						if (jsonToken == Json.Deserializer.JsonToken.SquaredClose)
						{
							this.nextToken(json, ref index);
							break;
						}
						object obj = this.parseValue(json, ref index);
						list.Add(obj);
					}
				}
				return list;
			}

			// Token: 0x0600007C RID: 124 RVA: 0x000051E4 File Offset: 0x000033E4
			private string parseString(char[] json, ref int index)
			{
				string text = string.Empty;
				this.eatWhitespace(json, ref index);
				char c = json[index++];
				bool flag = false;
				while (!flag)
				{
					if (index == json.Length)
					{
						break;
					}
					c = json[index++];
					if (c == '"')
					{
						flag = true;
						break;
					}
					if (c == '\\')
					{
						if (index == json.Length)
						{
							break;
						}
						c = json[index++];
						if (c == '"')
						{
							text += '"';
						}
						else if (c == '\\')
						{
							text += '\\';
						}
						else if (c == '/')
						{
							text += '/';
						}
						else if (c == 'b')
						{
							text += '\b';
						}
						else if (c == 'f')
						{
							text += '\f';
						}
						else if (c == 'n')
						{
							text += '\n';
						}
						else if (c == 'r')
						{
							text += '\r';
						}
						else if (c == 't')
						{
							text += '\t';
						}
						else if (c == 'u')
						{
							int num = json.Length - index;
							if (num < 4)
							{
								break;
							}
							char[] array = new char[4];
							Array.Copy(json, index, array, 0, 4);
							uint num2 = uint.Parse(new string(array), NumberStyles.HexNumber);
							try
							{
								text += char.ConvertFromUtf32((int)num2);
							}
							catch (Exception)
							{
								foreach (char c2 in array)
								{
									text += c2;
								}
							}
							index += 4;
						}
					}
					else
					{
						text += c;
					}
				}
				if (!flag)
				{
					return null;
				}
				return text;
			}

			// Token: 0x0600007D RID: 125 RVA: 0x00005404 File Offset: 0x00003604
			private object parseNumber(char[] json, ref int index)
			{
				this.eatWhitespace(json, ref index);
				int lastIndexOfNumber = this.getLastIndexOfNumber(json, index);
				int num = lastIndexOfNumber - index + 1;
				char[] array = new char[num];
				Array.Copy(json, index, array, 0, num);
				index = lastIndexOfNumber + 1;
				string text = new string(array);
				long num2;
				if (!text.Contains(".") && long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out num2))
				{
					return num2;
				}
				return double.Parse(new string(array), CultureInfo.InvariantCulture);
			}

			// Token: 0x0600007E RID: 126 RVA: 0x00005488 File Offset: 0x00003688
			private int getLastIndexOfNumber(char[] json, int index)
			{
				int i;
				for (i = index; i < json.Length; i++)
				{
					if ("0123456789+-.eE".IndexOf(json[i]) == -1)
					{
						break;
					}
				}
				return i - 1;
			}

			// Token: 0x0600007F RID: 127 RVA: 0x000054C4 File Offset: 0x000036C4
			private void eatWhitespace(char[] json, ref int index)
			{
				while (index < json.Length)
				{
					if (" \t\n\r".IndexOf(json[index]) == -1)
					{
						break;
					}
					index++;
				}
			}

			// Token: 0x06000080 RID: 128 RVA: 0x000054F4 File Offset: 0x000036F4
			private Json.Deserializer.JsonToken lookAhead(char[] json, int index)
			{
				int num = index;
				return this.nextToken(json, ref num);
			}

			// Token: 0x06000081 RID: 129 RVA: 0x0000550C File Offset: 0x0000370C
			private Json.Deserializer.JsonToken nextToken(char[] json, ref int index)
			{
				this.eatWhitespace(json, ref index);
				if (index == json.Length)
				{
					return Json.Deserializer.JsonToken.None;
				}
				char c = json[index];
				index++;
				switch (c)
				{
				case ',':
					return Json.Deserializer.JsonToken.Comma;
				case '-':
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					return Json.Deserializer.JsonToken.Number;
				default:
					switch (c)
					{
					case '[':
						return Json.Deserializer.JsonToken.SquaredOpen;
					default:
						switch (c)
						{
						case '{':
							return Json.Deserializer.JsonToken.CurlyOpen;
						default:
						{
							if (c == '"')
							{
								return Json.Deserializer.JsonToken.String;
							}
							index--;
							int num = json.Length - index;
							if (num >= 5 && json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
							{
								index += 5;
								return Json.Deserializer.JsonToken.False;
							}
							if (num >= 4 && json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
							{
								index += 4;
								return Json.Deserializer.JsonToken.True;
							}
							if (num >= 4 && json[index] == 'n' && json[index + 1] == 'u' && json[index + 2] == 'l' && json[index + 3] == 'l')
							{
								index += 4;
								return Json.Deserializer.JsonToken.Null;
							}
							return Json.Deserializer.JsonToken.None;
						}
						case '}':
							return Json.Deserializer.JsonToken.CurlyClose;
						}
						break;
					case ']':
						return Json.Deserializer.JsonToken.SquaredClose;
					}
					break;
				case ':':
					return Json.Deserializer.JsonToken.Colon;
				}
			}

			// Token: 0x0400002D RID: 45
			private char[] charArray;

			// Token: 0x02000013 RID: 19
			private enum JsonToken
			{
				// Token: 0x0400002F RID: 47
				None,
				// Token: 0x04000030 RID: 48
				CurlyOpen,
				// Token: 0x04000031 RID: 49
				CurlyClose,
				// Token: 0x04000032 RID: 50
				SquaredOpen,
				// Token: 0x04000033 RID: 51
				SquaredClose,
				// Token: 0x04000034 RID: 52
				Colon,
				// Token: 0x04000035 RID: 53
				Comma,
				// Token: 0x04000036 RID: 54
				String,
				// Token: 0x04000037 RID: 55
				Number,
				// Token: 0x04000038 RID: 56
				True,
				// Token: 0x04000039 RID: 57
				False,
				// Token: 0x0400003A RID: 58
				Null
			}
		}

		// Token: 0x02000014 RID: 20
		internal class Serializer
		{
			// Token: 0x06000082 RID: 130 RVA: 0x000056A5 File Offset: 0x000038A5
			private Serializer()
			{
				this._builder = new StringBuilder();
			}

			// Token: 0x06000083 RID: 131 RVA: 0x000056B8 File Offset: 0x000038B8
			public static string serialize(object obj)
			{
				Json.Serializer serializer = new Json.Serializer();
				serializer.serializeObject(obj);
				return serializer._builder.ToString();
			}

			// Token: 0x06000084 RID: 132 RVA: 0x000056E0 File Offset: 0x000038E0
			private void serializeObject(object value)
			{
				if (value == null)
				{
					this._builder.Append("null");
				}
				else if (value is string)
				{
					this.serializeString((string)value);
				}
				else if (value is IList)
				{
					this.serializeIList((IList)value);
				}
				else if (value is Dictionary<string, object>)
				{
					this.serializeDictionary((Dictionary<string, object>)value);
				}
				else if (value is IDictionary)
				{
					this.serializeIDictionary((IDictionary)value);
				}
				else if (value is bool)
				{
					this._builder.Append(value.ToString().ToLower());
				}
				else if (value.GetType().IsPrimitive)
				{
					this._builder.Append(value);
				}
				else if (value is DateTime)
				{
					DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
					double totalMilliseconds = ((DateTime)value).Subtract(dateTime).TotalMilliseconds;
					this.serializeString(Convert.ToString(totalMilliseconds, CultureInfo.InvariantCulture));
				}
				else
				{
					try
					{
						this.serializeClass(value);
					}
					catch (Exception ex)
					{
						Utils.logObject(string.Format("failed to serialize {0} with error: {1}", value, ex.Message));
					}
				}
			}

			// Token: 0x06000085 RID: 133 RVA: 0x00005848 File Offset: 0x00003A48
			private void serializeIList(IList anArray)
			{
				this._builder.Append("[");
				bool flag = true;
				for (int i = 0; i < anArray.Count; i++)
				{
					object obj = anArray[i];
					if (!flag)
					{
						this._builder.Append(", ");
					}
					this.serializeObject(obj);
					flag = false;
				}
				this._builder.Append("]");
			}

			// Token: 0x06000086 RID: 134 RVA: 0x000058B8 File Offset: 0x00003AB8
			private void serializeIDictionary(IDictionary dict)
			{
				this._builder.Append("{");
				bool flag = true;
				IEnumerator enumerator = dict.Keys.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						if (!flag)
						{
							this._builder.Append(", ");
						}
						this.serializeString(obj.ToString());
						this._builder.Append(":");
						this.serializeObject(dict[obj]);
						flag = false;
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = enumerator as IDisposable) != null)
					{
						disposable.Dispose();
					}
				}
				this._builder.Append("}");
			}

			// Token: 0x06000087 RID: 135 RVA: 0x00005978 File Offset: 0x00003B78
			private void serializeDictionary(Dictionary<string, object> dict)
			{
				this._builder.Append("{");
				bool flag = true;
				Dictionary<string, object>.KeyCollection keys = dict.Keys;
				foreach (string text in keys)
				{
					if (!flag)
					{
						this._builder.Append(", ");
					}
					this.serializeString(text.ToString());
					this._builder.Append(":");
					this.serializeObject(dict[text]);
					flag = false;
				}
				this._builder.Append("}");
			}

			// Token: 0x06000088 RID: 136 RVA: 0x00005A38 File Offset: 0x00003C38
			private void serializeString(string str)
			{
				this._builder.Append("\"");
				foreach (char c in str.ToCharArray())
				{
					if (c == '"')
					{
						this._builder.Append("\\\"");
					}
					else if (c == '\\')
					{
						this._builder.Append("\\\\");
					}
					else if (c == '\b')
					{
						this._builder.Append("\\b");
					}
					else if (c == '\f')
					{
						this._builder.Append("\\f");
					}
					else if (c == '\n')
					{
						this._builder.Append("\\n");
					}
					else if (c == '\r')
					{
						this._builder.Append("\\r");
					}
					else if (c == '\t')
					{
						this._builder.Append("\\t");
					}
					else
					{
						int num = Convert.ToInt32(c, CultureInfo.InvariantCulture);
						if (num >= 32 && num <= 126)
						{
							this._builder.Append(c);
						}
						else
						{
							this._builder.Append("\\u" + Convert.ToString(num, 16).PadLeft(4, '0'));
						}
					}
				}
				this._builder.Append("\"");
			}

			// Token: 0x06000089 RID: 137 RVA: 0x00005BB0 File Offset: 0x00003DB0
			private void serializeClass(object value)
			{
				this._builder.Append("{");
				bool flag = true;
				foreach (FieldInfo fieldInfo in value.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					if (!fieldInfo.IsPrivate || !fieldInfo.Name.Contains("k__BackingField"))
					{
						if (!flag)
						{
							this._builder.Append(", ");
						}
						this.serializeString(fieldInfo.Name);
						this._builder.Append(":");
						this.serializeObject(fieldInfo.GetValue(value));
						flag = false;
					}
				}
				foreach (PropertyInfo propertyInfo in value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					if (!flag)
					{
						this._builder.Append(", ");
					}
					this.serializeString(propertyInfo.Name);
					this._builder.Append(":");
					this.serializeObject(propertyInfo.GetValue(value, null));
					flag = false;
				}
				this._builder.Append("}");
			}

			// Token: 0x0400003B RID: 59
			private StringBuilder _builder;
		}
	}
}
