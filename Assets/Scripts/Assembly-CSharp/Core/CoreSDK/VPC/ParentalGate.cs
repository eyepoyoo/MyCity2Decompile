using System;
using LEGO.CoreSDK.DependencyInjection;

namespace LEGO.CoreSDK.VPC
{
	// Token: 0x02000007 RID: 7
	public class ParentalGate : IParentalGate
	{
		// Token: 0x0600000F RID: 15 RVA: 0x00002598 File Offset: 0x00000798
		public ParentalGate(IDependencyResolver dependencyResolver)
		{
			this._dependencyResolver = dependencyResolver;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000025A8 File Offset: 0x000007A8
		public void Guard(Action<Result> completionHandler)
		{
			ParentalGateModel parentalGateModel = new ParentalGateModel(completionHandler, null);
			this._dependencyResolver.Resolve<IParentalGateView>().Show(parentalGateModel);
		}

		// Token: 0x04000018 RID: 24
		private IDependencyResolver _dependencyResolver;
	}
}
