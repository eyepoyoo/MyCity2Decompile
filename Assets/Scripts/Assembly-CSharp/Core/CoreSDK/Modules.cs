using System;
using LEGO.CoreSDK.DependencyInjection;

namespace LEGO.CoreSDK
{
	// Token: 0x0200007F RID: 127
	public class Modules
	{
		// Token: 0x060001F8 RID: 504 RVA: 0x00008858 File Offset: 0x00006A58
		internal Modules(IDependencyResolver dependencyResolver)
		{
			this.DependencyResolver = dependencyResolver;
		}

		// Token: 0x0400010E RID: 270
		public IDependencyResolver DependencyResolver;
	}
}
