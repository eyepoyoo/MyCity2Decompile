using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Prime31.Reflection;

namespace Prime31
{
	// Token: 0x0200001C RID: 28
	public class PocoJsonSerializerStrategy : IJsonSerializerStrategy
	{
		// Token: 0x060000B5 RID: 181 RVA: 0x00006FF7 File Offset: 0x000051F7
		public PocoJsonSerializerStrategy()
		{
			this.cacheResolver = new CacheResolver(new MemberMapLoader(this.buildMap));
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00007018 File Offset: 0x00005218
		protected virtual void buildMap(Type type, SafeDictionary<string, CacheResolver.MemberMap> memberMaps)
		{
			foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				memberMaps.add(propertyInfo.Name, new CacheResolver.MemberMap(propertyInfo));
			}
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				memberMaps.add(fieldInfo.Name, new CacheResolver.MemberMap(fieldInfo));
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00007093 File Offset: 0x00005293
		public virtual bool serializeNonPrimitiveObject(object input, out object output)
		{
			return this.trySerializeKnownTypes(input, out output) || this.trySerializeUnknownTypes(input, out output);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000070B0 File Offset: 0x000052B0
		public virtual object deserializeObject(object value, Type type)
		{
			object obj = null;
			if (value is string)
			{
				string text = value as string;
				if (!string.IsNullOrEmpty(text) && (type == typeof(DateTime) || (ReflectionUtils.isNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(DateTime))))
				{
					obj = DateTime.ParseExact(text, PocoJsonSerializerStrategy.Iso8601Format, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
				}
				else
				{
					obj = text;
				}
			}
			else if (value is bool)
			{
				obj = value;
			}
			else if (value == null)
			{
				obj = null;
			}
			else if ((value is long && type == typeof(long)) || (value is double && type == typeof(double)))
			{
				obj = value;
			}
			else
			{
				if ((!(value is double) || type == typeof(double)) && (!(value is long) || type == typeof(long)))
				{
					if (value is IDictionary<string, object>)
					{
						IDictionary<string, object> dictionary = (IDictionary<string, object>)value;
						if (ReflectionUtils.isTypeDictionary(type))
						{
							Type type2 = type.GetGenericArguments()[0];
							Type type3 = type.GetGenericArguments()[1];
							Type type4 = typeof(Dictionary<, >).MakeGenericType(new Type[] { type2, type3 });
							IDictionary dictionary2 = (IDictionary)CacheResolver.getNewInstance(type4);
							foreach (KeyValuePair<string, object> keyValuePair in dictionary)
							{
								dictionary2.Add(keyValuePair.Key, this.deserializeObject(keyValuePair.Value, type3));
							}
							obj = dictionary2;
						}
						else
						{
							obj = CacheResolver.getNewInstance(type);
							SafeDictionary<string, CacheResolver.MemberMap> safeDictionary = this.cacheResolver.loadMaps(type);
							if (safeDictionary == null)
							{
								obj = value;
							}
							else
							{
								foreach (KeyValuePair<string, CacheResolver.MemberMap> keyValuePair2 in safeDictionary)
								{
									CacheResolver.MemberMap value2 = keyValuePair2.Value;
									if (value2.Setter != null)
									{
										string key = keyValuePair2.Key;
										if (dictionary.ContainsKey(key))
										{
											object obj2 = this.deserializeObject(dictionary[key], value2.Type);
											value2.Setter(obj, obj2);
										}
									}
								}
							}
						}
					}
					else if (value is IList<object>)
					{
						IList<object> list = (IList<object>)value;
						IList list2 = null;
						if (type.IsArray)
						{
							list2 = (IList)Activator.CreateInstance(type, new object[] { list.Count });
							int num = 0;
							foreach (object obj3 in list)
							{
								list2[num++] = this.deserializeObject(obj3, type.GetElementType());
							}
						}
						else if (ReflectionUtils.isTypeGenericeCollectionInterface(type) || typeof(IList).IsAssignableFrom(type))
						{
							Type type5 = type.GetGenericArguments()[0];
							Type type6 = typeof(List<>).MakeGenericType(new Type[] { type5 });
							list2 = (IList)CacheResolver.getNewInstance(type6);
							foreach (object obj4 in list)
							{
								list2.Add(this.deserializeObject(obj4, type5));
							}
						}
						obj = list2;
					}
					return obj;
				}
				if (value is long && type == typeof(DateTime))
				{
					DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
					obj = dateTime.AddMilliseconds((double)((long)value));
				}
				else if (type.IsEnum)
				{
					obj = Enum.ToObject(type, value);
				}
				else
				{
					obj = ((!typeof(IConvertible).IsAssignableFrom(type)) ? value : Convert.ChangeType(value, type, CultureInfo.InvariantCulture));
				}
			}
			if (ReflectionUtils.isNullableType(type))
			{
				return ReflectionUtils.toNullableType(obj, type);
			}
			return obj;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00007548 File Offset: 0x00005748
		protected virtual object serializeEnum(Enum p)
		{
			return Convert.ToDouble(p, CultureInfo.InvariantCulture);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000755C File Offset: 0x0000575C
		protected virtual bool trySerializeKnownTypes(object input, out object output)
		{
			bool flag = true;
			if (input is DateTime)
			{
				output = ((DateTime)input).ToUniversalTime().ToString(PocoJsonSerializerStrategy.Iso8601Format[0], CultureInfo.InvariantCulture);
			}
			else if (input is Guid)
			{
				output = ((Guid)input).ToString("D");
			}
			else if (input is Uri)
			{
				output = input.ToString();
			}
			else if (input is Enum)
			{
				output = this.serializeEnum((Enum)input);
			}
			else
			{
				flag = false;
				output = null;
			}
			return flag;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00007600 File Offset: 0x00005800
		protected virtual bool trySerializeUnknownTypes(object input, out object output)
		{
			output = null;
			Type type = input.GetType();
			if (type.FullName == null)
			{
				return false;
			}
			IDictionary<string, object> dictionary = new JsonObject();
			SafeDictionary<string, CacheResolver.MemberMap> safeDictionary = this.cacheResolver.loadMaps(type);
			foreach (KeyValuePair<string, CacheResolver.MemberMap> keyValuePair in safeDictionary)
			{
				if (keyValuePair.Value.Getter != null)
				{
					dictionary.Add(keyValuePair.Key, keyValuePair.Value.Getter(input));
				}
			}
			output = dictionary;
			return true;
		}

		// Token: 0x04000056 RID: 86
		internal CacheResolver cacheResolver;

		// Token: 0x04000057 RID: 87
		private static readonly string[] Iso8601Format = new string[] { "yyyy-MM-dd\\THH:mm:ss.FFFFFFF\\Z", "yyyy-MM-dd\\THH:mm:ss\\Z", "yyyy-MM-dd\\THH:mm:ssK" };
	}
}
