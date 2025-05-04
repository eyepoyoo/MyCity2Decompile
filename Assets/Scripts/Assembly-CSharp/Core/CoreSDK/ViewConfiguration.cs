using System;
using LEGO.CoreSDK.DependencyInjection;
using LEGO.CoreSDK.Legal;
using LEGO.CoreSDK.VPC;

namespace LEGO.CoreSDK
{
	// Token: 0x02000064 RID: 100
	public class ViewConfiguration
	{
		// Token: 0x06000171 RID: 369 RVA: 0x000076AC File Offset: 0x000058AC
		internal ViewConfiguration(IDependencyResolver dependencyResolver)
		{
			this._dependencyResolver = dependencyResolver;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x000076BC File Offset: 0x000058BC
		public void SetAlertView<TAlertView>() where TAlertView : IAlert, new()
		{
			this._dependencyResolver.UpdateRegistration<IAlert, TAlertView>(Lifecycle.Transient);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000076CC File Offset: 0x000058CC
		public void SetPolicyView<TPolicyView>() where TPolicyView : IPolicyView, new()
		{
			this._dependencyResolver.UpdateRegistration<IPolicyView, TPolicyView>(Lifecycle.Transient);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000076DC File Offset: 0x000058DC
		public void SetParentalGateView<TParentalGateView>() where TParentalGateView : IParentalGateView, new()
		{
			this._dependencyResolver.UpdateRegistration<IParentalGateView, TParentalGateView>(Lifecycle.Transient);
		}

		// Token: 0x040000CD RID: 205
		private IDependencyResolver _dependencyResolver;
	}
}
