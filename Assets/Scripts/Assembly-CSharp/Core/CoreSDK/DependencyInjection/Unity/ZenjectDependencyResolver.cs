using System;
using Zenject;

namespace LEGO.CoreSDK.DependencyInjection.Unity
{
	// Token: 0x0200005F RID: 95
	public class ZenjectDependencyResolver : IDependencyResolver
	{
		// Token: 0x06000165 RID: 357 RVA: 0x000074DC File Offset: 0x000056DC
		public TService Resolve<TService>() where TService : class
		{
			return this._container.Resolve<TService>();
		}

		// Token: 0x06000166 RID: 358 RVA: 0x000074EC File Offset: 0x000056EC
		public TService Instantiate<TService>(params object[] extraArgs) where TService : class
		{
			return this._container.Instantiate<TService>(extraArgs);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x000074FC File Offset: 0x000056FC
		public void Register<TService, TImplementation>(Lifecycle lifecycle) where TImplementation : TService
		{
			GenericBinder<TService> genericBinder = this._container.Bind<TService>();
			if (lifecycle != Lifecycle.Singleton)
			{
				if (lifecycle == Lifecycle.Transient)
				{
					genericBinder.ToTransient<TImplementation>();
				}
			}
			else
			{
				genericBinder.ToSingle<TImplementation>();
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00007544 File Offset: 0x00005744
		public void RegisterInstance<TService>(TService service)
		{
			this._container.Bind<TService>().ToInstance<TService>(service);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00007558 File Offset: 0x00005758
		public void UpdateRegistration<TService, TImplementation>(Lifecycle lifecycle) where TImplementation : TService
		{
			GenericBinder<TService> genericBinder = this._container.Rebind<TService>();
			if (lifecycle != Lifecycle.Singleton)
			{
				if (lifecycle == Lifecycle.Transient)
				{
					genericBinder.ToTransient<TImplementation>();
				}
			}
			else
			{
				genericBinder.ToSingle<TImplementation>();
			}
		}

		// Token: 0x040000C9 RID: 201
		private DiContainer _container = new DiContainer();
	}
}
