using System;
using LEGO.CoreSDK.DependencyInjection;

namespace LEGO.CoreSDK
{
	// Token: 0x02000065 RID: 101
	public class OfflineConfiguration
	{
		// Token: 0x06000175 RID: 373 RVA: 0x000076EC File Offset: 0x000058EC
		internal OfflineConfiguration(IDependencyResolver dependencyResolver)
		{
			this._dependencyResolver = dependencyResolver;
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000176 RID: 374 RVA: 0x000076FC File Offset: 0x000058FC
		public ViewConfiguration CustomViews
		{
			get
			{
				return this._viewConfiguration = this._viewConfiguration ?? new ViewConfiguration(this._dependencyResolver);
			}
		}

		// Token: 0x040000CE RID: 206
		private IDependencyResolver _dependencyResolver;

		// Token: 0x040000CF RID: 207
		private ViewConfiguration _viewConfiguration;
	}
}
