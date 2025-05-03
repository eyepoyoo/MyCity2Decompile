using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Prime31
{
	// Token: 0x0200001A RID: 26
	public class SimpleJson
	{
		// Token: 0x06000099 RID: 153 RVA: 0x0000613C File Offset: 0x0000433C
		public static string encode(object obj)
		{
			StringBuilder stringBuilder = new StringBuilder(2000);
			bool flag = SimpleJson.serializeValue(SimpleJson.currentJsonSerializerStrategy, obj, stringBuilder);
			return (!flag) ? null : stringBuilder.ToString();
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00006174 File Offset: 0x00004374
		public static bool tryDeserializeObject(string json, out object obj)
		{
			bool flag = true;
			if (json != null)
			{
				char[] array = json.ToCharArray();
				int num = 0;
				obj = SimpleJson.parseValue(array, ref num, ref flag);
			}
			else
			{
				obj = null;
			}
			return flag;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000061A8 File Offset: 0x000043A8
		public static object decode(string json)
		{
			object obj;
			if (SimpleJson.tryDeserializeObject(json, out obj))
			{
				return obj;
			}
			Utils.logObject("Something went wrong deserializing the json. We got a null return. Here is the json we tried to deserialize: " + json);
			return null;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000061D5 File Offset: 0x000043D5
		private static object decode(string json, Type type)
		{
			return SimpleJson.decode(json, type, null);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000061DF File Offset: 0x000043DF
		public static T decode<T>(string json)
		{
			return (T)((object)SimpleJson.decode(json, typeof(T)));
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000061F6 File Offset: 0x000043F6
		public static T decode<T>(string json, string rootElement) where T : new()
		{
			return (T)((object)SimpleJson.decode(json, typeof(T), rootElement));
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00006210 File Offset: 0x00004410
		private static object decode(string json, Type type, string rootElement = null)
		{
			object obj = SimpleJson.decode(json);
			if (type == null || (obj != null && obj.GetType().IsAssignableFrom(type)))
			{
				return obj;
			}
			if (rootElement != null)
			{
				if (obj is JsonObject)
				{
					JsonObject jsonObject = obj as JsonObject;
					if (jsonObject.ContainsKey(rootElement))
					{
						obj = jsonObject[rootElement];
					}
					else
					{
						Utils.logObject(string.Format("A rootElement was requested ({0})  but does not exist in the decoded Dictionary", rootElement));
					}
				}
				else
				{
					Utils.logObject(string.Format("A rootElement was requested ({0}) but the decoded object is not a Dictionary. It is a {1}", rootElement, obj.GetType()));
				}
			}
			return SimpleJson.currentJsonSerializerStrategy.deserializeObject(obj, type);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000062AC File Offset: 0x000044AC
		public static T decodeObject<T>(object jsonObject, string rootElement = null)
		{
			Type typeFromHandle = typeof(T);
			if (typeFromHandle == null || (jsonObject != null && jsonObject.GetType().IsAssignableFrom(typeFromHandle)))
			{
				return (T)((object)jsonObject);
			}
			if (rootElement != null)
			{
				if (jsonObject is Dictionary<string, object>)
				{
					Dictionary<string, object> dictionary = jsonObject as Dictionary<string, object>;
					if (dictionary.ContainsKey(rootElement))
					{
						jsonObject = dictionary[rootElement];
					}
					else
					{
						Utils.logObject(string.Format("A rootElement was requested ({0})  but does not exist in the decoded Dictionary", rootElement));
					}
				}
				else
				{
					Utils.logObject(string.Format("A rootElement was requested ({0}) but the decoded object is not a Dictionary. It is a {1}", rootElement, jsonObject.GetType()));
				}
			}
			return (T)((object)SimpleJson.currentJsonSerializerStrategy.deserializeObject(jsonObject, typeFromHandle));
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00006358 File Offset: 0x00004558
		protected static IDictionary<string, object> parseObject(char[] json, ref int index, ref bool success)
		{
			IDictionary<string, object> dictionary = new JsonObject();
			SimpleJson.nextToken(json, ref index);
			bool flag = false;
			while (!flag)
			{
				int num = SimpleJson.lookAhead(json, index);
				if (num == 0)
				{
					success = false;
					return null;
				}
				if (num == 6)
				{
					SimpleJson.nextToken(json, ref index);
				}
				else
				{
					if (num == 2)
					{
						SimpleJson.nextToken(json, ref index);
						return dictionary;
					}
					string text = SimpleJson.parseString(json, ref index, ref success);
					if (!success)
					{
						success = false;
						return null;
					}
					num = SimpleJson.nextToken(json, ref index);
					if (num != 5)
					{
						success = false;
						return null;
					}
					object obj = SimpleJson.parseValue(json, ref index, ref success);
					if (!success)
					{
						success = false;
						return null;
					}
					dictionary[text] = obj;
				}
			}
			return dictionary;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00006404 File Offset: 0x00004604
		protected static JsonArray parseArray(char[] json, ref int index, ref bool success)
		{
			JsonArray jsonArray = new JsonArray();
			SimpleJson.nextToken(json, ref index);
			bool flag = false;
			while (!flag)
			{
				int num = SimpleJson.lookAhead(json, index);
				if (num == 0)
				{
					success = false;
					return null;
				}
				if (num == 6)
				{
					SimpleJson.nextToken(json, ref index);
				}
				else
				{
					if (num == 4)
					{
						SimpleJson.nextToken(json, ref index);
						break;
					}
					object obj = SimpleJson.parseValue(json, ref index, ref success);
					if (!success)
					{
						return null;
					}
					jsonArray.Add(obj);
				}
			}
			return jsonArray;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00006484 File Offset: 0x00004684
		protected static object parseValue(char[] json, ref int index, ref bool success)
		{
			switch (SimpleJson.lookAhead(json, index))
			{
			case 1:
				return SimpleJson.parseObject(json, ref index, ref success);
			case 3:
				return SimpleJson.parseArray(json, ref index, ref success);
			case 7:
				return SimpleJson.parseString(json, ref index, ref success);
			case 8:
				return SimpleJson.parseNumber(json, ref index, ref success);
			case 9:
				SimpleJson.nextToken(json, ref index);
				return true;
			case 10:
				SimpleJson.nextToken(json, ref index);
				return false;
			case 11:
				SimpleJson.nextToken(json, ref index);
				return null;
			}
			success = false;
			return null;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000652C File Offset: 0x0000472C
		protected static string parseString(char[] json, ref int index, ref bool success)
		{
			StringBuilder stringBuilder = new StringBuilder(2000);
			SimpleJson.eatWhitespace(json, ref index);
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
						stringBuilder.Append('"');
					}
					else if (c == '\\')
					{
						stringBuilder.Append('\\');
					}
					else if (c == '/')
					{
						stringBuilder.Append('/');
					}
					else if (c == 'b')
					{
						stringBuilder.Append('\b');
					}
					else if (c == 'f')
					{
						stringBuilder.Append('\f');
					}
					else if (c == 'n')
					{
						stringBuilder.Append('\n');
					}
					else if (c == 'r')
					{
						stringBuilder.Append('\r');
					}
					else if (c == 't')
					{
						stringBuilder.Append('\t');
					}
					else if (c == 'u')
					{
						int num = json.Length - index;
						if (num < 4)
						{
							break;
						}
						uint num2;
						if (!(success = uint.TryParse(new string(json, index, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num2)))
						{
							return "";
						}
						if (55296U <= num2 && num2 <= 56319U)
						{
							index += 4;
							num = json.Length - index;
							uint num3;
							if (num < 6 || !(new string(json, index, 2) == "\\u") || !uint.TryParse(new string(json, index + 2, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num3) || 56320U > num3 || num3 > 57343U)
							{
								success = false;
								return "";
							}
							stringBuilder.Append((char)num2);
							stringBuilder.Append((char)num3);
							index += 6;
						}
						else
						{
							stringBuilder.Append(char.ConvertFromUtf32((int)num2));
							index += 4;
						}
					}
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			if (!flag)
			{
				success = false;
				return null;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00006788 File Offset: 0x00004988
		protected static object parseNumber(char[] json, ref int index, ref bool success)
		{
			SimpleJson.eatWhitespace(json, ref index);
			int lastIndexOfNumber = SimpleJson.getLastIndexOfNumber(json, index);
			int num = lastIndexOfNumber - index + 1;
			string text = new string(json, index, num);
			object obj;
			if (text.IndexOf(".", StringComparison.OrdinalIgnoreCase) != -1 || text.IndexOf("e", StringComparison.OrdinalIgnoreCase) != -1)
			{
				double num2;
				success = double.TryParse(new string(json, index, num), NumberStyles.Any, CultureInfo.InvariantCulture, out num2);
				obj = num2;
			}
			else
			{
				long num3;
				success = long.TryParse(new string(json, index, num), NumberStyles.Any, CultureInfo.InvariantCulture, out num3);
				obj = num3;
			}
			index = lastIndexOfNumber + 1;
			return obj;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00006830 File Offset: 0x00004A30
		protected static int getLastIndexOfNumber(char[] json, int index)
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

		// Token: 0x060000A7 RID: 167 RVA: 0x0000686C File Offset: 0x00004A6C
		protected static void eatWhitespace(char[] json, ref int index)
		{
			while (index < json.Length)
			{
				if (" \t\n\r\b\f".IndexOf(json[index]) == -1)
				{
					break;
				}
				index++;
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x0000689C File Offset: 0x00004A9C
		protected static int lookAhead(char[] json, int index)
		{
			int num = index;
			return SimpleJson.nextToken(json, ref num);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000068B4 File Offset: 0x00004AB4
		protected static int nextToken(char[] json, ref int index)
		{
			SimpleJson.eatWhitespace(json, ref index);
			if (index == json.Length)
			{
				return 0;
			}
			char c = json[index];
			index++;
			switch (c)
			{
			case ',':
				return 6;
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
				return 8;
			default:
				switch (c)
				{
				case '[':
					return 3;
				default:
					switch (c)
					{
					case '{':
						return 1;
					default:
					{
						if (c == '"')
						{
							return 7;
						}
						index--;
						int num = json.Length - index;
						if (num >= 5 && json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
						{
							index += 5;
							return 10;
						}
						if (num >= 4 && json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
						{
							index += 4;
							return 9;
						}
						if (num >= 4 && json[index] == 'n' && json[index + 1] == 'u' && json[index + 2] == 'l' && json[index + 3] == 'l')
						{
							index += 4;
							return 11;
						}
						return 0;
					}
					case '}':
						return 2;
					}
					break;
				case ']':
					return 4;
				}
				break;
			case ':':
				return 5;
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00006A4C File Offset: 0x00004C4C
		protected static bool serializeValue(IJsonSerializerStrategy jsonSerializerStrategy, object value, StringBuilder builder)
		{
			bool flag = true;
			if (value is string)
			{
				flag = SimpleJson.serializeString((string)value, builder);
			}
			else if (value is IDictionary<string, object>)
			{
				IDictionary<string, object> dictionary = (IDictionary<string, object>)value;
				flag = SimpleJson.serializeObject(jsonSerializerStrategy, dictionary.Keys, dictionary.Values, builder);
			}
			else if (value is IDictionary<string, string>)
			{
				IDictionary<string, string> dictionary2 = (IDictionary<string, string>)value;
				flag = SimpleJson.serializeObject(jsonSerializerStrategy, dictionary2.Keys, dictionary2.Values, builder);
			}
			else if (value is IDictionary)
			{
				IDictionary dictionary3 = (IDictionary)value;
				flag = SimpleJson.serializeObject(jsonSerializerStrategy, dictionary3.Keys, dictionary3.Values, builder);
			}
			else if (value is IEnumerable)
			{
				flag = SimpleJson.serializeArray(jsonSerializerStrategy, (IEnumerable)value, builder);
			}
			else if (SimpleJson.isNumeric(value))
			{
				flag = SimpleJson.serializeNumber(value, builder);
			}
			else if (value is bool)
			{
				builder.Append((!(bool)value) ? "false" : "true");
			}
			else if (value == null)
			{
				builder.Append("null");
			}
			else
			{
				object obj;
				flag = jsonSerializerStrategy.serializeNonPrimitiveObject(value, out obj);
				if (flag)
				{
					SimpleJson.serializeValue(jsonSerializerStrategy, obj, builder);
				}
			}
			return flag;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00006B94 File Offset: 0x00004D94
		protected static bool serializeObject(IJsonSerializerStrategy jsonSerializerStrategy, IEnumerable keys, IEnumerable values, StringBuilder builder)
		{
			builder.Append("{");
			IEnumerator enumerator = keys.GetEnumerator();
			IEnumerator enumerator2 = values.GetEnumerator();
			bool flag = true;
			while (enumerator.MoveNext() && enumerator2.MoveNext())
			{
				object obj = enumerator.Current;
				object obj2 = enumerator2.Current;
				if (!flag)
				{
					builder.Append(",");
				}
				if (obj is string)
				{
					SimpleJson.serializeString((string)obj, builder);
				}
				else if (!SimpleJson.serializeValue(jsonSerializerStrategy, obj2, builder))
				{
					return false;
				}
				builder.Append(":");
				if (!SimpleJson.serializeValue(jsonSerializerStrategy, obj2, builder))
				{
					return false;
				}
				flag = false;
			}
			builder.Append("}");
			return true;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00006C54 File Offset: 0x00004E54
		protected static bool serializeArray(IJsonSerializerStrategy jsonSerializerStrategy, IEnumerable anArray, StringBuilder builder)
		{
			builder.Append("[");
			bool flag = true;
			IEnumerator enumerator = anArray.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					if (!flag)
					{
						builder.Append(",");
					}
					if (!SimpleJson.serializeValue(jsonSerializerStrategy, obj, builder))
					{
						return false;
					}
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
			builder.Append("]");
			return true;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00006CF0 File Offset: 0x00004EF0
		protected static bool serializeString(string aString, StringBuilder builder)
		{
			builder.Append("\"");
			foreach (char c in aString.ToCharArray())
			{
				if (c == '"')
				{
					builder.Append("\\\"");
				}
				else if (c == '\\')
				{
					builder.Append("\\\\");
				}
				else if (c == '\b')
				{
					builder.Append("\\b");
				}
				else if (c == '\f')
				{
					builder.Append("\\f");
				}
				else if (c == '\n')
				{
					builder.Append("\\n");
				}
				else if (c == '\r')
				{
					builder.Append("\\r");
				}
				else if (c == '\t')
				{
					builder.Append("\\t");
				}
				else
				{
					builder.Append(c);
				}
			}
			builder.Append("\"");
			return true;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00006DEC File Offset: 0x00004FEC
		protected static bool serializeNumber(object number, StringBuilder builder)
		{
			if (number is long)
			{
				builder.Append(((long)number).ToString(CultureInfo.InvariantCulture));
			}
			else if (number is ulong)
			{
				builder.Append(((ulong)number).ToString(CultureInfo.InvariantCulture));
			}
			else if (number is int)
			{
				builder.Append(((int)number).ToString(CultureInfo.InvariantCulture));
			}
			else if (number is uint)
			{
				builder.Append(((uint)number).ToString(CultureInfo.InvariantCulture));
			}
			else if (number is decimal)
			{
				builder.Append(((decimal)number).ToString(CultureInfo.InvariantCulture));
			}
			else if (number is float)
			{
				builder.Append(((float)number).ToString(CultureInfo.InvariantCulture));
			}
			else
			{
				builder.Append(Convert.ToDouble(number, CultureInfo.InvariantCulture).ToString("r", CultureInfo.InvariantCulture));
			}
			return true;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00006F20 File Offset: 0x00005120
		protected static bool isNumeric(object value)
		{
			return value is sbyte || value is byte || value is short || value is ushort || value is int || value is uint || value is long || value is ulong || value is float || value is double || value is decimal;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00006FBD File Offset: 0x000051BD
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x00006FD6 File Offset: 0x000051D6
		public static IJsonSerializerStrategy currentJsonSerializerStrategy
		{
			get
			{
				IJsonSerializerStrategy jsonSerializerStrategy;
				if ((jsonSerializerStrategy = SimpleJson._currentJsonSerializerStrategy) == null)
				{
					jsonSerializerStrategy = (SimpleJson._currentJsonSerializerStrategy = SimpleJson.pocoJsonSerializerStrategy);
				}
				return jsonSerializerStrategy;
			}
			set
			{
				SimpleJson._currentJsonSerializerStrategy = value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00006FDE File Offset: 0x000051DE
		public static PocoJsonSerializerStrategy pocoJsonSerializerStrategy
		{
			get
			{
				PocoJsonSerializerStrategy pocoJsonSerializerStrategy;
				if ((pocoJsonSerializerStrategy = SimpleJson._pocoJsonSerializerStrategy) == null)
				{
					pocoJsonSerializerStrategy = (SimpleJson._pocoJsonSerializerStrategy = new PocoJsonSerializerStrategy());
				}
				return pocoJsonSerializerStrategy;
			}
		}

		// Token: 0x04000047 RID: 71
		private const int TOKEN_NONE = 0;

		// Token: 0x04000048 RID: 72
		private const int TOKEN_CURLY_OPEN = 1;

		// Token: 0x04000049 RID: 73
		private const int TOKEN_CURLY_CLOSE = 2;

		// Token: 0x0400004A RID: 74
		private const int TOKEN_SQUARED_OPEN = 3;

		// Token: 0x0400004B RID: 75
		private const int TOKEN_SQUARED_CLOSE = 4;

		// Token: 0x0400004C RID: 76
		private const int TOKEN_COLON = 5;

		// Token: 0x0400004D RID: 77
		private const int TOKEN_COMMA = 6;

		// Token: 0x0400004E RID: 78
		private const int TOKEN_STRING = 7;

		// Token: 0x0400004F RID: 79
		private const int TOKEN_NUMBER = 8;

		// Token: 0x04000050 RID: 80
		private const int TOKEN_TRUE = 9;

		// Token: 0x04000051 RID: 81
		private const int TOKEN_FALSE = 10;

		// Token: 0x04000052 RID: 82
		private const int TOKEN_NULL = 11;

		// Token: 0x04000053 RID: 83
		private const int BUILDER_CAPACITY = 2000;

		// Token: 0x04000054 RID: 84
		private static IJsonSerializerStrategy _currentJsonSerializerStrategy;

		// Token: 0x04000055 RID: 85
		private static PocoJsonSerializerStrategy _pocoJsonSerializerStrategy;
	}
}
