using System;
using LEGO.CoreSDK.DependencyInjection;

namespace LEGO.CoreSDK
{
	// Token: 0x02000081 RID: 129
	public interface Module
	{
		// Token: 0x060001FF RID: 511
		void OverrideCoreRegistrations(IDependencyResolver dependencyResolver);

		// Token: 0x06000200 RID: 512
		void InitializeModule(IDependencyResolver dependencyResolver);
	}
}
