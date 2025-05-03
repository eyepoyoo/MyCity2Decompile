using System;

namespace Prime31
{
	// Token: 0x0200001B RID: 27
	public interface IJsonSerializerStrategy
	{
		// Token: 0x060000B3 RID: 179
		bool serializeNonPrimitiveObject(object input, out object output);

		// Token: 0x060000B4 RID: 180
		object deserializeObject(object value, Type type);
	}
}
