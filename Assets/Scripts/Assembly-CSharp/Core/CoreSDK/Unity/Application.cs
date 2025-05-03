using System;
using System.Reflection;
using LEGO.CoreSDK.Native;
using UnityEngine;

namespace LEGO.CoreSDK.Unity
{
	// Token: 0x02000016 RID: 22
	internal class Application : IApplication
	{
		// Token: 0x06000035 RID: 53 RVA: 0x00002D08 File Offset: 0x00000F08
		public Version Version()
		{
			Type applicationVersionType = Application.GetApplicationVersionType();
			if (applicationVersionType != null)
			{
				MethodInfo method = applicationVersionType.GetMethod("Version");
				if (method != null)
				{
					Version version = method.Invoke(null, null) as Version;
					if (version != null)
					{
						return version;
					}
				}
			}
			throw new Exception("Failed to find ApplicationVersion class in project. So no application version could be retrieved.");
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002D5C File Offset: 0x00000F5C
		private static Type GetApplicationVersionType()
		{
			Assembly assembly = Assembly.Load(NativeUtils.DefaultUnityAssemblyName);
			return Application.GetApplicationVersionTypeInAssembly(assembly);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002D7C File Offset: 0x00000F7C
		private static Type GetApplicationVersionTypeInAssembly(Assembly assembly)
		{
			if (assembly == null)
			{
				return null;
			}
			foreach (Type type in assembly.GetTypes())
			{
				if (type.FullName == "LEGO.CoreSDK.ApplicationVersion")
				{
					return type;
				}
			}
			return null;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002DC8 File Offset: 0x00000FC8
		public void Quit()
		{
			UnityEngine.Application.Quit();
		}
	}
}
