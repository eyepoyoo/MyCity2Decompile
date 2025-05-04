using System;
using System.IO;
using System.Reflection;

namespace LEGO.CoreSDK.Native
{
	// Token: 0x02000017 RID: 23
	internal static class NativeUtils
	{
		// Token: 0x0600003A RID: 58 RVA: 0x00002DDC File Offset: 0x00000FDC
		internal static object CreateType(string typeName, string optionalAssembly)
		{
			Type type = null;
			try
			{
				type = Assembly.Load(optionalAssembly).GetType(typeName);
			}
			catch (FileNotFoundException)
			{
				type = Assembly.Load(NativeUtils.DefaultUnityAssemblyName).GetType(typeName);
			}
			if (type == null)
			{
				throw new TypeLoadException(string.Concat(new string[]
				{
					"Failed to find '",
					typeName,
					"' in either '",
					optionalAssembly,
					"' and '",
					NativeUtils.DefaultUnityAssemblyName,
					"'."
				}));
			}
			return Activator.CreateInstance(type);
		}

		// Token: 0x04000036 RID: 54
		internal static readonly string DefaultUnityAssemblyName = "Assembly-CSharp";
	}
}
