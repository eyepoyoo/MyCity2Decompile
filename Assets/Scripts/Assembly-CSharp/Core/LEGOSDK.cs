using System;
using System.Collections.Generic;
using System.Linq;
using LEGO.CoreSDK;
using LEGO.CoreSDK.DependencyInjection;
using LEGO.CoreSDK.DependencyInjection.Unity;
using LEGO.CoreSDK.Legal;
using LEGO.CoreSDK.Network;
using LEGO.CoreSDK.VPC;

namespace LEGO
{
	// Token: 0x02000082 RID: 130
	public sealed class LEGOSDK
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000203 RID: 515 RVA: 0x000089FC File Offset: 0x00006BFC
		internal static IDependencyResolver DependencyResolver
		{
			get
			{
				if (LEGOSDK._dependencyResolver == null)
				{
					LEGOSDK._dependencyResolver = new ZenjectDependencyResolver();
					LEGOSDK._dependencyResolver.RegisterInstance<IDependencyResolver>(LEGOSDK._dependencyResolver);
					GeneralInstaller.RegisterDependencies(LEGOSDK.DependencyResolver);
					UnityInstaller.RegisterDependencies(LEGOSDK.DependencyResolver);
				}
				return LEGOSDK._dependencyResolver;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000204 RID: 516 RVA: 0x00008A48 File Offset: 0x00006C48
		public static ILogger Log
		{
			get
			{
				return LEGOSDK.DependencyResolver.Resolve<ILogger>();
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000205 RID: 517 RVA: 0x00008A54 File Offset: 0x00006C54
		public static INetwork Network
		{
			get
			{
				if (!LEGOSDK.IsInitialized)
				{
					LEGOSDK.Log.Error("'Network' cannot be accessed before the SDK has been initialized.");
					return null;
				}
				return LEGOSDK.DependencyResolver.Resolve<INetwork>();
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00008A7C File Offset: 0x00006C7C
		public static IPolicies Policies
		{
			get
			{
				if (!LEGOSDK.IsInitialized)
				{
					LEGOSDK.Log.Error("'Policies' cannot be accessed before the SDK has been initialized.");
					return null;
				}
				return LEGOSDK.DependencyResolver.Resolve<IPolicies>();
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00008AA4 File Offset: 0x00006CA4
		public static IParentalGate ParentalGate
		{
			get
			{
				if (!LEGOSDK.IsInitialized)
				{
					LEGOSDK.Log.Error("'ParentalGate' cannot be accessed before the SDK has been initialized.");
					return null;
				}
				return LEGOSDK.DependencyResolver.Resolve<IParentalGate>();
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000208 RID: 520 RVA: 0x00008ACC File Offset: 0x00006CCC
		public static IApplicationConfigurationService ApplicationConfigurationService
		{
			get
			{
				if (!LEGOSDK.IsInitialized)
				{
					LEGOSDK.Log.Error("'ApplicationConfigurationService' cannot be accessed before the SDK has been initialized.");
					return null;
				}
				return LEGOSDK.DependencyResolver.Resolve<IApplicationConfigurationService>();
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00008AF4 File Offset: 0x00006CF4
		public static Modules Modules
		{
			get
			{
				Modules modules;
				if ((modules = LEGOSDK._modules) == null)
				{
					modules = (LEGOSDK._modules = new Modules(LEGOSDK.DependencyResolver));
				}
				return modules;
			}
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00008B14 File Offset: 0x00006D14
		public static void Initialize(Action<CoreConfiguration> configurationAction)
		{
			CoreConfiguration coreConfiguration = new CoreConfiguration(LEGOSDK.DependencyResolver);
			if (configurationAction != null)
			{
				configurationAction(coreConfiguration);
			}
			LEGOSDK.InitializeSDK(coreConfiguration.ApplicationConfigurationURL, coreConfiguration.SupportedLocales, coreConfiguration.FallbackLocale ?? coreConfiguration.SupportedLocales.First<string>(), false);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00008B64 File Offset: 0x00006D64
		public static void InitializeOffline(Action<OfflineConfiguration> configurationAction = null)
		{
			OfflineConfiguration offlineConfiguration = new OfflineConfiguration(LEGOSDK.DependencyResolver);
			if (configurationAction != null)
			{
				configurationAction(offlineConfiguration);
			}
			LEGOSDK.InitializeSDK(null, null, null, true);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00008B94 File Offset: 0x00006D94
		private static void InitializeSDK(string applicationConfigurationURL, IEnumerable<string> supportedLocales, string fallbackLocale, bool offlineMode)
		{
			if (LEGOSDK.IsInitialized)
			{
				throw new Exception("The LEGO SDK can only be initialized once.");
			}
			LEGOSDK.IsInitialized = true;
			if (offlineMode)
			{
				LEGOSDK.DependencyResolver.UpdateRegistration<INetwork, OfflineNetwork>(Lifecycle.Transient);
			}
			LEGOSDK.RegisterMarketService(supportedLocales, fallbackLocale);
			LEGOSDK.RegisterApplicationConfiguration(applicationConfigurationURL);
			LEGOSDK.DependencyResolver.Resolve<IAuthenticationProvider>().Authentication = new NoAuthentication();
			ModuleBootstrapper.BootstrapModules(LEGOSDK.DependencyResolver, LEGOSDK.Log);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00008C00 File Offset: 0x00006E00
		private static void RegisterMarketService(IEnumerable<string> supportedLocales, string fallbackLocale)
		{
			MarketService.Factory factory = LEGOSDK.DependencyResolver.Instantiate<MarketService.Factory>(new object[0]);
			LEGOSDK.DependencyResolver.RegisterInstance<IMarketService>(factory.Create(supportedLocales, fallbackLocale));
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00008C30 File Offset: 0x00006E30
		private static void RegisterApplicationConfiguration(string applicationConfigurationURL)
		{
			ApplicationConfigurationService applicationConfigurationService = LEGOSDK.DependencyResolver.Instantiate<ApplicationConfigurationService>(new object[0]);
			LEGOSDK.DependencyResolver.RegisterInstance<IApplicationConfigurationService>(applicationConfigurationService);
			applicationConfigurationService.DownloadApplicationConfiguration(applicationConfigurationURL);
			LEGOSDK.DependencyResolver.Instantiate<ApplicationConfigurationGuard>(new object[0]);
		}

		// Token: 0x04000111 RID: 273
		private static bool IsInitialized;

		// Token: 0x04000112 RID: 274
		private static IDependencyResolver _dependencyResolver;

		// Token: 0x04000113 RID: 275
		private static Modules _modules;
	}
}
