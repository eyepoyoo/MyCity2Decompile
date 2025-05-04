using System;

namespace LEGO.CoreSDK
{
	// Token: 0x0200000B RID: 11
	public static class Asserts
	{
		// Token: 0x06000015 RID: 21 RVA: 0x000025E0 File Offset: 0x000007E0
		public static void ParameterNotNull(object o, string parameterName)
		{
			if (o == null)
			{
				throw new ArgumentNullException("Argument '" + parameterName + "' cannot be null.", new Exception());
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002600 File Offset: 0x00000800
		public static void Precondition(bool condition, string message)
		{
			if (!condition)
			{
				throw new ArgumentException(message);
			}
		}
	}
}
