using System;
using System.Collections.Generic;
using LEGO.CoreSDK.DependencyInjection;

namespace LEGO.CoreSDK
{
	// Token: 0x02000066 RID: 102
	public class CoreConfiguration
	{
		// Token: 0x06000177 RID: 375 RVA: 0x0000772C File Offset: 0x0000592C
		internal CoreConfiguration(IDependencyResolver dependencyResolver)
		{
			this._dependencyResolver = dependencyResolver;
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000178 RID: 376 RVA: 0x0000773C File Offset: 0x0000593C
		// (set) Token: 0x06000179 RID: 377 RVA: 0x00007744 File Offset: 0x00005944
		public string ApplicationConfigurationURL { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00007750 File Offset: 0x00005950
		// (set) Token: 0x0600017B RID: 379 RVA: 0x00007758 File Offset: 0x00005958
		public IEnumerable<string> SupportedLocales { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00007764 File Offset: 0x00005964
		// (set) Token: 0x0600017D RID: 381 RVA: 0x0000776C File Offset: 0x0000596C
		public string FallbackLocale { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00007778 File Offset: 0x00005978
		public ViewConfiguration CustomViews
		{
			get
			{
				return this._viewConfiguration = this._viewConfiguration ?? new ViewConfiguration(this._dependencyResolver);
			}
		}

		// Token: 0x040000D0 RID: 208
		private IDependencyResolver _dependencyResolver;

		// Token: 0x040000D1 RID: 209
		private ViewConfiguration _viewConfiguration;
	}
}
