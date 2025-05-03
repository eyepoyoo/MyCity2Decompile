using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Prime31
{
	// Token: 0x0200000F RID: 15
	public class DTOBase
	{
		// Token: 0x06000062 RID: 98 RVA: 0x00004790 File Offset: 0x00002990
		public static List<T> listFromJson<T>(string json) where T : DTOBase
		{
			List<object> list = json.listFromJson();
			List<T> list2 = new List<T>();
			foreach (object obj in list)
			{
				T t = Activator.CreateInstance<T>();
				t.setDataFromDictionary(obj as Dictionary<string, object>);
				list2.Add(t);
			}
			return list2;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00004810 File Offset: 0x00002A10
		public void setDataFromJson(string json)
		{
			this.setDataFromDictionary(json.dictionaryFromJson());
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004820 File Offset: 0x00002A20
		public void setDataFromDictionary(Dictionary<string, object> dict)
		{
			Dictionary<string, Action<object>> membersWithSetters = this.getMembersWithSetters();
			foreach (KeyValuePair<string, object> keyValuePair in dict)
			{
				if (membersWithSetters.ContainsKey(keyValuePair.Key))
				{
					try
					{
						membersWithSetters[keyValuePair.Key](keyValuePair.Value);
					}
					catch (Exception ex)
					{
						Utils.logObject(ex);
					}
				}
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000048C0 File Offset: 0x00002AC0
		private bool shouldIncludeTypeWithSetters(Type type)
		{
			return !type.IsGenericType && type.Namespace.StartsWith("System");
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000048E8 File Offset: 0x00002AE8
		protected Dictionary<string, Action<object>> getMembersWithSetters()
		{
			Dictionary<string, Action<object>> dictionary = new Dictionary<string, Action<object>>();
			FieldInfo[] fields = base.GetType().GetFields();
			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo fieldInfo = fields[i];
				if (this.shouldIncludeTypeWithSetters(fieldInfo.FieldType))
				{
					FieldInfo theInfo2 = fieldInfo;
					dictionary[fieldInfo.Name] = delegate(object val)
					{
						theInfo2.SetValue(this, val);
					};
				}
			}
			foreach (PropertyInfo propertyInfo in base.GetType().GetProperties())
			{
				if (this.shouldIncludeTypeWithSetters(propertyInfo.PropertyType))
				{
					if (propertyInfo.CanWrite && propertyInfo.GetSetMethod() != null)
					{
						PropertyInfo theInfo = propertyInfo;
						dictionary[propertyInfo.Name] = delegate(object val)
						{
							theInfo.SetValue(this, val, null);
						};
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000049F4 File Offset: 0x00002BF4
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("[{0}]:", base.GetType());
			foreach (FieldInfo fieldInfo in base.GetType().GetFields())
			{
				stringBuilder.AppendFormat(", {0}: {1}", fieldInfo.Name, fieldInfo.GetValue(this));
			}
			foreach (PropertyInfo propertyInfo in base.GetType().GetProperties())
			{
				stringBuilder.AppendFormat(", {0}: {1}", propertyInfo.Name, propertyInfo.GetValue(this, null));
			}
			return stringBuilder.ToString();
		}
	}
}
