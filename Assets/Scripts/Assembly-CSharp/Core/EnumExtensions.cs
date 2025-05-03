using System;

// Token: 0x0200005D RID: 93
public class EnumExtensions
{
	// Token: 0x06000162 RID: 354 RVA: 0x00007410 File Offset: 0x00005610
	public static T RandomEnum<T>()
	{
		Type typeFromHandle = typeof(T);
		Array values = Enum.GetValues(typeFromHandle);
		Random rng = EnumExtensions.RNG;
		T t;
		lock (rng)
		{
			object value = values.GetValue(EnumExtensions.RNG.Next(values.Length));
			t = (T)((object)Convert.ChangeType(value, typeFromHandle));
		}
		return t;
	}

	// Token: 0x040000C8 RID: 200
	private static Random RNG = new Random();
}
