using System;
using System.Reflection;

namespace Prime31.Reflection
{
	// Token: 0x02000021 RID: 33
	public class CacheResolver
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x000077FE File Offset: 0x000059FE
		public CacheResolver(MemberMapLoader memberMapLoader)
		{
			this._memberMapLoader = memberMapLoader;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00007818 File Offset: 0x00005A18
		public static object getNewInstance(Type type)
		{
			CacheResolver.CtorDelegate ctorDelegate;
			if (CacheResolver.constructorCache.tryGetValue(type, out ctorDelegate))
			{
				return ctorDelegate();
			}
			ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
			ctorDelegate = () => constructorInfo.Invoke(null);
			CacheResolver.constructorCache.add(type, ctorDelegate);
			return ctorDelegate();
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00007878 File Offset: 0x00005A78
		public SafeDictionary<string, CacheResolver.MemberMap> loadMaps(Type type)
		{
			if (type == null || type == typeof(object))
			{
				return null;
			}
			SafeDictionary<string, CacheResolver.MemberMap> safeDictionary;
			if (this._memberMapsCache.tryGetValue(type, out safeDictionary))
			{
				return safeDictionary;
			}
			safeDictionary = new SafeDictionary<string, CacheResolver.MemberMap>();
			this._memberMapLoader(type, safeDictionary);
			this._memberMapsCache.add(type, safeDictionary);
			return safeDictionary;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000078D4 File Offset: 0x00005AD4
		private static GetHandler createGetHandler(FieldInfo fieldInfo)
		{
			return (object instance) => fieldInfo.GetValue(instance);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000078FC File Offset: 0x00005AFC
		private static SetHandler createSetHandler(FieldInfo fieldInfo)
		{
			if (fieldInfo.IsInitOnly || fieldInfo.IsLiteral)
			{
				return null;
			}
			return delegate(object instance, object value)
			{
				fieldInfo.SetValue(instance, value);
			};
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00007944 File Offset: 0x00005B44
		private static GetHandler createGetHandler(PropertyInfo propertyInfo)
		{
			MethodInfo getMethodInfo = propertyInfo.GetGetMethod(true);
			if (getMethodInfo == null)
			{
				return null;
			}
			return (object instance) => getMethodInfo.Invoke(instance, Type.EmptyTypes);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00007980 File Offset: 0x00005B80
		private static SetHandler createSetHandler(PropertyInfo propertyInfo)
		{
			MethodInfo setMethodInfo = propertyInfo.GetSetMethod(true);
			if (setMethodInfo == null)
			{
				return null;
			}
			return delegate(object instance, object value)
			{
				setMethodInfo.Invoke(instance, new object[] { value });
			};
		}

		// Token: 0x04000058 RID: 88
		private readonly MemberMapLoader _memberMapLoader;

		// Token: 0x04000059 RID: 89
		private readonly SafeDictionary<Type, SafeDictionary<string, CacheResolver.MemberMap>> _memberMapsCache = new SafeDictionary<Type, SafeDictionary<string, CacheResolver.MemberMap>>();

		// Token: 0x0400005A RID: 90
		private static readonly SafeDictionary<Type, CacheResolver.CtorDelegate> constructorCache = new SafeDictionary<Type, CacheResolver.CtorDelegate>();

		// Token: 0x02000022 RID: 34
		// (Invoke) Token: 0x060000D9 RID: 217
		private delegate object CtorDelegate();

		// Token: 0x02000023 RID: 35
		public sealed class MemberMap
		{
			// Token: 0x060000DC RID: 220 RVA: 0x000079C5 File Offset: 0x00005BC5
			public MemberMap(PropertyInfo propertyInfo)
			{
				this.MemberInfo = propertyInfo;
				this.Type = propertyInfo.PropertyType;
				this.Getter = CacheResolver.createGetHandler(propertyInfo);
				this.Setter = CacheResolver.createSetHandler(propertyInfo);
			}

			// Token: 0x060000DD RID: 221 RVA: 0x000079F8 File Offset: 0x00005BF8
			public MemberMap(FieldInfo fieldInfo)
			{
				this.MemberInfo = fieldInfo;
				this.Type = fieldInfo.FieldType;
				this.Getter = CacheResolver.createGetHandler(fieldInfo);
				this.Setter = CacheResolver.createSetHandler(fieldInfo);
			}

			// Token: 0x0400005B RID: 91
			public readonly MemberInfo MemberInfo;

			// Token: 0x0400005C RID: 92
			public readonly Type Type;

			// Token: 0x0400005D RID: 93
			public readonly GetHandler Getter;

			// Token: 0x0400005E RID: 94
			public readonly SetHandler Setter;
		}
	}
}
