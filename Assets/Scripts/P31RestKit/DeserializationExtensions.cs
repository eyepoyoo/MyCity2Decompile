using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Prime31
{
	// Token: 0x02000005 RID: 5
	public static class DeserializationExtensions
	{
		// Token: 0x06000015 RID: 21 RVA: 0x000027E4 File Offset: 0x000009E4
		public static List<T> toList<T>(this IList self)
		{
			List<T> list = new List<T>();
			IEnumerator enumerator = self.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
					list.Add(dictionary.toClass<T>());
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

		// Token: 0x06000016 RID: 22 RVA: 0x00002850 File Offset: 0x00000A50
		public static T toClass<T>(this IDictionary self)
		{
			object obj = Activator.CreateInstance(typeof(T));
			foreach (FieldInfo fieldInfo in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				foreach (object obj2 in fieldInfo.GetCustomAttributes(typeof(P31DeserializeableFieldAttribute), true))
				{
					P31DeserializeableFieldAttribute p31DeserializeableFieldAttribute = obj2 as P31DeserializeableFieldAttribute;
					if (self.Contains(p31DeserializeableFieldAttribute.key))
					{
						object obj3 = self[p31DeserializeableFieldAttribute.key];
						if (obj3 is IDictionary)
						{
							MethodInfo methodInfo = typeof(DeserializationExtensions).GetMethod("toClass").MakeGenericMethod(new Type[] { p31DeserializeableFieldAttribute.type });
							object obj4 = methodInfo.Invoke(null, new object[] { obj3 });
							fieldInfo.SetValue(obj, obj4);
							self.Remove(p31DeserializeableFieldAttribute.key);
						}
						else if (obj3 is IList)
						{
							if (!p31DeserializeableFieldAttribute.isCollection)
							{
								Debug.LogError("found an IList but the field is not a collection: " + p31DeserializeableFieldAttribute.key);
							}
							else
							{
								MethodInfo methodInfo2 = typeof(DeserializationExtensions).GetMethod("toList").MakeGenericMethod(new Type[] { p31DeserializeableFieldAttribute.type });
								object obj5 = methodInfo2.Invoke(null, new object[] { obj3 });
								fieldInfo.SetValue(obj, obj5);
								self.Remove(p31DeserializeableFieldAttribute.key);
							}
						}
						else
						{
							fieldInfo.SetValue(obj, Convert.ChangeType(obj3, fieldInfo.FieldType));
							self.Remove(p31DeserializeableFieldAttribute.key);
						}
					}
				}
			}
			return (T)((object)obj);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002A10 File Offset: 0x00000C10
		public static Dictionary<string, object> toDictionary(this object self)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			foreach (FieldInfo fieldInfo in self.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				foreach (object obj in fieldInfo.GetCustomAttributes(typeof(P31DeserializeableFieldAttribute), true))
				{
					P31DeserializeableFieldAttribute p31DeserializeableFieldAttribute = obj as P31DeserializeableFieldAttribute;
					if (p31DeserializeableFieldAttribute.isCollection)
					{
						IEnumerable enumerable = fieldInfo.GetValue(self) as IEnumerable;
						ArrayList arrayList = new ArrayList();
						IEnumerator enumerator = enumerable.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								object obj2 = enumerator.Current;
								arrayList.Add(obj2.toDictionary());
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
						dictionary[p31DeserializeableFieldAttribute.key] = arrayList;
					}
					else if (p31DeserializeableFieldAttribute.type != null)
					{
						dictionary[p31DeserializeableFieldAttribute.key] = fieldInfo.GetValue(self).toDictionary();
					}
					else
					{
						dictionary[p31DeserializeableFieldAttribute.key] = fieldInfo.GetValue(self);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002B58 File Offset: 0x00000D58
		[Obsolete("Use the toDictionary method to get a proper generic Dictionary returned. Hashtables are obsolute.")]
		public static Hashtable toHashtable(this object self)
		{
			Hashtable hashtable = new Hashtable();
			foreach (FieldInfo fieldInfo in self.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				foreach (object obj in fieldInfo.GetCustomAttributes(typeof(P31DeserializeableFieldAttribute), true))
				{
					P31DeserializeableFieldAttribute p31DeserializeableFieldAttribute = obj as P31DeserializeableFieldAttribute;
					if (p31DeserializeableFieldAttribute.isCollection)
					{
						IEnumerable enumerable = fieldInfo.GetValue(self) as IEnumerable;
						ArrayList arrayList = new ArrayList();
						IEnumerator enumerator = enumerable.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								object obj2 = enumerator.Current;
								arrayList.Add(obj2.toHashtable());
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
						hashtable[p31DeserializeableFieldAttribute.key] = arrayList;
					}
					else if (p31DeserializeableFieldAttribute.type != null)
					{
						hashtable[p31DeserializeableFieldAttribute.key] = fieldInfo.GetValue(self).toHashtable();
					}
					else
					{
						hashtable[p31DeserializeableFieldAttribute.key] = fieldInfo.GetValue(self);
					}
				}
			}
			return hashtable;
		}
	}
}
