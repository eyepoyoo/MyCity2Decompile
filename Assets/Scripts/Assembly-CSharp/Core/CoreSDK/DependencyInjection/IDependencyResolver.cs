using System;

namespace LEGO.CoreSDK.DependencyInjection
{
	// Token: 0x02000062 RID: 98
	public interface IDependencyResolver
	{
		// Token: 0x0600016B RID: 363
		TService Resolve<TService>() where TService : class;

		// Token: 0x0600016C RID: 364
		TService Instantiate<TService>(params object[] extraArgs) where TService : class;

		// Token: 0x0600016D RID: 365
		void Register<TService, TImplementation>(Lifecycle lifecycle) where TImplementation : TService;

		// Token: 0x0600016E RID: 366
		void RegisterInstance<TService>(TService service);

		// Token: 0x0600016F RID: 367
		void UpdateRegistration<TService, TImplementation>(Lifecycle lifecycle) where TImplementation : TService;
	}
}
