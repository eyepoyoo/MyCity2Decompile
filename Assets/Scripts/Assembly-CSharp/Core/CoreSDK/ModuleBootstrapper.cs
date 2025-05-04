using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LEGO.CoreSDK.DependencyInjection;

namespace LEGO.CoreSDK
{
	// Token: 0x02000080 RID: 128
	internal class ModuleBootstrapper
	{
		// Token: 0x060001FA RID: 506 RVA: 0x00008870 File Offset: 0x00006A70
		internal static void BootstrapModules(IDependencyResolver dependencyResolver, ILogger logger)
		{
			IEnumerable<Type> modules = ModuleBootstrapper.GetModules();
			ModuleBootstrapper.CallInitializeOnAllModules(modules, dependencyResolver, logger);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000888C File Offset: 0x00006A8C
		private static IEnumerable<Type> GetModules()
		{
			return AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly x) => from y in x.GetTypes()
				where y.IsClass && y.GetInterfaces().Contains(typeof(Module))
				select y);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x000088C8 File Offset: 0x00006AC8
		private static void CallInitializeOnAllModules(IEnumerable<Type> modules, IDependencyResolver dependencyResolver, ILogger logger)
		{
			foreach (Type type in modules)
			{
				Module module = Activator.CreateInstance(type) as Module;
				if (module == null)
				{
					throw new ArgumentException(string.Concat(new string[]
					{
						"Type '",
						type.ToString(),
						"' does not implement '",
						typeof(Module).ToString(),
						"'."
					}));
				}
				logger.Info("Initializing module: " + type);
				module.InitializeModule(dependencyResolver);
			}
		}
	}
}
