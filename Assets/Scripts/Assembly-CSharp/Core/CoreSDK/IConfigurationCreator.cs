using System;

namespace LEGO.CoreSDK
{
	// Token: 0x02000068 RID: 104
	public interface IConfigurationCreator<TModel>
	{
		// Token: 0x06000181 RID: 385
		TModel Create(string json);
	}
}
