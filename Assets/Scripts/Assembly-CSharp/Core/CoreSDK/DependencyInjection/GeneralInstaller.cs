using System;
using LEGO.CoreSDK.Legal;
using LEGO.CoreSDK.Network.Caching;
using LEGO.CoreSDK.VPC;

namespace LEGO.CoreSDK.DependencyInjection
{
	// Token: 0x02000063 RID: 99
	internal static class GeneralInstaller
	{
		// Token: 0x06000170 RID: 368 RVA: 0x0000766C File Offset: 0x0000586C
		internal static void RegisterDependencies(IDependencyResolver dependencyResolver)
		{
			dependencyResolver.Register<ILocaleFactory, LocaleFactory>(Lifecycle.Singleton);
			dependencyResolver.Register<IConfigurationCreator<ApplicationConfiguration>, ApplicationConfigurationCreator>(Lifecycle.Transient);
			dependencyResolver.Register<IConfigurationFactory<ApplicationConfiguration>, ApplicationConfigurationFactory>(Lifecycle.Transient);
			dependencyResolver.Register<IParentalGate, ParentalGate>(Lifecycle.Transient);
			dependencyResolver.Register<IPolicies, Policies>(Lifecycle.Transient);
			dependencyResolver.Register<IAuthenticationProvider, AuthenticationProvider>(Lifecycle.Singleton);
			dependencyResolver.Register<IHTTPCache, HTTPCache>(Lifecycle.Singleton);
		}
	}
}
