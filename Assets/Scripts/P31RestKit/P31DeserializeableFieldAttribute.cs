using System;

namespace Prime31
{
	// Token: 0x02000006 RID: 6
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class P31DeserializeableFieldAttribute : Attribute
	{
		// Token: 0x06000019 RID: 25 RVA: 0x00002CA0 File Offset: 0x00000EA0
		public P31DeserializeableFieldAttribute(string key)
		{
			this.key = key;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002CAF File Offset: 0x00000EAF
		public P31DeserializeableFieldAttribute(string key, Type type)
			: this(key)
		{
			this.type = type;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002CBF File Offset: 0x00000EBF
		public P31DeserializeableFieldAttribute(string key, Type type, bool isCollection)
			: this(key, type)
		{
			this.isCollection = isCollection;
		}

		// Token: 0x0400000C RID: 12
		public readonly string key;

		// Token: 0x0400000D RID: 13
		public readonly bool isCollection;

		// Token: 0x0400000E RID: 14
		public Type type;
	}
}
