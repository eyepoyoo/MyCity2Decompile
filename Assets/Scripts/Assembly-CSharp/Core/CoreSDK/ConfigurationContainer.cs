using System;

namespace LEGO.CoreSDK
{
	// Token: 0x0200006E RID: 110
	public struct ConfigurationContainer<T>
	{
		// Token: 0x06000193 RID: 403 RVA: 0x00007BB8 File Offset: 0x00005DB8
		internal ConfigurationContainer(T model, ConfigurationSource source)
		{
			this.Model = model;
			this.Source = source;
		}

		// Token: 0x040000E2 RID: 226
		public T Model;

		// Token: 0x040000E3 RID: 227
		public ConfigurationSource Source;
	}
}
