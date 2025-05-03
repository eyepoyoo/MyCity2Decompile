using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Prime31.Reflection
{
	// Token: 0x0200001D RID: 29
	public class ReflectionUtils
	{
		// Token: 0x060000BE RID: 190 RVA: 0x000076DD File Offset: 0x000058DD
		public static Attribute getAttribute(MemberInfo info, Type type)
		{
			if (info == null || type == null || !Attribute.IsDefined(info, type))
			{
				return null;
			}
			return Attribute.GetCustomAttribute(info, type);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00007700 File Offset: 0x00005900
		public static Attribute getAttribute(Type objectType, Type attributeType)
		{
			if (objectType == null || attributeType == null || !Attribute.IsDefined(objectType, attributeType))
			{
				return null;
			}
			return Attribute.GetCustomAttribute(objectType, attributeType);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00007724 File Offset: 0x00005924
		public static bool isTypeGenericeCollectionInterface(Type type)
		{
			if (!type.IsGenericType)
			{
				return false;
			}
			Type genericTypeDefinition = type.GetGenericTypeDefinition();
			return genericTypeDefinition == typeof(IList<>) || genericTypeDefinition == typeof(ICollection<>) || genericTypeDefinition == typeof(IEnumerable<>);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00007778 File Offset: 0x00005978
		public static bool isTypeDictionary(Type type)
		{
			if (typeof(IDictionary).IsAssignableFrom(type))
			{
				return true;
			}
			if (!type.IsGenericType)
			{
				return false;
			}
			Type genericTypeDefinition = type.GetGenericTypeDefinition();
			return genericTypeDefinition == typeof(IDictionary<, >);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000077BD File Offset: 0x000059BD
		public static bool isNullableType(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000077DF File Offset: 0x000059DF
		public static object toNullableType(object obj, Type nullableType)
		{
			return (obj != null) ? Convert.ChangeType(obj, Nullable.GetUnderlyingType(nullableType), CultureInfo.InvariantCulture) : null;
		}
	}
}
